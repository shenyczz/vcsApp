/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HDW - 干热风
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
    /// HDW - 干热风
    /// </summary>
    public class HDW
    {
        public HDW()
            : this(0) { }

        public HDW(Double value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 干热风等级阈值上限
        /// </summary>
        public static readonly Double LevelNone = 20;   // 无
        public static readonly Double LevelSlight = 60; // 轻
        public static readonly Double LevelMedium = 80; // 中
        public static readonly Double LevelSevere = 90; // 重

        /// <summary>
        /// 干热风值
        /// </summary>
        public Double Value { get; set; }

        /// <summary>
        /// 干热风等级
        /// </summary>
        public HDWLevel HdwLevel
        {
            get
            {
                HDWLevel hdwLevel = HDWLevel.None;
                //-----------------------------------------
                Double v = this.Value;
                if ((v < HDW.LevelNone)) { hdwLevel = HDWLevel.None; }
                else if ((v < HDW.LevelSlight)) { hdwLevel = HDWLevel.Slight; }
                else if ((v < HDW.LevelMedium)) { hdwLevel = HDWLevel.Medium; }
                else { hdwLevel = HDWLevel.Severe; }
                //-----------------------------------------
                return hdwLevel;
            }
        }

        public override string ToString()
        {
            string hdwString = "无";

            switch (this.HdwLevel)
            {
                case HDWLevel.None:
                    hdwString = "无";
                    break;
                case HDWLevel.Slight:
                    hdwString = "轻";
                    break;
                case HDWLevel.Medium:
                    hdwString = "中";
                    break;
                case HDWLevel.Severe:
                    hdwString = "重";
                    break;
            }

            return hdwString;
        }
    }
}
