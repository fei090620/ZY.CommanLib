using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Common.Datas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ZY.Common.Types;
namespace ZY.Common.Datas.Tests
{
    [TestClass()]
    public class ArcSegmentTests
    {
        public ArcSegment ArcSegment_Full = null; //整圆（360度）
        public ArcSegment ArcSegment_Part = null; //1/4圆（90度）

        [TestInitialize]
        public void Init()
        {
            Point3D center = new Point3D() { X = 0, Y = 0, Z = 0 };
            this.ArcSegment_Full = new ArcSegment(center, 1, Types.ArcDirctionType.CLOCK_WISE);

            Point3D start = new Point3D() { X = 0, Y = 1, Z = 0 };
            Point3D end = new Point3D() { X = 1, Y = 0, Z = 0 };
            this.ArcSegment_Part = new ArcSegment(center, start, end, ArcDirctionType.CLOCK_WISE);
        }

        /// <summary>
        /// 按照精度（弧度精度）获取点集合（分为整圆和部分圆情况）
        /// </summary>
        [TestMethod()]
        public void ToListTest()
        {
            //传入的精度为角度精度，需要先将弧度转化为角度
            //整圆非整除
            Init();
            var list = this.ArcSegment_Full.ToList(35);
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Count, Math.Ceiling((double)360 / 35));

            //半圆非整除
            Init();
            var list2 = this.ArcSegment_Part.ToList(20);
            Assert.IsNotNull(list2);
            Assert.AreEqual(list2.Count, Math.Ceiling((double)90 / 20) + 1); //向上取整再+1

            //整圆整除
            Init();
            var list3 = this.ArcSegment_Full.ToList(36);
            Assert.IsNotNull(list3);
            Assert.AreEqual(list3.Count, Math.Ceiling((double)360 / 36));

            //半圆整除
            Init();
            var list4 = this.ArcSegment_Part.ToList(30);
            Assert.IsNotNull(list4);
            Assert.AreEqual(list4.Count, Math.Ceiling((double)90 / 30) + 1);
        }

        /// <summary>
        /// 获取圆弧的圆心角都（弧度）
        /// </summary>
        [TestMethod()]
        public void GetArcTurnAngleTest()
        {
            //结果为弧度
            var result = this.ArcSegment_Full.GetArcTurnAngle();
            Assert.AreEqual(result, 2 * Math.PI);

            //结果为弧度
            var result2 = this.ArcSegment_Part.GetArcTurnAngle();
            Assert.AreEqual(result2, Math.PI / 2);
        }

        /// <summary>
        ///  根据圆外一点计算圆弧切点
        /// </summary>
        [TestMethod()]
        public void GetTangentPointbyOutPointTest()
        {
            //Case1: 点在圆弧内（整圆）
            Point3D point1 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D result1 = this.ArcSegment_Full.GetTangentPointbyOutPoint(point1, Types.TurnDirectionType.Right); //切点
            Assert.IsNull(result1);

            //Case2: 点在圆弧内（1/4圆）
            Point3D point2 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D result2 = this.ArcSegment_Part.GetTangentPointbyOutPoint(point2, Types.TurnDirectionType.Right); //切点
            Assert.IsNull(result2);

            //Case3: 点在圆弧上（整圆）
            Point3D point3 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D result3 = this.ArcSegment_Full.GetTangentPointbyOutPoint(point3, Types.TurnDirectionType.Right); //切点
            Assert.AreEqual(result3.X, 1);
            Assert.AreEqual(result3.Y, 0);

            //Case4: 点在圆弧上（1/4圆）
            Point3D point4 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D result4 = this.ArcSegment_Part.GetTangentPointbyOutPoint(point4, Types.TurnDirectionType.Right); //切点
            Assert.AreEqual(result4.X, 1);
            Assert.AreEqual(result4.Y, 0);

            //Case5: 点在圆弧外（整圆）
            Point3D point5 = new Point3D() { X = 1, Y = 1, Z = 0 };
            Point3D result5 = this.ArcSegment_Full.GetTangentPointbyOutPoint(point5, Types.TurnDirectionType.Left); //切点
            Assert.AreEqual(Math.Round(result5.X, 8), 1);
            Assert.AreEqual(Math.Round(result5.Y, 8), 0);

            //Case6: 点在圆弧外（1/4圆）
            Point3D point6 = new Point3D() { X = 1, Y = 1, Z = 0 };
            Point3D result6 = this.ArcSegment_Part.GetTangentPointbyOutPoint(point6, Types.TurnDirectionType.Left); //切点
            Assert.AreEqual(Math.Round(result6.X, 8), 1);
            Assert.AreEqual(Math.Round(result6.Y, 8), 0);

            //Case7: 点在圆弧外,圆弧相反方向上，应无切线（1/4圆）
            Point3D point7 = new Point3D() { X = -3, Y = -3, Z = 0 };
            Point3D result7 = this.ArcSegment_Part.GetTangentPointbyOutPoint(point7, Types.TurnDirectionType.Left); //切点
            Assert.IsNull(result7);
        }

