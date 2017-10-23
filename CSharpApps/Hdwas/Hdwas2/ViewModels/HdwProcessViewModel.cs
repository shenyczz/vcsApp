/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HdwProcessViewModel
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using CSharpKit;
using CSharpKit.Data;
using CSharpKit.Data.Axin;

namespace Hdwas
{
    public class HdwProcessViewModel : HdwViewModel
    {
        public HdwProcessViewModel()
        {
            this.SelectedDateBeg = DateTime.Now;
            this.SelectedDateEnd = DateTime.Now + new TimeSpan(1, 0, 0, 0);
        }

        #region SelectedDateBeg

        private DateTime _SelectedDateBeg;
        public DateTime SelectedDateBeg
        {
            get { return _SelectedDateBeg; }
            set
            {
                _SelectedDateBeg = value;
                RaisePropertyChanged("SelectedDateBeg");
            }
        }
        
        #endregion

        #region SelectedDateEnd

        private DateTime _SelectedDateEnd;
        public DateTime SelectedDateEnd
        {
            get { return _SelectedDateEnd; }
            set
            {
                _SelectedDateEnd = value;
                RaisePropertyChanged("SelectedDateEnd");
            }
        }

        #endregion


        protected override void RefreshFiles()
        {
            string filePath = System.IO.Path.Combine(TheApp.DataPathHdwProcess);
            DirectoryInfo dir = new DirectoryInfo(filePath);
            List<FileInfo> fis = dir.GetFiles("*.txt").ToList();
            fis.Sort((x, y) => { return -x.Name.CompareTo(y.Name); });

            this.Files.Clear();
            fis.ForEach(fi => this.Files.Add(fi));
        }

        protected override void OnSelectedItemChanged(Object parameter)
        {
            if (parameter == null)
                return;

            Workspace.Instance.MapViewModel.HdwFlag = 2;    // 1=干热风日 2=干热风过程
            Workspace.Instance.MapViewModel.FilePath = (parameter as FileInfo).FullName;
            Workspace.Instance.HdwProcessDataGridViewModel.FilePath = (parameter as FileInfo).FullName;
        }


        #region HdwProcessCommand

        private RelayCommand _HdwProcessCommand;
        public ICommand HdwProcessCommand
        {
            get
            {
                if (_HdwProcessCommand == null)
                {
                    _HdwProcessCommand = new RelayCommand(p => OnHdwProcess(p), p => CanHdwProcess(p));
                }

                return _HdwProcessCommand;
            }
        }

        private void OnHdwProcess(Object parameter)
        {
            DoHdwProcess();
        }
        private Boolean CanHdwProcess(Object parameter)
        {
            return true;
        }

        #endregion

        #region DoHdwProcess

        private void DoHdwProcess()
        {
            try
            {
                // 1.干热风过程起止时间
                DateTime dtStart = (DateTime)this.SelectedDateBeg;
                DateTime dtEnd = (DateTime)this.SelectedDateEnd;

                //dtStart = DateTime.Parse("2013-05-11");
                //dtEnd = DateTime.Parse("2013-05-21");
                //dtEnd = DateTime.Parse("2014-05-21");

                // 2.干热风日数据搜集
                List<HdwDay> hdwDays = this.HdwDayCollect(dtStart, dtEnd);
                if (hdwDays == null || hdwDays.Count == 0)
                {
                    MessageBox.Show("没有干热风数据!");
                    return;
                }

                // 3.构造干热风过程站点集合
                List<HdwProcessStation> hdwpStations = new List<HdwProcessStation>();
                {
                    foreach (StationInfo si in hdwDays[0].StationInfos)
                    {
                        HdwProcessStation hdwpStation = new HdwProcessStation() { Station = si };
                        hdwpStations.Add(hdwpStation);
                    }
                }

                // 4.干热风过程判断
                HdwProcessDetermine(hdwDays, ref hdwpStations);

                // 5.干热风过程输出
                HdwProcessOutput(hdwpStations, dtStart, dtEnd);
            }
            catch(Exception ex)
            {
                String msg = ex.Message;
            }
        }

