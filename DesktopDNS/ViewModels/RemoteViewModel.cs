using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDNS.ViewModels
{

    internal class RemoteViewModel:ViewModelBase
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
        /**
        更新间隔，单位：分钟
        **/
        public int Interval { get; set; } = 10;
        public bool Enable { get; set; } = true;
        public bool IsOk { get; set; } = false;
        public string Title
        {
            get
            {
                return IsNew ? "添加远程规则" : "编辑远程规则";
            }
        }
        public string OriginName { get; private set; } = "";
        public bool IsNew { get; private set; } = true;
        public RemoteViewModel(Configure.RemoteRule? remote = null)
        {
            if (remote == null) return;
            this.Name = remote.Name ?? "";
            this.Url = remote.Url ?? "";
            this.Enable = remote.Enable;
            this.OriginName = remote.Name ?? "";
            this.Interval = remote.Interval<=0?10:remote.Interval;
            this.IsNew = false;
        }
    }
}
