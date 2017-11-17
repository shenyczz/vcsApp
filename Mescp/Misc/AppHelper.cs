using System;
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
using CSharpKit.Vision.Presentation;

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
                    XClipMin = double.Parse(row["XClipMin"].ToString()),
                    XClipMax = double.Parse(row["XClipMax"].ToString()),
                    YClipMin = double.Parse(row["YClipMin"].ToString()),
                    YClipMax = double.Parse(row["YClipMax"].ToString()),
                    Cmin = double.Parse(row["Cmin"].ToString()),
                    Cmax = double.Parse(row["Cmax"].ToString()),
                    PalCode = int.Parse(row["PalCode"].ToString()),
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
                    RegionID= row["RegionID"].ToString(),
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

                        station.RegionID = row["RegionID"].ToString();

                        //string rgs = row["Region"].ToString();
                        //if (string.IsNullOrEmpty(rgs))
                        //{
                        //    station.RegionID = row["RgnID"].ToString();
                        //}
                        //else
                        //{
                        //    station.RegionID = row["RgnID"].ToString() + "," + row["Region"].ToString();
                        //}

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


        static string ip0 = "10.10.10.100";
        static string ip1 = "172.18.152.243";
        string _DataSourceIP = ip0;

        private string _OutputFileName;

        private Region _CurrentRegion;                  //区域
        private Crop _CurrentCrop;                      //作物
        private CropCultivar _CurrentCropCultivar;      //作物品种
        private List<XStation> _XStations;              //站点集合(当前区域)
        private List<CropGrwp> _CropGrwps;              //发育期集合((当前作物品种))
        private List<CropWorkspace> _CropWorkspaces;    //工作空间集合(当前区域、当前作物、当前品种)

        #endregion

        #region Protected Properties

        private DataSet _DsHenanClimate;
        protected DataSet DsHenanClimate
        {
            get
            {
                if (_DsHenanClimate == null)
                {
                    _DsHenanClimate = new DataSet("HenanClimate");
                }
                return _DsHenanClimate;
            }
        }

        private List<MeteoElement> _MeteoElements = new List<MeteoElement>();
        /// <summary>
        /// 气象要素集合
        /// </summary>
        protected List<MeteoElement> MeteoElements
        {
            get
            {
                if (_MeteoElements == null)
                {
                    _MeteoElements = new List<MeteoElement>();
                }

                return _MeteoElements;
            }
        }

        #endregion

        public void Test()
        {
            //MessageBox.Show("AppHelper.Test()");
            ClearStationColor();
        }

        /// <summary>
        /// 取得气象要素集合
        /// </summary>
        /// <param name="cnnString"></param>
        /// <param name="cmdText"></param>
        /// <param name="xStations"></param>
        /// <returns></returns>
        public List<MeteoElement> GetMeteoElementCollection(string cnnString, string cmdText, List<XStation> xStations)
        {
            List<MeteoElement> meteoElements = MeteoElements;
            meteoElements.Clear();

            try
            {
                // 1.Connection
                SqlConnection cnn = new SqlConnection(cnnString);
                cnn.Open();

                // 2.CommandText
                //cmdText;

                // 3.Command
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = cnn,
                    CommandType = CommandType.Text,
                    CommandText = cmdText,
                };

                // 4.DataAdapter
                SqlDataAdapter da = new SqlDataAdapter()
                {
                    SelectCommand = cmd,
                };

                // 5.填充数据集
                DataSet dsHenanClimate = this.DsHenanClimate;
                dsHenanClimate.Clear();
                string tabName = "tabMeteInfo";
                da.Fill(dsHenanClimate, tabName);

                // 数据行
                DataRowCollection rows = dsHenanClimate.Tables[tabName].Rows;
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
                            me.DateTime = App.Workspace.AppTools.ToDateTime((int)me.ObvDate, (int)me.ObvTime);

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

                //
                //END
                //
            }
            catch (Exception)
            {
            }

            return meteoElements;
        }




        //TODO:评估移到 EvaluReportViewModel


        private void ClearStationColor()
        {
            try
            {
                IMap map = App.Workspace.MapViewModel.Map;
                ILayer layer = map.LayerManager.GetLayer(App.Workspace.AppData.LayerID1);
                IVision vision = layer.Vision;
                ShapeFile shapeFile = vision?.Provider?.DataInstance as ShapeFile;
                List<IFeature> features = shapeFile.Features;
                features.ForEach(p =>
                {
                    p.Tag = System.Drawing.Color.Blue;
                });

                //刷新地图
                map.Refresh(true);
            }
            catch (Exception)
            {
            }
        }





        /*













    public void Test()
    {
        //PingGu();

    }

    //TODO:[20171117]把评估功能移到 EvaluReportViewModel
    public void PingGu()
    {
        Prepare();

        //清除站点填充
        //this.FillStationColor(_XStations, true);
        //if (App.Workspace.AppData.IsContour)
        //    return;

        GetData(_DataSourceIP);

        if (_MeteoElements == null || _MeteoElements.Count == 0)
        {
            MessageBox.Show("没有得到气象要素数据!");
            return;
        }


        XStationSetup();

        SaveData();

        App.Workspace.MapViewModel.FilePath = _OutputFileName;

        //Display();


    }


    #region Privates Functions

    /// <summary>
    /// 准备
    /// </summary>
    private void Prepare()
    {
        //1.区域(当前)
        _CurrentRegion = App.Workspace.AppData.CurrentRegion;
        //2.作物(当前)
        _CurrentCrop = App.Workspace.AppData.CurrentCrop;
        //3.品种(当前)
        _CurrentCropCultivar = App.Workspace.AppData.CurrentCropCultivar;

        //4.发育期集合(当前品种)
        _CropGrwps?.Clear();
        _CropGrwps = (from p in App.Workspace.AppData.CropGrwps
                      where p.CropID == _CurrentCrop.CropID
                      orderby p.GrwpID// descending
                      select p).ToList();

        //5.工作空间集合(当前区域、当前作物、当前品种)
        _CropWorkspaces?.Clear();
        _CropWorkspaces = App.Workspace.AppData.CropWorkspaces.FindAll
            (
                p => p.RegionID.Contains(_CurrentRegion.RgnID)
                  && p.CropID == _CurrentCrop.CropID
                  && p.CultivarID == _CurrentCropCultivar.CultivarID
            );

        //6.站点集合(当前区域)
        _XStations?.Clear();
        _XStations = App.Workspace.AppData.XStations.FindAll
            (
                p => p.RegionID.Contains(_CurrentRegion.RgnID)
            );
    }

    /// <summary>
    /// 取得气象要素集合 => _MeteoElements
    /// 
    /// </summary>
    private void GetData(string dataSource)
    {
        // 评估年份
        int eYear = App.Workspace.AppData.Year;

        // 气象要素集合
        if (_MeteoElements == null)
        {
            _MeteoElements = new List<MeteoElement>();
        }
        _MeteoElements.Clear();

        // 构建选定站点ID字符串，用于SQL语句条件
        // 格式如('57083','57090','53889')
        string strStationIn = GetStationIn();
        if (string.IsNullOrEmpty(strStationIn))
        {
            MessageBox.Show("没有找到符合当前区域条件的配置站点数据!", "提示");
            return;
        }

        //_DataSourceIP = ip1;    //172.18.152.243
        if (!App.Workspace.AppTools.Ping(dataSource))
        {
            MessageBox.Show(string.Format("网络: {0} 不畅通\n无法获取监测数据", dataSource));
            return;
        }

        //链接字符串
        string cnnString = string.Format(@"Data Source={0}; Initial Catalog={1}; User ID={2}; Password={3};",
                                          dataSource,    //[0]Data Source
                                          "HenanClimate",   //[1]Initial Catalog
                                          "nqzx",           //[2]User ID
                                          "KyCen5946");     //[3]Password

        try
        {
            // 1.Connection
            SqlConnection cnn = new SqlConnection(cnnString);
            cnn.Open();

            // 2.CommandText
            string strFields = string.Format("[iiiii],[ObvDate],[T],[Tmax],[Tmin],[E],[S],[F10],[R]");
            //string strTables = string.Format("[MeteDay{0}]", "2010S");
            string strTables = string.Format("[{0}]", App.Workspace.AppTools.ConvertTableName(eYear));
            string strWheres = string.Format("[ObvDate]>={0}0501 AND [ObvDate]<={0}1030 AND [iiiii] IN ({1})", eYear, strStationIn);
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
            _DsHenanClimate.Clear();
            string tabName = "tabMeteInfo";
            da.Fill(_DsHenanClimate, tabName);

            // 数据行
            DataRowCollection rows = _DsHenanClimate.Tables[tabName].Rows;
            if (rows.Count > 0)
            {
                foreach (DataRow row in rows)
                {
                    string stationId = row["iiiii"].ToString().Trim();
                    XStation curStation = _XStations.Find(p => p.Id == stationId);
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

                    _MeteoElements.Add(me);
                }
            }

            //
            //END
            //
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error in GetData() =>" + ex.Message);
        }
    }

    /// <summary>
    /// 设置站点数据并计算
    /// </summary>
    private void XStationSetup()
    {
        // 评估年份
        int eYear = App.Workspace.AppData.Year;

        //设置站点气象要素数据
        _XStations.ForEach(p =>
        {
            p.MeteoElements?.Clear();
            p.MeteoElements = (from me in _MeteoElements
                               where me.StationId == p.Id
                               orderby me.DateTime// descending
                               select me)
                               .ToList()
                               ;
        });

        //设置站点参数
        _XStations.ForEach(p =>
        {
            p.Year = eYear;                         //评估年份
            p.CropGrwps = _CropGrwps;               //作物发育期
            p.CropWorkspaces = _CropWorkspaces;     //作物工作空间
            p.FaMax = _CurrentRegion.Cmax;          //发育期适宜度最小值
            p.FaMin = _CurrentRegion.Cmin;          //发育期适宜度最大值
        });

        // 
        string stemp = "";
        try
        {
            //计算站点
            _XStations.ForEach(p =>
            {
                stemp = p.Id;
                p.DoIt();
            });
        }
        catch (Exception)
        {
            throw new Exception("Error in  XStationSetup(...)" + stemp);
        }

        //取得所有站点发育期适宜度的最大最小值 - 还有用
        double max, min;
        max = double.NegativeInfinity;
        min = double.PositiveInfinity;
        _XStations.ForEach(p =>
        {
            if (p.Fa > 0)
            {
                max = Math.Max(max, p.Fa);
                min = Math.Min(min, p.Fa);
            }
        });

        //评估(0.2max+0.8min、0.8max+0.2min) => 0:不适宜、1:次适宜、2:适宜
        //_XStations.ForEach(p =>
        //{
        //    p.Fae = App.Workspace.AppMethod.Fae(p.Fa, max, min);
        //});

#if DEBUG
        //TODO:====================================Test
        XStation[] xa0 = _XStations.FindAll(p => p.Fae == 0).ToArray();     //不适宜
        XStation[] xa1 = _XStations.FindAll(p => p.Fae == 1).ToArray();     //次适宜
        XStation[] xa2 = _XStations.FindAll(p => p.Fae == 2).ToArray();     //适宜
        XStation[] xa3 = _XStations.FindAll(p => p.Fae == -1).ToArray();    //未知
        //=========================================
#endif
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    private void SaveData()
    {
        // 评估年份
        int eYear = App.Workspace.AppData.Year;

        //输出文件名称
        //if (App.Workspace.AppData.IsStation)
        //    _OutputFileName = System.IO.Path.Combine(App.OutputPath, string.Format("{0}.txt", eYear));

        //if (App.Workspace.AppData.IsContour)
        //    _OutputFileName = System.IO.Path.Combine(App.OutputPath, string.Format("{0}_9999.txt", eYear));


        _OutputFileName = System.IO.Path.Combine(App.OutputPath, string.Format("{0}.txt", eYear));
        OutputStationFile(_XStations, _OutputFileName);
    }

    /// <summary>
    /// 输出站点数据文件
    /// </summary>
    /// <param name="xStations"></param>
    /// <param name="fileName"></param>
    private void OutputStationFile(List<XStation> xStations, string fileName)
    {
        int eYear = App.Workspace.AppData.Year; //评估年份
        Region curRegion = _CurrentRegion;      //当前区域

        //填充数据 StationInfos
        AxinStationFile fileAxinStation = new AxinStationFile();
        xStations.ForEach(p =>
        {
            StationInfo si = new StationInfo();
            {
                si.Id = p.Id;
                si.Name = p.Name;
                si.Lon = p.Lon;
                si.Lat = p.Lat;

                si.ElementCount = 2;            //要素数量
                si.ElementValues[0] = p.Fa;     //要素值
                si.ElementValues[1] = p.Fae;    //要素评估值
                si.CurrentElementIndex = 0;
                si.CurrentElementValue = si.ElementValues[0];
            }

            if (si.CurrentElementValue > 0 && si.CurrentElementValue != -999 && !(si.CurrentElementValue is double.NaN))
            {
                fileAxinStation.StationInfos.Add(si);
            }
        });

        //数据信息
        AxinStationFileDataInfo axin30di = fileAxinStation.DataInfo as AxinStationFileDataInfo;
        {
            axin30di.FileId = AxinConstants.FileLogo;
            axin30di.FormatCode = AxinConstants.FormatCode_Tin;
            axin30di.Comment = "永优玉米评估";
            axin30di.DateTime = new DateTime(eYear, 1, 1);
            axin30di.TimePeriod = 0;
            axin30di.Layer = 999;
            //axin30di.ProductCode = 6824;    //使用分段调色板
            axin30di.ProductCode = curRegion.PalCode;   //使用分段调色板
            axin30di.ElementCode = 1;                   //0：透明背景 1：插值背景
            axin30di.StationCount = fileAxinStation.StationInfos.Count; //站点数量
            axin30di.ElementCount = 2;      //要素数量
            axin30di.Flag = 1;              //具有站点名称字段

            //等值线
            axin30di.ContourInfo.ContourNums = 3;
            axin30di.ContourInfo.ContourValues[0] = double.Parse((0.8 * curRegion.Cmin + 0.2 * curRegion.Cmax).ToString("F1"));
            axin30di.ContourInfo.ContourValues[1] = double.Parse((0.5 * curRegion.Cmin + 0.5 * curRegion.Cmax).ToString("F1"));
            axin30di.ContourInfo.ContourValues[2] = double.Parse((0.2 * curRegion.Cmin + 0.8 * curRegion.Cmax).ToString("F1"));
            axin30di.ContourInfo.ContourBoldValue = 0;

            //剪切区
            axin30di.ClipArea.Id = 9999;
            axin30di.ClipArea.XClipMin = curRegion.XClipMin;
            axin30di.ClipArea.XClipMax = curRegion.XClipMax;
            axin30di.ClipArea.YClipMin = curRegion.YClipMin;
            axin30di.ClipArea.YClipMax = curRegion.YClipMax;
        }

        fileAxinStation.DataProcessor.SaveAs(fileName);

        //END_OF_FUNCTION
    }

    /// <summary>
    /// 显示
    /// </summary>
    private void Display()
    {
        if (App.Workspace.AppData.IsContour)
        {
            this.EnableStationColor(App.Workspace.AppData.LayerID1, false);
            this.EnableShowContour(App.Workspace.AppData.LayerID2, true);
            this.FillStationContour();
        }
        else if (App.Workspace.AppData.IsStation)
        {
            this.EnableStationColor(App.Workspace.AppData.LayerID1, true);
            this.EnableShowContour(App.Workspace.AppData.LayerID2, false);
            this.FillStationColor(_XStations);    //行政区填充
        }

    }

    /// <summary>
    /// 清除站点填充
    /// </summary>
    private void EnableStationColor(string layerId, bool enable)
    {
        try
        {
            IMap map = App.Workspace.MapViewModel.Map;
            ILayer layer = map.LayerManager.GetLayer(layerId);
            IVision vision = layer.Vision;
            vision.IsFill = enable;
        }
        catch (Exception)
        {
        }
    }

    private void EnableShowContour(string layerId, bool enable)
    {
        try
        {
            IMap map = App.Workspace.MapViewModel.Map;
            ILayer layer = map.LayerManager.GetLayer(layerId);
            IVision vision = layer.Vision;
            vision.IsDrawContour = enable;
            vision.IsFillContour = enable;
            vision.IsLabelContour = enable;

            App.Workspace.PropertyViewModel.VisionProperties = null;
            App.Workspace.PropertyViewModel.VisionProperties = vision.CustomProperties;
        }
        catch (Exception)
        {
        }

    }

    /// <summary>
    /// 填充县级颜色
    /// </summary>
    /// <param name="xStations"></param>
    private void FillStationColor(List<XStation> xStations)
    {
        try
        {
            IMap map = App.Workspace.MapViewModel.Map;
            ILayer layer = map.LayerManager.Layers.Find(l => l.Id == App.Workspace.AppData.LayerID1);
            IVision vision = layer.Vision;
            IProvider provider = vision.Provider;
            IDataInstance dataInstance = provider.DataInstance;

            vision.IsFill = true;

            // 取得Shape文件提供者
            ShapeFile shapeFile = dataInstance as ShapeFile;

            //AxinColortabFile 颜色表文件
            string s = System.IO.Path.Combine(App.StartupPath, "Palettes\\6801.pal");   //使用索引调色板
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
    /// 等高线填充
    /// </summary>
    private void FillStationContour()
    {
        IMap map = App.Workspace.MapViewModel.Map;
        ILayer layer = map.LayerManager.GetLayer(App.Workspace.AppData.LayerID2);
        if (layer != null)
        {
            map.LayerManager.Remove(layer);
        }

        try
        {
            String fileName = _OutputFileName;
            IProvider provider = new AxinFileProvider(fileName);
            IVision vision = new AxinVision(provider.DataInstance?.DataInfo.Comment)
            {
                Provider = provider,
                Renderer = new WfmAxinVisionRenderer(),

                Transparency = 0,

                //IsClip = true,
                IsColorContour = true,
                IsFillContour = true,       //填充等高线
                //IsLabelContour = false,   //标注等高线
                //IsDrawContour = false,    //绘制等高线

                Foreground = System.Drawing.Color.Yellow,

            };

            map.LayerManager.Add(new Layer(App.Workspace.AppData.LayerID2, vision));
            App.Workspace.PropertyViewModel.VisionProperties = null;
            App.Workspace.PropertyViewModel.VisionProperties = vision.CustomProperties;

            App.Workspace.EvaluReportViewModel.StationInfos = (provider.DataInstance as AxinStationFile).StationInfos;
        }
        catch (Exception ex)
        {
            string errMsg = ex.Message;
        }

        //End

    }


    // 构建选定站点ID字符串，用于SQL语句条件
    // 格式如('57083','57090','53889')
    private String GetStationIn()
    {
        int n = 0;
        StringBuilder sb = new StringBuilder();
        foreach (XStation sta in _XStations)
        {
            string s = string.Format("\'{0}\'", sta.Id);
            sb.Append(s);

            if (++n >= _XStations.Count)
                break;

            sb.Append(",");
        }

        return sb.ToString();
    }

    //生成文件名
    private void GenerateFileName()
    {

    }

    #endregion


    */





































    }

}
