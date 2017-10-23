/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: AmrsFileDataInfo
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CSharpKit.Data;

namespace Amrs.Core
{
    public class AmrsFileDataInfo : MeteoFileDataInfo
    {
        public AmrsFileDataInfo()
        {
            ChannelCode = new Int16[1000];
        }
        /// <summary>
        /// 文件头
        /// </summary>
        public Object FileHeader { get; set; }

        public int ChannelCount;

        public Int16[] ChannelCode;		// 通道代码

        public UInt16 Version;

        /// <summary>
        /// 遥感数据
        /// </summary>
        public UInt16[, ,] RsData { get; set; }
    }
}
