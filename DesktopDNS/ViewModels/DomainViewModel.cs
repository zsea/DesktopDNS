using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDNS.ViewModels;

internal class DomainViewModel : ViewModelBase
{
    public string Name { get => Group.Name ?? ""; }

    public List<I18n<Configure.Domain>> Domains { get => Group.Domains == null ? new List<I18n<Configure.Domain>>() : Group.Domains.Select(x=>new I18n<Configure.Domain>(x)).ToList(); }
    public Configure.DnsGroup Group { get; private set; }
    public DomainViewModel(Configure.DnsGroup group)
    {
        this.Group = group;
    }
    public void ReloadField(string fieldName)
    {
        this.OnPropertyChanged(fieldName);
    }
}

