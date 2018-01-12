using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;

using CSharpKit.Windows.Input;
using Mescp.Models;


namespace Mescp
{
    public class AppData
    {
        internal AppData()
        {
            this._Year = -1;

            _IsContour = true;
            _IsStation = !IsContour;

            // 
            int year = this.Year;
            List<CropGrwp> cropGrwps = this.CropGrwps.FindAll(p => p.CropID == "C02");
            CropGrwp cg1 = cropGrwps[0];
            CropGrwp cg2 = cropGrwps[cropGrwps.Count - 1];

            this.EvlDateTimeBeg = cg1.GrwpBeg(year);
            this.EvlDateTimeEnd = cg2.GrwpEnd(year);

            //
            //end
            //
        }

        // 配置数据库

        #region 区域

        #region Regions

        private List<Region> _Regions;
        public List<Region> Regions
        {
            get
            {
                if (_Regions == null || _Regions.Count == 0)
                {
                    _Regions = new List<Region>();
                    App.Workspace.AppHelper.GetRegions(_Regions);
                }
                return _Regions;
            }
        }

        #endregion

        #region CurrentRegion

        public Region CurrentRegion { get; set; }

        #endregion

        #endregion

        #region 作物

        #region Crops

        private List<Crop> _Crops;
        public List<Crop> Crops
        {
            get
            {
                if (_Crops == null || _Crops.Count == 0)
                {
                    _Crops = new List<Crop>();
                    App.Workspace.AppHelper.GetCrops(_Crops);

                    this.CurrentCrop = _Crops.Find(c => c.CropID == "C02");
                }
                return _Crops;
            }
        }

        #endregion

        #region CurrentCrop

        private Crop _CurrentCrop;
        public Crop CurrentCrop
        {
            get
            {
                if (_CurrentCrop == null)
                    _CurrentCrop = Crops.Find(c => c.CropID == "C02");

                return _CurrentCrop;
            }
            set
            {
                _CurrentCrop = value;
            }
        }

        #endregion

        #endregion

        #region 作物品种

        #region CropCultivars

        private List<CropCultivar> _CropCultivars;
        public List<CropCultivar> CropCultivars
        {
            get
            {
                if (_CropCultivars == null || _CropCultivars.Count == 0)
                {
                    _CropCultivars = new List<CropCultivar>();
                    App.Workspace.AppHelper.GetCropCultivars(_CropCultivars, this.CurrentCrop);
                }
                return _CropCultivars;
            }
        }

        #endregion

        #region CurrentCropCultivar

        private CropCultivar _CurrentCropCultivar;
        public CropCultivar CurrentCropCultivar
        {
            get
            {
                if (_CurrentCropCultivar == null)
                {
                    _CurrentCropCultivar = this.CropCultivars[0];
                }
                return _CurrentCropCultivar;
            }
            set
            {
                _CurrentCropCultivar = value;
            }
        }

        #endregion

        #endregion

        #region 作物发育期

        #region CropGrwps

        private List<CropGrwp> _CropGrwps;
        public List<CropGrwp> CropGrwps
        {
            get
            {
                if (_CropGrwps == null)
                {
                    _CropGrwps = new List<CropGrwp>();
                    App.Workspace.AppHelper.GetCropGrwps(_CropGrwps);
                }

                return _CropGrwps;
            }
        }

        #endregion

        #region CurrentCropGrwp

        public CropGrwp CurrentCropGrwp { get; set; }

        #endregion

        #endregion

        #region 作物工作空间

        #region CropWorkspaces

        private List<CropWorkspace> _CropWorkspaces;
        public List<CropWorkspace> CropWorkspaces
        {
            get
            {
                if (_CropWorkspaces == null)
                {
                    _CropWorkspaces = new List<CropWorkspace>();
                    App.Workspace.AppHelper.GetCropWorkspaces(_CropWorkspaces);
                }
                return _CropWorkspaces;
            }
        }

        #endregion

        #region CurrentCropWorkspace

        public CropWorkspace CurrentCropWorkspace { get; set; }

