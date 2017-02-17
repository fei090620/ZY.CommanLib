using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Common.Datas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZY.Common.Types;
namespace ZY.Common.Datas.Tests
{
    [TestClass()]
    public class LineSegmentTests
    {
        LineSegment LineSegment = null;

        [TestInitialize]
        public void Init()
        {
            Point3D startPoint = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D endPoint = new Point3D() { X = 1, Y = 0, Z = 0 };
            LineSegment = new LineSegment(startPoint, endPoint);
        }

        [TestMethod()]
        public void ToListTest()
        {
            var result = LineSegment.ToList(0.01);
            Assert.AreEqual(result.Count, 2);//中间9个点 + start + end
        }

        [TestMethod()]
        public void ReverseTest()
        {
            Point3D startPoint = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D endPoint = new Point3D() { X = 1, Y = 0, Z = 0 };
            LineSegment LineSegment = new LineSegment(startPoint, endPoint);
            var newLineSegment = LineSegment.Reverse();
            Assert.AreEqual(startPoint.GetHashCode(), newLineSegment.GetEndPoint().GetHashCode());
            Assert.AreEqual(endPoint.GetHashCode(), newLineSegment.GetStartPoint().GetHashCode());
        }

        [TestMethod()]
        public void CuteOutbyPointTest()
        {
            Point3D start = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D end = new Point3D() { X = 2, Y = 0, Z = 0 };
            LineSegment LineSegment = new LineSegment(start, end);

            //点在线外
            Point3D other = new Point3D() { X = 3, Y = 0, Z = 0 };
            var result1 = LineSegment.CuteOutbyPoint(other);
            Assert.AreEqual(result1.Count, 1);
            Assert.AreEqual(result1[0].GetEndPoint().X, 2);

            //点在线上
            Point3D other2 = new Point3D() { X = 1, Y = 0, Z = 0 };
            var result2 = LineSegment.CuteOutbyPoint(other2);
            Assert.AreEqual(result2.Count, 2);
            Assert.AreEqual(result2[0].GetEndPoint().X, 1);
        }

        [TestMethod()]
        public void RotateTest()
        {
            Point3D start = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D end = new Point3D() { X = 2, Y = 0, Z = 0 };
            LineSegment LineSegment = new LineSegment(start, end);

            Point3D other = new Point3D() { X = 3, Y = 0, Z = 0 };
            double radian = Math.PI / 2;
            LineSegment.Rotate(other, radian, Types.ArcDirctionType.CLOCK_WISE);
            Assert.AreEqual(LineSegment.GetStartPoint().X, 3);
            Assert.AreEqual(LineSegment.GetStartPoint().Y, 3);
            Assert.AreEqual(LineSegment.GetEndPoint().X, 3);
            Assert.AreEqual(LineSegment.GetEndPoint().Y, 1);
        }

        /// <summary>
        /// 计算两条线段方向向量的夹角圆弧
        /// 取值范围为（0-180度对应的圆弧）
        /// </summary>
        [TestMethod()]
        public void GetIncludeAngleTest()
        {
            //同向0度
            Point3D startPoint2 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D endPoint2 = new Point3D() { X = 1, Y = 0, Z = 0 };
            LineSegment LineSegment2 = new LineSegment(startPoint2, endPoint2);
            var result2 = LineSegment.GetIncludeAngle(LineSegment2);
            Assert.AreEqual(result2, 0);

            //逆向180度
            Point3D startPoint3 = new Point3D() { X = 2, Y = 0, Z = 0 };
            Point3D endPoint3 = new Point3D() { X = -1, Y = 0, Z = 0 };
            LineSegment LineSegment3 = new LineSegment(startPoint3, endPoint3);
            var result3 = LineSegment.GetIncludeAngle(LineSegment3);
            Assert.AreEqual(result3, Math.PI);

            //垂直90度
            Point3D startPoint4 = new Point3D() { X = 2, Y = 0, Z = 0 };
            Point3D endPoint4 = new Point3D() { X = 2, Y = -1, Z = 0 };
            LineSegment LineSegment4 = new LineSegment(startPoint4, endPoint4);
            var result4 = LineSegment.GetIncludeAngle(LineSegment4);
            Assert.AreEqual(result4, Math.PI / 2);

            //锐角30度
            Point3D startPoint5 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D endPoint5 = new Point3D() { X = 1, Y = Math.Pow(3, 0.5) / 3, Z = 0 };
            LineSegment LineSegment5 = new LineSegment(startPoint5, endPoint5);
            var result5 = LineSegment.GetIncludeAngle(LineSegment5);
            Assert.AreEqual(Math.Round(result5, 8), Math.Round(Math.PI / 6, 8));

            //钝角150度
            Point3D startPoint6 = new Point3D() { X = 1, Y = Math.Pow(3, 0.5) / 3, Z = 0 };
            Point3D endPoint6 = new Point3D() { X = 0, Y = 0, Z = 0 };
            LineSegment LineSegment6 = new LineSegment(startPoint6, endPoint6);
            var result6 = LineSegment.GetIncludeAngle(LineSegment6);
            Assert.AreEqual(Math.Round(result6, 8), Math.Round(Math.PI * 5 / 6, 8));
        }

