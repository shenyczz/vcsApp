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

            App.Workspace = Workspace.Instance;
            this.DataContext = Workspace.Instance;

            Console.WriteLine("MainWindow()");
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
