using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpKit;
using Mescp.Models;
using System.Windows;
using CSharpKit.Data.Axin;
using CSharpKit.Data;

namespace Mescp.ViewModels
{
    /// <summary>
    /// 评估报告 - VmEvaluReport
    /// </summary>
    public class EvaluReportViewModel : DataGridViewModelBase
    {
        public EvaluReportViewModel()
        {
        }

        #region Private Fields

        //private string _OutputFileName; //评估结果输出文件名
        //private string _CommandText;    //查询字符串

        private Region _CurrentRegion;                  //区域
        private Crop _CurrentCrop;                      //作物
        private CropCultivar _CurrentCropCultivar;      //作物品种

        private List<CropGrwp> _CropGrwps;              //发育期集合((当前作物品种))
        private List<CropWorkspace> _CropWorkspaces;    //工作空间集合(当前区域、当前作物、当前品种)

        private List<XStation> _XStations;              //站点集合(当前区域)

        //private List<MeteoElement> _MeteoElements;

        #endregion


        #region StationInfos

        List<StationInfo> _StationInfos;
        /// <summary>
        /// EvaluateReportView 的 ItemSource
        /// </summary>
        public List<StationInfo> StationInfos
        {
            get
            {
                return _StationInfos;
            }
            set
            {
                _StationInfos = value;
                RaisePropertyChanged("StationInfos");
            }
        }

        #endregion



        public void Evaluate()
        {
            // 评估年份
            int eYear = App.Workspace.AppData.Year;

            string ip = App.Workspace.AppData.RemoteDataSource;
            if (!App.Workspace.AppTools.Ping(ip))
            {
                MessageBox.Show(string.Format("网络: {0} 不畅通\n无法获取监测数据", ip));
                return;
            }


            Prepare();

            Region currentRegion = _CurrentRegion;
            List<CropGrwp> cropGrwps = _CropGrwps;
            List<CropWorkspace> cropWorkspaces = _CropWorkspaces;
            List<XStation> xStations = _XStations;

            SetupXStation(xStations);
            SetupXStation(xStations, eYear, currentRegion, cropGrwps, cropWorkspaces);

            string cmdText = GenerateCommandText(xStations, eYear);
            string cnnString = App.Workspace.AppData.RemoteConnectionString;
            List<MeteoElement> meteoElements = App.Workspace.AppHelper.GetMeteoElementCollection(cnnString, cmdText, xStations);
            if (meteoElements == null || meteoElements.Count == 0)
            {
                MessageBox.Show("没有得到气象要素数据!");
                return;
            }

            SetupXStation(xStations, meteoElements);

            DoXStation(xStations);

            SaveData();

            //
            //END
            //
        }



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

        private string GenerateCommandText(List<XStation> xStations, int year)
        {
            //FUN_BEGIN

            string cmdText = "";
            // 评估年份
            int eYear = year;

            // 构建选定站点ID字符串，用于SQL语句条件
            // 格式如('57083','57090','53889')
            string strStationIn = App.Workspace.AppTools.GetStationIn(_XStations);
            if (string.IsNullOrEmpty(strStationIn))
            {
                MessageBox.Show("没有找到符合当前区域条件的配置站点数据!", "提示");
                return cmdText;
            }

            // CommandText
            string strFields = string.Format("[iiiii],[ObvDate],[T],[Tmax],[Tmin],[E],[S],[F10],[R]");
            string strTables = string.Format("[{0}]", App.Workspace.AppTools.ConvertTableName(eYear));
            string strWheres = string.Format("[ObvDate]>={0}0501 AND [ObvDate]<={0}1030 AND [iiiii] IN ({1})", eYear, strStationIn);
            string strOders = string.Format("[ObvDate],[iiiii]");
            string strSql = string.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}",
                                           strFields, strTables, strWheres, strOders);

            //App.Workspace.AppData.RemoteCommandText = strSql;
            cmdText = strSql;

