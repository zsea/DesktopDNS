using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DesktopDNS.Views;

public partial class MessageBoxWindow : Window
{
    public MessageBoxWindow()
    {
        InitializeComponent();
    }
    private void SetResult(ViewModels.MessageBoxViewModel.MessageBoxResult result)
    {
        ViewModels.MessageBoxViewModel? model=this.DataContext as ViewModels.MessageBoxViewModel; ;
        if (model == null) return;
        model.Result = result;
    }
    private void OnCancel(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        this.SetResult(ViewModels.MessageBoxViewModel.MessageBoxResult.Cancel);
        Close();

    }
    private void OnOK(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        this.SetResult(ViewModels.MessageBoxViewModel.MessageBoxResult.OK);
        Close();

    }
}