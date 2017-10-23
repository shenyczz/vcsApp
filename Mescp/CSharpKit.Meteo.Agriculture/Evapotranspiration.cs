/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     RgnName: Evapotranspiration - 蒸散
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

namespace CSharpKit.Meteo.Agriculture
{
    /// <summary>
    /// Evapotranspiration - 蒸散
    ///     需要指定站点的9个参数
    /// </summary>
    public class Evapotranspiration
    {
        #region 属性参数

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 纬度(°)
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// 海拔(m)
        /// </summary>
        public double Alt { get; set; }
        /// <summary>
        /// 气温(℃)
        /// </summary>
        public double T { get; set; }
        /// <summary>
        /// 最高气温(℃)
        /// </summary>
        public double Tmax { get; set; }
        /// <summary>
        /// 最低气温(℃)
        /// </summary>
        public double Tmin { get; set; }
        /// <summary>
        /// 风速(m/s)
        /// </summary>
        public double Ws { get; set; }
        /// <summary>
        /// 实际水汽压(hPa)
        /// </summary>
        public double E { get; set; }
        /// <summary>
        /// 日照时数(h)
        /// </summary>
        public double Hos { get; set; }

        #endregion

        /// <summary>
        /// 潜在蒸散(mm·d-1)
        /// </summary>
        /// <returns></returns>
        public double Et0()
        {
            double dValue = 0;
            //------------------------------------
            double lat = this.Lat;          //纬度(°)
            double alt = this.Alt;          //海拔(m)
            DateTime dt = this.DateTime;    //日期
            double tavg = this.T;           //气温(℃)
            double tmax = this.Tmax;        //最高气温(℃)
            double tmin = this.Tmin;        //最低气温(℃)
            double ea = this.E;             //实际水汽压(hPa)
            double v = this.Ws;             //风速(m/s)
            double s = this.Hos;            //实际日照时数(h)
            //------------------------------------
            double a1 = 0.408 * Delta(ea, tavg) * Rn(tmax, tmin, ea, lat, dt);
            double a2 = 900.0 * Gama(Ph(alt)) * U2(v, 10) * (Es(tmax, tmin) / 10 - ea / 10) / (tavg + 273);
            double a3 = Delta(ea, tavg) + Gama(Ph(alt)) * (1 + 0.34 * U2(v, 10));
            //------------------------------------
            dValue = (a1 + a2) / a3;
            //------------------------------------
            return dValue;
        }


        #region Private Functions

