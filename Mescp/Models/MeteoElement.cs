using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mescp.Models
{
    /// <summary>
    /// MeteoElement - 气象要素
    /// </summary>
    public class MeteoElement
    {
        public MeteoElement()
        {
        }

        public String StationId { get; set; }
        public String StationName { get; set; }

        public DateTime DateTime{ get; set; }

        public int ObvDate { get; set; }
        public int ObvTime { get; set; }

        public double P { get; set; }
        public double P0 { get; set; }
        public double Pmax { get; set; }
        public double PmaxTime { get; set; }

        public double T { get; set; }
        public double Tmax { get; set; }
        public double Tmin { get; set; }

        public double U { get; set; }
        public double E { get; set; }
        public double R { get; set; }
        public double S { get; set; }



        public override string ToString()
        {
            return string.Format("{0} {1} {2}", StationId, StationName, DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }

    }


    public class MeteoElementCollection : List<MeteoElement>
    {

    }
}
