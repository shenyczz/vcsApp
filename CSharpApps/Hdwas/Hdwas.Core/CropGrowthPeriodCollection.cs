/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: FloweringStageCollection
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
    public class CropGrowthPeriodCollection : List<CropGrowthPeriod>
    {
        public XElement ToXElement()
        {
            XElement xElement = new XElement(this.GetType().Name);
            {
                this.ForEach(cgps =>
                {
                    xElement.Add(cgps.ToXElement());
                });
            }
            return xElement;
        }

        public void FromXElement(XElement xElement)
        {
            if (xElement.Name != this.GetType().Name)
                return;

            this.Clear();

            foreach (XElement xe in xElement.Nodes())
            {
                CropGrowthPeriod cgp = new CropGrowthPeriod(xe);
                this.Add(cgp);
            }
        }
    }
}
