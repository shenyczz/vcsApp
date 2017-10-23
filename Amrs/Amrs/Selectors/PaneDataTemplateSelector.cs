/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: PaneDataTemplateSelector
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
using CSharpKit.Windows.Anchoring.Layouts;

namespace Amrs
{
    public class PaneDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MapViewTemplate { get; set; }
        public DataTemplate LayerViewTemplate { get; set; }
        public DataTemplate PropertyGridViewTemplate { get; set; }
        

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is MapViewModel)
                return MapViewTemplate;

            if (item is LayerViewModel)
                return LayerViewTemplate;

            if (item is PropertyGridViewModel)
                return PropertyGridViewTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
