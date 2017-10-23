using System;
using System.Collections;
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
    /// UC_HdwProcess - 干热风过程列表控件
    /// </summary>
    public partial class UC_HdwProcess : UserControl
    {
        public UC_HdwProcess()
        {
            InitializeComponent();
            RefreshDataGrid();
        }
        public void RefreshDataGrid()
        {
            // 设置数据源
            this.dataGridHdwProcess.ItemsSource = null;
            this.dataGridHdwProcess.ItemsSource = TheApp.HdwProcessStations;
        }
    }
}