        /// <summary>
        /// 计算直线与圆弧的交点
        /// </summary>
        [TestMethod()]
        public void GetIntersectPointsWithLineTest()
        {
            Point3D start = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D end = new Point3D() { X = 1, Y = 0, Z = 0 };
            //Case1: 整圆（相交）
            List<Point3D> list1 = this.ArcSegment_Full.GetIntersectPointsWithLine(start, end);
            Assert.AreEqual(list1.Count, 2);
            Assert.AreEqual(Math.Round(list1[0].X, 8), 1);
            Assert.AreEqual(Math.Round(list1[0].Y, 8), 0);
            Assert.AreEqual(Math.Round(list1[1].X, 8), -1);
            Assert.AreEqual(Math.Round(list1[1].Y, 8), 0);

            //Case2: 1/4圆（相交）
            List<Point3D> list2 = this.ArcSegment_Part.GetIntersectPointsWithLine(start, end);
            Assert.AreEqual(list2.Count, 1);
            Assert.AreEqual(Math.Round(list2[0].X, 8), 1);
            Assert.AreEqual(Math.Round(list2[0].Y, 8), 0);

            Point3D start2 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D end2 = new Point3D() { X = 1, Y = 1, Z = 0 };
            //Case3: 整圆（相切）
            List<Point3D> list3 = this.ArcSegment_Full.GetIntersectPointsWithLine(start2, end2);
            Assert.AreEqual(list3.Count, 1);
            Assert.AreEqual(Math.Round(list3[0].X, 8), 1);
            Assert.AreEqual(Math.Round(list3[0].Y, 8), 0);

            //Case4: 1/4圆（相切）
            List<Point3D> list4 = this.ArcSegment_Part.GetIntersectPointsWithLine(start2, end2);
            Assert.AreEqual(list4.Count, 1);
            Assert.AreEqual(Math.Round(list4[0].X, 8), 1);
            Assert.AreEqual(Math.Round(list4[0].Y, 8), 0);

            Point3D start3 = new Point3D() { X = 2, Y = 0, Z = 0 };
            Point3D end3 = new Point3D() { X = 1, Y = 1, Z = 0 };
            //Case5: 整圆（相离）
            List<Point3D> list5 = null;
            try
            {
                list5 = this.ArcSegment_Full.GetIntersectPointsWithLine(start3, end3);
            }
            catch { }
            finally
            {
                Assert.IsNull(list5);
            }

            //Case6: 1/4圆（相离）
            List<Point3D> list6 = null;
            try
            {
                list6 = this.ArcSegment_Part.GetIntersectPointsWithLine(start3, end3);
            }
            catch { }
            finally
            {
                Assert.IsNull(list6);
            }
        }

        /// <summary>
        /// 直线与圆弧的位置关系
        /// </summary>
        [TestMethod()]
        public void GetRelationShipwithLineTest()
        {
            //Case1: 相离
            Point3D p11 = new Point3D() { X = 1.5, Y = 1, Z = 0 };
            Point3D p12 = new Point3D() { X = 1.4, Y = -1, Z = 0 };
            LineSegment line1 = new LineSegment(p11, p12);
            LineArcRelationShipType type11 = this.ArcSegment_Part.GetRelationShipwithLine(line1);
            Assert.AreEqual(type11, LineArcRelationShipType.Apart);

            LineArcRelationShipType type12 = this.ArcSegment_Full.GetRelationShipwithLine(line1);
            Assert.AreEqual(type12, LineArcRelationShipType.Apart);

            //Case2: 相切（线段）
            Point3D p21 = new Point3D() { X = 1, Y = 1, Z = 0 };
            Point3D p22 = new Point3D() { X = 1, Y = -1, Z = 0 };
            LineSegment line2 = new LineSegment(p21, p22);
            LineArcRelationShipType type21 = this.ArcSegment_Part.GetRelationShipwithLine(line2);
            Assert.AreEqual(type21, LineArcRelationShipType.Tangent);

            LineArcRelationShipType type22 = this.ArcSegment_Full.GetRelationShipwithLine(line2);
            Assert.AreEqual(type22, LineArcRelationShipType.Tangent);

            //Case3: 相切（线段所在的圆）
            Point3D p31 = new Point3D() { X = -1, Y = 1, Z = 0 };
            Point3D p32 = new Point3D() { X = -1, Y = -1, Z = 0 };
            LineSegment line3 = new LineSegment(p31, p32);
            LineArcRelationShipType type31 = this.ArcSegment_Part.GetRelationShipwithLine(line3);
            Assert.AreEqual(type31, LineArcRelationShipType.TangentWithArc);

            //Case4: 相交（线段）
            Point3D p41 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D p42 = new Point3D() { X = 0, Y = 1, Z = 0 };
            LineSegment line4 = new LineSegment(p41, p42);
            LineArcRelationShipType type41 = this.ArcSegment_Part.GetRelationShipwithLine(line4);
            Assert.AreEqual(type41, LineArcRelationShipType.Insert);

            LineArcRelationShipType type42 = this.ArcSegment_Full.GetRelationShipwithLine(line4);
            Assert.AreEqual(type42, LineArcRelationShipType.Insert);

            //Case5: 相交（线段所在的圆）
            Point3D p51 = new Point3D() { X = -1, Y = 0, Z = 0 };
            Point3D p52 = new Point3D() { X = 0, Y = -1, Z = 0 };
            LineSegment line5 = new LineSegment(p51, p52);
            LineArcRelationShipType type51 = this.ArcSegment_Part.GetRelationShipwithLine(line5);
            Assert.AreEqual(type51, LineArcRelationShipType.InsertWithArc);

            //Case6: 相交（线段和线段所在的圆）
            Point3D p61 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D p62 = new Point3D() { X = 0, Y = -1, Z = 0 };
            LineSegment line6 = new LineSegment(p61, p62);
            LineArcRelationShipType type61 = this.ArcSegment_Part.GetRelationShipwithLine(line6);
            Assert.AreEqual(type61, LineArcRelationShipType.InsertWithArcSegmentAndArc);

            Point3D p71 = new Point3D() { X = 0.5, Y = 0.5, Z = 0 };
            Point3D p72 = new Point3D() { X = -0.5, Y = -0.5, Z = 0 };
            LineSegment line7 = new LineSegment(p71, p72);
            LineArcRelationShipType type71 = this.ArcSegment_Part.GetRelationShipwithLine(line7);
            Assert.AreEqual(type71, LineArcRelationShipType.InsertWithArcSegmentAndArc);
        }

        /// <summary>
        /// 点和圆弧位置关系
        /// </summary>
        [TestMethod()]
        public void GetRelationShipWithPointTest()
        {
            //1. 顺时针测试
            GetRelationShipWithPoint_SubTest(this.ArcSegment_Full, this.ArcSegment_Part);

            //2. 逆时针测试
            Point3D center = new Point3D() { X = 0, Y = 0, Z = 0 };
            ArcSegment Full = new ArcSegment(center, 1, Types.ArcDirctionType.UNCLOCK_WISE);
            Point3D end = new Point3D() { X = 0, Y = 1, Z = 0 };
            Point3D start = new Point3D() { X = 1, Y = 0, Z = 0 };
            ArcSegment Part = new ArcSegment(center, start, end, ArcDirctionType.UNCLOCK_WISE);
            GetRelationShipWithPoint_SubTest(Full, Part);
        }

