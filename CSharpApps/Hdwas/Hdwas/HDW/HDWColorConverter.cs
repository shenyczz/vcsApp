using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Hdwas
{
    /// <summary>
    /// HDWColorConverter - 干热风颜色转换器
    /// </summary>
    [ValueConversion(typeof(Double), typeof(Color))]
    public class HDWColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color clr = Colors.Black;

            double v = (double)value;

            if (v < 20) { clr = Colors.Blue; }
            else if (v < 60) { clr = Colors.Green; }
            else if (v < 80) { clr = Colors.Red; }
            else { clr = Colors.DarkRed; }

            return clr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }

    }
}
