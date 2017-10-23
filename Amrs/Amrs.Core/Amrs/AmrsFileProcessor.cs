/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: AmrsFileProcessor
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CSharpKit.Data;

namespace Amrs.Core
{
    public class AmrsFileProcessor : MeteoFileProcessor
    {
        public AmrsFileProcessor(Object owner)
            : base(owner) { }

        /// <summary>
        /// 遥感数据灰度
        /// </summary>
        protected Byte[,] RsDataGray { get; set; }

        /// <summary>
        /// 数据转灰度参数
        /// </summary>
        protected Data2GrayParameter[] Data2GrayParameter;

        /// <summary>
        /// 遥感图像
        /// </summary>
        public Image RsImage { get; set; }

    }
}
