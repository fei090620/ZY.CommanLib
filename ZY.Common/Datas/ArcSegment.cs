using ZY.Common.Tools;
using ZY.Common.Types;
using System;
using System.Collections.Generic;

namespace ZY.Common.Datas
{
    [Serializable]
    /// <summary>
    /// 圆弧段类型
    /// </summary>
    public class ArcSegment : CurveSegment
    {
        #region  Properties
        /// <summary>
        /// 圆心
        /// </summary>
        public Point3D Center { get; set; }

        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// 圆弧方向
        /// </summary>
        public ArcDirctionType ArcDirection { get; set; }

        /// <summary>
        /// 起始弧度
        /// </summary>
        public double BeginRadian { get; set; }

        /// <summary>
        /// 结束弧度
        /// </summary>
        public double EndRadian { get; set; }

        /// <summary>
        /// 圆弧起始点
        /// </summary>
        public Point3D BeginPoint { get; set; }

        /// <summary>
        /// 圆弧结束点
        /// </summary>
        public Point3D EndPoint { get; set; }
        #endregion

        #region Construct
        public ArcSegment()
        {
            Center = new Point3D() { X = 0.0d, Y = 0.0d, Z = 0.0d };
            BeginPoint = new Point3D() { X = 0.0d, Y = 0.0d, Z = 0.0d };
            EndPoint = new Point3D() { X = 0.0d, Y = 0.0d, Z = 0.0d };
            Radius = 0.0d;
            BeginRadian = 0.0d;
            EndRadian = 0.0d;
            ArcDirection = ArcDirctionType.CLOCK_WISE;
        }

        /// <summary>
        /// 圆弧的构造
        /// </summary>
        /// <param name="center">圆心</param>
        /// <param name="radius">半径</param>
        /// <param name="beginRadian">开始弧度</param>
        /// <param name="endRadian">结束弧度</param>
        /// <param name="arcDir">方向,默认顺时针</param>
        public ArcSegment(Point3D center,
                          double radius,
                          ArcDirctionType arcDir = ArcDirctionType.CLOCK_WISE,
                          double beginRadian = 0,
                          double endRadian = 2 * Math.PI)
        {
            Center = center as Point3D;
            Radius = radius;
            BeginRadian = (beginRadian);
            EndRadian = (endRadian);
            BeginPoint = Center.GetNextPoint(beginRadian, Radius);
            EndPoint = Center.GetNextPoint(endRadian, Radius);
            ArcDirection = arcDir;
        }

        /// <summary>
        /// 圆弧的构造
        /// </summary>
        /// <param name="center">圆心</param>
        /// <param name="beginPoint">开始点坐标</param>
        /// <param name="endPoint">结束点坐标</param>
        /// <param name="arcDir">方向,默认顺时针</param>
        public ArcSegment(Point3D center,
                          Point3D beginPoint,
                          Point3D endPoint,
                          ArcDirctionType arcDir = ArcDirctionType.CLOCK_WISE)
        {
            Center = center as Point3D;
            BeginPoint = beginPoint as Point3D;
            EndPoint = endPoint as Point3D;
            Radius = center.DisTo(beginPoint);
            BeginRadian = Center.DirTo(beginPoint);
            EndRadian = Center.DirTo(endPoint);
            ArcDirection = arcDir;
        }
        #endregion

        #region SetProperties
        /// <summary>
        /// 设置圆弧开始点
        /// </summary>
        /// <param name="beginPoint"></param>
        public void SetBeginPoint(Point3D beginPoint)
        {
            if (Math.Abs(Center.DisTo(beginPoint) - Radius) >= OverrallVraTool.DoublePrecision)
                throw new ArgumentOutOfRangeException("在设置圆弧的起始（ArcSegment.SetBeginPoint）点时，参数beginPoint不在原有的圆弧上");

            BeginPoint = beginPoint;
            BeginRadian = Center.DirTo(beginPoint);
        }

        /// <summary>
        /// 设置圆弧结束点
        /// </summary>
        /// <param name="endPoint"></param>
        public void SetEndPoint(Point3D endPoint)
        {
            if (Math.Abs(Center.DisTo(endPoint) - Radius) >= OverrallVraTool.DoublePrecision)
                throw new ArgumentOutOfRangeException("在设置圆弧的起始（ArcSegment.SetEndPoint）点时，参数endPoint不在原有的圆弧上");

            EndPoint = endPoint;
            EndRadian = Center.DirTo(endPoint);
        }

        /// <summary>
        /// 设置圆弧起始方向
        /// </summary>
        /// <param name="beginRadian"></param>
        public void SetBeginRadian(double beginRadian)
        {
            BeginRadian = beginRadian;
            BeginPoint = Center.GetNextPoint(beginRadian, Radius);
        }