        /// <summary>
        /// 点和圆弧位置关系具体测试方法
        /// </summary>
        /// <param name="Full"></param>
        /// <param name="Part"></param>
        private void GetRelationShipWithPoint_SubTest(ArcSegment Full, ArcSegment Part)
        {
            //Case 1: 点在圆弧上（整圆）
            Point3D point = new Point3D() { X = 1, Y = 0, Z = 0 };
            PointArcRelationShipType type = Full.GetRelationShipWithPoint(point);
            Assert.AreEqual(type, PointArcRelationShipType.On_ArcSegment);

            //Case 2: 点在圆弧上（1/4圆）
            Point3D point2 = new Point3D() { X = 1, Y = 0, Z = 0 };
            PointArcRelationShipType type2 = Part.GetRelationShipWithPoint(point2);
            Assert.AreEqual(type2, PointArcRelationShipType.On_ArcSegment);

            //Case 3: 点在圆弧外（整圆）点在圆弧所在方位
            Point3D point3 = new Point3D() { X = 2, Y = 0, Z = 0 };
            PointArcRelationShipType type3 = Full.GetRelationShipWithPoint(point3);
            Assert.AreEqual(type3, PointArcRelationShipType.To_ArcSegment_Center);

            //Case 4: 点在圆弧外（1/4圆）点在圆弧所在方位
            Point3D point4 = new Point3D() { X = 2, Y = 0, Z = 0 };
            PointArcRelationShipType type4 = Part.GetRelationShipWithPoint(point4);
            Assert.AreEqual(type4, PointArcRelationShipType.To_ArcSegment_Center);

            //Case 5: 点在圆弧所在的圆上，但不在圆弧上（1/4圆适用）
            Point3D point5 = new Point3D() { X = -1, Y = 0, Z = 0 };
            PointArcRelationShipType type5 = Part.GetRelationShipWithPoint(point5);
            Assert.AreEqual(type5, PointArcRelationShipType.On_Arc);

            //Case 6: 点在圆弧所在的圆内，但不在圆弧和圆心之间（1/4圆适用）
            Point3D point6 = new Point3D() { X = -0.1, Y = 0.1, Z = 0 };
            PointArcRelationShipType type6 = Part.GetRelationShipWithPoint(point6);
            Assert.AreEqual(type6, PointArcRelationShipType.In_Arc);

            //Case 7: 点在圆弧与圆心之间（1/4圆）
            Point3D point7 = new Point3D() { X = 0.1, Y = 0.1, Z = 0 };
            PointArcRelationShipType type7 = Part.GetRelationShipWithPoint(point7);
            Assert.AreEqual(type7, PointArcRelationShipType.In_ArcSegment_Center);

            //Case 8: 点在圆弧与圆心之间（整圆）
            Point3D point8 = new Point3D() { X = 0.1, Y = 0.1, Z = 0 };
            PointArcRelationShipType type8 = Full.GetRelationShipWithPoint(point8);
            Assert.AreEqual(type8, PointArcRelationShipType.In_ArcSegment_Center);

            //Case 9: 点在圆弧外，点在圆弧背离的方位(1/4圆)
            Point3D point9 = new Point3D() { X = -2, Y = -2, Z = 0 };
            PointArcRelationShipType type9 = Part.GetRelationShipWithPoint(point9);
            Assert.AreEqual(type9, PointArcRelationShipType.Back_ArcSegment_Center);
        }

        /// <summary>
        /// 用给定点（必须在圆弧上，否则返回圆弧自身）截取圆弧
        /// </summary>
        [TestMethod()]
        public void CutebyPointTest()
        {
            Point3D p1 = new Point3D() { X = 1.01, Y = 0, Z = 0 };
            Point3D p2 = new Point3D() { X = Math.Pow(0.5, 0.5), Y = Math.Pow(0.5, 0.5), Z = 0 };

            //Case1: 点不在圆弧上（整圆）
            ArcSegment result1 = this.ArcSegment_Full.CutebyPoint(p1, true);
            bool same1 = result1.Equals(this.ArcSegment_Full);
            Assert.IsTrue(same1);

            //Case2: 点不在圆弧上（半圆）
            ArcSegment result2 = this.ArcSegment_Part.CutebyPoint(p1, true);
            bool same2 = result2.Equals(this.ArcSegment_Part);
            Assert.IsTrue(same2);

            //Case3: 点在圆弧上第一段（整圆）
            ArcSegment result3 = this.ArcSegment_Full.CutebyPoint(p2, true);
            bool same3 = result3.Equals(this.ArcSegment_Full);
            Assert.IsFalse(same3);
            Assert.AreEqual(result3.BeginPoint.X, 0);
            Assert.AreEqual(result3.BeginPoint.Y, 1);
            Assert.AreEqual(result3.EndPoint.X, Math.Pow(0.5, 0.5));
            Assert.AreEqual(result3.EndPoint.Y, Math.Pow(0.5, 0.5));

            //Case4: 点在圆弧上第一段（半圆）
            ArcSegment result4 = this.ArcSegment_Part.CutebyPoint(p2, true);
            bool same4 = result4.Equals(this.ArcSegment_Part);
            Assert.IsFalse(same4);
            Assert.AreEqual(result4.BeginPoint.X, 0);
            Assert.AreEqual(result4.BeginPoint.Y, 1);
            Assert.AreEqual(result4.EndPoint.X, Math.Pow(0.5, 0.5));
            Assert.AreEqual(result4.EndPoint.Y, Math.Pow(0.5, 0.5));

            //Case5: 点在圆弧上第二段（整圆）
            ArcSegment result5 = this.ArcSegment_Full.CutebyPoint(p2, false);
            bool same5 = result5.Equals(this.ArcSegment_Full);
            Assert.IsFalse(same5);
            Assert.AreEqual(result5.BeginPoint.X, Math.Pow(0.5, 0.5));
            Assert.AreEqual(result5.BeginPoint.Y, Math.Pow(0.5, 0.5));
            Assert.AreEqual(result5.EndPoint.X, 0);
            Assert.AreEqual(result5.EndPoint.Y, 1);

            //Case6: 点在圆弧上第二段（半圆）
            ArcSegment result6 = this.ArcSegment_Part.CutebyPoint(p2, false);
            bool same6 = result6.Equals(this.ArcSegment_Part);
            Assert.IsFalse(same6);
            Assert.AreEqual(result6.BeginPoint.X, Math.Pow(0.5, 0.5));
            Assert.AreEqual(result6.BeginPoint.Y, Math.Pow(0.5, 0.5));
            Assert.AreEqual(result6.EndPoint.X, 1);
            Assert.AreEqual(result6.EndPoint.Y, 0);
        }

