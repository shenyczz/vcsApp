/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: SatelliteType - 卫星类型
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
    public enum SatelliteType : ushort
    {
        None = 0,
        Avhrr_Fy1d = 114,
        Avhrr_Noaa12 = 12,
        Avhrr_Noaa14 = 14,
        Avhrr_Noaa15 = 7,
        Avhrr_Noaa16 = 3,
        Avhrr_Noaa18 = 17,
        Avhrr_Noaa19 = 19,

        Modis_Aqua = 1002,
        Modis_Terra = 1001,

        Mersi_Fy3a = 1100,
        Virr_Fy3a = 123,
    }
}
