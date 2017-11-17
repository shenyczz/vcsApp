using System;
using System.Windows;
using CSharpKit;
using CSharpKit.Vision.Mapping;
using Mescp.ViewModels;

namespace Mescp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Console.WriteLine("OnStartup");
            base.OnStartup(e);

            // 闪屏
            SplashScreen splashScreen = new SplashScreen("Assets/splash.bmp");
            //splashScreen.Show(false, true);
            splashScreen.Close(TimeSpan.FromSeconds(5));

            //设置应用程序路径
            KitHelper.AppPath = App.StartupPath;
        }

        #region Path

        public static String StartupPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }
        public static String MapPath
        {
            get { return System.IO.Path.Combine(StartupPath, "Map"); }
        }
        public static String ConfigPath
        {
            get { return System.IO.Path.Combine(StartupPath, "Config"); }
        }
        public static String ImagePath
        {
            get { return System.IO.Path.Combine(StartupPath, "Image"); }

        }
        public static String OutputPath
        {
            get { return System.IO.Path.Combine(StartupPath, "Output"); }
        }

        #endregion

        public static IMap Map { get; set; }

        public static Workspace Workspace { get { return Workspace.Instance; } }

        [System.Runtime.InteropServices.DllImport("Libs/KSuiteCore100u.dll")]
        public static extern IntPtr CaptureRect(IntPtr handle, ref CSharpKit.Win32.RECT rect);
    }
}
