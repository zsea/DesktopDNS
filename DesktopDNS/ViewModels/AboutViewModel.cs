using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDNS.ViewModels
{
    internal class AboutViewModel : ViewModelBase
    {

        public string Version
        {
            get
            {
                Version? version = Assembly.GetExecutingAssembly().GetName().Version;
                if (version == null) return "";
                return version.ToString();

            }
        }
    }
}
