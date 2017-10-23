using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using CSharpKit;
using CSharpKit.Data;
using CSharpKit.Data.Axin;
using CSharpKit.Data.Esri;
using CSharpKit.Devices.Input;
using CSharpKit.Palettes;
using CSharpKit.Vision;
using CSharpKit.Vision.Mapping;
using CSharpKit.Vision.Presentation;
using CSharpKit.Win32;
using CSharpKit.Win32.Interop;
using CSharpKit.Windows.Anchoring;
using CSharpKit.Windows.Anchoring.Layouts;
using CSharpKit.Windows.Forms;
using CSharpKit.Windows.Forms.Controls;
using Microsoft.Win32;

namespace Hdwas
{
    /// <summary>
    /// UC_HdwDayFileList - 干热风文件列表
    /// </summary>
    public partial class UC_HdwDayFileList : UserControl
    {
        public UC_HdwDayFileList()
        {
            InitializeComponent();
            Initialize();
        }


        // 干热风日文件全路径
        private string _hdwdFilePathName = "";

        private AxinFileProvider _axinFileProvider;
        private CropGrowthPeriodCollection _cropGrowthPeriods;

        private String _CurrentFileName = "";
        //[Obsolete("",true)]
        //public String CurrentFileName1
        //{
        //    get { return _CurrentFileName; }
        //}


        #region --Initialize--
        
        private void Initialize()
        {
            datePicker.SelectedDate = DateTime.Now;

            InitializeConfig();
            InitializeListBox();
        }
        private void InitializeConfig()
        {
            string fn = System.IO.Path.Combine(TheApp.ConfigPath, "30_StationDhw.txt");
            _axinFileProvider = new AxinFileProvider(fn);

            // 作物发育期
            _cropGrowthPeriods = TheApp.CropGrowthPeriods;
        }
        private void InitializeListBox()
        {
            listBoxFiles.SelectionChanged += listBoxFiles_SelectionChanged;
            RefreshListBox();
        }

        #endregion

        #region --ListBox--

        private void RefreshListBox()
        {
            listBoxFiles.Items.Clear();

            string filePath = System.IO.Path.Combine(TheApp.DataPath_Hdwd);
            DirectoryInfo dir = new DirectoryInfo(filePath);
            List<FileInfo> fileInfos = dir.GetFiles("*.txt").ToList();
            fileInfos.Sort((x, y) => { return -x.Name.CompareTo(y.Name); });
            foreach (FileInfo fi in fileInfos)
            {
                listBoxFiles.Items.Add(fi);
            }

            // 选中刚生成的文件
            if (!string.IsNullOrEmpty(_hdwdFilePathName))
            {
                FileInfo fileInfo = fileInfos.Find(fi => fi.FullName == _hdwdFilePathName);
                _hdwdFilePathName = ""; // 重置干热风过程文件名称
                if (fileInfo != null)
                    listBoxFiles.SelectedItem = fileInfo;  // 激发 listBoxHdwAssessFiles_SelectionChanged 事件
            }
        }
        private void listBoxFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBox listBox = sender as ListBox;
                FileInfo fileInfo = listBox.SelectedItem as FileInfo;
                if (fileInfo == null)
                    return;

                string filePathName = fileInfo.FullName;

                // 显示评估数据的空间分布
                this.FillCountyColor(filePathName);

