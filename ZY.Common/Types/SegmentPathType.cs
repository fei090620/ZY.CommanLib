using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 航径编码类型
    /// </summary>
    [Flags]
    public enum SegmentPathType
    {
        OTHER = 0x1,
        CA = 0x2,
        CF = 0x4,
        TF = 0x8,
        DF = 0x10,
        AF = 0x20,
        CD = 0x40,
        CI = 0x80,
        CR = 0x100,
        FA = 0x200,
        FC = 0x400,
        FD = 0x800,
        FM = 0x1000,
        HA = 0x2000,
        HF = 0x4000,
        IF = 0x8000,
        PI = 0x10000,
        RF = 0x20000,
        VA = 0x40000,
        VI = 0x80000,
        VM = 0x100000,
        VR = 0x200000,
        VD = 0x400000,
        HM = 0x800000
    }
}
