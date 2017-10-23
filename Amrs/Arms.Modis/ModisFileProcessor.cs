/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: AmrsFileProcessor
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Amrs.Core;
using CSharpKit;

namespace Amrs.Modis
{
    public class ModisFileProcessor : AmrsFileProcessor
    {
        public ModisFileProcessor(Object owner)
            : base(owner) { }

        int _flag = 0;

        protected override bool Load_bin(string fileName)
        {
            this.IsLoad = false;

            try
            {

                bool bLd2File = AmrsFile.IsLd2File(fileName);
                bool bLd3File = AmrsFile.IsLd3File(fileName);

                if (bLd2File)
                {
                    this.IsLoad = Load_bin_ld2(fileName);
                }
                else if (bLd3File)
                {
                    this.IsLoad = Load_bin_ld3(fileName);
                }
            }
            catch (Exception)
            {
                this.IsLoad = false;
            }

            return this.IsLoad;

            /*
		// 保存文件名
		this->SetFileName(lpszFile);
		// 映射通道(通道号和数据索引对应)
		this->MapChannel();
		// 设置默认的显示通道
		this->SetDefaultDisplayChannel();
		// 非空标记置位
		m_bEmpty = FALSE;
             */
        }

        private bool Load_bin_ld2(string fileName)
        {
            bool bLoad = true;

            try
            {
                ModisFile modisFile = this.Owner as ModisFile;
                ModisFileDataInfo dataInfo = modisFile.DataInfo as ModisFileDataInfo;
                LD2Header fileHeader = null;

                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        // 文件头
                        int nHeadSize = Marshal.SizeOf(typeof(LD2Header));
                        byte[] bs = br.ReadBytes(nHeadSize);
                        fileHeader = (LD2Header)AmrsHelper.BytesToStruct(bs, typeof(LD2Header));
                        if (fileHeader.FileID != AmrsFile.ModisMarker)
                            return false;


                        // 跳过头后填充的字节数,准备读取数据
                        long lSkipLength = fileHeader.SkipLength;
                        fs.Seek(lSkipLength, SeekOrigin.Current);

                        // 通道数量
                        // 通道数据宽度和高度
                        int channelNums = fileHeader.ChannelNums;
                        int width = fileHeader.Width;
                        int height = fileHeader.Height;

                        dataInfo.RsData = new ushort[channelNums, height, width];
                        bs = br.ReadBytes(channelNums * height * width * 2);
                        for (int k = 0; k < channelNums; k++)
                        {
                            for (int i = 0; i < height; i++)
                            {
                                for (int j = 0; j < width; j++)
                                {
                                    int jj = j * 2;
                                    int pos = (k * height * width + i * width + j) * 2;
                                    int v = bs[pos] + bs[pos + 1] * 255;
                                    dataInfo.RsData[k, i, j] = (ushort)v;
                                }
                            }
                        }

                        //int xx = 0;

                    }
                }

                // 分配数据灰度内存
                this.RsDataGray = new Byte[fileHeader.ChannelNums, AmrsFile.ModisDataValueMax + 1];

                _flag = 2;
                this.FillDataInfo(fileHeader);

                this.LookupExtremum();
                this.ComputeBoundingBox();

                this.MapChannel();
                this.SetDataChannelDef();

                int rc, gc, bc;
                modisFile.GetDataChannel(out rc, out gc, out bc);
                this.RsImage = Data2Dib(rc, gc, bc);

                //======
                //RsImage.Save("e:\\temp\\2.bmp");

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                bLoad = false;
            }

