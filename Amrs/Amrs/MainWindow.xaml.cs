using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using Amrs.Core;
using Amrs.Modis;
using Amrs.ViewModels;
using CSharpKit;
using CSharpKit.Data;
using CSharpKit.Windows.Anchoring.Layouts;
using CSharpKit.Windows.Anchoring.Layouts.Serialization;

namespace Amrs
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            TheApp.Workspace.Initialize();
            this.DataContext = TheApp.Workspace;


            this.Title = "卫星遥感信息分析处理与应用系统";

            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
            //this.Closed += MainWindow_Closed;
        }

        #region --Layout--

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLayout();
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //throw new NotImplementedException();
        }


        void MainWindow_Closed(object sender, EventArgs e)
        {
            SaveLayout();
        }

        private void SaveLayout()
        {
            try
            {
                var serializer = new XmlLayoutSerializer(dockManager);
                using (var stream = new StreamWriter(string.Format(@".\Anchor.xml")))
                {
                    serializer.Serialize(stream);
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }
        private void LoadLayout()
        {
            try
            {
                var currentContentsList = dockManager.Layout.Descendents().OfType<LayoutContent>().Where(c => c.ContentId != null).ToArray();

                var serializer = new XmlLayoutSerializer(dockManager);
                using (var stream = new StreamReader(string.Format(@".\Anchor.xml")))
                {
                    serializer.Deserialize(stream);
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }

        #endregion

    }
}
