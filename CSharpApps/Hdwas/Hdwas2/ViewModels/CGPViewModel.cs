/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: CGPViewModel
 *  Version: 
 * Function: 
 * 
 *   Author: 申永辰.郑州 (shenyczz@163.com)
 * DateTime: 2010 - 2014
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hdwas
{
    public class CGPViewModel : DocumentViewModel
    {

        public CropGrowthPeriodCollection _CropGrowthPeriods;
        public CropGrowthPeriodCollection CropGrowthPeriods
        {
            get { return _CropGrowthPeriods; }
            set
            {
                if (_CropGrowthPeriods != value)
                {
                    _CropGrowthPeriods = value;
                    RaisePropertyChanged("CropGrowthPeriods");
                }
            }
        }


        #region OpenCgpCommand

        private RelayCommand _OpenCgpCommand;
        public ICommand OpenCgpCommand
        {
            get
            {
                if (_OpenCgpCommand == null)
                {
                    _OpenCgpCommand = new RelayCommand(p => OnOpenCgp(p), p => CanOpenCgp(p));
                }

                return _OpenCgpCommand;
            }
        }

        private void OnOpenCgp(Object parameter)
        {
            Workspace workspace = Workspace.Instance;
            if (!Workspace.Instance.Documents.Contains(Workspace.Instance.CGPViewModel))
                Workspace.Instance.Documents.Add(Workspace.Instance.CGPViewModel);

            Workspace.Instance.ActiveDocument = Workspace.Instance.CGPViewModel;


            if (TheApp.CropGrowthPeriods == null)
            {
                TheApp.CropGrowthPeriods = new CropGrowthPeriodCollection();
            }

            if (TheApp.CropGrowthPeriods.Count == 0)
            {
                string fn = System.IO.Path.Combine(TheApp.ConfigPath, "CropGrowthPeriod.xml");
                TheApp.CropGrowthPeriods.FromXElement(XElement.Load(fn));
            }

            this.CropGrowthPeriods = null;
            this.CropGrowthPeriods = TheApp.CropGrowthPeriods;
        }
        private Boolean CanOpenCgp(Object parameter)
        {
            return true;
        }

        void test1()
        {
            string fn = System.IO.Path.Combine(TheApp.ConfigPath, "CropGrowthPeriod.xml");
            this.CropGrowthPeriods = new CropGrowthPeriodCollection();
            this.CropGrowthPeriods.FromXElement(XElement.Load(fn));
        }
        void test2()
        {
            string fn = System.IO.Path.Combine(TheApp.ConfigPath, "未命名.xml");
            this.CropGrowthPeriods = new CropGrowthPeriodCollection();
            this.CropGrowthPeriods.FromXElement(XElement.Load(fn));
        }

        #endregion

        #region LoadCgpCommand

        private RelayCommand _LoadCgpCommand;
        public ICommand LoadCgpCommand
        {
            get
            {
                if (_LoadCgpCommand == null)
                {
                    _LoadCgpCommand = new RelayCommand(p => OnLoadCgp(p), p => CanLoadCgp(p));
                }

                return _LoadCgpCommand;
            }
        }

        private void OnLoadCgp(Object parameter)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.InitialDirectory = TheApp.ConfigPath;
            ofd.Filter = "配置文件|*.xml";

            if ((bool)ofd.ShowDialog())
            {
                if (TheApp.CropGrowthPeriods == null)
                    TheApp.CropGrowthPeriods = new CropGrowthPeriodCollection();

                string fn = ofd.FileName;
                TheApp.CropGrowthPeriods.FromXElement(XElement.Load(fn));
                this.CropGrowthPeriods = null;
                this.CropGrowthPeriods = TheApp.CropGrowthPeriods;
            }
        }
        private Boolean CanLoadCgp(Object parameter)
        {
            return true;
        }

        #endregion

        #region SaveAsCgpCommand

        private RelayCommand _SaveAsCgpCommand;
        public ICommand SaveAsCgpCommand
        {
            get
            {
                if (_SaveAsCgpCommand == null)
                {
                    _SaveAsCgpCommand = new RelayCommand(p => OnSaveAsCgp(p));
                }

                return _SaveAsCgpCommand;
            }
        }
        private void OnSaveAsCgp(Object parameter)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.InitialDirectory = TheApp.ConfigPath;
            sfd.FileName = "未命名";
            sfd.Filter = "配置文件|*.xml";
            sfd.AddExtension = true;

            if ((bool)sfd.ShowDialog())
            {
                //SaveAs(sfd.FileName);
                TheApp.CropGrowthPeriods.ToXElement().Save(sfd.FileName);
            }
        }

        #endregion

    }
}
