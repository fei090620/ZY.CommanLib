using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 垂直距离单位
    /// </summary>
    [Flags]
    public enum VerticalDistanceUomType
    {
        M = 0x1,
        KM = 0x2,
        FT = 0x4
    }
}
