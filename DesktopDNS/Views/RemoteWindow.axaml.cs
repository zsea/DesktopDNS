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
            this.alert("���Ʋ���Ϊ�ա�", "��ʾ");
            return;
        }
        if (!Helper.IsUrl(model.Url??""))
        {
            this.alert("URL��ַ��Ч��", "��ʾ");
            return;
        }
        if(model.Interval<=0)
        {
            this.alert("���¼��ʱ����Ч��", "��ʾ");
            return;
        }
        if (Server.configure.Remotes != null)
        {
            if (model.IsNew)
            {
                if (Server.configure.Remotes.Exists(x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    this.alert("���ܴ�����ͬ�ķ������ơ�", "��ʾ");
                    return;
                }
            }
            else
            {
                if (!string.Equals(model.Name, model.OriginName, StringComparison.CurrentCultureIgnoreCase))
                {
                    // �޸�������
                    if (Server.configure.Remotes.Exists(x => string.Equals(x.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)))
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