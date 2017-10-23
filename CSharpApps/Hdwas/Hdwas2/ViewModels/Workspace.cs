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
 * DateTime: 2010 - 
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Hdwas.Commands;
using CSharpKit.Vision.Mapping;

namespace Hdwas
{
    public class Workspace : ViewModelBase
    {
        protected Workspace()
        {
            Documents = new ObservableCollection<DocumentViewModel>();
            Documents.Add(this.MapViewModel);
            
            Tools = new List<ToolViewModel>();
            Tools.Add(this.HdwDayViewModel);
            Tools.Add(this.HdwProcessViewModel);

            this.APPCommands = new APPCommands();
            this.MapFunctions = new MapFunctions();

            this.ActiveDocumentChanged += OnActiveDocumentChanged;
        }

        #region Instance

        private static Workspace _Instance = new Workspace();
        public static Workspace Instance
        {
            get { return _Instance; }
        }

        #endregion


        #region Documents

        public ObservableCollection<DocumentViewModel> Documents { get; private set; }

        #endregion

        #region Tools

        public List<ToolViewModel> Tools { get; private set; }

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
                        ActiveDocumentChanged(this, EventArgs.Empty);
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


        #region APPCommands

        public APPCommands APPCommands { get; private set; }

        #endregion

        #region MapFunctions

        public MapFunctions MapFunctions { get; private set; }

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
                    //bi.UriSource = new Uri("pack://application:,,,/Images/property-blue.png");
                    bi.UriSource = new Uri("pack://application:,,,/Images/Globe.png");
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

        #region CGPViewModel
        
        private CGPViewModel _CGPViewModel;
        public CGPViewModel CGPViewModel
        {
            get
            {
                if (_CGPViewModel == null)
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("pack://application:,,,/Images/Notepad-16x16.png");
                    bi.EndInit();
                    _CGPViewModel = new CGPViewModel()
                    {
                        Title = "发育期",
                        ToolTip = "发育期管理",
                        IconSource = bi,
                    };
                }

                return _CGPViewModel;
            }
        }
        
        #endregion

        #region HdwDayViewModel

        private HdwDayViewModel _HdwDayViewModel;
        public HdwDayViewModel HdwDayViewModel
        {
            get
            {
                if (_HdwDayViewModel == null)
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("pack://application:,,,/Images/property-blue.png");
                    bi.EndInit();
                    _HdwDayViewModel = new HdwDayViewModel()
                    {
                        IsVisible = true,
                        Title = "干热风监测",
                        ToolTip = "干热风监测",
                        IconSource = bi,
                    };
                }

                return _HdwDayViewModel;
            }
        }

        #endregion

        #region HdwDayDataGridViewModel

        HdwDayDataGridViewModel _HdwDayDataGridViewModel;
        public HdwDayDataGridViewModel HdwDayDataGridViewModel
        {
            get
            {
                if (_HdwDayDataGridViewModel == null)
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("pack://application:,,,/Images/property-blue.png");
                    bi.EndInit();
                    _HdwDayDataGridViewModel = new HdwDayDataGridViewModel()
                    {
                        Title = "干热风日",
                        ToolTip = "干热风日",
                        IconSource = bi,
                    };
                }

                return _HdwDayDataGridViewModel;
            }
        }

        #endregion

        #region HdwProcessViewModel

        HdwProcessViewModel _HdwProcessViewModel;
        public HdwProcessViewModel HdwProcessViewModel
        {
            get
            {
                if (_HdwProcessViewModel == null)
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("pack://application:,,,/Images/property-blue.png");
                    bi.EndInit();
                    _HdwProcessViewModel = new HdwProcessViewModel()
                    {
                        IsVisible = true,
                        Title = "干热风过程",
                        ToolTip = "干热风过程",
                        IconSource = bi,
                    };
                }

                return _HdwProcessViewModel;
            }
        }

        #endregion

        #region HdwProcessDataGridViewModel

        HdwProcessDataGridViewModel _HdwProcessDataGridViewModel;
        public HdwProcessDataGridViewModel HdwProcessDataGridViewModel
        {
            get
            {
                if (_HdwProcessDataGridViewModel == null)
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("pack://application:,,,/Images/property-blue.png");
                    bi.EndInit();
                    _HdwProcessDataGridViewModel = new HdwProcessDataGridViewModel()
                    {
                        Title = "干热风过程",
                        ToolTip = "干热风过程",
                        IconSource = bi,
                    };
                }

                return _HdwProcessDataGridViewModel;
            }
        }

        #endregion



        #region Misc

        /*
        public DocumentViewModel Open(String filePathName)
        {
            return Open(filePathName, "");
        }

        public DocumentViewModel Open(String filePathName, Object parameter)
        {
            var fileViewModel = this.Documents.FirstOrDefault(fm => fm.FilePath == filePathName);

            try
            {

            }
            catch
            {
            }

            return fileViewModel;
        }

        public void Save(DocumentViewModel viewModel)
        {
            //if (fileToSave.FilePath == null || saveAsFlag)
            //{
            //    var dlg = new SaveFileDialog();
            //    if (dlg.ShowDialog().GetValueOrDefault())
            //        fileToSave.FilePath = dlg.SafeFileName;
            //}

            //File.WriteAllText(fileToSave.FilePath, fileToSave.TextContent);
            //ActiveDocument.IsDirty = false;
        }

        public void SaveAs(DocumentViewModel viewModel)
        {
            //if (fileToSave.FilePath == null || saveAsFlag)
            //{
            //    var dlg = new SaveFileDialog();
            //    if (dlg.ShowDialog().GetValueOrDefault())
            //        fileToSave.FilePath = dlg.SafeFileName;
            //}

            //File.WriteAllText(fileToSave.FilePath, fileToSave.TextContent);
            //ActiveDocument.IsDirty = false;
        }
        */

        public void Close(DocumentViewModel viewModel)
        {
            this.Documents.Remove(viewModel);
        }


        #endregion

    }
}
