/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: AmrsVisionRenderer
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
using CSharpKit;
using CSharpKit.Data;
using CSharpKit.Data.Axin;
using CSharpKit.Vision;
using CSharpKit.Vision.Mapping;
using CSharpKit.Vision.Presentation;

namespace CSharpKit.Vision.Presentation
{
    public abstract class AmrsVisionRenderer : Renderer
    {
        public override void Render(IRenderEngine renderEngine)
        {
            // 视觉
            IVision vision = this.Owner as IVision;
            if (vision == null)
                return;

            if (!vision.IsVisible)
                return;

            this.OnRenderAmrsVision(renderEngine);

            return;
        }

        /// <summary>
        /// 视觉渲染(在WFM和WPF中渲染方法有差异)
        /// </summary>
        /// <param name="renderEngine"></param>
        protected abstract void OnRenderAmrsVision(IRenderEngine renderEngine);
    }
}
