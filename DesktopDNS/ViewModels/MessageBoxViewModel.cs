using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDNS.ViewModels
{
    internal class MessageBoxViewModel:ViewModelBase
    {
        public enum MessageBoxResult { OK, Cancel };
        public enum MessageBoxIcon { None,Info,Error,Stop,Question };
        public enum MessageBoxButton { OK=1,Cancel=2};

        private MessageBoxIcon icon = MessageBoxIcon.None;
        private MessageBoxButton buttons = MessageBoxButton.OK | MessageBoxButton.Cancel;
        public static async Task<MessageBoxResult> Show(Avalonia.Controls.Window owner,string title,string message,MessageBoxIcon icon,MessageBoxButton buttons)
        {
            Views.MessageBoxWindow box = new Views.MessageBoxWindow()
            {
                Title = title,
            };
            MessageBoxViewModel model= new MessageBoxViewModel() { Title = title, Message = message,icon=icon,buttons=buttons };
            box.DataContext = model;
            await box.ShowDialog(owner);
            return model.Result;
        }
        public static Task<MessageBoxResult> Show(Avalonia.Controls.Window owner, string title, string message)
        {
            return Show(owner, title, message, MessageBoxIcon.None, MessageBoxButton.OK | MessageBoxButton.Cancel);
        }
        public static Task<MessageBoxResult> Show(Avalonia.Controls.Window owner, string title, string message,MessageBoxIcon icon)
        {
            return Show(owner, title, message, icon, MessageBoxButton.OK | MessageBoxButton.Cancel);
        }
        public static Task<MessageBoxResult> Show(Avalonia.Controls.Window owner, string title, string message, MessageBoxButton buttons)
        {
            return Show(owner, title, message, MessageBoxIcon.None, buttons);
        }
        public string Title { get; private set; } = "DesktopDNS";
        public string Message { get; private set; } = "";
        public string Icon
        {
            get
            {
                switch (icon)
                {
                    case MessageBoxIcon.Info: return "/Assets/info.svg";
                    case MessageBoxIcon.Error: return "/Assets/error.svg";
                    case MessageBoxIcon.Stop: return "/Assets/stop.svg";
                    case MessageBoxIcon.Question: return "/Assets/question.svg";
                }
                return string.Empty;
            }
        }
        public string OKText { get; private set; } = "确定";
        public string CancelText { get; private set; } = "取消";
        public bool ButtonOKIsVisible { get {
                return (this.buttons & MessageBoxButton.OK) == MessageBoxButton.OK;
            } }
        public bool ButtonCancelIsVisible { get {
                return (this.buttons & MessageBoxButton.Cancel) == MessageBoxButton.Cancel;
            } }

        internal MessageBoxResult Result { get; set; }
    }
}
