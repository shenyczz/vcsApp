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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hdwas
{
    /// <summary>
    /// UC_CgpEdit.xaml 的交互逻辑
    /// </summary>
    public partial class UC_CgpEdit : UserControl
    {
        public UC_CgpEdit()
        {
            InitializeComponent();
        }

        public void RefreshDataGrid()
        {
            this.dataGridCropGrowthPeriod.ItemsSource = null;
            if (TheApp.MrConfiguration.AreaCode == "410000")
            {
                this.dataGridCropGrowthPeriod.ItemsSource = TheApp.CropGrowthPeriods;
            }
            else
            {
                this.dataGridCropGrowthPeriod.ItemsSource =
                    TheApp.CropGrowthPeriods.FindAll(c => c.AreaCode == TheApp.MrConfiguration.AreaCode);
            }
        }
        void Save()
        {
            string fileName = System.IO.Path.Combine(TheApp.ConfigPath, "CropGrowthPeriod.xml");
            SaveAs(fileName);
        }
        void SaveAs(string fileName)
        {
            TheApp.CropGrowthPeriods.ToXElement().Save(fileName);
        }

        void Load(string fileName)
        {
            TheApp.CropGrowthPeriods.FromXElement(System.Xml.Linq.XElement.Load(fileName));
            this.RefreshDataGrid();
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.InitialDirectory = TheApp.ConfigPath;
            ofd.Filter = "配置文件|*.xml";

            if ((bool)ofd.ShowDialog())
            {
                Load(ofd.FileName);
            }
        }

        private void buttonSaveAs_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.InitialDirectory = TheApp.ConfigPath;
            sfd.FileName = "未命名";
            sfd.Filter = "配置文件|*.xml";
            sfd.AddExtension = true;

            if ((bool)sfd.ShowDialog())
            {
                SaveAs(sfd.FileName);
            }

        }
    }
}
