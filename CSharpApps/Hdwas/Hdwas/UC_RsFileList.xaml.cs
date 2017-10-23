using System;
using System.Collections.Generic;
using System.IO;
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
using CSharpKit.Windows.Anchoring;
using CSharpKit.Windows.Anchoring.Layouts;

namespace Hdwas
{
    /// <summary>
    /// UC_RsFileList.xaml 的交互逻辑
    /// </summary>
    public partial class UC_RsFileList : UserControl
    {
        public UC_RsFileList()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            listBoxRsFiles.SelectionChanged += listBoxRsFiles_SelectionChanged;
            RefreshListBox();
        }

        private void RefreshListBox()
        {
            // 清除列表显示
            listBoxRsFiles.Items.Clear();

            // 添加指定文件夹里的文件
            string filePath = System.IO.Path.Combine(TheApp.RsImagePath);
            DirectoryInfo dir = new DirectoryInfo(filePath);
            List<FileInfo> fileInfos = dir.GetFiles("*.*").ToList();
            fileInfos.Sort((x, y) => { return -x.Name.CompareTo(y.Name); });
            foreach (FileInfo fi in fileInfos)
            {
                listBoxRsFiles.Items.Add(fi);
            }

            // 选中刚生成的文件
            //if (!string.IsNullOrEmpty(_hdwaFilePathName))
            //{
            //    FileInfo fileInfo = fileInfos.Find(fi => fi.FullName == _hdwaFilePathName);
            //    _hdwaFilePathName = ""; // 重置干热风过程文件名称
            //    if (fileInfo != null)
            //    {
            //        // 激发 listBoxHdwAssessFiles_SelectionChanged 事件
            //        listBoxHdwAssessFiles.SelectedItem = fileInfo;
            //    }
            //}
        }

        private void listBoxRsFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBox listBox = sender as ListBox;
                FileInfo fileInfo = listBox.SelectedItem as FileInfo;
                if (fileInfo == null)
                    return;

                string filePathName = fileInfo.FullName;
                // 显示评估数据
                this.ShowRsImage(filePathName);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                //throw ex;
            }
        }

        private void ShowRsImage(string filePathName)
        {
            DockingManager dockManager = TheApp.DockingManager;

            // 文档面板
            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane == null)
                return;

            // 文档
            List<LayoutDocument> docs = dockManager.Layout.Descendents().OfType<LayoutDocument>().ToList();
            LayoutDocument doc = docs.Find(d => d.Description == "遥感图像");
            if (doc == null)
            {
                doc = new LayoutDocument();
                {
                    doc.Description = "遥感图像";
                    doc.Content = new UC_RsImage();
                }
                firstDocumentPane.Children.Add(doc);
            }

            // 设置文档标题
            FileInfo fi = new FileInfo(filePathName);
            doc.Title = string.Format("遥感图像 - {0}", fi.Name);
            doc.IsActive = true;

            UC_RsImage ucRsImage = doc.Content as UC_RsImage;
            if (ucRsImage != null)
                ucRsImage.FilePathName = filePathName;
            //    ucRsImage.RefreshDataGrid();

            /*

            //doc.IsActive = true;
            UC_HdwAssessment ucHdwAssessment = doc.Content as UC_HdwAssessment;
            if (ucHdwAssessment != null)
                ucHdwAssessment.RefreshDataGrid();

            return;
             */
        }
    }
}
