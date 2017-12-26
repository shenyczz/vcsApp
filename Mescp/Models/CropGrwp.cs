using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mescp.Models
{
    /// <summary>
    /// 作物发育期
    /// </summary>
    public class CropGrwp : CropModelBase
    {
        public CropGrwp()
        {
        }

        private string _GrwpSpan = "";
        private int[] _SpanMonth = new int[2];
        private int[] _SpanDay = new int[2];

        public String CropID { get; set; }

        //

        /// <summary>
        /// 发育期ID
        /// </summary>
        public String GrwpID { get; set; }
        /// <summary>
        /// 发育期代码
        /// </summary>
        public String GrwpCode { get; set; }
        /// <summary>
        /// 发育期名称
        /// </summary>
        public String GrwpName { get; set; }
        /// <summary>
        /// 发育期跨度
        /// 格式：mm.dd-mm.dd
        /// </summary>
        public String GrwpSpan
        {
            get
            {
                return _GrwpSpan;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                _GrwpSpan = value;

                try
                {
                    string[] mmdda = _GrwpSpan.Split(new char[] { '-' });
                    string[] mmdd0 = mmdda[0].Split(new char[] { '.' });
                    string[] mmdd1 = mmdda[1].Split(new char[] { '.' });

                    _SpanMonth[0] = int.Parse(mmdd0[0]);
                    _SpanMonth[1] = int.Parse(mmdd1[0]);

                    _SpanDay[0] = int.Parse(mmdd0[1]);
                    _SpanDay[1] = int.Parse(mmdd1[1]);

                }
                catch (Exception)
                {
                    throw new Exception("error in GrwpSpan set Property.");
                }

            }
        }

        //


        /// <summary>
        /// 年跨度
        /// </summary>
        public int SpanYear
        {
            get
            {
                return SpanMonth[0] <= SpanMonth[1] ? 0 : 1;
            }
        }

        /// <summary>
        /// 月
        /// </summary>
        public int[] SpanMonth
        {
            get
            {
                return _SpanMonth;
            }
        }

        /// <summary>
        /// 日
        /// </summary>
        public int[] SpanDay
        {
            get
            {
                return _SpanDay;
            }
        }


        /// <summary>
        /// 开始发育期
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DateTime GrwpBeg(int year)
        {
            int yyyy = year;
            int mm = SpanMonth[0];
            int dd = SpanDay[0];

            DateTime dt = new DateTime(yyyy, mm, dd);
            return dt;
        }
        /// <summary>
        /// 结束发育期
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DateTime GrwpEnd(int year)
        {
            int yyyy = year+ SpanYear;
            int mm = SpanMonth[1];
            int dd = SpanDay[1];

            DateTime dt = new DateTime(yyyy, mm, dd);
            return dt;
        }


        public override string ToString()
        {
            return String.Format("{0} {1}：[{2}]", GrwpID, GrwpName, GrwpSpan);
        }
    }

    //public class CropGrwpCollection : List<CropGrwp>
    //{
    //}

}
