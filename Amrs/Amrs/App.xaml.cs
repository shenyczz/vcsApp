using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Amrs
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //---------------------------------------------
            // 设置应用程序路径
            CSharpKit.KitHelper.AppPath = TheApp.StartupPath;

            // 启动屏幕
            //SplashScreen ss = new SplashScreen("Images/Splash.bmp");
            //ss.Show(false, true);
            //ss.Close(TimeSpan.FromSeconds(5));
            //---------------------------------------------
        }
    }
}
