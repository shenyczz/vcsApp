/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: ModisFileProvider
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
using Amrs.Core;

namespace Amrs.Modis
{
    public class ModisFileProvider : AmrsFileProvider
    {
        public ModisFileProvider(String connectionString)
            : base(connectionString) { }

        public override void Open()
        {
            try
            {
                this.Close();
                this.DataInstance = new ModisFile(this.ConnectionString);
                this.IsOpen = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void Close()
        {
            if (!IsOpen)
                return;

            ModisFile modisFile = this.DataInstance as ModisFile;
            modisFile.Dispose();

            this.IsOpen = false;
        }
    }
}
