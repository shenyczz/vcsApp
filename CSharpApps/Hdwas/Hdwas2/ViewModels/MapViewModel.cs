/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: MapViewModel
 *  Version: 
 * Function: 
 * 
 *   Author: 申永辰.郑州 (shenyczz@163.com)
 * DateTime: 2010 - 
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CSharpKit;
using CSharpKit.Win32;
using CSharpKit.Data;
using CSharpKit.Data.Axin;
using CSharpKit.Data.Esri;
using CSharpKit.Devices.Input;
using CSharpKit.Palettes;
using CSharpKit.Vision;
using CSharpKit.Vision.Mapping;
using CSharpKit.Vision.Presentation;
using CSharpKit.Windows.Forms.Controls;
using Microsoft.Win32;

namespace Hdwas
{
    public sealed class MapViewModel : DocumentViewModel
    {
        public MapViewModel()
        {
            this.IsMapViewModel = true;

            InitializeMap();
            InitializeMapData();
        }

        private readonly string layerID = "a123456789";

        #region Map

        private IMap _Map;
        public IMap Map
        {
            get { return _Map; }
            set
            {
                _Map = value;
                RaisePropertyChanged("Map");
            }
        }

        private void InitializeMap()
        {
            this.Map = new Map();
            this.Map.Rendered += Map_Rendered;  // 地图绘制完成事件
        }

        private void InitializeMapData()
        {
            IMap map = this.Map;

            string areaCode = "410000";

            #region 图元图层

            try
            {
                // 添加图元图层
                IProvider provider = null;
                IRenderer renderer = new WfmGeometryVisionRenderer();
                IVision vision = GeometryVision.CreateGeometryVision("GeometryVision", provider, renderer);
                vision.Comment = "图元图层";
                vision.IsAllowDeleted = false;
                vision.RenderPriority = RenderPriority.Topmost;
                map.LayerManager.PrimiviteLayer = new Layer(vision);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            #endregion

            #region 县界

            try
            {
                // 河南省县界
                String fileName = System.IO.Path.Combine(TheApp.MapPath, string.Format("{0}\\Boundary.shp", areaCode));
                IProvider provider = new ShapeFileProvider(fileName);
                IRenderer renderer = new WfmShapeVisionRenderer();
                IVision vision = new ShapeVision("县界", provider, renderer);
                vision.IsAllowDeleted = false;
                vision.Transparency = 0;
                vision.IsFill = true;
                //(vision as ShapeVision).IsClipPathData = true;
                //ILayer layer = new Layer(TheApp.MapLayerId_County, vision);
                ILayer layer = new Layer(this.layerID, vision);
                map.LayerManager.Add(layer);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            #endregion

            #region 县标注

            try
            {
                // 河南省县标注
                String fileName = System.IO.Path.Combine(TheApp.MapPath, string.Format("{0}\\Label.shp", areaCode));
                IProvider provider = new ShapeFileProvider(fileName);
                IRenderer renderer = new WfmShapeVisionRenderer();
                IVision vision = new ShapeVision("县标注", provider, renderer);
                vision.FontSize = 8;
                (vision as VisionBase).Foreground = System.Drawing.Color.Blue;
                map.LayerManager.Add(new Layer(vision));
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            #endregion
        }

        private void Map_Rendered(object sender, EventArgs e)
        {
            Workspace.Instance.MapFunctions.OnMapRendered(sender as IMap);
        }

        #endregion

        public int HdwFlag { get; set; }

        private String Title1
        { 
            get
            {
                string title1 = "";
                switch(HdwFlag)
                {
                    case 1:
                        title1 = string.Format("{0}冬小麦干热风监测", "河南省");
                        break;
                    case 2:
                        title1 = string.Format("{0}冬小麦干热风过程", "河南省");
                        break;
                }
                
                return title1;
            }
        }


        protected override void OnFilePathChanged(string filePath)
        {
            // 显示数据的空间分布
            this.FillCountyColor(filePath);
        }

        /// <summary>
        /// 显示数据的空间分布
        /// </summary>
        /// <param name="filePathName"></param>
        private void FillCountyColor(string filePathName)
        {
            try
            {
                IMap map = this.Map;
                ILayer layer = map.LayerManager.Layers.Find(l => l.Id == this.layerID);
                IVision vision = layer.Vision;
                IProvider provider = vision.Provider;
                IDataInstance dataInstance = provider.DataInstance;

                // 取得Shape文件提供者
                ShapeFile shapeFile = dataInstance as ShapeFile;

                IProvider dataProvider = new AxinFileProvider(filePathName);

                AxinStationFile axinStationFile = dataProvider.DataInstance as AxinStationFile;
                List<StationInfo> stationInfos = axinStationFile.StationInfos;
                IPalette palette = axinStationFile.Palette;

                foreach (StationInfo si in stationInfos)
                {
                    List<IFeature> features = shapeFile.Features.FindAll(f => f.Id == si.Id);
                    foreach (IFeature f in features)
                    {
                        System.Drawing.Color clr = palette.GetColor(si.CurrentElementValue, System.Drawing.Color.Green);
                        f.Tag = clr;
                    }
                }

                map.Refresh(true);

                FileInfo fi = new FileInfo(filePathName);
                Workspace.Instance.MapFunctions.Palette = palette;

                //Workspace.Instance.MapFunctions.Title1 = string.Format("{0}冬小麦干热风灾害评估", TheApp.AreaName);
                Workspace.Instance.MapFunctions.Title1 = this.Title1;
                Workspace.Instance.MapFunctions.Title2 = fi.Name.Replace(".txt", "");
                Workspace.Instance.MapFunctions.Title3 = "河南省气象科学研究所农气中心制作";
                //Workspace.Instance.MapFunctions.LegendTitle = "减产率(%)";
                Workspace.Instance.MapFunctions.LegendTitle = "图例";

            }
            catch
            {
            }
        }



        #region IsMapViewModel

        public Boolean IsMapViewModel { get; set; }

        #endregion

        #region CancelCommand

        private RelayCommand _CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new RelayCommand(p => OnCancel(p), p => CanCancel(p));
                }

                return _CancelCommand;
            }
        }

