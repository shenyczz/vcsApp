using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Hdwas
{
    /// <summary>
    /// HDWStrengthConverter - 干热风强度转换器
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    public class HDWStrengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string retString = "无";

            double v = (double)value;

            string stemp = "";
            if (v < 20) { stemp = "无"; }
            else if (v < 60) { stemp = "轻"; }
            else if (v < 80) { stemp = "中"; }
            else { stemp = "重"; }

            retString = string.Format("{0}", stemp);

            return retString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }

    }
}
