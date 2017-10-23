/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HDWLevel - 干热风等级
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

namespace Hdwas
{
    /// <summary>
    /// HDWLevel - 干热风等级
    /// </summary>
    public enum HDWLevel
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 轻
        /// </summary>
        Slight = 60,

        /// <summary>
        /// 中
        /// </summary>
        Medium = 80,

        /// <summary>
        /// 重
        /// </summary>
        Severe = 90,
    }
}
