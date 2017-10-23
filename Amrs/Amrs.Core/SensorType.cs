/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: SensorType - (卫星携带的)传感器类型
 *  Version: 
 * Function: 
 * 
 *   Author: 申永辰.郑州 (shenyczz@163.com)
 * DateTime: 2010 - 2015
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amrs.Core
{
    /// <summary>
    /// SensorType - 传感器类型 AGMRS
    /// </summary>
    public enum SensorType
    {
        None = 0,
        Avhrr,          // Noaa 卫星搭载
        Modis,          // Terra & Aqua 卫星搭载
        Mersi,          // Fy3a 卫星搭载
        Virr,           // Fy3a 卫星搭载
    }
}
