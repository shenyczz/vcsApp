/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HDWProcess - 干热风过程
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

namespace Hdwas
{
    /// <summary>
    /// HDWProcess - 干热风过程 
    /// </summary>
    public class HDWProcess
    {
        public HDWProcess()
        {
            GradeString = "";
        }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 干热风日等级串(轻;中;重)
        /// 由指定时段内站点的干热风等级的集合组成，以";"分隔
        /// </summary>
        public String GradeString { get; set; }

        /// <summary>
        /// 等级(无/轻/中/重)
        /// 由 GradeString 分析得来
        /// </summary>
        public String Grade
        {
            get
            {
                String grade = "无";

                String[] sa = GradeString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                int len = sa.Length;

                List<string> ss = sa.ToList();
                List<string> sl3 = ss.FindAll(s => s == "重");
                List<string> sl2 = ss.FindAll(s => s == "中");
                List<string> sl1 = ss.FindAll(s => s == "轻");
                List<string> sl0 = ss.FindAll(s => s == "无");

                if (sl0.Count == len)
                {
                    grade = "无";
                }
                else
                {
                    if (len == 1)
                    {
                        // 重: 无
                        // 中: 无
                        // 轻: 1重
                        if (sl3.Count > 0)
                            grade = "轻";
                    }

                    if (len == 2)
                    {
                        // 重: 2重
                        // 中: 2中/1中1重
                        // 轻: 其余
                        if (sl3.Count == 2) grade = "重";
                        else if (sl2.Count == 2 || sl3.Count == 1 && sl2.Count == 1) grade = "中";
                        else grade = "轻";
                    }

                    if (len == 3)
                    {
                        // 重: >=2重
                        // 中: 其余
                        // 轻: 3轻
                        if (sl3.Count >= 2) grade = "重";
                        else if (sl1.Count == 3) grade = "轻";
                        else grade = "中";
                    }

                    if (len == 4)
                    {
                        // 重: >=2重/>=3中
                        // 中: 其余
                        // 轻: 无
                        if (sl3.Count >= 2 || sl2.Count >= 3) grade = "重";
                        else grade = "中";
                    }

                    if (len >= 5)
                    {
                        // 重: 其余
                        // 中: 5轻
                        // 轻: 无
                        if (sl2.Count >= 5) grade = "中";
                        else grade = "重";
                    }
                }

                return grade;
            }
        }


        public override string ToString()
        {
            return String.Format("{0}", this.Grade);
        }
    }
}
