using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace DesktopDNS.Configure
{
    [YamlStaticContext]
    public partial class StaticContext : YamlDotNet.Serialization.StaticContext
    {
    }
}
