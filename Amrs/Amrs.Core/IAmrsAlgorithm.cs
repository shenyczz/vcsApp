/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: IAmrsAlgorithm - 遥感算法
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
    public interface IAmrsAlgorithm
    {
        /// <summary>
        /// 归一化植被指数
        /// </summary>
        /// <returns></returns>
        Double Ndvi();

        Double Lst(int iMethod);


        /*
		///植被指数
		//-------------------------------------------------
		// 
		virtual double Ndvi() { return AGMRS_INVALID_DATA; }
		// 比值植被指数
		virtual double Rvi() { return AGMRS_INVALID_DATA; }
		// 差值植被指数
		virtual double Dvi() { return AGMRS_INVALID_DATA; }
		// 土壤调整植被指数
		virtual double Savi() { return AGMRS_INVALID_DATA; }
		// 增强型植被指数
		virtual double Evi() { return AGMRS_INVALID_DATA; }

		///干旱指数
		//-------------------------------------------------
		// 地表含水量指数
		virtual double Swci() { return AGMRS_INVALID_DATA; }

		///其他指数
		//-------------------------------------------------
		// 归一化积雪指数
		virtual double Ndsi() { return AGMRS_INVALID_DATA; }

		/// 地表温度
		//-------------------------------------------------
		virtual double Lst(int iMethod = 0) { return AGMRS_INVALID_DATA; }
         */
    }
}
