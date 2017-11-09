using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mescp.Views
{
    /// <summary>
    /// PropertyView.xaml 的交互逻辑
    /// </summary>
    public partial class PropertyView : System.Windows.Controls.UserControl
    {
        public PropertyView()
        {
            InitializeComponent();

            //this.propertyGrid.HelpVisible = false;  //帮助窗口
            //propertyGrid.BackColor = System.Drawing.Color.WhiteSmoke;
            //propertyGrid.CategoryForeColor = System.Drawing.Color.Blue;  // 类别标题的文本颜色
            //propertyGrid.CommandsBackColor = System.Drawing.Color.Red;
            //propertyGrid.ToolbarVisible = false;
            propertyGrid.PropertySort = PropertySort.Categorized;
        }

        #region Properties

        public static readonly DependencyProperty PropertiesProperty =
            DependencyProperty.RegisterAttached("Properties",
            typeof(Object),
            typeof(PropertyView),
            new PropertyMetadata(PropertiesChangedCallback));

        private static void PropertiesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertyView)d).OnPropertiesChanged(e);
        }
        public Object Properties
        {
            get { return (Object)this.GetValue(PropertiesProperty); }
            set { this.SetValue(PropertiesProperty, value); }
        }
        private void OnPropertiesChanged(DependencyPropertyChangedEventArgs e)
        {
            this.propertyGrid.SelectedObject = Properties;
            this.propertyGrid.Refresh();
        }

        #endregion

    }
}
