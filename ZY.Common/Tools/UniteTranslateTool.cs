using System;

namespace ZY.Common.Tools
{
    /// <summary>
    /// 单位转换类
    /// </summary>
    public static class UniteTranslateTool
    {
        private static readonly double _meterToFt = 3.28;
        private static readonly double _kilometerToFt = 3280;
        private static readonly int _kilometerToMeter = 1000;
        private static readonly int _mileToMeter = 1852;
        private static readonly double _mileToKilometer = 1.852;
        private static readonly double _mperSecondToKmperHour = 3.6;
        private static readonly double _ktToKmperHour = 1.852;
        private static readonly double _ktToMperSecond = 0.514444;
        private static readonly int _mHzToKHz = 1000;

        #region 角度
        /// <summary>
        /// 弧度转换为角度
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double ToAngle(this double radian)
        {
            if (radian < 0)
                radian += Math.PI * 2;
            if (radian > 360)
                radian -= Math.PI * 2;
            return radian * 180 / Math.PI;
        }

        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double ToRadian(this double angle)
        {
            return angle * Math.PI / 180;
        }
        #endregion

        #region 距离或高度
        /// <summary>
        /// 米转换为英尺
        /// </summary>
        /// <param name="meter"></param>
        /// <returns></returns>
        public static double MeterToFt(this double meter)
        {
            return meter * _meterToFt;
        }

        /// <summary>
        /// 米转换为千米
        /// </summary>
        /// <param name="meter"></param>
        /// <returns></returns>
        public static double MeterToKilometer(this double meter)
        {
            return meter / _kilometerToMeter;
        }

        /// <summary>
        /// 千米转换为米
        /// </summary>
        /// <param name="kiloMeter"></param>
        /// <returns></returns>
        public static double KilometerToMeter(this double kiloMeter)
        {
            return kiloMeter * _kilometerToMeter;
        }

        /// <summary>
        /// 英尺转换为米
        /// </summary>
        /// <param name="fit"></param>
        /// <returns></returns>
        public static double FtToMeter(this double fit)
        {
            return fit / _meterToFt;
        }

        /// <summary>
        /// ft转换为km
        /// </summary>
        /// <param name="fit"></param>
        /// <returns></returns>
        public static double FtToKilometer(this double fit)
        {
            return fit / _kilometerToFt;
        }

        /// <summary>
        /// km转换为ft
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static double KilometerToFt(this double km)
        {
            return km * _kilometerToFt;
        }

        /// <summary>
        /// km转换为海里
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static double KilometerToMile(this double km)
        {
            return km / _mileToKilometer;
        }

        /// <summary>
        /// 海里转换为km
        /// </summary>
        /// <param name="mile"></param>
        /// <returns></returns>
        public static double MileToKilometer(this double mile)
        {
            return mile * _mileToKilometer;
        }

        /// <summary>
        /// 米转化为海里
        /// </summary>
        /// <param name="meter"></param>
        /// <returns></returns>
        public static double MeterToMile(this double meter)
        {
            return meter / _mileToMeter;
        }

        /// <summary>
        /// 海里转化为米
        /// </summary>
        /// <param name="nmi"></param>
        /// <returns></returns>
        public static double MileToMeter(this double nmi)
        {
            return nmi * _mileToMeter;
        }
        #endregion

        #region 速度
        /// <summary>
        /// 速度km/h转化为m/s
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static double KM_HtoM_S(this double speed)
        {
            return speed / _mperSecondToKmperHour;
        }

        /// <summary>
        /// 速度m/s转化为km/h
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static double M_StoKM_H(this double speed)
        {
            return speed * _mperSecondToKmperHour;
        }

        /// <summary>
        /// 速度Kt转化为km/h
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static double KttoKM_H(this double speed)
        {
            return speed * _ktToKmperHour;
        }

        /// <summary>
        /// km/h转化为Kt
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static double KM_HtoKt(this double speed)
        {
            return speed / _ktToKmperHour;
        }

        /// <summary>
        /// m/s转化为Kt
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static double M_StoKt(this double speed)
        {
            return speed / _ktToMperSecond;
        }

        /// <summary>
        /// Kt转化为m/s
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static double KttoM_S(this double speed)
        {
            return speed * _ktToMperSecond;
        }

        /// <summary>
        /// KHz转为MHz
        /// </summary>
        /// <param name="kHz"></param>
        /// <returns></returns>
        public static double KHzToMHz(this double kHz)
        {
            return kHz / _mHzToKHz;
        }

        /// <summary>
        /// MHz转为KHz
        /// </summary>
        /// <param name="mHz"></param>
        /// <returns></returns>
        public static double MHzToKHz(this double mHz)
        {
            return mHz * _mHzToKHz;
        }
        #endregion

        #region 温度
        /// <summary>
        /// 摄氏度转开氏度
        /// </summary>
        /// <param name="c">摄氏度</param>
        /// <returns>开氏度</returns>
        public static double TemperatureCtoK(this double c)
        {
            return c  + 273.15;
        }

        /// <summary>
        /// 摄氏度转华氏度
        /// </summary>
        /// <param name="c">摄氏度</param>
        /// <returns>华氏度</returns>
        public static double TemperatureCtoF(this double c)
        {
            return (c * 9 / 5) + 32;
        }

        /// <summary>
        /// 开氏度转摄氏度
        /// </summary>
        /// <param name="k">开氏度</param>
        /// <returns>摄氏度</returns>
        public static double TemperatureKtoC(this double k)
        {
            return k - 273.15;
        }

        /// <summary>
        /// 华氏度转摄氏度
        /// </summary>
        /// <param name="h">华氏度</param>
        /// <returns>摄氏度</returns>
        public static double TemperatureFtoC(this double h)
        {
            return (h - 32) * 5 / 9;
        }

        /// <summary>
        /// 华氏度转开氏度
        /// </summary>
        /// <param name="h">华氏度</param>
        /// <returns>开氏度</returns>
        public static double TemperatureFtoK(this double h)
        {
            return h.TemperatureFtoC().TemperatureCtoK();
        }

        /// <summary>
        /// 开氏度转华氏度
        /// </summary>
        /// <param name="k">开氏度</param>
        /// <returns>华氏度</returns>
        public static double TemperatureKtoF(this double k)
        {
            return k.TemperatureKtoC().TemperatureCtoF();
        }
        #endregion

    }
}
