using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 航路点类型
    /// </summary>
    [Flags]
    public enum AirwayPointType
    {
        Fly_By = 0x1,
        Fly_Over = 0x2,
        OTHER = 0x4
    }
}
