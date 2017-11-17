/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     RgnName: MapViewModel
 *  Version: 
 * Function: 
 * 
 *   Author: 申永辰.郑州 (shenyczz@163.com)
 * DateTime: 2010 - 
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Windows.Input;
using CSharpKit;
using CSharpKit.Data;
using CSharpKit.Data.Axin;
using CSharpKit.Data.Esri;
using CSharpKit.Vision;
using CSharpKit.Vision.Mapping;
using CSharpKit.Vision.Presentation;
using CSharpKit.Win32;
using CSharpKit.Windows.Input;
using Microsoft.Win32;
using System.Windows;
using CSharpKit.Palettes;
using System.Collections.Generic;

namespace Mescp.ViewModels
{
    public sealed class MapViewModel : VmMapPrimiviteBase
    {
        public MapViewModel()
        {
            this.IsMapViewModel = true;

            InitializeMap();
            InitializeMapData();
        }


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
            this.Map.Rendered += Map_Rendered;
        }

        private void InitializeMapData()
        {
            IMap map = this.Map;

            // 图层清零
            map.LayerManager.Clear();

            // 区域代码
            string regionCode = this.RegionCode;

            #region 图元图层

            try
            {
                //1.添加图源图层
                IVision vision = new GeometryVision("图元图层")
                {
                    Provider = null,
                    Renderer = new WfmGeometryVisionRenderer(),

                    IsAllowDeleted = false,
                    RenderPriority = RenderPriority.Topmost,
                };

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
                //2.河南省县界
                String fileName = System.IO.Path.Combine(App.MapPath, string.Format("{0}\\县市界.shp", regionCode));
                IVision vision = new ShapeVision("县界")
                {
                    Provider = new ShapeFileProvider(fileName),
                    Renderer = new WfmShapeVisionRenderer(),

                    IsAllowDeleted = false,
                    IsClipPathData = true,
                    IsFill = true,
                    Transparency = 0,
                };

                map.LayerManager.Add(new Layer(App.Workspace.AppData.LayerID1, vision));
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
                String fileName = System.IO.Path.Combine(App.MapPath, string.Format("{0}\\县市点.shp", regionCode));
                IVision vision = new ShapeVision("县标注")
                {
                    Provider = new ShapeFileProvider(fileName),
                    Renderer = new WfmShapeVisionRenderer(),

                    IsVisible = true,

                    Foreground = System.Drawing.Color.Blue,
                    FontSize = 9,
                };

                map.LayerManager.Add(new Layer(vision));
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            #endregion


            #region 测试数据

            try
            {
                String fileName;
                fileName = "";
                //fileName = System.IO.Path.Combine(App.OutputPath, "30_9999.txt");
                //fileName = System.IO.Path.Combine(App.OutputPath, "40_9999.txt");
                //fileName = System.IO.Path.Combine(App.OutputPath, "2007.txt");
                //fileName = System.IO.Path.Combine(App.OutputPath, "2017_9999.txt");

                IProvider provider = new AxinFileProvider(fileName);
                IVision vision = new AxinVision(provider.DataInstance?.DataInfo.Comment)
                {
                    Provider = provider,
                    Renderer = new WfmAxinVisionRenderer(),

                    IsClip = true,
                    //IsColorContour = false,
                    IsFillContour = true,
                    //IsLabelContour = false,
                    //IsDrawContour = false,

                    Foreground = System.Drawing.Color.White,
                };

                map.LayerManager.Add(new Layer(App.Workspace.AppData.LayerID2, vision));
                App.Workspace.PropertyViewModel.VisionProperties = vision.CustomProperties;
            }
            catch(Exception ex)
            {
                string errMsg = ex.Message;
            }

            #endregion

        }

        private void Map_Rendered(object sender, EventArgs e)
        {
            //Console.WriteLine("地图绘制完成事件!");
        }

        #endregion

        #region CurrentCoordinate

        private string _CurrentCoordinate = "当前鼠标位置坐标";
        public string CurrentCoordinate
        {
            get { return _CurrentCoordinate; }
            set
            {
                _CurrentCoordinate = value;
                RaisePropertyChanged("CurrentCoordinate");
            }
        }

        #endregion

        #region RegionCode

        string _RegionCode = "";
        public String RegionCode
        {
            get { return _RegionCode; }
            set
            {
                if (value != null && value != _RegionCode)
                {
                    _RegionCode = value;
                    OnRegionCodeChanged();

                    RaisePropertyChanged("RegionCode");
                }
            }
        }

        void OnRegionCodeChanged()
        {
            InitializeMapData();
            Map.Reset();
        }

        #endregion

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

            ClearChecked();
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
            this.ClearChecked();
            this.IsChecked_ZoomIn = true;
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
            this.ClearChecked();
            this.IsChecked_ZoomOut = true;
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

            this.ClearChecked();
            this.IsCheckedZoomPan = true;
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
            sfd.InitialDirectory = App.ImagePath;
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

            CSharpKit.Windows.Forms.Controls.IMapControl mapControl = map.Container as CSharpKit.Windows.Forms.Controls.IMapControl;
            if (mapControl == null)
                return;

            int w = (int)mapControl.GetWidth();
            int h = (int)mapControl.GetHeight();

            RECT rc = new RECT(w, h);

            IntPtr ih = App.CaptureRect(mapControl.Handle, ref rc);
            System.Drawing.Bitmap bmp = System.Drawing.Bitmap.FromHbitmap(ih);
            bmp.Save(filePathName);
        }

