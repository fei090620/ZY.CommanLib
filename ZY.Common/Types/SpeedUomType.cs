using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 速度单位类型
    /// </summary>
    [Flags]
    public enum SpeedUomType
    {
        KT = 0x1, // kt
        KMperH = 0x2,// km/h
        MperS = 0x4 // m/s
    }
}