        /// <summary>
        /// 根据指定方向和距离平移直线段
        /// y轴正向为0角度，顺时针增加度数
        /// </summary>
        [TestMethod()]
        public void MoveTest()
        {
            //沿y轴
            Init();
            LineSegment.Move(0, 1);
            Assert.AreEqual(Math.Round(LineSegment.BeginPoint.X, 6), 0);
            Assert.AreEqual(Math.Round(LineSegment.BeginPoint.Y, 6), 1);
            Assert.AreEqual(Math.Round(LineSegment.EndPoint.X, 6), 1);
            Assert.AreEqual(Math.Round(LineSegment.EndPoint.Y, 6), 1);

            //y轴逆向
            Init();
            LineSegment.Move(Math.PI, 1);
            Assert.AreEqual(Math.Round(LineSegment.BeginPoint.X, 6), 0);
            Assert.AreEqual(Math.Round(LineSegment.BeginPoint.Y, 6), -1);
            Assert.AreEqual(Math.Round(LineSegment.EndPoint.X, 6), 1);
            Assert.AreEqual(Math.Round(LineSegment.EndPoint.Y, 6), -1);

            //x轴正向
            Init();
            LineSegment.Move(Math.PI / 2, 1);
            Assert.AreEqual(Math.Round(LineSegment.BeginPoint.X, 6), 1);
            Assert.AreEqual(Math.Round(LineSegment.BeginPoint.Y, 6), 0);
            Assert.AreEqual(Math.Round(LineSegment.EndPoint.X, 6), 2);
            Assert.AreEqual(Math.Round(LineSegment.EndPoint.Y, 6), 0);

            //x轴逆向
            Init();
            LineSegment.Move(Math.PI * 3 / 2, 1);
            Assert.AreEqual(Math.Round(LineSegment.BeginPoint.X, 6), -1);
            Assert.AreEqual(Math.Round(LineSegment.BeginPoint.Y, 6), 0);
            Assert.AreEqual(Math.Round(LineSegment.EndPoint.X, 6), 0);
            Assert.AreEqual(Math.Round(LineSegment.EndPoint.Y, 6), 0);
        }

        /// <summary>
        /// 延长类型：0都延长，1延长尾点，-1延长首点
        /// </summary>
        [TestMethod()]
        public void ExpandTest()
        {
            //type 0
            Init();
            LineSegment newLine1 = LineSegment.Expand(0, 1);
            Assert.AreEqual(Math.Round(newLine1.BeginPoint.X, 6), -1);
            Assert.AreEqual(Math.Round(newLine1.BeginPoint.Y, 6), 0);
            Assert.AreEqual(Math.Round(newLine1.EndPoint.X, 6), 2);
            Assert.AreEqual(Math.Round(newLine1.EndPoint.Y, 6), 0);

            //type 1
            Init();
            LineSegment newLine2 = LineSegment.Expand(1, 1);
            Assert.AreEqual(Math.Round(newLine2.BeginPoint.X, 6), 0);
            Assert.AreEqual(Math.Round(newLine2.BeginPoint.Y, 6), 0);
            Assert.AreEqual(Math.Round(newLine2.EndPoint.X, 6), 2);
            Assert.AreEqual(Math.Round(newLine2.EndPoint.Y, 6), 0);

            //type -1
            Init();
            LineSegment newLine3 = LineSegment.Expand(-1, 1);
            Assert.AreEqual(Math.Round(newLine3.BeginPoint.X, 6), -1);
            Assert.AreEqual(Math.Round(newLine3.BeginPoint.Y, 6), 0);
            Assert.AreEqual(Math.Round(newLine3.EndPoint.X, 6), 1);
            Assert.AreEqual(Math.Round(newLine3.EndPoint.Y, 6), 0);
        }

