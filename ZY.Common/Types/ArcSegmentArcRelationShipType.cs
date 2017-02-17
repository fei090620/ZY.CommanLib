using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Common.Types
{
    /// <summary>
    /// 圆弧与圆弧的位置关系
    /// </summary>
    public enum ArcSegmentArcRelationShipType
    {
        InnerTangent = 0, //相切(内)
        OuterTangent = 1, //相切(外)
        Insert = 2, //相交
        Apart = 3, //相离
        Others = 4 //其他情况
    }
}
