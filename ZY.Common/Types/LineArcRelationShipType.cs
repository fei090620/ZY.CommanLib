namespace ZY.Common.Types
{
    /// <summary>
    /// 直线与圆弧的位置关系
    /// </summary>
    public enum LineArcRelationShipType
    {
        Tangent = 0x1, //相切(切点在圆弧上)
        Insert = 0x2, //相交（交点在圆弧上）
        Apart = 0x3, //相离
        TangentWithArc = 0x4,//相切(切点不在圆弧上，但在圆弧所在的圆上)
        InsertWithArc = 0x5,//相交（交点不在圆弧上，但在圆弧所在的圆上）
        InsertWithArcSegmentAndArc = 0x6,//相交（一个交点在圆弧上，另一个交点不在圆弧上，但在圆弧所在的圆上）
        Others = 0x7
    }
}
