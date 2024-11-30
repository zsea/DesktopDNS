using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDNS.ViewModels
{
    internal class LogsViewModel : ViewModelBase
    {
        public string LogsText { get => string.Join("\r\n", Logger.Logs.Where(x=>x.level>=Logger.Level).Select(p=>$"[{p.time.ToString("yyyy-MM-dd HH:mm:ss")}] [{p.level.ToString()}] {p.message}").ToArray()); }
        public LogsViewModel()
        {
            Logger.OnAppended = (log) =>
            {
                OnPropertyChanged(nameof(LogsText));
            };
        }
        ~LogsViewModel()
        {
            Logger.OnAppended = null;
        }
    }
}
