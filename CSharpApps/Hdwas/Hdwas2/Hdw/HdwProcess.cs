/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HdwProcess
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
using System.Xml.Linq;

namespace Hdwas
{
    public class HdwProcess : ICloneable, IComparable<HdwProcess>
    {
        public HdwProcess()
        {
            GradeString = "";
        }

        protected HdwProcess(HdwProcess rhs)
        {
            this.StartDate = rhs.StartDate;
            this.EndDate = rhs.EndDate;
            this.GradeString = rhs.GradeString;
        }


        private static readonly String defaultName = typeof(HdwProcess).Name;


        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 干热风日等级串(比如 "轻;中;重")
        /// 由指定时段内站点的干热风等级的集合组成，以";"分隔
        /// </summary>
        public String GradeString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HdwGrade HdwGrade
        {
            get
            {
                //return GetGrade_L3();
                return GetGrade_L2();
            }
        }

        private HdwGrade GetGrade_L2()
        {
            HdwGrade grade = HdwGrade.None;

            String[] sa = GradeString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int days = sa.Length;   // 干热风日天数

            List<string> ss = sa.ToList();
            List<string> sl3 = ss.FindAll(s => s == "重");
            //List<string> sl2 = ss.FindAll(s => s == "中");
            List<string> sl1 = ss.FindAll(s => s == "轻");
            List<string> sl0 = ss.FindAll(s => s == "无");

            if (days == sl0.Count)
            {// 全"无"
                grade = HdwGrade.None;
            }
            else
            {
                // 1个干热风日
                if (days == 1)
                {
                    // 轻过程: 1重
                    // 无过程: 其余
                    if (sl3.Count > 0)
                        grade = HdwGrade.Slight;
                    else
                        grade = HdwGrade.None;
                }
                // 2个干热风日
                if (days == 2)
                {
                    // 重过程: 2重
                    // 轻过程: 1重+1轻
                    // 无过程: 其余
                    if (sl3.Count == 2)
                        grade = HdwGrade.Severe;
                    else if (sl3.Count == 1 || sl1.Count == 1)
                        grade = HdwGrade.Slight;
                    else
                        grade = HdwGrade.None;
                }

                // 3个干热风日
                if (days >= 3)
                {
                    // 重过程: >=2重;1重+>=2轻
                    // 轻过程: 3轻
                    // 无过程: 其余
                    if (sl3.Count >= 2 || sl3.Count == 1 && sl1.Count >= 2)
                        grade = HdwGrade.Severe;
                    else if (sl3.Count == 1 && sl1.Count >= 1)
                        grade = HdwGrade.Slight;
                    else
                        grade = HdwGrade.None;
                }
            }

            return grade;
        }

        private HdwGrade GetGrade_L3()
        {
            HdwGrade grade = HdwGrade.None;

            String[] sa = GradeString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int days = sa.Length;   // 干热风日天数

            List<string> ss = sa.ToList();
            List<string> sl3 = ss.FindAll(s => s == "重");
            List<string> sl2 = ss.FindAll(s => s == "中");
            List<string> sl1 = ss.FindAll(s => s == "轻");
            List<string> sl0 = ss.FindAll(s => s == "无");

            if (sl0.Count == days)
            {// 全"无"
                grade = HdwGrade.None;
            }
            else
            {

                // 1个干热风日
                if (days == 1)
                {
                    // 轻过程: 1重
                    // 无过程: 其余
                    if (sl3.Count > 0)
                        grade = HdwGrade.Slight;
                    else
                        grade = HdwGrade.None;
                }

                // 2个干热风日
                if (days == 2)
                {
                    // 重过程: 2重
                    // 中过程: 2中/1中1重
                    // 轻过程: 其余
                    if (sl3.Count == 2)
                        grade = HdwGrade.Severe;
                    else if (sl2.Count == 2 || sl3.Count == 1 && sl2.Count == 1)
                        grade = HdwGrade.Medium;
                    else
                        grade = HdwGrade.Slight;
                }

                // 3个干热风日
                if (days == 3)
                {
                    // 重过程: >=2重
                    // 中过程: 其余
                    // 轻过程: 3轻
                    if (sl3.Count >= 2)
                        grade = HdwGrade.Severe;
                    else if (sl1.Count == 3)
                        grade = HdwGrade.Slight;
                    else
                        grade = HdwGrade.Medium;
                }

                // 4个干热风日
                if (days == 4)
                {
                    // 重过程: >=2重/>=3中
                    // 中过程: 其余
                    if (sl3.Count >= 2 || sl2.Count >= 3)
                        grade = HdwGrade.Severe;
                    else
                        grade = HdwGrade.Medium;
                }

                // 5个干热风日以上
                if (days >= 5)
                {
                    // 重过程: 其余
                    // 中过程: 5轻
                    if (sl2.Count >= 5)
                        grade = HdwGrade.Medium;
                    else
                        grade = HdwGrade.Severe;
                }

            }

            return grade;
        }

