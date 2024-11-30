using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DesktopDNS.ViewModels;
using System;
using System.Net;

namespace DesktopDNS.Views;

public partial class DnsGroupWindow : Window
{
    public DnsGroupWindow()
    {
        InitializeComponent();
    }

    private void OnCancel(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        Close();

    }
    private void OnOk(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        //if(!IPAddress.TryParse())
        if (sender == null) return;
        ViewModels.DnsGroupViewModel? model = sender.GetContext<DnsGroupViewModel>();
        if (model == null) { return; }
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            this.alert("名称不能为空。", "提示");
            return;
        }
        if (!Helper.IsIPv4(model.Server))
        {
            this.alert("默认DNS服务器无效。必须是一个有效的IPv4地址。", "提示");
            return;
        }
        if (Server.configure.Groups != null)
        {
            if (model.IsNew)
            {
                if (Server.configure.Groups.Exists(x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    this.alert("不能存在相同的分组名称。", "提示");
                    return;
                }
            }
            else
            {
                if(!string.Equals(model.Name,model.OriginName, StringComparison.CurrentCultureIgnoreCase))
                {
                    // 修改了名称
                    if (Server.configure.Groups.Exists(x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        this.alert("不能存在相同的分组名称。", "提示");
                        return;
                    }
                }
            }

        }
        model.IsOk = true;
        Close();

    }
}