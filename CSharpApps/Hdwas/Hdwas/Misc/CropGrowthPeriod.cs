/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: CropGrowthPeriod - 作物发育期 (Anthesis)
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
using System.Windows;
using System.Xml.Linq;
using CSharpKit.Extentions;

namespace Hdwas
{
    /// <summary>
    /// CropGrowthPeriod - 作物发育期
    /// </summary>
    public class CropGrowthPeriod : DependencyObject
    {
        static CropGrowthPeriod()
        {
            FrameworkPropertyMetadata metadata01 = new FrameworkPropertyMetadata(String.Empty);
            GroutingPeriod01Property = DependencyProperty.Register("GroutingPeriod01", typeof(String), typeof(CropGrowthPeriod), metadata01);

            FrameworkPropertyMetadata metadata02 = new FrameworkPropertyMetadata(String.Empty);
            GroutingPeriod02Property = DependencyProperty.Register("GroutingPeriod02", typeof(String), typeof(CropGrowthPeriod), metadata02);

            FrameworkPropertyMetadata metadata03 = new FrameworkPropertyMetadata(String.Empty);
            GroutingPeriod03Property = DependencyProperty.Register("GroutingPeriod03", typeof(String), typeof(CropGrowthPeriod), metadata03);
        }

        public CropGrowthPeriod()
        {
            AreaCode = "";
            StationId = "";
            StationName = "";
            Region = 0;

            FloweringPeriod = "";
            GroutingPeriod01 = "";
            GroutingPeriod02 = "";
            GroutingPeriod03 = "";
        }

        public CropGrowthPeriod(XElement xe)
            : this()
        {
            this.FromXElement(xe);
        }


