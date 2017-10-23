/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HDW
 *  Version: 
 * Function: 
 * 
 *   Author: 申永辰.郑州 (shenyczz@163.com)
 * DateTime: 2010 - 2014
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hdwas.Core;

namespace Hdwas
{
    public class Hdw
    {
        public Hdw()
            : this(0.0) { }

        public Hdw(Double value)
        {
            this.Value = value;
        }

        public Hdw(HdwGrade grade)
        {
            this.HdwGrade = grade;
        }

        /// <summary>
        /// 用于2级
        /// </summary>
        /// <param name="t"></param>
        /// <param name="u"></param>
        /// <param name="w"></param>
        public Hdw(Double t, Double u, Double w)
        {
            HdwGrade hdwGrade = HdwGrade.None;
            //---------------------------------------------
            // 2级业务标准(轻、重)
            if (t >= 32 && u <= 30 && w >= 3)
                hdwGrade = HdwGrade.Slight;
            if (t >= 35 && u <= 25 && w >= 3)
                hdwGrade = HdwGrade.Severe;
            //---------------------------------------------
            this.HdwGrade = hdwGrade;
        }

        /// <summary>
        /// 用于3级
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="me"></param>
        public Hdw(MeteoElement meteoElement, CropGrowthPeriod cgp)
        {
            HdwGrade hdwGrade = HdwGrade.None;
            //---------------------------------------------
            // 3级业务标准(轻、中、重)
            // 取得气象要素
            double t = meteoElement.T14;
            double u = meteoElement.U14;
            double w = meteoElement.W14;

            // 灌浆期标志 0=不是灌浆前 1=灌浆前期 2=灌浆中期 3=灌浆后期
            int flag = cgp.GetFlag(meteoElement.DateTime);
            switch (flag)
            {
                case 1: //灌浆前期
                    {
                        // 轻
                        if (t >= 31.5 && u < 30 && w >= 2.5)
                            hdwGrade = HdwGrade.Slight;

                        // 中
                        if (t >= 33.1 && u < 30 && w >= 2.5)
                            hdwGrade = HdwGrade.Medium;

                        // 重
                        if (t >= 34.0 && u < 30 && w >= 3.0)
                            hdwGrade = HdwGrade.Severe;
                    }
                    break;

                case 2: //灌浆中期
                    {
                        // 轻
                        if (t >= 32.0 && u < 30 && w >= 2.5)
                            hdwGrade = HdwGrade.Slight;

                        // 中
                        if (t >= 32.0 && u < 26 && w >= 3.0)
                            hdwGrade = HdwGrade.Medium;

                        // 重
                        if (t >= 35.0 && u < 23 && w >= 3.5)
                            hdwGrade = HdwGrade.Severe;
                    }
                    break;

                case 3: //灌浆后期
                    {
                        // 轻
                        //14时气温32.4～33.9℃，相对湿度＜31%，14时风速≥2.5m/s 
                        if (t >= 32.4 && t < 33.9 && u < 31 && w >= 2.5)
                            hdwGrade = HdwGrade.Slight;

                        // 中
                        //14时气温34.0～36.9℃，相对湿度＜28%，14时风速≥3.0m/s 
                        if (t >= 34.0 && t < 36.9 && u < 28 && w >= 3.0)
                            hdwGrade = HdwGrade.Medium;

                        // 重
                        if (t >= 37.0 && u < 24 && w >= 4.0)
                            hdwGrade = HdwGrade.Severe;
                    }
                    break;

                default:
                    hdwGrade = HdwGrade.None;
                    break;
            }
            //---------------------------------------------
            this.HdwGrade = hdwGrade;
        }



        private static readonly Double GradeNone = 0;
        private static readonly Double GradeSlight = 20;
        private static readonly Double GradeMedium = 60;
        private static readonly Double GradeSevere = 80;

        private Double _Value;
        public Double Value
        {
            get { return _Value; }
            set
            {
                _Value = value;

                if (_Value <= Hdw.GradeNone)
                    _HdwGrade = HdwGrade.None;
                else if (_Value <= Hdw.GradeSlight)
                    _HdwGrade = HdwGrade.Slight;
                else if (_Value <= Hdw.GradeMedium)
                    _HdwGrade = HdwGrade.Medium;
                else
                    _HdwGrade = HdwGrade.Severe;
            }
        }

        private HdwGrade _HdwGrade;
        public HdwGrade HdwGrade
        {
            get { return _HdwGrade; }
            set
            {
                _HdwGrade = value;
                switch(_HdwGrade)
                {
                    case HdwGrade.None:
                        _Value = Hdw.GradeNone;
                        break;

                    case HdwGrade.Slight:
                        _Value = Hdw.GradeSlight;
                        break;

                    case HdwGrade.Medium:
                        _Value = Hdw.GradeMedium;
                        break;

                    case HdwGrade.Severe:
                        _Value = Hdw.GradeSevere;
                        break;

                    default:
                        _Value = Hdw.GradeNone;
                        break;
                }
            }
        }

        public override String ToString()
        {
            string hdwString = "无";

            switch (this.HdwGrade)
            {
                case HdwGrade.None:
                    hdwString = "无";
                    break;
                case HdwGrade.Slight:
                    hdwString = "轻";
                    break;
                case HdwGrade.Medium:
                    hdwString = "中";
                    break;
                case HdwGrade.Severe:
                    hdwString = "重";
                    break;
            }

            return hdwString;
        }
    }
}