        /// <summary>
        /// 搜集干热风日数据
        /// </summary>
        /// <param name="dateTimeStart"></param>
        /// <param name="dateTimeEnd"></param>
        /// <returns></returns>
        private List<HdwDay> HdwDayCollect(DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            List<HdwDay> hdwDays = new List<HdwDay>();

            try
            {
                string filePathName = "";
                TimeSpan ts = new TimeSpan(1, 0, 0, 0);

                for (DateTime dt = dateTimeStart; dt <= dateTimeEnd; dt += ts)
                {
                    filePathName = System.IO.Path.Combine(TheApp.DataPathHdwDay,
                        string.Format("{0}.txt", dt.ToString("yyyyMMdd")));

                    if (!File.Exists(filePathName))
                        continue;

                    IProvider dataProvider = new AxinFileProvider(filePathName);
                    hdwDays.Add(new HdwDay
                    {
                        DateTime = dataProvider.DataInstance.DataInfo.DateTime,
                        StationInfos = (dataProvider.DataInstance as AxinStationFile).StationInfos,
                    });

                }//for (DateTime dt = dateTimeStart; dt <= dateTimeEnd; dt += ts)
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return hdwDays;
        }

        private void HdwProcessDetermine(List<HdwDay> hdwDays, ref List<HdwProcessStation> hdwpStations)
        {
            Hdw hdw = new Hdw();

            foreach (HdwProcessStation hdwpStaion in hdwpStations)
            {
                Station station = hdwpStaion.Station;       // 取出一个站点
                DateTime dtHdwDayPrv = DateTime.MinValue;   // 上个干热风日日期
                DateTime dtHdwDayCur = DateTime.MinValue;   // 当前干热风日日期

                // 对每个站点便利干热风日集合
                foreach (HdwDay hdwDay in hdwDays)
                {
                    StationInfo stationInfo = hdwDay.StationInfos.Find(si => si.Id == station.Id);
                    if (stationInfo == null)
                        continue;

                    stationInfo.CurrentElementIndex = 0;
                    hdw.Value = stationInfo.CurrentElementValue;

                    // 判断干热风过程是否开始
                    // 干热风过程起始日必须是轻、中、重干热风日
                    if (hdw.HdwGrade == HdwGrade.None)
                        continue;

                    // 干热风过程开始
                    // 保存当前干热风日期
                    dtHdwDayCur = hdwDay.DateTime;

                    // 计算时间跨度
                    TimeSpan timeSpan;
                    if (dtHdwDayPrv == DateTime.MinValue)
                        timeSpan = new TimeSpan(0);    // 没有上个干热风日日期，时间跨度为0天
                    else
                        timeSpan = dtHdwDayCur - dtHdwDayPrv;

                    // 保存当前干热风日期
                    // 用于下一个干热风日比较
                    dtHdwDayPrv = hdwDay.DateTime;


                    // 时间(日)跨度=0，表示一次干热风过程的第一个干热风日。
                    // 时间(日)跨度=1，表示一次干热风过程的连续干热风日。
                    // 时间(日)跨度>1，表示上一次干热风过程结束，新一次干热风过程开始。

                    // 干热风过程
                    HdwProcess hdwp = null;
                    if (timeSpan.Days == 0 || timeSpan.Days > 1)
                    {
                        // 时间(日)跨度=0，
                        // 表示一次干热风过程的第一个干热风日,
                        // 添加新的干热风过程
                        hdwp = new HdwProcess();
                        hdwp.StartDate = hdwDay.DateTime;   // 开始日期
                        hdwp.EndDate = hdwDay.DateTime;     // 结束日期
                        hdwpStaion.HdwProcesses.Add(hdwp);
                    }
                    else if (timeSpan.Days == 1)
                    {
                        // 时间(日)跨度=1，
                        // 表示一次干热风过程的连续干热风日,
                        // 修改最后一个干热风过程结束日期
                        hdwp = hdwpStaion.HdwProcesses[hdwpStaion.HdwProcesses.Count - 1];
                        hdwp.EndDate = hdwDay.DateTime;
                    }

                    string stemp = hdw.ToString();
                    if (string.IsNullOrEmpty(hdwp.GradeString))
                    {
                        hdwp.GradeString = stemp;
                    }
                    else
                    {
                        hdwp.GradeString += ";" + stemp;
                    }

                }//foreach (HdwDay hdwDay in hdwDays)

                int n = hdwpStaion.HdwProcesses.Count;

            }// foreach (HdwProcessStation hdwpStaion in hdwpStations)

            return;
        }

        private void HdwProcessOutput(List<HdwProcessStation> hdwpStations, DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            //---------------------------------------------
            // xml 文件
            String fileName1 = string.Format("{0}_{1}.xml",
                dateTimeStart.ToString("yyyyMMdd"), dateTimeEnd.ToString("yyyyMMdd"));
            string filePathName1 = System.IO.Path.Combine(TheApp.DataPathHdwProcess, fileName1);
            HdwProcessStationCollection.ToXElement(hdwpStations).Save(filePathName1);
            //---------------------------------------------
            // axin 文件
            string fn = System.IO.Path.Combine(TheApp.ConfigPath, "30_StationDhw.txt");
            AxinFileProvider axinFileProvider = new AxinFileProvider(fn);
            AxinStationFile axinStationFile = axinFileProvider.DataInstance as AxinStationFile;
            AxinStationFileDataInfo dataInfo = axinStationFile.DataInfo as AxinStationFileDataInfo;
            List<StationInfo> stationInfos = axinStationFile.StationInfos;

            foreach (StationInfo si in stationInfos)
            {
                HdwProcessStation hdwpStation = hdwpStations.Find(hdwps => hdwps.Station.Id == si.Id);
                if (hdwpStation == null)
                    continue;

                Hdw hdw = new Hdw(hdwpStation.HdwGrade);
                si.CurrentElementValue = hdw.Value;
            }

            String fileName2 = string.Format("{0}_{1}.txt",
                dateTimeStart.ToString("yyyyMMdd"), dateTimeEnd.ToString("yyyyMMdd"));
            string filePathName2 = System.IO.Path.Combine(TheApp.DataPathHdwProcess, fileName2);
            axinFileProvider.DataInstance.DataInfo.ProductCode = 6812;         // 产品代码（用于确定调色板）
            axinFileProvider.DataInstance.DataProcessor.SaveAs(filePathName2);
            //---------------------------------------------
            // 更新显示
            this.RefreshFiles();
            FileInfo fi = this.Files.FirstOrDefault(p => p.Name == fileName2);
            this.SelectedItem = fi;
            //---------------------------------------------
            return;
        }

        #endregion

    }
}
