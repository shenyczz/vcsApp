/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: ToolViewModel
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

namespace Hdwas
{
    public class ToolViewModel : PaneViewModel
    {
        protected ToolViewModel()
        {
            this.IsVisible = false;
        }

        public String Name { get; set; }


        #region IsVisible

        private Boolean _IsVisible = true;
        public Boolean IsVisible
        {
            get { return _IsVisible; }
            set
            {
                if (_IsVisible != value)
                {
                    _IsVisible = value;
                    RaisePropertyChanged("IsVisible");
                }
            }
        }

        #endregion


    }
}
