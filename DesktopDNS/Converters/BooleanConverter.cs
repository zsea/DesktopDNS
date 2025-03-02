using Avalonia.Data.Converters;
using DesktopDNS.ViewModels;
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
            string True = "Enabled", False = "Disabled";
            if(parameter is string)
            {
                string p = ((string)parameter);
                switch (p) {
                    case "Status":
                        {
                            True = I18n.i18n.Boolean_Status_True;
                            False = I18n.i18n.Boolean_Status_False;
                            break;
                        }
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
