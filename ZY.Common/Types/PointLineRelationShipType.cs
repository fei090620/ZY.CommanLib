using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 点与线段的位置关系
    /// </summary>
    [Flags]
    public enum PointLineRelationShipType
    {
        Point_On_Line = 0x1, //点在线上
        Point_Not_On_Line = 0x2,//点不在线上（不在方向上）
        Point_On_Line_BeginDir = 0x4,//点在起点延长线上
        Point_On_Line_EndDir = 0x8//点在尾点延长线上
    }
}
