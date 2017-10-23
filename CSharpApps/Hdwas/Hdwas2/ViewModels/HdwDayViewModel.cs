/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: HdwDayViewModel
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
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hdwas.Core;
using CSharpKit;
using CSharpKit.Data;
using CSharpKit.Data.Axin;

namespace Hdwas
{
    public class HdwViewModel : ToolViewModel
    {
        public HdwViewModel()
        {
            Files = new ObservableCollection<FileInfo>();
            RefreshFiles();
        }

        #region Files

        public ObservableCollection<FileInfo> Files { get; protected set; }
        
        #endregion

        #region SelectedItem

        private Object _SelectedItem;
        public Object SelectedItem
        {
            protected get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                RaisePropertyChanged("SelectedItem");

                OnSelectedItemChanged(_SelectedItem);
            }
        }

        protected virtual void RefreshFiles() { }

        protected virtual void OnSelectedItemChanged(Object parameter) { }

        #endregion

    }

    public class HdwDayViewModel : HdwViewModel
    {
        public HdwDayViewModel()
        {
            this.SelectedDate = DateTime.Now;
        }


        #region SelectedDate

        private DateTime _SelectedDate;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set
            {
                _SelectedDate = value;
                RaisePropertyChanged("SelectedDate");
            }
        }

        #endregion


        protected override void RefreshFiles()
        {
            string filePath = System.IO.Path.Combine(TheApp.DataPathHdwDay);
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

            Workspace.Instance.MapViewModel.HdwFlag = 1;    // 1=干热风日 2=干热风过程
            Workspace.Instance.MapViewModel.FilePath = (parameter as FileInfo).FullName;
            Workspace.Instance.HdwDayDataGridViewModel.FilePath = (parameter as FileInfo).FullName;
        }



        #region HdwDayCommand

        private RelayCommand _HdwDayCommand;
        public ICommand HdwDayCommand
        {
            get
            {
                if (_HdwDayCommand == null)
                {
                    _HdwDayCommand = new RelayCommand(p => OnHdwDay(p), p => CanHdwDay(p));
                }

                return _HdwDayCommand;
            }
        }

        private void OnHdwDay(Object parameter)
        {
            DoHdwDay();
        }
        private Boolean CanHdwDay(Object parameter)
        {
            return true;
        }

        #endregion

        #region DoHdwDay

        private void DoHdwDay()
        {
            string ip = "172.18.152.243";

            if (!PingIP(ip))
            {
                MessageBox.Show(string.Format("网络: {0} 不畅通\n无法获取监测数据", ip));
                return;
            }

            DateTime dt = this.SelectedDate;
            //dt = DateTime.Parse("2014-5-21");

            // 取得地面自动站每天14时气象观测资料
            MeteoElementCollection meteoElements = GetMeteoElements(dt);
            if (meteoElements == null || meteoElements.Count == 0)
            {
                MessageBox.Show(string.Format("没有取得气象数据!"));
                return;
            }

            //meteoElements.ToXElement().Save("e:\\temp\\1.xml");
            //MeteoElementCollection mes = MeteoElementCollection.FromXElement(System.Xml.Linq.XElement.Load("e:\\temp\\1.xml"));

            // 输出数据
            string fileName = this.HdwDayOutput(meteoElements, dt);
            if (fileName == null)
            {
                MessageBox.Show(string.Format("保存文件错误!"));
                return;
            }

            // 更新显示
            this.RefreshFiles();
            FileInfo fi = this.Files.FirstOrDefault(p => p.Name == fileName);
            this.SelectedItem = fi;

        }

        private Boolean PingIP(string strIP)
        {
            PingOptions pingOption = new PingOptions();
            pingOption.DontFragment = true;

            string data = "sendData:goodgoodgoodgoodgoodgood";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 500;

            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(strIP, timeout, buffer);

            return (reply.Status == IPStatus.Success);
        }

        private MeteoElementCollection GetMeteoElements(DateTime dateTime)
        {
            MeteoElementCollection meteoElements = new MeteoElementCollection();

            SqlConnection conn = new SqlConnection();

            try
            {
                // 取得地面自动站每天14时气象观测资料
                DateTime dt = dateTime;

                string strFields = "[iiiii],[ObvDate],[ObvTime],[F10],[T],[U]";
                string strTables = string.Format("MeteHour{0}", dt.Year);
                string strWhere = string.Format("[ObvDate]={0} AND ObvTime=1400", dt.ToString("yyyyMMdd"));
                string strOrderbys = "[iiiii]";

                string strSql = string.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}",
                     strFields, strTables, strWhere, strOrderbys);

                conn.ConnectionString = "Data Source=172.18.152.243;Initial Catalog=HenanClimate;User ID=nqzx;Password=KyCen5946";
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = strSql;

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    MeteoElement meteoElement = new MeteoElement();
                    meteoElement.DateTime = dt;
                    meteoElement.StationId = rd.GetValue(0).ToString();
                    meteoElement.W14 = double.Parse(rd.GetValue(3).ToString()) / 10;
                    meteoElement.T14 = double.Parse(rd.GetValue(4).ToString()) / 10;
                    meteoElement.U14 = double.Parse(rd.GetValue(5).ToString());

                    meteoElements.Add(meteoElement);
                }

                conn.Close();

            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return meteoElements;
        }

        private String HdwDayOutput(MeteoElementCollection meteoElements, DateTime dateTime)
        {
            string fileName = string.Format("{0}.txt", dateTime.ToString("yyyyMMdd"));

            try
            {
                int level = 2;  // 级别

                string fn = System.IO.Path.Combine(TheApp.ConfigPath, "30_StationDhw.txt");
                AxinFileProvider axinFileProvider = new AxinFileProvider(fn);

                AxinStationFile axinStationFile = axinFileProvider.DataInstance as AxinStationFile;
                AxinStationFileDataInfo dataInfo = axinStationFile.DataInfo as AxinStationFileDataInfo;
                List<StationInfo> stationInfos = axinStationFile.StationInfos;

                // 1.更改数据信息
                dataInfo.DateTime = dateTime;

                // 2.更改数据
                foreach (StationInfo si in stationInfos)
                {
                    MeteoElement meteoElement = meteoElements.Find(p => p.StationId == si.Id);
                    if (meteoElement == null)
                        continue;

                    if (level == 2)
                    {
                        Hdw hdw = new Hdw(meteoElement.T14, meteoElement.U14, meteoElement.W14);
                        si.CurrentElementValue = hdw.Value;
                    }
                    else if (level == 3)
                    {
                        CropGrowthPeriod cgp = TheApp.CropGrowthPeriods.Find(p => p.StationId == si.Id);
                        Hdw hdw = new Hdw(meteoElement, cgp);
                        si.CurrentElementValue = hdw.Value;
                    }
                }

                string filePathName = System.IO.Path.Combine(TheApp.DataPathHdwDay, fileName);
                //axinStationFile.DataInfo.ProductCode = 6810 + level;         // 产品代码（用于确定调色板）
                axinStationFile.DataInfo.ProductCode = 6812;         // 产品代码（用于确定调色板）
                axinStationFile.DataProcessor.SaveAs(filePathName);
            }
            catch (Exception)
            {
                fileName = null;
            }

            return fileName;
        }

        #endregion


    }
}