        /// <summary>
        /// 点与线段位置关系
        /// </summary>
        [TestMethod()]
        public void GetPointRelationShipTest()
        {
            Init();
            Point3D testPoint = new Point3D() { X = 0, Y = 0, Z = 0 };
            var relation = this.LineSegment.GetPointRelationShip(testPoint);
            Assert.AreEqual(relation, PointLineRelationShipType.Point_On_Line);

            Init();
            Point3D testPoint1 = new Point3D() { X = 0.5, Y = 0, Z = 0 };
            var relation1 = this.LineSegment.GetPointRelationShip(testPoint1);
            Assert.AreEqual(relation1, PointLineRelationShipType.Point_On_Line);

            Point3D testPoint2 = new Point3D() { X = 2, Y = 0, Z = 0 };
            var relation2 = this.LineSegment.GetPointRelationShip(testPoint2);
            Assert.AreEqual(relation2, PointLineRelationShipType.Point_On_Line_EndDir);

            Point3D testPoint3 = new Point3D() { X = -1, Y = 0, Z = 0 };
            var relation3 = this.LineSegment.GetPointRelationShip(testPoint3);
            Assert.AreEqual(relation3, PointLineRelationShipType.Point_On_Line_BeginDir);

            Point3D testPoint4 = new Point3D() { X = -1, Y = 1, Z = 0 };
            var relation4 = this.LineSegment.GetPointRelationShip(testPoint4);
            Assert.AreEqual(relation4, PointLineRelationShipType.Point_Not_On_Line);
        }

        /// <summary>
        /// 点位于线段的方位
        /// </summary>
        [TestMethod()]
        public void GetTurnDirectionTest()
        {
            Point3D start = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D end = new Point3D() { X = 1, Y = 0, Z = 0 };
            LineSegment line = new LineSegment(start, end);

            Assert.AreEqual(TurnDirectionType.Stright, line.GetTurnDirection(new Point3D() { X = -2, Y = 0, Z = 0 }));
            Assert.AreEqual(TurnDirectionType.Left, line.GetTurnDirection(new Point3D() { X = 2, Y = 1, Z = 0 }));
            Assert.AreEqual(TurnDirectionType.Left, line.GetTurnDirection(new Point3D() { X = -2, Y = 1, Z = 0 }));
            Assert.AreEqual(TurnDirectionType.Right, line.GetTurnDirection(new Point3D() { X = 2, Y = -1, Z = 0 }));
            Assert.AreEqual(TurnDirectionType.Right, line.GetTurnDirection(new Point3D() { X = -2, Y = -1, Z = 0 }));
        }

        /// <summary>
        /// 获取线段方向（弧度）(向量方向与正北方向夹角)
        /// 范围为 （-pi 到 pi]
        /// </summary>
        [TestMethod()]
        public void GetDirectionTest()
        {
            //线段起始点为空测试
            object exception = null;
            LineSegment nullTest = new Datas.LineSegment(null, null);
            try
            {
                var dir_null = nullTest.GetDirection();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);

            //垂直测试(向东)
            Init();
            var dir = this.LineSegment.GetDirection();
            Assert.AreEqual(dir, Math.PI / 2);

            //正北测试
            Point3D startPoint2 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D endPoint2 = new Point3D() { X = 0, Y = 1, Z = 0 };
            LineSegment LineSegment2 = new LineSegment(startPoint2, endPoint2);
            var dir2 = LineSegment2.GetDirection();
            Assert.AreEqual(dir2, 0);

            //正南测试
            Point3D startPoint3 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D endPoint3 = new Point3D() { X = 0, Y = -1, Z = 0 };
            LineSegment LineSegment3 = new LineSegment(startPoint3, endPoint3);
            var dir3 = LineSegment3.GetDirection();
            Assert.AreEqual(dir3, Math.PI);

            //垂直测试(向西)
            Point3D startPoint4 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D endPoint4 = new Point3D() { X = -1, Y = 0, Z = 0 };
            LineSegment LineSegment4 = new LineSegment(startPoint4, endPoint4);
            var dir4 = LineSegment4.GetDirection();
            Assert.AreEqual(dir4, -Math.PI / 2);
        }

        /// <summary>
        /// 获取线段长度
        /// </summary>
        [TestMethod()]
        public void GetLengthTest()
        {
            //线段起始点为空测试
            object exception = null;
            LineSegment nullTest = new Datas.LineSegment(null, null);
            try
            {
                var dir_null = nullTest.GetLength();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);


            //长度测试
            Init();
            var length = this.LineSegment.GetLength();
            Assert.AreEqual(length, 1);
        }

        [TestMethod()]
        public void GetStartCourseTest()
        {
            //重复调用
            //GetDirectionTest();
        }

        [TestMethod()]
        public void GetEndCourseTest()
        {
            //重复调用
            //GetDirectionTest();
        }

        [TestMethod()]
        public void GetStartPointTest()
        {
            Init();
            Point3D start = this.LineSegment.GetStartPoint();
            Assert.AreEqual(start.X, 0);
            Assert.AreEqual(start.Y, 0);
            Assert.AreEqual(start.Z, 0);
        }

