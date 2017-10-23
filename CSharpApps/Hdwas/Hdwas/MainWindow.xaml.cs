using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using CSharpKit.Windows;
using CSharpKit.Windows.Anchoring;
using CSharpKit.Windows.Anchoring.Layouts;
using CSharpKit.Windows.Anchoring.Layouts.Serialization;
using CSharpKit.Windows.Anchoring.Themes;
using CSharpKit.Windows.Controls;
using CSharpKit.Windows.Forms;
using CSharpKit.Windows.Forms.Controls;
using Microsoft.Win32;
using Drawing = System.Drawing;
using GdiImage = System.Drawing.Image;
using GdiPoint = System.Drawing.Point;
using GdiSize = System.Drawing.Size;

namespace Hdwas
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        #region Fields

        IProvider _provider;

        public IPalette _Palette;
        public String _Title1 = "";
        public String _Title2 = "";
        public String _Title3 = "";
        public String LegendTitle = "";

        System.Drawing.Bitmap _Image = null;
        System.Drawing.Point _pltPoint;
        System.Drawing.Size _pltSize;

        #endregion

        #region Properties

        //public IProvider MapProvider { get; set; }

        /// <summary>
        /// 用户控件 - 干热风日数据网格
        /// </summary>
        public UC_HdwDay UC_HdwDay { get; set; }

        /// <summary>
        /// 干热风站点列表文档
        /// </summary>
        public LayoutDocument HdwDayDocument { get; set; }

        private String _CurrentFileName = "";
        public String CurrentFileName
        {
            get { return _CurrentFileName; }
            set
            {
                _CurrentFileName = value;
                if (!string.IsNullOrEmpty(_CurrentFileName))
                {
                    if (HdwDayDocument != null)
                    {
                        HdwDayDocument.Title = string.Format("干热风监测数据 - {0}", _CurrentFileName).Replace(".txt", "");
                    }
                }
            }
        }

        MapControl mapControl
        {
            get { return this.ucMapControl.MapControl; }
        }

        #endregion


        #region --Initialize--

        private void Initialize()
        {
            InitializeNormal();
            InitializeConfig();
            InitializeMap();
            InitializeMapData();
        }
        private void InitializeNormal()
        {
            string s = TheApp.MrConfiguration.AreaCode;
            string s1 = System.Configuration.ConfigurationManager.AppSettings["AreaCode"];

            TheApp.AreaCode = "410000";
            TheApp.AreaCode = "411000";
            TheApp.AreaCode = "411700";
            TheApp.AreaCode = TheApp.MrConfiguration.AreaCode;

            this.Title = string.Format("{0}冬小麦干热风动态监测评估系统", TheApp.AreaName);

            TheApp.DockingManager = this.dockManager;
        }
        private void InitializeConfig()
        {
            // 作物发育期
            string fn = System.IO.Path.Combine(TheApp.ConfigPath, "CropGrowthPeriod.xml");
            TheApp.CropGrowthPeriods = new CropGrowthPeriodCollection();
            TheApp.CropGrowthPeriods.FromXElement(XElement.Load(fn));
        }

        private void InitializeMap()
        {
            IMap map = new Map();

            TheApp.Map = map;
            this.mapControl.Map = map;

            this.mapControl.MouseMove += mapControl_MouseMove;

            map.MapTool = MapTool.MapController.Cancel;
            map.Rendered += map_Rendered;
        }

        void mapControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            double x = e.X;
            double y = e.Y;
            IMap map = TheApp.Map;
            map.ViewToWorld(ref x, ref y);
            this.TextBlock2.Text = String.Format("{0},  {1}", x.ToString("F2"), y.ToString("F2"));
        }

        private void InitializeMapData()
        {
            IMap map = TheApp.Map;

            string areaCode = TheApp.AreaCode;


            #region 图元图层

            try
            {
                // 添加图元图层
                IProvider provider = null;
                IRenderer renderer = new WfmGeometryVisionRenderer();
                IVision vision = GeometryVision.CreateGeometryVision("GeometryVision", provider, renderer);
                vision.Comment = "图元图层";
                vision.IsAllowDeleted = false;
                vision.RenderPriority = RenderPriority.Topmost;
                map.LayerManager.PrimiviteLayer = new Layer(vision);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            #endregion

            #region 县界

            try
            {
                // 河南省县界
                String fileName = System.IO.Path.Combine(TheApp.StartupPath, string.Format("System\\Map\\{0}\\Boundary.shp", areaCode));
                //String fileName = System.IO.Path.Combine(TheApp.StartupPath, "System\\Map\\411000\\Boundary.shp");
                IProvider provider = new ShapeFileProvider(fileName);
                IRenderer renderer = new WfmShapeVisionRenderer();
                IVision vision = new ShapeVision("县界", provider, renderer);
                vision.IsAllowDeleted = false;
                //vision.Opacity = 0;
                vision.Transparency = 0;
                vision.IsFill = true;
                //(vision as ShapeVision).IsClipPathData = true;
                ILayer layer = new Layer(TheApp.MapLayerId_County, vision);
                map.LayerManager.Add(layer);

                // 保存提供者
                _provider = provider;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            #endregion

            #region 县标注

            try
            {
                // 河南省县标注
                String fileName = System.IO.Path.Combine(TheApp.StartupPath, string.Format("System\\Map\\{0}\\Label.shp", areaCode));
                //String fileName = System.IO.Path.Combine(TheApp.StartupPath, "System\\Map\\411000\\Label.shp");
                IProvider provider = new ShapeFileProvider(fileName);
                IRenderer renderer = new WfmShapeVisionRenderer();
                IVision vision = new ShapeVision("县标注", provider, renderer);
                vision.FontSize = 8;
                (vision as VisionBase).Foreground = System.Drawing.Color.Blue;
                map.LayerManager.Add(new Layer(vision));
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            #endregion

        }//InitializeMapData

        #endregion

        private void map_Rendered(object sender, EventArgs e)
        {
            IMap map = sender as IMap;
            TheApp.MapFunctions.OnMapRendered(map);
            return;
        }


        #region --Layout--

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadLayout();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {

        }
        private void Window_Closed(object sender, EventArgs e)
        {
            SaveLayout();
            SaveConfig();
        }

        private void SaveLayout()
        {
            try
            {
                var serializer = new XmlLayoutSerializer(dockManager);
                using (var stream = new StreamWriter(string.Format(@".\Anchor.xml")))
                {
                    serializer.Serialize(stream);
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }
        private void LoadLayout()
        {
            try
            {
                var currentContentsList = dockManager.Layout.Descendents().OfType<LayoutContent>().Where(c => c.ContentId != null).ToArray();

                var serializer = new XmlLayoutSerializer(dockManager);
                using (var stream = new StreamReader(string.Format(@".\Anchor.xml")))
                {
                    serializer.Deserialize(stream);
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }

        void SaveConfig()
        {
            // 发育期配置文件
            string fileName = System.IO.Path.Combine(TheApp.ConfigPath, "CropGrowthPeriod.xml");
            TheApp.CropGrowthPeriods.ToXElement().Save(fileName);
        }

        #endregion

        #region --Button_Click--

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ButtonBase button = sender as ButtonBase;
            switch (button.Name)
            {
                case "buttonReset": // 刷新
                    Reset();
                    break;

                case "buttonGis": // 地理信息
                    ShowMap();
                    break;

                case "buttonCrop":  // 作物发育期
                    CropEdit();
                    break;

                case "buttonHdwd":  // 干热风监测
                    hdwDPA("干热风监测");
                    break;

                case "buttonHdwp":  // 干热风过程
                    hdwDPA("干热风过程");
                    break;

                case "buttonHdwa":  // 干热风评估
                    hdwDPA("干热风评估");
                    break;

                case "buttonRsimg":  // 遥感图数据
                    hdwDPA("遥感图数据");
                    break;

                case "buttonSave":  // 
                    SaveImage();
                    break;
            }
        }

        #endregion

        private void Reset()
        {
            TheApp.Map.Reset();
            TheApp.Map.Refresh(true);
        }

        private void ShowMap()
        {
            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();

            if (firstDocumentPane != null)
            {
                List<LayoutDocument> docs = dockManager.Layout.Descendents().OfType<LayoutDocument>().ToList();
                LayoutDocument doc = docs.Find(d => d.ContentId == "wfmMapDocument");
                if (doc != null)
                {
                    doc.IsActive = true;
                }
            }
        }

        private void SaveImage()
        {
            ShowMap();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = TheApp.ImagePath;
            string[] sa = _CurrentFileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (sa.Length > 0)
            {
                sfd.FileName = sa[0];
            }
            else
            {
                sfd.FileName = "未命名";
            }
            sfd.Filter = "PNG图片|*.png|JPG图片|*.jpg";
            sfd.AddExtension = true;

            if ((bool)sfd.ShowDialog())
            {
                SaveImage(sfd.FileName);
            }
        }
        private void SaveImage(string filePathName)
        {
            IMap map = TheApp.Map;

            IMapControl mapControl = map.Container as IMapControl;
            if (mapControl == null)
                return;

            int w = (int)mapControl.GetWidth();
            int h = (int)mapControl.GetHeight();

            RECT rc = new RECT(w, h);

            IntPtr ih = TheApp.CaptureRect(mapControl.Handle, ref rc);
            System.Drawing.Bitmap bmp = System.Drawing.Bitmap.FromHbitmap(ih);
            bmp.Save(filePathName);
        }


        /// <summary>
        /// 作物发育期编辑窗口
        /// </summary>
        private void CropEdit()
        {
            // 文档面板
            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane == null)
                return;

            // 查找文档
            List<LayoutDocument> docs = dockManager.Layout.Descendents().OfType<LayoutDocument>().ToList();
            LayoutDocument doc = docs.Find(d => d.Description == "CGPEDIT");
            if (doc == null)
            {
                doc = new LayoutDocument();
                doc.Description = "CGPEDIT";
                doc.Content = new UC_CgpEdit();
                firstDocumentPane.Children.Add(doc);
            }

            // 设置文档标题
            //doc.Title = string.Format("干热风评估 - {0}", fi.Name);
            doc.Title = string.Format("作物发育期");
            doc.IsActive = true;

            UC_CgpEdit ucCgpEdit = doc.Content as UC_CgpEdit;
            if (ucCgpEdit != null)
                ucCgpEdit.RefreshDataGrid();

            return;
        }

        private void hdwDPA(string contentId)
        {
            var firstAnchorablePane = dockManager.Layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault();
            if (firstAnchorablePane != null)
            {
                List<LayoutAnchorable> anchors = dockManager.Layout.Descendents().OfType<LayoutAnchorable>().ToList();
                LayoutAnchorable anchor = anchors.Find(a => a.ContentId == contentId);
                if (anchor != null)
                {
                    anchor.IsActive = true;
                }
            }

            return;
        }


    }// end class - MainWindow
}
