using System;
using System.Windows;
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

            // 闪屏
            SplashScreen splashScreen = new SplashScreen("Assets/splash.bmp");
            //splashScreen.Show(false, true);
            splashScreen.Close(TimeSpan.FromSeconds(5));

            // 初始化工作空间
            //App.Workspace = Mescp.ViewModels.Workspace.Instance;

            base.OnStartup(e);
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

        public static Workspace Workspace { get; set; }

        [System.Runtime.InteropServices.DllImport("Libs/KSuiteCore100u.dll")]
        public static extern IntPtr CaptureRect(IntPtr handle, ref CSharpKit.Win32.RECT rect);
    }
}
