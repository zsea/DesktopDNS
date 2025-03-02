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
using System.Reflection;

namespace DesktopDNS.Views
{
    public partial class MainWindow : Window
    {
        private void TrayIcon()
        {
            //byte[] msg = new byte[] { 0, 98, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 3, 119, 119, 119, 3, 108, 115, 122, 3, 103, 111, 118, 2, 99, 110, 0, 0, 28, 0, 1 };
            //DNS.Protocol.Request.FromArray(msg);

            //System.IO.File.AppendAllLines("G:\\project_codes\\desktop_dns\\desktop_dns\\DesktopDNS\\bin\\Release\\net9.0\\publish\\win-x64\\d.txt", new string[] { "[Question] ====", "x:" + x, "y:" + y, "[Question] ====" });
            var notifyIcon = new TrayIcon();
            notifyIcon.Menu ??= new NativeMenu();
            notifyIcon.ToolTipText = "DesktopDNS";
            //var assets = AvaloniaLocator
            //AssetLoader.Exists()
            var bitmap = new Bitmap(AssetLoader.Open(new Uri("avares://DesktopDNS/Assets/DesktopDNS.ico")));
            notifyIcon.Icon = new WindowIcon(bitmap);
            var status = new NativeMenuItem() { Header = I18n.i18n.Menu_Status };
            status.Click += (s, e) => { SwitchContent("status"); };
            var settings = new NativeMenuItem() { Header = I18n.i18n.Menu_Settings };
            settings.Click += (s, e) => { SwitchContent("settings"); };
            var logs = new NativeMenuItem() { Header = I18n.i18n.Menu_Logs };
            logs.Click += (s, e) => { SwitchContent("logs"); };
            var about = new NativeMenuItem() { Header = I18n.i18n.Menu_About };
            about.Click += (s, e) => { SwitchContent("about"); };

            notifyIcon.Menu.Add(status);
            notifyIcon.Menu.Add(settings);
            notifyIcon.Menu.Add(logs);
            notifyIcon.Menu.Add(about);
            notifyIcon.Menu.Add(new NativeMenuItemSeparator());

            var exit = new NativeMenuItem() { Header = I18n.i18n.Menu_Exit };
            exit.Click += async (sender, args) =>
            {
                if (Server.IsRuning)
                {
                    bool isVisible = this.IsVisible;
                    if (!isVisible)
                    {
                        this.Show();
                    }
                    if (!await this.confirm(I18n.i18n.Confirm_Exit_Message, I18n.i18n.Confirm_Default_Title))
                    {
                        if (!isVisible)
                        {
                            this.Hide();
                        }
                        return;
                    }
                }
                Environment.Exit(0);
            };
            notifyIcon.Menu.Add(exit);
            notifyIcon.Clicked += (sender, args) =>
            {
                this.Show();
            };
            I18n.i18n.PropertyChanged += (sender, args) => { 
                if(args.PropertyName == nameof(I18n.i18n.Menu_Status))
                {
                    about.Header = I18n.i18n.Menu_Status;
                }
                else if (args.PropertyName == nameof(I18n.i18n.Menu_Settings))
                {
                    about.Header = I18n.i18n.Menu_Settings;
                }
                else if (args.PropertyName == nameof(I18n.i18n.Menu_Logs))
                {
                    about.Header = I18n.i18n.Menu_Logs;
                }
                else if (args.PropertyName == nameof(I18n.i18n.Menu_About))
                {
                    about.Header = I18n.i18n.Menu_About;
                }
                else if (args.PropertyName == nameof(I18n.i18n.Menu_Exit))
                {
                    about.Header = I18n.i18n.Menu_Exit;
                }
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
        public MainWindow() : this(false, false)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isHide">启动时是否隐藏窗口</param>
        /// <param name="isAuto">启动时是否自动启动服务</param>
        public MainWindow(bool isHide, bool isAuto)
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
                Configure.DnsGroup? group = (label.DataContext as I18n<Configure.DnsGroup>)?.Value;
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
                Configure.DnsGroup? group = (label.DataContext as I18n<Configure.DnsGroup>)?.Value;
                if (group == null) return;
                if (await this.confirm(string.Format(I18n.i18n.Settings_Group_Confirm_Delete, group.Name), I18n.i18n.Confirm_Default_Title))
                {
                    Server.configure.Groups = Server.configure.Groups.Where(x => x.Name != group.Name).ToList();
                    Server.configure.Save();
                    this.ReloadSettingsField("Groups");
                }
            }
            else if (label.Name == "settings")
            {
                if (Server.configure.Groups == null) return;
                Configure.DnsGroup? group = (label.DataContext as I18n<Configure.DnsGroup>)?.Value;
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
                Configure.RemoteRule? rule = (label.DataContext as I18n<Configure.RemoteRule>)?.Value;
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
                Configure.RemoteRule? rule = (label.DataContext as I18n<Configure.RemoteRule>)?.Value;
                if (rule == null) return;
                if (await this.confirm(string.Format(I18n.i18n.Settings_Remote_Confirm_Delete,rule.Name), I18n.i18n.Confirm_Default_Title))
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
            if (!model.Startup())
            {
                this.error(I18n.i18n.Settings_Service_Error_Startup_Fail, I18n.i18n.Confirm_Error_Title);
            }
        }
        /// <summary>
        /// 关闭DNS服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnShutdown(object sender, RoutedEventArgs e)
        {
            if (MessageBoxViewModel.MessageBoxResult.OK != await ViewModels.MessageBoxViewModel.Show(this, I18n.i18n.Confirm_Default_Title, I18n.i18n.Confirm_Stop_Message, MessageBoxViewModel.MessageBoxIcon.Question))
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
                this.alert(I18n.i18n.Settings_Service_Error_IPv4_Invalid, I18n.i18n.Confirm_Info_Title);
                //_ =ViewModels.MessageBoxViewModel.Show(this, "错误", "默认服务器不是有效的IP地址。", MessageBoxViewModel.MessageBoxIcon.Error, MessageBoxViewModel.MessageBoxButton.OK);
                return;
            }
            //bool changedAutoRun = Server.configure.AutoRun != model.AutoRun;
            Server.configure.Port = model.Port;
            Server.configure.AutoRun = model.AutoRun;
            Server.configure.DefaultServer = model.DefaultServer;
            Server.configure.LogLevel = model.LogLevel;

            if (Server.configure.Save())
            {

                string appName = "DesktopDNS";
                if (Server.configure.AutoRun)
                {
                    string? appPath = Environment.ProcessPath;
                    if (appPath != null)
                    {
                        if (OperatingSystem.IsWindows())
                        {
                            Helper.SetAutoStartRegistry(appName, appPath);
                        }
                        else if (OperatingSystem.IsLinux())
                        {
                            Helper.SetLinuxAutoStart(appName, appPath);
                        }

                    }
                }
                else
                {
                    if (OperatingSystem.IsWindows())
                    {
                        Helper.RemoveAutoStartRegistry(appName);
                    }
                    else if (OperatingSystem.IsLinux())
                    {
                        Helper.RemoveLinuxAutoStart(appName);
                    }

                }

                Logger.Level = Logger.ParseLevel(model.LogLevel);
                this.alert(I18n.i18n.Settings_Service_Save_Success, I18n.i18n.Confirm_Info_Title);
            }
            else
            {
                this.error(I18n.i18n.Settings_Service_Error_Save_Fail, I18n.i18n.Confirm_Error_Title);
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
                    this.alert(I18n.i18n.Settings_Group_Window_Error_Name_Exists, I18n.i18n.Confirm_Info_Title);
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
                    this.alert(I18n.i18n.Settings_Remote_Window_Error_Name_Exists, I18n.i18n.Confirm_Info_Title);
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
            if (menu == null || menu.Header == null) return;
            string level = menu.Header.ToString() ?? "";
            SettingsViewModel? svm = menu.DataContext as SettingsViewModel;
            if (svm == null) return;
            svm.LogLevel = level;
            svm.OnPropertyChanged("LogLevel");
        }
        public void OnSaveSystemSettings(object sender, RoutedEventArgs e)
        {
            SettingsViewModel? model = sender.GetContext<SettingsViewModel>();
            if (model == null) return;
            Server.configure.Language = model.CurrentLanguage;
            if (Server.configure.Save())
            {
                this.alert(I18n.i18n.Settings_Save_Success, I18n.i18n.Confirm_Info_Title);
                //TrayIcon();
            }
            else
            {
                this.error(I18n.i18n.Settings_Error_Save_Fail, I18n.i18n.Confirm_Info_Title);
            }
        }
        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);
            if (this.IsHide)
            {
                this.Hide();
            }
            if (this.IsAuto)
            {
                MainWindowViewModel? main = this.DataContext as MainWindowViewModel;
                if (main != null)
                {
                    ViewModelBase? status = null;
                    main.contentAreas.TryGetValue("status", out status);
                    if (status != null)
                    {

                        StatusViewModel? svm = status as StatusViewModel;
                        if (svm != null)
                        {
                            if (!svm.Startup())
                            {
                                this.error(I18n.i18n.Settings_Service_Error_Startup_Fail, I18n.i18n.Confirm_Error_Title);
                            }
                        }
                    }
                }

            }
        }
    }
}