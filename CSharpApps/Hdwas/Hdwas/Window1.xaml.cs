using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Hdwas
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            this.Loaded += Window1_Loaded;
            this.Closed += Window1_Closed;

            Initialize();
        }

        void Initialize()
        {
            this.ShowInTaskbar = false;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            //this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            //this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
        }


        void Window1_Closed(object sender, EventArgs e)
        {
            Save();
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        //CropGrowthPeriodCollection _cropGrowthPeriods;

        void LoadData()
        {
            //string fn2 = System.IO.Path.Combine(TheApp.ConfigPath, "CropGrowthPeriod-00.xml");
            //_cropGrowthPeriods = new CropGrowthPeriodCollection();
            //_cropGrowthPeriods.FromXElement(XElement.Load(fn2));
            //_cropGrowthPeriods = TheApp.CropGrowthPeriods;

            this.dataGridCropGrowthPeriod.ItemsSource = TheApp.CropGrowthPeriods;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }
        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        void Save()
        {
            string fileName = System.IO.Path.Combine(TheApp.ConfigPath, "CropGrowthPeriod.xml");
            TheApp.CropGrowthPeriods.ToXElement().Save(fileName);
        }
    }
}
