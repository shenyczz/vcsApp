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

namespace Hdwas.Core
{
    public class MeteoElement
    {
        public MeteoElement()
        {
            this.StationId = "";
            this.DateTime = DateTime.Now;
        }

        private static readonly String defaultName = "MeteoElement";

        /// <summary>
        /// 站点
        /// </summary>
        public String StationId { get; set; }

        /// <summary>
        /// 日期时间
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 14时气温
        /// </summary>
        public Double T14 { get; set; }

        /// <summary>
        /// 14时相对湿度
        /// </summary>
        public Double U14 { get; set; }

        /// <summary>
        /// 14时风速
        /// </summary>
        public Double W14 { get; set; }

        #region XElement

        public XElement ToXElement()
        {
            return MeteoElement.ToXElement(this);
        }

        public static XElement ToXElement(MeteoElement meteoElement)
        {
            XElement xElement = new XElement(MeteoElement.defaultName);

            try
            {
                xElement.Add(new XElement("StationId", meteoElement.StationId.ToString()));
                xElement.Add(new XElement("DateTime", meteoElement.DateTime.ToString("yyyy-MM-dd HH:mm:ss")));
                xElement.Add(new XElement("T14", meteoElement.T14.ToString("F3")));
                xElement.Add(new XElement("U14", meteoElement.U14.ToString("F3")));
                xElement.Add(new XElement("W14", meteoElement.W14.ToString("F3")));
            }
            catch (Exception)
            {
                xElement = null;
            }

            return xElement;
        }


        public static MeteoElement FromXElement(XElement xElement)
        {
            if (xElement.Name != MeteoElement.defaultName)
                return null;

            MeteoElement meteoElement = new MeteoElement();

            try
            {
                foreach (XElement xe in xElement.Nodes())
                {
                    switch (xe.Name.ToString())
                    {
                        case "StationId":
                            meteoElement.StationId = xe.Value;
                            break;

                        case "DateTime":
                            meteoElement.DateTime = DateTime.Parse(xe.Value);
                            break;

                        case "T14":
                            meteoElement.T14 = Double.Parse(xe.Value);
                            break;

                        case "U14":
                            meteoElement.U14 = Double.Parse(xe.Value);
                            break;

                        case "W14":
                            meteoElement.W14 = Double.Parse(xe.Value);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                meteoElement = null;
            }

            return meteoElement;
        }

        #endregion
    }

}
