using YamlDotNet.Serialization;

namespace DesktopDNS.Configure;
[YamlSerializable]
public class RemoteRule
{
    public string? Name { get; set; }
    public string? Url { get; set; }
    /**
    更新间隔，单位：分钟
    **/
    public int Interval{get;set;}
    public bool Enable{get;set;}
}