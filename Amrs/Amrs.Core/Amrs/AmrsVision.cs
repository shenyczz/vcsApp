/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: AmrsVision
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpKit.ComponentModel;
using CSharpKit.Data;
using Amrs.Core;
using CSharpKit.Palettes;
using CSharpKit.Vision.Presentation;

namespace CSharpKit.Vision
{
    public class AmrsVision : VisionBase
    {
        public AmrsVision(String name, IProvider provider)
            : this(name, provider, null) { }

        public AmrsVision(String name, IProvider provider, IRenderer renderer)
            : base(name, provider, renderer)
        {
            this.RenderPriority = RenderPriority.SurfaceImages;
        }

        protected override void CreateCustomProperty_Legend()
        {
            CustomProperty item = null;
            //---------------------------------------------
            AmrsFile amrsFile = this.Provider.DataInstance as AmrsFile;
            IPalette palette = amrsFile.Palette;
            if (palette == null)
                return;

            int count = palette.Count;
            if (count == 0)
                return;

            for (int i = 0; i < count; i++)
            {
                // 取得调色板条目
                IPaletteItem palItem = palette[i];

                item = new CustomProperty(palItem.Comment, palItem.Color);
                item.Category = CustomProperty.Category_Legend;
                //item.DisplayName = "名称";
                //item.Description = "图层名称";
                item.Browsable = true;
                item.ReadOnly = false;
                item.Tag = palItem;
                item.Editor = null;
                item.Converter = null;
                this.CustomProperties.Add(item);
            }
            //---------------------------------------------
            return;
        }

        protected override void InitializeCustomProperty()
        {
            base.InitializeCustomProperty();

            this.Foreground = Color.Black;
            this.LineWidth = 1;

            this.Transparency = 10;
            this.IsDrawContour = true;
            this.IsFillContour = false;
            this.IsLabelContour = true;
            this.Decimals = 1;
            this.IsColorContour = true;
        }
    }
}