        /// <summary>
        /// 用给定点（必须在圆弧上，否则返回圆弧自身）截取圆弧
        /// </summary>
        [TestMethod()]
        public void CuteOutbyPointTest()
        {
            Point3D p1 = new Point3D() { X = 1.01, Y = 0, Z = 0 };
            Point3D p2 = new Point3D() { X = Math.Pow(0.5, 0.5), Y = Math.Pow(0.5, 0.5), Z = 0 };

            //Case1: 点不在圆弧上（整圆）
            List<CurveSegment> result1 = this.ArcSegment_Full.CuteOutbyPoint(p1);
            Assert.AreEqual(result1.Count, 1);
            bool same1 = result1[0].Equals(this.ArcSegment_Full);
            Assert.IsTrue(same1);

            //Case2: 点不在圆弧上（半圆）
            List<CurveSegment> result2 = this.ArcSegment_Part.CuteOutbyPoint(p1);
            Assert.AreEqual(result1.Count, 1);
            bool same2 = result2[0].Equals(this.ArcSegment_Part);
            Assert.IsTrue(same2);

            //Case3: 点在圆弧上（整圆）
            List<CurveSegment> result3 = this.ArcSegment_Full.CuteOutbyPoint(p2);
            Assert.AreEqual(result3.Count, 2);
            Assert.AreEqual((result3[0] as ArcSegment).BeginPoint.X, 0);
            Assert.AreEqual((result3[0] as ArcSegment).BeginPoint.Y, 1);
            Assert.AreEqual((result3[0] as ArcSegment).EndPoint.X, Math.Pow(0.5, 0.5));
            Assert.AreEqual((result3[0] as ArcSegment).EndPoint.Y, Math.Pow(0.5, 0.5));

            Assert.AreEqual((result3[1] as ArcSegment).BeginPoint.X, Math.Pow(0.5, 0.5));
            Assert.AreEqual((result3[1] as ArcSegment).BeginPoint.Y, Math.Pow(0.5, 0.5));
            Assert.AreEqual((result3[1] as ArcSegment).EndPoint.X, 0);
            Assert.AreEqual((result3[1] as ArcSegment).EndPoint.Y, 1);

            //Case4: 点在圆弧上第一段（半圆）
            List<CurveSegment> result4 = this.ArcSegment_Part.CuteOutbyPoint(p2);
            Assert.AreEqual(result4.Count, 2);
            Assert.AreEqual((result4[0] as ArcSegment).BeginPoint.X, 0);
            Assert.AreEqual((result4[0] as ArcSegment).BeginPoint.Y, 1);
            Assert.AreEqual((result4[0] as ArcSegment).EndPoint.X, Math.Pow(0.5, 0.5));
            Assert.AreEqual((result4[0] as ArcSegment).EndPoint.Y, Math.Pow(0.5, 0.5));

            Assert.AreEqual((result4[1] as ArcSegment).BeginPoint.X, Math.Pow(0.5, 0.5));
            Assert.AreEqual((result4[1] as ArcSegment).BeginPoint.Y, Math.Pow(0.5, 0.5));
            Assert.AreEqual((result4[1] as ArcSegment).EndPoint.X, 1);
            Assert.AreEqual((result4[1] as ArcSegment).EndPoint.Y, 0);
        }

        /// <summary>
        /// 给定方向计算圆弧切线
        /// </summary>
        [TestMethod()]
        public void GetTangentPointbyDirTest()
        {
            double dir = Math.PI * 3 / 4;

            //Case1: 整圆
            List<LineSegment> list1 = this.ArcSegment_Full.GetTangentPointbyDir(dir);
            Assert.AreEqual(list1.Count, 2);

            Assert.AreEqual(list1[0].GetDirection(), dir);
            Point3D p = new Point3D() { X = -Math.Pow(0.5, 0.5), Y = -Math.Pow(0.5, 0.5), Z = 0 };
            Assert.AreNotEqual(list1[0].GetPointRelationShip(p), PointLineRelationShipType.Point_Not_On_Line);

            Assert.AreEqual(list1[1].GetDirection(), dir);
            Point3D p2 = new Point3D() { X = Math.Pow(0.5, 0.5), Y = Math.Pow(0.5, 0.5), Z = 0 };
            Assert.AreNotEqual(list1[1].GetPointRelationShip(p2), PointLineRelationShipType.Point_Not_On_Line);

            //Case2: 半圆
            List<LineSegment> list2 = this.ArcSegment_Part.GetTangentPointbyDir(dir);
            Assert.AreEqual(list2.Count, 1);

            Assert.AreEqual(list1[1].GetDirection(), dir);
            Point3D p3 = new Point3D() { X = Math.Pow(0.5, 0.5), Y = Math.Pow(0.5, 0.5), Z = 0 };
            Assert.AreNotEqual(list1[1].GetPointRelationShip(p3), PointLineRelationShipType.Point_Not_On_Line);
        }

