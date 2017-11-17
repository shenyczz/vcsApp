using System;
using System.Windows.Controls.Ribbon;
using Mescp.ViewModels;

namespace Mescp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            //Assembly a = Assembly.GetExecutingAssembly();
            //string s = a.Location;
            //FileInfo fi = new FileInfo(s);
            //DirectoryInfo di = fi.Directory;
            //string fn = di.FullName;

            this.DataContext = App.Workspace;
            Console.WriteLine("MainWindow()");

            //_fillColor = (Color)feature.Tag;
            System.Drawing.Color c = (System.Drawing.Color)System.Drawing.Color.Transparent;
#pragma warning disable CS0183 // "is" 表达式的给定表达式始终是所提供的类型
            bool b = c is System.Drawing.Color;
#pragma warning restore CS0183 // "is" 表达式的给定表达式始终是所提供的类型

        }

    }
}




/*
void testSQLite()
{
    //Data Source = filename; Version = 3;
    string cnnString = @"Data Source = Config\Mescp.db; Version = 3;";

    try
    {
        DbConnection cnn = new SQLiteConnection(cnnString);
        cnn.Open();

        DbCommand cmd = cnn.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "select * from MaizeCultivars";

        DbDataReader dr = cmd.ExecuteReader();
    }
    catch (Exception ex)
    {
        string msg = ex.Message;
        throw;
    }
}
*/
