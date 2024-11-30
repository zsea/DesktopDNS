using DNS.Client.RequestResolver;
using DNS.Client;
using DNS.Protocol.ResourceRecords;
using DNS.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DNS.Protocol.Utils;

namespace DesktopDNS
{
    class RecordTypeName
    {
        public static string GetName(RecordType type)
        {
            switch (type)
            {
                case RecordType.A: return "A";
                case RecordType.AAAA: return "AAAA";
                case RecordType.CNAME: return "CNAME";
                case RecordType.NS: return "NS";
                case RecordType.MX: return "MX";
                case RecordType.OPT: return "OPT";
                case RecordType.PTR: return "PTR";
                case RecordType.SOA: return "SOA";
                case RecordType.SRV: return "SRV";
                case RecordType.TXT: return "TXT";
                case RecordType.WKS: return "WKS";
                case RecordType.ANY: return "ANY";
                default: return "UNKNOW";
            }
        }
    }
    public class LocalRequestResolver : IRequestResolver
    {
        private Dictionary<string, IList<Configure.Domain>> remotes = new Dictionary<string, IList<Configure.Domain>>();
        private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();
        private void UpdateConfigure(Configure.Server configure)
        {
            this.Configure = configure;
            this.ResetTimer();
        }
        private void ResetTimer()
        {
            // 移除所有
            string[] allKeys = timers.Keys.ToArray();
            foreach (string key in allKeys)
            {
                Timer? t = null;
                if (!timers.TryGetValue(key, out t)) continue;
                timers.Remove(key);
                if (t != null)
                {
                    t.Dispose();
                }
            }
            if (this.Configure == null || this.Configure.Remotes == null)
            {

                return;
            }



            // 添加新的Timer
            foreach (Configure.RemoteRule rule in this.Configure.Remotes)
            {
                if (!rule.Enable || string.IsNullOrWhiteSpace(rule.Name)) continue;
                int interval = rule.Interval;
                if (interval == 0) interval = 10;
                Timer t = new Timer((state) =>
                {
                    if (state == null) return;
                    _ = this.LoadRemoteDomains(state as Configure.RemoteRule);

                }, rule, 0, interval * 60 * 1000);
                timers[rule.Name] = t;
            }
        }
        public LocalRequestResolver(Configure.Server configure)
        {
            UpdateConfigure(configure);
        }
        public void SetConfigure(Configure.Server configure)
        {
            UpdateConfigure(configure);
        }
        public Configure.Server? Configure { get; private set; }
        public async Task<IResponse> Resolve(IRequest request, CancellationToken cancellationToken = default)
        {
            /**
                先从本地查询对应的记录，比如AAAA,A
                    查询到：
                        直接返回
                    没查询到：
                        再查询CNAME记录
                            查询到CNAME记录
                                ：使用默认服务器再查进行CNAME解析
                            没查询到CNAME：
                                : 反回空结果

            **/
            
            IResponse response = Response.FromRequest(request);
            if (this.Configure == null) return response;
            foreach (Question question in response.Questions)
            {
                
                bool query_result = await this.Query(question.Name, question.Type, response, cancellationToken);
                
                if (!query_result && question.Type != RecordType.CNAME)
                {
                    // 查询CNAME
                    query_result = await this.Query(question.Name, RecordType.CNAME, response, cancellationToken);
                    if (query_result)
                    {
                        CanonicalNameResourceRecord? cname = response.AnswerRecords[response.AnswerRecords.Count - 1] as CanonicalNameResourceRecord;
                        if (cname != null)
                        {
                            query_result = await this.Query(cname.CanonicalDomainName, question.Type, response, cancellationToken);
                            if (!query_result && !string.IsNullOrWhiteSpace(this.Configure.DefaultServer))
                            {
                                await Forward(cname.CanonicalDomainName.ToString(), question.Type, this.Configure.DefaultServer, response, cancellationToken);
                            }
                        }
                    }
                }
                //Logger.Debug($"{RecordTypeName.GetName(question.Type)}\t{question.Name}");
                if (query_result)
                {
                    Server.LocalResolveTimes++;
                }

            }

            return response;
        }


