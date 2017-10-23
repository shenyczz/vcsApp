/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: MapFunctions
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
using CSharpKit;
using CSharpKit.Palettes;
using CSharpKit.Vision.Mapping;
using CSharpKit.Win32.Interop;
using CSharpKit.Windows.Forms;
using CSharpKit.Windows.Forms.Controls;
using GdiImage = System.Drawing.Image;
using GdiPoint = System.Drawing.Point;
using GdiSize = System.Drawing.Size;

namespace Hdwas
{
    public class MapFunctions
    {
        public MapFunctions()
        {
            Title1 = "";
            Title2 = "";
            Title3 = "";

            LegendTitle = "";

            FilePathName = "";
        }

        public IPalette Palette { get; set; }


        public String Title1;
        public String Title2;
        public String Title3;
        public String LegendTitle;

        public string FilePathName { get; set; }


        public void OnMapRendered(IMap map)
        {
            IMapControl mapControl = map.Container as IMapControl;
            if (mapControl == null)
                return;

            // 绘制缓冲区
            IntPtr hWnd = mapControl.Handle;
            IntPtr hDC = NativeMethods.GetDC(hWnd);

            // 绘制画布
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHdc(hDC);
            if (graphics == null)
                return;

            // 绘制标题
            // 字符串格式
            System.Drawing.StringFormat strfmt = new System.Drawing.StringFormat();
            strfmt.Alignment = System.Drawing.StringAlignment.Center;
            graphics.DrawString(this.Title1,
                new System.Drawing.Font("黑体", 24, System.Drawing.FontStyle.Bold),
                new System.Drawing.SolidBrush(System.Drawing.Color.Black),
                10, 10);

            graphics.DrawString(this.Title2,
                new System.Drawing.Font("宋体", 16),
                new System.Drawing.SolidBrush(System.Drawing.Color.Black),
                20, 56);

            GdiSize pltSize = GetPaleteSize(this.Palette);
            GdiPoint pltPoint = GetPalettePoint(map.Owner as IMapContainer, this.Palette);

            graphics.DrawString(this.Title3,
                new System.Drawing.Font("宋体", 16),
                new System.Drawing.SolidBrush(System.Drawing.Color.Black),
                20, pltPoint.Y + pltSize.Height - 32);

            // 绘制图例
            System.Drawing.Image image = LegendFromPalete(this.Palette);
            if (image != null)
                graphics.DrawImage(image, pltPoint);

            graphics.Dispose();
            NativeMethods.ReleaseDC(hWnd, hDC);

            return;
        }


        private GdiImage LegendFromPalete(IPalette palette)
        {
            GdiImage image = null;

            if (palette == null)
                return null;

            int iCount = palette.Count;

            int topshift = 16;      // 顶部位移
            int margin = 5;         // 页边空白
            int legWidth = 40;      // 图例条目宽度
            int legHeight = 12;     // 图例条目高度
            int legGap = 13;        // 图例条目间隙

            int iWidth = 100;                                           // 图例宽度
            int iHeight = topshift + legGap * iCount + margin * 2;      // 图例高度

            image = new System.Drawing.Bitmap(iWidth, iHeight);

            // GDI+ 画布
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(image);

            // 字符串格式
            System.Drawing.StringFormat strfmt = new System.Drawing.StringFormat();
            strfmt.Alignment = System.Drawing.StringAlignment.Center;

            // 背景颜色
            System.Drawing.Color backColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);

            // 绘制边框
            graphics.FillRectangle(new System.Drawing.SolidBrush(backColor), 0, 0, iWidth, iHeight);
            graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 0, 0, 0)), 0, 0, iWidth - 1, iHeight - 1);

            // 绘制标题
            strfmt.Alignment = System.Drawing.StringAlignment.Center;
            graphics.DrawString
                (
                    this.LegendTitle,
                    new System.Drawing.Font("宋体", 9, System.Drawing.FontStyle.Bold),
                    new System.Drawing.SolidBrush(System.Drawing.Color.Black),
                    new System.Drawing.RectangleF(1.0f * margin, 1.0f * margin, 1.0f * iWidth - margin, 1.0f * topshift),
                    strfmt
                );

            strfmt.Alignment = System.Drawing.StringAlignment.Far;
            for (int i = 0; i < iCount; i++)
            {
                IPaletteItem item = palette[i];

                // 1.绘制图例色标
                graphics.FillRectangle(new System.Drawing.SolidBrush(item.Color),
                    new System.Drawing.Rectangle(margin, topshift + margin + legGap * i, legWidth, legHeight));

                // 2.绘制图例文本
                String comment = item.Comment;
                if (String.IsNullOrEmpty(comment))
                { comment = item.Value.ToString("F2"); }

                graphics.DrawString
                    (
                        comment,
                        new System.Drawing.Font("宋体", 9),
                        new System.Drawing.SolidBrush(System.Drawing.Color.Black),
                        new System.Drawing.RectangleF(10.0f + legWidth, 1.0f * legGap * i + margin + topshift, 1.0f * iWidth - 10.0f - legWidth - 16, 1.0f * legHeight),
                        strfmt
                    );
            }


            return image;
        }
        private GdiSize GetPaleteSize(IPalette palette)
        {
            if (palette == null)
                return System.Drawing.Size.Empty;

            int topshift = 16;      // 顶部位移
            int margin = 5;         // 页边空白
            //int legWidth = 40;      // 图例条目宽度
            //int legHeight = 12;     // 图例条目高度
            int legGap = 13;        // 图例条目间隙

            int iCount = palette.Count;


            int iWidth = 100;                                           // 图例宽度
            int iHeight = topshift + legGap * iCount + margin * 2;      // 图例高度

            return new System.Drawing.Size(iWidth, iHeight);
        }
        private GdiPoint GetPalettePoint(IMapContainer mapContainer, IPalette palette)
        {
            System.Drawing.Size sizePalette = GetPaleteSize(palette);
            if (sizePalette.IsEmpty)
                return GdiPoint.Empty;

            GdiSize size = new GdiSize((int)mapContainer.GetWidth(), (int)mapContainer.GetHeight());
            //GdiSize size = this.mapControl.Size;
            int x = size.Width - sizePalette.Width - 10;
            int y = size.Height - sizePalette.Height - 10;

            return new System.Drawing.Point(x, y);
        }

    }
}
