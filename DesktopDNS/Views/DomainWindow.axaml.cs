using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System;
using DesktopDNS.ViewModels;
using System.Linq;

namespace DesktopDNS.Views;

public partial class DomainWindow : Window
{
    public DomainWindow()
    {
        InitializeComponent();
    }
    private async void OnClickItemButtons(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        DomainViewModel? items = this.DataContext as DomainViewModel;
        if (items == null) return;
        Configure.DnsGroup group = items.Group;
        if(group.Domains==null) return;

        Label? label = sender as Label;
        if (label == null) return;
        if (label.Name == "editor")
        {
            Configure.Domain? domain = label.DataContext as Configure.Domain;
            if (domain == null) return;

            DomainItemWindow window = new DomainItemWindow();
            DomainItemViewModel model = new DomainItemViewModel(group,domain);
            //groupViewModel.Title = "编辑分组";
            window.DataContext = model;
            await window.ShowDialog(this);
            if (!model.IsOk) return;
            domain.Enable = model.Enable;
            domain.Value=model.Value;
            domain.Mode=DomainItemViewModel.GetMode(model.ModeIndex);
            domain.RecordType=DomainItemViewModel.GetRecordType(model.RecordTypeIndex);
            domain.Hostname=model.Hostname;
            domain.Server=model.Server;
            Server.configure.Save();
            items.ReloadField("Domains");
        }
        else if (label.Name == "delete")
        {

            Configure.Domain? domain = label.DataContext as Configure.Domain;
            if (domain == null) return;
            if (await this.confirm($"你确定要删除域名 [{domain.Hostname}] 的解析吗？", "请确认"))
            {
                group.Domains = group.Domains.Where(x => !string.Equals(x.Hostname,domain.Hostname,StringComparison.CurrentCultureIgnoreCase)||x.Mode!=domain.Mode).ToList();
                Server.configure.Save();
                items.ReloadField("Domains");
            }
        }
    }
    private async void OnClickAddDomain(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        
        DomainViewModel? items= this.DataContext as DomainViewModel;
        if(items == null) return;
        Configure.DnsGroup group = items.Group;
        DomainItemWindow window = new DomainItemWindow();
        ViewModels.DomainItemViewModel model = new ViewModels.DomainItemViewModel(group);

        window.DataContext = model;
        await window.ShowDialog(this);
        if (model.IsOk)
        {
            // 开始保存
            Configure.Domain domain = new Configure.Domain()
            {
                Enable = model.Enable,
                Hostname=model.Hostname,
                Mode = DomainItemViewModel.GetMode(model.ModeIndex),
                RecordType = DomainItemViewModel.GetRecordType(model.RecordTypeIndex),
                Server = model.Server,
                Value = model.Value,
            };
            if (group.Domains == null)
            {
                group.Domains= new List<Configure.Domain>();
            }
            else if (group.Domains.Exists(x => string.Equals(x.Hostname, domain.Hostname, StringComparison.CurrentCultureIgnoreCase)&&x.RecordType==domain.RecordType))
            {
                this.alert("不能存在相同的域名规则。", "提示");
                return;
            }
            group.Domains.Add(domain);
            Server.configure.Save();
            items.ReloadField("Domains");
        }
    }
}