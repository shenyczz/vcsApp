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
using CSharpKit.Windows.Forms.Controls;

namespace Hdwas
{
    /// <summary>
    /// UC_MapControl.xaml 的交互逻辑
    /// </summary>
    public partial class UC_MapControl : UserControl
    {
        public UC_MapControl()
        {
            InitializeComponent();

            this.LostFocus += UC_MapControl_LostFocus;
        }

        void UC_MapControl_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        public MapControl MapControl
        {
            get { return this.mapControl; }
        }
    }
}
