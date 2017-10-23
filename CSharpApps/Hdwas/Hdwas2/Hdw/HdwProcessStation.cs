/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HdwProcessStation - 站点干热风过程
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
    public class HdwProcessStation
    {
        public HdwProcessStation()
        {
            this.HdwProcesses = new HdwProcessCollection();
        }

        private static readonly String defaultName = typeof(HdwProcessStation).Name;

        /// <summary>
        /// 站点ID
        /// </summary>
        public Station Station { get; set; }

        /// <summary>
        /// 干热风过程集合（每个站点可能有多个干热风过程）
        /// </summary>
        public HdwProcessCollection HdwProcesses { get; private set; }

        /// <summary>
        /// 站点干热风等级 = 最严重干热风过程的等级
        /// </summary>
        public HdwGrade HdwGrade
        {
            get
            {
                HdwGrade grade = HdwGrade.None;
                if (HdwProcesses.Count>0)
                {
                    this.HdwProcesses.Sort((x, y) => -1 * x.CompareTo(y));
                    grade = this.HdwProcesses[0].HdwGrade;
                }
                return grade;
            }
        }

        /// <summary>
        /// 干热风过程信息
        /// </summary>
        public String HdwProcessesInfo
        {
            get
            {
                string info = "";
                HdwProcesses.ForEach(p => info += p.GradeInfo + ";");
                return info;
            }
        }

        // 干热风过程统计信息

        public String HdwProcessCountInfo
        {
            get
            {
                int count = 0;
                string info = "";

                count = HdwProcesses.FindAll(p => p.HdwGrade == HdwGrade.None).Count;
                if (count > 0)
                    info += string.Format("无[{0}]", count) + ";";

                count = HdwProcesses.FindAll(p => p.HdwGrade == HdwGrade.Slight).Count;
                if (count > 0)
                    info += string.Format("轻[{0}]", count) + ";";

                count = HdwProcesses.FindAll(p => p.HdwGrade == HdwGrade.Medium).Count;
                if (count > 0)
                    info += string.Format("中[{0}]", count) + ";";

                count = HdwProcesses.FindAll(p => p.HdwGrade == HdwGrade.Severe).Count;
                if (count > 0)
                    info += string.Format("重[{0}]", count) + ";";

                return info;
            }
        }

        #region XElement
        
        public XElement ToXElement()
        {
            return HdwProcessStation.ToXElement(this);
        }

        public static XElement ToXElement(HdwProcessStation hdwpStation)
        {
            XElement xElement = new XElement(defaultName);

            try
            {
                XElement xeStation = new XElement("Station");
                {
                    xeStation.Add(new XAttribute("Id", hdwpStation.Station.Id.ToString()));
                    xeStation.Add(new XAttribute("Name", hdwpStation.Station.Name.ToString()));
                    xeStation.Add(new XAttribute("Lon", hdwpStation.Station.Lon.ToString("F3")));
                }

                // 元素
                xElement.Add(xeStation);
                xElement.Add(hdwpStation.HdwProcesses.ToXElement());
            }
            catch (Exception)
            {
                xElement = null;
            }

            return xElement;
        }

        public static HdwProcessStation FromXElement(XElement xElement)
        {
            HdwProcessStation instance = new HdwProcessStation();

            try
            {
                if (xElement.Name != defaultName)
                    return null;

                foreach (XElement xe in xElement.Nodes())
                {
                    switch (xe.Name.ToString())
                    {
                        case "Station":
                            instance.Station = new Station();
                            foreach (XAttribute attr in xe.Attributes())
                            {
                                switch (attr.Name.ToString())
                                {
                                    case "Id":
                                        instance.Station.Id = attr.Value;
                                        break;
                                    case "Name":
                                        instance.Station.Name = attr.Value;
                                        break;
                                }
                            }
                            break;

                        case "HdwProcessCollection":
                            instance.HdwProcesses = HdwProcessCollection.FromXElement(xe);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                instance = null;
            }

            return instance;
        }

        #endregion

        public override string ToString()
        {
            return String.Format("{0}:{1}:{2}:{3}", Station.Id, Station.Name, HdwProcesses.Count, HdwGrade.ToString());
        }
    }
}
