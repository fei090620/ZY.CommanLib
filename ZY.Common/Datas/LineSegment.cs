using ZY.Common.Tools;
using ZY.Common.Types;
using System;
using System.Collections.Generic;

namespace ZY.Common.Datas
{
    [Serializable]
    /// <summary>
    /// 线段
    /// </summary>
    public class LineSegment : CurveSegment
    {
        #region Properties
        /// <summary>
        /// 起始点
        /// </summary>
        public Point3D BeginPoint { get; set; }

        /// <summary>
        /// 末尾点
        /// </summary>
        public Point3D EndPoint { get; set; }

        /// <summary>
        /// 获取线段方向（弧度）
        /// </summary>
        /// <returns></returns>
        public double GetDirection()
        {
            if (BeginPoint == null)
                throw new ArgumentNullException("计算线段方向时，线段起始点出现空引用");

            return BeginPoint.DirTo(EndPoint);
        }

        /// <summary>
        /// 获取线段长度
        /// </summary>
        /// <returns></returns>
        public double GetLength()
        {
            if (BeginPoint == null)
                throw new ArgumentNullException("计算线段长度时，线段起始点出现空引用");

            return BeginPoint.DisTo(EndPoint);
        }
        #endregion

        #region Construct
        public LineSegment()
        {
            BeginPoint = new Point3D() { X = 0.0d, Y = 0.0d, Z = 0.0d };
            EndPoint = new Point3D() { X = 0.0d, Y = 0.0d, Z = 0.0d };
        }

        /// <summary>
        /// 已知起点和终点够造线段
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public LineSegment(Point3D startPoint, Point3D endPoint)
        {
            BeginPoint = startPoint;
            EndPoint = endPoint;
        }

        /// <summary>
        /// 已知起点，线段长度和方向构造线段
        /// </summary>
        /// <param name="beginPoint"></param>
        /// <param name="length"></param>
        /// <param name="dir"></param>
        public LineSegment(Point3D beginPoint, double length, double dir)
        {
            BeginPoint = beginPoint;
            EndPoint = beginPoint.GetNextPoint(dir, length);
        }
        #endregion

        #region SetProperties
        /// <summary>
        /// 设置起始点
        /// </summary>
        /// <param name="beginPoint"></param>
        public void SetBeginPoint(Point3D beginPoint)
        {
            BeginPoint = beginPoint;
        }

