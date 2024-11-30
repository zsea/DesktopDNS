using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDNS.ViewModels
{
    internal class StatusViewModel : ViewModelBase
    {
        //private ulong response_times = 0;
        public bool IsRuning { get => Server.IsRuning; }
        public string RunText
        {
            get
            {
                long unixTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                long minutes = (unixTimestamp - Server.StartupTime) / 60;
                return $"{minutes} 分钟";
            }
        }
        public string HandleTimes { get => $"{Server.RequestedTimes} 次"; }
        public string LocalResolveTimes { get => $"{Server.LocalResolveTimes} 次"; }
        public long CacheCount { get => 0; }
        public string? DefaultServer { get => Server.DefaultServer; }
        public int Port { get => Server.configure.Port; }
        public int ListeningPort { get=>Server.ListenPort; }

        private System.Timers.Timer timer;
        public void Startup()
        {
            if (!Server.Start()) return;
            OnPropertyChanged(nameof(IsRuning));
            OnPropertyChanged(nameof(DefaultServer));
            OnPropertyChanged(nameof(Port));
            OnPropertyChanged(nameof(RunText));
            OnPropertyChanged(nameof(HandleTimes));
            OnPropertyChanged(nameof(ListeningPort));
            timer.Start();

        }

        private void OnTimer(object? sender, System.Timers.ElapsedEventArgs e)
        {
            OnPropertyChanged(nameof(RunText));
        }

        public void Shutdown()
        {
            timer.Stop();
            Server.Shutodwn();
            OnPropertyChanged(nameof(IsRuning));
            OnPropertyChanged(nameof(DefaultServer));
            OnPropertyChanged(nameof(Port));
            

        }

        public StatusViewModel()
        {
            Server.OnRequested = (e) =>
            {
                OnPropertyChanged(nameof(HandleTimes));
                OnPropertyChanged(nameof(LocalResolveTimes));
            };
            timer = new System.Timers.Timer(60 * 1000);
            timer.Elapsed += OnTimer;
            if (Server.IsRuning)
            {
                timer.Start();
            }
        }
        ~StatusViewModel()
        {
            timer.Stop();
            Server.OnRequested = null;
            Server.OnResponded = null;
        }

    }
}
