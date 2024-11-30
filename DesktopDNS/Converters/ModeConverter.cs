using Avalonia.Data.Converters;
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
                case "FULL":return "全等";
                case "REGEX":return "正则";
                case "WILDCARD":return "模式匹配";
            }
            return "";

        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
