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

namespace Mescp.ViewModels
{
    public class Workspace : ViewModelBase
    {
        static Workspace()
        {
            _Instance = new Workspace();
        }

        protected Workspace()
        {
            this.Tools = new List<ToolViewModel>();
            this.Documents = new ObservableCollection<DocumentViewModel>();

            this.Documents.Add(this.MapViewModel);

            this.Tools.Add(this.PropertyViewModel);
            //this.Tools.Add(this.LayerViewModel);

            _MapHelper = new MapHelper();
        }

        #region Instance

        private static Workspace _Instance;
        public static Workspace Instance
        {
            get { return _Instance; }
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

        #region AppMethod

        AppMethod _AppMethod;
        public AppMethod AppMethod
        {
            get
            {
                if (_AppMethod == null)
                {
                    _AppMethod = new AppMethod();
                }

                return _AppMethod;
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

        #region MapHelper

        private MapHelper _MapHelper;
        public MapHelper MapHelper
        {
            get
            {
                if (_MapHelper == null)
                {
                    _MapHelper = new MapHelper();
                }

                return _MapHelper;
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

                    if (ActiveDocumentChanged != null)
                    {
                        ActiveDocumentChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler ActiveDocumentChanged;
        private void OnActiveDocumentChanged(object sender, EventArgs e)
        {
            Console.WriteLine(this.ActiveDocument.ToString());
            MapViewModel.IsMapViewModel = this.ActiveDocument is MapViewModel;
        }

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




        #region TestCommands

        private TestCommands _TestCommands;
        public TestCommands TestCommands
        {
            get
            {
                if (_TestCommands == null)
                {
                    _TestCommands = new TestCommands();
                }
                return _TestCommands;
            }
        }

        #endregion

    }
}