        [TestMethod()]
        public void GetEndPointTest()
        {
            Init();
            Point3D end = this.LineSegment.GetEndPoint();
            Assert.AreEqual(end.X, 1);
            Assert.AreEqual(end.Y, 0);
            Assert.AreEqual(end.Z, 0);
        }

        [TestMethod()]
        public void EqualsTest()
        {
            Init();
            LineSegment newLine = new Datas.LineSegment(new Point3D() { X = 0, Y = 0, Z = 0 }, new Point3D() { X = 1, Y = 0, Z = 0 });
            var result = this.LineSegment.Equals(newLine);
            Assert.AreEqual(result, true);

            Init();
            LineSegment newLine2 = new Datas.LineSegment(new Point3D() { X = 2, Y = 0, Z = 0 }, new Point3D() { X = 3, Y = 0, Z = 0 });
            var result2 = this.LineSegment.Equals(newLine2);
            Assert.AreEqual(result2, false);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Init();
            string str = this.LineSegment.ToString();
            Assert.AreEqual(str, "0,0,0;1,0,0");
        }

        /// <summary>
        /// 直线的交点
        /// </summary>
        [TestMethod()]
        public void GetIntersectTest()
        {
            GetIntersectTest1();
        }

        /// <summary>
        /// 计算线段所在直线的交点
        /// </summary>
        [TestMethod()]
        public void GetIntersectTest1()
        {
            Init();
            Point3D otherStart = new Point3D() { X = 0.5, Y = 1, Z = 0 };
            Point3D otherEnd = new Point3D() { X = 0.5, Y = 0.5, Z = 0 };
            Point3D point = LineSegment.GetIntersect(LineSegment.BeginPoint, LineSegment.EndPoint, otherStart, otherEnd);
            Assert.AreEqual(Math.Round(point.X, 8), 0.5);
            Assert.AreEqual(Math.Round(point.Y, 8), 0);

            Init();
            Point3D otherStart2 = new Point3D() { X = 0.5, Y = 1, Z = 0 };
            Point3D otherEnd2 = new Point3D() { X = 0.5, Y = -1, Z = 0 };
            Point3D point2 = LineSegment.GetIntersect(LineSegment.BeginPoint, LineSegment.EndPoint, otherStart2, otherEnd2);
            Assert.AreEqual(Math.Round(point2.X, 8), 0.5);
            Assert.AreEqual(Math.Round(point2.Y, 8), 0);

            Init();
            Point3D otherStart3 = new Point3D() { X = 1.5, Y = 1, Z = 0 };
            Point3D otherEnd3 = new Point3D() { X = 1.5, Y = 0.5, Z = 0 };
            Point3D point3 = LineSegment.GetIntersect(LineSegment.BeginPoint, LineSegment.EndPoint, otherStart3, otherEnd3);
            Assert.AreEqual(Math.Round(point3.X, 8), 1.5);
            Assert.AreEqual(Math.Round(point3.Y, 8), 0);

            Init();
            Point3D otherStart4 = new Point3D() { X = 1, Y = 1, Z = 0 };
            Point3D otherEnd4 = new Point3D() { X = 1, Y = -0.5, Z = 0 };
            Point3D point4 = LineSegment.GetIntersect(LineSegment.BeginPoint, LineSegment.EndPoint, otherStart4, otherEnd4);
            Assert.AreEqual(Math.Round(point4.X, 8), 1);
            Assert.AreEqual(Math.Round(point4.Y, 8), 0);

            Init();
            Point3D otherStart5 = new Point3D() { X = 1, Y = 1, Z = 0 };
            Point3D otherEnd5 = new Point3D() { X = 2, Y = 1, Z = 0 };
            Point3D point5 = LineSegment.GetIntersect(LineSegment.BeginPoint, LineSegment.EndPoint, otherStart5, otherEnd5);
            Assert.IsNull(point5);

            Init();
            Point3D otherStart6 = new Point3D() { X = 0.5, Y = 0, Z = 0 };
            Point3D otherEnd6 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D point6 = LineSegment.GetIntersect(LineSegment.BeginPoint, LineSegment.EndPoint, otherStart6, otherEnd6);
            Assert.IsNull(point6);
        }

