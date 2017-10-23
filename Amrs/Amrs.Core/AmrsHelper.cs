/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: AmrsHelper
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Amrs.Core
{
    public static class AmrsHelper
    {
        public static Byte[] StructToBytes(Object structObj)
        {
            // 取得结构体大小
            int size = Marshal.SizeOf(structObj);

            // 分配数组
            Byte[] bytes = new Byte[size];

            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);

            return bytes;
        }
        public static Object BytesToStruct(Byte[] bytes, Type structType)
        {
            int size = Marshal.SizeOf(structType);
            if (bytes.Length < size)
                return null;

            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, structPtr, size);
            Object obj = Marshal.PtrToStructure(structPtr, structType);
            Marshal.FreeHGlobal(structPtr);

            return obj;
        }


        //#define DIBWIDTHBYTES(bits)	((((bits)+31)/32)*4)	//DIB宽度字节数(必须是4的整数倍)
        public static int DibWidthBytes(int bits)
        {
            return ((((bits) + 31) / 32) * 4);
        }
    }
}
