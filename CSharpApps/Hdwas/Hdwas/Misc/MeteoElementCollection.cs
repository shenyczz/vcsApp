/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: Region
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

namespace Hdwas
{
    public class MeteoElementCollection : List<MeteoElement>
    {
        public XElement ToXElement()
        {
            XElement xElement = new XElement(this.GetType().Name);
            {
                this.ForEach(fs =>
                {
                    xElement.Add(fs.ToXElement());
                });
            }
            return xElement;
        }

        public void FromXElement(XElement xElement)
        {
            if (xElement.Name != this.GetType().Name)
                return;

            foreach (XElement xe in xElement.Nodes())
            {
                this.Add(new MeteoElement(xe));
            }
        }
    }
}
