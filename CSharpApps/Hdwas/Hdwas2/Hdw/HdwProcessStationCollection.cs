/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HdwProcessStationCollection
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
    public class HdwProcessStationCollection : List<HdwProcessStation>
    {
        private static readonly String defaultName = typeof(HdwProcessStationCollection).Name;

        public static XElement ToXElement(IEnumerable<HdwProcessStation> hdwStations)
        {
            XElement xElement = new XElement(defaultName);

            try
            {
                hdwStations.ToList().ForEach(p => xElement.Add(p.ToXElement()));
            }
            catch (Exception)
            {
                xElement = null;
            }

            return xElement;
        }

        public static HdwProcessStationCollection FromXElement(XElement xElement)
        {
            HdwProcessStationCollection hdwpStations = new HdwProcessStationCollection();

            try
            {
                if (xElement.Name != defaultName)
                    return null;

                xElement.Nodes().ToList().ForEach(xe =>
                {
                    hdwpStations.Add(HdwProcessStation.FromXElement(xe as XElement));
                });
            }
            catch (Exception)
            {
                hdwpStations = null;
            }

            return hdwpStations;
        }
    }
}
