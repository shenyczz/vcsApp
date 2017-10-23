/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: APPCommands
 *  Version: 
 * Function: 
 * 
 *   Author: 申永辰.郑州 (shenyczz@163.com)
 * DateTime: 2010 - 2014
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hdwas.Commands
{
    public class APPCommands
    {

        #region HdwDayCommand - 干热风监测命令

        private RelayCommand _HdwDayCommand;
        public ICommand HdwDayCommand
        {
            get
            {
                if (_HdwDayCommand == null)
                {
                    _HdwDayCommand = new RelayCommand(p => OnHdwDay(p), p => CanHdwDay(p));
                }

                return _HdwDayCommand;
            }
        }

        private void OnHdwDay(Object parameter)
        {
            try
            {
                ToolViewModel vm = Workspace.Instance.Tools.Find(p => p is HdwDayViewModel);
                vm.IsSelected = true;
            }
            catch (Exception)
            {
            }
        }

        private Boolean CanHdwDay(Object parameter)
        {
            return Workspace.Instance.HdwDayViewModel.IsVisible;
        }

        #endregion

        #region HdwProcessCommand - 干热风过程

        private RelayCommand _HdwProcessCommand;
        public ICommand HdwProcessCommand
        {
            get
            {
                if (_HdwProcessCommand == null)
                {
                    _HdwProcessCommand = new RelayCommand(p => OnHdwProcess(p), p => CanHdwProcess(p));
                }

                return _HdwProcessCommand;
            }
        }

        private void OnHdwProcess(Object parameter)
        {
            try
            {
                ToolViewModel vm = Workspace.Instance.Tools.Find(p => p is HdwProcessViewModel);
                vm.IsSelected = true;
            }
            catch (Exception)
            {
            }
        }

        private Boolean CanHdwProcess(Object parameter)
        {
            return Workspace.Instance.HdwProcessViewModel.IsVisible;
        }

        #endregion


    }
}