        /// <summary>
        /// 设置圆弧结束方向
        /// </summary>
        /// <param name="endRadian"></param>
        public void SetEndRadian(double endRadian)
        {
            EndRadian = endRadian;
            EndPoint = Center.GetNextPoint(endRadian, Radius);
        }

        /// <summary>
        /// 设置圆弧方向
        /// </summary>
        /// <param name="arcSegmentDirection"></param>
        public void SetArcSegmentDirection(ArcDirctionType arcSegmentDirection)
        {
            ArcDirection = arcSegmentDirection;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 取散点
        /// </summary>
        /// <param name="precision">角度经度：°</param>
        /// <returns></returns>
        public override IReadOnlyCollection<Point3D> ToList(double precision = 0.01)
        {
            double arcAngle = GetArcTurnAngle();
            if (double.IsNaN(arcAngle))
                throw new ArgumentNullException("在圆弧取点列（ArcSegment.ToList）时，计算圆弧的角度为NaN");

            List<Point3D> points = new List<Point3D>();
            points.Add(this.BeginPoint);
            if (!points.Exists(x => x.Equals(this.EndPoint)))
            {
                points.Add(this.EndPoint);
            }
            precision = Math.Abs(precision);
            int pointCount = (int)(arcAngle / precision);
            for (int i = 1; i <= pointCount; i++)
            {
                Point3D p = Center.GetNextPoint(BeginRadian +
                    ((ArcDirection == ArcDirctionType.CLOCK_WISE ? 1 : -1) * i * precision), Radius);
                if (!points.Exists(x => x.Equals(p)))
                {
                    points.Add(p);
                }
            }
            return points;
        }

        /// <summary>
        /// 获取圆弧的圆心角都（弧度）
        /// </summary>
        /// <returns></returns>
        public double GetArcTurnAngle()
        {
            Point3D centerToBeginVector = this.Center.Vector(BeginPoint);
            Point3D centerToEndVector = this.Center.Vector(EndPoint);
            double includeAngle = centerToBeginVector.IncludedAngle(centerToEndVector);
            TurnDirectionType turnDirection = new LineSegment(this.Center, this.BeginPoint).GetTurnDirection(this.EndPoint);
            includeAngle = ((turnDirection == TurnDirectionType.Left && this.ArcDirection == ArcDirctionType.UNCLOCK_WISE)
                || (turnDirection == TurnDirectionType.Right && this.ArcDirection == ArcDirctionType.CLOCK_WISE) ? includeAngle : 2 * Math.PI - includeAngle);

            return includeAngle;
        }

        /// <summary>
        /// 根据圆外一点计算圆弧切点
        /// </summary>
        /// <param name="outPoint"></param>
        /// <param name="turnDirection"></param>
        /// <returns></returns>
        public Point3D GetTangentPointbyOutPoint(Point3D outPoint, TurnDirectionType turnDirection)
        {
            //List<Point3D> list = GetIntersectPointsWithLine(outPoint, this.Center);
            //if (list == null || list.Count <= 0)
            //{
            //    return null;
            //}

            double outPointToCenterDistance = Center.DisTo(outPoint);
            if (Math.Abs(outPointToCenterDistance - Radius) < OverrallVraTool.DoublePrecision) //近似相等
            {
                return outPoint;
            }

            if (outPointToCenterDistance < Radius) //点在圆内无切点
            {
                return null;
            }
            
            double pointToTangentPointDistance = Math.Sqrt(Math.Pow(outPointToCenterDistance, 2) - Math.Pow(Radius, 2));
            double tangentLineDirection = Center.DirTo(outPoint)
                + Math.Atan(pointToTangentPointDistance / Radius)
                * (turnDirection == TurnDirectionType.Left ? 1 : -1);

            Point3D p = Center.GetNextPoint(tangentLineDirection, Radius);
            if (this.GetRelationShipWithPoint(p) == PointArcRelationShipType.On_ArcSegment)
            {
                return p;
            }
            return null;
        }

        /// <summary>
        /// 计算直线与圆弧交点
        /// </summary>
        /// <param name="ptStart"></param>
        /// <param name="ptEnd"></param>
        /// <returns></returns>
        public List<Point3D> GetIntersectPointsWithLine(Point3D ptStart, Point3D ptEnd)
        {
            LineSegment line = new LineSegment(ptStart, ptEnd);
            Point3D pedal = new Point3D();
            double centerToLineDistance = line.ToDis(Center, out pedal);
            if (centerToLineDistance - Radius > 0) //相离
            {
                return null;
            }
            else if (Math.Abs(centerToLineDistance - Radius) < OverrallVraTool.DoublePrecision) //相切
            {
                return new List<Point3D>() { pedal };
            }

            double dDisTemp = Math.Sqrt(Math.Pow(Radius, 2) - Math.Pow(centerToLineDistance, 2));
            Point3D insertPoint1 = pedal.GetNextPoint(line.GetDirection(), dDisTemp);
            Point3D insertPoint2 = pedal.GetNextPoint(line.GetDirection() + Math.PI, dDisTemp);
            List<Point3D> insertPoints = new List<Point3D>();
            if (this.GetRelationShipWithPoint(insertPoint1) == PointArcRelationShipType.On_ArcSegment)
                insertPoints.Add(insertPoint1);

            if (this.GetRelationShipWithPoint(insertPoint2) == PointArcRelationShipType.On_ArcSegment)
                insertPoints.Add(insertPoint2);

            return insertPoints;
        }

        public List<Point3D> GetIntersectPointsWithLine(LineSegment line)
        {
            if (line != null && line.BeginPoint != null && line.EndPoint != null)
            {
                return GetIntersectPointsWithLine(line.BeginPoint, line.EndPoint);
            }
            return null;
        }

        /// <summary>
        /// 计算给定直线段所在直线与圆弧的位置关系
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public LineArcRelationShipType GetRelationShipwithLine(LineSegment line)
        {
            Point3D tangentPoint = new Point3D();
            double dis = line.ToDis(Center, out tangentPoint);
            if (dis - Radius > 0)
            {
                return LineArcRelationShipType.Apart; //相离
            }
            else if (Math.Abs(dis - Radius) <= OverrallVraTool.DoublePrecision)  //相切
            {
                if (this.GetRelationShipWithPoint(tangentPoint) == PointArcRelationShipType.On_ArcSegment)
                {
                    return LineArcRelationShipType.Tangent;
                }
                else
                {
                    return LineArcRelationShipType.TangentWithArc;
                }
            }
            else  //相交
            {
                List<Point3D> list = GetIntersectPointsWithLine(line.BeginPoint, line.EndPoint);//线段与圆弧交点集合

                if (list != null && list.Count == 2)
                {
                    return LineArcRelationShipType.Insert;
                }
                else if (list == null || (list != null && list.Count == 0))
                {
                    return LineArcRelationShipType.InsertWithArc;
                }
                else if (list != null && list.Count == 1)
                {
                    return LineArcRelationShipType.InsertWithArcSegmentAndArc;
                }
                else
                {
                    return LineArcRelationShipType.Others;
                }
            }
        }

        /// <summary>
        /// 计算给定点与圆弧的位置关系
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public PointArcRelationShipType GetRelationShipWithPoint(Point3D point)
        {
            LineSegment startLine = new LineSegment(this.Center, this.BeginPoint);
            double dirStart = startLine.GetDirection();

            LineSegment endLine = new LineSegment(this.Center, this.EndPoint);
            double dirEnd = endLine.GetDirection();
            double dirEndWithStart = dirEnd - dirStart <= -Math.PI ? dirEnd - dirStart + 2 * Math.PI : dirEnd - dirStart;
            dirEndWithStart = Math.Round(dirEndWithStart, OverrallVraTool.DoublePrecision.ToString().Length - 2 + 1);

            LineSegment centerLine = new LineSegment(this.Center, point);
            double dirCenter = centerLine.GetDirection();
            double dirCenterWithStart = dirCenter - dirStart <= -Math.PI ? dirCenter - dirStart + 2 * Math.PI : dirCenter - dirStart;
            dirCenterWithStart = Math.Round(dirCenterWithStart, OverrallVraTool.DoublePrecision.ToString().Length - 2 + 1);

            bool inArcArea = false;
            if (this.BeginPoint.Equals(this.EndPoint))
            {
                inArcArea = true;
            }
            else
            {
                if (this.ArcDirection == ArcDirctionType.CLOCK_WISE)
                {
                    if (dirEndWithStart >= 0)
                    {
                        if (dirCenterWithStart >= 0 && dirCenterWithStart <= dirEndWithStart)
                        {
                            inArcArea = true;
                        }
                    }
                    else
                    {
                        if (dirCenterWithStart >= 0 || dirCenterWithStart <= dirEndWithStart)
                        {
                            inArcArea = true;
                        }
                    }
                }
                else
                {
                    if (dirEndWithStart <= 0)
                    {
                        if (dirCenterWithStart <= 0 && dirCenterWithStart >= dirEndWithStart)
                        {
                            inArcArea = true;
                        }
                    }
                    else
                    {
                        if (dirCenterWithStart <= 0 || dirCenterWithStart >= dirEndWithStart)
                        {
                            inArcArea = true;
                        }
                    }
                }
            }

            if (Math.Abs(Center.DisTo(point) - Radius) <= OverrallVraTool.DoublePrecision)
            {
                if (inArcArea == true)
                {
                    return PointArcRelationShipType.On_ArcSegment; //点在圆弧上
                }
                else
                {
                    return PointArcRelationShipType.On_Arc; //点不在圆弧上，但在圆弧所在的圆上
                }
            }

            if (Center.DisTo(point) <= Radius)
            {
                if (inArcArea == true)
                {
                    return PointArcRelationShipType.In_ArcSegment_Center;
                }
                else
                {
                    return PointArcRelationShipType.In_Arc;
                }
            }

            if (Center.DisTo(point) > Radius)
            {
                if (inArcArea == true)
                {
                    return PointArcRelationShipType.To_ArcSegment_Center;
                }
                else
                {
                    return PointArcRelationShipType.Back_ArcSegment_Center;
                }
            }

            return PointArcRelationShipType.OTHER;
        }

        /// <summary>
        /// 计算圆与圆的位置关系(圆弧所在的圆的位置关系)
        /// </summary>
        /// <param name="arcSegment"></param>
        /// <returns></returns>
        public ArcSegmentArcRelationShipType GetRelationShipWithArc(ArcSegment arcSegment)
        {
            Point3D center1 = this.Center;
            Point3D center2 = arcSegment.Center;
            double dis = center1.DisTo(center2);

            double r1 = this.Radius;
            double r2 = arcSegment.Radius;

            if (dis > r1 + r2)
            {
                return ArcSegmentArcRelationShipType.Apart;//相离
            }
            else if (dis == r1 + r2)
            {
                return ArcSegmentArcRelationShipType.OuterTangent;//外切
            }
            else if (dis == Math.Abs(r1 - r2))
            {
                return ArcSegmentArcRelationShipType.InnerTangent;//内切
            }
            else if (dis > Math.Abs(r1 - r2) && dis < r1 + r2)
            {
                return ArcSegmentArcRelationShipType.Insert;
            }
            else
            {
                return ArcSegmentArcRelationShipType.Others;
            }
        }

        /// <summary>
        /// 用给定点（必须在圆弧上，否则返回圆弧自身）截取圆弧
        /// </summary>
        /// <param name="point"></param>
        /// <param name="bCuteEnd">返回截取后得第一段还是第二段圆弧（true：第一段）</param>
        /// <returns></returns>
        public ArcSegment CutebyPoint(Point3D point, bool bCuteEnd)
        {
            PointArcRelationShipType ship = this.GetRelationShipWithPoint(point);

            if (ship == PointArcRelationShipType.On_ArcSegment)
            {
                if (bCuteEnd == true)
                {
                    return new ArcSegment(this.Center, this.BeginPoint, point, this.ArcDirection);
                }
                else
                {
                    return new ArcSegment(this.Center, point, this.EndPoint, this.ArcDirection);
                }
            }
            else
            {
                return this;
            }

            #region Old Method
            //if (ship != PointArcRelationShipType.In_ArcSegment_Center  //点在圆弧与圆心之间
            //    && ship != PointArcRelationShipType.To_ArcSegment_Center  //点在圆弧所在方位
            //    && ship != PointArcRelationShipType.On_ArcSegment)   //点在圆弧上
            //{
            //    return this;
            //}

            //return new ArcSegment(Center, Radius, ArcDirection, bCuteEnd ? BeginRadian : Center.DirTo(point),
            //    bCuteEnd ? Center.DirTo(point) : EndRadian);
            #endregion
        }

        /// <summary>
        /// 给定方向计算圆弧切线
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns> 
        public List<LineSegment> GetTangentPointbyDir(double dir)
        {
            List<LineSegment> list = new List<LineSegment>();

            LineSegment line = new LineSegment(this.Center, 1000, dir);
            line.Move(Math.PI / 2 + dir, this.Radius);
            LineArcRelationShipType type = GetRelationShipwithLine(line);
            if (type == LineArcRelationShipType.Tangent) //直线为圆弧的切线的情况下
            {
                list.Add(line);
            }

            LineSegment line2 = new LineSegment(this.Center, 1000, dir);
            line2.Move(-Math.PI / 2 + dir, this.Radius);  //相反方向移动
            LineArcRelationShipType type2 = GetRelationShipwithLine(line2);
            if (type2 == LineArcRelationShipType.Tangent) //直线为圆弧的切线的情况下
            {
                list.Add(line2);
            }
            return list;
        }

        /// <summary>
        /// 给定直线段，计算圆弧和直线段所在直线间的公切圆，并返回满足指定半径的圆弧
        /// </summary>
        /// <param name="line"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public List<ArcSegment> GetCommonTangentArcsbyArcandLine(LineSegment line, double radius)
        {
            List<ArcSegment> list = new List<ArcSegment>();
            List<Point3D> cacheList = new List<Point3D>();

            double dir = line.GetDirection();
            LineSegment line1 = line.CopyMove(dir - Math.PI / 2, radius) as LineSegment;  //公切圆圆心所在的直线1
            LineSegment line2 = line.CopyMove(dir + Math.PI / 2, radius) as LineSegment;  //公切圆圆心所在的直线2
            var r = this.Radius + radius;
            var r2 = Math.Abs(this.Radius - radius);
            //构造假想圆弧
            ArcSegment arc = new ArcSegment(this.Center, r);
            ArcSegment arc2 = new ArcSegment(this.Center, r2);

            //1.如果圆弧与直线相切，最多存在四个所求圆
            //2.如果圆弧与直线相交，最多存在八个所求圆
            //3.如果圆弧与直线相离，可能存在两个所求圆
            LineArcRelationShipType lineArcRelationShipType = GetRelationShipwithLine(line);
            if (lineArcRelationShipType == LineArcRelationShipType.Tangent ||
                lineArcRelationShipType == LineArcRelationShipType.TangentWithArc)
            {
                var list1 = arc.GetIntersectPointsWithLine(line1);
                if (list1 != null && list1.Count > 0)
                {
                    cacheList.AddRange(list1);
                }
                var list2 = arc2.GetIntersectPointsWithLine(line1);
                if (list2 != null && list2.Count > 0)
                {
                    cacheList.AddRange(list2);
                }
                var list3 = arc.GetIntersectPointsWithLine(line2);
                if (list3 != null && list3.Count > 0)
                {
                    cacheList.AddRange(list3);
                }
                var list4 = arc2.GetIntersectPointsWithLine(line2);
                if (list4 != null && list4.Count > 0)
                {
                    cacheList.AddRange(list4);
                }
            }
            else if (lineArcRelationShipType == LineArcRelationShipType.Insert ||
                lineArcRelationShipType == LineArcRelationShipType.InsertWithArc ||
                lineArcRelationShipType == LineArcRelationShipType.InsertWithArcSegmentAndArc)
            {
                var list1 = arc.GetIntersectPointsWithLine(line1);
                if (list1 != null && list1.Count > 0)
                {
                    cacheList.AddRange(list1);
                }
                var list2 = arc2.GetIntersectPointsWithLine(line1);
                if (list2 != null && list2.Count > 0)
                {
                    cacheList.AddRange(list2);
                }
                var list3 = arc.GetIntersectPointsWithLine(line2);
                if (list3 != null && list3.Count > 0)
                {
                    cacheList.AddRange(list3);
                }
                var list4 = arc2.GetIntersectPointsWithLine(line2);
                if (list4 != null && list4.Count > 0)
                {
                    cacheList.AddRange(list4);
                }
            }
            else if (lineArcRelationShipType == LineArcRelationShipType.Apart)
            {
                var list1 = arc.GetIntersectPointsWithLine(line1);
                if (list1 != null && list1.Count > 0)
                {
                    cacheList.AddRange(list1);
                }
                var list2 = arc.GetIntersectPointsWithLine(line2);
                if (list2 != null && list2.Count > 0)
                {
                    cacheList.AddRange(list2);
                }
            }
            else if (lineArcRelationShipType == LineArcRelationShipType.Others)
            {
                return null;
            }

            foreach (Point3D p in cacheList)
            {
                PointArcRelationShipType t = GetRelationShipWithPoint(p);
                if (t == PointArcRelationShipType.On_ArcSegment ||
                    t == PointArcRelationShipType.In_ArcSegment_Center ||
                    t == PointArcRelationShipType.To_ArcSegment_Center)
                {
                    list.Add(new ArcSegment(p, radius));
                }
            }

            return list;
        }

        /// <summary>
        /// 给定圆弧，计算圆弧与圆弧之间的公切线段集合
        /// </summary>
        /// <param name="rightArcSegment"></param>
        /// <returns></returns>
        public List<LineSegment> GetCommonTangentLinebyArcs(ArcSegment other)
        {
            ArcSegment leftArcSegment = null;
            ArcSegment rightArcSegment = null;
            if (this.Center.X < other.Center.X)
            {
                leftArcSegment = this;
                rightArcSegment = other;
            }
            else
            {
                leftArcSegment = other;
                rightArcSegment = this;
            }

            ArcSegmentArcRelationShipType type = leftArcSegment.GetRelationShipWithArc(rightArcSegment);
            //相交,外切，相离会用到两条
            if (type == ArcSegmentArcRelationShipType.Insert || type == ArcSegmentArcRelationShipType.Apart || type == ArcSegmentArcRelationShipType.OuterTangent)
            {
                double r1 = leftArcSegment.Radius;
                double r2 = rightArcSegment.Radius;
                double dis = leftArcSegment.Center.DisTo(rightArcSegment.Center);
                double x = Math.Abs(dis * r1 / (r2 - r1));
                double angle = Math.Asin(r1 / x);
                double dir0 = new LineSegment(leftArcSegment.Center, rightArcSegment.Center).GetDirection() - Math.PI / 2;

                LineSegment lineRadius = null;
                LineSegment lineRadius2 = null;
                if (r1 > r2)
                {
                    lineRadius = new LineSegment(leftArcSegment.Center, rightArcSegment.Center);
                    lineRadius.Rotate(leftArcSegment.Center, angle, ArcDirctionType.CLOCK_WISE);
                    lineRadius.Move(dir0 + angle, r1);

                    lineRadius2 = new LineSegment(leftArcSegment.Center, rightArcSegment.Center);
                    lineRadius2.Rotate(leftArcSegment.Center, angle, ArcDirctionType.UNCLOCK_WISE);
                    lineRadius2.Move(dir0 + Math.PI - angle, r1);
                }
                else
                {
                    lineRadius = new LineSegment(leftArcSegment.Center, rightArcSegment.Center);
                    lineRadius.Rotate(leftArcSegment.Center, angle, ArcDirctionType.UNCLOCK_WISE);
                    lineRadius.Move(dir0 - angle, r1);

                    lineRadius2 = new LineSegment(leftArcSegment.Center, rightArcSegment.Center);
                    lineRadius2.Rotate(leftArcSegment.Center, angle, ArcDirctionType.CLOCK_WISE);
                    lineRadius2.Move(dir0 - Math.PI + angle, r1);
                }

                List<LineSegment> list = new List<LineSegment>();

                if (leftArcSegment.GetRelationShipwithLine(lineRadius) == LineArcRelationShipType.Tangent &&
                    rightArcSegment.GetRelationShipwithLine(lineRadius) == LineArcRelationShipType.Tangent)
                {
                    list.Add(lineRadius);
                }

                if (leftArcSegment.GetRelationShipwithLine(lineRadius2) == LineArcRelationShipType.Tangent &&
                   rightArcSegment.GetRelationShipwithLine(lineRadius2) == LineArcRelationShipType.Tangent)
                {
                    list.Add(lineRadius2);
                }
                return list;
            }

            return null;
        }

        /// <summary>
        /// 翻转圆弧段（起点=>末尾点,末尾点=>起点，方向更改）
        /// </summary>
        /// <returns></returns>
        public override CurveSegment Reverse()
        {
            ArcDirctionType reverseArcDirection =
                ArcDirection == ArcDirctionType.CLOCK_WISE ? ArcDirctionType.UNCLOCK_WISE
                : ArcDirection == ArcDirctionType.UNCLOCK_WISE ? ArcDirctionType.CLOCK_WISE
                : ArcDirctionType.UNKONW;

            return new ArcSegment(Center, EndPoint, BeginPoint, reverseArcDirection);
        }

        /// <summary>
        /// 根据指定方向和距离平移圆弧
        /// </summary>
        /// <param name="radian"></param>
        /// <param name="distance"></param>
        public override void Move(double radian, double distance)
        {
            this.Center = this.Center.GetNextPoint(radian, distance);
            SetBeginPoint(BeginPoint.GetNextPoint(radian, distance));
            SetEndPoint(EndPoint.GetNextPoint(radian, distance));
        }

        /// <summary>
        ///  根据指定基准点，旋转角度和旋转方向旋转圆弧
        /// </summary>
        /// <param name="referencePos"></param>
        /// <param name="radius"></param>
        /// <param name="rotateDirection"></param>
        public override void Rotate(Point3D referencePos, double radius, ArcDirctionType rotateDirection)
        {
            this.Center = RotatePoint(this.Center, referencePos, radius, rotateDirection);
            this.BeginPoint = RotatePoint(this.BeginPoint, referencePos, radius, rotateDirection);
            this.EndPoint = RotatePoint(this.EndPoint, referencePos, radius, rotateDirection);
        }

        /// <summary>
        /// 坐标点绕某点旋转radian
        /// </summary>
        /// <param name="point"></param>
        /// <param name="referencePos"></param>
        /// <param name="radius"></param>
        /// <param name="rotateDirection"></param>
        /// <returns></returns>
        private Point3D RotatePoint(Point3D point, Point3D referencePos, double radius, ArcDirctionType rotateDirection)
        {
            radius = Math.Abs(radius);
            if (rotateDirection == ArcDirctionType.UNKONW)
            {
                throw new Exception("ArcDirctionType is wrong.");
            }
            else if (rotateDirection == ArcDirctionType.CLOCK_WISE)
            {
                radius = -radius;
            }
            //假设对图片上任意点(x,y)，绕一个坐标点(rx0,ry0)逆时针旋转a角度后的新的坐标设为(x0, y0)，有公式：
            //x0 = (x - rx0) * cos(a) - (y - ry0) * sin(a) + rx0;
            //y0 = (x - rx0) * sin(a) + (y - ry0) * cos(a) + ry0;
            Point3D returnPoint = new Point3D();
            returnPoint.X = (point.X - referencePos.X) * Math.Cos(radius) - (point.Y - referencePos.Y) * Math.Sin(radius) + referencePos.X;
            returnPoint.Y = (point.X - referencePos.X) * Math.Sin(radius) + (point.Y - referencePos.Y) * Math.Cos(radius) + referencePos.Y;
            returnPoint.Z = point.Z;
            return returnPoint;
        }

        /// <summary>
        /// 根据圆弧上的一点把圆弧截为两个子圆弧
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override List<CurveSegment> CuteOutbyPoint(Point3D point)
        {
            PointArcRelationShipType ship = this.GetRelationShipWithPoint(point);
            if (ship == PointArcRelationShipType.On_ArcSegment)
            {
                List<CurveSegment> list = new List<CurveSegment>();
                list.Add(new ArcSegment(this.Center, this.BeginPoint, point, this.ArcDirection));
                list.Add(new ArcSegment(this.Center, point, this.EndPoint, this.ArcDirection));
                return list;
            }
            else
            {
                return new List<CurveSegment>() { this };
            }
        }

        /// <summary>
        /// 计算传入的射线和当前圆弧位置关系
        /// </summary>
        /// <param name="line">传入的射线</param>
        /// <returns></returns>
        public override LineCurveRelationShipType GetCurveCurveRelationShipType(LineSegment line)
        {
            LineArcRelationShipType type = GetRelationShipwithLine(line);
            var dir = line.GetTurnDirection(this.Center);
            if (type == LineArcRelationShipType.Apart) //相离
            {
                if (dir == TurnDirectionType.Left)
                {
                    return LineCurveRelationShipType.Left;
                }
                else
                {
                    return LineCurveRelationShipType.Right;
                }
            }
            else if (type == LineArcRelationShipType.Insert) //和圆弧相交
            {
                if (this.BeginPoint.Equals(this.EndPoint))
                {
                    return LineCurveRelationShipType.Intersect;
                }
                else
                {
                    //1.半圆情况下，可能begin，end点为交点，则onleft或onright
                    /*
                     * （1）过begin，end点做直线
                     * （2）过直线中心做垂线，交圆弧于点p
                     * （3）判断点p和直线的位置关系
                     */
                    //2.交点存在非begin，end点，则为intersect
                    TurnDirectionType dirB = line.GetTurnDirection(this.BeginPoint);
                    TurnDirectionType dirE = line.GetTurnDirection(this.EndPoint);
                    if (dirB == TurnDirectionType.Stright && dirE == TurnDirectionType.Stright)
                    {
                        Point3D lineCenterPoint = new Point3D() { X = (this.BeginPoint.X + this.EndPoint.X) / 2, Y = (this.BeginPoint.Y + this.EndPoint.Y) / 2, Z = (this.BeginPoint.Z + this.EndPoint.Z) / 2 };

                        LineSegment centerLine = new LineSegment(lineCenterPoint, 1, line.GetDirection() + Math.PI / 2);
                        List<Point3D> listPoint = this.GetIntersectPointsWithLine(centerLine.BeginPoint, centerLine.EndPoint);
                        if (listPoint != null && listPoint.Count > 0)
                        {
                            if (line.GetTurnDirection(listPoint[0]) == TurnDirectionType.Right)
                            {
                                return LineCurveRelationShipType.On_Right;
                            }
                            else if (line.GetTurnDirection(listPoint[0]) == TurnDirectionType.Left)
                            {
                                return LineCurveRelationShipType.On_Left;
                            }
                        }
                    }
                    else
                    {
                        return LineCurveRelationShipType.Intersect;
                    }
                }
            }
            else if (type == LineArcRelationShipType.InsertWithArc) //和圆弧所在的圆相交
            {
                if (this.BeginPoint.Equals(this.EndPoint))
                {
                }
                else
                {
                    //只有半圆存在该情况
                    if (line.GetTurnDirection(this.BeginPoint) == TurnDirectionType.Left)
                    {
                        return LineCurveRelationShipType.Left;
                    }
                    else
                    {
                        return LineCurveRelationShipType.Right;
                    }
                }
            }
            else if (type == LineArcRelationShipType.InsertWithArcSegmentAndArc) //和圆弧，圆弧所在的圆分别相交
            {
                if (this.BeginPoint.Equals(this.EndPoint))
                {
                }
                else
                {
                    //只有半圆存在该情况
                    TurnDirectionType dirB = line.GetTurnDirection(this.BeginPoint);
                    TurnDirectionType dirE = line.GetTurnDirection(this.EndPoint);

                    if ((dirB == TurnDirectionType.Right && dirE == TurnDirectionType.Left) ||
                        (dirB == TurnDirectionType.Left && dirE == TurnDirectionType.Right))
                    {
                        return LineCurveRelationShipType.Intersect;
                    }
                    else if ((dirB == TurnDirectionType.Right && dirE == TurnDirectionType.Stright) ||
                        (dirB == TurnDirectionType.Stright && dirE == TurnDirectionType.Right))
                    {
                        return LineCurveRelationShipType.On_Right;
                    }
                    else if ((dirB == TurnDirectionType.Left && dirE == TurnDirectionType.Stright) ||
                       (dirB == TurnDirectionType.Stright && dirE == TurnDirectionType.Left))
                    {
                        return LineCurveRelationShipType.On_Left;
                    }
                }
            }
            else if (type == LineArcRelationShipType.Tangent) //和圆弧相切
            {
                if (dir == TurnDirectionType.Left)
                {
                    return LineCurveRelationShipType.On_Left;
                }
                else
                {
                    return LineCurveRelationShipType.On_Right;
                }
            }
            else if (type == LineArcRelationShipType.TangentWithArc) //和圆弧所在的圆相切
            {
                if (this.BeginPoint.Equals(this.EndPoint))
                {
                }
                else
                {
                    //只有半圆存在该情况
                    if (dir == TurnDirectionType.Left)
                    {
                        return LineCurveRelationShipType.Left;
                    }
                    else
                    {
                        return LineCurveRelationShipType.Right;
                    }
                }
            }
            return LineCurveRelationShipType.Others;
        }

        /// <summary>
        /// 获取传入射线与圆弧的交点
        /// </summary>
        /// <param name="line">传入的射线</param>
        /// <returns></returns>
        public override IReadOnlyCollection<Point3D> GetCrossPoints(LineSegment line)
        {
            List<Point3D> list = GetIntersectPointsWithLine(line.GetStartPoint(), line.GetEndPoint()); //直线和圆弧交点的集合
            List<Point3D> returnList = new List<Point3D>();
            if (list != null)
            {
                foreach (Point3D point in list)
                {
                    var type = line.GetPointRelationShip(point);
                    if (type == PointLineRelationShipType.Point_On_Line || type == PointLineRelationShipType.Point_On_Line_EndDir) //点在射线上
                    {
                        if (this.GetRelationShipWithPoint(point) == PointArcRelationShipType.On_ArcSegment) //点在圆弧上
                        {
                            returnList.Add(point);
                        }
                    }
                }
            }
            return returnList;
        }

        public override double GetStartCourse()
        {
            return BeginRadian;
        }

        public override double GetEndCourse()
        {
            return EndRadian;
        }

        public override Point3D GetStartPoint()
        {
            return BeginPoint;
        }

        public override Point3D GetEndPoint()
        {
            return EndPoint;
        }

        /// <summary>
        /// 判断点是否在圆弧线段上
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool PointIsOnCurveSegment(Point3D point)
        {
            PointArcRelationShipType type = GetRelationShipWithPoint(point);
            if (type == PointArcRelationShipType.On_ArcSegment)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Override Object Methods
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || !GetType().IsEquivalentTo(obj.GetType()))
                return false;

            ArcSegment other = (ArcSegment)obj;
            return BeginPoint.Equals(other.BeginPoint)
                && EndPoint.Equals(other.EndPoint)
                && Center.Equals(other.Center)
                && ArcDirection.Equals(other.ArcDirection);
        }

        public override int GetHashCode()
        {
            const int hashIndex = 307;
            var result = (Center != null) ? Center.GetHashCode() : 0;
            result = (result * hashIndex) ^ ((BeginPoint != null) ? BeginPoint.GetHashCode() : 0);
            result = (result * hashIndex) ^ ((EndPoint != null) ? EndPoint.GetHashCode() : 0);
            result = (result * hashIndex) ^ ArcDirection.GetHashCode();

            return result;
        }

        public override string ToString()
        {
            return string.Format("{0};{1};{2};{3}", Center.ToString(), BeginPoint.ToString(), EndPoint.ToString(), ArcDirection.ToString());
        }
        #endregion
    }
}