        /// <summary>
        /// 计算线段所在直线的交点
        /// </summary>
        [TestMethod()]
        public void GetArcDirToLineDirintersectionTest()
        {
            Init();
            Point3D otherStart = new Point3D() { X = 0.5, Y = 1, Z = 0 };
            Point3D otherEnd = new Point3D() { X = 0.5, Y = 0.5, Z = 0 };
            Point3D point = LineSegment.GetArcDirToLineDirintersection(new Datas.LineSegment(otherStart, otherEnd));
            Assert.AreEqual(Math.Round(point.X, 8), 0.5);
            Assert.AreEqual(Math.Round(point.Y, 8), 0);

            Init();
            Point3D otherStart2 = new Point3D() { X = 0.5, Y = 1, Z = 0 };
            Point3D otherEnd2 = new Point3D() { X = 0.5, Y = -1, Z = 0 };
            Point3D point2 = LineSegment.GetArcDirToLineDirintersection(new Datas.LineSegment(otherStart2, otherEnd2));
            Assert.AreEqual(Math.Round(point2.X, 8), 0.5);
            Assert.AreEqual(Math.Round(point2.Y, 8), 0);

            Init();
            Point3D otherStart3 = new Point3D() { X = 1.5, Y = 1, Z = 0 };
            Point3D otherEnd3 = new Point3D() { X = 1.5, Y = 0.5, Z = 0 };
            Point3D point3 = LineSegment.GetArcDirToLineDirintersection(new Datas.LineSegment(otherStart3, otherEnd3));
            Assert.AreEqual(Math.Round(point3.X, 8), 1.5);
            Assert.AreEqual(Math.Round(point3.Y, 8), 0);

            Init();
            Point3D otherStart4 = new Point3D() { X = 1, Y = 1, Z = 0 };
            Point3D otherEnd4 = new Point3D() { X = 1, Y = -0.5, Z = 0 };
            Point3D point4 = LineSegment.GetArcDirToLineDirintersection(new Datas.LineSegment(otherStart4, otherEnd4));
            Assert.AreEqual(Math.Round(point4.X, 8), 1);
            Assert.AreEqual(Math.Round(point4.Y, 8), 0);

            Init();
            Point3D otherStart5 = new Point3D() { X = 1, Y = 1, Z = 0 };
            Point3D otherEnd5 = new Point3D() { X = 2, Y = 1, Z = 0 };
            Point3D point5 = LineSegment.GetArcDirToLineDirintersection(new Datas.LineSegment(otherStart5, otherEnd5));
            Assert.IsNull(point5);

            Init();
            Point3D otherStart6 = new Point3D() { X = 0.5, Y = 0, Z = 0 };
            Point3D otherEnd6 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D point6 = LineSegment.GetArcDirToLineDirintersection(new Datas.LineSegment(otherStart6, otherEnd6));
            Assert.IsNull(point6);
        }

        /// <summary>
        /// 计算两条线段的交点
        /// </summary>
        [TestMethod()]
        public void GetIntersectionTest()
        {
            Init();
            Point3D otherStart = new Point3D() { X = 0.5, Y = 1, Z = 0 };
            Point3D otherEnd = new Point3D() { X = 0.5, Y = -1, Z = 0 };
            LineSegment otherLine = new Datas.LineSegment(otherStart, otherEnd);
            Point3D point = LineSegment.GetIntersection(otherLine);
            Assert.AreEqual(Math.Round(point.X, 6), 0.5);
            Assert.AreEqual(Math.Round(point.Y, 6), 0);

            Init();
            Point3D otherStart2 = new Point3D() { X = 0.5, Y = 1, Z = 0 };
            Point3D otherEnd2 = new Point3D() { X = 0.5, Y = 0.5, Z = 0 };
            LineSegment otherLine2 = new Datas.LineSegment(otherStart2, otherEnd2);
            Point3D point2 = LineSegment.GetIntersection(otherLine2);
            Assert.IsNull(point2);

            Init();
            Point3D otherStart3 = new Point3D() { X = 1.5, Y = 1, Z = 0 };
            Point3D otherEnd3 = new Point3D() { X = 1.5, Y = -1, Z = 0 };
            LineSegment otherLine3 = new Datas.LineSegment(otherStart3, otherEnd3);
            Point3D point3 = LineSegment.GetIntersection(otherLine3);
            Assert.IsNull(point3);

            Init();
            Point3D otherStart4 = new Point3D() { X = 1, Y = 1, Z = 0 };
            Point3D otherEnd4 = new Point3D() { X = 1, Y = -1, Z = 0 };
            LineSegment otherLine4 = new Datas.LineSegment(otherStart4, otherEnd4);
            Point3D point4 = LineSegment.GetIntersection(otherLine4);
            Assert.AreEqual(Math.Round(point4.X, 6), 1);
            Assert.AreEqual(Math.Round(point4.Y, 6), 0);

            Init();
            Point3D otherStart5 = new Point3D() { X = 1.5, Y = 0, Z = 0 };
            Point3D otherEnd5 = new Point3D() { X = 2, Y = 0, Z = 0 };
            LineSegment otherLine5 = new Datas.LineSegment(otherStart5, otherEnd5);
            Point3D point5 = LineSegment.GetIntersection(otherLine5);
            Assert.IsNull(point5);
        }

