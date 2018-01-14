using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharpKit;
using CSharpKit.Data.Axin;
using CSharpKit.Maths.Interpolation;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Test_by_df();
            Test_misc();
        }





        // 降水
        public void Test_r()
        {
            //MessageBox.Show("AppHelper.Test()");

            //string path = @"D:\Temp\区划\HA\河南省气候标准值降水数据";
            string path = @"d:\Users\shenyc\Documents\知天科技\区划\HA\河南省气候标准值数据-降水";
            string filePath = System.IO.Path.Combine(path, "");

            string f3 = "";
            string f4 = "";

            int m = 1;
            for (m = 1; m <= 12; m++)
            {
                f3 = System.IO.Path.Combine(path, string.Format("{0}月总降水量.txt", m));
                f4 = System.IO.Path.Combine(path, string.Format("{0}月总降水量.asc", m));

                this.Cz(f3, f4);
            }

            for (m = 1; m <= 4; m++)
            {
                string j = "春";
                if (m == 1) j = "春";
                if (m == 2) j = "夏";
                if (m == 3) j = "秋";
                if (m == 4) j = "冬";

                f3 = System.IO.Path.Combine(path, string.Format("{0}季总降水量.txt", j));
                f4 = System.IO.Path.Combine(path, string.Format("{0}季总降水量.asc", j));
                this.Cz(f3, f4);
            }

            {
                f3 = System.IO.Path.Combine(path, string.Format("年降水量.txt"));
                f4 = System.IO.Path.Combine(path, string.Format("年降水量.asc"));
                this.Cz(f3, f4);
            }


            MessageBox.Show("OK");

            return;

        }

        // 日照
        public void Test_s()
        {
            //MessageBox.Show("AppHelper.Test()");

            //string path = @"D:\Temp\区划\HA\河南省气候标准值降水数据";
            string path = @"d:\Users\shenyc\Documents\知天科技\区划\HA\河南省气候标准值数据-日照";
            string filePath = System.IO.Path.Combine(path, "");

            string f3 = "";
            string f4 = "";

            {
                f3 = System.IO.Path.Combine(path, string.Format("年日照时数.txt"));
                f4 = System.IO.Path.Combine(path, string.Format("年日照时数.asc"));
                this.Cz(f3, f4);
            }

            int m = 1;
            for (m = 1; m <= 12; m++)
            {
                f3 = System.IO.Path.Combine(path, string.Format("{0}月总日照时数.txt", m));
                f4 = System.IO.Path.Combine(path, string.Format("{0}月总日照时数.asc", m));

                this.Cz(f3, f4);
            }

            for (m = 1; m <= 4; m++)
            {
                string j = "春";
                if (m == 1) j = "春";
                if (m == 2) j = "夏";
                if (m == 3) j = "秋";
                if (m == 4) j = "冬";

                f3 = System.IO.Path.Combine(path, string.Format("{0}季总日照时数.txt", j));
                f4 = System.IO.Path.Combine(path, string.Format("{0}季总日照时数.asc", j));
                this.Cz(f3, f4);
            }


            MessageBox.Show("OK");

            return;

        }

        // 干旱
        public void Test_gh()
        {
            //string path = @"D:\Temp\区划\HA\河南省气候标准值降水数据";
            string path = @"d:\Users\shenyc\Documents\知天科技\区划\HA\河南省气候标准值数据-干旱";
            string filePath = System.IO.Path.Combine(path, "");

            string f3 = "";
            string f4 = "";

            {
                f3 = System.IO.Path.Combine(path, string.Format("干旱.txt"));
                f4 = System.IO.Path.Combine(path, string.Format("干旱.asc"));
                this.Cz(f3, f4);
            }


            MessageBox.Show("OK");

            return;

        }

        // 积温
        public void Test_jw()
        {
            //string path = @"D:\Temp\区划\HA\河南省气候标准值降水数据";
            string path = @"d:\Users\shenyc\Documents\知天科技\区划\HA\河南省气候标准值数据-积温";
            string filePath = System.IO.Path.Combine(path, "");

            string f3 = "";
            string f4 = "";

            {
                f3 = System.IO.Path.Combine(path, string.Format("玉米全生育期积温.txt"));
                f4 = System.IO.Path.Combine(path, string.Format("玉米全生育期积温.asc"));
                this.Cz(f3, f4);
            }

            MessageBox.Show("OK");

            return;

        }

        // 暴雨、大风
        public void Test_by_df()
        {
            string strElement = "";
            strElement = "暴雨";
            strElement = "大风";

            string path = string.Format(@"d:\Users\shenyc\Documents\知天科技\区划\HA\河南省气候标准值数据-{0}", strElement);
            string filePath = System.IO.Path.Combine(path, "");

            string f3 = "";
            string f4 = "";

            {
                f3 = System.IO.Path.Combine(path, string.Format("年{0}日数.txt", strElement));
                f4 = System.IO.Path.Combine(path, string.Format("年{0}日数.asc", strElement));
                this.Cz(f3, f4);
            }

            int m = 1;

            for (m = 1; m <= 4; m++)
            {
                string strtemp = "春";
                if (m == 1) strtemp = "春";
                if (m == 2) strtemp = "夏";
                if (m == 3) strtemp = "秋";
                if (m == 4) strtemp = "冬";

                f3 = System.IO.Path.Combine(path, string.Format("{0}季{1}日数.txt", strtemp, strElement));
                f4 = System.IO.Path.Combine(path, string.Format("{0}季{1}日数.asc", strtemp, strElement));
                this.Cz(f3, f4);
            }

            for (m = 1; m <= 12; m++)
            {
                //f3 = System.IO.Path.Combine(path, string.Format("{0}月总日照时数.txt", m));
                //f4 = System.IO.Path.Combine(path, string.Format("{0}月总日照时数.asc", m));
                //this.Cz(f3, f4);
            }


            MessageBox.Show("OK");

            return;

        }

        public void Test_misc()
        {
            //string path = @"D:\Temp\区划\HA\河南省气候标准值降水数据";
            string path = @"d:\Users\shenyc\Documents\知天科技\区划\HA\河南省气候标准值数据-Misc";
            string filePath = System.IO.Path.Combine(path, "");

            string f3 = "";
            string f4 = "";

            {
                f3 = System.IO.Path.Combine(path, string.Format("冬小麦生育期积温.txt"));
                f4 = System.IO.Path.Combine(path, string.Format("冬小麦生育期积温.asc"));
                this.Cz(f3, f4);
            }

            MessageBox.Show("OK");

            return;

        }




        /// <summary>
        /// 插值
        /// </summary>
        /// <param name="f3"></param>
        /// <param name="f4"></param>
        private void Cz(string f3, string f4)
        {

            //------------------------插值
            AxinStationFile f30 = new AxinStationFile(f3);
            //
            // 站点数据插值到格点数据
            //

            List<StationInfo> stationInfos = f30.StationInfos;
            AxinStationFileDataInfo dataInfo = f30.DataInfo as AxinStationFileDataInfo;
            // 包围盒
            IExtent extent = dataInfo.Extent;

            double xInterval = 0.01;
            double yInterval = 0.01;

            double xmin = extent.MinX;
            double ymin = extent.MinY;

            double w = extent.Width;
            double h = extent.Height;

            double xmax = xmin + w + xInterval;
            double ymax = ymin + h + yInterval;

            // input data
            int ni = stationInfos.Count;
            double[] pxi = new double[ni];
            double[] pyi = new double[ni];
            double[] pvi = new double[ni];

            int iCurrentElementIndex = f30.CurrentElementIndex;

            for (int i = 0; i < ni; i++)
            {
                StationInfo si = stationInfos[i];
                pxi[i] = si.Lon;
                pyi[i] = si.Lat;
                pvi[i] = si.ElementValues[iCurrentElementIndex];
            }

            //-----------------------------------------------------
            // 1.声明类对象
            V2GInterpolater v2g = new V2GInterpolater();
            // 2.设置源数据
            v2g.Xsource = pxi;
            v2g.Ysource = pyi;
            v2g.Vsource = pvi;
            // 3.设置网格属性参数
            v2g.GridParam = new GridParam(xmin, ymin, xmax, ymax, xInterval, yInterval);
            // 4.插值
            v2g.Transact();
            //-----------------------------------------------------
            //结果在 GridParam.Vgrid[,]
            Double[,] vGrid = v2g.GridParam.Vgrid;

            //string f40 = "d:\\temp\\40.asc";
            string f40 = f4;
            FileStream fs = new FileStream(f40, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("ncols {0}\r\n", v2g.GridParam.Vgrid.GetLength(1)));
                sb.Append(string.Format("nrows {0}\r\n", v2g.GridParam.Vgrid.GetLength(0)));
                sb.Append(string.Format("xllcorner {0}\r\n", v2g.GridParam.Xmin));
                sb.Append(string.Format("yllcorner {0}\r\n", v2g.GridParam.Ymin));
                sb.Append(string.Format("cellsize {0}\r\n", v2g.GridParam.Xinterval));
                sb.Append(string.Format("nodata_value {0}\r\n", -9999));

                sw.Write(sb.ToString());

                sb.Clear();

                int r = v2g.GridParam.Vgrid.GetLength(0);
                int c = v2g.GridParam.Vgrid.GetLength(1);
                for (int i = 0; i < r; i++)
                {
                    sb.Clear();

                    int ii = r - 1 - i;

                    for (int j = 0; j < c; j++)
                    {
                        sb.Append(string.Format("{0,8:F2}", v2g.GridParam.Vgrid[ii, j]));
                    }

                    sw.WriteLine(sb.ToString());
                }

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            sw.Close();
            fs.Close();

            return;
        }


    }
}