            return cmdText;

            //FUN_END
        }

        private void SetupXStation(List<XStation> xStations)
        {
            xStations.ForEach(p =>
            {
                p.Fa = -999;
                p.Fae = -1;
            });
        }
        private void SetupXStation(List<XStation> xStations, List<MeteoElement> meteoElements)
        {
            //设置站点气象要素数据
            xStations.ForEach(p =>
            {
                p.MeteoElements?.Clear();
                p.MeteoElements = (from me in meteoElements
                                   where me.StationId == p.Id
                                   orderby me.DateTime// descending
                                   select me)
                                   .ToList()
                                   ;
            });
        }
        private void SetupXStation(List<XStation> xStations, int year, Region currentRegion, List<CropGrwp> cropGrwps, List<CropWorkspace> cropWorkspaces)
        {
            //设置站点参数
            xStations.ForEach(p =>
            {
                p.Year = year;                         //评估年份
                p.CropGrwps = cropGrwps;               //作物发育期
                p.CropWorkspaces = cropWorkspaces;     //作物工作空间
                p.FaMax = currentRegion.Cmax;          //发育期适宜度最小值
                p.FaMin = currentRegion.Cmin;          //发育期适宜度最大值
            });
        }

        private void DoXStation(List<XStation> xStations)
        {
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
                throw new Exception("Error in  DoXStation(...)" + stemp);
            }

            //取得所有站点发育期适宜度的最大最小值 - 还有用
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


#if DEBUG
            //TODO:====================================Test
            XStation[] xa0 = xStations.FindAll(p => p.Fae == 0).ToArray();     //不适宜
            XStation[] xa1 = xStations.FindAll(p => p.Fae == 1).ToArray();     //次适宜
            XStation[] xa2 = xStations.FindAll(p => p.Fae == 2).ToArray();     //适宜
            XStation[] xa3 = xStations.FindAll(p => p.Fae == -1).ToArray();    //未知
            //=========================================
#endif
        }

        private void SaveData()
        {
            int eYear = App.Workspace.AppData.Year;
            Region region = _CurrentRegion;
            Crop crop = _CurrentCrop;
            CropCultivar cropCultivar = _CurrentCropCultivar;
            string fileName = App.Workspace.AppTools.GenerateFileName( eYear, region, crop, cropCultivar);
            string filePath = System.IO.Path.Combine(App.OutputPath, fileName);

            List<XStation> xStations = _XStations;

            OutputStationFile(xStations, filePath);

            //改变
            App.Workspace.ChangeFilePath(filePath);

            //
            //end
            //
        }

        private void OutputStationFile(List<XStation> xStations, string fileName)
        {
            int year = App.Workspace.AppData.Year; //评估年份
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
                axin30di.DateTime = new DateTime(year, 1, 1);
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

            //
            //END_OF_FUNCTION
            //
        }




        //TODO:waiting...
        protected override void OnFilePathChanged(string filePath)
        {
            try
            {
                var vm = App.Workspace.Documents.FirstOrDefault(p => p is EvaluReportViewModel);
                if (vm == null)
                {
                    Workspace.Instance.Documents.ToList().ForEach(p =>
                    {
                        if (p is DataGridViewModelBase)
                            Workspace.Instance.Documents.Remove(p);
                    });

                    Workspace.Instance.Documents.Add(App.Workspace.EvaluReportViewModel);
                }

                IProvider provider = new AxinFileProvider(filePath);
                AxinStationFile axinStationFile = provider?.DataInstance as AxinStationFile;

                this.StationInfos = null;
                this.StationInfos = axinStationFile?.StationInfos;

                App.Workspace.EvaluReportViewModel.ToolTip = filePath;
                //App.Workspace.ActiveDocument = App.Workspace.EvaluReportViewModel;
            }
            catch (Exception ex)
            {
                String errMessage = ex.Message;
                this.StationInfos = null;
            }
        }




    }
}
