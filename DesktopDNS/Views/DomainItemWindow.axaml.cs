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
            this.alert("��������Ϊ�ա�", "��ʾ"); return;
        }
        //if (string.IsNullOrWhiteSpace(model.Value) && string.IsNullOrWhiteSpace(model.Server))
        //{
        //    this.alert("��¼ֵ�ͽ������������ܶ�Ϊ�ա�", "��ʾ"); return;
        //}
        if (!string.IsNullOrWhiteSpace(model.Server) && !Helper.IsIPv4(model.Server))
        {
            this.alert("��������������һ����Ч��IPv4��ַ��", "��ʾ"); return;
        }
        if(DomainItemViewModel.GetRecordType(model.RecordTypeIndex)=="A"&& !string.IsNullOrWhiteSpace(model.Value) && !Helper.IsIPv4(model.Value))
        {
            this.alert("��¼ֵ����һ����Ч��IPv4��ַ��", "��ʾ"); return;
        }
        if (model.IsNew&& model.Group.Domains != null&&model.Group.Domains.Exists(x=>string.Equals(x.Hostname,model.Hostname,System.StringComparison.CurrentCultureIgnoreCase)&&x.RecordType== DomainItemViewModel.GetRecordType(model.RecordTypeIndex)))
        {
            this.alert("���ܴ�����ͬ������������¼��", "��ʾ"); return;
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