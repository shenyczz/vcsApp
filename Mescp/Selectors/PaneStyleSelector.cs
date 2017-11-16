/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     RgnName: PaneStyleSelector
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Mescp.ViewModels;

namespace Mescp.Selectors
{
    public class PaneStyleSelector : StyleSelector
    {
        public Style MapViewStyle { get; set; }
        public Style EvaluReportViewStyle { get; set; }
        public Style ToolViewStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is MapViewModel)
                return MapViewStyle;

            if (item is EvaluReportViewModel)
                return EvaluReportViewStyle;

            if (item is ToolViewModel)
                return ToolViewStyle;

            return base.SelectStyle(item, container);
        }
    }
}
