/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: LDHeader
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
using System.Runtime.InteropServices;
using Amrs.Core;

namespace Amrs.Core
{
    /// <summary>
    /// Size = 76
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class LDHeader
    {
        /// <summary>
        /// 文件标识
        /// 局地文件为"LA" 
        /// </summary>
        public UInt16 FileID;

        /// <summary>
        /// 卫星标识
        /// </summary>
        public SatelliteType SatelliteType;

        /// <summary>
        /// 轨道号
        /// </summary>
        public UInt16 OrbitNo;

        /// <summary>
        /// 升降轨标记
        /// 1: 升轨
        /// 0: 降轨
        /// </summary>
        public UInt16 UpOrDown;

        /// <summary>
        /// 时间
        /// </summary>
        public UInt16 Year;
        public UInt16 Month;
        public UInt16 Day;
        public UInt16 Hour;
        public UInt16 Minute;

        /// <summary>
        /// 白天黑夜标识
        /// 0: 白天
        /// 1: 黑夜
        /// </summary>
        public UInt16 DayOrNight;

        /// <summary>
        /// 通道数
        /// LDF - 白天: 5, 黑夜: 3
        /// LD2 - ?
        /// </summary>
        public UInt16 ChannelNums;

        /// <summary>
        /// 投影方式
        /// 0: 不投影
        /// 1: 等角投影
        /// 2: 麦卡托投影
        /// 3: 兰布托投影
        /// 4: 极射赤面投影
        /// 5: 艾尔伯斯投影
        /// </summary>
        public UInt16 ProjectType;

        /// <summary>
        /// 宽度(列数)
        /// </summary>
        public UInt16 Width;

        /// <summary>
        /// 高度(行数)
        /// </summary>
        public UInt16 Height;

        /// <summary>
        /// 经度分辨率 -- 等角投影
        /// x分辨率    -- 麦卡托、兰布托、极射赤面投影 
        /// </summary>
        public Single LonResolution;

        /// <summary>
        /// 纬度分辨率 -- 等角投影
        /// y分辨率    -- 麦卡托、兰布托、极射赤面投影
        /// </summary>
        public Single LatResolution;

        /// <summary>
        /// 标准纬度1
        /// -- 麦卡托、兰布托、极射赤面投影有效
        /// </summary>
        public Single StandardLat1;

        /// <summary>
        /// 标准纬度2
        /// -- 兰布托投影有效
        /// </summary>
        public Single StandardLat2;

        /// <summary>
        /// 地球半径
        /// -- 麦卡托、兰布托、极射赤面投影有效
        /// </summary>
        public Single RadiusOfEarth;

        public Single LatMin;			// 最小纬度
        public Single LatMax;			// 最大纬度
        public Single LonMin;			// 最小经度
        public Single LonMax;			// 最大经度

        public Single StandardLon;		// 圆锥投影中心经线 (LAMBERT,POLAR,ALBERS)

        public Single CenterLon;			// 图象中心点的经度
        public Single CenterLat;			// 图象中心点的纬度


        public DateTime DateTime
        {
            get
            {
                return new DateTime(Year, Month, Day, Hour, Minute, 0);
            }
        }

        /// <summary>
        /// 传感器类型
        /// </summary>
        public SensorType SensorType
        {
            get
            {
                SensorType sensorType = SensorType.None;

                switch (this.SatelliteType)
                {
                    case SatelliteType.Avhrr_Fy1d:
                    case SatelliteType.Avhrr_Noaa12:
                    case SatelliteType.Avhrr_Noaa14:
                    case SatelliteType.Avhrr_Noaa15:
                    case SatelliteType.Avhrr_Noaa16:
                    case SatelliteType.Avhrr_Noaa18:
                    case SatelliteType.Avhrr_Noaa19:
                        sensorType = SensorType.Avhrr;
                        break;

                    case SatelliteType.Modis_Aqua:
                    case SatelliteType.Modis_Terra:
                        sensorType = SensorType.Modis;
                        break;

                    case SatelliteType.Mersi_Fy3a:
                        sensorType = SensorType.Mersi;
                        break;

                    case SatelliteType.Virr_Fy3a:
                        sensorType = SensorType.Virr;
                        break;
                }

                return sensorType;
            }
        }

        public Boolean IsDay
        {
            get { return DayOrNight == 0; }
        }
    }

    /// <summary>
    /// Size = 128
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class LD2Header : LDHeader
    {
        public LD2Header()
        {
            ChannelIndex = new Byte[40];
        }

        /// <summary>
        /// 通道索引
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public Byte[] ChannelIndex;

        public UInt16 wRate;				// 已合成天数
        public UInt16 Version;			// 版本号
        public UInt16 wBytes;				// 本结构字节数
        public UInt16 wResvere;			// 保留
        public UInt32 SkipLength;		// 局地文件头记录的后面填充字段长度



    }

    /// <summary>
    /// Size = 4096
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class LD3Header : LDHeader
    {
        public LD3Header()
        {
            ChannelIndex = new UInt16[1000];
            wAncillaryChIndex = new UInt16[100];
            ucFlag3 = new Byte[1807];
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
        public UInt16[] ChannelIndex;				// 通道索引

        public UInt16 wChannelNumsOfAncillary;	// 辅助通道数 

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public UInt16[] wAncillaryChIndex;		// 辅助通道索引

        public UInt16 wComposeDays;			// 已合成天数
        public UInt16 Version;			// 版本号

        public Byte NDVIOffset;			// ??

        public Byte CloudMask;
        public UInt32 SkipLength;			// 局地文件头记录的后面填充字段长度

        public Byte ucNoaach3Flag;			// 0-3A  1-3B

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1807)]
        public Byte[] ucFlag3;
    }

}
