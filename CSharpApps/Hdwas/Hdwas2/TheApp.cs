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
using System.Windows;
using CSharpKit.Win32;
using CSharpKit.Vision.Mapping;

namespace Hdwas
{
    public static class TheApp
    {

        #region --Path

        public static String StartupPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public static String MapPath
        {
            get { return System.IO.Path.Combine(StartupPath, "Map"); }
        }

        public static String ConfigPath
        {
            get { return System.IO.Path.Combine(StartupPath, "Config"); }
        }

        public static String DataPath
        {
            get { return System.IO.Path.Combine(StartupPath, "Data"); }
        }

        public static String ImagePath
        {
            get { return System.IO.Path.Combine(StartupPath, "Image"); }
        }

        public static String DataPathHdwDay
        {
            get { return System.IO.Path.Combine(DataPath, "HdwDay"); }
        }

        public static String DataPathHdwProcess
        {
            get { return System.IO.Path.Combine(DataPath, "HdwProcess"); }
        }

        #endregion

        public static MainWindow MainWindow
        {
            get { return (MainWindow)App.Current.MainWindow; }
        }

        /// <summary>
        /// 作物发育期集合
        /// </summary>
        public static CropGrowthPeriodCollection CropGrowthPeriods { get; set; }

        [DllImport("KSuiteCore100u.dll")]
        public static extern IntPtr CaptureRect(IntPtr handle, ref RECT rect);

        public static int Level { get; set; }
    }
}
