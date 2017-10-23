using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpKit;

namespace Mescp.Models
{
    public class XStationCollection : List<XStation> { }

    public class XStation : Station
    {
        public XStation()
        {
            this.Region = "";
            this.Year = 0;
            this.Fa = -1;
            this.Fz = -999;
        }

        #region Private Fields

        private List<Double> _Fcs = new List<Double>(); //日适宜度集合
        private List<Double> _Fgs = new List<Double>(); //各发育阶段适宜度集合
        private List<Double> _Gws = new List<Double>(); //各发育阶段权重

        #endregion

        /// <summary>
        /// 发育期
        /// </summary>
        public List<CropGrwp> CropGrwps { get; set; }
        /// <summary>
        /// 工作空间
        /// </summary>
        public List<CropWorkspace> CropWorkspaces { get; set; }
        /// <summary>
        /// 气象要素集合
        /// </summary>
        public List<MeteoElement> MeteoElements { get; set; }

        public int Year { get; set; }

        public string Region { get; set; }

        /// <summary>
        /// 整个发育期适宜度
        /// </summary>
        public Double Fz { get; set; }


        /// <summary>
        /// 评价结果
        /// -1：未知
        ///  0：不适宜
        ///  1：次适宜
        ///  2：适宜
        /// </summary>
        public int Fa { get; set; }

        public string FaStrting
        {
            get
            {
                string s = "";
                switch(Fa)
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


        public void DoIt()
        {
            if (Year == 0)
                return;

            if (MeteoElements.Count <= 0)
                return;

            //============================================
            if (this.Id == "57189")
            {
                int yyy = 0;
                yyy++;
            }
            //============================================

            // 计算各个发育阶段适宜度
            int year = this.Year;
            int grwpCount = CropGrwps.Count;
            TimeSpan timeSpan = TimeSpan.FromDays(1);

            //发育阶段适宜度的最大、最小值
            double fgMax = double.MinValue;
            double fgMin = double.MaxValue;

            _Fgs.Clear();
            _Gws.Clear();
            for (int i = 0; i < grwpCount; i++)
            {
                CropGrwp cropGrwp = CropGrwps[i];                   //发育阶段
                CropWorkspace cropWorkspace = CropWorkspaces[i];    //工作空间

                DateTime dtBeg = cropGrwp.GrwpBeg(year);    //发育阶段开始日期
                DateTime dtEnd = cropGrwp.GrwpEnd(year);    //发育阶段结束日期
                //TimeSpan ts = dtEnd - dtBeg;
                //int nDays = (int)ts.TotalDays;

                // 计算该发育阶段的日适宜度Fc，并保存到_Fcs
                _Fcs.Clear();
                for (DateTime dt = dtBeg; dt <= dtEnd; dt += timeSpan)
                {
                    MeteoElement me = MeteoElements.Find(p => p.DateTime == dt);
                    if (me == null)
                        continue;

                    double ft = App.Workspace.AppMethod.Ft(me.T, cropWorkspace.T0, cropWorkspace.Tl, cropWorkspace.Th); //温度适宜度
                    double fr = App.Workspace.AppMethod.Fr();                                                           //降水适宜度
                    double fs = App.Workspace.AppMethod.Fs(me.S, double.Parse(cropWorkspace.ThrSunlight));              //日照适宜度

                    //日适宜度
                    double fc = App.Workspace.AppMethod.Fc(ft, fr, fs);
                    //if (double.IsNaN(fc))
                    //{
                    //    int xxx = 0;
                    //}
                    _Fcs.Add(fc);
                }

                // 计算单个发育阶段适宜度Fg,并保存到_Fgs
                if (_Fcs.Count > 0)
                {
                    double fg = App.Workspace.AppMethod.Fg(_Fcs);
                    _Fgs.Add(fg);

                    //顺手获取发育阶段适宜度的最大、最小值
                    fgMax = Math.Max(fgMax, fg);
                    fgMin = Math.Min(fgMin, fg);
                }

                // 取得单个发育阶段权重
                double gw = cropWorkspace.Weight;
                _Gws.Add(gw);

            }

            // 计算整个发育期适宜度
            //if (_Fgs.Count == _Fws.Count)
            if (_Fgs.Count > 0)
            {
                this.Fz = App.Workspace.AppMethod.Fz(_Fgs, _Gws);
                this.Fa = App.Workspace.AppMethod.Fa(this.Fz, fgMax, fgMin);
            }

            return;
        }





        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3:f3}", Id, Name, FaStrting, Fz);
        }
    }


}
