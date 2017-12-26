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
            this.RegionID = "";

            this.Year = 0;
            this.Fa = -999;
            this.Fae = -1;
            this.FaMax = 0;
            this.FaMin = 0;
        }

        #region Private Fields

        private List<Double> _Fcs = new List<Double>(); //日适宜度集合
        private List<Double> _Fgs = new List<Double>(); //各发育阶段适宜度集合
        private List<Double> _Gws = new List<Double>(); //各发育阶段权重(配置文件)

        #endregion

        /// <summary>
        /// 区域综合ID
        /// </summary>
        public string RegionID { get; set; }

        /// <summary>
        /// 替代站点
        /// </summary>
        public string ReplaceSite { get; set; }

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

        /// <summary>
        /// 评估年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 整个发育期适宜度
        /// </summary>
        public Double Fa { get; set; }
        public int Fae { get; set; }

        /// <summary>
        /// 所有站点整个育期适宜度最大值
        /// </summary>
        public Double FaMax { get; set; }
        /// <summary>
        /// 所有站点整个发育期适宜度最小值
        /// </summary>
        public Double FaMin { get; set; }

        /// <summary>
        /// 评价结果
        /// -1：未知
        ///  0：不适宜
        ///  1：次适宜
        ///  2：适宜
        /// </summary>
        public string FaeStrting
        {
            get
            {
                string s = "";
                switch(Fae)
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

        public override string ToString()
        {
            return string.Format("{0} {1} {2}:{3} {4:f3}", Id, Name, Fae, FaeStrting, Fa);
        }




        public void DoIt()
        {
            //评估年份
            if (Year == 0)
                return;

            //气象要素
            if (MeteoElements.Count <= 0)
                return;

#if DEBUG
            if (this.Id == "57189")
            {//鹤壁
                int yyy = 0;
                yyy++;
            }
#endif

            // 计算各个发育阶段适宜度
            int year = this.Year;
            int grwpCount = CropGrwps.Count;    //整个发育期发育阶段数量
            TimeSpan timeSpan = TimeSpan.FromDays(1);

            //TODO:test - 整个发育期起始、终止日期(条件：区域、作物、品种)
            DateTime dt0 = CropWorkspaces[0].CropGrwp.GrwpBeg(year);
            DateTime dt1 = CropWorkspaces[grwpCount-1].CropGrwp.GrwpEnd(year);
            //====

            _Fgs.Clear();
            _Gws.Clear();
            for (int i = 0; i < grwpCount; i++)
            {
                CropGrwp cropGrwp = CropGrwps[i];                   //发育阶段
                CropWorkspace cropWorkspace = CropWorkspaces[i];    //工作空间

                DateTime dtBeg = cropGrwp.GrwpBeg(year);    //发育阶段开始日期
                DateTime dtEnd = cropGrwp.GrwpEnd(year);    //发育阶段结束日期

                // 计算该发育阶段的日适宜度Fc，并保存到_Fcs
                _Fcs.Clear();
                for (DateTime dt = dtBeg; dt <= dtEnd; dt += timeSpan)
                {
                    MeteoElement me = MeteoElements.Find(p => p.DateTime == dt);
                    if (me == null)
                        continue;

                    double ft = App.Workspace.BusinessMethords.Ft(me.T, cropWorkspace.T0, cropWorkspace.Tl, cropWorkspace.Th); //温度适宜度
                    double fr = App.Workspace.BusinessMethords.Fr(this, dt);                                          //降水适宜度
                    double fs = App.Workspace.BusinessMethords.Fs(me.Hos, double.Parse(cropWorkspace.ThrSunlight));              //日照适宜度

                    //日适宜度
                    double fc = App.Workspace.BusinessMethords.Fc(ft, fr, fs);

                    _Fcs.Add(fc);
                }

                // 计算单个发育阶段适宜度Fg,并保存到_Fgs
                if (_Fcs.Count > 0)
                {
                    double fg = App.Workspace.BusinessMethords.Fg(_Fcs);
                    _Fgs.Add(fg);
                }

                // 取得单个发育阶段权重
                double gw = cropWorkspace.Weight;
                _Gws.Add(gw);

            }

            // 计算整个发育期适宜度
            if (_Fgs.Count > 0)
            {
                this.Fa = App.Workspace.BusinessMethords.Fa(_Fgs, _Gws);           //整个发育期适宜度
                this.Fae = App.Workspace.BusinessMethords.Fae(Fa, FaMax, FaMin);   //整个发育期适宜度评估值
            }

            return;
        }




        public double FcAll { get; set; }
        public double FcAvg { get; set; }

        public void DoIt2(DateTime dtBeg, DateTime dtEnd)
        {
            List<Double> fcs = new List<Double>();     //日适宜度集合
            TimeSpan timeSpan = TimeSpan.FromDays(1);

            for (DateTime dt = dtBeg; dt <= dtEnd; dt += timeSpan)
            {
                MeteoElement me = MeteoElements.Find(p => p.DateTime == dt);
                if (me == null)
                    continue;

                //TODO:查找日期对应的发育期所在的工作空间
                CropWorkspace cropWorkspace = this.FindCropWorkspace(CropWorkspaces, dt);
                if (cropWorkspace == null)
                    continue;

                double ft = App.Workspace.BusinessMethords.Ft(me.T, cropWorkspace.T0, cropWorkspace.Tl, cropWorkspace.Th); //温度适宜度
                double fr = App.Workspace.BusinessMethords.Fr(this, dt);                                          //降水适宜度
                double fs = App.Workspace.BusinessMethords.Fs(me.Hos, double.Parse(cropWorkspace.ThrSunlight));              //日照适宜度

                //日适宜度
                double fc = App.Workspace.BusinessMethords.Fc(ft, fr, fs);

                fcs.Add(fc);
            }

            int j = 0;
            double fca = 0;
            fcs.ForEach(p =>
            {
                //if (p > 0.01)
                {
                    fca += p;
                    j++;
                }
            });
            double fcavg = fca / j;

            this.FcAll = fca;
            this.FcAvg = fcavg;


            return;
            //
            //End
            //
        }

        private CropWorkspace FindCropWorkspace(List<CropWorkspace> cropWorkspaces, DateTime dt)
        {
            return cropWorkspaces[0];
        }



    }


}
