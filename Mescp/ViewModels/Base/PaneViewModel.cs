/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     RgnName: PaneViewModel
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Mescp.ViewModels
{
    public abstract class PaneViewModel : ViewModelBase
    {

        #region Title

        private String _Title;
        public String Title
        {
            get { return _Title; }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        #endregion

        #region IconSource

        private ImageSource _IconSource;
        public ImageSource IconSource
        {
            get { return _IconSource; }
            set { _IconSource = value; }
        }

        #endregion

        #region ContentId

        private String _ContentId;
        public String ContentId
        {
            get { return _ContentId; }
            set
            {
                if (_ContentId != value)
                {
                    _ContentId = value;
                    RaisePropertyChanged("ContentId");
                }
            }
        }

        #endregion

        #region IsActive

        private Boolean _IsActive;
        public Boolean IsActive
        {
            get { return _IsActive; }
            set
            {
                if (_IsActive != value)
                {
                    _IsActive = value;
                    RaisePropertyChanged("IsActive");
                }
            }
        }

        #endregion

        #region IsSelected

        private Boolean _IsSelected;
        public Boolean IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    RaisePropertyChanged("IsSelected");
                }
            }
        }

        #endregion

        #region ToolTip

        private String _ToolTip;
        public String ToolTip
        {
            get { return _ToolTip; }
            set
            {
                if (_ToolTip != value)
                {
                    _ToolTip = value;
                    RaisePropertyChanged("ToolTip");
                }
            }
        }

        #endregion

    }
}
