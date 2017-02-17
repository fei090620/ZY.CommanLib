using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 飞行程序导航类型
    /// </summary>
    [Flags]
    public enum ProcedureNavigationType
    {
        RNAV5 = 0x1,
        RNAV2 = 0x2,
        RNAV1 = 0x4,
        RNP4 = 0x8,
        RNP2 = 0x10,
        RNP1 = 0x20,
        AdvancedRNP = 0x40,
        // RNP0.3
        RNP03 = 0x80,
        RNPAPCH = 0x100,
        /// <summary>
        /// RNAV导航
        /// </summary>
        RNAV = RNAV1 | RNAV2 | RNAV5 | RNP1 | RNP2 | RNP4 | RNPAPCH | RNP03 | AdvancedRNP,

        /// <summary>
        /// 传统导航
        /// </summary>
        Traditional = 0x200,
        Any = Traditional | RNAV
    }
}
