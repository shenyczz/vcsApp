﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using CSharpKit;
using CSharpKit.Data;
using CSharpKit.Data.Axin;
using CSharpKit.Data.Esri;
using CSharpKit.Maths.ContourTracing;
using CSharpKit.Palettes;
using CSharpKit.Vision;
using CSharpKit.Vision.Mapping;
using Mescp.Models;

namespace Mescp
{
    /// <summary>
    /// 本地配置数据库
    /// AppHelper - partial_0[Mescp.mdb]
    /// </summary>
    public partial class AppHelper
    {
        public AppHelper()
        {
            LoadConfigMdb();
        }

        #region Private Fields

        private DataSet _dsMescpConfig = new DataSet("MescpConfig");
        private string _mdbFile = System.IO.Path.Combine(App.ConfigPath, "Mescp.mdb");

        #endregion

        #region 配置数据库

        private void LoadConfigMdb()
        {
            string cnnString =
                string.Format("Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; User ID=;Jet OLEDB:Database Password=;"
                , _mdbFile);

            try
            {
                // 1.Connection
                OleDbConnection cnn = new OleDbConnection(cnnString);
                cnn.Open();

                // 2.Command
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;

                // 3.DataAdapter
                OleDbDataAdapter da = new OleDbDataAdapter();
                da.SelectCommand = cmd;

                // sp_Region
                cmd.CommandText = "sp_Region";
                da.Fill(_dsMescpConfig, "TRegion");

                // sp_Crop
                cmd.CommandText = "sp_Crop";
                da.Fill(_dsMescpConfig, "TCrop");

                // sp_CropCultivar
                cmd.CommandText = "sp_CropCultivar";
                da.Fill(_dsMescpConfig, "TCropCultivar");

                // sp_CropGrwp
                cmd.CommandText = "sp_CropGrwp";
                da.Fill(_dsMescpConfig, "TCropGrwp");

                // sp_CropWorksapce
                cmd.CommandText = "sp_CropWorksapce";
                da.Fill(_dsMescpConfig, "TCropWorksapce");

                DataTable t = _dsMescpConfig.Tables["TCropWorksapce"];

                // sp_Station
                cmd.CommandText = "sp_Station";
                da.Fill(_dsMescpConfig, "TStation");

                cnn.Close(); 

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Region
        public void GetRegions(List<Region> regions)
        {
            regions.Clear();
            foreach (DataRow row in _dsMescpConfig.Tables["TRegion"].Rows)
            {
                regions.Add(new Region()
                {
                    RgnID = row["RgnID"].ToString(),
                    RgnCode = row["RgnCode"].ToString(),
                    RgnName = row["RgnName"].ToString(),
                });
            }
        }

        // Crop
        public void GetCrops(List<Crop> crops)
        {
            crops.Clear();
            foreach (DataRow row in _dsMescpConfig.Tables["TCrop"].Rows)
            {
                crops.Add(new Crop()
                {
                    CropID = row["CropID"].ToString(),
                    CropCode = row["CropCode"].ToString(),
                    CropName = row["CropName"].ToString(),
                });
            }
        }

        // CropCultivar
        public void GetCropCultivars(List<CropCultivar> cropCultivars)
        {
            cropCultivars.Clear();
            foreach (DataRow row in _dsMescpConfig.Tables["TCropCultivar"].Rows)
            {
                cropCultivars.Add(new CropCultivar()
                {
                    CropID = row["CropID"].ToString(),
                    CultivarID = row["CultivarID"].ToString(),
                    CultivarCode = row["CultivarCode"].ToString(),
                    CultivarName = row["CultivarName"].ToString(),
                });
            }
        }
        public void GetCropCultivars(List<CropCultivar> cropCultivars, Crop crop)
        {
            cropCultivars.Clear();
            foreach (DataRow row in _dsMescpConfig.Tables["TCropCultivar"].Rows)
            {
                if (crop.CropID != row["CropID"].ToString())
                    continue;

                cropCultivars.Add(new CropCultivar()
                {
                    CropID = row["CropID"].ToString(),
                    CultivarID = row["CultivarID"].ToString(),
                    CultivarCode = row["CultivarCode"].ToString(),
                    CultivarName = row["CultivarName"].ToString(),
                });
            }
        }

        // CropGrwp
        public void GetCropGrwps(List<CropGrwp> cropGrwps)
        {
            cropGrwps.Clear();
            foreach (DataRow row in _dsMescpConfig.Tables["TCropGrwp"].Rows)
            {
                cropGrwps.Add(new CropGrwp()
                {
                    CropID = row["CropID"].ToString(),
                    GrwpID = row["GrwpID"].ToString(),
                    GrwpCode = row["GrwpCode"].ToString(),
                    GrwpName = row["GrwpName"].ToString(),
                    GrwpSpan = row["GrwpSpan"].ToString(),
                });
            }
        }
        public void GetCropGrwps(List<CropGrwp> cropGrwps, Crop crop)
        {
            cropGrwps.Clear();
            foreach (DataRow row in _dsMescpConfig.Tables["TCropGrwp"].Rows)
            {
                if (crop.CropID != row["CropID"].ToString())
                    continue;

                cropGrwps.Add(new CropGrwp()
                {
                    CropID = row["CropID"].ToString(),
                    GrwpID = row["GrwpID"].ToString(),
                    GrwpCode = row["GrwpCode"].ToString(),
                    GrwpName = row["GrwpName"].ToString(),
                    GrwpSpan = row["GrwpSpan"].ToString(),
                });
            }
        }

        // CropWorkspace
        public void GetCropWorkspaces(List<CropWorkspace> cropWorkspaces)
        {
            DataRowCollection rows = _dsMescpConfig.Tables["TCropWorksapce"].Rows;
            if (rows.Count <= 0)
                return;

            cropWorkspaces.Clear();
            foreach (DataRow row in rows)
            {
                cropWorkspaces.Add(new CropWorkspace()
                {
                    RgnID = row["RgnID"].ToString(),
                    CropID = row["CropID"].ToString(),
                    CultivarID = row["CultivarID"].ToString(),

                    GrwpID = row["GrwpID"].ToString(),
                    GrwpName = row["GrwpName"].ToString(),
                    GrwpSpan = row["GrwpSpan"].ToString(),

                    ThrRain = row["thrRain"].ToString(),
                    ThrSunlight = row["thrSunlight"].ToString(),
                    ThrTemperature = row["thrTemperature"].ToString(),

                    Weight = double.Parse(row["Weight01"].ToString()),
                });
            }
        }

        // XStation
        public void GetStations(List<XStation> stations)
        {
            DataRowCollection rows = _dsMescpConfig.Tables["TStation"].Rows;
            if (rows.Count <= 0)
                return;

            stations.Clear();
            foreach (DataRow row in rows)
            {
                try
                {
                    XStation station = new XStation();
                    {
                        station.Id = row["StationID"].ToString();
                        station.Name = row["StationName"].ToString();
                        station.Lon = double.Parse(row["Lon"].ToString()) * 0.01;
                        station.Lat = double.Parse(row["Lat"].ToString()) * 0.01;
                        station.Alt = double.Parse(row["Alt"].ToString()) * 0.1;

                        string rgs= row["Region"].ToString();
                        if (string.IsNullOrEmpty(rgs))
                        {
                            station.Region = row["RgnID"].ToString();
                        }
                        else
                        {
                            station.Region = row["RgnID"].ToString() + "," + row["Region"].ToString();
                        }
                    }
                    stations.Add(station);
                }
                catch (Exception)
                {
                    throw new Exception("Error in getStations()!");
                }
            }
        }

        #endregion

    }


    /// <summary>
    /// 远程数据库
    /// AppHelper - partial_1[HenanClimate]
    /// </summary>
    partial class AppHelper
    {
        #region Private Fields

        private DataSet _dsHenanClimate = new DataSet("HenanClimate");
        static string ip0 = "10.10.10.100";
        static string ip1 = "172.18.152.243";
        string _DataSourceIP = ip0;

        #endregion

        private DateTime ToDateTime(int yyyymmdd, int HHMM)
        {
            int year = yyyymmdd / 10000;
            int mmdd = yyyymmdd % 10000;
            int month = mmdd / 100;
            int day = mmdd % 100;

            int hour = HHMM / 100;
            int minute = HHMM % 100;

            return new DateTime(year, month, day, hour, minute, 0);
        }

        public void Test()
        {
            //MessageBox.Show("Test");

            //IMap map = App.Workspace.MapViewModel.Map;
            //ILayer layer = map.LayerManager.Layers.Find(l => l.Id == "t123");
            //IVision vision = layer.Vision;
            //App.Workspace.PropertyViewModel.VisionProperties = vision.CustomProperties;

            //App.Workspace.MapViewModel.ClearChecked();
            //App.Workspace.PrimiviteViewModel.ClearChecked();

            PingGu();

        }


        // 取得指定站点集多天的气象数据
        /// <summary>
        /// 评估
        /// </summary>
        public void PingGu()
        {
            // 1.区域(当前)
            Region currentRegion = App.Workspace.AppData.CurrentRegion;
            // 2.作物(当前)
            Crop currentCrop = App.Workspace.AppData.CurrentCrop;
            // 3.品种(当前)
            CropCultivar currentCropCultivar = App.Workspace.AppData.CurrentCropCultivar;

            // 4.发育期集合(当前品种)
            List<CropGrwp> cropGrwps = (from p in App.Workspace.AppData.CropGrwps
                                        where p.CropID == currentCrop.CropID
                                        orderby p.GrwpID// descending
                                        select p)
                                        .ToList();

            // 5.工作空间集合(当前区域、当前作物、当前品种)
            List<CropWorkspace> cropWorkspaces = App.Workspace.AppData.CropWorkspaces.FindAll
                (
                    p => p.RgnID == currentRegion.RgnID
                      && p.CropID == currentCrop.CropID
                      && p.CultivarID == currentCropCultivar.CultivarID
                );

            // 6.站点集合(当前区域)
            List<XStation> xStations = App.Workspace.AppData.XStations.FindAll(p => p.Region.Contains(currentRegion.RgnID));

            // 构建选定站点ID字符串，用于SQL语句条件
            // 格式如('57083','57090','53889')
            int n = 0;
            StringBuilder sb = new StringBuilder();
            foreach (XStation sta in xStations)
            {
                string s = string.Format("\'{0}\'", sta.Id);
                sb.Append(s);

                if (++n >= xStations.Count)
                    break;

                sb.Append(",");
            }
            string strIN = sb.ToString();
            if (string.IsNullOrEmpty(strIN))
            {
                MessageBox.Show("没有找到符合当前区域条件的配置站点数据!","提示");
                return;
            }

            //_DataSourceIP = ip1;    //172.18.152.243
            if (!App.Workspace.AppTools.Ping(_DataSourceIP))
            {
                MessageBox.Show(string.Format("网络: {0} 不畅通\n无法获取监测数据", _DataSourceIP));
                return;
            }

            // 评估年份
            int year = App.Workspace.AppData.Year;
            //MessageBox.Show(year.ToString());
            //return;

            //链接字符串
            string cnnString =
                string.Format(@"Data Source={0}; Initial Catalog=HenanClimate; User ID=nqzx; Password=KyCen5946;", _DataSourceIP);

            try
            {
                // 1.Connection
                SqlConnection cnn = new SqlConnection(cnnString);
                cnn.Open();

                // 2.CommandText
                string strFields = string.Format("[iiiii],[ObvDate],[T],[Tmax],[Tmin],[E],[S],[F10],[R]");
                string strTables = string.Format("[MeteDay{0}]", "2010S");
                string strWheres = string.Format("[ObvDate]>={0}0501 AND [ObvDate]<={0}1030 AND [iiiii] IN ({1})", year, strIN);
                string strOders = string.Format("[ObvDate],[iiiii]");
                string strSql = string.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}",
                    strFields, strTables, strWheres, strOders);

                // SELECT [iiiii],[ObvDate],[ObvTime],[P],[T],[Tmax],[Tmin],[U],[E],[R]
                // FROM [MeteHour2016]
                // WHERE [ObvDate]>=20160601 AND [ObvDate]<=20160610 AND [ObvTime]=1100 AND [iiiii] IN ('57083','57090')
                // ORDER BY [ObvDate],[iiiii]

                // 3.Command
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = cnn,
                    CommandType = CommandType.Text,
                    CommandText = strSql,
                };

                // 4.DataAdapter
                SqlDataAdapter da = new SqlDataAdapter()
                {
                    SelectCommand = cmd,
                };

                // 5.填充数据集
                string tabName = "tabMeteInfo";
                da.Fill(_dsHenanClimate, tabName);

                // 气象要素集合
                MeteoElementCollection meteoElements = new MeteoElementCollection();
                meteoElements.Clear();

                // 数据行
                DataRowCollection rows = _dsHenanClimate.Tables[tabName].Rows;
                if (rows.Count > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        string stationId = row["iiiii"].ToString().Trim();
                        XStation curStation = xStations.Find(p => p.Id == stationId);
                        if (curStation == null)
                            continue;

                        MeteoElement me = new MeteoElement();
                        {
                            me.StationId = curStation.Id;
                            me.StationName = curStation.Name;

                            //观测时间
                            me.ObvDate = int.Parse(row["ObvDate"].ToString());
                            //me.ObvTime = int.Parse(row["ObvTime"].ToString());
                            me.ObvTime = 0;
                            me.DateTime = this.ToDateTime((int)me.ObvDate, (int)me.ObvTime);

                            me.T = 0.1 * double.Parse(row["T"].ToString());         //温度
                            me.Tmax = 0.1 * double.Parse(row["Tmax"].ToString());   //最高温度
                            me.Tmin = 0.1 * double.Parse(row["Tmin"].ToString());   //最低温度

                            me.E = 0.1 * double.Parse(row["E"].ToString());         //水汽压
                            me.Ws = 0.1 * double.Parse(row["F10"].ToString());      //风速
                            me.Hos = 0.1 * double.Parse(row["S"].ToString());       //日照

                            me.R = 0.1 * double.Parse(row["R"].ToString());         //降水
                        }

                        meteoElements.Add(me);
                    }
                }

