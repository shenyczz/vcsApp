/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: Workspace
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Amrs.ViewModels;

namespace Amrs.ViewModels
{
    public class Workspace : ViewModelBase
    {
        public Workspace()
        {
            this.ActiveDocumentChanged += OnActiveDocumentChanged;
        }

        public void Initialize()
        {
            Documents = new ObservableCollection<DocViewModel>();
            {
                Documents.Add(this.MapViewModel);
            }

            Tools = new List<ToolViewModel>();
            {
                Tools.Add(this.LayerViewModel);
                Tools.Add(this.PropertyGridViewModel);
            }
        }

        #region Documents

        public ObservableCollection<DocViewModel> Documents { get; private set; }

        #endregion

        #region Tools

        public List<ToolViewModel> Tools { get; private set; }

        #endregion


        #region ActiveDocument

        private DocViewModel _ActiveDocument;
        public DocViewModel ActiveDocument
        {
            get { return _ActiveDocument; }
            set
            {
                if (_ActiveDocument != value)
                {
                    _ActiveDocument = value;
                    RaisePropertyChanged("ActiveDocument");

                    if (ActiveDocumentChanged != null)
                        ActiveDocumentChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler ActiveDocumentChanged;
        private void OnActiveDocumentChanged(object sender, EventArgs e)
        {
            this.MapViewModel.IsMapViewModel = this.ActiveDocument is MapViewModel;
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
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("pack://application:,,,/Images/globe.png");
                    bi.EndInit();
                    _MapViewModel = new MapViewModel()
                    {
                        Title = "地理信息",
                        //ToolTip = "查看信息",
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
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("pack://application:,,,/Images/document.png");
                    bi.EndInit();

                    _LayerViewModel = new LayerViewModel();
                    _LayerViewModel.Title = "图层控制";
                    _LayerViewModel.ContentId = "图层控制"; // 要想保存布局必须使用 ContentId 属性
                    _LayerViewModel.IconSource = bi;
                }

                return _LayerViewModel;
            }
        }

        #endregion

        #region PropertyGridViewModel

        private PropertyGridViewModel _PropertyGridViewModel;
        public PropertyGridViewModel PropertyGridViewModel
        {
            get
            {
                if (_PropertyGridViewModel == null)
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("pack://application:,,,/Images/Property.png");
                    bi.EndInit();

                    _PropertyGridViewModel = new PropertyGridViewModel();
                    _PropertyGridViewModel.Title = "图层属性";
                    _PropertyGridViewModel.ContentId = "图层属性";
                    _PropertyGridViewModel.IconSource = bi;
                }

                return _PropertyGridViewModel;
            }
        }

        #endregion

    }
}
