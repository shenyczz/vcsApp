/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HdwProcessDataGridViewModel
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
    public class HdwProcessDataGridViewModel : DataGridViewModel
    {
        #region HdwProcessStations

        HdwProcessStationCollection _HdwProcessStations;
        public HdwProcessStationCollection HdwProcessStations
        {
            get { return _HdwProcessStations; }
            set
            {
                _HdwProcessStations = value;
                RaisePropertyChanged("HdwProcessStations");
            }
        }
        
        #endregion

        protected override void OnFilePathChanged(string filePath)
        {
            try
            {
                var vm = Workspace.Instance.Documents.FirstOrDefault(p => p is HdwProcessDataGridViewModel);

                if (vm == null)
                {
                    Workspace.Instance.Documents.ToList().ForEach(p =>
                    {
                        if (p is DataGridViewModel)
                            Workspace.Instance.Documents.Remove(p);
                    });

                    Workspace.Instance.Documents.Add(Workspace.Instance.HdwProcessDataGridViewModel);
                }

                string xmlfile = filePath.Replace(".txt", ".xml");
                Workspace.Instance.HdwProcessDataGridViewModel.ToolTip = xmlfile;
                this.HdwProcessStations = HdwProcessStationCollection.FromXElement(XElement.Load(xmlfile));
            }
            catch (Exception)
            {
                this.HdwProcessStations = null;
            }
        }
    }
}
