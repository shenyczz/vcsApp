/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HDWStationInfo - 干热风站点信息
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
    [Obsolete("暂时不用",true)]
    public class HDWStationInfo : StationInfo
    {
        public HDWStationInfo()
        {
            this.Hdw = new HDW();
        }

        public HDW Hdw { get; private set; }
    }
}
