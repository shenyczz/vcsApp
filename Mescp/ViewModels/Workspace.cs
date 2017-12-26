/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     RgnName: Workspace
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mescp.ViewModels;
using Mescp.Misc;
using CSharpKit.Windows.Input;
using System.Windows.Input;

namespace Mescp.ViewModels
{
    public class Workspace : ViewModelBase
    {
        protected Workspace()
        {
            this.Documents = new ObservableCollection<DocumentViewModel>()
            {
                this.MapViewModel,
                //this.EvaluReportViewModel,
            };

            this.Tools = new List<ToolViewModel>()
            {
                //this.LayerViewModel,
                this.PropertyViewModel,
            };
        }


        #region Instance

        private static Workspace _Instance;
        public static Workspace Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Workspace();
                }
                return _Instance;
            }
        }

        #endregion

        

        #region Documents

        public ObservableCollection<DocumentViewModel> Documents { get; private set; }

        #endregion

        #region Tools

        public List<ToolViewModel> Tools
        { get; private set; }

        #endregion

        #region ActiveDocument

        private DocumentViewModel _ActiveDocument;
        public DocumentViewModel ActiveDocument
        {
            get { return _ActiveDocument; }
            set
            {
                if (_ActiveDocument != value)
                {
                    _ActiveDocument = value;
                    RaisePropertyChanged("ActiveDocument");

                    OnActiveDocumentChanged(_ActiveDocument);
                }
            }
        }
        protected void OnActiveDocumentChanged(object sender)
        {
            Console.WriteLine(this.ActiveDocument.ToString());
            MapViewModel.IsMapViewModel = this.ActiveDocument is MapViewModel;
        }

        #endregion



        #region AppCommands

        private AppCommands _AppCommands;
        public AppCommands AppCommands
        {
            get
            {
                if (_AppCommands == null)
                {
                    _AppCommands = new AppCommands();
                }

                return _AppCommands;
            }
        }

        #endregion

        #region AppData

        private AppData _AppData;
        public AppData AppData
        {
            get
            {
                if (_AppData == null)
                {
                    _AppData = new AppData();
                }

                return _AppData;
            }
        }

        #endregion

        #region AppHelper

        AppHelper _AppHelper;
        public AppHelper AppHelper
        {
            get
            {
                if (_AppHelper == null)
                {
                    _AppHelper = new AppHelper();
                }

                return _AppHelper;
            }
        }

        #endregion

        #region AppTools

        AppTools _AppTools;
        public AppTools AppTools
        {
            get
            {
                if (_AppTools == null)
                {
                    _AppTools = new AppTools();
                }

                return _AppTools;
            }
        }

        #endregion

        #region BusinessMethords

        BusinessMethords _BusinessMethords;
        public BusinessMethords BusinessMethords
        {
            get
            {
                if (_BusinessMethords == null)
                {
                    _BusinessMethords = new BusinessMethords();
                }

                return _BusinessMethords;
            }
        }

        #endregion

        #region MapHelper

        //private MapHelper _MapHelper;
        //public MapHelper MapHelper
        //{
        //    get
        //    {
        //        if (_MapHelper == null)
        //        {
        //            _MapHelper = new MapHelper();
        //        }

        //        return _MapHelper;
        //    }
        //}

        #endregion



        #region MapViewModel

        private MapViewModel _MapViewModel;
        public MapViewModel MapViewModel
        {
            get
            {
                if (_MapViewModel == null)
                {
                    System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("pack://application:,,,/Assets/map_globe.png");
                    bi.EndInit();
                    _MapViewModel = new MapViewModel()
                    {
                        Title = "地理信息",
                        ToolTip = null,
                        IconSource = bi,
                    };
                }

                return _MapViewModel;
            }
        }

        #endregion

        #region EvaluateReportViewModel - 评估报告

        private EvaluReportViewModel _EvaluReportViewModel;
        public EvaluReportViewModel EvaluReportViewModel
        {
            get
            {
                if (_EvaluReportViewModel == null)
                {
                    _EvaluReportViewModel = new EvaluReportViewModel()
                    {
                        Title = "评估报告",
                    };
                }
                return _EvaluReportViewModel;
            }
        }

        #endregion

        #region LayerViewModel

        private LayerViewModel _LayerViewModel;
        public LayerViewModel LayerViewModel
        {
            get
            {
                if (_LayerViewModel == null)
                {
                    _LayerViewModel = new LayerViewModel()
                    {
                        Title = "图层管理",
                    };
                }

                return _LayerViewModel;
            }
       }

        #endregion

        #region PropertyViewModel

        private PropertyViewModel _PropertyViewModel;
        public PropertyViewModel PropertyViewModel
        {
            get
            {
                if (_PropertyViewModel == null)
                {
                    _PropertyViewModel = new PropertyViewModel()
                    {
                        Title = "属性窗口",
                    };
                }
                return _PropertyViewModel;
            }
        }

        #endregion

        #region PrimiviteViewModel

        private PrimiviteViewModel _PrimiviteViewModel;
        public PrimiviteViewModel PrimiviteViewModel
        {
            get
            {
                if (_PrimiviteViewModel == null)
                {
                    _PrimiviteViewModel = new PrimiviteViewModel();
                }
                return _PrimiviteViewModel;
            }
        }

        #endregion



        #region Public Functions

        public void Close(DocumentViewModel vm)
        {
            this.Documents.Remove(vm);
        }

        public void ChangeFilePath(string filePath)
        {
            App.Workspace.MapViewModel.FilePath = filePath;
            App.Workspace.EvaluReportViewModel.FilePath = filePath;
        }

        #endregion

    }
}
