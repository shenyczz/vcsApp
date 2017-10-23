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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CSharpKit.Data;

namespace Amrs.Core
{
    public abstract class AmrsFile : MeteoFile
    {
        protected AmrsFile(String fileName)
            : base(fileName) { }

        public static readonly UInt16 ModisMarker = 0x414c;  // 'LA'
        public static readonly int ModisChannelCount = 38;
        public static readonly int ModisChannelCountMax = 40;
        public static readonly UInt16 ModisDataValueMax = 0x0FFF;   // MODIS通道数据最大值

        public int RChannel;			//红色通道
        public int GChannel;			//绿色通道
        public int BChannel;			//蓝色通道

        public int RChannelDef;			//红色通道默认
        public int GChannelDef;			//绿色通道默认
        public int BChannelDef;			//蓝色通道默认

        //通道映射
        public Dictionary<int, RsDataChannel> RsDataChannelMap { get; set; }

        public void SetDataChannelDef(int rChannel, int gChannel, int bChannel)
        {
            this.RChannelDef = rChannel;
            this.GChannelDef = gChannel;
            this.BChannelDef = bChannel;

            this.RChannel = rChannel;
            this.GChannel = gChannel;
            this.BChannel = bChannel;
        }

        public void GetDataChannelDef(out int rChannel, out int gChannel, out int bChannel)
        {
            rChannel = this.RChannelDef;
            gChannel = this.GChannelDef;
            bChannel = this.BChannelDef;
        }

        public void SetDataChannel(int rChannel, int gChannel, int bChannel)
        {
            this.RChannel = rChannel;
            this.GChannel = gChannel;
            this.BChannel = bChannel;
        }
        public void GetDataChannel(out int rChannel, out int gChannel, out int bChannel)
        {
            rChannel = this.RChannel;
            gChannel = this.GChannel;
            bChannel = this.BChannel;
        }


        public static Boolean IsAmrsData(String fileName)
        {
            SensorType sensorType = AmrsFile.GetSensorType(fileName);
            return sensorType != SensorType.None;
        }
        public static Boolean IsLd2File(String fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            // 文件头
            int nHeadSize = Marshal.SizeOf(typeof(LD2Header));
            byte[] bs = br.ReadBytes(nHeadSize);
            LD2Header ld2h = (LD2Header)AmrsHelper.BytesToStruct(bs, typeof(LD2Header));

            br.Close();
            fs.Close();

            return ld2h.Version == 2002;
        }
        public static Boolean IsLd3File(String fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            // 文件头
            int nHeadSize = Marshal.SizeOf(typeof(LD3Header));
            byte[] bs = br.ReadBytes(nHeadSize);
            LD3Header ld3h = (LD3Header)AmrsHelper.BytesToStruct(bs, typeof(LD3Header));

            br.Close();
            fs.Close();

            return ld3h.Version == 2006;
        }

        public static SensorType GetSensorType(String fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            // 文件头
            int nHeadSize = Marshal.SizeOf(typeof(LDHeader));
            byte[] bs = br.ReadBytes(nHeadSize);
            LDHeader ldh = (LDHeader)AmrsHelper.BytesToStruct(bs, typeof(LDHeader));

            br.Close();
            fs.Close();

            return ldh.SensorType;
        }
    }
}
