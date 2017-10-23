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
 * DateTime: 2010 - 2015
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amrs.ViewModels;

namespace Amrs
{
    public static class TheApp
    {
        static TheApp()
        {
            Workspace = new Workspace();
        }

        public static Workspace Workspace { get; private set; }

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
            get { return System.IO.Path.Combine(StartupPath, "Images"); }
        }

        #endregion
    }
}