        /// <summary>
        /// 给定直线段，计算圆弧和直线段所在直线间的公切圆，并返回满足指定半径的圆弧
        /// </summary>
        [TestMethod()]
        public void GetCommonTangentArcsbyArcandLineTest()
        {
            //Case1: 相交（整圆）
            Point3D p11 = new Point3D() { X = 0, Y = 0.5, Z = 0 };
            Point3D p12 = new Point3D() { X = 2, Y = 0.5, Z = 0 };
            LineSegment line1 = new LineSegment(p11, p12);
            List<ArcSegment> list1 = this.ArcSegment_Full.GetCommonTangentArcsbyArcandLine(line1, 0.1);
            Assert.AreEqual(list1.Count, 8);

            //Case2: 相交（半圆）
            List<ArcSegment> list2 = this.ArcSegment_Part.GetCommonTangentArcsbyArcandLine(line1, 0.1);
            Assert.AreEqual(list2.Count, 4);

            //Case3: 相切（整圆）
            Point3D p21 = new Point3D() { X = 0, Y = 1, Z = 0 };
            Point3D p22 = new Point3D() { X = 2, Y = 1, Z = 0 };
            LineSegment line2 = new LineSegment(p21, p22);
            List<ArcSegment> list3 = this.ArcSegment_Full.GetCommonTangentArcsbyArcandLine(line2, 0.1);
            Assert.AreEqual(list3.Count, 4);

            //Case4: 相切（半圆）
            List<ArcSegment> list4 = this.ArcSegment_Part.GetCommonTangentArcsbyArcandLine(line2, 0.1);
            Assert.AreEqual(list4.Count, 3);

            //Case5: 相离（整圆）(指定的公切圆半径过小)
            Point3D p31 = new Point3D() { X = 0, Y = 2, Z = 0 };
            Point3D p32 = new Point3D() { X = 2, Y = 2, Z = 0 };
            LineSegment line3 = new LineSegment(p31, p32);
            List<ArcSegment> list5 = this.ArcSegment_Full.GetCommonTangentArcsbyArcandLine(line3, 0.1);
            Assert.AreEqual(list5.Count, 0);

            //Case6: 相离（半圆）(指定的公切圆半径过小)
            List<ArcSegment> list6 = this.ArcSegment_Part.GetCommonTangentArcsbyArcandLine(line3, 0.1);
            Assert.AreEqual(list6.Count, 0);

            //Case7: 相离（整圆）(指定的公切圆半径正常)
            List<ArcSegment> list7 = this.ArcSegment_Full.GetCommonTangentArcsbyArcandLine(line3, 0.6);
            Assert.AreEqual(list7.Count, 2);

            //Case8: 相离（半圆）(指定的公切圆半径正常)
            List<ArcSegment> list8 = this.ArcSegment_Part.GetCommonTangentArcsbyArcandLine(line3, 0.6);
            Assert.AreEqual(list8.Count, 1);

            //Case9: 相离特殊情况（整圆）（公切圆正好夹在圆弧和直线之间）
            List<ArcSegment> list9 = this.ArcSegment_Full.GetCommonTangentArcsbyArcandLine(line3, 0.5);
            Assert.AreEqual(list9.Count, 1);

            //Case10: 相离特殊情况（半圆）（公切圆正好夹在圆弧和直线之间）
            List<ArcSegment> list10 = this.ArcSegment_Part.GetCommonTangentArcsbyArcandLine(line3, 0.5);
            Assert.AreEqual(list10.Count, 1);
        }

        /// <summary>
        /// 给定圆弧，计算圆弧与圆弧之间的公切线段集合
        /// </summary>
        [TestMethod()]
        public void GetCommonTangentLinebyArcsTest()
        {
            //Case1: 相离
            ArcSegment other1 = new ArcSegment(new Point3D() { X = 4, Y = 0, Z = 0 }, 2);
            List<LineSegment> list11 = this.ArcSegment_Full.GetCommonTangentLinebyArcs(other1);
            Assert.AreEqual(list11.Count, 2);

            var list12 = this.ArcSegment_Part.GetCommonTangentLinebyArcs(other1);
            Assert.AreEqual(list12.Count, 0);

            //Case2: 圆弧内切
            ArcSegment other2 = new ArcSegment(new Point3D() { X = 4, Y = 0, Z = 0 }, 5);
            List<LineSegment> list21 = this.ArcSegment_Full.GetCommonTangentLinebyArcs(other2);
            Assert.IsNull(list21);

            //Case3: 圆内切，圆弧相离
            ArcSegment other3 = new ArcSegment(new Point3D() { X = 4, Y = 0, Z = 0 }, 5);
            List<LineSegment> list31 = this.ArcSegment_Part.GetCommonTangentLinebyArcs(other3);
            Assert.IsNull(list31);

            //Case4: 圆弧外切
            ArcSegment other4 = new ArcSegment(new Point3D() { X = -4, Y = 0, Z = 0 }, 3);
            List<LineSegment> list41 = this.ArcSegment_Full.GetCommonTangentLinebyArcs(other4);
            Assert.AreEqual(list41.Count, 2);

            List<LineSegment> list42 = this.ArcSegment_Part.GetCommonTangentLinebyArcs(other4);
            Assert.AreEqual(list42.Count, 1);

            //Case5: 圆外切，圆弧相离
            ArcSegment other5 = new ArcSegment(new Point3D() { X = -4, Y = 0, Z = 0 }, new Point3D() { X = -4, Y = 3, Z = 0 }, new Point3D() { X = -4, Y = -3, Z = 0 }, ArcDirctionType.UNCLOCK_WISE);
            List<LineSegment> list51 = this.ArcSegment_Part.GetCommonTangentLinebyArcs(other5);
            Assert.AreEqual(list51.Count, 0);

            //Case6: 圆弧相交（两个交点）
            ArcSegment other6 = new ArcSegment(new Point3D() { X = -3, Y = 0, Z = 0 }, new Point3D() { X = -3, Y = 3, Z = 0 }, new Point3D() { X = -3, Y = 3, Z = 0 }, ArcDirctionType.UNCLOCK_WISE);
            List<LineSegment> list61 = this.ArcSegment_Full.GetCommonTangentLinebyArcs(other6);
            Assert.AreEqual(list61.Count, 2);

            //Case7: 圆弧相交（一个交点）
            ArcSegment other7 = new ArcSegment(new Point3D() { X = -3, Y = 0, Z = 0 }, new Point3D() { X = 0, Y = 0, Z = 0 }, new Point3D() { X = -3, Y = -3, Z = 0 }, ArcDirctionType.UNCLOCK_WISE);
            List<LineSegment> list71 = this.ArcSegment_Full.GetCommonTangentLinebyArcs(other7);
            Assert.AreEqual(list71.Count, 1);

            //Case8: 圆相交，圆弧相离
            ArcSegment other8 = new ArcSegment(new Point3D() { X = -3, Y = 0, Z = 0 }, new Point3D() { X = -3, Y = 3, Z = 0 }, new Point3D() { X = -3, Y = -3, Z = 0 }, ArcDirctionType.UNCLOCK_WISE);
            List<LineSegment> list81 = this.ArcSegment_Part.GetCommonTangentLinebyArcs(other8);
            Assert.AreEqual(list81.Count, 0);
        }

