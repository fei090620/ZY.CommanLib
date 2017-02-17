using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 点与圆弧的位置关系
    /// </summary>
    [Flags]
    public enum PointArcRelationShipType
    {
        On_ArcSegment = 0x1, //点在圆弧上
        On_Arc = 0x2, //点不在圆弧上，但在圆弧所在的圆上
        In_Arc = 0x4, //点在圆弧所在的圆内，但不在圆弧和圆心之间
        In_ArcSegment_Center = 0x8,//点在圆弧与圆心之间
        To_ArcSegment_Center = 0x10, //点在圆弧所在方位
        Back_ArcSegment_Center = 0x20, //点在圆弧背离的方位
        OTHER = 0x40
    }
}
