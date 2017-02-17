using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 转弯方向
    /// </summary>
    [Flags]
    public enum TurnDirectionType
    {
        Left = 0x1,
        Right = 0x2,
        Stright = 0x4,
        OTHER = 0x8
    }
}