        /// <summary>
        /// 计算饱和水汽压-温度曲线斜率(hPa/℃)
        /// </summary>
        /// <param name="ea">实际气压(hPa)</param>
        /// <param name="tavg">日平均气温(℃)</param>
        /// <returns>ok</returns>
        private double Delta(double ea, double tavg)
        {
            double dValue = 0;
            //--------------------------------------------
            double temp = Math.Pow((tavg + 237.3), 2);
            dValue = 4098.0 * ea / temp;
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 计算干湿表常数(hPa/℃)
        /// </summary>
        /// <param name="ph">本站气压(hPa)</param>
        /// <returns>ok</returns>
        private double Gama(double ph)
        {
            double dValue = 0;
            //--------------------------------------------
            dValue = ph * 0.00163 / 2.45;
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 计算本站气压(hPa)
        /// </summary>
        /// <param name="alt">站点海拔(m)</param>
        /// <returns>ok</returns>
        private double Ph(double alt)
        {
            double dValue = 0;
            //--------------------------------------------
            double temp = (293.0 - 0.0064 * alt) / 293.0;
            dValue = Math.Pow(temp, 5.26) * 1013.0;
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 计算2m高度处平均风速(m/s) [对于常规气象台站，u为10米高度处风速,z=10m]
        /// </summary>
        /// <param name="u">风速(m/s)</param>
        /// <param name="z">观测高度(m)</param>
        /// <returns>ok</returns>
        private double U2(double u, double z)
        {
            double dValue = 0;
            //--------------------------------------------
            dValue = 4.87 * u / Math.Log(67.8 * z - 5.42);
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 计算饱和水汽压(hPa)
        /// </summary>
        /// <param name="tmax">最高气温(℃)</param>
        /// <param name="tmin">最低气温(℃)</param>
        /// <returns>ok</returns>
        private double Es(double tmax, double tmin)
        {
            double dValue = 0;
            //--------------------------------------------
            dValue = (E0(tmax) + E0(tmin)) / 2;
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 计算水汽压(hPa)
        /// </summary>
        /// <param name="t">气温(℃)</param>
        /// <returns>ok</returns>
        private double E0(double t)
        {
            double dValue = 0;
            //--------------------------------------------
            dValue = 6.108 * Math.Exp((17.27 * t) / (t + 237.3));
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 作物冠层表面净辐射(MJ.m^-2.d^-1)
        /// </summary>
        /// <param name="tmax">最高温度(℃)</param>
        /// <param name="tmin">最低温度(℃)</param>
        /// <param name="ea">实际水汽压(hPa)</param>
        /// <param name="lat">纬度(度)</param>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        private double Rn(double tmax, double tmin, double ea, double lat, DateTime dateTime)
        {
            double dValue = 0;
            //--------------------------------------------
            dValue = Rns(tmax, tmin, lat, dateTime) - Rnl(tmax, tmin, ea, lat, dateTime);
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 净短波辐射(MJ.m^-2.d^-1)
        /// </summary>
        /// <param name="hos">日照时数(h)</param>
        /// <param name="lat">纬度(度)</param>
        /// <param name="dateTime">日期</param>
        /// <returns>ok</returns>
        private double Rns(double tmax, double tmin, double lat, DateTime dateTime)
        {
            double dValue = 0;
            //--------------------------------------------
            dValue = 0.77 * Rs(tmax, tmin, lat, dateTime);
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 净长波辐射(MJ.m^-2.d^-1)
        /// </summary>
        /// <param name="tmax">最高温度(℃)</param>
        /// <param name="tmin">最低温度(℃)</param>
        /// <param name="ea">实际水汽压(hPa)</param>
        /// <param name="lat">纬度(度)</param>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        private double Rnl(double tmax, double tmin, double ea, double lat, DateTime dateTime)
        {
            double dValue = 0;
            //--------------------------------------------
            double tmaxk = tmax + 273.16;
            double tmink = tmin + 273.16;

            double a0 = 4.903e-9;   // 斯蒂芬-波尔兹曼常数

            double a1 = (Math.Pow(tmaxk, 4) + Math.Pow(tmink, 4)) / 2;
            double a2 = (0.34 - 0.14 * Math.Sqrt(ea / 10));
            double a3 = 1.35 * Rs(tmax, tmin, lat, dateTime) / Rso(lat, dateTime) - 0.35;

            dValue = a0 * a1 * a2 * a3;
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 太阳总辐射(MJ.m^-2.d^-1)
        /// </summary>
        /// <param name="lat">纬度(°)</param>
        /// <param name="dateTime">日期</param>
        /// <returns>ok</returns>
        private double Ra(double lat, DateTime dateTime)
        {
            double dValue = 0;
            //--------------------------------------------
            double rad_lat = lat * Math.PI / 180.0;         //纬度转换成弧度
            double dr = DistanceSolarEarth(dateTime);       //日地相对距离
            double sa = SunsetAngle(lat, dateTime);         //日落时角度
            double dets = SolarDeclinationAngle(dateTime);  //太阳磁偏角
            //计算
            dValue = 37.6 * dr * (sa * Math.Sin(rad_lat) * Math.Sin(dets) + Math.Cos(rad_lat) * Math.Cos(dets) * Math.Sin(sa));
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 太阳辐射(MJ.m^-2.d^-1)
        /// </summary>
        /// <param name="tmax">最高温度(℃)</param>
        /// <param name="tmin">最低温度(℃)</param>
        /// <param name="lat">纬度(度)</param>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        private double Rs(double tmax, double tmin, double lat, DateTime dateTime)
        {
            double dValue = 0;
            //--------------------------------------------
            double kr = 0.18;
            dValue = kr * Math.Sqrt(tmax - tmin) * Ra(lat, dateTime);
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 晴空辐射(MJ.m^-2.d^-1)
        /// </summary>
        /// <param name="lat">纬度(度)</param>
        /// <param name="dateTime">日期</param>
        /// <returns></returns>
        private double Rso(double lat, DateTime dateTime)
        {
            double dValue = 0;
            //--------------------------------------------
            dValue = 0.75 * Ra(lat, dateTime);
            //--------------------------------------------
            return dValue;
        }

        #endregion


        #region Public Functions - Static

        /// <summary>
        /// 日地相对距离(m)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static double DistanceSolarEarth(DateTime dateTime)
        {
            double dValue = 0;
            //--------------------------------------------
            double pi = Math.PI;
            double doy = dateTime.DayOfYear;
            dValue = 1.0 + 0.033 * Math.Cos(2 * pi * doy / 365);
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 太阳赤纬角(磁偏角)(rad)
        /// </summary>
        /// <returns></returns>
        public static double SolarDeclinationAngle(DateTime dateTime)
        {
            double dValue = 0;
            //--------------------------------------------
            double pi = Math.PI;
            double doy = dateTime.DayOfYear;
            dValue = 0.409 * Math.Sin(2 * pi * doy / 365 - 1.39);
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 日落角度(rad)
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static double SunsetAngle(double lat, DateTime dateTime)
        {
            double dValue = 0;
            //--------------------------------------------
            double rad_lat = lat * Math.PI / 180.0;         //纬度转换成弧度
            double sda = SolarDeclinationAngle(dateTime);   //太阳赤纬角

            dValue = Math.Acos(-1.0 * Math.Tan(rad_lat) * Math.Tan(sda));
            //--------------------------------------------
            return dValue;
        }

        /// <summary>
        /// 可照时数(h)
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static double Sunable(double lat, DateTime dateTime)
        {
            double dValue = 0;
            //--------------------------------------------
            double sa = SunsetAngle(lat, dateTime);
            dValue = 24.0 * sa / Math.PI;
            //--------------------------------------------
            return dValue;
        }


        #endregion

    }
}
