/******************************************************************************
 * 
 * Copyright (C) 2010 - 2016 shenyc(shenyczz@163.com).
 * All rights reserved.
 *
 * RgnName: TestViewModel
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
using System.Windows.Input;
using CSharpKit.Windows.Input;
using Mescp.Models;

namespace Mescp.ViewModels
{
    public class TestViewModel : ViewModelBase
    {
        public TestViewModel()
        {
        }

        #region TestCommand

        private RelayCommand _TestCommand;
        public ICommand TestCommand
        {
            get
            {
                if (_TestCommand == null)
                {
                    _TestCommand = new RelayCommand(p => OnTestCommand(p));
                }

                return _TestCommand;
            }
        }

        private void OnTestCommand(Object parameter)
        {
            System.Windows.Controls.Ribbon.RibbonGallery rg = parameter as System.Windows.Controls.Ribbon.RibbonGallery;
            Region xs = rg.SelectedItem as Region;
            //App.Workspace.MapViewModel.RegionCode = xs.Code;
        }

        #endregion


    }
}