        #endregion




        protected override void OnFilePathChanged(string filePath)
        {
            LoadData(filePath);

            Presentation();
        }

        #region Presentation - 数据呈现

        private IProvider _Provider;


        public void Presentation()
        {
            IProvider provider = _Provider;
            if (provider == null)
                return;

            if (App.Workspace.AppData.IsContour)
            {
                DisableStationColor(App.Workspace.AppData.LayerID1);
                this.FillStationContour(provider);
            }

            if (App.Workspace.AppData.IsStation)
            {
                this.DisableStationContour(App.Workspace.AppData.LayerID2);
                this.FillStationColor(provider);    //行政区填充
            }


        }


        private void LoadData(string filePath)
        {
            IProvider provider = null;

            try
            {
                provider = new AxinFileProvider(filePath);
            }
            catch (Exception)
            {
            }

            _Provider = provider;
        }

        private void FillStationContour(IProvider provider)
        {
            try
            {
                IMap map = App.Workspace.MapViewModel.Map;
                ILayer layer = map.LayerManager.GetLayer(App.Workspace.AppData.LayerID2);
                if (layer != null)
                {
                    map.LayerManager.Remove(layer);
                }

                IVision vision = new AxinVision(provider.DataInstance?.DataInfo.Comment)
                {
                    Provider = provider,
                    Renderer = new WfmAxinVisionRenderer(),

                    Transparency = 0,

                    IsClip = true,
                    IsColorContour = true,
                    IsFillContour = true,       //填充等高线
                    IsLabelContour = true,   //标注等高线
                    IsDrawContour = true,    //绘制等高线

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
        }

        private void FillStationColor(IProvider provider)
        {
            try
            {
                List<StationInfo> stationInfos = (provider.DataInstance as AxinStationFile).StationInfos;
                if (stationInfos == null || stationInfos.Count == 0)
                    return;

                //AxinColortabFile 颜色表文件
                string s = System.IO.Path.Combine(App.StartupPath, "Palettes\\6801.pal");   //使用索引调色板
                AxinColortabFile axinColortabFile = new AxinColortabFile(s);
                IPalette palette = axinColortabFile.Palette;    // 调色板

                //查找县界图层
                IMap map = App.Workspace.MapViewModel.Map;
                ILayer layer = map.LayerManager.GetLayer(App.Workspace.AppData.LayerID1);
                IVision vision = layer.Vision;

                // 取得Shape文件提供者
                ShapeFile shapeFile = vision.Provider.DataInstance as ShapeFile;
                shapeFile.Features.ForEach(p =>
                {
                    p.Tag = null;
                });

                foreach (StationInfo si in stationInfos)
                {
                    List<IFeature> features = shapeFile.Features.FindAll(p => p.Id == si.Id);
                    features.ForEach(p =>
                    {
                        double fae = si.ElementValues[1];    //站点适宜度值
                        System.Drawing.Color clr = palette.GetColor(fae, System.Drawing.Color.Black);
                        p.Tag = clr;
                    });
                }

                //刷新地图
                map.Refresh(true);

            }
            catch (Exception)
            {
            }
        }


        private void DisableStationContour(string layerId)
        {
            try
            {
                IMap map = App.Workspace.MapViewModel.Map;
                ILayer layer = map.LayerManager.GetLayer(layerId);
                IVision vision = layer.Vision;

                vision.IsDrawContour = false;
                vision.IsFillContour = false;
                vision.IsLabelContour = false;

                App.Workspace.PropertyViewModel.VisionProperties = null;
                App.Workspace.PropertyViewModel.VisionProperties = vision.CustomProperties;
            }
            catch (Exception)
            {
            }
        }

        private void DisableStationColor(string layerId)
        {
            try
            {
                IMap map = App.Workspace.MapViewModel.Map;
                ILayer layer = map.LayerManager.GetLayer(layerId);
                IVision vision = layer.Vision;
                ShapeFile shapeFile = vision.Provider.DataInstance as ShapeFile;

                List<IFeature> features = shapeFile.Features;
                features.ForEach(p =>
                {
                    p.Tag = null;
                });
            }
            catch (Exception)
            {
            }
        }


        [Obsolete("Not in use",true)]
        private void EnableStationColor(string layerId, bool enable)
        {
            try
            {
                IMap map = App.Workspace.MapViewModel.Map;
                ILayer layer = map.LayerManager.GetLayer(layerId);
                IVision vision = layer.Vision;
                vision.IsFill = enable;

                //ShapeFile shapeFile = vision?.Provider?.DataInstance as ShapeFile;
                //List<IFeature> features = shapeFile.Features;
                //features.ForEach(p =>
                //{
                //    p.Tag = null;
                //});

            }
            catch (Exception)
            {
            }
        }

        #endregion

    }
}
