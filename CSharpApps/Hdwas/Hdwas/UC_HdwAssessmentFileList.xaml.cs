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
using System.Xml.Linq;
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
    /// UC_HdwAssessmentFileList.xaml 的交互逻辑
    /// </summary>
    public partial class UC_HdwAssessmentFileList : UserControl
    {
        public UC_HdwAssessmentFileList()
        {
            InitializeComponent();
            Initialize();
        }

        #region Fields

        private string _hdwaFilePathName = "";

        private AxinFileProvider _axinFileProvider;
        private CropGrowthPeriodCollection _cropGrowthPeriods;

        #endregion

        private void Initialize()
        {
            this.datePickerHdwaStart.SelectedDate = DateTime.Now;
            this.datePickerHdwaEnd.SelectedDate = DateTime.Now + new TimeSpan(1, 0, 0, 0);

            listBoxHdwAssessFiles.SelectionChanged += listBoxHdwAssessFiles_SelectionChanged;
            RefreshListBox();
        }

        #region --ListBox--
        
        private void RefreshListBox()
        {
            // 清除列表显示
            listBoxHdwAssessFiles.Items.Clear();

            // 添加指定文件夹里的文件
            string filePath = System.IO.Path.Combine(TheApp.DataPath_Hdwa);
            DirectoryInfo dir = new DirectoryInfo(filePath);
            List<FileInfo> fileInfos = dir.GetFiles("*.txt").ToList();
            fileInfos.Sort((x, y) => { return -x.Name.CompareTo(y.Name); });
            foreach (FileInfo fi in fileInfos)
            {
                listBoxHdwAssessFiles.Items.Add(fi);
            }

            // 选中刚生成的文件
            if (!string.IsNullOrEmpty(_hdwaFilePathName))
            {
                FileInfo fileInfo = fileInfos.Find(fi => fi.FullName == _hdwaFilePathName);
                _hdwaFilePathName = ""; // 重置干热风过程文件名称
                if (fileInfo != null)
                    listBoxHdwAssessFiles.SelectedItem = fileInfo;  // 激发 listBoxHdwAssessFiles_SelectionChanged 事件
            }
        }

        private void listBoxHdwAssessFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

                // 显示评估数据
                this.ShowHdwAssess(filePathName);
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
            switch (button.Name)
            {
                case "buttonHdwAssess": // 干热风评估
                    HdwAssess();
                    break;

                case "buttonSaveImage":
                    //SaveImage();
                    break;
            }
        }

        #region --干热风灾害评估--

        /// <summary>
        /// 干热风评估
        /// </summary>
        private void HdwAssess()
        {
            // 1.干热风过程起止时间
            DateTime dateTimeStart = (DateTime)this.datePickerHdwaStart.SelectedDate;
            DateTime dateTimeEnd = (DateTime)this.datePickerHdwaEnd.SelectedDate;

            // 2.干热风日数据搜集
            List<HDWDay> hdwDays = this.HdwDayCollect(dateTimeStart, dateTimeEnd);
            if (hdwDays == null || hdwDays.Count == 0)
            {
                MessageBox.Show("没有干热风数据!");
                return;
            }

            // 3.构造干热风过程站点集合
            List<HDWProcessStation> hdwpStations = new List<HDWProcessStation>();
            foreach (StationInfo si in hdwDays[0].StationInfos)
            {
                HDWProcessStation hdwpSta = new HDWProcessStation();
                hdwpSta.Station = si;
                hdwpStations.Add(hdwpSta);
            }

            // 4.判断干热风过程
            this.HdwProcessDetermine(hdwDays, ref hdwpStations);

            // 5.作物发育期
            string fn2 = System.IO.Path.Combine(TheApp.ConfigPath, "CropGrowthPeriod.xml");
            _cropGrowthPeriods = new CropGrowthPeriodCollection();
            _cropGrowthPeriods.FromXElement(XElement.Load(fn2));

            // 6.站点配置文件
            string fn = System.IO.Path.Combine(TheApp.ConfigPath, "30_StationDhw.txt");
            _axinFileProvider = new AxinFileProvider(fn);
            AxinStationFile axinStationFile = _axinFileProvider.DataInstance as AxinStationFile;
            AxinStationFileDataInfo dataInfo = axinStationFile.DataInfo as AxinStationFileDataInfo;
            List<StationInfo> stationInfos = axinStationFile.StationInfos;

            // 6.根据干热风过程、作物发育期、灾害标准进行灾害损失评估
            //   HDWDay 减产率：
            //      灌浆中期 -> 轻（0-1）、中（1.5-2.5）
            //      灌浆后期 -> 轻（0-1）、中（1.5-3.0）
            //   HDWPro 减产率：
            //      灌浆中期 -> 轻（0-5）、中（5-10）、重（>10）
            //      灌浆后期 -> 轻（0-3）、中（3-15）、重（>15）
            foreach (StationInfo si in stationInfos)
            {
                HDWProcessStation hdwpStation = hdwpStations.Find(o => o.Station.Id == si.Id);
                foreach (HDWProcess hp in hdwpStation.HdwProcesses)
                {
                    DateTime t0 = hp.StartDate;
                    DateTime t1 = hp.EndDate;

                    if (hp.Grade == "无")
                    {
                        // 无过程
                        string gs = hp.GradeString; // 
                        si.CurrentElementValue = 2.0;
                    }
                    else
                    {
                        // 按最重干热风过程评估灾害
                        if (hp.Grade == "轻")    // 轻过程
                        {
                            // 根据站点、起止日期判断是灌浆中期或是灌浆后期
                            // ?? 干热风过程所处的发育期如何判断？
                            si.CurrentElementValue = 2.5;
                        }
                        if (hp.Grade == "中")    // 中过程
                        {
                            si.CurrentElementValue = 7.8;
                        }
                        if (hp.Grade == "重")    // 轻重程
                        {
                            si.CurrentElementValue = 12.0;
                        }
                    }
                }
            }

            // 输出结果
            String fileName = string.Format("{0}_{1}.txt", dateTimeStart.ToString("yyyyMMdd"), dateTimeEnd.ToString("yyyyMMdd"));
            string filePathName = System.IO.Path.Combine(TheApp.DataPath_Hdwa, fileName);

            _axinFileProvider.DataInstance.DataInfo.ProductCode = 6820;         // 产品代码（用于确定调色板）
            _axinFileProvider.DataInstance.DataProcessor.SaveAs(filePathName);

            // 刷新列表框
            _hdwaFilePathName = filePathName;
            RefreshListBox();

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
                TimeSpan ts = new TimeSpan(1, 0, 0, 0); // 间隔一天

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
            catch (Exception ex)
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
                    string stemp = hdw.ToString();  // 无、轻、中、重

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

                    // 构造干热风字符串(轻;中;重)
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
        /// 显示评估数据的空间分布
        /// </summary>
        /// <param name="filePathName"></param>
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

            // 标题、图例
            FileInfo fi = new FileInfo(filePathName);

            TheApp.MapFunctions.Title1 = string.Format("{0}冬小麦干热风灾害评估", TheApp.AreaName);
            TheApp.MapFunctions.Title2 = fi.Name.Replace(".txt", "");
            TheApp.MapFunctions.Title3 = "河南省气象科学研究所农气中心制作";
            TheApp.MapFunctions.LegendTitle = "减产率(%)";

            TheApp.MapFunctions.FilePathName = filePathName;

            TheApp.MapFunctions.Palette = palette;

            map.Refresh(true);
        }

        /// <summary>
        /// 显示干热风评估数据
        /// </summary>
        /// <param name="filePathName"></param>
        private void ShowHdwAssess(string filePathName)
        {
            DockingManager dockManager = TheApp.DockingManager;

            AxinFileProvider axinFileProvider = new AxinFileProvider(filePathName);
            AxinStationFile axinStationFile = axinFileProvider.DataInstance as AxinStationFile;
            AxinStationFileDataInfo dataInfo = axinStationFile.DataInfo as AxinStationFileDataInfo;
            List<StationInfo> stationInfos = axinStationFile.StationInfos;

            TheApp.StationInfos_Assess = stationInfos;

            FileInfo fi = new FileInfo(filePathName);

            // 文档面板
            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane == null)
                return;

            // 文档
            List<LayoutDocument> docs = dockManager.Layout.Descendents().OfType<LayoutDocument>().ToList();
            LayoutDocument doc = docs.Find(d => d.Description == "干热风评估");
            if (doc == null)
            {
                doc = new LayoutDocument();
                {
                    doc.Description = "干热风评估";
                    doc.Content = new UC_HdwAssessment();
                }
                firstDocumentPane.Children.Add(doc);
            }

            // 设置文档标题
            doc.Title = string.Format("干热风评估 - {0}", fi.Name);

            //doc.IsActive = true;
            UC_HdwAssessment ucHdwAssessment = doc.Content as UC_HdwAssessment;
            if (ucHdwAssessment != null)
                ucHdwAssessment.RefreshDataGrid();

            return;
        }

        #endregion

    }// end class - UC_HdwAssessmentFileList
}
