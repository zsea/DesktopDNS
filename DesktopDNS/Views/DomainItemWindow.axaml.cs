using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DesktopDNS.ViewModels;
using System.Text.RegularExpressions;

namespace DesktopDNS.Views;

public partial class DomainItemWindow : Window
{
    public DomainItemWindow()
    {
        InitializeComponent();
    }
    private void OnOk(object? sender, Avalonia.Input.TappedEventArgs e) { 
        DomainItemViewModel? model= this.DataContext as DomainItemViewModel;
        if (model == null) return;
        if (string.IsNullOrWhiteSpace(model.Hostname))
        {
            this.alert(I18n.i18n.Settings_Domain_Window_Error_Domain_Empty, I18n.i18n.Confirm_Info_Title); return;
        }
        //if (string.IsNullOrWhiteSpace(model.Value) && string.IsNullOrWhiteSpace(model.Server))
        //{
        //    this.alert("记录值和解析服务器不能都为空。", "提示"); return;
        //}
        if (!string.IsNullOrWhiteSpace(model.Server) && !Helper.IsIPv4(model.Server))
        {
            this.alert(I18n.i18n.Settings_Domain_Window_Error_IPv4_Invalid, I18n.i18n.Confirm_Info_Title); return;
        }
        if(DomainItemViewModel.GetRecordType(model.RecordTypeIndex)=="A"&& !string.IsNullOrWhiteSpace(model.Value) && !Helper.IsIPv4(model.Value))
        {
            this.alert(I18n.i18n.Settings_Domain_Window_Error_Record_Invalid, I18n.i18n.Confirm_Info_Title); return;
        }
        if (model.IsNew&& model.Group.Domains != null&&model.Group.Domains.Exists(x=>string.Equals(x.Hostname,model.Hostname,System.StringComparison.CurrentCultureIgnoreCase)&&x.RecordType== DomainItemViewModel.GetRecordType(model.RecordTypeIndex)))
        {
            this.alert(I18n.i18n.Settings_Domain_Window_Error_Exists, I18n.i18n.Confirm_Info_Title); return;
        }
        if (DomainItemViewModel.GetMode(model.ModeIndex) == "REGEX")
        {
            Regex regex = new Regex(model.Hostname);
            
        }
        model.IsOk = true;
        Close();
    }
    private void OnCancel(object? sender, Avalonia.Input.TappedEventArgs e) { 
        Close();
    }
}