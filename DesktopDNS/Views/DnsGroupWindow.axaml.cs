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
            this.alert("���Ʋ���Ϊ�ա�", "��ʾ");
            return;
        }
        if (!Helper.IsIPv4(model.Server))
        {
            this.alert("Ĭ��DNS��������Ч��������һ����Ч��IPv4��ַ��", "��ʾ");
            return;
        }
        if (Server.configure.Groups != null)
        {
            if (model.IsNew)
            {
                if (Server.configure.Groups.Exists(x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    this.alert("���ܴ�����ͬ�ķ������ơ�", "��ʾ");
                    return;
                }
            }
            else
            {
                if(!string.Equals(model.Name,model.OriginName, StringComparison.CurrentCultureIgnoreCase))
                {
                    // �޸�������
                    if (Server.configure.Groups.Exists(x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        this.alert("���ܴ�����ͬ�ķ������ơ�", "��ʾ");
                        return;
                    }
                }
            }

        }
        model.IsOk = true;
        Close();

    }
}