        /// <summary>
        /// 干热风过程等级描述
        /// </summary>
        public String GradeInfo
        {
            get
            {
                string gradeInfo = "无";

                switch(HdwGrade)
                {
                    case HdwGrade.None:
                        gradeInfo = "无";
                        break;
                    case HdwGrade.Slight:
                        gradeInfo = "轻";
                        break;
                    case HdwGrade.Medium:
                        gradeInfo = "中";
                        break;
                    case HdwGrade.Severe:
                        gradeInfo = "重";
                        break;
                }

                return String.Format("{0}过程[{1}-{2}]",
                    gradeInfo,
                    StartDate.ToString("yyyyMMdd"),
                    EndDate.ToString("yyyyMMdd")
                    );
            }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1})", this.HdwGrade, this.GradeString);
        }


        #region ICloneable 成员

        public Object Clone()
        {
            return new HdwProcess(this);
        }

        #endregion


        #region IComparable<HdwProcess> 成员

        public int CompareTo(HdwProcess other)
        {
            if (this.HdwGrade > other.HdwGrade)
                return 1;
            if (this.HdwGrade < other.HdwGrade)
                return -1;
            return 0;
        }

        #endregion


        #region XElement

        public XElement ToXElement()
        {
            return HdwProcess.ToXElement(this);
        }

        public static XElement ToXElement(HdwProcess hdwProcess)
        {
            XElement xElement = new XElement(defaultName);

            try
            {
                // 属性
                //xElement.Add(new XAttribute("AreaCode", AreaCode.ToString()));

                // 元素
                xElement.Add(new XElement("StartDate", hdwProcess.StartDate.ToString("yyyy-MM-dd")));
                xElement.Add(new XElement("EndDate", hdwProcess.EndDate.ToString("yyyy-MM-dd")));
                xElement.Add(new XElement("GradeString", hdwProcess.GradeString.ToString()));
            }
            catch (Exception)
            {
                xElement = null;
            }

            return xElement;
        }

        public static HdwProcess FromXElement(XElement xElement)
        {
            HdwProcess instance = new HdwProcess();

            try
            {
                if (xElement.Name != defaultName)
                    return null;

                // 属性
                foreach (XAttribute attr in xElement.Attributes())
                {
                    switch (attr.Name.ToString())
                    {
                        default:
                            break;
                    }
                }

                // 元素
                foreach (XElement xe in xElement.Nodes())
                {
                    switch (xe.Name.ToString())
                    {
                        case "StartDate":
                            instance.StartDate = DateTime.Parse(xe.Value);
                            break;

                        case "EndDate":
                            instance.EndDate = DateTime.Parse(xe.Value);
                            break;

                        case "GradeString":
                            instance.GradeString = xe.Value;
                            break;

                        default:
                            break;
                    }
                }
            }
            catch
            {
                instance = null;
            }

            return instance;
        }

        #endregion
    }
}
