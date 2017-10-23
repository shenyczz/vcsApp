/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HdwProcessStation - 站点干热风过程
 *  Version: 
 * Function: 
 * 
 *   Author: 申永辰.郑州 (shenyczz@163.com)
 * DateTime: 2010 - 
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpKit;

namespace Hdwas
{
    /// <summary>
    /// HDWProcessStation - 站点干热风过程
    /// </summary>
    public class HDWProcessStation
    {
        public HDWProcessStation()
        {
            this.HdwProcesses = new List<HDWProcess>();
        }

        /// <summary>
        /// 站点ID
        /// </summary>
        public Station Station { get; set; }

        /// <summary>
        /// 干热风过程集合（每个站点可能有多个干热风过程）
        /// </summary>
        public List<HDWProcess> HdwProcesses { get; private set; }

        /// <summary>
        /// 标记：
        /// 0-日期不连续（新建干热风日）
        /// 1-日期连续
        /// </summary>
        public int Flag { get; set; }



        private String _HdwProcessInfo;
        /// <summary>
        /// 干热风过程信息(由 HdwProcesses 转换而来)
        /// </summary>
        public String HdwProcessInfo
        {
            get
            {
                //-----------------------------------------
                if (_HdwProcessInfo == null)
                {
                    StringBuilder sb = new StringBuilder();
                    if (HdwProcesses.Count == 0)
                    {
                        sb.Append("XX"); // 无表示没有干热风日
                        sb.Append(";");
                    }
                    else
                    {
                        foreach (HDWProcess hdwp in HdwProcesses)
                        {
                            sb.Append
                            (
                                hdwp.Grade + "过程"
                                + hdwp.StartDate.ToString("[yyyyMMdd")
                                + "-"
                                + hdwp.EndDate.ToString("yyyyMMdd]")
                            );
                            sb.Append(";");
                        }
                    }

                    _HdwProcessInfo = sb.ToString();
                }
                //-----------------------------------------
                return _HdwProcessInfo;
            }
            set { _HdwProcessInfo = value; }
        }

        public String HdwProcessGrade
        {
            get
            {
                string hdwProcessGrade = "";

                StringBuilder sb = new StringBuilder();
                if (HdwProcesses.Count == 0)
                {
                    sb.Append("XX"); // 无表示没有干热风日
                    sb.Append(";");
                }
                else
                {
                    foreach (HDWProcess hdwp in HdwProcesses)
                    {
                        sb.Append(hdwp.Grade);
                        sb.Append(";");
                    }
                }

                string hdwProcessString = sb.ToString();

                String maxGrade = "无";

                String[] sa = hdwProcessString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                int len = sa.Length;

                List<string> ss = sa.ToList();
                List<string> sl3 = ss.FindAll(s => s == "重");
                List<string> sl2 = ss.FindAll(s => s == "中");
                List<string> sl1 = ss.FindAll(s => s == "轻");
                List<string> sl0 = ss.FindAll(s => s == "无");

                if (sl3.Count > 0)
                    maxGrade = "重";
                else if (sl2.Count > 0)
                    maxGrade = "中";
                else if (sl1.Count > 0)
                    maxGrade = "轻";
                else
                    maxGrade = "无";

                hdwProcessGrade = maxGrade;

                return hdwProcessGrade;
            }
        }

        public HDWLevel HdwProcessLevel
        {
            get
            {
                HDWLevel hdwLevel = HDWLevel.None;

                switch(HdwProcessGrade)
                {
                    case "无":
                        hdwLevel = HDWLevel.None;
                        break;
                    case "轻":
                        hdwLevel = HDWLevel.Slight;
                        break;
                    case "中":
                        hdwLevel = HDWLevel.Medium;
                        break;
                    case "重":
                        hdwLevel = HDWLevel.Severe;
                        break;
                }

                return hdwLevel;
            }
        }

        // 次数统计 - 无
        public int PNum0
        {
            get { return GetNum("无过程"); }
        }
        public int PNum1
        {
            get { return GetNum("轻过程"); }
        }
        public int PNum2
        {
            get { return GetNum("中过程"); }
        }
        public int PNum3
        {
            get { return GetNum("重过程"); }
        }

        private int GetNum(string s)
        {
            string info = HdwProcessInfo;
            string[] ia = info.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int nn = ia.Where(n => n.StartsWith(s)).Select(n => n).ToList().Count;
            return nn;
        }
    }
}
