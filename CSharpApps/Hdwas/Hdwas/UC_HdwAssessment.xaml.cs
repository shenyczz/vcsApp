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
    /// UC_HdwAssessment - 干热风评估
    /// </summary>
    public partial class UC_HdwAssessment : UserControl
    {
        public UC_HdwAssessment()
        {
            InitializeComponent();
            RefreshDataGrid();
        }

        public void RefreshDataGrid()
        {
            // 设置数据源
            this.dataGridHdwAssess.ItemsSource = null;
            this.dataGridHdwAssess.ItemsSource = TheApp.StationInfos_Assess;
        }

    }
}