        /// <summary>
        /// 获取传入射线与圆弧的交点
        /// </summary>
        [TestMethod()]
        public void GetCrossPointsTest()
        {
            //Case1: 相离
            Point3D p11 = new Point3D() { X = 1.5, Y = 1, Z = 0 };
            Point3D p12 = new Point3D() { X = 1.4, Y = -1, Z = 0 };
            LineSegment line1 = new LineSegment(p11, p12);
            List<Point3D> list11 = this.ArcSegment_Part.GetCrossPoints(line1) as List<Point3D>;
            Assert.AreEqual(list11.Count, 0);

            List<Point3D> list12 = this.ArcSegment_Full.GetCrossPoints(line1) as List<Point3D>;
            Assert.AreEqual(list12.Count, 0);

            //Case2: 相切（线段）
            /*（1）切点在射线上*/
            Point3D p21 = new Point3D() { X = 1, Y = 1, Z = 0 };
            Point3D p22 = new Point3D() { X = 1, Y = -1, Z = 0 };
            LineSegment line2 = new LineSegment(p21, p22);
            List<Point3D> list21 = this.ArcSegment_Part.GetCrossPoints(line2) as List<Point3D>;
            Assert.AreEqual(list21.Count, 1);

            List<Point3D> list22 = this.ArcSegment_Full.GetCrossPoints(line2) as List<Point3D>;
            Assert.AreEqual(list22.Count, 1);

            /*（2）切点不在射线上*/
            Point3D p21N = new Point3D() { X = 1, Y = -1, Z = 0 };
            Point3D p22N = new Point3D() { X = 1, Y = -2, Z = 0 };
            LineSegment line2N = new LineSegment(p21N, p22N);
            List<Point3D> list21N = this.ArcSegment_Part.GetCrossPoints(line2N) as List<Point3D>;
            Assert.AreEqual(list21N.Count, 0);

            List<Point3D> list22N = this.ArcSegment_Full.GetCrossPoints(line2N) as List<Point3D>;
            Assert.AreEqual(list22N.Count, 0);

            //Case3: 相切（线段所在的圆）
            Point3D p31 = new Point3D() { X = -1, Y = 1, Z = 0 };
            Point3D p32 = new Point3D() { X = -1, Y = -1, Z = 0 };
            LineSegment line3 = new LineSegment(p31, p32);
            List<Point3D> list31 = this.ArcSegment_Part.GetCrossPoints(line3) as List<Point3D>;
            Assert.AreEqual(list31.Count, 0);

            //Case4: 相交（线段）
            Point3D p41 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D p42 = new Point3D() { X = 0, Y = 1, Z = 0 };
            LineSegment line4 = new LineSegment(p41, p42);
            List<Point3D> list41 = this.ArcSegment_Part.GetCrossPoints(line4) as List<Point3D>;
            Assert.AreEqual(list41.Count, 2);

            List<Point3D> list42 = this.ArcSegment_Full.GetCrossPoints(line4) as List<Point3D>;
            Assert.AreEqual(list42.Count, 2);

            //Case5: 相交（线段所在的圆）
            Point3D p51 = new Point3D() { X = -1, Y = 0, Z = 0 };
            Point3D p52 = new Point3D() { X = 0, Y = -1, Z = 0 };
            LineSegment line5 = new LineSegment(p51, p52);
            List<Point3D> type51 = this.ArcSegment_Part.GetCrossPoints(line5) as List<Point3D>;
            Assert.AreEqual(type51.Count, 0);

            //Case6: 相交（线段和线段所在的圆） 
            Point3D p61 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D p62 = new Point3D() { X = 0, Y = -1, Z = 0 };
            LineSegment line6 = new LineSegment(p61, p62);
            List<Point3D> type61 = this.ArcSegment_Part.GetCrossPoints(line6) as List<Point3D>;
            Assert.AreEqual(type61.Count, 1);

            //Case7: 相交（线段和线段所在的圆） 
            Point3D p71 = new Point3D() { X = 0.5, Y = 0.5, Z = 0 };
            Point3D p72 = new Point3D() { X = -0.5, Y = -0.5, Z = 0 };
            LineSegment line7 = new LineSegment(p71, p72);
            List<Point3D> type71 = this.ArcSegment_Part.GetCrossPoints(line7) as List<Point3D>;
            Assert.AreEqual(type71.Count, 0);

            //Case8:: Case7的射线反向
            LineSegment line8 = new LineSegment(p72, p71);
            List<Point3D> type81 = this.ArcSegment_Part.GetCrossPoints(line8) as List<Point3D>;
            Assert.AreEqual(type81.Count, 1);
        }

