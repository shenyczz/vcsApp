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

        private readonly string _LayerID = "a123456789";
        public String LayerID
        {
            get { return _LayerID; }
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

                map.LayerManager.Add(new Layer(_LayerID, vision));
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
                //String fileName = System.IO.Path.Combine(App.OutputPath, "30.txt");
                String fileName = System.IO.Path.Combine(App.OutputPath, "301.txt");
                //String fileName = System.IO.Path.Combine(App.OutputPath, "300.txt");
                //String fileName = System.IO.Path.Combine(App.OutputPath, "300_9999.txt");
                IProvider provider = new AxinFileProvider(fileName);
                IVision vision = new AxinVision(provider.DataInstance?.DataInfo.Comment)
                {
                    Provider = provider,
                    Renderer = new WfmAxinVisionRenderer(),

                    //IsClip = true,
                    //IsColorContour = false,
                    IsFillContour = true,
                    //IsLabelContour = false,
                    //IsDrawContour = false,

                    Foreground = System.Drawing.Color.White,
                };

                map.LayerManager.Add(new Layer("t123", vision));
                App.Workspace.PropertyViewModel.VisionProperties = vision.CustomProperties;
            }
            catch { }

            #endregion

        }

        private void Map_Rendered(object sender, EventArgs e)
        {
            //IMap map = App.Workspace?.MapViewModel?.Map;
            //if (map == null)
            //    return;

            //ILayer layer = map.LayerManager.Layers.Find(l => l.Id == "t123");
            //IVision vision = layer?.Vision;
            //App.Workspace.PropertyViewModel.VisionProperties = vision?.CustomProperties;

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
            //Map.Refresh(true);
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


    }
}
