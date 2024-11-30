using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;

namespace DesktopDNS.ViewModels
{
    internal class SettingsViewModel:ViewModelBase
    {
        public List<Configure.DnsGroup> Groups { get => Server.configure.Groups == null ? new List<Configure.DnsGroup>() : Server.configure.Groups.ToList(); }
        public List<Configure.RemoteRule> Remotes { get=>Server.configure.Remotes==null?new List<Configure.RemoteRule>():Server.configure.Remotes.ToList(); }
        public string LogLevel { get; set; }
        public SettingsViewModel() {
            this.Port = Server.configure.Port;
            this.DefaultServer = Server.configure.DefaultServer;
            this.AutoRun = Server.configure.AutoRun;
            this.LogLevel = Server.configure.LogLevel;
            // OnPropertyChanged()
        }
        public int Port { get; set; }
        public string DefaultServer { get;set; }
        public bool AutoRun { get; set; }
        public new void OnPropertyChanged(string name)
        {
            base.OnPropertyChanged(name);
        }
    }
}