            return bLoad;
        }
        private bool Load_bin_ld3(string fileName)
        {
            bool bLoad = true;

            try
            {
                string fn = @"D:\shenyc\vs2013\vcs\Amrs\Amrs\bin\Code\_TestData\terra_2011_02_20_03_35_sy_1km_河南1000.ld2";
                fileName = fn;

                FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                // 文件头
                int nHeadSize = Marshal.SizeOf(typeof(LD3Header));
                byte[] bs = br.ReadBytes(nHeadSize);
                LD3Header ld3h = (LD3Header)AmrsHelper.BytesToStruct(bs, typeof(LD3Header));

                br.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                bLoad = false;
            }
            return bLoad;
        }

        protected override void FillDataInfo(object fileHeader)
        {
            ModisFile modisFile = this.Owner as ModisFile;
            ModisFileDataInfo dataInfo = modisFile.DataInfo as ModisFileDataInfo;

            dataInfo.FileHeader = fileHeader;

            LDHeader head = fileHeader as LDHeader;


            dataInfo.FormatCode = _flag;
            dataInfo.FileId = Encoding.ASCII.GetString(BitConverter.GetBytes(head.FileID));
            ////pDataInfo->DataSource()		= pFileHeader->wSatelliteID;
            //dataInfo.OrbitNo = head.OrbitNo;
            //dataInfo.UpOrDown = head.UpOrDown;

            dataInfo.ChannelCount = head.ChannelNums;

            Grid grid = new Grid();
            grid.Width = head.Width;
            grid.MinX = head.LonMin;
            grid.XInterval = head.LonResolution;
            grid.MaxX = head.LonMax;

            grid.Height = head.Height;
            grid.MinY = head.LatMin;
            grid.YInterval = head.LatResolution;
            grid.MaxY = head.LatMax;
            dataInfo.Grid = grid;

            if (dataInfo.FormatCode == 2)
            {
                for (int i = 0; i < AmrsFile.ModisChannelCountMax; i++)
                {
                    dataInfo.ChannelCode[i] = (dataInfo.FileHeader as LD2Header).ChannelIndex[i];
                }
                dataInfo.Version = (dataInfo.FileHeader as LD2Header).Version;
            }
            else if (dataInfo.FormatCode == 3)
            {
                for (int i = 0; i < AmrsFile.ModisChannelCountMax; i++)
                {
                    dataInfo.ChannelCode[i] = (short)(dataInfo.FileHeader as LD3Header).ChannelIndex[i];
                }
                dataInfo.Version = (dataInfo.FileHeader as LD3Header).Version;
            }
        }

        protected override void LookupExtremum()
        {
            //base.LookupExtremum();
        }

        protected override void ComputeBoundingBox()
        {
            ModisFile modisFile = this.Owner as ModisFile;
            ModisFileDataInfo dataInfo = modisFile.DataInfo as ModisFileDataInfo;
            LDHeader head = dataInfo.FileHeader as LDHeader;
            dataInfo.Extent = new Extent(head.LonMin, head.LatMin, head.LonMax, head.LatMax);
        }

        /// <summary>
        /// 通道映射
        /// 通道索引为 0~37
        /// 通道代码为 1~38
        /// 实际通道为 1~36（13、14通道分LO和HI）
        /// 映射通道 [ index->ch38->name ]
        ///          [ 内存数据通道(Base0) -> 数据通道(Base1) + 通道名称
        /// </summary>
        private void MapChannel()
        {
            int ch38;
            ModisFile modisFile = this.Owner as ModisFile;
            ModisFileDataInfo dataInfo = modisFile.DataInfo as ModisFileDataInfo;

            var chMap = modisFile.RsDataChannelMap;
            chMap.Clear();

            for (int i = 0; i < AmrsFile.ModisChannelCountMax; i++)
            {
                // 通道号
                ch38 = dataInfo.ChannelCode[i];
                if (ch38 <= 0)  // 没有通道数据
                    continue;

                RsDataChannel rsch = new RsDataChannel();
                rsch.Index = i;
                rsch.Name = ChannelCode2ChannelName(ch38);
                chMap.Add(i, rsch);
            }

            return;
        }

        /// <summary>
        /// 通道代码转换为通道名称
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        private String ChannelCode2ChannelName(int channel)
        {
            string channelName = "";
            ModisFile modisFile = this.Owner as ModisFile;
            ModisFileDataInfo dataInfo = modisFile.DataInfo as ModisFileDataInfo;

            int iChannelNums = (dataInfo.FileHeader as LDHeader).ChannelNums;

            // MODIS 数据有36通道，
            // 但13通道和14通道分高位和低位
            // 故此有38个通道数据
            int ch38 = channel;
            if (ch38 <= 12)
            {
                channelName = string.Format("{0:D2} 通道", ch38);
            }
            else if (ch38 >= 17)
            {
                channelName = string.Format("{0:D2} 通道", ch38 - 2);
            }
            else
            {
                if (ch38 == 13)
                    channelName = string.Format("13lo 通道");
                if (ch38 == 14)
                    channelName = string.Format("13hi 通道");

                if (ch38 == 15)
                    channelName = string.Format("14lo 通道");
                if (ch38 == 16)
                    channelName = string.Format("14hi 通道");
            }

            return channelName;
        }

        /// <summary>
        /// 设置默认的显示通道
        /// </summary>
        private void SetDataChannelDef()
        {
            ModisFile modisFile = this.Owner as ModisFile;
            ModisFileDataInfo dataInfo = modisFile.DataInfo as ModisFileDataInfo;

            LDHeader head = (dataInfo.FileHeader as LDHeader);
            int iChannelNums = (dataInfo.FileHeader as LDHeader).ChannelNums;

            //
            int addHour = 8;
            int iHour = head.Hour + addHour;
            if (iHour >= 24)
                iHour -= 24;

            if (iHour >= 6 && iHour <= 18)
                head.DayOrNight = 0;    // 白天黑夜标识, 0: 白天
            else
                head.DayOrNight = 1;    // 白天黑夜标识, 1: 黑夜

            if(head.IsDay)
            {// 白天
                if (iChannelNums == 1)
                {
                    int ch = dataInfo.ChannelCode[0];
                    modisFile.SetDataChannelDef(ch, ch, ch);
                }
                else if (iChannelNums == 2)
                {
                    int ch1 = dataInfo.ChannelCode[0];
                    int ch2 = dataInfo.ChannelCode[1];
                    modisFile.SetDataChannelDef(ch1, ch2, ch1);
                }
                else
                {
                    modisFile.SetDataChannelDef(1, 2, 1);
                }
            }
            else
            {// 晚上
                if (iChannelNums == AmrsFile.ModisChannelCount)
                {
                    modisFile.SetDataChannelDef(32, 32, 31);
                }
                else
                {
                    int ch = dataInfo.ChannelCode[0];
                    modisFile.SetDataChannelDef(ch, ch, ch);
                }
            }

            return;
        }

        #region Data2Dib - 数据转换为 DIB 图像

        /// <summary>
        /// 数据转换为 DIB
        /// </summary>
        /// <param name="iRChannel"></param>
        /// <param name="iGChannel"></param>
        /// <param name="iBChannel"></param>
        /// <returns></returns>
        public Image Data2Dib(int iRChannel, int iGChannel, int iBChannel)
        {
            Bitmap bm = null;

            try
            {
                int i, j;

                ModisFile modisFile = this.Owner as ModisFile;
                ModisFileDataInfo dataInfo = modisFile.DataInfo as ModisFileDataInfo;

                // 数据转换为灰度
                this.Data2Gray();

                // 设置融合通道值
                int chr, chg, chb;
                modisFile.GetDataChannelDef(out chr, out chg, out chb);
                if (iRChannel == 0 && iGChannel == 0 && iBChannel == 0)
                {
                    modisFile.SetDataChannel(chr, chg, chb);
                }
                else
                {
                    modisFile.SetDataChannel(iRChannel, iGChannel, iBChannel);
                }

                // 选择数据通道
                modisFile.GetDataChannel(out chr, out chg, out chb);

                // 通道转换 把 1-36 => 1-38
                int chr38 = this.Channel2ChannelCode(chr);
                int chg38 = this.Channel2ChannelCode(chg);
                int chb38 = this.Channel2ChannelCode(chb);

                // 通道转换为数据索引(Modis特有)
                int rDataIndex = this.GetChannelIndex(chr38);
                int gDataIndex = this.GetChannelIndex(chg38);
                int bDataIndex = this.GetChannelIndex(chb38);

                // 通道缩引 = [0,37]
                if (rDataIndex < 0 || gDataIndex < 0 || bDataIndex < 0)
                    return null;

                //数据尺寸
                int iWidth = dataInfo.Grid.Width;
                int iHeight = dataInfo.Grid.Height;

                // 通道数据
                UInt16[, ,] rsData = dataInfo.RsData;
                // 通道数据灰度指针
                Byte[,] rsDataGray = this.RsDataGray;

                bm = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                BitmapData bmData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                {
                    IntPtr Scan0 = bmData.Scan0;
                    int stride = bmData.Stride;

                    unsafe
                    {
                        byte* p = (byte*)(void*)Scan0;
                        for (i = 0; i < iHeight; i++)
                        {
                            for (j = 0; j < iWidth; j++)
                            {
                                UInt16 wRData = rsData[rDataIndex, i, j];
                                UInt16 wGData = rsData[gDataIndex, i, j];
                                UInt16 wBData = rsData[bDataIndex, i, j];

                                byte byRGray = rsDataGray[rDataIndex, wRData];
                                byte byGGray = rsDataGray[gDataIndex, wGData];
                                byte byBGray = rsDataGray[bDataIndex, wBData];

                                //升降轨标记, 1: 升轨, 0: 降轨
                                //int ii = (iHeight - 1 - i);
                                int ii = i;

                                p[ii * stride + j * 3 + 0] = byBGray; // B
                                p[ii * stride + j * 3 + 1] = byGGray;  // G
                                p[ii * stride + j * 3 + 2] = byRGray;  // R

                                //p[ii * stride + j * 3 + 0] = (byte)(255 - byBGray); // B
                                //p[ii * stride + j * 3 + 1] = (byte)(255 - byGGray);  // G
                                //p[ii * stride + j * 3 + 2] = (byte)(255 - byRGray);  // R
                            }
                        }
                    }//unsafe
                }
                bm.UnlockBits(bmData);
            }
            catch (Exception)
            {
                bm = null;
            }


            return bm;
        }

        // 把 1-36 转换为 1-38
        // 1 2 .............. 12     13           14      15 ................ 36		// 习惯通道
        // 1 2 .............. 12  [13 + 14]   [15 + 16]   17 ................ 38		// 实际通道
        // 通道号转换为通道代码
        public int Channel2ChannelCode(int iChannel)
        {
            int ch36 = iChannel;
            int ch38 = -1;

            if (ch36 <= 12) ch38 = ch36;
            if (ch36 >= 15) ch38 = ch36 + 2;

            // 13 14 -> 13l 13h
            // 15 16 -> 14l 14h
            if (ch36 == 13) ch38 = 13;	// or 14
            if (ch36 == 14) ch38 = 15;	// or 16

            return ch38;
        }

        /// <summary>
        /// 取得通道代码对应的通道索引 (1-38 转换为 0-37)
        /// </summary>
        /// <param name="iChannelCode"></param>
        /// <returns></returns>
        public int GetChannelIndex(int iChannelCode)
        {
            // 默认数据通道
            int channelIndex = -1;

            ModisFile modisFile = this.Owner as ModisFile;
            ModisFileDataInfo dataInfo = modisFile.DataInfo as ModisFileDataInfo;
            for (int index = 0; index < 40; index++)
            {
                if (iChannelCode == dataInfo.ChannelCode[index])
                {
                    channelIndex = index;
                    break;
                }
            }

            return channelIndex;
        }

        /// <summary>
        /// 数据转换为灰度
        /// </summary>
        private void Data2Gray()
        {
            ModisFile modisFile = this.Owner as ModisFile;
            ModisFileDataInfo dataInfo = modisFile.DataInfo as ModisFileDataInfo;

            // 计算通道数据转换为灰度数据的参数
            CalcData2GrayParam();

            // 通道数
            int iChannelNums = dataInfo.ChannelCount;

            // 通道数据转换为灰度值
            for (int ch = 0; ch < iChannelNums; ch++)	// 0-37
            {
                ChannelData2Gray(this.Data2GrayParameter[ch]);
            }

            return;
        }

        /// <summary>
        /// 计算通道数据转换为灰度数据的参数
        /// 顺带对数据进行质量控制
        /// </summary>
        private void CalcData2GrayParam()
        {
            try
            {
                int i, ch, value;

                ModisFile modisFile = this.Owner as ModisFile;
                ModisFileDataInfo dataInfo = modisFile.DataInfo as ModisFileDataInfo;

                int iAvailPoint = 0;		// 有效点数

                UInt16 wMaxValue = 0;			// 最大值
                UInt16 wMinValue = 0xFFFF;	// 最小值

                UInt16 wUpThreshold = AmrsFile.ModisDataValueMax;	// 通道数据上阈值
                UInt16 wDnThreshold = 0;							// 通道数据下阈值

                int iChannelNums = dataInfo.ChannelCount;

                // 单个通道数据字节数
                int iDataSizePerChannel = dataInfo.Grid.Width * dataInfo.Grid.Height;
                int width = dataInfo.Grid.Width;
                int height = dataInfo.Grid.Height;

                // 通道值密度
                int[] iValueIntensity = new int[AmrsFile.ModisDataValueMax + 10];
                // 存储累计概率
                float[] fProbability = new float[AmrsFile.ModisDataValueMax + 10];

                // 初始化通道数据转换为灰度数据的参数
                this.Data2GrayParameter = new Data2GrayParameter[AmrsFile.ModisChannelCountMax];

                // 通道数据
                UInt16[, ,] rsData = dataInfo.RsData;

                // 计算各通道参数
                for (ch = 0; ch < iChannelNums; ch++)
                {
                    for (i = 0; i < AmrsFile.ModisDataValueMax + 10; i++)
                    {
                        iValueIntensity[i] = 0;
                        fProbability[i] = 0;
                    }

                    wMaxValue = 0;
                    wMinValue = 0xFFFF;
                    for (i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            rsData[ch, i, j] &= AmrsFile.ModisDataValueMax;
                            ushort wData = rsData[ch, i, j];
                            if (wData < AmrsFile.ModisDataValueMax)
                            {
                                // 记录最大值和最小值
                                wMaxValue = wData > wMaxValue ? wData : wMaxValue;
                                wMinValue = wData < wMinValue ? wData : wMinValue;
                                // 对应通道值计数器 +1
                                iValueIntensity[wData]++;	// 通道值密度
                            }
                        }
                    }//for(i)

                    // 有效数据点数量
                    iAvailPoint = iDataSizePerChannel - iValueIntensity[0];	// 减去值为0的点

                    //计算累计概率
                    fProbability[0] = 0.0f;
                    fProbability[1] = (float)iValueIntensity[1] / iAvailPoint;	// 次数除以总有效点数
                    for (value = 2; value <= AmrsFile.ModisDataValueMax; value++)
                    {
                        fProbability[value] = fProbability[value - 1] + (float)iValueIntensity[value] / iAvailPoint;
                    }

                    // 过滤掉数量少、非常小的值，得到通道数据下阈值
                    for (value = 0; value < AmrsFile.ModisDataValueMax; value++)
                    {
                        if (fProbability[value] > 0.012)
                        {
                            wDnThreshold = (ushort)value; // 通道数据下阈值
                            break;
                        }
                    }

                    // 过滤掉数量少、值很大的值，得到通道数据上阈值
                    for (value = AmrsFile.ModisDataValueMax; value >= 0; value--)
                    {
                        if (fProbability[value] < 0.92)
                        {
                            wUpThreshold = (ushort)value; // 通道数据上阈值
                            break;
                        }
                    }

                    // 通道数据转换为灰度数据的参数
                    this.Data2GrayParameter[ch].iChannel = ch;			// 通道号
                    this.Data2GrayParameter[ch].iEnhanceType = 2;			// 增强方式(0-直线，1-指数，2-对数 ...)
                    this.Data2GrayParameter[ch].wMinValue = wMinValue;	// 最小值
                    this.Data2GrayParameter[ch].wMaxValue = wMaxValue;	// 最大值
                    this.Data2GrayParameter[ch].wThreshold_min = wDnThreshold;	// 去掉一些小值后的极小值(通道数据下阈值)
                    this.Data2GrayParameter[ch].wThreshold_max = wUpThreshold;	// 去掉一些大值后的极大值(通道数据上阈值)
                    this.Data2GrayParameter[ch].cMinGray = 50;			// 最小灰度
                    this.Data2GrayParameter[ch].cMaxGray = 255;			// 最大灰度
                    this.Data2GrayParameter[ch].cLessDownGray = 50;			// 弱最小灰度
                    this.Data2GrayParameter[ch].cExceedUpGray = 255;			// 超最大灰度
                    this.Data2GrayParameter[ch].bAntiPhase = false;		// 是否反相

                    if (ch >= 19)
                    {// 反相
                        this.Data2GrayParameter[ch].bAntiPhase = true;
                    }

                    if (ch == 24)
                    {
                        this.Data2GrayParameter[ch].iEnhanceType = 1;		// 增强方式(0-直线，1-指数，2-对数 ...)
                        this.Data2GrayParameter[ch].bAntiPhase = false;
                    }

                    if (this.Data2GrayParameter[ch].bAntiPhase)
                    {//反相
                        this.Data2GrayParameter[ch].cLessDownGray = this.Data2GrayParameter[ch].cMaxGray;		// 弱最小灰度
                        this.Data2GrayParameter[ch].cExceedUpGray = this.Data2GrayParameter[ch].cMinGray;		// 超最大灰度
                    }
                    else
                    {
                        this.Data2GrayParameter[ch].cLessDownGray = this.Data2GrayParameter[ch].cMinGray;		// 弱最小灰度
                        this.Data2GrayParameter[ch].cExceedUpGray = this.Data2GrayParameter[ch].cMaxGray;		// 超最大灰度
                    }

                }//for(ch)

                iValueIntensity = null;
                fProbability = null;
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                throw ex;
            }
            return;
        }

        /// <summary>
        /// 通道数据转换为灰度
        /// </summary>
        /// <param name="data2GrayParameter"></param>
        private void ChannelData2Gray(Data2GrayParameter data2GrayParameter)
        {
            int value;

            ModisFile modisFile = this.Owner as ModisFile;
            ModisFileDataInfo dataInfo = modisFile.DataInfo as ModisFileDataInfo;

            int ch = data2GrayParameter.iChannel;						// 通道号
            int iEnhanceType = data2GrayParameter.iEnhanceType;		// 增强方式(0-直线，1-指数，2-对数 ...)
            UInt16 wMinValue = data2GrayParameter.wMinValue;			// 最小值
            UInt16 wMaxValue = data2GrayParameter.wMaxValue;			// 最大值
            UInt16 wDnThreshold = data2GrayParameter.wThreshold_min;	// 去掉一些小值后的极小值
            UInt16 wUpThreshold = data2GrayParameter.wThreshold_max;	// 去掉一些大值后的极大值
            byte cMinGray = data2GrayParameter.cMinGray;				// 最小灰度
            byte cMaxGray = data2GrayParameter.cMaxGray;				// 最大灰度
            byte cLessDownGray = data2GrayParameter.cLessDownGray;	// 弱最小灰度
            byte cExceedUpGray = data2GrayParameter.cExceedUpGray;	// 超最大灰度
            bool bAntiPhase = data2GrayParameter.bAntiPhase;			// 是否反相

            // 通道数据
            UInt16[, ,] rsData = dataInfo.RsData;
            // 通道数据灰度指针
            Byte[,] rsDataGray = this.RsDataGray;

            for (int i = 0; i < AmrsFile.ModisDataValueMax; i++)
            {
                rsDataGray[ch, i] = cExceedUpGray;
            }

            int width = dataInfo.Grid.Width;
            int height = dataInfo.Grid.Height;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    ushort wData = (ushort)(rsData[ch, i, j] & 0x0FFF);
                    if (wData == 0)
                    {
                        rsDataGray[ch, wData] = 0;  // 通道值为0的灰度为0
                    }
                    else if (wData > wUpThreshold)
                    {
                        rsDataGray[ch,wData] = cExceedUpGray;
                    }
                    else if (wData < wDnThreshold)
                    {
                        rsDataGray[ch,wData] = cLessDownGray;
                    }
                    else
                    {
                        // 其余进行增强处理
                    }
                }
            }

            ushort wUpValue = wUpThreshold;
            ushort wDownValue = wDnThreshold;

            double a, b;
            switch (iEnhanceType)
            {
                case 0:	// 线性增强
                    a = (double)(cMaxGray - cMinGray) / (wUpValue - wDownValue);
                    b = (double)cMinGray - a * wDownValue;
                    for (value = wDownValue; value < wUpValue + 1; value++)
                    {
                        rsDataGray[ch, value] = (byte)(bAntiPhase ? cMaxGray - (Byte)(a * value + b) : (Byte)(a * value + b));
                    }
                    break;

                case 1:	// 指数增强 y=a*exp(b*x)  ,取点(1,min),(255,max)求出a,b
                    if (wUpValue - wDownValue != 0)
                        b = Math.Log((double)cMaxGray / (wUpValue - wDownValue));
                    else
                        b = 2;

                    a = (double)cMaxGray / Math.Exp(b * wUpValue);
                    for (value = wDownValue; value < wUpValue + 1; value++)
                    {
                        rsDataGray[ch, value] = (byte)(bAntiPhase ? cMaxGray - a * Math.Exp(b * value) : a * Math.Exp(b * value));

                        if (rsDataGray[ch, value] > cMaxGray)
                        {
                            rsDataGray[ch, value] = bAntiPhase ? cMinGray : cMaxGray;
                        }
                    }
                    break;

                case 2:	// 对数增强
                    a = (double)(cMaxGray - cMinGray) / (Math.Log((double)wUpValue) - Math.Log((double)wDownValue));
                    b = (double)cMinGray - a * Math.Log((double)wDownValue);
                    for (value = wDownValue; value < wUpValue + 1; value++)
                    {
                        rsDataGray[ch, value] = (byte)(bAntiPhase ? cMaxGray - (a * Math.Log((double)value) + b) : (a * Math.Log((double)value) + b));
                    }
                    break;
            }
        }

        #endregion

    }
}