                //设置站点气象要素数据
                xStations.ForEach(p=>
                {
                    p.MeteoElements = (from me in meteoElements
                                       where me.StationId == p.Id
                                       orderby me.DateTime// descending
                                       select me)
                                       .ToList()
                                       ;
                });

                //设置站点参数
                xStations.ForEach(p =>
                {
                    p.Year = year;                      //评估年份
                    p.CropGrwps = cropGrwps;            //作物发育期
                    p.CropWorkspaces = cropWorkspaces;  //作物工作空间
                });

                // 
                string stemp = "";
                try
                {
                    //计算站点
                    xStations.ForEach(p =>
                    {
                        stemp = p.Id;
                        p.DoIt();
                    });
                }
                catch (Exception)
                {
                    throw new Exception(stemp);
                }

                //取得所有站点发育期适宜度的最大最小值
                double max, min;
                max = double.NegativeInfinity;
                min = double.PositiveInfinity;
                xStations.ForEach(p =>
                {
                    if (p.Fa > 0)
                    {
                        max = Math.Max(max, p.Fa);
                        min = Math.Min(min, p.Fa);
                    }
                });

                //评估(0.2max+0.8min、0.8max+0.2min) => 不适宜、次适宜、适宜
                xStations.ForEach(p =>
                {
                    p.Fae = App.Workspace.AppMethod.Fae(p.Fa, max, min);
                });

