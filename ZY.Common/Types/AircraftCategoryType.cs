using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 航空器类型
    /// </summary>
    [Flags]
    public enum AircraftCategoryType
    {
        A = 0x1,
        B = 0x2,
        C = 0x4,
        D = 0x8,
        E = 0x10,
        H = 0x20,
        ABCDE = A | B | C | D | E,
        ALL = ABCDE | H 
    }
}