        /// <summary>
        /// 线段与射线的位置关系（当前线段在射线的左/右...）
        /// </summary>
        [TestMethod()]
        public void GetCurveCurveRelationShipTypeTest()
        {
            Init();
            Point3D otherStart = new Point3D() { X = 0.5, Y = 1, Z = 0 };
            Point3D otherEnd = new Point3D() { X = 0.5, Y = -1, Z = 0 };
            LineSegment otherLine = new Datas.LineSegment(otherStart, otherEnd);
            LineCurveRelationShipType type = LineSegment.GetCurveCurveRelationShipType(otherLine);
            Assert.AreEqual(type, LineCurveRelationShipType.Intersect);

            Init();
            Point3D otherStart2 = new Point3D() { X = 0.5, Y = 1, Z = 0 };
            Point3D otherEnd2 = new Point3D() { X = 0.5, Y = 0.5, Z = 0 };
            LineSegment otherLine2 = new Datas.LineSegment(otherStart2, otherEnd2);
            LineCurveRelationShipType type2 = LineSegment.GetCurveCurveRelationShipType(otherLine2);
            Assert.AreEqual(type2, LineCurveRelationShipType.Intersect);

            Init();
            Point3D otherStart3 = new Point3D() { X = 2, Y = 1, Z = 0 };
            Point3D otherEnd3 = new Point3D() { X = 2, Y = 0.5, Z = 0 };
            LineSegment otherLine3 = new Datas.LineSegment(otherStart3, otherEnd3);
            LineCurveRelationShipType type3 = LineSegment.GetCurveCurveRelationShipType(otherLine3);
            Assert.AreEqual(type3, LineCurveRelationShipType.Right);

            Init();
            Point3D otherStart4 = new Point3D() { X = 0.5, Y = 0.5, Z = 0 };
            Point3D otherEnd4 = new Point3D() { X = 0.5, Y = 1, Z = 0 };
            LineSegment otherLine4 = new Datas.LineSegment(otherStart4, otherEnd4);
            LineCurveRelationShipType type4 = LineSegment.GetCurveCurveRelationShipType(otherLine4);
            Assert.AreEqual(type4, LineCurveRelationShipType.Others);

            Init();
            Point3D otherStart5 = new Point3D() { X = 0.5, Y = 0, Z = 0 };
            Point3D otherEnd5 = new Point3D() { X = 0.5, Y = 0.5, Z = 0 };
            LineSegment otherLine5 = new Datas.LineSegment(otherStart5, otherEnd5);
            LineCurveRelationShipType type5 = LineSegment.GetCurveCurveRelationShipType(otherLine5);
            Assert.AreEqual(type5, LineCurveRelationShipType.Intersect);

            Init();
            Point3D otherStart6 = new Point3D() { X = -0.5, Y = 0, Z = 0 };
            Point3D otherEnd6 = new Point3D() { X = 0.5, Y = 0, Z = 0 };
            LineSegment otherLine6 = new Datas.LineSegment(otherStart6, otherEnd6);
            LineCurveRelationShipType type6 = LineSegment.GetCurveCurveRelationShipType(otherLine6);
            Assert.AreEqual(type6, LineCurveRelationShipType.Coincide);

            Init();
            Point3D otherStart7 = new Point3D() { X = 2, Y = 2, Z = 0 };
            Point3D otherEnd7 = new Point3D() { X = 2, Y = 3, Z = 0 };
            LineSegment otherLine7 = new Datas.LineSegment(otherStart7, otherEnd7);
            LineCurveRelationShipType type7 = LineSegment.GetCurveCurveRelationShipType(otherLine7);
            Assert.AreEqual(type7, LineCurveRelationShipType.Left);

            Init();
            Point3D otherStart8 = new Point3D() { X = 1, Y = -1, Z = 0 };
            Point3D otherEnd8 = new Point3D() { X = 1, Y = 3, Z = 0 };
            LineSegment otherLine8 = new Datas.LineSegment(otherStart8, otherEnd8);
            LineCurveRelationShipType type8 = LineSegment.GetCurveCurveRelationShipType(otherLine8);
            Assert.AreEqual(type8, LineCurveRelationShipType.On_Left);
        }

        [TestMethod()]
        public void GetCrossPointsTest()
        {
            Init();
            Point3D otherStart = new Point3D() { X = 0.5, Y = 1, Z = 0 };
            Point3D otherEnd = new Point3D() { X = 0.5, Y = -1, Z = 0 };
            LineSegment otherLine = new Datas.LineSegment(otherStart, otherEnd);
            List<Point3D> list = LineSegment.GetCrossPoints(otherLine) as List<Point3D>;
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count, 1);
            Assert.AreEqual(Math.Round(list[0].X, 8), 0.5);
            Assert.AreEqual(Math.Round(list[0].Y, 8), 0);


