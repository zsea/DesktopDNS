﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDNS.ViewModels
{
    internal class DnsGroupViewModel:ViewModelBase
    {
        public string Name { get; set; } = "";
        public string Server { get; set; } = "";
        public bool Enable { get; set; } = true;

        public bool IsOk { get; set; } = false;
        public string Title { get {
                return IsNew ? "添加分组" : "编辑分组";
            } }
        public string OriginName { get; private set; } = "";
        public bool IsNew { get; private set; } = true;
        public DnsGroupViewModel(Configure.DnsGroup? group=null) {
            if (group == null) return;
            this.Name = group.Name??"";
            this.Server = group.Server ?? "";
            this.Enable = group.Enable;
            this.OriginName = group.Name ?? "";
            this.IsNew = false;
        }
    }
}