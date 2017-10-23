/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HdwProcessCollection
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
    public class HdwProcessCollection : List<HdwProcess>
    {
        private static readonly String defaultName = "HdwProcessCollection";

        public XElement ToXElement()
        {
            return HdwProcessCollection.ToXElement(this);
        }

        public static XElement ToXElement(IEnumerable<HdwProcess> hdwProcesses)
        {
            XElement xElement = new XElement(defaultName);

            try
            {
                hdwProcesses.ToList().ForEach(p => xElement.Add(p.ToXElement()));
            }
            catch (Exception)
            {
                xElement = null;
            }

            return xElement;
        }

        public static HdwProcessCollection FromXElement(XElement xElement)
        {
            HdwProcessCollection hdwps = new HdwProcessCollection();

            try
            {
                if (xElement.Name != defaultName)
                    return null;

                xElement.Nodes().ToList().ForEach(xe =>
                {
                    hdwps.Add(HdwProcess.FromXElement(xe as XElement));
                });
            }
            catch (Exception)
            {
                hdwps = null;
            }

            return hdwps;
        }
    }
}
