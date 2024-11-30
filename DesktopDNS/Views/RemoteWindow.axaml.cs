using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DesktopDNS.ViewModels;
using System;

namespace DesktopDNS.Views;

public partial class RemoteWindow : Window
{
    public RemoteWindow()
    {
        InitializeComponent();
    }
    private void OnCancel(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        Close();

    }
    private void OnOk(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (sender == null) return;
        ViewModels.RemoteViewModel? model = sender.GetContext<RemoteViewModel>();
        if (model == null) { return; }
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            this.alert("名称不能为空。", "提示");
            return;
        }
        if (!Helper.IsUrl(model.Url??""))
        {
            this.alert("URL地址无效。", "提示");
            return;
        }
        if(model.Interval<=0)
        {
            this.alert("更新间隔时间无效。", "提示");
            return;
        }
        if (Server.configure.Remotes != null)
        {
            if (model.IsNew)
            {
                if (Server.configure.Remotes.Exists(x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    this.alert("不能存在相同的分组名称。", "提示");
                    return;
                }
            }
            else
            {
                if (!string.Equals(model.Name, model.OriginName, StringComparison.CurrentCultureIgnoreCase))
                {
                    // 修改了名称
                    if (Server.configure.Remotes.Exists(x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)))
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