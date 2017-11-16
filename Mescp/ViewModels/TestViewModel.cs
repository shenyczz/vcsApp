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
    [Obsolete("没有使用",true)]
    public class TestViewModel : ViewModelBase
    {
        public TestViewModel()
        {
        }

        #region Test

        private Boolean _Test;
        public Boolean Test
        {
            get
            {
                return _Test;
            }
            set
            {
                _Test = value;
                RaisePropertyChanged("Test");
            }
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
            //
        }
        private Boolean CanTest(Object parameter)
        {
            return true;
        }

        #endregion



    }
}
