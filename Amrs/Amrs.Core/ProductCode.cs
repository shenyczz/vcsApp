/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: ProductCode - 产品代码
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
    public abstract class ProductCode
    {
        #region Vix - 植被指数(6000 ~ 6099)

        // 6000 ~ 6099
        // 植被指数 - VIX (Vegetation Index)
        public static readonly UInt32 Vix = 6000;           // 植被指数 - VIX
        public static readonly UInt32 Vix_Ndvi = 6001;      // 归一化植被指数	- NDVI
        public static readonly UInt32 Vix_Dvi = 6002;       // 差值植被指数		- DVI
        public static readonly UInt32 Vix_Rvi = 6003;       // 比值植被指数		- RVI
        public static readonly UInt32 Vix_Savi = 6004;      // 土壤调整植被指数 - SAVI
        public static readonly UInt32 Vix_Evi = 6005;       // 增强型植被指数	- EVI

        public static readonly UInt32 VixDif = 6050;        // 植被指数变化
        public static readonly UInt32 VixDif_Ndvi = 6051;   // 植被指数变化		- NDVI
        public static readonly UInt32 VixDif_Dvi = 6052;    // 植被指数变化		- DVI
        public static readonly UInt32 VixDif_Rvi = 6053;    // 植被指数变化		- RVI
        public static readonly UInt32 VixDif_Savi = 6054;   // 植被指数变化		- SAVI
        public static readonly UInt32 VixDif_Evi = 6055;    // 植被指数变化		- EVI
        public static readonly UInt32 Vix_End = 6099;

        #endregion

        #region Dix - 干旱指数(6100 ~ 6199)

        // 6100 ~ 6199
        // 干旱指数 - DIX (Drought Index)
        public static readonly UInt32 Dix = 6100;
        public static readonly UInt32 Dix_Ati = 6101;       // 热惯量指数			- ATI
        public static readonly UInt32 Dix_Vswi = 6102;      // 植被供水指数			- VSWI
        public static readonly UInt32 Dix_Swci = 6103;      // 土壤含水量指数		- SWCI
        public static readonly UInt32 Dix_Tvdi = 6104;      // 温度植被干旱指数		- TVDI
        public static readonly UInt32 Dix_Csmi = 6105;      // 耕作层土壤湿度指数	- CSMI
        public static readonly UInt32 Dix_Pdi = 6106;       // 垂直干旱指数			- PDI
        public static readonly UInt32 Dix_Mpdi = 6107;      // 修正垂直干旱指数		- MPDI

        public static readonly UInt32 DixDif = 6150;
        public static readonly UInt32 DixDif_Ati = 6151;    // 干旱指数变化			- ATI
        public static readonly UInt32 DixDif_Vswi = 6152;   // 干旱指数变化			- VSWI
        public static readonly UInt32 DixDif_Swci = 6153;   // 干旱指数变化			- SWCI
        public static readonly UInt32 DixDif_Tvdi = 6154;   // 干旱指数变化			- TVDI
        public static readonly UInt32 DixDif_Csmi = 6155;   // 干旱指数变化			- CSMI
        public static readonly UInt32 DixDif_Pdi = 6156;    // 干旱指数变化			- PDI
        public static readonly UInt32 DixDif_Mpdi = 6157;   // 干旱指数变化			- MPDI
        public static readonly UInt32 Dix_End = 6199;

        #endregion

        #region Smc - 土壤墒情(6200 ~ 6299)

        // 6200 ~ 6299
        // 土壤墒情 - SMC (Soil Moisture Content)
        public static readonly UInt32 Smc = 6200;
        public static readonly UInt32 Smc_Ah = 6201;    // 绝对湿度 - Absolute Humidity
        public static readonly UInt32 Smc_Rh = 6202;    // 相对湿度 - Relative Humidity
        public static readonly UInt32 Smc_End = 6299;

        #endregion

        #region Irg - 灌溉(6300 ~ 6399)

        // 6300 ~ 6399
        // 灌溉 - IRG (Irrigation)
        public static readonly UInt32 Irg = 6300;
        public static readonly UInt32 Irg_Depth = 6301;     // 灌溉深度
        public static readonly UInt32 Irg_Area = 6302;      // 灌溉面积
        public static readonly UInt32 Irg_Amount = 6303;    // 灌溉量
        public static readonly UInt32 Irg_End = 6399;

        #endregion

        #region Snow - 积雪(6400 ~ 6499)

        // 6400 ~ 6499
        // 积雪 - SNOW
        public static readonly UInt32 Snow = 6400;
        public static readonly UInt32 Snow_Ndsi = 6401;     // 积雪指数
        public static readonly UInt32 Snow_Depth = 6402;    // 积雪深度
        public static readonly UInt32 Snow_Cover = 6403;    // 积雪覆盖
        public static readonly UInt32 Snow_End = 6499;

        #endregion

        #region Grade - 等级(6800 ~ 6899)

        // 6800 ~ 6899
        // 等级 - Grade
        public static readonly UInt32 Grade = 6800;
        public static readonly UInt32 Grade_Mq = 6801;
        public static readonly UInt32 Grade_Dry = 6811;
        public static readonly UInt32 Grade_Land_Use = 6898;
        public static readonly UInt32 Grade_End = 6899;

        #endregion

        #region Misce - 杂项(6900 ~ 6999)

        // 6900 ~ 6999
        // 杂项 - Misce

        public static readonly UInt32 Misce = 6900;             // 
        public static readonly UInt32 Misce_Cloud = 6901;       // 云
        public static readonly UInt32 Misce_Fog = 6902;         // 雾
        public static readonly UInt32 Misce_Fire = 6903;        // 火
        public static readonly UInt32 Misce_Snow = 6904;        // 雪
        public static readonly UInt32 Misce_Ice = 6905;         // 冰
        public static readonly UInt32 Misce_Land = 6906;        // 陆
        public static readonly UInt32 Misce_Water = 6907;       // 水

        public static readonly UInt32 Misce_Lst = 6947;         // 地表温度
        public static readonly UInt32 Misce_Abe = 6948;         // 全反照率
        public static readonly UInt32 Misce_End = 6999;

        #endregion
    }
}
