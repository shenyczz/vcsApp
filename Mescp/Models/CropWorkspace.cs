using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mescp.Models
{
    /// <summary>
    /// 作物工作空间
    /// </summary>
    public class CropWorkspace : CropModelBase
    {
        public CropWorkspace()
        {
            //this.CropGrwp = new CropGrwp();
        }

        /// <summary>
        /// 区域组合ID
        /// </summary>
        public String RegionID { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        public String RgnID { get; set; }
        public String CropID { get; set; }
        public String CultivarID { get; set; }

        // 发育期
        //public String GrwpID { get; set; }
        //public String GrwpName { get; set; }
        //public String GrwpSpan { get; set; }

        public CropGrwp CropGrwp { get; set; }

        #region 降水阈值

        public String ThrRain { get; set; }

        #endregion

        #region 日照阈值

        public String ThrSunlight { get; set; }

        #endregion

        #region 温度阈值

        public String ThrTemperature { get; set; }


        /// <summary>
        /// 温度阈值数组
        /// [0,1,2]=>[T0,Tl,Th]
        /// </summary>
        private Double[] _TA = new double[3];
        protected Double[] TA
        {
            set { _TA = value; }
            get
            {
                try
                {
                    string[] sa = ThrTemperature.Split(new char[] { '-' });
                    if (sa != null)
                    {
                        _TA[0] = Double.Parse(sa[0]);
                        _TA[1] = Double.Parse(sa[1]);
                        _TA[2] = Double.Parse(sa[2]);
                    }
                }
                catch (Exception)
                {
                    throw new Exception("error in array TA's get! IN class CropWorkspace!");
                }

                return _TA;
            }
        }

        public Double T0
        {
            set { TA[0] = value; }
            get { return TA[0]; }
        }
        public Double Tl
        {
            set { TA[1] = value; }
            get { return TA[1]; }
        }
        public Double Th
        {
            set { TA[2] = value; }
            get { return TA[2]; }
        }

        #endregion

        #region 生育期权重(计算整个生育阶段适宜度使用)

        public Double Weight { get; set; }

        #endregion

    }

    //public class CropWorkspaceCollection : List<CropWorkspace>
    //{
    //}

}
