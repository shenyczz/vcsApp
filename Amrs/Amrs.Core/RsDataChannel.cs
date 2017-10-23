/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: RsDataChannel - 遥感数据通道
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
    public class RsDataChannel
    {
        public RsDataChannel()
        {
            Index = -1;
            Name = "";
        }

        /// <summary>
        /// 索引(0 - 37)
        /// </summary>
        public int Index;

        /// <summary>
        /// 名称
        /// </summary>
        public String Name;

        /// <summary>
        /// 是否存在
        /// </summary>
        public Boolean IsExist
        {
            get { return Index >= 0; }
        }
    }
}
