/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: MeteoElement
 *  Version: 
 * Function: 
 * 
 *   Author: 申永辰.郑州 (shenyczz@163.com)
 * DateTime: 2010 - 2014
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CSharpKit;

namespace Hdwas
{
    public class MeteoElement
    {
        public MeteoElement()
        {
            this.StationId = "";
            this.DateTime = DateTime.Now;
        }

        public MeteoElement(XElement xe)
        {
            this.FromXElement(xe);
        }

        /// <summary>
        /// 站点
        /// </summary>
        public String StationId { get; set; }

        /// <summary>
        /// 日期时间
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 14时风速
        /// </summary>
        public Double F14 { get; set; }

        /// <summary>
        /// 14时气温
        /// </summary>
        public Double T14 { get; set; }

        /// <summary>
        /// 14时相对湿度
        /// </summary>
        public Double U14 { get; set; }

        public XElement ToXElement()
        {
            XElement xElement = new XElement(this.GetType().Name);
            {
                xElement.Add(new XElement("StationId", StationId.ToString()));
                xElement.Add(new XElement("DateTime", DateTime.ToString("yyyy-MM-dd HH:mm:ss")));
                xElement.Add(new XElement("F14", F14.ToString("F3")));
                xElement.Add(new XElement("T14", T14.ToString("F3")));
                xElement.Add(new XElement("U14", U14.ToString("F3")));
            }
            return xElement;
        }

        public void FromXElement(XElement xElement)
        {
            if (xElement.Name != this.GetType().Name)
                return;

            // 元素
            foreach (XElement xe in xElement.Nodes())
            {
                switch (xe.Name.ToString())
                {
                    case "StationId":
                        this.StationId = xe.Value;
                        break;

                    case "DateTime":
                        this.DateTime = DateTime.Parse(xe.Value);
                        break;

                    case "F14":
                        this.F14 = Double.Parse(xe.Value);
                        break;

                    case "T14":
                        this.T14 = Double.Parse(xe.Value);
                        break;

                    case "U14":
                        this.U14 = Double.Parse(xe.Value);
                        break;
                }
            }
        }
    }
}
