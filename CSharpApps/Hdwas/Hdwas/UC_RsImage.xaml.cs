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
    /// UC_RsImage.xaml 的交互逻辑
    /// </summary>
    public partial class UC_RsImage : UserControl
    {
        public UC_RsImage()
        {
            InitializeComponent();
            this.SizeChanged += UC_RsImage_SizeChanged;
        }

        void UC_RsImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            image.Width = this.ActualWidth;
            image.Height = this.ActualHeight;
        }

        string _FilePathName = "";
        public string FilePathName
        {
            get { return _FilePathName; }
            set
            {
                _FilePathName = value;
                showImage();
            }
        }

        void showImage()
        {
            image.Width = this.ActualWidth;
            image.Height = this.ActualHeight;

            Uri uri = new Uri(_FilePathName);
            BitmapImage bmi = new BitmapImage(uri);
            image.Source = bmi;
        }
    }
}
