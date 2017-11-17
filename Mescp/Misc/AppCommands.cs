/******************************************************************************
 * 
 * Copyright (C) 2010 - 2016 shenyc(shenyczz@163.com).
 * All rights reserved.
 *
 * RgnName: AppCommands
 *     
 * Usage:
 *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using CSharpKit.Windows.Input;
using Mescp.Models;

namespace Mescp
{
    public class AppCommands
    {
        #region AboutCommand

        private RelayCommand _AboutCommand;
        public ICommand AboutCommand
        {
            get
            {
                if (_AboutCommand == null)
                {
                    _AboutCommand = new RelayCommand(p => OnAboutCommand(p));
                }

                return _AboutCommand;
            }
        }

        private void OnAboutCommand(Object parameter)
        {
            AboutBox aboutBox = new AboutBox(App.Current.MainWindow);
            aboutBox.ShowDialog();
        }

        #endregion


        #region RegionChangedCommand

        private RelayCommand _RegionChangedCommand;
        public ICommand RegionChangedCommand
        {
            get
            {
                if (_RegionChangedCommand == null)
                {
                    _RegionChangedCommand = new RelayCommand(p => OnRegionChanged(p), p => CanRegionChanged(p));
                }

                return _RegionChangedCommand;
            }
        }

        void OnRegionChanged(Object parameter)
        {
            RibbonGallery rg = parameter as RibbonGallery;
            Models.Region rgn = rg.SelectedItem as Models.Region;
            App.Workspace.AppData.CurrentRegion = rgn;
            App.Workspace.MapViewModel.RegionCode = rgn.RgnCode;    // 改变当前地图
        }

        private Boolean CanRegionChanged(Object parameter)
        {
            return App.Workspace.MapViewModel.IsMapViewModel;
        }

        #endregion

        #region CultivarChangedCommand

        private RelayCommand _CultivarChangedCommand;
        public ICommand CultivarChangedCommand
        {
            get
            {
                if (_CultivarChangedCommand == null)
                {
                    _CultivarChangedCommand = new RelayCommand(p => OnCultivarChanged(p), p => CanCultivarChanged(p));
                }

                return _CultivarChangedCommand;
            }
        }

        void OnCultivarChanged(Object parameter)
        {
            RibbonGallery rg = parameter as RibbonGallery;
            CropCultivar cropCultivar = rg.SelectedItem as CropCultivar;
            App.Workspace.AppData.CurrentCropCultivar = cropCultivar;
        }

        private Boolean CanCultivarChanged(Object parameter)
        {
            return true;
        }

        #endregion

        #region YearChangedCommand

        private RelayCommand _YearChangedCommand;
        public ICommand YearChangedCommand
        {
            get
            {
                if (_YearChangedCommand == null)
                {
                    _YearChangedCommand = new RelayCommand(p => OnYearChanged(p), p => CanYearChanged(p));
                }

                return _YearChangedCommand;
            }
        }

        private void OnYearChanged(Object parameter)
        {
            RibbonGallery rg = parameter as RibbonGallery;
            Models.YClass yclass = rg.SelectedItem as YClass;
            App.Workspace.AppData.Year = int.Parse(yclass.Year);

            Console.WriteLine(App.Workspace.AppData.Year);
        }

        private Boolean CanYearChanged(Object parameter)
        {
            return true;
        }

        #endregion

        #region EvaluateCommand

        private RelayCommand _EvaluateCommand;
        public ICommand EvaluateCommand
        {
            get
            {
                if (_EvaluateCommand == null)
                {
                    _EvaluateCommand = new RelayCommand(p => OnEvaluate(p), p => CanEvaluate(p));
                }

                return _EvaluateCommand;
            }
        }

        private void OnEvaluate(Object parameter)
        {
            App.Workspace.EvaluReportViewModel.Evaluate();
        }
        private Boolean CanEvaluate(Object parameter)
        {
            return true;
        }

        #endregion

        #region TestCommand

        private RelayCommand _TestCommand;
        public ICommand TestCommand
        {
            get
            {
                if (_TestCommand == null)
                {
                    _TestCommand = new RelayCommand(p => OnTest(p), p => CanTest(p));
                }

                return _TestCommand;
            }
        }

        private void OnTest(Object parameter)
        {
            App.Workspace.AppHelper.Test();
        }
        private Boolean CanTest(Object parameter)
        {
            return true;
        }

        #endregion


    }
}
