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
 * DateTime: 2010 - 2015
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CSharpKit;
using CSharpKit.Data;
using CSharpKit.Data.Axin;
using CSharpKit.Data.Esri;
using CSharpKit.Devices.Input;
using CSharpKit.Palettes;
using CSharpKit.Vision;
using CSharpKit.Vision.Mapping;
using CSharpKit.Vision.Presentation;
using CSharpKit.Win32;
using CSharpKit.Windows.Anchoring.Commands;
using CSharpKit.Windows.Forms.Controls;
using Microsoft.Win32;
using Amrs.Modis;
using CSharpKit.Windows.Input;

namespace Amrs.ViewModels
{
    public class MapViewModel : DocViewModel
    {
        public MapViewModel()
        {
            this.IsMapViewModel = true;

            InitializeMap();
            InitializeMapData();
        }

        private readonly string cid = "a123456789";

        #region Map

        private IMap _Map;
        public IMap Map
        {
            get { return _Map; }
            set
            {
                _Map = value;
                this.RaisePropertyChanged("Map");
            }
        }

        private void InitializeMap()
        {
            this.Map = new Map();
            this.Map.Rendered += Map_Rendered;
            this.Map.LayerManager.LayerChanged += LayerManager_LayerChanged;
        }

        private void InitializeMapData()
        {
            IMap map = this.Map;

            string areaCode = "410000";

            string mapPath = TheApp.MapPath;

            #region 图元图层

            try
            {
                // 添加图元图层
                IProvider provider = null;
                IRenderer renderer = new WfmGeometryVisionRenderer();
                IVision vision = GeometryVision.CreateGeometryVision("图元图层", provider, renderer);
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
                String fileName = System.IO.Path.Combine(mapPath, string.Format("{0}\\Boundary.shp", areaCode));
                IProvider provider = new ShapeFileProvider(fileName);
                IRenderer renderer = new WfmShapeVisionRenderer();
                IVision vision = new ShapeVision("县界", provider, renderer);
                vision.IsAllowDeleted = false;
                vision.Transparency = 0;
                vision.IsFill = true;
                //vision.Foreground = Color.Red;
                (vision as ShapeVision).IsClipPathData = true;
                //ILayer layer = new Layer(TheApp.MapLayerId_County, vision);
                ILayer layer = new Layer(this.cid, vision);
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
                String fileName = System.IO.Path.Combine(mapPath, string.Format("{0}\\Label.shp", areaCode));
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
            //Workspace.Instance.MapFunctions.OnMapRendered(sender as IMap);
        }

        private void LayerManager_LayerChanged(object sender, LayerChangedEventArgs e)
        {
            //ILayer l = e.Layer;
            //LayerChangedType lct = e.LayerChangedType;

            //IVision v = l.Vision;

            //TreeViewNode root = TheApp.Workspace.LayerViewModel.Layers[0];
            //root.AddChildren(new TreeViewNode(v.Name), true);
            //root.IsExpanded = true;

            return;
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

        #region OpenAmrsCommand

        private RelayCommand _OpenAmrsCommand;
        public ICommand OpenAmrsCommand
        {
            get
            {
                if (_OpenAmrsCommand == null)
                {
                    _OpenAmrsCommand = new RelayCommand(p => OnOpenAmrsCommand(p), p => CanOpenAmrsCommand(p));
                }

                return _OpenAmrsCommand;
            }
        }

        private void OnOpenAmrsCommand(Object parameter)
        {
            OpenAmrs();
            //Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            //ofd.InitialDirectory = TheApp.ConfigPath;
            //ofd.Filter = "配置文件|*.xml";

            //if ((bool)ofd.ShowDialog())
            //{
            //    //Load(ofd.FileName);
            //}
        }

        private Boolean CanOpenAmrsCommand(Object parameter)
        {
            return IsMapViewModel;
        }

        #endregion




        private void OpenAmrs()
        {
            //int n = Marshal.SizeOf(typeof(LD2Header));
            //n = Marshal.SizeOf(typeof(LDHeader));
            //n = Marshal.SizeOf(typeof(LD3Header));

            //string fileName = @"D:\shenyc\vs2013\vcs\Amrs\Amrs\bin\Code\_TestData\terra_2011_02_20_03_35_sy_250_河南250.ld2";
            string fileName = System.IO.Path.Combine(TheApp.StartupPath, @"_TestData\terra_2011_02_20_03_35_sy_250_河南250.ld2");
            IProvider provider = new ModisFileProvider(fileName);
            IRenderer renderer = new WfmAmrsVisionRenderer();
            IVision vision = new AmrsVision("1234", provider, renderer);
            //vision.IsClip = true;
            ILayer layer = new Layer(vision);
            this.Map.LayerManager.Add(layer);
        }


    }
}
