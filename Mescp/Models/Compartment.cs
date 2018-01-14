using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mescp.Models
{
    /// <summary>
    /// 区划
    /// </summary>
    public class Compartment
    {
        public string SFID { get; set; }
        public string CultivarID { get; set; }
        public string PeriodID { get; set; }

        public double A_MIN { get; set; }
        public double A_MAX { get; set; }
        public double B_MIN { get; set; }
        public double B_MAX { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public double Wgt { get; set; }
        public string Flag { get; set; }

        public bool IsUsed
        {
            get { return Flag.Trim().ToUpper() == "YES"; }
        }


        /// <summary>
        /// 取得区划值
        /// </summary>
        /// <param name="x">要素值</param>
        /// <returns></returns>
        public double GetPLV(double x)
        {
            double plv = 0;

            if (IsUsed)
            {
                if (x >= A_MIN && x <= A_MAX)
                {
                    plv = 1;
                }
                else if (x < B_MIN || x > B_MAX)
                {
                    plv = 0;
                }
                else
                {
                    if (x >= B_MIN && x < A_MIN)
                    {
                        plv = (x - B_MIN) / (A_MIN - B_MIN);
                    }

                    if (x > A_MAX && x <= B_MAX)
                    {
                        plv = (B_MAX - x) / (B_MAX - A_MAX);
                    }
                }
            }

            return plv;
        }


        public override string ToString()
        {
            return string.Format("{0}", SFID);
        }

    }
}
