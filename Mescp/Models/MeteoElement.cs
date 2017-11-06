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

        /// <summary>
        /// 温度
        /// </summary>
        public double T { get; set; }
        /// <summary>
        /// 最高温度
        /// </summary>
        public double Tmax { get; set; }
        /// <summary>
        /// 最低温度
        /// </summary>
        public double Tmin { get; set; }
        /// <summary>
        /// 水汽压
        /// </summary>
        public double E { get; set; }
        /// </summary>
        /// 风速
        /// <summary>
        public double Ws { get; set; }
        /// <summary>
        /// 日照时数
        /// </summary>
        public double Hos { get; set; }
        /// <summary>
        /// 降水
        /// </summary>
        public double R { get; set; }

        /// <summary>
        /// 本站气压
        /// </summary>
        //public double P { get; set; }
        //public double P0 { get; set; }
        //public double Pmax { get; set; }


        public override string ToString()
        {
            return string.Format("{0} {1} {2}", StationId, StationName, DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }

    }


    public class MeteoElementCollection : List<MeteoElement>
    {

    }
}
