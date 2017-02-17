using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 逻辑跑道 跑道标志类型
    /// </summary>
    [Flags]
    public enum ApproachGuidanceType
    {
        ILS_PRECISION_CAT_I = 0x1,
        ILS_PRECISION_CAT_II = 0x2,
        ILS_PRECISION_CAT_IIIA = 0x4,
        ILS_PRECISION_CAT_IIIB = 0x8,
        ILS_PRECISION_CAT_IIIC = 0x10,
        ILS_PRECISION_CAT_IIID = 0x20,
        MLS_PRECISION = 0x40,
        NON_PRECISION = 0x80,
        ALL = ILS_PRECISION_CAT_I 
            | ILS_PRECISION_CAT_II 
            | ILS_PRECISION_CAT_IIIA 
            | ILS_PRECISION_CAT_IIIB 
            | ILS_PRECISION_CAT_IIIC 
            | ILS_PRECISION_CAT_IIID
            | MLS_PRECISION
            | NON_PRECISION
    }
}
