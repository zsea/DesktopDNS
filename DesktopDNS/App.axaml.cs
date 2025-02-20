using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DesktopDNS.ViewModels;
using DesktopDNS.Views;
using ExCSS;
using System;
using System.Globalization;
using System.Linq;
using Ursa.Controls;

namespace DesktopDNS
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators?.RemoveAt(0);
                bool isHide = false,isAuto=false;
                if (desktop.Args != null)
                {
                    if (desktop.Args.FirstOrDefault(x => string.Equals(x, "--hide",StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        isHide = true;
                    }
                    if (desktop.Args.FirstOrDefault(x => string.Equals(x, "--auto",StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        isAuto = true;
                    }
                }
                desktop.MainWindow = new MainWindow(isHide,isAuto)
                {
                    DataContext = new MainWindowViewModel(),
                };
                
                
                
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}