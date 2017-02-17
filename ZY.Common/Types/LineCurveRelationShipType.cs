using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Common.Types
{
    /// <summary>
    /// 直线与曲线段的位置关系
    /// </summary>
    public enum LineCurveRelationShipType
    {
        Left, //曲线段在直线左侧且无交点
        Right, //曲线段在直线右侧且无交点
        On_Left, //曲线段在直线左侧且有一个交点
        On_Right, //曲线段在直线右侧且有一个交点
        Coincide, //重合（曲线段为直线）
        Intersect, //相交（可能有多个交点）
        Others
    }
}
