using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using CSharpKit;
using CSharpKit.Data;
using CSharpKit.Data.Axin;
using CSharpKit.Data.Esri;
using CSharpKit.Palettes;
using CSharpKit.Vision;
using CSharpKit.Vision.Mapping;
using CSharpKit.Windows.Anchoring;
using CSharpKit.Windows.Anchoring.Layouts;

namespace Hdwas
{
    /// <summary>
    /// UC_HdwProcessFileList.xaml 的交互逻辑
    /// </summary>
    public partial class UC_HdwProcessFileList : UserControl
    {
        public UC_HdwProcessFileList()
        {
            InitializeComponent();
            Initialize();
        }

        // 干热风过程文件全路径
        string _hdwpFilePathName = "";

        #region --Initialize
        
        private void Initialize()
        {
            InitializeNormal();
            InitializeListBox();
        }
        private void InitializeNormal()
        {
            this.datePickerHdwpStart.SelectedDate = DateTime.Now;
            this.datePickerHdwpEnd.SelectedDate = DateTime.Now + new TimeSpan(1, 0, 0, 0);
        }
        private void InitializeListBox()
        {
            listBoxHdwProcessFiles.SelectionChanged += listBoxHdwProcessFiles_SelectionChanged;

            RefreshListBox();
        }

        #endregion

