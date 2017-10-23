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
            this.Year = DateTime.Now.Year;
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
                    App.Workspace.AppHelper.GetCropCultivars(_CropCultivars, CurrentCrop);
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

        #region 评价时段

        #region 评价起始日期

        private DateTime _dBeg = DateTime.Now;
        public DateTime DtBeg
        {
            get { return _dBeg; }
            set { _dBeg = value; }
        }

        #endregion

        #region 评价终止日期

        private DateTime _dEnd = DateTime.Now + TimeSpan.FromDays(10);
        public DateTime DtEnd
        {
            get { return _dEnd; }
            set { _dEnd = value; }
        }

        #endregion

        #endregion

        #region 评价年份

        public int Year { get; set; }

        #endregion

    }
}
