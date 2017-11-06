using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpKit.Meteo.Agriculture;
using Mescp.Models;

namespace Mescp
{
    public class AppMethod
    {
        /// <summary>
        /// Ft - 气温适宜度模型
        /// </summary>
        /// <param name="t"></param>
        /// <param name="t0"></param>
        /// <param name="tl"></param>
        /// <param name="th"></param>
        /// <returns></returns>
        public double Ft(double t, double t0, double tl, double th)
        {
            double v = 1;

            if (t >= tl && t <= th)
            {
                v = ((t - tl) * (th - t)) / ((t0 - tl) * (th - t0));
            }

            return v;
        }

        /// <summary>
        /// Fr - 降水适宜度模型
        /// </summary>
        /// <remarks>
        /// 由于单日的降水不能反映降水对玉米的影响，
        /// 因而选取连续10日的蒸散和降水数据,滚动分析(算术和)降水对玉米的影响情况。
        /// 用第10日的降水和蒸散情况表示当日的降水适宜度情况
        /// 1.计算连续10日蒸散和 => W
        /// 2.计算连续10日降水和 => R
        /// 
        ///      |-- 1 (R>W)
        /// Fr = |
        ///      |-- R / W
        /// </remarks>
        /// <returns></returns>
        public double Fr(XStation xStation, DateTime currentDateTime)
        {
            TimeSpan timeSpan = TimeSpan.FromDays(1);

            double W = 0, R = 0;

            DateTime dtBeg = currentDateTime - TimeSpan.FromDays(10);
            DateTime dtEnd = currentDateTime;
            for (DateTime dt = dtBeg; dt <= dtEnd; dt += timeSpan)
            {
                MeteoElement me = xStation.MeteoElements.Find(p => p.DateTime == dt);
                if (me == null)
                    continue;

                Evapotranspiration_t eca = new Evapotranspiration_t()
                {
                    DateTime = dt,
                    Lat = xStation.Lat,
                    Alt = xStation.Alt,
                    T = me.T,
                    Tmax = me.Tmax,
                    Tmin = me.Tmin,
                    E = me.E,
                    Ws = me.Ws,
                };

                W += eca.Et0();
                R += me.R;
            }

            double x = W;

            return W > R ? R / W : 1;
        }

        /// <summary>
        /// Fs - 日照适宜度模型
        /// </summary>
        /// <returns></returns>
        public double Fs(double s, double s0)
        {
            double v = 0.0001;

            if (s > 0 && s0 != 0 && s < s0)
            {
                v = s / s0;
            }

            return v;
        }

        /// <summary>
        /// Fc - 当日适宜度模型
        /// </summary>
        /// <remarks>
        /// 是当日温度、降水、日照适宜度模型的几何平均值
        /// </remarks>
        /// <param name="ft"></param>
        /// <param name="fr"></param>
        /// <param name="fs"></param>
        /// <returns>几何平均值</returns>
        public double Fc(double ft, double fr, double fs)
        {
            return Math.Pow(ft * fr * fs, 1.0 / 3);
        }

        /// <summary>
        /// 各发育阶段适宜度模型
        /// 本系统玉米发育期划分为3个发育阶段：出苗-拔节期、拔节-抽雄期、抽雄-成熟期
        /// </summary>
        /// <param name="fcs">生育阶段每天适宜度集合</param>
        /// <returns></returns>
        public double Fg(IList<double> fcs)
        {
            double v = 0;

            foreach (double fc in fcs)
            {
                v += fc;
            }

            return v;
        }

        /// <summary>
        /// 整个生育阶段适宜度模型
        /// 本系统玉米发育期划分为3个发育阶段：出苗-拔节期、拔节-抽雄期、抽雄-成熟期
        /// </summary>
        /// <param name="fgs">各个发育阶段适宜度</param>
        /// <param name="gws">各个发育阶段适宜度系数(对应Weight)</param>
        /// <returns></returns>
        public double Fa(IList<double> fgs, IList<double> gws)
        {
            double v = 0;

            for (int i = 0; i < fgs.Count; i++)
            {
                v += fgs[i] * gws[i];
            }

            return v;
        }

        /// <summary>
        /// Fae - 气候适应度
        ///     -1：未知
        ///      0：不适宜
        ///      1：次适宜
        ///      2：适宜
        /// </summary>
        /// <param name="fz">整个发育期适宜度模型</param>
        /// <param name="fmax"></param>
        /// <param name="fmin"></param>
        /// <returns></returns>
        public int Fae(double fz, double fmax, double fmin)
        {
            int v = -1;

            if (fz < 0.2 * fmax + 0.8 * fmin)
                v = 0;
            else if (fz >= 0.8 * fmax + 0.2 * fmin)
                v = 2;
            else
                v = 1;

            return v;
        }
    }
}
