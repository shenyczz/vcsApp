/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: Disic
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
using CSharpKit.Data;

namespace Hdwas
{
    /// <summary>
    /// HdwDay - 干热风日
    /// </summary>
    public class HdwDay
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 站点信息
        /// </summary>
        public List<StationInfo> StationInfos { get; set; }
    }
}