        /// <summary>
        /// 设置末尾点
        /// </summary>
        /// <param name="endPoint"></param>
        public void SetEndPoint(Point3D endPoint)
        {
            EndPoint = endPoint;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 将线段根据精度分解为点列
        /// </summary>
        /// <param name="presence"></param>
        /// <returns></returns>
        public override IReadOnlyCollection<Point3D> ToList(double presence = 0.01)
        {
            List<Point3D> points = new List<Point3D>() { };
            //if (presence <= 0
            //    || presence >= BeginPoint.DisTo(EndPoint))
            //    return points;

            //int numPoints = (int)(BeginPoint.DisTo(EndPoint) / presence);
            //for (int i = 0; i < numPoints; i++)
            //    points.Add(BeginPoint.GetNextPoint(GetDirection(), i * presence));

            //if (!points.FindLast((x) => { return x is Point3D; }).Equals(EndPoint))
            //    points.Add(EndPoint);
            points.Add(this.BeginPoint);
            points.Add(this.EndPoint);
            return points;
        }

        /// <summary>
        /// 翻转直线段（起点=>末尾点，末尾点=>起点）
        /// </summary>
        /// <returns></returns>
        public override CurveSegment Reverse()
        {
            return new LineSegment(EndPoint, BeginPoint);
        }

        /// <summary>
        /// 计算两条线段所在射线的交点
        /// </summary>
        /// <param name="otherLine"></param>
        /// <returns></returns>
        public Point3D GetLineDirIntersection(LineSegment otherLine)
        {
            Point3D crossPoint = GetIntersect(otherLine.BeginPoint, otherLine.EndPoint);
            if (crossPoint == null)
            {
                return null;
            }
            else if ((this.GetPointRelationShip(crossPoint) != PointLineRelationShipType.Point_On_Line
                && this.GetPointRelationShip(crossPoint) != PointLineRelationShipType.Point_On_Line_EndDir)
                || (otherLine.GetPointRelationShip(crossPoint) != PointLineRelationShipType.Point_On_Line
                && otherLine.GetPointRelationShip(crossPoint) != PointLineRelationShipType.Point_On_Line_EndDir))
            {
                return null;
            }

            return crossPoint;
        }

        /// <summary>
        /// 计算两条线段所在直线的交点
        /// </summary>
        /// <param name="otherLine"></param>
        /// <returns></returns>
        public Point3D GetArcDirToLineDirintersection(LineSegment otherLine)
        {
            Point3D crossPoint = GetIntersect(otherLine.BeginPoint, otherLine.EndPoint);
            if (crossPoint == null)
            {
                return null;
            }
            else if ((this.GetPointRelationShip(crossPoint) == PointLineRelationShipType.Point_Not_On_Line)
                || (otherLine.GetPointRelationShip(crossPoint) == PointLineRelationShipType.Point_Not_On_Line))
            {
                return null;
            }

            return crossPoint;
        }

        /// <summary>
        /// 计算两条线段方向向量的夹角
        /// </summary>
        /// <param name="otherLine"></param>
        /// <returns></returns>
        public double GetIncludeAngle(LineSegment otherLine)
        {
            Point3D v1 = (BeginPoint as Point3D).Vector(EndPoint);
            Point3D v2 = (otherLine.BeginPoint as Point3D).Vector(otherLine.EndPoint);
            return (v1 as Point3D).IncludedAngle(v2);
        }

        /// <summary>
        /// 延长直线段
        /// </summary>
        /// <param name="type">延长类型：0都延长，1延长尾点，-1延长首点</param>
        /// <param name="expandDis"></param>
        /// <returns></returns>
        public LineSegment Expand(int type, double expandDis)
        {
            switch (type)
            {
                case 0:
                    return new LineSegment(BeginPoint.GetNextPoint(GetDirection() + Math.PI, expandDis),
                        EndPoint.GetNextPoint(GetDirection(), expandDis));
                case -1:
                    return new LineSegment(BeginPoint.GetNextPoint(GetDirection() + Math.PI, expandDis), EndPoint);
                case 1:
                    return new LineSegment(BeginPoint, EndPoint.GetNextPoint(GetDirection(), expandDis));
                default:
                    break;
            }

            throw new ArgumentOutOfRangeException("扩展线段（LineSegment.Expand）时,出现了非法的延长类型");
        }

        /// <summary>
        /// 计算两条线段的交点
        /// </summary>
        /// <param name="otherLine"></param>
        /// <returns></returns>
        public Point3D GetIntersection(LineSegment otherLine)
        {
            Point3D crossPoint = GetIntersect(otherLine.BeginPoint, otherLine.EndPoint);
            if (crossPoint == null)
            {
                return null;
            }
            if (this.GetPointRelationShip(crossPoint) != PointLineRelationShipType.Point_On_Line
                || otherLine.GetPointRelationShip(crossPoint) != PointLineRelationShipType.Point_On_Line)
            {
                return null;
            }

            return crossPoint;
        }

        /// <summary>
        /// 计算当前线段与射线的交点
        /// </summary>
        /// <param name="Otherlinepoint"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Point3D GetIntersectionWithRay(Point3D Otherlinepoint, double direction)
        {
            if (Otherlinepoint != null)
            {
                Point3D OtherLineOtherpoint = Otherlinepoint.GetNextPoint(direction, 100000);
                LineSegment line = new LineSegment(Otherlinepoint, OtherLineOtherpoint); //射线
                List<Point3D> list = GetCrossPoints(line) as List<Point3D>;
                if (list != null && list.Count > 0)
                {
                    return list[0];
                }
            }
            return null;
        }

        /// <summary>
        /// 计算点到直线段所在直线的距离
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pedalPoint">垂足</param>
        /// <returns></returns>
        public double ToDis(Point3D point, out Point3D pedalPoint)
        {
            LineSegment otherLine = new LineSegment(point, 10, GetDirection() + Math.PI / 2).Expand(0, 10000);
            pedalPoint = this.GetIntersect(otherLine.BeginPoint, otherLine.EndPoint);
            if (pedalPoint != null)
            {
                return pedalPoint.DisTo(point);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 计算当前线段与指定点的位置关系
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public PointLineRelationShipType GetPointRelationShip(Point3D point)
        {
            if (point.Equals(BeginPoint) || point.Equals(EndPoint))
            {
                return PointLineRelationShipType.Point_On_Line;
            }
            Point3D vBegin = (point as Point3D).Vector(BeginPoint);
            Point3D vEnd = (point as Point3D).Vector(EndPoint);
            double dIncludeRadian = (vBegin as Point3D).IncludedAngle(vEnd);
            if (Math.Abs(dIncludeRadian - Math.PI) < OverrallVraTool.DoublePrecision)
                return PointLineRelationShipType.Point_On_Line;
            else if (Math.Abs(dIncludeRadian) < OverrallVraTool.DoublePrecision)
            {
                if (point.DisTo(BeginPoint) - point.DisTo(EndPoint) > OverrallVraTool.DoublePrecision)
                    return PointLineRelationShipType.Point_On_Line_EndDir;
                else
                    return PointLineRelationShipType.Point_On_Line_BeginDir;
            }
            else
                return PointLineRelationShipType.Point_Not_On_Line;
        }

        /// <summary>
        /// 计算线段所在直线的交点
        /// </summary>
        /// <param name="OtherlineStar"></param>
        /// <param name="OtherlineEnd"></param>
        /// <returns></returns>
        public Point3D GetIntersect(Point3D OtherlineStar, Point3D OtherlineEnd)
        {
            return GetIntersect(this.BeginPoint, this.EndPoint, OtherlineStar, OtherlineEnd);

            //Point3D startPoint = new Point3D(OtherlineStar);
            //Point3D endPoint = new Point3D(OtherlineEnd);
            //if (GetPointRelationShip(startPoint) == PointLineRelationShipType.Point_On_Line)
            //    return startPoint;

            //if (GetPointRelationShip(endPoint) == PointLineRelationShipType.Point_On_Line)
            //    return endPoint;

            //Point3D vNormalVector = (BeginPoint as Point3D).Vector(EndPoint);
            //Point3D vOtherNormalVector = (startPoint as Point3D).Vector(endPoint);
            //double vectorProduct = (vNormalVector as Point3D).VectorProduct(vOtherNormalVector);
            //if (Math.Abs(vectorProduct) <= OverrallVraTool.DoublePrecision)
            //    return null;

            //double distC_other = vOtherNormalVector.X * startPoint.X + vOtherNormalVector.Y * startPoint.Y;
            //double distA_other = vOtherNormalVector.X * BeginPoint.X + vOtherNormalVector.Y * BeginPoint.Y - distC_other;

            //double fraction = distA_other / vectorProduct;
            //double dx = fraction * vNormalVector.Y;
            //double dy = fraction * (-1) * vNormalVector.X;
            //return new Point3D(BeginPoint.X + dx, BeginPoint.Y + dy, 0);
        }

        public Point3D GetIntersect(Point3D lineFirstStar, Point3D lineFirstEnd, Point3D lineSecondStar, Point3D lineSecondEnd)
        {
            #region
            /*
             * L1，L2都存在斜率的情况：
             * 直线方程L1: ( y - y1 ) / ( y2 - y1 ) = ( x - x1 ) / ( x2 - x1 ) 
             * => y = [ ( y2 - y1 ) / ( x2 - x1 ) ]( x - x1 ) + y1
             * 令 a = ( y2 - y1 ) / ( x2 - x1 )
             * 有 y = a * x - a * x1 + y1   .........1
             * 直线方程L2: ( y - y3 ) / ( y4 - y3 ) = ( x - x3 ) / ( x4 - x3 )
             * 令 b = ( y4 - y3 ) / ( x4 - x3 )
             * 有 y = b * x - b * x3 + y3 ..........2
             * 
             * 如果 a = b，则两直线平等，否则， 联解方程 1,2，得:
             * x = ( a * x1 - b * x3 - y1 + y3 ) / ( a - b )
             * y = a * x - a * x1 + y1
             * 
             * L1存在斜率, L2平行Y轴的情况：
             * x = x3
             * y = a * x3 - a * x1 + y1
             * 
             * L1 平行Y轴，L2存在斜率的情况：
             * x = x1
             * y = b * x - b * x3 + y3
             * 
             * L1与L2都平行Y轴的情况：
             * 如果 x1 = x3，那么L1与L2重合，否则平等
             * 
            */
            #endregion
            double a = 0, b = 0;
            int state = 0;
            if (lineFirstStar.X != lineFirstEnd.X)
            {
                a = (lineFirstEnd.Y - lineFirstStar.Y) / (lineFirstEnd.X - lineFirstStar.X);
                state |= 1;
            }
            if (lineSecondStar.X != lineSecondEnd.X)
            {
                b = (lineSecondEnd.Y - lineSecondStar.Y) / (lineSecondEnd.X - lineSecondStar.X);
                state |= 2;
            }
            switch (state)
            {
                case 0: //L1与L2都平行Y轴
                    {
                        if (lineFirstStar.X == lineSecondStar.X)
                        {
                            //throw new Exception("两条直线互相重合，且平行于Y轴，无法计算交点。");
                            return null;
                        }
                        else
                        {
                            //throw new Exception("两条直线互相平行，且平行于Y轴，无法计算交点。");
                            return null;
                        }
                    }
                case 1: //L1存在斜率, L2平行Y轴
                    {
                        double x = lineSecondStar.X;
                        double y = (lineFirstStar.X - x) * (-a) + lineFirstStar.Y;
                        return new Point3D(x, y);
                    }
                case 2: //L1 平行Y轴，L2存在斜率
                    {
                        double x = lineFirstStar.X;
                        //网上有相似代码的，这一处是错误的。你可以对比case 1 的逻辑 进行分析
                        //源code:lineSecondStar * x + lineSecondStar * lineSecondStar.X + p3.Y;
                        double y = (lineSecondStar.X - x) * (-b) + lineSecondStar.Y;
                        return new Point3D(x, y);
                    }
                case 3: //L1，L2都存在斜率
                    {
                        if (a == b)
                        {
                            // throw new Exception("两条直线平行或重合，无法计算交点。");
                            return null;
                        }
                        double x = (a * lineFirstStar.X - b * lineSecondStar.X - lineFirstStar.Y + lineSecondStar.Y) / (a - b);
                        double y = a * x - a * lineFirstStar.X + lineFirstStar.Y;
                        return new Point3D(x, y);
                    }
            }
            // throw new Exception("不可能发生的情况");
            return null;
        }

        /// <summary>
        /// 计算指定点相对于当前线段的转弯方向
        /// </summary>
        /// <param name="otherPoint"></param>
        /// <returns></returns>
        public TurnDirectionType GetTurnDirection(Point3D otherPoint)
        {
            Point3D v1 = BeginPoint.Vector(EndPoint);
            Point3D v2 = EndPoint.Vector(otherPoint);
            double vProduct = (v2 as Point3D).VectorProduct(v1);
            return vProduct > 0 ? TurnDirectionType.Right : vProduct < 0 ? TurnDirectionType.Left : TurnDirectionType.Stright;
        }

        /// <summary>
        /// 根据指定方向和距离平移直线段
        /// </summary>
        /// <param name="radian"></param>
        /// <param name="distance"></param>
        public override void Move(double radian, double distance)
        {
            SetBeginPoint(BeginPoint.GetNextPoint(radian, distance));
            SetEndPoint(EndPoint.GetNextPoint(radian, distance));
        }

        /// <summary>
        /// 根据指定基准点，旋转角度和旋转方向旋转直线段
        /// </summary>
        /// <param name="referencePos"></param>
        /// <param name="radian"></param>
        /// <param name="rotateDirection"></param>
        public override void Rotate(Point3D referencePos, double radian, ArcDirctionType rotateDirection)
        {
            this.BeginPoint = RotatePoint(this.BeginPoint, referencePos, radian, rotateDirection);
            this.EndPoint = RotatePoint(this.EndPoint, referencePos, radian, rotateDirection);
        }

        /// <summary>
        /// 坐标点绕某点旋转radian
        /// </summary>
        /// <param name="point"></param>
        /// <param name="referencePos"></param>
        /// <param name="radian"></param>
        /// <param name="rotateDirection"></param>
        /// <returns></returns>
        private Point3D RotatePoint(Point3D point, Point3D referencePos, double radian, ArcDirctionType rotateDirection)
        {
            radian = Math.Abs(radian);
            if (rotateDirection == ArcDirctionType.UNKONW)
            {
                throw new Exception("ArcDirctionType is wrong.");
            }
            else if (rotateDirection == ArcDirctionType.CLOCK_WISE)
            {
                radian = -radian;
            }
            //假设对图片上任意点(x,y)，绕一个坐标点(rx0,ry0)逆时针旋转a角度后的新的坐标设为(x0, y0)，有公式：
            //x0 = (x - rx0) * cos(a) - (y - ry0) * sin(a) + rx0;
            //y0 = (x - rx0) * sin(a) + (y - ry0) * cos(a) + ry0;
            Point3D returnPoint = new Point3D();
            returnPoint.X = (point.X - referencePos.X) * Math.Cos(radian) - (point.Y - referencePos.Y) * Math.Sin(radian) + referencePos.X;
            returnPoint.Y = (point.X - referencePos.X) * Math.Sin(radian) + (point.Y - referencePos.Y) * Math.Cos(radian) + referencePos.Y;
            returnPoint.Z = point.Z;
            return returnPoint;
        }

        /// <summary>
        /// 根据直线段上的一点把线段截为两个子线段
        /// </summary>
        /// <param name="point">直线上用于截取的点</param>
        /// <returns>截取后的子线段，点不在直线上返回原始线段</returns>
        public override List<CurveSegment> CuteOutbyPoint(Point3D point)
        {
            PointLineRelationShipType relationShipType = this.GetPointRelationShip(point);
            if (relationShipType == PointLineRelationShipType.Point_On_Line)
            {
                List<CurveSegment> list = new List<CurveSegment>();
                list.Add(new LineSegment(this.BeginPoint, point));
                list.Add(new LineSegment(point, this.EndPoint));
                return list;
            }
            else
            {
                return new List<CurveSegment>() { this };
            }
        }

        /// <summary>
        /// 判断传入的射线和当前线段位置关系（当前线段在射线的左、右...）
        /// </summary>
        /// <param name="line">传入的射线</param>
        /// <returns></returns>
        public override LineCurveRelationShipType GetCurveCurveRelationShipType(LineSegment line)
        {

            var dirStart = line.GetTurnDirection(this.GetStartPoint());
            var dirEnd = line.GetTurnDirection(this.GetEndPoint());

            if (dirStart == TurnDirectionType.Stright && dirEnd == TurnDirectionType.Stright)
            {
                return LineCurveRelationShipType.Coincide;
            }
            else if (dirStart == TurnDirectionType.Left && dirEnd == TurnDirectionType.Right ||
                dirStart == TurnDirectionType.Right && dirEnd == TurnDirectionType.Left)
            {
                Point3D p = line.GetIntersect(this.BeginPoint, this.EndPoint);
                PointLineRelationShipType t = line.GetPointRelationShip(p);
                if (line != null && t == PointLineRelationShipType.Point_On_Line || t == PointLineRelationShipType.Point_On_Line_EndDir)
                {
                    return LineCurveRelationShipType.Intersect;
                }
                else
                {
                    return LineCurveRelationShipType.Others;
                }
            }
            else if (dirStart == TurnDirectionType.Left && dirEnd == TurnDirectionType.Left)
            {
                return LineCurveRelationShipType.Left;
            }
            else if (dirStart == TurnDirectionType.Right && dirEnd == TurnDirectionType.Right)
            {
                return LineCurveRelationShipType.Right;
            }
            else if (dirStart == TurnDirectionType.Stright && dirEnd == TurnDirectionType.Left ||
                dirStart == TurnDirectionType.Left && dirEnd == TurnDirectionType.Stright)
            {
                return LineCurveRelationShipType.On_Left;
            }
            else if (dirStart == TurnDirectionType.Stright && dirEnd == TurnDirectionType.Right ||
                dirStart == TurnDirectionType.Right && dirEnd == TurnDirectionType.Stright)
            {
                return LineCurveRelationShipType.On_Right;
            }
            return LineCurveRelationShipType.Others;
        }

        /// <summary>
        /// 获取传入射线与当前线段的交点
        /// </summary>
        /// <param name="line">传入的射线</param>
        /// <returns></returns>
        public override IReadOnlyCollection<Point3D> GetCrossPoints(LineSegment line)
        {
            Point3D point = line.GetIntersect(this.GetStartPoint(), this.GetEndPoint());
            List<Point3D> returnList = new List<Point3D>();
            if (point != null)
            {
                var type = line.GetPointRelationShip(point);
                if (type == PointLineRelationShipType.Point_On_Line || type == PointLineRelationShipType.Point_On_Line_EndDir)  //点在射线上
                {
                    if (this.GetPointRelationShip(point) == PointLineRelationShipType.Point_On_Line) //点在当前线段上
                    {
                        returnList.Add(point);
                    }
                }
            }
            return returnList;
        }

        /// <summary>
        /// 获取曲线段的起始方向（弧线为沿曲线方向的切线方向），单位弧度
        /// </summary>
        /// <returns></returns>
        public override double GetStartCourse()
        {
            return GetDirection();
        }

        /// <summary>
        /// 获取曲线段的末尾方向（弧线为沿曲线方向的切线方向），单位弧度
        /// </summary>
        /// <returns></returns>
        public override double GetEndCourse()
        {
            return GetDirection();
        }

        /// <summary>
        /// 获取起始点
        /// </summary>
        /// <returns></returns>
        public override Point3D GetStartPoint()
        {
            return BeginPoint;
        }

        /// <summary>
        /// 获取末尾点
        /// </summary>
        /// <returns></returns>
        public override Point3D GetEndPoint()
        {
            return EndPoint;
        }

        /// <summary>
        /// 判断点是否在线段上
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool PointIsOnCurveSegment(Point3D point)
        {
            PointLineRelationShipType type = GetPointRelationShip(point);
            if (type == PointLineRelationShipType.Point_On_Line)
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

            LineSegment other = (LineSegment)obj;
            return BeginPoint.Equals(other.BeginPoint)
                && EndPoint.Equals(other.EndPoint);
        }

        public override int GetHashCode()
        {
            const int hashIndex = 307;
            var result = (BeginPoint != null) ? BeginPoint.GetHashCode() : 0;
            result = (result * hashIndex) ^ ((EndPoint != null) ? EndPoint.GetHashCode() : 0);

            return result;
        }

        public override string ToString()
        {
            return string.Format("{0};{1}", BeginPoint.ToString(), EndPoint.ToString());
        }
        #endregion
    }
}
