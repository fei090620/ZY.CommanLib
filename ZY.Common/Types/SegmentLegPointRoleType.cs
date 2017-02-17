using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 航段 点角色类型
    /// </summary>
    [Flags]
    public enum SegmentLegPointRoleType
    {
        /// <summary>
        /// 起始进近定位点
        /// </summary>
        IAF = 0x1,

        /// <summary>
        /// 中间进近定位点
        /// </summary>
        IF = 0x2,

        /// <summary>
        /// 最后进近定位点
        /// </summary>
        FAF = 0x4,

        /// <summary>
        /// 梯级下降点
        /// </summary>
        SDF = 0x8,
        FPAP = 0x10,
        FTP = 0x20,
        FROP = 0x40,
        TP = 0x80,

        /// <summary>
        /// 复飞点
        /// </summary>
        MAPT = 0x100,
        MAHF = 0x200
    }
}
