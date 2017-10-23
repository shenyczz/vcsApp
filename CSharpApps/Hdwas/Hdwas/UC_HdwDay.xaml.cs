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
using CSharpKit;

namespace Hdwas
{
    /// <summary>
    /// UC_HdwDay - 站点干热风日数据列表
    /// </summary>
    public partial class UC_HdwDay : UserControl
    {
        public UC_HdwDay()
        {
            InitializeComponent();

            RefreshDataGrid();
            this.dataGridHdwDay.LoadingRow += gridHdwStationList_LoadingRow;
        }

        public void RefreshDataGrid()
        {
            this.dataGridHdwDay.ItemsSource = null;
            this.dataGridHdwDay.ItemsSource = TheApp.StationInfos_Day;
        }

        /// <summary>
        /// OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridHdwStationList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //DataGridRow dataGridRow = e.Row;
            //StationInfo stationInfo = e.Row.DataContext as StationInfo;
            //if (stationInfo == null)
            //    return;

            //if (stationInfo.CurrentElementValue < 20)
            //    dataGridRow.Foreground = Brushes.Green;
            //else if (stationInfo.CurrentElementValue < 60)
            //    dataGridRow.Foreground = Brushes.Yellow;
            //else if (stationInfo.CurrentElementValue < 80)
            //    dataGridRow.Foreground = Brushes.Brown;
            //else
            //    dataGridRow.Foreground = Brushes.Red;

            return;
        }
    }
}
