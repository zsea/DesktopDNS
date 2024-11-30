using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DesktopDNS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DesktopDNS.Views
{
    public partial class MainWindow : Window
    {
        private void TrayIcon()
        {
            var notifyIcon = new TrayIcon();
            notifyIcon.Menu ??= new NativeMenu();
            notifyIcon.ToolTipText = "DesktopDNS";
            //var assets = AvaloniaLocator
            //AssetLoader.Exists()
            var bitmap = new Bitmap(AssetLoader.Open(new Uri("avares://DesktopDNS/Assets/DesktopDNS.ico")));
            notifyIcon.Icon = new WindowIcon(bitmap);
            var status = new NativeMenuItem() { Header = "状态" };
            status.Click += (s, e) => { SwitchContent("status"); };
            var settings = new NativeMenuItem() { Header = "设置" };
            settings.Click += (s, e) => { SwitchContent("settings"); };
            var logs = new NativeMenuItem() { Header = "日志" };
            logs.Click += (s, e) => { SwitchContent("logs"); };
            var about = new NativeMenuItem() { Header = "关于" };
            about.Click += (s, e) => { SwitchContent("about"); };

            notifyIcon.Menu.Add(status);
            notifyIcon.Menu.Add(settings);
            notifyIcon.Menu.Add(logs);
            notifyIcon.Menu.Add(about);
            notifyIcon.Menu.Add(new NativeMenuItemSeparator());

            var exit = new NativeMenuItem() { Header = "退出" };
            exit.Click += async (sender, args) => {
                if (Server.IsRuning) {
                    bool isVisible = this.IsVisible;
                    if (!isVisible)
                    {
                        this.Show();
                    }
                    if(!await this.confirm("DNS服务正在运行，请确认是否退出程序？", "请确认"))
                    {
                        if (!isVisible) {
                            this.Hide();
                        }
                        return;
                    }
                }
                Environment.Exit(0);
            };
            notifyIcon.Menu.Add(exit);
            notifyIcon.Clicked += (sender, args) => {
                this.Show();
            };
        }
        private void SwitchContent(string name)
        {
            ViewModels.MainWindowViewModel? context = this.DataContext as ViewModels.MainWindowViewModel;
            if (context == null) return;
            context.SetChecked(name);
        }

        /// <summary>
        /// 启动时是否隐藏窗口
        /// </summary>
        public bool IsHide { get; private set; }
        /// <summary>
        /// 启动时是否自动启动服务
        /// </summary>
        public bool IsAuto { get; private set; }
        public MainWindow():this(false,false)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isHide">启动时是否隐藏窗口</param>
        /// <param name="isAuto">启动时是否自动启动服务</param>
        public MainWindow(bool isHide,bool isAuto)
        {
            this.IsHide = isHide;
            this.IsAuto = isAuto;
            InitializeComponent();
            TrayIcon();
            
        }
       
        protected override void OnClosing(WindowClosingEventArgs e)
        {
            if (Server.IsRuning)
            {
                e.Cancel = true;
                this.Hide();
                
            }
            else
            {
                base.OnClosing(e);
            }
            //base.OnClosing(e);
        }
        //public Interaction<DnsGroupViewModel, string?> ShowDialog { get; }
        /// <summary>
        /// 存储右边内容区域对应不同菜单的Model
        /// </summary>
        //private readonly Dictionary<string, ViewModelBase> contentAreas = new Dictionary<string, ViewModelBase>();
        /// <summary>
        /// 左边菜单切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickMenuItem(object sender, RoutedEventArgs e)
        {
            Border? fromer = sender as Border;
            if (fromer == null) return;
            ViewModels.MainWindowViewModel? context = this.DataContext as ViewModels.MainWindowViewModel;
            if (context == null) return;
            if (fromer.Name == null) return;
            context.SetChecked(fromer.Name);

        }

        /// <summary>
        /// 重新加载分组管理数据
        /// </summary>
        private void ReloadSettingsField(string field)
        {
            MainWindowViewModel? main = this.DataContext as MainWindowViewModel;
            if (main == null) return;
            SettingsViewModel? settings = main.ContentArea as SettingsViewModel;
            if (settings == null) return;
            settings.OnPropertyChanged(field);
        }
        /// <summary>
        /// 设置页面编辑分组信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickGroupItemButtons(object sender, RoutedEventArgs e)
        {
            Label? label = sender as Label;
            if (label == null) return;
            if (label.Name == "editor")
            {
                if (Server.configure.Groups == null) return;
                Configure.DnsGroup? group = label.DataContext as Configure.DnsGroup;
                if (group == null) return;

                DnsGroupWindow window = new DnsGroupWindow();
                DnsGroupViewModel model = new DnsGroupViewModel(group);
                //groupViewModel.Title = "编辑分组";
                window.DataContext = model;
                await window.ShowDialog(this);
                if (!model.IsOk) return;
                group.Enable = model.Enable;
                group.Name = model.Name;
                group.Server = model.Server;
                Server.configure.Save();
                this.ReloadSettingsField("Groups");
            }
            else if (label.Name == "delete")
            {
                if (Server.configure.Groups == null) return;
                Configure.DnsGroup? group = label.DataContext as Configure.DnsGroup;
                if (group == null) return;
                if (await this.confirm($"你确定要删除分组 [{group.Name}] 吗？", "请确认"))
                {
                    Server.configure.Groups = Server.configure.Groups.Where(x => x.Name != group.Name).ToList();
                    Server.configure.Save();
                    this.ReloadSettingsField("Groups");
                }
            }
            else if(label.Name== "settings")
            {
                if (Server.configure.Groups == null) return;
                Configure.DnsGroup? group = label.DataContext as Configure.DnsGroup;
                if (group == null) return;

                DomainWindow window = new DomainWindow();
                DomainViewModel model = new DomainViewModel(group);
                window.DataContext = model;
                await window.ShowDialog(this);
            }
        }
        private async void OnClickRemoteItemButtons(object sender, RoutedEventArgs e)
        {
            Label? label = sender as Label;
            if (label == null) return;
            if (label.Name == "editor")
            {
                if (Server.configure.Remotes == null) return;
                Configure.RemoteRule? rule = label.DataContext as Configure.RemoteRule;
                if (rule == null) return;

                RemoteWindow window = new RemoteWindow();
                RemoteViewModel model = new RemoteViewModel(rule);
                //groupViewModel.Title = "编辑分组";
                window.DataContext = model;
                await window.ShowDialog(this);
                if (!model.IsOk) return;
                rule.Enable = model.Enable;
                rule.Name = model.Name;
                rule.Url = model.Url;
                rule.Interval = model.Interval;
                Server.configure.Save();
                this.ReloadSettingsField("Remotes");
            }
            else if (label.Name == "delete")
            {
                if (Server.configure.Remotes == null) return;
                Configure.RemoteRule? rule = label.DataContext as Configure.RemoteRule;
                if (rule == null) return;
                if (await this.confirm($"你确定要删除远程规则 [{rule.Name}] 吗？", "请确认"))
                {
                    Server.configure.Remotes = Server.configure.Remotes.Where(x => x.Name != rule.Name).ToList();
                    Server.configure.Save();
                    this.ReloadSettingsField("Remotes");
                }
            }
        }
        /// <summary>
        /// 开启DNS服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStart(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button == null) return;
            ViewModels.StatusViewModel? model = button.DataContext as ViewModels.StatusViewModel;
            if (model == null) return;
            model.Startup();
        }
        /// <summary>
        /// 关闭DNS服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnShutdown(object sender, RoutedEventArgs e)
        {
            if (MessageBoxViewModel.MessageBoxResult.OK != await ViewModels.MessageBoxViewModel.Show(this, "请确认", "你确定要停止DNS服务吗？", MessageBoxViewModel.MessageBoxIcon.Question))
            {
                return;
            }
            Button? button = sender as Button;
            if (button == null) return;
            ViewModels.StatusViewModel? model = (button.DataContext) as ViewModels.StatusViewModel;
            if (model == null) return;
            model.Shutdown();
        }

        /// <summary>
        /// 保存服务设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSaveServerSettings(object sender, RoutedEventArgs e)
        {
            SettingsViewModel? model = sender.GetContext<SettingsViewModel>();
            if (model == null) return;
            if (!Helper.IsIPv4(model.DefaultServer))
            {
                this.alert("默认服务器不是有效的IP地址。", "提示");
                //_ =ViewModels.MessageBoxViewModel.Show(this, "错误", "默认服务器不是有效的IP地址。", MessageBoxViewModel.MessageBoxIcon.Error, MessageBoxViewModel.MessageBoxButton.OK);
                return;
            }
            Server.configure.Port = model.Port;
            Server.configure.AutoRun = model.AutoRun;
            Server.configure.DefaultServer = model.DefaultServer;
            Server.configure.LogLevel = model.LogLevel;
            if (Server.configure.Save())
            {
                Logger.Level=Logger.ParseLevel(model.LogLevel);
                this.alert("保存成功，配置内容将在下次启动服务时生效。", "提示");
            }
            else
            {
                this.error("保存失败，请检查设置是否正确。");
            }
        }
        public async void OnClickAddGroup(object sender, RoutedEventArgs e)
        {
            DnsGroupWindow window = new DnsGroupWindow();
            ViewModels.DnsGroupViewModel dnsGroupViewModel = new ViewModels.DnsGroupViewModel();

            window.DataContext = dnsGroupViewModel;
            await window.ShowDialog(this);
            if (dnsGroupViewModel.IsOk)
            {
                // 开始保存
                Configure.DnsGroup group = new Configure.DnsGroup() { Enable = dnsGroupViewModel.Enable, Server = dnsGroupViewModel.Server, Name = dnsGroupViewModel.Name };
                if (Server.configure.Groups == null)
                {
                    Server.configure.Groups = new List<Configure.DnsGroup>();
                }
                else if (Server.configure.Groups.Exists(x => string.Equals(x.Name, group.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    this.alert("不能存在相同的分组名称。", "提示");
                    return;
                }
                Server.configure.Groups.Add(group);
                Server.configure.Save();
                this.ReloadSettingsField("Groups");
            }
        }
        public async void OnClickAddRemote(object sender, RoutedEventArgs e)
        {
            RemoteWindow window = new RemoteWindow();
            ViewModels.RemoteViewModel model = new ViewModels.RemoteViewModel();

            window.DataContext = model;
            await window.ShowDialog(this);
            if (model.IsOk)
            {
                // 开始保存
                Configure.RemoteRule remote = new Configure.RemoteRule()
                {
                    Enable = model.Enable,
                    Interval = model.Interval,
                    Name = model.Name,
                    Url = model.Url,
                };
                if (Server.configure.Remotes == null)
                {
                    Server.configure.Remotes = new List<Configure.RemoteRule>();
                }
                else if (Server.configure.Remotes.Exists(x => string.Equals(x.Name, remote.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    this.alert("不能存在相同的规则名称。", "提示");
                    return;
                }
                Server.configure.Remotes.Add(remote);
                Server.configure.Save();
                this.ReloadSettingsField("Remotes");
            }
        }
        public void OnSwitchLogLevel(object sender, RoutedEventArgs e)
        {
            MenuItem? menu = sender as MenuItem;
            if (menu == null||menu.Header==null) return;
            string level= menu.Header.ToString()??"";
            SettingsViewModel? svm = menu.DataContext as SettingsViewModel;
            if (svm == null) return;
            svm.LogLevel=level;
            svm.OnPropertyChanged("LogLevel");
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);
            if (this.IsHide) {
                this.Hide();
            }
            if (this.IsAuto)
            {
                MainWindowViewModel? main=this.DataContext as MainWindowViewModel;
                if (main != null) {
                    ViewModelBase? status = null;
                    main.contentAreas.TryGetValue("status", out status);
                    if (status != null) { 
                    
                        StatusViewModel? svm=status as StatusViewModel;
                        if (svm != null) {
                            svm.Startup();
                        }
                    }
                }
                
            }
        }
    }
}