using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace DesktopDNS.Configure;
[YamlSerializable]
public class DnsGroup
{
    public string? Name { get; set; }
    public bool Enable { get; set; }
    /**
        该分组中使用的上级Dns服务器，可以设置
    **/
    public string? Server{get;set;}
    public List<Domain>? Domains{get;set;}

}