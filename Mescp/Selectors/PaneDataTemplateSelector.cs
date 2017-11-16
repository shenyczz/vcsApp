/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     RgnName: PaneDataTemplateSelector
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
    /// <summary>
    /// 数据模板选择器把数据模板和ViewModel联系起来
    /// 在数据模板字典中把数据模板和View联系起来
    /// </summary>
    public class PaneDataTemplateSelector : DataTemplateSelector
    {
        // 
        // 在布局项目 DataTemplate 选择器中
        //<anchor:DockingManager.LayoutItemTemplateSelector>
        //    <sl:PaneDataTemplateSelector
        //        MapDataTemplate="{StaticResource MapViewDataTemplate}"
        //        ></sl:PaneDataTemplateSelector>
        //</anchor:DockingManager.LayoutItemTemplateSelector>
        //
        //把本类属性 MapDataTemplate 绑定到定义好的数据模板资源 MapViewDataTemplate
        //

        /// <summary>
        /// 地图数据模板
        /// </summary>
        public DataTemplate MapDataTemplate { get; set; }

        public DataTemplate EvaluReportDataTemplate { get; set; }

        /// <summary>
        /// 图层数据模板
        /// </summary>
        public DataTemplate LayerDataTemplate { get; set; }

        /// <summary>
        /// 属性数据模板
        /// </summary>
        public DataTemplate PropertyDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is MapViewModel)
                return MapDataTemplate;

            if (item is EvaluReportViewModel)
                return EvaluReportDataTemplate;

            if (item is LayerViewModel)
                return LayerDataTemplate;

            if (item is PropertyViewModel)
                return PropertyDataTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
