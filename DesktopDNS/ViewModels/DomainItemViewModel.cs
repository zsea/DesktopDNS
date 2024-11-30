using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDNS.ViewModels
{
    internal class DomainItemViewModel : ViewModelBase
    {
        public static int GetRecordTypeIndex(string? recordType)
        {
            switch (recordType)
            {
                case "A": return 0;
                case "AAAA": return 1;
                case "CNAME": return 2;
            }
            return 0;
        }
        public static string GetRecordType(int index)
        {

            switch (index)
            {
                case 0: return "A";
                case 1: return "AAAA";
                case 2: return "CNAME";
            }
            return "A";
        }
        public static int GetModeIndex(string? mode)
        {
            switch (mode)
            {
                case "FULL":return 0;
                case "REGEX":return 1;
                case "WILDCARD":return 2;
            }
            return 0;
        }
        public static string GetMode(int index)
        {
            switch (index)
            {
                case 0: return "FULL";
                case 1: return "REGEX";
                case 2: return "WILDCARD";
            }
            return "FULL";
        }
        public string Hostname { get; set; } = "";
        /**
            可以是IP，也可能是CNAME等记录
        **/
        public string Value { get; set; } = "";
        public bool Enable { get; set; } = true;

        /**
            匹配模式：FULL/REGEX/WILDCARD
        **/
        //public string Mode { get; set; } = "FULL";
        public int ModeIndex { get; set; } = 0;
        /**
            资源类型：A或者AAAA
        **/
        //public string RecordType { get; set; } = "";
        public int RecordTypeIndex { get; set; } = 0;

        /**
            该分组中使用的上级Dns服务器，IP与Server必须设置一项
        **/
        public string Server { get; set; } = "";

        public bool IsOk { get; set; } = false;
        public string Title
        {
            get
            {
                return IsNew ? "添加解析" : "编辑解析";
            }
        }
        public bool IsNew { get; private set; } = true;

        public Configure.DnsGroup Group {get;private set;}
        public DomainItemViewModel(Configure.DnsGroup group, Configure.Domain? domain=null) {
            this.Group = group;
            if (domain == null) return;
            this.Hostname = domain.Hostname??"";
            this.Value = domain.Value ?? "";
            this.Enable = domain.Enable;
            //this.Mode = domain.Mode ?? "FULL";
            this.ModeIndex = GetModeIndex(domain.Mode);
            this.RecordTypeIndex = GetRecordTypeIndex(domain.RecordType);
            this.Server = domain.Server ?? "";
            this.IsNew = false;
        }
    }
}
