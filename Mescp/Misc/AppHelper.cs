using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Windows;
using CSharpKit.Extentions;
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

                // sp_Station
                cmd.CommandText = "sp_Station";
                da.Fill(_dsMescpConfig, "TStation");

                // sp_SFPeriod
                cmd.CommandText = "sp_SFPeriod";
                da.Fill(_dsMescpConfig, "TSFPeriod");

                // sp_SFAll
                cmd.CommandText = "sp_SFAll";
                da.Fill(_dsMescpConfig, "TSFAll");

                cnn.Close(); 

                //DataTable t = _dsMescpConfig.Tables["TCropWorksapce"];

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
                    RegionID = row["RegionID"].ToString(),
                    RgnID = row["RgnID"].ToString(),
                    CropID = row["CropID"].ToString(),
                    CultivarID = row["CultivarID"].ToString(),

                    //GrwpID = row["GrwpID"].ToString(),
                    //GrwpName = row["GrwpName"].ToString(),
                    //GrwpSpan = row["GrwpSpan"].ToString(),

                    CropGrwp = new CropGrwp()
                    {
                        CropID = row["CropID"].ToString(),

                        GrwpID = row["GrwpID"].ToString(),
                        GrwpName = row["GrwpName"].ToString(),
                        GrwpSpan = row["GrwpSpan"].ToString(),

                        GrwpCode = "",
                        Remark = "",
                    },

                    //阈值
                    ThrRain = row["thrRain"].ToString(),
                    ThrSunlight = row["thrSunlight"].ToString(),
                    ThrTemperature = row["thrTemperature"].ToString(),

                    //权重
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
                        station.ReplaceSite= row["ReplaceSite"].ToString();
                    }
                    stations.Add(station);
                }
                catch (Exception)
                {
                    throw new Exception("Error in getStations()!");
                }
            }
        }

        // Period
        public void GetPeriods(List<Period> periods)
        {
            DataRowCollection rows = _dsMescpConfig.Tables["TSFPeriod"].Rows;
            if (rows.Count <= 0)
                return;

            periods.Clear();
            foreach (DataRow row in rows)
            {
                try
                {
                    Period period = new Period();
                    {
                        period.PeriodID = row["PeriodID"].ToString();
                        period.PeriodCode = row["PeriodCode"].ToString().ToInt();
                        period.PeriodName = row["PeriodName"].ToString();
                    }
                    periods.Add(period);
                }
                catch (Exception)
                {
                    throw new Exception("Error in GetPeriods()!");
                }
            }

            // END
        }

        // Compartment
        public void GetCompartments(List<Compartment> compartments)
        {

            DataRowCollection rows = _dsMescpConfig.Tables["TSFAll"].Rows;
            if (rows.Count <= 0)
                return;

            compartments.Clear();
            foreach (DataRow row in rows)
            {
                try
                {
                    Compartment cpm = new Compartment();
                    {
                        cpm.SFID = row["SFID"].ToString();
                        cpm.CultivarID = row["CultivarID"].ToString();
                        cpm.PeriodID = row["PeriodID"].ToString();

                        cpm.A_MIN = row["A_MIN"].ToString().ToDouble();
                        cpm.A_MAX = row["A_MAX"].ToString().ToDouble();
                        cpm.B_MIN = row["B_MIN"].ToString().ToDouble();
                        cpm.B_MAX = row["B_MAX"].ToString().ToDouble();
                        cpm.Wgt = row["Wgt"].ToString().ToDouble();

                        cpm.Flag = row["Flag"].ToString();
                    }
                    compartments.Add(cpm);
                }
                catch (Exception)
                {
                    throw new Exception("Error in GetCompartments()!");
                }
            }

            // END
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

        #endregion

        #region Protected Properties

        private DataSet _DsHenanClimate;
        /// <summary>
        /// 河南气候数据集
        /// </summary>
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

        /// <summary>
        /// 取得气象要素集合
        /// </summary>
        /// <param name="cnnString"></param>
        /// <param name="cmdText"></param>
        /// <param name="xStations"></param>
        /// <returns></returns>
        public List<MeteoElement> GetMeteoElementCollection(string cnnString, string cmdText, List<XStation> xStations)
        {
            List<MeteoElement> meteoElements = this.MeteoElements;
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

                            me.E = 0.1 * double.Parse(row["E"].ToString());         //水汽压(hPa)
                            me.Ws = 0.1 * double.Parse(row["F10"].ToString());      //风速
                            me.Hos = 0.1 * double.Parse(row["S"].ToString());       //日照

                            me.R = 0.1 * double.Parse(row["R"].ToString());         //降水
                            me.U = 1.0 * double.Parse(row["U"].ToString());         //相对湿度
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






        // TODO:评估移到 EvaluReportViewModel


        
        /// <summary>
        /// 测试
        /// </summary>
        public void Test()
        {
            MessageBox.Show("AppHelper.Test()");
        }

    }

}
