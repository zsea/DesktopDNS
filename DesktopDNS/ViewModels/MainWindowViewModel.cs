using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace DesktopDNS.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CA1822 // Mark members as static
        //public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static

       
        private string checkedItem  = "status";
        
        public bool IsCheckedStatus => checkedItem== "status";
        public bool IsCheckedSettings => checkedItem == "settings";
        public bool IsCheckedLogs => checkedItem == "logs";
        public bool IsCheckedAbout => checkedItem == "about";

        public void SetChecked(string checkedName)
        {
            this.checkedItem = checkedName;
            OnPropertyChanged("IsCheckedStatus");
            OnPropertyChanged("IsCheckedSettings");
            OnPropertyChanged("IsCheckedLogs");
            OnPropertyChanged("IsCheckedAbout");
            OnPropertyChanged("ContentArea");
        }

        internal readonly Dictionary<string, ViewModelBase> contentAreas = new Dictionary<string, ViewModelBase>();
        public ViewModelBase ContentArea { get {
                ViewModelBase? viewModel = null;
                contentAreas.TryGetValue(this.checkedItem, out viewModel);
                if (viewModel == null)
                {
                    if(this.checkedItem == "status")
                    {
                        viewModel=new StatusViewModel();
                    }
                    else if(this.checkedItem == "settings")
                    {
                        viewModel=new SettingsViewModel();
                    }
                    else if (this.checkedItem == "logs")
                    {
                        viewModel=new LogsViewModel();
                    }
                    else if (this.checkedItem == "about")
                    {
                        viewModel=new AboutViewModel();
                    }
                    else
                    {
                        viewModel=new NotFoundViewModel();
                    }
                    contentAreas[this.checkedItem] = viewModel;
                }
                return viewModel;
            } }

        public new void OnPropertyChanged(string fieldName)
        {
            base.OnPropertyChanged(fieldName);
        }

    }
}
