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
    internal class ModeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return "";
            if (value is not string) return "";
            string? mode=value as string;
            if (mode == null) return "";
            switch(mode)
            {
                case "FULL":return I18n.i18n.Settings_Domain_Mode_FULL;
                case "REGEX":return I18n.i18n.Settings_Domain_Mode_REGEX;
                case "WILDCARD":return I18n.i18n.Settings_Domain_Mode_WILDCARD;
            }
            return "";

        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