        /// <summary>
        /// 计算传入的射线和当前圆弧位置关系
        /// </summary>
        [TestMethod()]
        public void GetCurveCurveRelationShipTypeTest()
        {
            //Case1: 相离
            Point3D p11 = new Point3D() { X = 1.5, Y = 1, Z = 0 };
            Point3D p12 = new Point3D() { X = 1.4, Y = -1, Z = 0 };
            LineSegment line1 = new LineSegment(p11, p12);
            LineCurveRelationShipType type11 = this.ArcSegment_Part.GetCurveCurveRelationShipType(line1);
            Assert.AreEqual(type11, LineCurveRelationShipType.Right);

            LineCurveRelationShipType type12 = this.ArcSegment_Full.GetCurveCurveRelationShipType(line1);
            Assert.AreEqual(type12, LineCurveRelationShipType.Right);

            //Case2: 相切（线段）
            Point3D p21 = new Point3D() { X = 1, Y = 1, Z = 0 };
            Point3D p22 = new Point3D() { X = 1, Y = -1, Z = 0 };
            LineSegment line2 = new LineSegment(p21, p22);
            LineCurveRelationShipType type21 = this.ArcSegment_Part.GetCurveCurveRelationShipType(line2);
            Assert.AreEqual(type21, LineCurveRelationShipType.On_Right);

            LineCurveRelationShipType type22 = this.ArcSegment_Full.GetCurveCurveRelationShipType(line2);
            Assert.AreEqual(type22, LineCurveRelationShipType.On_Right);

            //Case3: 相切（线段所在的圆）
            Point3D p31 = new Point3D() { X = -1, Y = 1, Z = 0 };
            Point3D p32 = new Point3D() { X = -1, Y = -1, Z = 0 };
            LineSegment line3 = new LineSegment(p31, p32);
            LineCurveRelationShipType type31 = this.ArcSegment_Part.GetCurveCurveRelationShipType(line3);
            Assert.AreEqual(type31, LineCurveRelationShipType.Left);

            //Case4: 相交（线段）
            Point3D p41 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D p42 = new Point3D() { X = 0, Y = 1, Z = 0 };
            LineSegment line4 = new LineSegment(p41, p42);
            LineCurveRelationShipType type41 = this.ArcSegment_Part.GetCurveCurveRelationShipType(line4);
            Assert.AreEqual(type41, LineCurveRelationShipType.On_Right);

            LineCurveRelationShipType type42 = this.ArcSegment_Full.GetCurveCurveRelationShipType(line4);
            Assert.AreEqual(type42, LineCurveRelationShipType.Intersect);

            //Case5: 相交（线段所在的圆）
            Point3D p51 = new Point3D() { X = -1, Y = 0, Z = 0 };
            Point3D p52 = new Point3D() { X = 0, Y = -1, Z = 0 };
            LineSegment line5 = new LineSegment(p51, p52);
            LineCurveRelationShipType type51 = this.ArcSegment_Part.GetCurveCurveRelationShipType(line5);
            Assert.AreEqual(type51, LineCurveRelationShipType.Left);

            //Case6: 相交（线段和线段所在的圆） 
            Point3D p61 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D p62 = new Point3D() { X = 0, Y = -1, Z = 0 };
            LineSegment line6 = new LineSegment(p61, p62);
            LineCurveRelationShipType type61 = this.ArcSegment_Part.GetCurveCurveRelationShipType(line6);
            Assert.AreEqual(type61, LineCurveRelationShipType.On_Right);

            //Case7: 相交（线段和线段所在的圆） 
            Point3D p71 = new Point3D() { X = 0.5, Y = 0.5, Z = 0 };
            Point3D p72 = new Point3D() { X = -0.5, Y = -0.5, Z = 0 };
            LineSegment line7 = new LineSegment(p71, p72);
            LineCurveRelationShipType type71 = this.ArcSegment_Part.GetCurveCurveRelationShipType(line7);
            Assert.AreEqual(type71, LineCurveRelationShipType.Intersect);
        }

