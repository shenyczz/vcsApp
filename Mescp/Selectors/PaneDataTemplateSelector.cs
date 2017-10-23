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
    public class PaneDataTemplateSelector : DataTemplateSelector
    {
        // 
        // 在布局项目 DataTemplate 选择器中
        //<anchor:DockingManager.LayoutItemTemplateSelector>
        //    <sl:PaneDataTemplateSelector
        //        MapDataTemplate="{StaticResource MapViewDataTemplate}"
        //        ></sl:PaneDataTemplateSelector>
        //</anchor:DockingManager.LayoutItemTemplateSelector>
        //把本类属性 MapDataTemplate 绑定到定义好的数据模板资源 MapViewDataTemplate
        //
        public DataTemplate MapDataTemplate { get; set; }
        public DataTemplate LayerDataTemplate { get; set; }

        public override System.Windows.DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is MapViewModel)
                return MapDataTemplate;

            if (item is LayerViewModel)
                return LayerDataTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
