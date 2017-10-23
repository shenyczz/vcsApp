/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: AmrsFileProvider
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
using CSharpKit.Data;

namespace Amrs.Core
{
    public abstract class AmrsFileProvider : MeteoFileProvider
    {
        public AmrsFileProvider(String connectionString)
            : base(connectionString) { }

        //public override void Open()
        //{
        //    throw new NotImplementedException();
        //}

        //public override void Close()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