        #endregion

        #endregion

        #region 站点

        private List<XStation> _XStations;
        public List<XStation> XStations
        {
            get
            {
                if (_XStations == null)
                {
                    _XStations = new List<XStation>();
                    App.Workspace.AppHelper.GetStations(_XStations);
                }
                return _XStations;
            }
        }

        #endregion

        #region 区划

        private List<Compartment> _Compartments;
        public List<Compartment> Compartments
        {
            get
            {
                if (_Compartments == null)
                {
                    _Compartments = new List<Compartment>();
                    App.Workspace.AppHelper.GetCompartments(_Compartments);
                }
                return _Compartments;
            }
        }

        #endregion


        #region 评价年份

        int _Year;
        public int Year
        {
            get
            {
                if (_Year < 0)
                {
                    _Year = DateTime.Now.Year;
                }
                return _Year;
            }
            set
            {
                _Year = value;
            }
        }

        List<YClass> _Years;
        public List<YClass> Years
        {
            get
            {
                if (null == _Years)
                {
                    _Years = new List<YClass>();

                    int yearCount = 17;
                    TimeSpan ts = TimeSpan.FromDays(365 * yearCount);
                    DateTime dtBeg = DateTime.Now - ts;
                    DateTime dtEnd = DateTime.Now;
                    for (int i = dtBeg.Year; i <= dtEnd.Year; i++)
                    {
                        YClass yc = new YClass();
                        yc.Year = i.ToString();
                        _Years.Add(yc);
                    }
                }
                return _Years;
            }
        }

        #endregion

        #region 评价时段

        private DateTime _dBeg = DateTime.Now;
        /// <summary>
        /// 评估起始日期
        /// </summary>
        public DateTime EvlDateTimeBeg
        {
            get { return _dBeg; }
            set { _dBeg = value; }
        }

        private DateTime _dEnd = DateTime.Now + TimeSpan.FromDays(30);
        /// <summary>
        /// 评估终止日期
        /// </summary>
        public DateTime EvlDateTimeEnd
        {
            get { return _dEnd; }
            set { _dEnd = value; }
        }

        #endregion

        #region 图形显示 -- 是等高线色斑图还是站点填充图

        Boolean _IsContour;
        public Boolean IsContour
        {
            get { return _IsContour; }
            set
            {
                _IsContour = value;
                App.Workspace.MapViewModel.Presentation();  //数据呈现
            }
        }

        Boolean _IsStation;
        public Boolean IsStation
        {
            get { return _IsStation; }
            set
            {
                _IsStation = value;
                App.Workspace.MapViewModel.Presentation();  //数据呈现
            }
        }

        #endregion

        #region 操作图层ID

        private readonly string _LayerID1 = "_ID1_31BF3856AD36_1DI_";
        /// <summary>
        /// 站点填充图层
        /// </summary>
        public String LayerID1
        {
            get { return _LayerID1; }
        }

        private readonly string _LayerID2 = "_ID2_B77A5C561934_2DI_";
        /// <summary>
        /// 等高线图层
        /// </summary>
        public String LayerID2
        {
            get { return _LayerID2; }
        }

        #endregion


        #region 远程数据库

        static string ip = "10.10.10.100";
        //static string ip = "172.18.152.243";

        string _RemoteDataSource = ip;
        string _RemoteInitialCatalog = "HenanClimate";
        string _RemoteUserID = "nqzx";
        string _RemotePassword = "KyCen5946";

        public String RemoteDataSource
        {
            get { return _RemoteDataSource; }
        }

        public String RemoteConnectionString
        {
            get
            {
                string cnnString = string.Format(@"Data Source={0}; Initial Catalog={1}; User ID={2}; Password={3};",
                                                  RemoteDataSource,         //[0]Data Source
                                                  _RemoteInitialCatalog,    //[1]Initial Catalog
                                                  _RemoteUserID,            //[2]User ID
                                                  _RemotePassword);         //[3]Password
                return cnnString;
            }
        }

        #endregion



    }


}
