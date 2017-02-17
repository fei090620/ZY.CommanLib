using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// RNAV导航类型
    /// </summary>
    [Flags]
    public enum SensorsType
    {
        DME_DME = 0x1,
        GBAS = 0x2,
        GNSS = 0x4,
        SBAS = 0x8
    }
}
