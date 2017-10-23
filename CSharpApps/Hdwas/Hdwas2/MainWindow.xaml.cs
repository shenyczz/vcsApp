using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using CSharpKit.Windows.Anchoring;

namespace Hdwas
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Workspace.Instance;
        }





        #region OnInitialized

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            CustomInitialize();
        }

        private void CustomInitialize()
        {
            Console.WriteLine("Initialize");
            CustomInitialize_Normal();
            CustomInitialize_Config();
        }

        private void CustomInitialize_Normal()
        {
            this.Title = "干热风监测";
            this.WindowState = WindowState.Maximized;
        }

        private void CustomInitialize_Config()
        {
            // 作物发育期
            string fn = System.IO.Path.Combine(TheApp.ConfigPath, "CropGrowthPeriod.xml");
            TheApp.CropGrowthPeriods = new CropGrowthPeriodCollection();
            TheApp.CropGrowthPeriods.FromXElement(XElement.Load(fn));
        }

        #endregion

        public DockingManager DockingManager
        {
            get { return this.dockManager; }
        }

        public TextBlock TextBlock2
        {
            get { return this.t2; } 
        }
    }
}
