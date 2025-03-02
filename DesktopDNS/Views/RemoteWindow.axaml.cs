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
            this.alert(I18n.i18n.Settings_Remote_Window_Error_Name_Empty, I18n.i18n.Confirm_Info_Title);
            return;
        }
        if (!Helper.IsUrl(model.Url??""))
        {
            this.alert(I18n.i18n.Settings_Remote_Window_Error_Url_Invalid, I18n.i18n.Confirm_Info_Title);
            return;
        }
        if(model.Interval<=0)
        {
            this.alert(I18n.i18n.Settings_Remote_Window_Error_Interval_Invalid, I18n.i18n.Confirm_Info_Title);
            return;
        }
        if (Server.configure.Remotes != null)
        {
            if (model.IsNew)
            {
                if (Server.configure.Remotes.Exists(x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    this.alert(I18n.i18n.Settings_Remote_Window_Error_Name_Exists, I18n.i18n.Confirm_Info_Title);
                    return;
                }
            }
            else
            {
                if (!string.Equals(model.Name, model.OriginName, StringComparison.CurrentCultureIgnoreCase))
                {
                    // ÐÞ¸ÄÁËÃû³Æ
                    if (Server.configure.Remotes.Exists(x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        this.alert(I18n.i18n.Settings_Remote_Window_Error_Name_Exists, I18n.i18n.Confirm_Info_Title);
                        return;
                    }
                }
            }

        }
        model.IsOk = true;
        Close();
    }
}