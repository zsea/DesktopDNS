using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDNS.ViewModels
{
    public class AboutViewModel: ViewModelBase
    {
        public string Version { get;  } = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}