        #region --Button Click
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "buttonHdwProcess":
                    HdwProcess();
                    break;
            }
        }

        #endregion

        #region --ListBox
        
        private void RefreshListBox()
        {
            // 清除列表显示
            listBoxHdwProcessFiles.Items.Clear();

            // 添加指定文件夹里的文件
            string filePath = System.IO.Path.Combine(TheApp.DataPath_Hdwp);
            DirectoryInfo dir = new DirectoryInfo(filePath);
            List<FileInfo> fileInfos = dir.GetFiles("*.txt").ToList();
            fileInfos.Sort((x, y) => { return -x.Name.CompareTo(y.Name); });
            foreach (FileInfo fi in fileInfos)
            {
                listBoxHdwProcessFiles.Items.Add(fi);
            }

            // 选中刚生成的文件
            if (!string.IsNullOrEmpty(_hdwpFilePathName))
            {
                FileInfo fileInfo = fileInfos.Find(fi => fi.FullName == _hdwpFilePathName);
                listBoxHdwProcessFiles.SelectedItem = fileInfo;
                _hdwpFilePathName = ""; // 重置干热风过程文件名称
            }
        }

        private void listBoxHdwProcessFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBox listBox = sender as ListBox;
                FileInfo fileInfo = listBox.SelectedItem as FileInfo;
                if (fileInfo == null)
                    return;

                string filePathName = fileInfo.FullName;

                this.FillCountyColor(filePathName.Replace(".txt", ".pxt"));

                this.ShowHdwProcess(filePathName);
            }
            catch(Exception ex)
            {
                string errMsg = ex.Message;
                throw ex;
            }

            return;
        }

        #endregion

        #region --干热风过程处理
        
        /// <summary>
        /// 处理干热风过程
        /// </summary>
        private void HdwProcess()
        {
            // 1.干热风过程起止时间
            DateTime dtStart = (DateTime)this.datePickerHdwpStart.SelectedDate;
            DateTime dtEnd = (DateTime)this.datePickerHdwpEnd.SelectedDate;

            // 2.干热风日数据搜集
            List<HDWDay> hdwDays = this.HdwDayCollect(dtStart, dtEnd);
            if (hdwDays == null || hdwDays.Count == 0)
            {
                MessageBox.Show("没有干热风数据!");
                return;
            }

            // 3.构造干热风过程站点集合
            List<HDWProcessStation> hdwpStations = new List<HDWProcessStation>();
            foreach (StationInfo si in hdwDays[0].StationInfos)
            {
                HDWProcessStation hdwpStation = new HDWProcessStation();
                hdwpStation.Station = si;
                hdwpStations.Add(hdwpStation);
            }

            // 4.判断干热风过程
            HdwProcessDetermine(hdwDays, ref hdwpStations);


            // 5.输出干热风过程
            HdwProcessOutput(hdwpStations, dtStart, dtEnd);


            // 刷新文件列表
            this.RefreshListBox();

            return;
        }

        /// <summary>
        /// 搜集干热风日数据
        /// </summary>
        /// <param name="dateTimeStart"></param>
        /// <param name="dateTimeEnd"></param>
        /// <returns></returns>
        private List<HDWDay> HdwDayCollect(DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            List<HDWDay> hdwDays = new List<HDWDay>();

            try
            {
                string filePathName = "";
                TimeSpan ts = new TimeSpan(1, 0, 0, 0);

                for (DateTime dt = dateTimeStart; dt <= dateTimeEnd; dt += ts)
                {
                    filePathName = System.IO.Path.Combine(TheApp.DataPath_Hdwd,
                        string.Format("{0}.txt", dt.ToString("yyyyMMdd")));

                    if (!File.Exists(filePathName))
                        continue;

                    IProvider dataProvider = new AxinFileProvider(filePathName);
                    hdwDays.Add(new HDWDay
                    {
                        DateTime = dataProvider.DataInstance.DataInfo.DateTime,
                        StationInfos = (dataProvider.DataInstance as AxinStationFile).StationInfos,
                    });

                }//for (DateTime dt = dateTimeStart; dt <= dateTimeEnd; dt += ts)
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return hdwDays;
        }

        /// <summary>
        /// 干热风过程判断
        /// </summary>
        /// <param name="hdwDays"></param>
        /// <param name="hdwpStations"></param>
        private void HdwProcessDetermine(List<HDWDay> hdwDays, ref List<HDWProcessStation> hdwpStations)
        {
            HDW hdw = new HDW();

            foreach (HDWProcessStation hdwpStaion in hdwpStations)
            {
                Station station = hdwpStaion.Station;       // 取出一个站点
                DateTime dtHdwDayPrv = DateTime.MinValue;   // 上个干热风日日期
                DateTime dtHdwDayCur = DateTime.MinValue;   // 当前干热风日日期

                // 针对一个站点进行处理
                foreach (HDWDay hdwDay in hdwDays)
                {
                    StationInfo stationInfo = hdwDay.StationInfos.Find(si => si.Id == station.Id);
                    if (stationInfo == null)
                        continue;

                    double v = stationInfo.CurrentElementValue;
                    hdw.Value = v;
                    string stemp = hdw.ToString();

                    // 判断干热风过程是否开始(干热风过程起始日必须是轻、中、重干热风日)
                    if (stemp == "无")
                        continue;

                    // 当前干热风日期
                    dtHdwDayCur = hdwDay.DateTime;

                    // 计算时间跨度
                    TimeSpan timeSpan;
                    if (dtHdwDayPrv == DateTime.MinValue)
                    {
                        timeSpan = new TimeSpan(0);    // 没有上个干热风日日期，时间跨度为0天
                    }
                    else
                    {
                        timeSpan = dtHdwDayCur - dtHdwDayPrv;
                    }

                    // 保存当前日期到前一个日期
                    // 用于下一个干热风日比较
                    dtHdwDayPrv = hdwDay.DateTime;

                    // 时间(日)跨度=0，表示是干热风过程的第一个干热风日。
                    // 时间(日)跨度>0，表示干热风过程结束，结束日期是上个干热风日（dtHdwDayPrv）；
                    // 当前干热风日(dtHdwDayCur)是下个干热风过程的起始日期
                    if (timeSpan.Days == 0 || timeSpan.Days > 1)
                        hdwpStaion.Flag = 0;
                    else
                        hdwpStaion.Flag = 1;

                    // 干热风过程
                    HDWProcess hdwp = null;
                    if (hdwpStaion.Flag == 0)
                    {
                        // 是热风过程的首个干热风日则添加站点干热风过程
                        hdwp = new HDWProcess();
                        hdwp.StartDate = hdwDay.DateTime;
                        hdwp.EndDate = hdwDay.DateTime;
                        hdwpStaion.HdwProcesses.Add(hdwp);
                    }
                    else
                    {
                        // 非热风过程的首个干热风日则修改结束日期
                        hdwp = hdwpStaion.HdwProcesses[hdwpStaion.HdwProcesses.Count - 1];
                        hdwp.EndDate = hdwDay.DateTime;
                    }

                    if (string.IsNullOrEmpty(hdwp.GradeString))
                    {
                        hdwp.GradeString = stemp;
                    }
                    else
                    {
                        hdwp.GradeString += ";" + stemp;
                    }

                }// foreach (HDWDay hdwDay in hdwDays)

            }//foreach (HDWProcessStation hdwpStaion in hdwpStations)

            return;
        }

        /// <summary>
        /// 干热风过程输出到文件
        /// </summary>
        /// <param name="hdwpStations"></param>
        /// <param name="dateTimeStart"></param>
        /// <param name="dateTimeEnd"></param>
        private void HdwProcessOutput(List<HDWProcessStation> hdwpStations, DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            StringBuilder sb = new StringBuilder();

            String fileName = string.Format("{0}_{1}.txt", 
                dateTimeStart.ToString("yyyyMMdd"), dateTimeEnd.ToString("yyyyMMdd"));
            string filePathName = System.IO.Path.Combine(TheApp.DataPath_Hdwp, fileName);

            FileStream fs = new FileStream(filePathName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);

            foreach (HDWProcessStation hdwpStation in hdwpStations)
            {
                sb.Clear();
                sb.Append(hdwpStation.Station.Id);
                sb.Append(" ");
                sb.Append(hdwpStation.Station.Name);
                sb.Append("\t");
                sb.Append(hdwpStation.HdwProcessInfo);
                sw.WriteLine(sb.ToString());
            }// foreach (HDWProcessStation hdwpStation in hdwProcessStations)

            sw.Close();
            fs.Close();

            _hdwpFilePathName = filePathName;

            // 
            string fn = System.IO.Path.Combine(TheApp.ConfigPath, "30_StationDhw.txt");
            AxinFileProvider axinFileProvider = new AxinFileProvider(fn);
            AxinStationFile axinStationFile = axinFileProvider.DataInstance as AxinStationFile;
            AxinStationFileDataInfo dataInfo = axinStationFile.DataInfo as AxinStationFileDataInfo;
            List<StationInfo> stationInfos = axinStationFile.StationInfos;

            foreach (StationInfo si in stationInfos)
            {
                HDWProcessStation hdwpStation = hdwpStations.Find(hdwps => hdwps.Station.Id == si.Id);
                if (hdwpStation == null)
                    continue;

                //hdwpStation.
                string g = hdwpStation.HdwProcessGrade;
                HDWLevel l = hdwpStation.HdwProcessLevel;

                si.CurrentElementValue = (int)l;
            }

            String fileName2 = string.Format("{0}_{1}.pxt",
                dateTimeStart.ToString("yyyyMMdd"), dateTimeEnd.ToString("yyyyMMdd"));
            string filePathName2 = System.IO.Path.Combine(TheApp.DataPath_Hdwp, fileName2);

            //_axinFileProvider.DataInstance.DataInfo.ProductCode = 6820;         // 产品代码（用于确定调色板）
            axinFileProvider.DataInstance.DataProcessor.SaveAs(filePathName2);
            // ok,等待显示空间分布图

            return;
        }

        #endregion


        /// <summary>
        /// 填充县区域颜色
        /// </summary>
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
                List<IFeature> features = shapeFile.Features.FindAll(f => f.Id == si.Id);
                foreach (IFeature f in features)
                {
                    System.Drawing.Color clr = palette.GetColor(si.CurrentElementValue, System.Drawing.Color.Green);
                    f.Tag = clr;
                }
            }

            FileInfo fi = new FileInfo(filePathName);

            TheApp.MapFunctions.Palette = palette;
            TheApp.MapFunctions.FilePathName = filePathName;

            TheApp.MapFunctions.Title1 = string.Format("{0}冬小麦干热风过程", TheApp.AreaName);
            TheApp.MapFunctions.Title2 = fi.Name.Replace(".pxt", "");
            TheApp.MapFunctions.Title3 = "河南省气象科学研究所农气中心制作";
            TheApp.MapFunctions.LegendTitle = "图例";

            map.Refresh(true);
        }

        /// <summary>
        /// 显示干热风过程列表
        /// </summary>
        /// <param name="filePathName"></param>
        private void ShowHdwProcess(string filePathName)
        {
            // 装载数据
            List<HDWProcessStation> hdwProcessStations = new List<HDWProcessStation>();

            StreamReader sr = new StreamReader(filePathName, Encoding.Default);
            while (!sr.EndOfStream)
            {
                string strLine = sr.ReadLine();
                if (string.IsNullOrEmpty(strLine))
                    continue;

                HDWProcessStation hdwProcessStation = this.GetHDWProcessStation(strLine);
                if (hdwProcessStation != null)
                    hdwProcessStations.Add(hdwProcessStation);
            }
            sr.Close();

            TheApp.HdwProcessStations = hdwProcessStations;

            // 显示数据
            DockingManager dockManager = TheApp.DockingManager;
            FileInfo fi = new FileInfo(filePathName);

            // 文档面板
            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane == null)
                return;

            // 文档
            List<LayoutDocument> docs = dockManager.Layout.Descendents().OfType<LayoutDocument>().ToList();
            LayoutDocument doc = docs.Find(d => d.Description == "干热风过程");
            if (doc == null)
            {
                doc = new LayoutDocument();
                {
                    doc.Description = "干热风过程";
                    doc.Content = new UC_HdwProcess();
                }
                firstDocumentPane.Children.Add(doc);
            }

            // 设置文档标题
            doc.Title = string.Format("干热风过程 - {0}", fi.Name);

            UC_HdwProcess ucHdwProcess = doc.Content as UC_HdwProcess;
            if (ucHdwProcess != null)
                ucHdwProcess.RefreshDataGrid();

            return;
        }

        /// <summary>
        /// 返回站点的干热风过程(HDWProcessStation)
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        private HDWProcessStation GetHDWProcessStation(string strLine)
        {
            HDWProcessStation hdwProcessStation = new HDWProcessStation();

            string[] ary_strLine = strLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (ary_strLine.Length != 3)
                return null;

            string id = ary_strLine[0];
            string name = ary_strLine[1];
            string processInfo = ary_strLine[2];  // 过程信息

            // 1.Station
            Station station = new Station();
            station.Id = id;
            station.Name = name;

            // 2.HDWProcess
            // 把过程信息处理成过程集合
            // 轻过程[20130511-20130512];无过程[20130519-20130519];轻过程[20140520-20140521];
            //HDWProcess hdwp = new HDWProcess();

            hdwProcessStation.Station = station;
            hdwProcessStation.HdwProcessInfo = processInfo;

            return hdwProcessStation;
        }

    }// end class - UC_HdwProcessFileList
}
