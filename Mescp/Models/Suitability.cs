using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Mescp.Models
{
    public class Suitability
    {
        public Suitability()
        {
            Value = -1;
        }
        public Suitability(double value)
        {
            Value = value;
        }

        public double Value { get; set; }

        Brush _VBrush;
        public Brush VBrush
        {
            get
            {
                int flag = (int)(Value + 0.1);
                switch (flag)
                {
                    case 0:
                        _VBrush = Brushes.Red;
                        break;
                    case 1:
                        _VBrush = new SolidColorBrush(Color.FromRgb(255, 192, 128));
                        break;
                    case 2:
                        _VBrush = Brushes.LightGreen;
                        break;
                    default:
                        _VBrush = null;
                        break;
                }
                return _VBrush;
            }
            set
            {
                _VBrush = value;
            }
        }

        public override string ToString()
        {
            int flag = (int)(Value + 0.1);
            string s = "";
            switch (flag)
            {
                case 0:
                    s = "不适宜";
                    break;
                case 1:
                    s = "次适宜";
                    break;
                case 2:
                    s = "适宜";
                    break;
                default:
                    s = "未知";
                    break;
            }
            return s;
        }

    }
}
