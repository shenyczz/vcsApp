/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HdwDayDataGridViewModel
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
using CSharpKit;
using CSharpKit.Data;
using CSharpKit.Data.Axin;

namespace Hdwas
{
    public class HdwDayDataGridViewModel : DataGridViewModel
    {
        List<StationInfo> _StationInfos;
        public List<StationInfo> StationInfos
        {
            get { return _StationInfos; }
            set
            {
                _StationInfos = value;
                RaisePropertyChanged("StationInfos");
            }
        }

        protected override void OnFilePathChanged(string filePath)
        {
            try
            {
                var vm = Workspace.Instance.Documents.FirstOrDefault(p => p is HdwDayDataGridViewModel);

                if (vm == null)
                {
                    Workspace.Instance.Documents.ToList().ForEach(p =>
                    {
                        if (p is DataGridViewModel)
                            Workspace.Instance.Documents.Remove(p);
                    });

                    Workspace.Instance.Documents.Add(Workspace.Instance.HdwDayDataGridViewModel);
                }

                Workspace.Instance.HdwDayDataGridViewModel.ToolTip = filePath;

                IProvider dataProvider = new AxinFileProvider(filePath);
                AxinStationFile axinStationFile = dataProvider.DataInstance as AxinStationFile;
                this.StationInfos = axinStationFile.StationInfos;
            }
            catch (Exception)
            {
                this.StationInfos = null;
            }
        }
    }
}
