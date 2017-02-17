using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 导航台设备类型
    /// </summary>
    [Flags]
    public enum NavaidEquipmentType
    {
        /// <summary>
        /// 组合导航设备
        /// </summary>
        ILS_DME = 0x1,
        MLS_DME = 0x2,
        NDB_DME = 0x4,
        LOC_DME = 0x8,
        NDB_MKR = 0x10,
        /// <summary>
        /// 合装导航设备
        /// </summary>
        VOR_DME = 0x20,
        VOR_TACAN = 0x40,
        /// <summary>
        /// 单台导航设备
        /// </summary>
        VOR = 0x80,
        DME = 0x100,
        NDB = 0x200,
        TACAN = 0x400,
        MKR = 0x800,
        ILS = 0x1000,
        MLS = 0x2000,
        TLS = 0x4000,
        LOC = 0x8000,
        GP = 0x10000,
        DF = 0x20000,
        SDF = 0x40000
    }

    /// <summary>
    /// 单个导航台类型
    /// </summary>
    [Flags]
    public enum NavaidType
    {
        VOR = 0x1,
        DME = 0x2,
        NDB = 0x4,
        TACAN = 0x8,
        MKR = 0x10,
        ILS = 0x20,
        MLS = 0x40,
        TLS = 0x80,
        LOC = 0x100,
        DF = 0x200,
        SDF = 0x400,
        GP = 0x800
    }
}
