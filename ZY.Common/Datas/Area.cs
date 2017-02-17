using ZY.Common.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ZY.Common.Datas
{
    [Serializable]
    /// <summary>
    /// 区域（封闭图形：可以多边形或多边形与圆弧组成的封闭区域）
    /// </summary>
    public class Area
    {
        protected Area() { }

        /// <summary>
        /// 通用构造区域方法
        /// </summary>
        /// <param name="outterLine">外轮廓曲线，方向顺时针</param>
        /// <param name="innerLine">内轮廓曲线，方向逆时针</param>
        public static Area CreateNew(Curve outterLine, Curve innerLine = null)
        {
            Area area = new Area
            {
                OutterLine = outterLine,
                InnerLine = innerLine
            };

            return area;
        }

        /// <summary>
        /// 通过两条同向直线段构造
        /// （首点与首点连接，末尾点与末尾点链接）区域的方法（区域为矩形或平行四边形）
        /// </summary>
        /// <param name="boundary">一条边</param>
        /// <param name="otherBoundary">另一条边，在构造过程中被翻转方向</param>
        public static Area CreateNewBySameDirectionLines(LineSegment boundary, LineSegment otherBoundary)
        {
            LineSegment firstLine = boundary;
            LineSegment secondLine = new LineSegment(boundary.EndPoint, otherBoundary.EndPoint);
            LineSegment thirdLine = otherBoundary.Reverse() as LineSegment;
            LineSegment fourthLine = new LineSegment(otherBoundary.BeginPoint, boundary.BeginPoint);
            Curve outterLine = new Curve(new List<CurveSegment>() { firstLine, secondLine, thirdLine, fourthLine });

            return CreateNew(outterLine);
        }

        /// <summary>
        /// 通过两条反向直线段构造
        /// （首点与末尾点连接，末尾点与首点链接）区域的方法（区域为矩形或平行四边形）
        /// </summary>
        /// <param name="boundary">一条边</param>
        /// <param name="otherBoundary">另一条边</param>
        /// <returns></returns>
        public static Area CreateNewByInverseDirectionLines(LineSegment boundary, LineSegment otherBoundary)
        {
            LineSegment firstLine = boundary;
            LineSegment secondLine = new LineSegment(boundary.EndPoint, otherBoundary.BeginPoint);
            LineSegment thirdLine = otherBoundary;
            LineSegment fourthLine = new LineSegment(otherBoundary.EndPoint, boundary.BeginPoint);
            Curve outterLine = new Curve(new List<CurveSegment>() { firstLine, secondLine, thirdLine, fourthLine });

            return CreateNew(outterLine);
        }

        /// <summary>
        /// 从指定点集合中获取区域内的点集合
        /// </summary>
        /// <param name="points">待选点集合</param>
        /// <returns>区域中的点集合</returns>
        public IReadOnlyCollection<Point3D> GetIncludedPoints(List<Point3D> points)
        {
            Contract.Requires<ArgumentNullException>(points != null);
            return points.FindAll(x => { return IsInclude(x); });
        }

        /// <summary>
        /// 区域中是否包含指定点
        /// </summary>
        /// <param name="point">指定点</param>
        /// <returns>是否包含指定点</returns>
        public bool IsInclude(Point3D point)
        {
            bool outter = CurveIncludePointCheck(this.OutterLine, point, true);
            bool inner = CurveIncludePointCheck(this.InnerLine, point, false);
            if (outter == true && inner == false)
            {
                return true;
            }
            return false;
        }

        ///// <summary>
        ///// 判断点是否在密闭区域内
        ///// </summary>
        ///// <param name="curve">曲线（曲线段结合）</param>
        ///// <param name="point">待验证点</param>
        ///// <returns></returns>
        private bool CurveIncludePointCheck(Curve curve, Point3D point, bool includePointOnSegment)
        {
            if (curve == null)
            {
                return false;
            }
            int count = 0;
            LineSegment line = new LineSegment(point, 1, Math.PI / 2); //沿某一方向构造射线
            List<CurveSegment> list = curve.Tracks as List<CurveSegment>;
            foreach (CurveSegment item in list)
            {
                bool pointIsOnCurveSegment = item.PointIsOnCurveSegment(point);

                if (pointIsOnCurveSegment == true)
                {
                    if (includePointOnSegment == true)//点在线段上，是否包含在区域内
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var type = item.GetCurveCurveRelationShipType(line);
                    if (type == LineCurveRelationShipType.Left || type == LineCurveRelationShipType.Right) //无交点
                    {
                        continue;
                    }
                    else if (type == LineCurveRelationShipType.Coincide) //默认忽略（重合）
                    {
                        continue;
                    }
                    else if (type == LineCurveRelationShipType.On_Left) //默认忽略（左侧忽略）
                    {
                        continue;
                    }
                    else if (type == LineCurveRelationShipType.On_Right) //符合默认逻辑（右侧）
                    {

                        count += item.GetCrossPoints(line).Count;
                    }
                    else if (type == LineCurveRelationShipType.Intersect) //符合默认逻辑（相交）
                    {
                        count += item.GetCrossPoints(line).Count;
                    }
                }
            }

            if (count % 2 != 0) //奇数include，偶数反之
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 外轮廓线（方向顺时针）
        /// </summary>
        public Curve OutterLine { get; set; }

        /// <summary>
        /// 内轮廓线（默认为null，方向逆时针）
        /// </summary>
        public Curve InnerLine { get; set; }
    }
}
