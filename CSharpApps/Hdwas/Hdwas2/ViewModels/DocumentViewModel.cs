/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: DocumentViewModel
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hdwas
{
    public class DocumentViewModel : PaneViewModel
    {
        protected DocumentViewModel()
        {
            IsDirty = true;
        }

        private static ImageSourceConverter ISC = new ImageSourceConverter();


        #region FilePath

        private String _FilePath;
        public String FilePath
        {
            get { return _FilePath; }
            set
            {
                _FilePath = value;
                RaisePropertyChanged("FilePath");

                OnFilePathChanged(_FilePath);
            }
        }

        protected virtual void OnFilePathChanged(String filePath) { }

        #endregion

        #region IsDirty

        private Boolean _IsDirty;
        public Boolean IsDirty
        {
            get { return _IsDirty; }
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    RaisePropertyChanged("IsDirty");
                }
            }
        }

        #endregion



        #region CloseCommand

        private RelayCommand _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                {
                    _CloseCommand = new RelayCommand((p) => OnClose(), (p) => CanClose());
                }

                return _CloseCommand;
            }
        }

        protected virtual void OnClose()
        {
            Workspace.Instance.Close(this);
        }

        private bool CanClose()
        {
            return true;
        }

        #endregion
    }
}