            Init();
            Point3D otherStart2 = new Point3D() { X = 0.5, Y = 1, Z = 0 };
            Point3D otherEnd2 = new Point3D() { X = 0.5, Y = 0.5, Z = 0 };
            LineSegment otherLine2 = new Datas.LineSegment(otherStart2, otherEnd2);
            List<Point3D> list2 = LineSegment.GetCrossPoints(otherLine2) as List<Point3D>;
            Assert.IsNotNull(list2);
            Assert.AreEqual(list2.Count, 1);
            Assert.AreEqual(Math.Round(list2[0].X, 8), 0.5);
            Assert.AreEqual(Math.Round(list2[0].Y, 8), 0);

            Init();
            Point3D otherStart3 = new Point3D() { X = 2, Y = 1, Z = 0 };
            Point3D otherEnd3 = new Point3D() { X = 2, Y = 0.5, Z = 0 };
            LineSegment otherLine3 = new Datas.LineSegment(otherStart3, otherEnd3);
            List<Point3D> list3 = LineSegment.GetCrossPoints(otherLine3) as List<Point3D>;
            Assert.IsNotNull(list3);
            Assert.AreEqual(list3.Count, 0);

            Init();
            Point3D otherStart4 = new Point3D() { X = 0.5, Y = 0.5, Z = 0 };
            Point3D otherEnd4 = new Point3D() { X = 0.5, Y = 1, Z = 0 };
            LineSegment otherLine4 = new Datas.LineSegment(otherStart4, otherEnd4);
            List<Point3D> list4 = LineSegment.GetCrossPoints(otherLine4) as List<Point3D>;
            Assert.IsNotNull(list4);
            Assert.AreEqual(list4.Count, 0);

            Init();
            Point3D otherStart5 = new Point3D() { X = 0.5, Y = 0, Z = 0 };
            Point3D otherEnd5 = new Point3D() { X = 0.5, Y = -0.5, Z = 0 };
            LineSegment otherLine5 = new Datas.LineSegment(otherStart5, otherEnd5);
            List<Point3D> list5 = LineSegment.GetCrossPoints(otherLine5) as List<Point3D>;
            Assert.IsNotNull(list5);
            Assert.AreEqual(list5.Count, 1);
            Assert.AreEqual(Math.Round(list5[0].X, 8), 0.5);
            Assert.AreEqual(Math.Round(list5[0].Y, 8), 0);

            Init();
            Point3D otherStart6 = new Point3D() { X = -0.5, Y = 0, Z = 0 };
            Point3D otherEnd6 = new Point3D() { X = 0.5, Y = 0, Z = 0 };
            LineSegment otherLine6 = new Datas.LineSegment(otherStart6, otherEnd6);
            List<Point3D> list6 = LineSegment.GetCrossPoints(otherLine6) as List<Point3D>;
            Assert.IsNotNull(list6);
            Assert.AreEqual(list6.Count, 0);
        }

        /// <summary>
        ///  计算两条线段所在射线的交点
        /// </summary>
        [TestMethod()]
        public void GetLineDirIntersectionTest()
        {
            Point3D startPoint = new Point3D() { X = 0.5, Y = 2, Z = 0 };
            Point3D endPoint = new Point3D() { X = 0.5, Y = -1, Z = 0 };
            LineSegment otherLine = new LineSegment(startPoint, endPoint);
            Point3D point = LineSegment.GetLineDirIntersection(otherLine);
            Assert.AreEqual(point.X, 0.5);
            Assert.AreEqual(point.Y, 0);

            Point3D startPoint2 = new Point3D() { X = 2, Y = 2, Z = 0 };
            Point3D endPoint2 = new Point3D() { X = 2, Y = 1, Z = 0 };
            LineSegment otherLine2 = new LineSegment(startPoint2, endPoint2);
            Point3D point2 = LineSegment.GetLineDirIntersection(otherLine2);
            Assert.AreEqual(point2.X, 2);
            Assert.AreEqual(point2.Y, 0);

            Point3D startPoint3 = new Point3D() { X = 2, Y = 1, Z = 0 };
            Point3D endPoint3 = new Point3D() { X = 2, Y = 2, Z = 0 };
            LineSegment otherLine3 = new LineSegment(startPoint3, endPoint3);
            Point3D point3 = LineSegment.GetLineDirIntersection(otherLine3);
            Assert.IsNull(point3);

            Point3D startPoint4 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D endPoint4 = new Point3D() { X = 1, Y = -2, Z = 0 };
            LineSegment otherLine4 = new LineSegment(startPoint4, endPoint4);
            Point3D point4 = LineSegment.GetLineDirIntersection(otherLine4);
            Assert.AreEqual(point4.X, 1);
            Assert.AreEqual(point4.Y, 0);

            Point3D startPoint5 = new Point3D() { X = -1, Y = 0, Z = 0 };
            Point3D endPoint5 = new Point3D() { X = -2, Y = 0, Z = 0 };
            LineSegment otherLine5 = new LineSegment(startPoint5, endPoint5);
            Point3D point5 = LineSegment.GetLineDirIntersection(otherLine5);
            Assert.IsNull(point5);
        }

