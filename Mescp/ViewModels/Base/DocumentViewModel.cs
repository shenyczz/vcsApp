/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     RgnName: DocumentViewModel
 *  Version: 
 * Function: 
 * 
 *   Author: 申永辰.郑州 (shenyczz@163.com)
 * DateTime: 2010 - 
 *  WebSite: 
 *
******************************************************************************/
using CSharpKit.Data;
using CSharpKit.Data.Esri;
using CSharpKit.Vision;
using CSharpKit.Vision.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mescp.ViewModels
{
    public abstract class DocumentViewModel : PaneViewModel
    {
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

    }
}
