/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: PaneViewModel
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Amrs.ViewModels
{
    public class PaneViewModel : ViewModelBase
    {
        protected PaneViewModel() { }

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

        #region ContentId

        public String ContentId { get; set; }

        #endregion

        #region IconSource

        private ImageSource _IconSource;
        public ImageSource IconSource
        {
            get { return _IconSource; }
            set { _IconSource = value; }
        }

        #endregion
    }
}