        /// <summary>
        /// 区域代码
        /// </summary>
        public String AreaCode { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public String StationId { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public String StationName { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public Region Region { get; set; }

        #region FloweringPeriod

        //int _FPMonth;
        //int _FPDay;
        public int FPMonth
        {
            get
            {
                int fpm = 5;
                string[] fpa = FloweringPeriod.Split(new string[] { "月", "日" }, StringSplitOptions.RemoveEmptyEntries);
                if (fpa.Length == 2)
                {
                    fpm = fpa[0].ToInt();
                }
                return fpm;
            }
        }
        public int FPDay
        {
            get
            {
                int fpd = 20;
                string[] fpa = FloweringPeriod.Split(new string[] { "月", "日" }, StringSplitOptions.RemoveEmptyEntries);
                if (fpa.Length == 2)
                {
                    fpd = fpa[1].ToInt();
                }
                return fpd;
            }
        }

        private String _FloweringPeriod;
        /// <summary>
        /// 开花期
        /// </summary>
        public String FloweringPeriod
        {
            get { return _FloweringPeriod; }
            set
            {
                _FloweringPeriod = value;
                if (!string.IsNullOrEmpty(value))
                {
                    // 改变灌浆日期
                    Days01 = _Days01;
                    Days02 = _Days02;
                    Days03 = _Days03;
                }
            }
        }

        #endregion

        #region GroutingPeriod01

        public static readonly DependencyProperty GroutingPeriod01Property;
       
        /// <summary>
        /// 灌浆前期
        /// </summary>
        public String GroutingPeriod01
        {
            get { return (String)GetValue(GroutingPeriod01Property); }
            private set
            {
                SetValue(GroutingPeriod01Property, value);
                Days02 = _Days02;
            }
        }

        private int _Days01;
        public int Days01
        {
            get { return _Days01; }
            set
            {
                try
                {
                    _Days01 = value;
                    if (value > 0)
                    {
                        DateTime dt = new DateTime(2000, FPMonth, FPDay);
                        TimeSpan ts = new TimeSpan(value, 0, 0, 0);
                        dt += ts;

                        this.GroutingPeriod01 = string.Format("{0}月{1}日", dt.Month, dt.Day);
                    }
                }
                catch { }
            }
        }

        #endregion

        #region GroutingPeriod02

        public static readonly DependencyProperty GroutingPeriod02Property;

        /// <summary>
        /// 灌浆中期
        /// </summary>
        public String GroutingPeriod02
        {
            get { return (String)GetValue(GroutingPeriod02Property); }
            private set
            {
                SetValue(GroutingPeriod02Property, value);
                Days03 = _Days03;
            }
        }
        private int _Days02;
        public int Days02
        {
            get { return _Days02; }
            set
            {
                try
                {
                    _Days02 = value;
                    if (value > 0)
                    {
                        int m = 0, d = 0;
                        Get_Month_Day(GroutingPeriod01, ref m, ref d);
                        //DateTime dt = new DateTime(2000, FPMonth, FPDay);
                        DateTime dt = new DateTime(2000, m, d);
                        TimeSpan ts = new TimeSpan(value, 0, 0, 0);
                        dt += ts;

                        this.GroutingPeriod02 = string.Format("{0}月{1}日", dt.Month, dt.Day);
                    }
                }
                catch { }
            }
        }

        #endregion

        #region GroutingPeriod03

        public static readonly DependencyProperty GroutingPeriod03Property;

        /// <summary>
        /// 灌浆后期
        /// </summary>
        public String GroutingPeriod03
        {
            get { return (String)GetValue(GroutingPeriod03Property); }
            private set { SetValue(GroutingPeriod03Property, value); }
        }
        private int _Days03;
        public int Days03
        {
            get { return _Days03; }
            set
            {
                try
                {
                    _Days03 = value;
                    if (value > 0)
                    {
                        int m = 0, d = 0;
                        Get_Month_Day(GroutingPeriod02, ref m, ref d);
                        //DateTime dt = new DateTime(2000, FPMonth, FPDay);
                        DateTime dt = new DateTime(2000, m, d);
                        TimeSpan ts = new TimeSpan(value, 0, 0, 0);
                        dt += ts;

                        this.GroutingPeriod03 = string.Format("{0}月{1}日", dt.Month, dt.Day);
                    }
                }
                catch { }
            }
        }

        #endregion


        public XElement ToXElement()
        {
            XElement xElement = new XElement(this.GetType().Name);
            {
                // 属性
                xElement.Add(new XAttribute("AreaCode", AreaCode.ToString()));
                xElement.Add(new XAttribute("StationId", StationId.ToString()));
                xElement.Add(new XAttribute("StationName", StationName.ToString()));
                xElement.Add(new XAttribute("Region", Region.ToString()));

                // 元素
                xElement.Add(new XElement("FloweringPeriod", FloweringPeriod.ToString()));

                xElement.Add(new XElement("GroutingPeriod01", GroutingPeriod01.ToString()));
                xElement.Add(new XElement("GroutingPeriod02", GroutingPeriod02.ToString()));
                xElement.Add(new XElement("GroutingPeriod03", GroutingPeriod03.ToString()));

                xElement.Add(new XElement("Days01", Days01.ToString()));
                xElement.Add(new XElement("Days02", Days02.ToString()));
                xElement.Add(new XElement("Days03", Days03.ToString()));

            }
            return xElement;
        }

        public void FromXElement(XElement xElement)
        {
            if (xElement.Name != this.GetType().Name)
                return;

            // 属性
            foreach (XAttribute attr in xElement.Attributes())
            {
                switch (attr.Name.ToString())
                {
                    case "AreaCode":
                        this.AreaCode = attr.Value;
                        break;

                    case "StationId":
                        this.StationId = attr.Value;
                        break;

                    case "StationName":
                        this.StationName = attr.Value;
                        break;

                    case "Region":
                        this.Region = (Region)Enum.Parse(typeof(Region), attr.Value);
                        break;
                }
            }

            // 元素
            foreach (XElement xe in xElement.Nodes())
            {
                switch (xe.Name.ToString())
                {
                    case "FloweringPeriod":
                        this.FloweringPeriod = xe.Value;
                        break;

                    case "GroutingPeriod01":
                        this.GroutingPeriod01 = xe.Value;
                        break;

                    case "GroutingPeriod02":
                        this.GroutingPeriod02 = xe.Value;
                        break;

                    case "GroutingPeriod03":
                        this.GroutingPeriod03 = xe.Value;
                        break;

                    case "Days01":
                        this.Days01 = xe.Value.ToInt();
                        break;

                    case "Days02":
                        this.Days02 = xe.Value.ToInt();
                        break;

                    case "Days03":
                        this.Days03 = xe.Value.ToInt();
                        break;
                }

            }

        }



        private void Get_Month_Day(string mds, ref int month, ref int day)
        {
            month = 5;
            day = 20;
            string[] fpa = mds.Split(new string[] { "月", "日" }, StringSplitOptions.RemoveEmptyEntries);
            if (fpa.Length == 2)
            {
                month = fpa[0].ToInt();
                day = fpa[1].ToInt();
            }
        }


        /// <summary>
        /// 灌浆期标志 0=不是灌浆前 1=灌浆前期 2=灌浆中期 3=灌浆后期
        /// </summary>
        /// <param name="cropGrowthPeriod"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetFlag(CropGrowthPeriod cropGrowthPeriod, DateTime dateTime)
        {
            int flag = 0;

            try
            {
                string s1 = cropGrowthPeriod.GroutingPeriod01;
                string[] sa1 = s1.Split(new char[] { '月', '日' }, StringSplitOptions.RemoveEmptyEntries);
                int m1 = int.Parse(sa1[0]);
                int d1 = int.Parse(sa1[1]);

                string s2 = cropGrowthPeriod.GroutingPeriod01;
                string[] sa2 = s2.Split(new char[] { '月', '日' }, StringSplitOptions.RemoveEmptyEntries);
                int m2 = int.Parse(sa2[0]);
                int d2 = int.Parse(sa2[1]);

                string s3 = cropGrowthPeriod.GroutingPeriod01;
                string[] sa3 = s3.Split(new char[] { '月', '日' }, StringSplitOptions.RemoveEmptyEntries);
                int m3 = int.Parse(sa3[0]);
                int d3 = int.Parse(sa3[1]);

                int mm = dateTime.Month;
                int dd = dateTime.Day;

                if (mm >= m1 && dd >= d1)
                {
                    flag = 1;
                }

                if (mm >= m2 && dd >= d2)
                {
                    flag = 2;
                }

                if (mm >= m3 && dd >= d3)
                {
                    flag = 3;
                }
            }
            catch
            {
                flag = 0;
            }

            return flag;
        }

        /// <summary>
        /// 根据起止日期确定灌浆期 0=不是灌浆前 1=灌浆前期 2=灌浆中期 3=灌浆后期
        /// </summary>
        /// <param name="cropGrowthPeriod"></param>
        /// <param name="dateTim0"></param>
        /// <param name="dateTime1"></param>
        /// <returns></returns>
        [Obsolete("",true)]
        public static int GetFlag(CropGrowthPeriod cropGrowthPeriod, DateTime dateTim0, DateTime dateTime1)
        {
            return 0;
        }


    }
}
