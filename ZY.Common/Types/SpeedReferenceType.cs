using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 速度参考类型
    /// </summary>
    [Flags]
    public enum SpeedReferenceType
    {
        /// <summary>
        /// 地速
        /// </summary>
        GS = 0x1,

        /// <summary>
        /// 指示空速
        /// </summary>
        IAS = 0x2,

        /// <summary>
        /// 真空速
        /// </summary>
        TAS = 0x4
    }
}
