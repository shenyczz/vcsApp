﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using CSharpKit;
using CSharpKit.Data;
using CSharpKit.Data.Esri;
using CSharpKit.Vision;
using CSharpKit.Vision.Mapping;
using CSharpKit.Vision.Presentation;
using CSharpKit.Windows;
using CSharpKit.Windows.Anchoring.Themes;

using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using Mescp.ViewModels;
using System.Reflection;

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
