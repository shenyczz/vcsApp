/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: AmrsFile - 农业气象遥感文件
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
    public class ModisFile : AmrsFile
    {
        public ModisFile(String fileName)
            : base(fileName) { }


        protected override void Initialize()
        {
            try
            {
                this.DataInfo = new ModisFileDataInfo();                        // 数据信息
                this.RsDataChannelMap = new Dictionary<int, RsDataChannel>();
                this.DataProcessor = new ModisFileProcessor(this);              // 数据处理器
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Dispose()
        {
            //throw new NotImplementedException();
        }

    }
}
