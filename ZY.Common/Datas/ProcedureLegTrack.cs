using ADCC.Common.Datas;
using System.Collections.Generic;

namespace ADCC.FPDAM.Domain.Models.Procedure
{
    /// <summary>
    /// 程序段航迹
    /// </summary>
    public class ProcedureLegTrack : DomainObject
    {
        public IReadOnlyCollection<ICurveSegment> Tracks { get; private set; }
        public static ProcedureLegTrack CreateNew(string id, IReadOnlyCollection<ICurveSegment> tracks)
        {
            ProcedureLegTrack track = new ProcedureLegTrack()
            {
                Identify = id,
                Tracks = tracks
            };

            return track;
        }

        /// <summary>
        /// 获取航段航迹的第一个直线段,没有返回null
        /// </summary>
        /// <returns></returns>
        public LineSegment GetFirstLineSegment()
        {
            return null;
        }

        /// <summary>
        /// 获取航段航迹的第一个圆弧段，没有返回null
        /// </summary>
        /// <returns></returns>
        public ArcSegment GetFirstArcSegment()
        {
            return null;
        }

        /// <summary>
        /// 获取航段航迹的最后一个直线段，没有返回null
        /// </summary>
        /// <returns></returns>
        public LineSegment GetLastLineSegment()
        {
            return null;
        }

        /// <summary>
        /// 获取航段航迹的最后一个圆弧段，没有返回null
        /// </summary>
        /// <returns></returns>
        public ArcSegment GetLastArcSegment()
        {
            return null;
        }
    }
}
