using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Common.Types
{
    /// <summary>
    /// 高度用法类型
    /// </summary>
    public enum AltitudeUseType
    {
        ABOVE_LOWER,   //low
        AS_ASSIGNED,
        AT_LOWER,         //low
        BELOW_UPPER, //up
        BETWEEN,         //low  up
        EXPECT_LOWER,        //low
        RECOMMENDED,   //low
        OTHER
    }
}
