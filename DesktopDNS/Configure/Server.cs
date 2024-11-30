using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DesktopDNS.Configure;

[YamlSerializable]
public class Server
{
    
    public int Port { get; set; }
    /**
        默认DNS服务器
    **/
    public string DefaultServer { get; set; } = "8.8.8.8";
    public bool UseUDP { get; set; }
    public bool UseTCP { get; set; }

    public bool UseHttp { get; set; }
    public string LogLevel { get; set; } = "Trace";
    /// <summary>
    /// 是否开机自动启动
    /// </summary>
    public bool AutoRun { get; set; }

    public List<DnsGroup>? Groups { get; set; }
    public List<RemoteRule>? Remotes { get; set; }

    private static string CONFIGURE_FILE { get{
            string? homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                       Environment.OSVersion.Platform == PlatformID.MacOSX)
        ? Environment.GetEnvironmentVariable("HOME")
        : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            if (string.IsNullOrWhiteSpace(homePath)) homePath = "/";
            string CONFIGURE_FILE = System.IO.Path.Combine(homePath, ".DesktopDNS.yaml");
            return CONFIGURE_FILE;
        } }
    public static Server Load()
    {
        
        string? configureText = null;
        try
        {
            configureText = System.IO.File.ReadAllText(CONFIGURE_FILE);
            Logger.Info("Configure file load success.");
        }
        catch
        {
            Logger.Error("Configure file not found.");
        }
        
        var deserializer = new StaticDeserializerBuilder(new StaticContext())
              .WithNamingConvention(PascalCaseNamingConvention.Instance)
              .Build();

        Configure.Server? configure = deserializer.Deserialize<Configure.Server>(configureText??"");
        if (configure == null)
        {
            configure = new Configure.Server();
        }
        Logger.Level = Logger.ParseLevel(configure.LogLevel);
        if (string.IsNullOrWhiteSpace(configure.DefaultServer)) configure.DefaultServer = "8.8.8.8";
        if (configure.Port <= 0) configure.Port = 53;
        configure.UseUDP = true;
        return configure;
    }

    public bool Save()
    {
        var serializer = new StaticSerializerBuilder(new StaticContext())
              .WithNamingConvention(PascalCaseNamingConvention.Instance)
              .Build();
        string text=serializer.Serialize(this);
        try
        {
            System.IO.File.WriteAllText(CONFIGURE_FILE,text);
            return true;
        }
        catch
        {
            return false;
        }
    }
}