        /// <summary>
        /// 重写HashCode方法
        /// </summary>
        [TestMethod()]
        public void GetHashCodeTest()
        {
            Init();

            Point3D startPoint2 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D endPoint2 = new Point3D() { X = 1, Y = 0, Z = 0 };
            LineSegment LineSegment2 = new LineSegment(startPoint2, endPoint2);

            Point3D startPoint3 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D endPoint3 = new Point3D() { X = 2, Y = 0, Z = 0 };
            LineSegment LineSegment3 = new LineSegment(startPoint3, endPoint3);

            var hasCode1 = this.LineSegment.GetHashCode();
            var hasCode2 = LineSegment2.GetHashCode();
            var hasCode3 = LineSegment3.GetHashCode();

            Assert.AreEqual(hasCode1, hasCode2);
            Assert.AreNotEqual(hasCode1, hasCode3);
        }

        /// <summary>
        /// 当前线段与传入射线的交点
        /// </summary>
        [TestMethod()]
        public void GetIntersectionWithRayTest()
        {
            Point3D point = new Point3D() { X = 0, Y = -0.5, Z = 0 };
            Point3D p = LineSegment.GetIntersectionWithRay(point, Math.PI / 4);
            Assert.AreEqual(Math.Round(p.X, 8), 0.5);
            Assert.AreEqual(Math.Round(p.Y, 8), 0);
        }

        /// <summary>
        /// 判断点是否在线段上
        /// </summary>
        [TestMethod()]
        public void PointIsOnCurveSegmentTest()
        {
            Init();
            Point3D testPoint = new Point3D() { X = 0, Y = 0, Z = 0 };
            var relation = this.LineSegment.PointIsOnCurveSegment(testPoint);
            Assert.AreEqual(relation, true);

            Init();
            Point3D testPoint1 = new Point3D() { X = 0.5, Y = 0, Z = 0 };
            var relation1 = this.LineSegment.PointIsOnCurveSegment(testPoint1);
            Assert.AreEqual(relation1, true);

            Point3D testPoint2 = new Point3D() { X = 2, Y = 0, Z = 0 };
            var relation2 = this.LineSegment.PointIsOnCurveSegment(testPoint2);
            Assert.AreEqual(relation2, false);

            Point3D testPoint3 = new Point3D() { X = -1, Y = 0, Z = 0 };
            var relation3 = this.LineSegment.PointIsOnCurveSegment(testPoint3);
            Assert.AreEqual(relation3, false);

            Point3D testPoint4 = new Point3D() { X = -1, Y = 1, Z = 0 };
            var relation4 = this.LineSegment.PointIsOnCurveSegment(testPoint4);
            Assert.AreEqual(relation4, false);
        }

        /// <summary>
        /// 计算点到直线段所在直线的距离
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pedalPoint">垂足</param>
        /// <returns></returns>
        [TestMethod()]
        public void ToDisTest()
        {
            Init();
            Point3D point;
            double dis = LineSegment.ToDis(new Point3D() { X = 0.5, Y = 1, Z = 0 }, out point);
            Assert.AreEqual(Math.Round(point.X, 6), 0.5);
            Assert.AreEqual(Math.Round(point.Y, 6), 0);
            Assert.AreEqual(dis, 1);

            Init();
            Point3D point2;
            double dis2 = LineSegment.ToDis(new Point3D() { X = 2, Y = 2, Z = 0 }, out point2);
            Assert.AreEqual(Math.Round(point2.X, 6), 2);
            Assert.AreEqual(Math.Round(point2.Y, 6), 0);
            Assert.AreEqual(dis2, 2);

            Init();
            Point3D point3;
            double dis3 = LineSegment.ToDis(new Point3D() { X = -2, Y = 0, Z = 0 }, out point3);
            Assert.AreEqual(Math.Round(point3.X, 6), -2);
            Assert.AreEqual(Math.Round(point3.Y, 6), 0);
            Assert.AreEqual(Math.Round(dis3, 8), 0);

            Init();
            Point3D point4;
            double dis4 = LineSegment.ToDis(new Point3D() { X = 0, Y = 0, Z = 0 }, out point4);
            Assert.AreEqual(Math.Round(point4.X, 6), 0);
            Assert.AreEqual(Math.Round(point4.Y, 6), 0);
            Assert.AreEqual(Math.Round(dis4, 8), 0);
        }
    }
}
