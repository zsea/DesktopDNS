using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;

namespace DesktopDNS.ViewModels
{
    internal class SettingsViewModel : ViewModelBase
    {
        public List<I18n<Configure.DnsGroup>> Groups { get => Server.configure.Groups == null ? new List<I18n<Configure.DnsGroup>>() : Server.configure.Groups.Select(x => new I18n<Configure.DnsGroup>(x)).ToList(); }
        public List<I18n<Configure.RemoteRule>> Remotes { get => Server.configure.Remotes == null ? new List<I18n<Configure.RemoteRule>>() : Server.configure.Remotes.Select(x => new I18n<Configure.RemoteRule>(x)).ToList(); }
        public string LogLevel { get; set; }
        public string? CurrentLanguage { get; set; }
        public MenuItem[] Languages { get {

             return I18n.GetLanguages().Select(item => {
                 MenuItem menu = new MenuItem();
                 menu.Header = $"{item.Name}({item.Code})";

                 
                 menu.Click += (sender, e) => {
                     
                     this.CurrentLanguage = item.Code;
                     this.OnPropertyChanged(nameof(CurrentLanguage));
                     I18n.i18n.Change(item.Code);
                 };
                 return menu;
             }).ToArray();
            } }
        public SettingsViewModel() {
            this.Port = Server.configure.Port;
            this.DefaultServer = Server.configure.DefaultServer;
            this.AutoRun = Server.configure.AutoRun;
            this.LogLevel = Server.configure.LogLevel;
            this.CurrentLanguage = Server.configure.Language;
            // OnPropertyChanged()
            //I18n.GetLanguages();
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