                //======================
                XStation[] xa0 = xStations.FindAll(p => p.Fae == 0).ToArray();     //不适宜
                XStation[] xa1 = xStations.FindAll(p => p.Fae == 1).ToArray();     //次适宜
                XStation[] xa2 = xStations.FindAll(p => p.Fae == 2).ToArray();     //适宜
                XStation[] xa3 = xStations.FindAll(p => p.Fae == -1).ToArray();    //未知
                //======================
                //下面绘图
                this.FillCountyColor(xStations);    //绘图（行政区填充）

                //下面输出站点文件
                //
                string s = System.IO.Path.Combine(App.OutputPath, "30.txt");
                OutputStationFile(xStations, s);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Test(...)" + ex.Message);
            }
        }

        /// <summary>
        /// 填充县级颜色
        /// </summary>
        /// <param name="xStations"></param>
        private void FillCountyColor(List<XStation> xStations)
        {
            //double a1 = 4.29;
            //double a2 = 13.26;
            try
            {
                IMap map = App.Workspace.MapViewModel.Map;
                ILayer layer = map.LayerManager.Layers.Find(l => l.Id == App.Workspace.MapViewModel.LayerID);
                IVision vision = layer.Vision;
                IProvider provider = vision.Provider;
                IDataInstance dataInstance = provider.DataInstance;

                // 取得Shape文件提供者
                ShapeFile shapeFile = dataInstance as ShapeFile;

                //AxinColortabFile 颜色表文件
                string s = System.IO.Path.Combine(App.StartupPath, "Palettes\\6822.pal");   //使用索引调色板
                AxinColortabFile axinColortabFile = new AxinColortabFile(s);
                IPalette palette = axinColortabFile.Palette;    // 调色板

                foreach (XStation xs in xStations)
                {
                    List<IFeature> features = shapeFile.Features.FindAll(f => f.Id == xs.Id);
                    features.ForEach(p =>
                    {
                        double f = xs.Fae;    //站点适宜度值
                        System.Drawing.Color clr = palette.GetColor(f, System.Drawing.Color.Black);
                        p.Tag = clr;
                    });
                }

                //刷新地图
                map.Refresh(true);

            }
            catch (Exception)
            {
                throw;
            }
        }

         /// <summary>
         /// 输出站点数据文件
         /// </summary>
         /// <param name="xStations"></param>
         /// <param name="fileName"></param>
        private void OutputStationFile(List<XStation> xStations, string fileName)
        {
            /*
                第30类文件 – 离散点数据
                [文件头]
                FID  FormatCode  Comment
                yyyy mm dd HH MM SS MS
                TimePeriod  Layer  ProductCode  ElementCode
                TotalNum  ElementNum  Flag
                CID C1  C2  C3 … Cn  Cb
                ClipArea  X1 Y1  X2 Y2 … Xn Yn

                [数据]
                站点  经度  纬度  海拔  站点级别 要素值1…N  站点名称
             */
            //填充数据
            AxinStationFile f = new AxinStationFile();
            xStations.ForEach(p =>
            {
                StationInfo si = new StationInfo();
                {
                    si.Id = p.Id;
                    si.Name = p.Name;
                    si.Lon = p.Lon;
                    si.Lat = p.Lat;

                    si.ElementCount = 1;            //要素数量
                    si.ElementValues[0] = p.Fa;     //要素值
                    si.CurrentElementIndex = 0;
                    si.CurrentElementValue = si.ElementValues[0];
                }

                if (si.CurrentElementValue != -999)
                {
                    f.StationInfos.Add(si);
                }
            });

            //数据信息
            AxinStationFileDataInfo axin30di = f.DataInfo as AxinStationFileDataInfo;
            {
                axin30di.FileId = AxinConstants.FileLogo;
                axin30di.FormatCode = AxinConstants.FormatCode_Tin;
                axin30di.Comment = "永优玉米评估";
                axin30di.DateTime = DateTime.Now;
                axin30di.TimePeriod = 0;
                axin30di.Layer = 999;
                axin30di.ProductCode = 0;
                axin30di.ElementCode = 0;
                axin30di.StationCount = f.StationInfos.Count;
                axin30di.ElementCount = 1;
                axin30di.Flag = 1;

                axin30di.ContourInfo.ContourNums = 3;
                axin30di.ContourInfo.ContourValues[0] = 5;
                axin30di.ContourInfo.ContourValues[1] = 10;
                axin30di.ContourInfo.ContourValues[2] = 15;
                //axin30di.ContourInfo.ContourValues[3] = 20;
                axin30di.ContourInfo.ContourBoldValue = 0;

                //axin30di.ContourInfo.ContourNums = 9999;
                //axin30di.ContourInfo.ContourInterval = 1;
                //axin30di.ContourInfo.ContourMin = 5;
                //axin30di.ContourInfo.ContourMax = 35;
                //axin30di.ContourInfo.ContourBoldValue = 15;

                axin30di.ClipArea.Id = 9999;
                axin30di.ClipArea.XClipMin = 110.33;
                axin30di.ClipArea.XClipMax = 116.66;
                axin30di.ClipArea.YClipMin = 31.36;
                axin30di.ClipArea.YClipMax = 36.38;
            }

            f.DataProcessor.SaveAs(fileName);


            MessageBox.Show("ok");
        }




    }

}