                // 显示干热风日监测数据
                this.ShowHdwDay(filePathName);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                throw ex;
            }
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            switch(button.Name)
            {
                case "buttonGetData":
                    HdwDay();
                    break;

                case "buttonSaveImage":
                    //SaveImage();
                    break;
            }
        }

        #region --干热风日监测--

        /// <summary>
        /// 干热风日监测
        /// </summary>
        private void HdwDay()
        {
            string ip = "172.18.152.243";

            if (!PingIP(ip))
            {
                MessageBox.Show(string.Format("网络: {0} 不畅通\n无法获取监测数据", ip));
                return;
            }

            // 取得地面自动站每天14时气象观测资料
            DateTime dt = (DateTime)this.datePicker.SelectedDate;

            if (GetData(dt))
            {
                RefreshListBox();
            }
            else
            {
                MessageBox.Show("取得数据错误!");
            }
        }

        private bool GetData(DateTime dateTime)
        {
            bool retValue = true;

            SqlConnection conn = new SqlConnection();

            try
            {
                MeteoElementCollection meteoElements = new MeteoElementCollection();

                // 取得地面自动站每天14时气象观测资料
                DateTime dt = (DateTime)this.datePicker.SelectedDate;

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
                    meteoElement.F14 = double.Parse(rd.GetValue(3).ToString());
                    meteoElement.T14 = double.Parse(rd.GetValue(4).ToString());
                    meteoElement.U14 = double.Parse(rd.GetValue(5).ToString());

                    meteoElements.Add(meteoElement);
                }

                conn.Close();


                if (meteoElements.Count > 0)
                {
                    // 保存气象要素
                    string filePathName = System.IO.Path.Combine(TheApp.DataPath_Hdwd,
                        string.Format("{0}.xml", dt.ToString("yyyyMMdd")));
                    meteoElements.ToXElement().Save(filePathName);

                    // 保存站点干热风强度
                    SaveStationFile(meteoElements);
                }
                else
                {
                    retValue = false;
                }

            }
            catch (InvalidOperationException ex)
            {
                string errMsg = ex.Message;
                retValue = false;
                MessageBox.Show(ex.Message);
            }
            catch (SqlException ex)
            {
                string errMsg = ex.Message;
                retValue = false;
                MessageBox.Show(ex.Message);
            }
            catch (System.Configuration.ConfigurationErrorsException ex)
            {
                string errMsg = ex.Message;
                retValue = false;
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                retValue = false;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return retValue;
        }
        private void SaveStationFile(MeteoElementCollection meteoElements)
        {
            try
            {
                AxinStationFile axinStationFile = _axinFileProvider.DataInstance as AxinStationFile;
                AxinStationFileDataInfo dataInfo = axinStationFile.DataInfo as AxinStationFileDataInfo;
                List<StationInfo> stationInfos = axinStationFile.StationInfos;

                // 1.更改数据信息
                dataInfo.DateTime = meteoElements[0].DateTime;

                // 2.更改数据
                foreach (StationInfo si in stationInfos)
                {
                    double v = 0;
                    HDWLevel dhwl = GetDryHotWindLevel(meteoElements.Find(me => me.StationId == si.Id));

                    switch (dhwl)
                    {
                        case HDWLevel.None:
                            v = 0;
                            break;

                        case HDWLevel.Slight:
                            v = 20;
                            break;

                        case HDWLevel.Medium:
                            v = 60;
                            break;

                        case HDWLevel.Severe:
                            v = 80;
                            break;
                    }

                    si.ElementValues[0] = v;
                }

                string filePathName = System.IO.Path.Combine(TheApp.DataPath_Hdwd,
                    string.Format("{0}.txt", dataInfo.DateTime.ToString("yyyyMMdd")));

                axinStationFile.DataProcessor.SaveAs(filePathName);
                _hdwdFilePathName = filePathName;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return;
        }
        private HDWLevel GetDryHotWindLevel(MeteoElement meteoElement)
        {
            HDWLevel dhwl = HDWLevel.None;

            try
            {
                // 取得气象要素
                double t14 = meteoElement.T14 / 10;
                double u14 = meteoElement.U14;
                double f14 = meteoElement.F14 / 10;

                // 作物发育期
                CropGrowthPeriod cropGrowthPeriod = _cropGrowthPeriods.Find(cgp => cgp.StationId == meteoElement.StationId);

                // 灌浆期标志 0=不是灌浆前 1=灌浆前期 2=灌浆中期 3=灌浆后期
                int flag = GetFlag(cropGrowthPeriod, meteoElement.DateTime);   

                if (flag == 0)
                {
                    dhwl = HDWLevel.None;
                }
                else if (flag == 1)
                {//灌浆前期
                    // 轻
                    if (t14 >= 31.5 && u14 < 30 && f14 >= 2.5)
                    { dhwl = HDWLevel.Slight; }

                    // 中
                    if (t14 >= 33.1 && u14 < 30 && f14 >= 2.5)
                    { dhwl = HDWLevel.Medium; }

                    // 重
                    if (t14 >= 34.0 && u14 < 30 && f14 >= 3.0)
                    { dhwl = HDWLevel.Severe; }
                }
                else if (flag == 2)
                {// 灌浆中期
                    // 轻
                    if (t14 >= 32.0 && u14 < 30 && f14 >= 2.5)
                    { dhwl = HDWLevel.Slight; }

                    // 中
                    if (t14 >= 32.0 && u14 < 26 && f14 >= 3.0)
                    { dhwl = HDWLevel.Medium; }

                    // 重
                    if (t14 >= 35.0 && u14 < 23 && f14 >= 3.5)
                    { dhwl = HDWLevel.Severe; }
                }
                else if (flag == 3)
                {// 灌浆后期
                    // 轻
                    //14时气温32.4～33.9℃，相对湿度＜31%，14时风速≥2.5m/s 
                    if (t14 >= 32.4 && t14 < 33.9 && u14 < 31 && f14 >= 2.5)
                    { dhwl = HDWLevel.Slight; }

                    // 中
                    //14时气温34.0～36.9℃，相对湿度＜28%，14时风速≥3.0m/s 
                    if (t14 >= 34.0 && t14 < 36.9 && u14 < 28 && f14 >= 3.0)
                    { dhwl = HDWLevel.Medium; }

                    // 重
                    if (t14 >= 37.0 && u14 < 24 && f14 >= 4.0)
                    { dhwl = HDWLevel.Severe; }
                }
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return dhwl;
        }

        // 灌浆期标志 0=不是灌浆前 1=灌浆前期 2=灌浆中期 3=灌浆后期
        private int GetFlag(CropGrowthPeriod cropGrowthPeriod, DateTime dateTime)
        {
            int flag = 0;

            try
            {
                string s1 = cropGrowthPeriod.GroutingPeriod01;
                string[] sa1 = s1.Split(new char[] { '月', '日' }, StringSplitOptions.RemoveEmptyEntries);
                int m1 = int.Parse(sa1[0]);
                int d1 = int.Parse(sa1[1]);

                string s2 = cropGrowthPeriod.GroutingPeriod01;
                string[] sa2 = s2.Split(new char[] { '月', '日' }, StringSplitOptions.RemoveEmptyEntries);
                int m2 = int.Parse(sa2[0]);
                int d2 = int.Parse(sa2[1]);

                string s3 = cropGrowthPeriod.GroutingPeriod01;
                string[] sa3 = s3.Split(new char[] { '月', '日' }, StringSplitOptions.RemoveEmptyEntries);
                int m3 = int.Parse(sa3[0]);
                int d3 = int.Parse(sa3[1]);

                int mm = dateTime.Month;
                int dd = dateTime.Day;

                if (mm >= m1 && dd >= d1)
                {
                    flag = 1;
                }

                if (mm >= m2 && dd >= d2)
                {
                    flag = 2;
                }

                if (mm >= m3 && dd >= d3)
                {
                    flag = 3;
                }
            }
            catch
            {
                flag = 0;
            }

            return flag;
        }

        #endregion

        #region --SaveImage--[不再使用 - 20141030,shenyc.]
        
        [Obsolete("",true)]
        private void SaveImage()
        {/*
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = TheApp.ImagePath;
            sfd.FileName = "未命名";
            sfd.Filter = "PNG图片|*.png|JPG图片|*.jpg";
            sfd.AddExtension = true;

            if ((bool)sfd.ShowDialog())
            {
                SaveImage(sfd.FileName);
            }
            */
        }

        [Obsolete("", true)]
        private void SaveImage(string filePathName)
        {
            /*IMap map = TheApp.Map;

            IMapControl mapControl = map.Container as IMapControl;
            if (mapControl == null)
                return;

            int w = (int)mapControl.GetWidth();
            int h = (int)mapControl.GetHeight();

            RECT rc = new RECT(w, h);

            IntPtr ih = TheApp.CaptureRect(mapControl.Handle, ref rc);
            System.Drawing.Bitmap bmp = System.Drawing.Bitmap.FromHbitmap(ih);
            bmp.Save(filePathName);*/
        }

        #endregion

        bool PingIP(string strIP)
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

        private void FillCountyColor(string filePathName)
        {
            IMap map = TheApp.Map;
            ILayer layer = map.LayerManager.Layers.Find(l => l.Id == TheApp.MapLayerId_County);
            IVision vision = layer.Vision;
            IProvider provider = vision.Provider;
            IDataInstance dataInstance = provider.DataInstance;

            // 取得Shape文件提供者
            ShapeFile shapeFile = dataInstance as ShapeFile;

            IProvider dataProvider = new AxinFileProvider(filePathName);
            AxinStationFile axinStationFile = dataProvider.DataInstance as AxinStationFile;
            List<StationInfo> stationInfos = axinStationFile.StationInfos;

            IPalette palette = axinStationFile.Palette;

            foreach (StationInfo si in stationInfos)
            {
                // 属性添加 StationID
                List<IFeature> features = shapeFile.Features.FindAll(f => f.Id == si.Id);
                foreach (IFeature f in features)
                {
                    System.Drawing.Color clr = palette.GetColor(si.CurrentElementValue, System.Drawing.Color.Green);
                    f.Tag = clr;
                }
            }

            // 标题、图例
            FileInfo fi = new FileInfo(filePathName);

            TheApp.MapFunctions.Title1 = string.Format("{0}冬小麦干热风灾害监测", TheApp.AreaName);
            TheApp.MapFunctions.Title2 = fi.Name.Replace(".txt", "");
            TheApp.MapFunctions.Title3 = "河南省气象科学研究所农气中心制作";
            TheApp.MapFunctions.LegendTitle = "图例";

            TheApp.MapFunctions.FilePathName = filePathName;

            TheApp.MapFunctions.Palette = palette;

            map.Refresh(true);
        }

        private void ShowHdwDay(string filePathName)
        {
            DockingManager dockManager = TheApp.DockingManager;

            AxinFileProvider axinFileProvider = new AxinFileProvider(filePathName);
            AxinStationFile axinStationFile = axinFileProvider.DataInstance as AxinStationFile;
            AxinStationFileDataInfo dataInfo = axinStationFile.DataInfo as AxinStationFileDataInfo;
            List<StationInfo> stationInfos = axinStationFile.StationInfos;

            TheApp.StationInfos_Day = stationInfos;

            FileInfo fi = new FileInfo(filePathName);
            string fileName = fi.Name;

            // 文档面板
            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane == null)
                return;

            // 文档
            List<LayoutDocument> docs = dockManager.Layout.Descendents().OfType<LayoutDocument>().ToList();
            LayoutDocument doc = docs.Find(d => d.Description == "干热风日");
            if (doc == null)
            {
                doc = new LayoutDocument();
                {
                    doc.Description = "干热风日";
                    doc.Content = new UC_HdwDay();
                }
                firstDocumentPane.Children.Add(doc);
            }

            // 设置文档标题
            doc.Title = string.Format("干热风日 - {0}", fi.Name);

            //doc.IsActive = true;
            UC_HdwDay ucHdwAssessment = doc.Content as UC_HdwDay;
            if (ucHdwAssessment != null)
                ucHdwAssessment.RefreshDataGrid();

            return;
        }


    }//UC_HdwDayFileList
}
