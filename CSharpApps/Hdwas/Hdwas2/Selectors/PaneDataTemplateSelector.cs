/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: PaneDataTemplateSelector - 窗格数据模板选择器
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
using CSharpKit.Windows.Anchoring.Layouts;

namespace Hdwas
{
    public class PaneDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MapViewTemplate { get; set; }
        public DataTemplate CGPViewTemplate { get; set; }
        public DataTemplate HdwDayViewTemplate { get; set; }
        public DataTemplate HdwProcessViewTemplate { get; set; }
        public DataTemplate HdwDayDataGridViewTemplate { get; set; }
        public DataTemplate HdwProcessDataGridViewTemplate { get; set; }

        //
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is MapViewModel)
                return MapViewTemplate;

            if (item is CGPViewModel)
                return CGPViewTemplate;

            if (item is HdwDayViewModel)
                return HdwDayViewTemplate;

            if (item is HdwProcessViewModel)
                return HdwProcessViewTemplate;

            if (item is HdwDayDataGridViewModel)
                return HdwDayDataGridViewTemplate;

            if (item is HdwProcessDataGridViewModel)
                return HdwProcessDataGridViewTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