        private static async Task Forward(string domainName, RecordType type, string server, IResponse response, CancellationToken cancellationToken = default)
        {
            ClientRequest proxy = new ClientRequest(server);

            // 代理请求
            proxy.Questions.Add(new Question(new Domain(domainName), type));
            proxy.RecursionDesired = true;

            IResponse res = await proxy.Resolve(cancellationToken);
            foreach (IResourceRecord item in res.AnswerRecords)
            {
                if (item.Type == type)
                {
                    response.AnswerRecords.Add(item);
                }

            }
        }
        private async Task<bool> Query(Domain name, RecordType type, IResponse response, CancellationToken cancellationToken = default)
        {
            if (this.Configure == null) return false;
            string domainName = name.ToString(), recordType = RecordTypeName.GetName(type);
            if (this.Configure.Groups != null)
            {
                foreach (Configure.DnsGroup group in Configure.Groups)
                {
                    if (group == null || !group.Enable || group.Domains == null) continue;

                    Configure.Domain? domain = group.Domains.Where(domain => domain.Enable && domain.RecordType == recordType && domain.CanMatched(domainName)).FirstOrDefault();
                    if (domain != null)
                    {

                        if (!string.IsNullOrEmpty(domain.Value))
                        {
                            AppendRecoredToResponse(name, domain, response);
                        }
                        else if (!String.IsNullOrWhiteSpace(domain.Server) || !String.IsNullOrWhiteSpace(group.Server))
                        {
                            string? server = String.IsNullOrWhiteSpace(domain.Server) ? group.Server : domain.Server;
                            if (!string.IsNullOrWhiteSpace(server))
                            {
                                await Forward(domainName, type, server, response);
                            }


                        }
                        return true;
                    }
                }
            }
            // TODO: 从在线规则中查找
            if (this.Configure.Remotes != null)
            {
                foreach (Configure.RemoteRule rule in this.Configure.Remotes)
                {
                    if (!rule.Enable || string.IsNullOrWhiteSpace(rule.Name)) continue;
                    IList<Configure.Domain>? domains = null;
                    if (!this.remotes.TryGetValue(rule.Name, out domains)) continue;
                    //IList<Configure.Domain>? domains = this.remotes[rule.Name];
                    if (domains == null) continue;
                    Configure.Domain? domain = domains.Where(domain => domain.Enable && domain.RecordType == recordType && domain.CanMatched(domainName)).FirstOrDefault();
                    if (domain != null)
                    {
                        if (!string.IsNullOrEmpty(domain.Value))
                        {
                            AppendRecoredToResponse(name, domain, response);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private void AppendRecoredToResponse(Domain name, Configure.Domain domain, IResponse response)
        {
            IResourceRecord? record = null;
            if (domain.RecordType == "CNAME")
            {
                record = new CanonicalNameResourceRecord(name, new Domain(domain.Value));
            }
            else if (domain.RecordType == "A" || domain.RecordType == "AAAA")
            {
                if (!string.IsNullOrWhiteSpace(domain.Value))
                {
                    IPAddress? ip = null;
                    if (IPAddress.TryParse(domain.Value, out ip))
                    {
                        record = new IPAddressResourceRecord(name, ip);
                    }

                }
            }
            else if (domain.RecordType == "MX")
            {
                record = new MailExchangeResourceRecord(name, 0, new Domain(domain.Value));
            }
            else if (domain.RecordType == "TXT")
            {
                record = new TextResourceRecord(name, new CharacterString[] { new CharacterString(domain.Value) });
            }
            else if (domain.RecordType == "NS")
            {
                record = new NameServerResourceRecord(name, new Domain(domain.Value));
            }
            else if (domain.RecordType == "PTR" || domain.RecordType == "SOA" || domain.RecordType == "SRV")
            {
                //PTR 反向解析
                // 不实现
            }
            if (record != null)
            {
                response.AnswerRecords.Add(record);

            }
        }
        private async Task LoadRemoteDomains(Configure.RemoteRule? rule)
        {
            
            if (rule == null || !rule.Enable || string.IsNullOrWhiteSpace(rule.Url) || string.IsNullOrWhiteSpace(rule.Name))
            {
                return;
            }
            string text = "";
            using (HttpClient client = new HttpClient())
            {
                // 指定 URL
                string url = rule.Url;

                try
                {
                    // 发送 GET 请求
                    HttpResponseMessage response = await client.GetAsync(url);

                    // 确保响应状态码为成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容
                    text = await response.Content.ReadAsStringAsync();

                }
                catch (HttpRequestException e)
                {
                    Logger.Error($"Load remote Error:[{rule.Name}] {rule.Url} \r\n{e.StackTrace}");
                }
            }
            StringReader reader = new StringReader(text);
            List<Configure.Domain> domains = new List<Configure.Domain>();
            string? line = null;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrWhiteSpace(line) || line[0] == '#') continue;
                string[] items = Regex.Split(line, @"\s+");
                if (items.Length < 2) continue;
                Configure.Domain domain = new Configure.Domain() { Enable = true };
                domain.Value = items[0];
                domain.Mode = "FULL";
                domain.Hostname = items[1];
                string type = items.Length >= 3 ? items[2] : "";
                if (string.IsNullOrWhiteSpace(type))
                {
                    // 判断是A还是AAAA
                    IPAddress? ip;
                    if (!IPAddress.TryParse(domain.Value, out ip)) continue;
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        type = "A";
                    }
                    else if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        type = "AAAA";
                    }
                }
                if (string.IsNullOrWhiteSpace(type)) continue;
                domain.RecordType = type;
                domains.Add(domain);
            }
            this.remotes[rule.Name] = domains;
            Logger.Info($"Load remote success:[{rule.Name}][{domains.Count}] {rule.Url} ");
        }
    }
}