        private void OnCancel(Object parameter)
        {
            this.Map.MapTool = MapTool.MapController.Cancel;
        }

        private Boolean CanCancel(Object parameter)
        {
            return IsMapViewModel;
        }

        #endregion

        #region ResetCommand

        private RelayCommand _ResetCommand;
        public ICommand ResetCommand
        {
            get
            {
                if (_ResetCommand == null)
                {
                    _ResetCommand = new RelayCommand(p => OnReset(p), p => CanReset(p));
                }

                return _ResetCommand;
            }
        }

        private void OnReset(Object parameter)
        {
            this.Map.Reset();
            this.Map.Refresh(true);
        }

        private Boolean CanReset(Object parameter)
        {
            return IsMapViewModel;
        }

        #endregion

        #region RefreshCommand

        private RelayCommand _RefreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_RefreshCommand == null)
                {
                    _RefreshCommand = new RelayCommand(p => OnRefresh(p), p => CanRefresh(p));
                }

                return _RefreshCommand;
            }
        }
        private void OnRefresh(Object parameter)
        {
            this.Map.MapTool = MapTool.MapController.Refresh;
            this.Map.Refresh(true);

        }

        private Boolean CanRefresh(Object parameter)
        {
            return IsMapViewModel;
        }

        #endregion

        #region ZoomInCommand

        private RelayCommand _ZoomInCommand;
        public ICommand ZoomInCommand
        {
            get
            {
                if (_ZoomInCommand == null)
                {
                    _ZoomInCommand = new RelayCommand(p => OnZoomIn(p), p => CanZoomIn(p));
                }

                return _ZoomInCommand;
            }
        }

        private void OnZoomIn(Object parameter)
        {
            this.Map.MapTool = MapTool.MapController.ZoomIn;
        }

        private Boolean CanZoomIn(Object parameter)
        {
            return IsMapViewModel;
        }

        #endregion

        #region ZoomOutCommand

        private RelayCommand _ZoomOutCommand;
        public ICommand ZoomOutCommand
        {
            get
            {
                if (_ZoomOutCommand == null)
                {
                    _ZoomOutCommand = new RelayCommand(p => OnZoomOut(p), p => CanZoomOut(p));
                }

                return _ZoomOutCommand;
            }
        }

        private void OnZoomOut(Object parameter)
        {
            this.Map.MapTool = MapTool.MapController.ZoomOut;
        }

        private Boolean CanZoomOut(Object parameter)
        {
            return IsMapViewModel;
        }

        #endregion

        #region ZoomPanCommand

        private RelayCommand _ZoomPanCommand;
        public ICommand ZoomPanCommand
        {
            get
            {
                if (_ZoomPanCommand == null)
                {
                    _ZoomPanCommand = new RelayCommand(p => OnZoomPan(p), p => CanZoomPan(p));
                }

                return _ZoomPanCommand;
            }
        }

        private void OnZoomPan(Object parameter)
        {
            this.Map.MapTool = MapTool.MapController.ZoomPan;
        }

        private Boolean CanZoomPan(Object parameter)
        {
            return IsMapViewModel;
        }

        #endregion


        #region SaveImageCommand

        private RelayCommand _SaveImageCommand;
        public ICommand SaveImageCommand
        {
            get
            {
                if (_SaveImageCommand == null)
                {
                    _SaveImageCommand = new RelayCommand(p => OnSaveImage(p), p => CanSaveImage(p));
                }

                return _SaveImageCommand;
            }
        }

        private void OnSaveImage(Object parameter)
        {
            try
            {
                SaveImage();
            }
            catch (Exception)
            {
            }
        }

        private Boolean CanSaveImage(Object parameter)
        {
            return this.IsSelected;
        }

        private void SaveImage()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = TheApp.ImagePath;
            sfd.FileName = "未命名";
            sfd.Filter = "PNG图片|*.png|JPG图片|*.jpg";
            sfd.AddExtension = true;

            if ((bool)sfd.ShowDialog())
            {
                SaveImage(sfd.FileName);
            }
        }
        private void SaveImage(string filePathName)
        {
            IMap map = this.Map;

            IMapControl mapControl = map.Container as IMapControl;
            if (mapControl == null)
                return;

            int w = (int)mapControl.GetWidth();
            int h = (int)mapControl.GetHeight();

            RECT rc = new RECT(w, h);

            IntPtr ih = TheApp.CaptureRect(mapControl.Handle, ref rc);
            System.Drawing.Bitmap bmp = System.Drawing.Bitmap.FromHbitmap(ih);
            bmp.Save(filePathName);
        }

        #endregion

    }
}
