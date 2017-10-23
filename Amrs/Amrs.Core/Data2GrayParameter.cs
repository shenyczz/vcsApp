/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: A
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
    public struct Data2GrayParameter
    {
        public int iChannel;					// 通道号
        public int iEnhanceType;				// 增强方式(0-直线，1-指数，2-对数 ...)
        public ushort wMinValue;					// 最小值
        public ushort wMaxValue;					// 最大值
        public ushort wThreshold_min;			// 去掉一些小值后的极小值(通道数据下阈值)
        public ushort wThreshold_max;			// 去掉一些大值后的极大值(通道数据上阈值)
        public Byte cMinGray;					// 最小灰度
        public Byte cMaxGray;					// 最大灰度
        public Byte cLessDownGray;				// 弱最小灰度
        public Byte cExceedUpGray;				// 超最大灰度
        public Boolean bAntiPhase;				// 是否反相
    }
}

/*
 	// 通道数据转换为灰度数据时使用的参数
	struct Data2GrayParam
	{
		int iChannel;					// 通道号
		int iEnhanceType;				// 增强方式(0-直线，1-指数，2-对数 ...)
		WORD wMinValue;					// 最小值
		WORD wMaxValue;					// 最大值
		WORD wThreshold_min;			// 去掉一些小值后的极小值(通道数据下阈值)
		WORD wThreshold_max;			// 去掉一些大值后的极大值(通道数据上阈值)
		BYTE cMinGray;					// 最小灰度
		BYTE cMaxGray;					// 最大灰度
		BYTE cLessDownGray;				// 弱最小灰度
		BYTE cExceedUpGray;				// 超最大灰度
		BOOL bAntiPhase;				// 是否反相
	};
*/