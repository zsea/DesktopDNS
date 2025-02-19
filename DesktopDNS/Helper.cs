using Avalonia.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace DesktopDNS
{
    internal static class Helper
    {
        public static T? GetContext<T>(this object sender) where T : class
        {
            if (sender == null) return null;
            ContentControl? control = sender as ContentControl;
            if (control == null) return null;
            T? content = control.DataContext as T;

            return content;
        }
        public static void alert(this Avalonia.Controls.Window window, string message, string title = "DesktopDNS")
        {
            _ = ViewModels.MessageBoxViewModel.Show(window, title, message, ViewModels.MessageBoxViewModel.MessageBoxIcon.Info, ViewModels.MessageBoxViewModel.MessageBoxButton.OK);
        }
        public static void error(this Avalonia.Controls.Window window, string message, string title = "DesktopDNS")
        {
            _ = ViewModels.MessageBoxViewModel.Show(window, title, message, ViewModels.MessageBoxViewModel.MessageBoxIcon.Error, ViewModels.MessageBoxViewModel.MessageBoxButton.OK);
        }
        public static async Task<bool> confirm(this Avalonia.Controls.Window window, string message, string title = "DesktopDNS")
        {
            return ViewModels.MessageBoxViewModel.MessageBoxResult.OK == await ViewModels.MessageBoxViewModel.Show(window, title, message, ViewModels.MessageBoxViewModel.MessageBoxIcon.Question, ViewModels.MessageBoxViewModel.MessageBoxButton.OK | ViewModels.MessageBoxViewModel.MessageBoxButton.Cancel);
        }
        public static bool IsIPv4(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip)) return false;
            string[] segs = ip.Split(".");
            if (segs.Length != 4) return false;
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                if (!int.TryParse(segs[i], out value)) return false;
                if (value < 0 || value > 255) return false;
                if (i == 0 && value == 0) return false;
            }
            return true;
        }
        public static bool IsUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            if (!url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase) && !url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)) return false;
            return true;
        }
        // 通过注册表设置开机启动
        public static void SetAutoStartRegistry(string appName, string appPath)
        {
            if (!OperatingSystem.IsWindows()) return;
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(
                "Software\\Microsoft\\Windows\\CurrentVersion\\Run",
                writable: true
            );
            key?.SetValue(appName, $"\"{appPath}\"");

        }
        public static void RemoveAutoStartRegistry(string appName)
        {
            if (!OperatingSystem.IsWindows()) return;
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(
                "Software\\Microsoft\\Windows\\CurrentVersion\\Run",
                writable: true
            );

            key?.DeleteValue(appName, throwOnMissingValue: false);
        }

        public static void SetLinuxAutoStart(string appName, string appPath)
        {
            string autostartDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".config/autostart"
            );

            string desktopFilePath = Path.Combine(autostartDir, $"{appName}.desktop");

            Directory.CreateDirectory(autostartDir);
            string icon=Path.ChangeExtension(appPath, ".png");
            File.WriteAllText(desktopFilePath,
                $"[Desktop Entry]\n" +
                $"Type=Application\n" +
                $"Name={appName}\n" +
                $"Exec={appPath}\n" +
                $"Icon={icon}\n" +
                $"X-GNOME-Autostart-enabled=true\n");
        }

        public static void RemoveLinuxAutoStart(string appName)
        {
            string desktopFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".config/autostart",
                $"{appName}.desktop"
            );

            File.Delete(desktopFilePath);
        }
    }
}
