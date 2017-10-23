/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: MeteoElementCollection
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
    public class MeteoElementCollection : List<MeteoElement>
    {
        private static readonly String defaultName = "MeteoElementCollection";

        public XElement ToXElement()
        {
            return MeteoElementCollection.ToXElement(this);
        }

        public static XElement ToXElement(IEnumerable<MeteoElement> meteoElements)
        {
            XElement xElement = new XElement(defaultName);

            try
            {
                meteoElements.ToList().ForEach(p => xElement.Add(p.ToXElement()));
            }
            catch (Exception)
            {
                xElement = null;
            }

            return xElement;
        }

        public static MeteoElementCollection FromXElement(XElement xElement)
        {

            MeteoElementCollection meteoElements = new MeteoElementCollection();

            try
            {
                if (xElement.Name != defaultName)
                    return null;

                xElement.Nodes().ToList().ForEach(xe =>
                {
                    meteoElements.Add(MeteoElement.FromXElement(xe as XElement));
                });
            }
            catch (Exception)
            {
                meteoElements = null;
            }

            return meteoElements;
        }
    }
}
