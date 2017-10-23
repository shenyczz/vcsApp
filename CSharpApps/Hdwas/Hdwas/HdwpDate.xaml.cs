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

namespace Hdwas
{
    /// <summary>
    /// HwdpDate.xaml 的交互逻辑
    /// </summary>
    public partial class HwdpDate : Window
    {
        public HwdpDate()
        {
            InitializeComponent();
            Initialize();
        }

        public DatePicker DatePickerStart
        {
            get { return datePickerStart; }
        }
        public DatePicker DatePickerEnd
        {
            get { return datePickerEnd; }
        }

        void Initialize()
        {
            this.datePickerStart.SelectedDate = DateTime.Now;
            this.datePickerEnd.SelectedDate = DateTime.Now;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            this.Close();
        }
    }
}
