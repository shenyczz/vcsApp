/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: TheApp
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CSharpKit;
using CSharpKit.Vision.Mapping;
using CSharpKit.Win32;
using CSharpKit.Windows.Anchoring;

namespace Hdwas
{
    public class TheApp : App
    {
        static TheApp()
        {
            MapFunctions = new MapFunctions();

            _MrConfiguration = new MrConfiguration();
        }

        TheApp() { }

        public static readonly String MapLayerId_County = "HA_MAP_COUNTY_20140821_151703";

        private static MrConfiguration _MrConfiguration;
        public static MrConfiguration MrConfiguration
        {
            get { return _MrConfiguration; }
        }

        /// <summary>
        /// 地图
        /// </summary>
        public static IMap Map { get; set; }

        /// <summary>
        /// 主窗口
        /// </summary>
        public static MainWindow MainWindow
        {
            get { return (MainWindow)App.Current.MainWindow; }
        }

        /// <summary>
        /// 地图函数
        /// </summary>
        public static MapFunctions MapFunctions { get; private set; }


        /// <summary>
        /// 停靠管理器
        /// </summary>
        public static DockingManager DockingManager { get; set; }

        #region --Path
        
        /// <summary>
        /// 启动路径
        /// </summary>
        public static String StartupPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        /// <summary>
        /// 配置路径
        /// </summary>
        public static String ConfigPath
        {
            get { return System.IO.Path.Combine(StartupPath, "Config"); }
        }

        /// <summary>
        /// 数据路径
        /// </summary>
        static String DataPath
        {
            get { return System.IO.Path.Combine(StartupPath, "Data"); }
        }

        public static String RsImagePath
        {
            get { return System.IO.Path.Combine(StartupPath, "Images"); }
        }

        /// <summary>
        /// 数据路径 - 干热风日
        /// </summary>
        public static String DataPath_Hdwd
        {
            get { return System.IO.Path.Combine(DataPath, "Hdwd"); }
        }

        /// <summary>
        /// 数据路径 - 干热风过程
        /// </summary>
        public static String DataPath_Hdwp
        {
            get { return System.IO.Path.Combine(DataPath, "Hdwp"); }
        }

        /// <summary>
        /// 数据路径 - 干热风评估
        /// </summary>
        public static String DataPath_Hdwa
        {
            get { return System.IO.Path.Combine(DataPath, "Hdwa"); }
        }

        /// <summary>
        /// 图像路径
        /// </summary>
        public static String ImagePath
        {
            get { return System.IO.Path.Combine(StartupPath, "Images"); }
        }

        #endregion

        /// <summary>
        /// 站点信息集合 - 干热风日使用
        /// </summary>
        public static List<StationInfo> StationInfos { get; set; }

        public static List<StationInfo> StationInfos_Day { get; set; }

        /// <summary>
        /// 站点信息集合 - 干热风评估使用
        /// </summary>
        public static List<StationInfo> StationInfos_Assess { get; set; }


        /// <summary>
        /// 干热风过程集合
        /// </summary>
        public static List<HDWProcessStation> HdwProcessStations { get; set; }

        /// <summary>
        /// 作物发育期集合
        /// </summary>
        public static CropGrowthPeriodCollection CropGrowthPeriods { get; set; }


        /// <summary>
        /// 区域代码
        /// </summary>
        public static String AreaCode { get; set; }

        public static String AreaName
        {
            get
            {
                string areaName = "";
                switch(AreaCode)
                {
                    case "":
                        break;

                    case "410000":
                        areaName = "河南省";
                        break;

                    case "411000":
                        areaName = "许昌市";
                        break;

                    case "411700":
                        areaName = "驻马店";
                        break;
                }

                return areaName;
            }
        }



        [DllImport("KSuiteCore100u.dll")]
        public static extern IntPtr CaptureRect(IntPtr handle, ref RECT rect);

    }
}