        /// <summary>
        /// 圆弧绕某一点顺时针、逆时针旋转某弧度
        /// </summary>
        [TestMethod()]
        public void RotateTest()
        {
            Init();
            Point3D p = new Point3D() { X = 1, Y = 1, Z = 0 };
            double radian = Math.PI / 2;

            //Case1:整圆顺时针旋转
            this.ArcSegment_Full.Rotate(p, radian, ArcDirctionType.CLOCK_WISE);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.Center.X, 8), 0);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.Center.Y, 8), 2);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.BeginPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.BeginPoint.Y, 8), 2);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.EndPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.EndPoint.Y, 8), 2);

            //Case2:半圆顺时针旋转
            this.ArcSegment_Part.Rotate(p, radian, ArcDirctionType.CLOCK_WISE);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.Center.X, 8), 0);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.Center.Y, 8), 2);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.BeginPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.BeginPoint.Y, 8), 2);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.EndPoint.X, 8), 0);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.EndPoint.Y, 8), 1);

            Init();

            //Case3:整圆逆时针旋转
            this.ArcSegment_Full.Rotate(p, radian, ArcDirctionType.UNCLOCK_WISE);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.Center.X, 8), 2);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.Center.Y, 8), 0);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.BeginPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.BeginPoint.Y, 8), 0);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.EndPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.EndPoint.Y, 8), 0);

            //Case4:半圆逆时针旋转
            this.ArcSegment_Part.Rotate(p, radian, ArcDirctionType.UNCLOCK_WISE);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.Center.X, 8), 2);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.Center.Y, 8), 0);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.BeginPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.BeginPoint.Y, 8), 0);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.EndPoint.X, 8), 2);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.EndPoint.Y, 8), 1);

            Init();
        }

        /// <summary>
        /// 圆弧位移
        /// </summary>
        [TestMethod()]
        public void MoveTest()
        {
            Init();

            //Case1: 整圆位移
            this.ArcSegment_Full.Move(Math.PI / 2, 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.Center.X, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.Center.Y, 8), 0);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.BeginPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.BeginPoint.Y, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.EndPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Full.EndPoint.Y, 8), 1);
            Assert.AreEqual(this.ArcSegment_Full.ArcDirection, ArcDirctionType.CLOCK_WISE);

            //Case2: 半圆位移
            this.ArcSegment_Part.Move(Math.PI / 2, 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.Center.X, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.Center.Y, 8), 0);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.BeginPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.BeginPoint.Y, 8), 1);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.EndPoint.X, 8), 2);
            Assert.AreEqual(Math.Round(this.ArcSegment_Part.EndPoint.Y, 8), 0);
            Assert.AreEqual(this.ArcSegment_Part.ArcDirection, ArcDirctionType.CLOCK_WISE);

            Init();
        }

        /// <summary>
        /// 翻转
        /// </summary>
        [TestMethod()]
        public void ReverseTest()
        {
            Init();

            //Case1: 整圆
            ArcSegment newFull = this.ArcSegment_Full.Reverse() as ArcSegment;
            Assert.AreEqual(Math.Round(newFull.Center.X, 8), 0);
            Assert.AreEqual(Math.Round(newFull.Center.Y, 8), 0);
            Assert.AreEqual(Math.Round(newFull.BeginPoint.X, 8), 0);
            Assert.AreEqual(Math.Round(newFull.BeginPoint.Y, 8), 1);
            Assert.AreEqual(Math.Round(newFull.EndPoint.X, 8), 0);
            Assert.AreEqual(Math.Round(newFull.EndPoint.Y, 8), 1);
            Assert.AreEqual(newFull.ArcDirection, ArcDirctionType.UNCLOCK_WISE);

            //Case2: 半圆
            ArcSegment newPart = this.ArcSegment_Part.Reverse() as ArcSegment;
            Assert.AreEqual(Math.Round(newPart.Center.X, 8), 0);
            Assert.AreEqual(Math.Round(newPart.Center.Y, 8), 0);
            Assert.AreEqual(Math.Round(newPart.BeginPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(newPart.BeginPoint.Y, 8), 0);
            Assert.AreEqual(Math.Round(newPart.EndPoint.X, 8), 0);
            Assert.AreEqual(Math.Round(newPart.EndPoint.Y, 8), 1);
            Assert.AreEqual(newPart.ArcDirection, ArcDirctionType.UNCLOCK_WISE);
        }

        /// <summary>
        /// 计算圆与圆的位置关系(圆弧所在的圆的位置关系)
        /// </summary>
        [TestMethod()]
        public void GetRelationShipWithArcTest()
        {
            //Case1:相离
            ArcSegment other1 = new ArcSegment(new Point3D() { X = 4, Y = 0, Z = 0 }, 2);
            ArcSegmentArcRelationShipType result11 = this.ArcSegment_Full.GetRelationShipWithArc(other1);
            Assert.AreEqual(result11, ArcSegmentArcRelationShipType.Apart);

            ArcSegmentArcRelationShipType result12 = this.ArcSegment_Part.GetRelationShipWithArc(other1);
            Assert.AreEqual(result12, ArcSegmentArcRelationShipType.Apart);

            //Case2: 内切
            ArcSegment other2 = new ArcSegment(new Point3D() { X = 4, Y = 0, Z = 0 }, 5);
            ArcSegmentArcRelationShipType result21 = this.ArcSegment_Full.GetRelationShipWithArc(other2);
            Assert.AreEqual(result21, ArcSegmentArcRelationShipType.InnerTangent);

            ArcSegmentArcRelationShipType result22 = this.ArcSegment_Part.GetRelationShipWithArc(other2);
            Assert.AreEqual(result22, ArcSegmentArcRelationShipType.InnerTangent);

            //Case3: 外切
            ArcSegment other3 = new ArcSegment(new Point3D() { X = 4, Y = 0, Z = 0 }, 3);
            ArcSegmentArcRelationShipType result31 = this.ArcSegment_Full.GetRelationShipWithArc(other3);
            Assert.AreEqual(result31, ArcSegmentArcRelationShipType.OuterTangent);

            ArcSegmentArcRelationShipType result32 = this.ArcSegment_Part.GetRelationShipWithArc(other3);
            Assert.AreEqual(result32, ArcSegmentArcRelationShipType.OuterTangent);

            //Case4: 相交
            ArcSegment other4 = new ArcSegment(new Point3D() { X = 4, Y = 0, Z = 0 }, 4);
            ArcSegmentArcRelationShipType result41 = this.ArcSegment_Full.GetRelationShipWithArc(other4);
            Assert.AreEqual(result41, ArcSegmentArcRelationShipType.Insert);

            ArcSegmentArcRelationShipType result42 = this.ArcSegment_Part.GetRelationShipWithArc(other4);
            Assert.AreEqual(result42, ArcSegmentArcRelationShipType.Insert);
        }

        /// <summary>
        /// Equals测试
        /// </summary>
        [TestMethod()]
        public void EqualsTest()
        {
            //Test1
            var result1 = this.ArcSegment_Full.Equals(this.ArcSegment_Part);
            Assert.AreEqual(result1, false);

            //Test2
            Point3D center = new Point3D() { X = 0, Y = 0, Z = 0 };
            ArcSegment full = new ArcSegment(center, 1, Types.ArcDirctionType.CLOCK_WISE);
            var result2 = this.ArcSegment_Full.Equals(full);
            Assert.AreEqual(result2, true);

            //Test3
            Point3D start = new Point3D() { X = 0, Y = 1, Z = 0 };
            Point3D end = new Point3D() { X = 1, Y = 0, Z = 0 };
            ArcSegment part = new ArcSegment(center, start, end, ArcDirctionType.CLOCK_WISE);
            var result3 = this.ArcSegment_Part.Equals(part);
            Assert.AreEqual(result3, true);
        }

        /// <summary>
        /// Get HashCode测试
        /// </summary>
        [TestMethod()]
        public void GetHashCodeTest()
        {
            //Test1
            var code1 = this.ArcSegment_Full.GetHashCode();
            var code2 = this.ArcSegment_Part.GetHashCode();
            Assert.AreNotEqual(code1, code2);

            //Test2
            Point3D center = new Point3D() { X = 0, Y = 0, Z = 0 };
            ArcSegment full = new ArcSegment(center, 1, Types.ArcDirctionType.CLOCK_WISE);
            var code3 = full.GetHashCode();
            Assert.AreEqual(code1, code3);

            //Test3
            Point3D start = new Point3D() { X = 0, Y = 1, Z = 0 };
            Point3D end = new Point3D() { X = 1, Y = 0, Z = 0 };
            ArcSegment part = new ArcSegment(center, start, end, ArcDirctionType.CLOCK_WISE);
            var code4 = part.GetHashCode();
            Assert.AreEqual(code2, code4);
        }

        /// <summary>
        /// ToString测试
        /// </summary>
        [TestMethod()]
        public void ToStringTest()
        {
            string str = this.ArcSegment_Part.ToString();
            Assert.AreEqual(str, "0,0,0;0,1,0;1,0,0;CLOCK_WISE");
        }

        /// <summary>
        /// 判断点是否在圆弧线段上
        /// </summary>
        [TestMethod()]
        public void PointIsOnCurveSegmentTest()
        {
            Point3D point = new Point3D() { X = 1, Y = 0, Z = 0 };
            bool result1 = this.ArcSegment_Full.PointIsOnCurveSegment(point);
            Assert.AreEqual(result1, true);

            Point3D point2 = new Point3D() { X = 1, Y = 0, Z = 0 };
            bool result2 = this.ArcSegment_Part.PointIsOnCurveSegment(point2);
            Assert.AreEqual(result2, true);

            Point3D point3 = new Point3D() { X = 2, Y = 2, Z = 0 };
            bool result3 = this.ArcSegment_Full.PointIsOnCurveSegment(point3);
            Assert.AreEqual(result3, false);

            Point3D point4 = new Point3D() { X = -1, Y = 0, Z = 0 };
            bool result4 = this.ArcSegment_Part.PointIsOnCurveSegment(point4);
            Assert.AreEqual(result4, false);
        }
    }
}
