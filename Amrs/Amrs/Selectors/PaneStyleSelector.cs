/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: PaneStyleSelector
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
using System.Windows;
using System.Windows.Controls;
using Amrs.ViewModels;

namespace Amrs
{
    public class PaneStyleSelector : StyleSelector
    {
        public Style MapStyle { get; set; }
        public Style ToolStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is MapViewModel)
                return MapStyle;

            if (item is LayerViewModel)
                return ToolStyle;

            if (item is PropertyGridViewModel)
                return ToolStyle;

            return base.SelectStyle(item, container);
        }
    }
}
