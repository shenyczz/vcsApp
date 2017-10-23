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
using CSharpKit;
using CSharpKit.Data;
using CSharpKit.Data.Axin;
using CSharpKit.Data.Esri;
using CSharpKit.Devices.Input;
using CSharpKit.Palettes;
using CSharpKit.Vision;
using CSharpKit.Vision.Mapping;
using CSharpKit.Vision.Presentation;
using CSharpKit.Windows.Forms.Controls;

namespace Amrs.Views
{
    /// <summary>
    /// MapView.xaml 的交互逻辑
    /// </summary>
    public partial class MapView : UserControl
    {
        public MapView()
        {
            InitializeComponent();
        }

        #region Map

        public static readonly DependencyProperty MapProperty =
            DependencyProperty.RegisterAttached("Map",
            typeof(IMap),
            typeof(MapView),
            new PropertyMetadata(OnMapChanged));

        private static void OnMapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapView)d).OnMapChanged(e);
        }

        public IMap Map
        {
            get { return (IMap)this.GetValue(MapProperty); }
            set { this.SetValue(MapProperty, value); }
        }

        private void OnMapChanged(DependencyPropertyChangedEventArgs e)
        {
            this.mapControl.Map = this.Map;
            this.mapControl.MouseMove += mapControl_MouseMove;
        }

        private void mapControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            double x = e.X;
            double y = e.Y;
            IMap map = Map;
            map.ViewToWorld(ref x, ref y);
            //if (TheApp.MainWindow != null)
            //{
            //    TheApp.MainWindow.TextBlock2.Text = String.Format("{0},  {1}", x.ToString("F2"), y.ToString("F2"));
            //}
        }

        #endregion

    }
}
