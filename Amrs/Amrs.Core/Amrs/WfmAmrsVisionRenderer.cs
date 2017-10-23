/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: WfmAmrsVisionRenderer
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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amrs.Core;
using CSharpKit.Data;
using CSharpKit.Data.Esri;
using CSharpKit.Geometrics;
using CSharpKit.Vision.Mapping;

namespace CSharpKit.Vision.Presentation
{
    public class WfmAmrsVisionRenderer : AmrsVisionRenderer
    {
        protected override void OnRenderAmrsVision(IRenderEngine renderEngine)
        {
            //---------------------------------------------
            IMap map = renderEngine.Owner as IMap;
            if (map == null)
                return;

            IVision vision = this.Owner as IVision;
            if (vision == null)
                return;

            ILayer layer = vision.Owner as ILayer;
            if (layer == null)
                return;

            IProvider provider = vision.Provider;
            if (provider == null)
                return;

            IDataInstance dataInstance = provider.DataInstance;
            if (dataInstance == null)
                return;

            IDataProcessor dataProcessor = dataInstance.DataProcessor;
            if (dataProcessor == null)
                return;

            IDataInfo dataInfo = dataInstance.DataInfo;
            if (dataInfo == null)
                return;

            // 绘制缓冲区
            IntPtr hDC = (IntPtr)map.DrawBuffer;
            if (hDC == null)
                return;

            // 绘制画布
            Graphics graphics = Graphics.FromHdc(hDC);
            if (graphics == null)
                return;
            //---------------------------------------------
            // 是否剪切
            if (vision.IsClip)
            {
                map.BuildClipPath();
                GraphicsPath clipPath = map.ClipPath as GraphicsPath;
                if (clipPath != null)
                {
                    graphics.ResetClip();
                    graphics.SetClip(clipPath);
                }
            }
            //---------------------------------------------
            // 图像
            Image image = (dataProcessor as AmrsFileProcessor).RsImage;
            if (image == null)
                return;

            ColorMap[] clrMap = new ColorMap[] { new ColorMap() };
            clrMap[0].OldColor = Color.FromArgb(255, 0, 0, 0);
            clrMap[0].NewColor = Color.FromArgb(255, 255, 255, 255);

            // 透明度
            //float traf = 1.0f;
            float traf = 0.01f * (100 - layer.Vision.Transparency);
            float[][] faa = new float[][]
            {
                new float[]{1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                new float[]{0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                new float[]{0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                new float[]{0.0f, 0.0f, 0.0f, traf, 0.0f},
                new float[]{0.0f, 0.0f, 0.0f, 0.0f, 1.0f},
            };
            ColorMatrix clrMatrix = new ColorMatrix(faa);

            // 图像属性
            ImageAttributes imgAttributes = new ImageAttributes();
            imgAttributes.SetRemapTable(clrMap, ColorAdjustType.Bitmap);
            imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            // 绑定矩形
            IExtent box = dataInfo.Extent;
            double xmin = box.MinX;
            double ymin = box.MinY;
            double xmax = box.MaxX;
            double ymax = box.MaxY;
            renderEngine.WorldToView(ref xmin, ref ymin);
            renderEngine.WorldToView(ref xmax, ref ymax);

            RectangleF rcf = new RectangleF(
                (float)xmin,
                (float)(ymin - Math.Abs(ymax - ymin)),
                (float)Math.Abs(xmax - xmin),
                (float)Math.Abs(ymax - ymin));

            Rectangle rcf2 = new Rectangle(
                (int)xmin,
                (int)(ymin - Math.Abs(ymax - ymin)),
                (int)Math.Abs(xmax - xmin),
                (int)Math.Abs(ymax - ymin));

            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            graphics.DrawImage(image
                , rcf2
                , 0
                , 0
                , image.Width
                , image.Height
                , GraphicsUnit.Pixel
                , imgAttributes
                );
            //---------------------------------------------
            graphics.Dispose();
            //---------------------------------------------
            return;
        }
    }
}
