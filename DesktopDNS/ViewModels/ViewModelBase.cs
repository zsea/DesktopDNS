using CommunityToolkit.Mvvm.ComponentModel;

namespace DesktopDNS.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        internal I18n i18n => I18n.i18n;
    }
}
