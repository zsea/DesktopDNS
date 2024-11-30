using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDNS.Converters
{
    internal class BooleanConverter : IValueConverter
    {
        
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string True = "启用", False = "禁用";
            if(parameter is string)
            {
                string[] vs = ((string)parameter).Split("|");
                if (vs.Length>=1)
                {
                    True = vs[0]; 
                }
                if(vs.Length>=2)
                {
                    False = vs[1];
                }
            }
            if (value == null) return False;
            if (!(value is Boolean)) return False;
            return ((bool)value) == true ? True : False;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
