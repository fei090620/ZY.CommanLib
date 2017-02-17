using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 温度单位
    /// </summary>
    [Flags]
    public enum TemperatureUomType
    {
        /// <summary>
        /// 摄氏度
        /// </summary>
        C = 0x1,
        /// <summary>
        /// 华氏度
        /// </summary>
        F = 0x2,
        /// <summary>
        /// 开氏度
        /// </summary>
        K = 0x4
    }
}
