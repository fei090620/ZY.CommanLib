using ZY.Common.Types;
using System;
using System.Diagnostics.Contracts;

namespace ZY.Common.Tools
{
    /// <summary>
    /// 数据格式化工具类
    /// </summary>
    public static class FormateTool
    {
        private static readonly int _degreeToMiniute = 60;
        private static readonly int _miniuteToSecond = 60;
        private static readonly int _degreeToSecond = 3600;

        /// <summary>
        /// 将角度（格式为弧度值,范围不限）转换为0-2*PI范围内的弧度值（标准弧度）
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double ToStandardRadian(double radian)
        {
            if (double.IsNaN(radian))
                return radian;

            double standarRadian = 0;
            if (radian >= 0)
            {
                standarRadian = radian % (Math.PI * 2);
            }
            else
            {
                standarRadian = Math.PI * 2 + radian % (Math.PI * 2);
            }

            //standarRadian = Math.Abs(radian) % (Math.PI * 2);
            //while (!(standarRadian >= 0 && standarRadian < Math.PI * 2))
            //{
            //    if (standarRadian >= Math.PI * 2)
            //        standarRadian -= Math.PI * 2;
            //    if (standarRadian < 0)
            //        standarRadian += Math.PI * 2;
            //}

            return standarRadian;
        }

        /// <summary>
        /// 纬度转化为度（带单位）
        /// </summary>
        /// <param name="degree"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <param name="latitudeType"></param>
        /// <returns></returns>
        public static double ToStandardRadian(double subDegree, double subMinute, double subSecond, LatitudeType latitudeType)
        {
            var totalDegree = subDegree + subMinute / _degreeToMiniute + subSecond / _degreeToSecond;
            var type = latitudeType == LatitudeType.N ? 1 : -1;
            return type * totalDegree;
        }

        /// <summary>
        /// 经度转化为度（带单位）
        /// </summary>
        /// <param name="subDegree"></param>
        /// <param name="subMinute"></param>
        /// <param name="subSecond"></param>
        /// <param name="longitudeType"></param>
        /// <returns></returns>
        public static double ToStandardRadian(double subDegree, double subMinute, double subSecond, LongitudeType longitudeType)
        {
            var totalDegree = subDegree + subMinute / _degreeToMiniute + subSecond / _degreeToSecond;
            var type = longitudeType == LongitudeType.E ? 1 : -1;
            return type * totalDegree;
        }

        /// <summary>
        /// 根据标准经纬度截取度（无单位）
        /// </summary>
        /// <param name="totalDegree"></param>
        /// <returns></returns>
        public static double GetSubDegree(double totalDegree)
        {
            return Math.Ceiling(Math.Abs(totalDegree));
        }

        /// <summary>
        /// 根据标准经纬度截取分（无单位）
        /// </summary>
        /// <param name="totalDegree"></param>
        /// <returns></returns>
        public static double GetSubMinute(double totalDegree)
        {
            return Math.Ceiling((totalDegree - GetSubDegree(totalDegree)) * _degreeToMiniute);
        }

        /// <summary>
        /// 根据标准经纬度截取秒（无单位）
        /// </summary>
        /// <param name="totalDegree"></param>
        /// <returns></returns>
        public static double GetSubSecond(double totalDegree)
        {
            return ((totalDegree - GetSubDegree(totalDegree)) * _degreeToMiniute - GetSubMinute(totalDegree)) * _degreeToMiniute;
        }

        /// <summary>
        /// 根据标准经纬度获取纬度单位
        /// </summary>
        /// <param name="totalDegree"></param>
        /// <returns></returns>
        public static LatitudeType GetLatitudeType(double totalDegree)
        {
            return totalDegree > 0 ? LatitudeType.N : LatitudeType.S;
        }

        /// <summary>
        /// 根据标准经纬度获取经度单位
        /// </summary>
        /// <param name="totalDegree"></param>
        /// <returns></returns>
        public static LongitudeType GetLongitudeType(double totalDegree)
        {
            return totalDegree > 0 ? LongitudeType.E : LongitudeType.W;
        }

        /// <summary>
        /// 把double类型的值转化为按精度划分的份数
        /// </summary>
        /// <param name="doubleValue"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static int ToStandardByPrecision(this double doubleValue, double precision = OverrallVraTool.DoublePrecision)
        {
            Contract.Requires<ArgumentNullException>(!double.IsNaN(doubleValue));
            return (int)Math.Round(doubleValue / precision);
        }
    }
}
