using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace DesktopDNS.Configure;
[YamlSerializable]
public class Domain{
    public string? Hostname{get;set;}
    /**
        可以是IP，也可能是CNAME等记录
    **/
    public string? Value{get;set;}
    public bool Enable{get;set;}

    /**
        匹配模式：FULL/REGEX/WILDCARD
    **/
    public string? Mode{get;set;}
    /**
        资源类型：A或者AAAA
    **/
    public string? RecordType{get;set;}
    /**
        该分组中使用的上级Dns服务器，IP与Server必须设置一项
    **/
    public string? Server{get;set;}

    //implement both "*" and "?"
    private static string WildCardToRegular(string value)
    {
        return "^" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + "$";
    }
    public bool CanMatched(string hostname){
        if(string.IsNullOrWhiteSpace(this.Hostname)) return false;
        if (this.Mode == "FULL") return this.Hostname.ToLower() == hostname.ToLower();
        if (this.Mode == "REGEX")
        {
            Regex regex = new Regex(this.Hostname);
            return regex.IsMatch(hostname);
        }
        if(this.Mode== "WILDCARD")
        {
            Regex regex = new Regex(WildCardToRegular(this.Hostname));
            return regex.IsMatch(hostname);
        }
        return false;
    }
}