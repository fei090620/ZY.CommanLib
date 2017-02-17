using ZY.Common.Tools;
using System.Collections.Generic;
using System.Linq;

namespace ZY.Common.Datas
{
    /// <summary>
    /// 曲线
    /// </summary>
    public class Curve
    {
        #region Properties
        public List<CurveSegment> Tracks { get; set; }
        #endregion

        #region Construct
        public Curve(IEnumerable<CurveSegment> tracks)
        {
            Tracks = tracks == null ? new List<CurveSegment>() : tracks.ToList();
        }

        public Curve()
        {
            Tracks = new List<CurveSegment>();
        }

        public Curve(CurveSegment curveSegment)
        {
            if (curveSegment == null)
                Tracks = new List<CurveSegment>();
            else
                Tracks = new List<CurveSegment> { curveSegment };
        }

        public Curve(IEnumerable<Point3D> points)
        {
            Tracks = new List<CurveSegment>();
            if (points == null || points.Count() <= 1) return; 
            for (int i = 0; i < points.Count() - 1; i++)
            {
                var line = new LineSegment(points.ElementAt(i), points.ElementAt(i + 1));
                Tracks.Add(line);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 根据精度取点列
        /// </summary>
        /// <returns></returns>
        public List<Point3D> ToList(double precision = 0.01)
        {
            List<Point3D> points = new List<Point3D>();
            foreach (var item in Tracks)
            {
                points.AddRange(item.ToList(precision));
            }

            return points;
        }

        /// <summary>
        /// 获取航段航迹的第一个直线段,没有返回null
        /// </summary>
        /// <returns></returns>
        public LineSegment GetFirstLineSegment()
        {
            LineSegment first = null;
            foreach (var item in this.Tracks)
            {
                if (item is LineSegment)
                {
                    return item as LineSegment;
                }
            }
            return first;
        }

        /// <summary>
        /// 获取航段航迹的第一个圆弧段，没有返回null
        /// </summary>
        /// <returns></returns>
        public ArcSegment GetFirstArcSegment()
        {
            ArcSegment first = null;
            foreach (var item in this.Tracks)
            {
                if (item is ArcSegment)
                {
                    return item as ArcSegment;
                }
            }
            return first;
        }

        /// <summary>
        /// 获取航段航迹的最后一个直线段，没有返回null
        /// </summary>
        /// <returns></returns>
        public LineSegment GetLastLineSegment()
        {
            LineSegment last = null;
            foreach (var item in this.Tracks)
            {
                if (item is LineSegment)
                {
                    last = item as LineSegment;
                }
            }
            return last;
        }

        /// <summary>
        /// 获取航段航迹的最后一个圆弧段，没有返回null
        /// </summary>
        /// <returns></returns>
        public ArcSegment GetLastArcSegment()
        {
            ArcSegment last = null;
            foreach (var item in this.Tracks)
            {
                if (item is ArcSegment)
                {
                    last = item as ArcSegment;
                }
            }
            return last;
        }

        /// <summary>
        /// 获取起始方向（直线段的方向or圆弧中心点到起点的方向）
        /// </summary>
        /// <returns></returns>
        public double? GetStartTangentRadian()
        {
            if (Tracks == null || Tracks.Count == 0)
                return null;

            var firstTrack = Tracks.First();
            if (firstTrack == null)
                return null;

            return firstTrack.GetStartCourse();
        }

        /// <summary>
        /// 获取结束方向（直线段的方向or圆弧中心点到起点的方向）
        /// </summary>
        /// <returns></returns>
        public double? GetEndTangentRadian()
        {
            if (Tracks == null || Tracks.Count == 0)
                return null;

            var lastTrack = Tracks.Last();
            if (lastTrack == null)
                return null;

            return lastTrack.GetEndCourse();
        }

        /// <summary>
        /// 在曲线的后端连接另一条曲线
        /// （如果末尾端点和另一条曲线的首点不是同一个点，则用直线段连接）
        /// </summary>
        /// <param name="other">另一条曲线</param>
        /// <returns>连接后的曲线</returns>
        public Curve ConnectBackWith(Curve other)
        {
            Point3D this_lastPoint = (this.Tracks as List<CurveSegment>)[this.Tracks.Count - 1].GetEndPoint();
            Point3D other_firstPoint = (other.Tracks as List<CurveSegment>)[0].GetStartPoint();

            if (this_lastPoint.Equals(other_firstPoint))
            {
                List<CurveSegment> list = this.Tracks as List<CurveSegment>;
                list.AddRange(other.Tracks);
                return new Curve(list);
            }
            else
            {
                List<CurveSegment> list = this.Tracks as List<CurveSegment>;
                LineSegment line = new LineSegment(this_lastPoint, other_firstPoint);
                list.Add(line);
                list.AddRange(other.Tracks);
                return new Curve(list);
            }
        }

        /// <summary>
        /// 在曲线的后端连接另一条曲线段
        /// （如果末尾端点和另一条曲线的首点不是同一个点，则用直线段连接）
        /// </summary>
        /// <param name="otherCurveSegment">另一条曲线段</param>
        /// <returns>连接后的曲线</returns>
        public Curve ConnectBackWith(CurveSegment otherCurveSegment)
        {
            Point3D this_lastPoint = (this.Tracks as List<CurveSegment>)[this.Tracks.Count - 1].GetEndPoint();
            Point3D other_firstPoint = otherCurveSegment.GetStartPoint();

            if (this_lastPoint.Equals(other_firstPoint))
            {
                List<CurveSegment> list = this.Tracks as List<CurveSegment>;
                list.Add(otherCurveSegment);
                return new Curve(list);
            }
            else
            {
                List<CurveSegment> list = this.Tracks as List<CurveSegment>;
                LineSegment line = new LineSegment(this_lastPoint, other_firstPoint);
                list.Add(line);
                list.Add(otherCurveSegment);
                return new Curve(list);
            }
        }

        /// <summary>
        /// 在曲线的前端连接另一条曲线
        /// （如果起始端点和另一条曲线的末尾点不是同一个点，则用直线段连接）
        /// </summary>
        /// <param name="other">另一条曲线</param>
        /// <returns>连接后的曲线</returns>
        public Curve ConnectFrontWith(Curve other)
        {
            Point3D this_firstPoint = (this.Tracks as List<CurveSegment>)[0].GetStartPoint();
            Point3D other_lastPoint = (other.Tracks as List<CurveSegment>)[other.Tracks.Count - 1].GetEndPoint();

            if (this_firstPoint.Equals(other_lastPoint))
            {
                List<CurveSegment> list = other.Tracks as List<CurveSegment>;
                list.AddRange(this.Tracks);
                return new Curve(list);
            }
            else
            {
                List<CurveSegment> list = other.Tracks as List<CurveSegment>;
                LineSegment line = new LineSegment(other_lastPoint, this_firstPoint);
                list.Add(line);
                list.AddRange(this.Tracks);
                return new Curve(list);
            }
        }

        /// <summary>
        /// 在曲线的前端连接另一条曲线段
        /// （如果起始端点和另一条曲线段的末尾点不是同一个点，则用直线段连接）
        /// </summary>
        /// <param name="otherCurveSegment">另一条曲线段</param>
        /// <returns>连接后的曲线</returns>
        public Curve ConnectFrontWith(CurveSegment otherCurveSegment)
        {
            Point3D this_firstPoint = (this.Tracks as List<CurveSegment>)[0].GetStartPoint();
            Point3D other_lastPoint = otherCurveSegment.GetEndPoint();

            if (this_firstPoint.Equals(other_lastPoint))
            {
                List<CurveSegment> list = new List<CurveSegment>() { otherCurveSegment };
                list.AddRange(this.Tracks);
                return new Curve(list);
            }
            else
            {
                List<CurveSegment> list = new List<CurveSegment>() { otherCurveSegment };
                LineSegment line = new LineSegment(other_lastPoint, this_firstPoint);
                list.Add(line);
                list.AddRange(this.Tracks);
                return new Curve(list);
            }
        }

        /// <summary>
        /// 翻转曲线（曲线段顺序翻转，曲线段方向翻转）
        /// </summary>
        /// <returns></returns>
        public Curve Reverse()
        {
            List<CurveSegment> tracks = CloneTool.DeepClone(Tracks, DeepCloneType.Serialize) as List<CurveSegment>;
            tracks.Reverse();
            for (int i = 0; i < tracks.Count; i++)
            {
                tracks[i] = tracks[i].Reverse();
            }
            //tracks.ForEach(x => x.Reverse());
            return new Curve(tracks);
        }
        #endregion
    }
}
