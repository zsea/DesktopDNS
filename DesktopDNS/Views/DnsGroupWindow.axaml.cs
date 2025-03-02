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
            this.alert(I18n.i18n.Settings_Group_Window_Error_Name_Empty, I18n.i18n.Confirm_Info_Title);
            return;
        }
        if (!Helper.IsIPv4(model.Server))
        {
            this.alert(I18n.i18n.Settings_Group_Window_Error_DNS_Invalid, I18n.i18n.Confirm_Info_Title);
            return;
        }
        if (Server.configure.Groups != null)
        {
            if (model.IsNew)
            {
                if (Server.configure.Groups.Exists(x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    this.alert(I18n.i18n.Settings_Group_Window_Error_Name_Exists, I18n.i18n.Confirm_Info_Title);
                    return;
                }
            }
            else
            {
                if(!string.Equals(model.Name,model.OriginName, StringComparison.CurrentCultureIgnoreCase))
                {
                    // ÐÞ¸ÄÁËÃû³Æ
                    if (Server.configure.Groups.Exists(x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        this.alert(I18n.i18n.Settings_Group_Window_Error_Name_Exists, I18n.i18n.Confirm_Info_Title);
                        return;
                    }
                }
            }

        }
        model.IsOk = true;
        Close();

    }
}