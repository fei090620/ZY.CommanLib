using System;

namespace ZY.Common.Types
{
    /// <summary>
    /// 程序段类型
    /// </summary>
    [Flags]
    public enum ProcedurePhaseType
    {
        RWY = 0x1,
        COMMON = 0x2,
        EN_ROUTE = 0x4,
        APPROACH = 0x8,
        FINAL = 0x10,
        MISSED = 0x20,
        MISSED_P = 0x40,
        MISSED_S = 0x80,
        ENGINE_OUT = 0x100,
        OTHER = 0x200
    }

    /// <summary>
    /// 程序类型
    /// </summary>
    [Flags]
    public enum ProcedureType
    {
        SID = 0x1,
        STAR = 0x2,
        APPROACH = 0x4,
        OTHER = 0x8
    }

    /// <summary>
    /// 飞行阶段
    /// </summary>
    [Flags]
    public enum FlyStageType
    {
        /// <summary>
        /// 海洋
        /// </summary>
        Ocean = 0x1,
        /// <summary>
        /// 航路
        /// </summary>
        EnRoute = 0x2,
        /// <summary>
        /// 进场
        /// </summary>
        STAR = 0x4,
        /// <summary>
        /// 起始进近
        /// </summary>
        InitialApproach = 0x8,
        /// <summary>
        /// 中间进近
        /// </summary>
        MiddleApproach = 0x10,
        /// <summary>
        /// 最后进近
        /// </summary>
        FinalApproach = 0x20,
        /// <summary>
        /// 复飞
        /// </summary>
        MissedApproach = 0x40,
        /// <summary>
        /// 离场
        /// </summary>
        SID = 0x80,
        /// <summary>
        /// 所有阶段
        /// </summary>
        All = Ocean | EnRoute | STAR | InitialApproach | MiddleApproach | FinalApproach | MissedApproach | SID,
        /// <summary>
        /// 进近阶段
        /// </summary>
        Approach = InitialApproach | MiddleApproach | FinalApproach | MissedApproach,
        /// <summary>
        /// 终端区阶段
        /// </summary>
        Termianl = STAR | Approach | SID
    }
}
