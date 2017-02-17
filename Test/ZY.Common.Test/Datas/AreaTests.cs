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
    public class AreaTests
    {
        Area Area = null;
        Curve Outer = null;
        Curve Inner = null;

        /// <summary>
        ///  创建测试
        /// </summary>
        [TestInitialize]
        public void CreateNewTest()
        {
            List<CurveSegment> list = new List<CurveSegment>();
            List<CurveSegment> list2 = new List<CurveSegment>();

            Point3D center = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D start = new Point3D() { X = 0, Y = 1, Z = 0 };
            Point3D end = new Point3D() { X = 1, Y = 0, Z = 0 };
            ArcSegment arc1 = new ArcSegment(center, start, end, ArcDirctionType.CLOCK_WISE);

            Point3D start1 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D end1 = new Point3D() { X = 2, Y = 0, Z = 0 };
            LineSegment line1 = new LineSegment(start1, end1);

            Point3D start2 = new Point3D() { X = 2, Y = 0, Z = 0 };
            Point3D end2 = new Point3D() { X = 2, Y = -1, Z = 0 };
            LineSegment line2 = new LineSegment(start2, end2);

            Point3D start3 = new Point3D() { X = 2, Y = -1, Z = 0 };
            Point3D end3 = new Point3D() { X = 0, Y = -1, Z = 0 };
            LineSegment line3 = new LineSegment(start3, end3);

            Point3D center4 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D start4 = new Point3D() { X = 0, Y = -1, Z = 0 };
            Point3D end4 = new Point3D() { X = -1, Y = 0, Z = 0 };
            ArcSegment arc4 = new ArcSegment(center4, start4, end4, ArcDirctionType.CLOCK_WISE);

            Point3D start5 = new Point3D() { X = -1, Y = 0, Z = 0 };
            Point3D end5 = new Point3D() { X = -1, Y = 1, Z = 0 };
            LineSegment line5 = new LineSegment(start5, end5);

            Point3D start6 = new Point3D() { X = -1, Y = 1, Z = 0 };
            Point3D end6 = new Point3D() { X = 0, Y = 1, Z = 0 };
            LineSegment line6 = new LineSegment(start6, end6);

            list.Add(arc1);
            list.Add(line1);
            list.Add(line2);
            list.Add(line3);
            list.Add(arc4);
            list.Add(line5);
            list.Add(line6);
            this.Outer = new Curve(list);


            Point3D start21 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D end21 = new Point3D() { X = 0, Y = 0.5, Z = 0 };
            LineSegment line21 = new LineSegment(start21, end21);

            Point3D start22 = new Point3D() { X = 0, Y = 0.5, Z = 0 };
            Point3D end22 = new Point3D() { X = -0.5, Y = 0.5, Z = 0 };
            LineSegment line22 = new LineSegment(start22, end22);

            Point3D start23 = new Point3D() { X = -0.5, Y = 0.5, Z = 0 };
            Point3D end23 = new Point3D() { X = -0.5, Y = 0, Z = 0 };
            LineSegment line23 = new LineSegment(start23, end23);

            Point3D start24 = new Point3D() { X = -0.5, Y = 0, Z = 0 };
            Point3D end24 = new Point3D() { X = 0, Y = 0, Z = 0 };
            LineSegment line24 = new LineSegment(start24, end24);

            list2.Add(line21);
            list2.Add(line22);
            list2.Add(line23);
            list2.Add(line24);
            this.Inner = new Curve(list2);

            this.Area = Area.CreateNew(this.Outer, this.Inner);

            Assert.AreNotEqual(this.Area, null);
        }

        /// <summary>
        /// 通过两条同向直线段构造
        /// （首点与首点连接，末尾点与末尾点链接）区域的方法（区域为矩形或平行四边形）
        /// </summary>
        [TestMethod()]
        public void CreateNewBySameDirectionLinesTest()
        {
            Point3D start1 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D end1 = new Point3D() { X = 2, Y = 0, Z = 0 };
            LineSegment line1 = new LineSegment(start1, end1);

            Point3D start2 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D end2 = new Point3D() { X = 3, Y = -1, Z = 0 };
            LineSegment line2 = new LineSegment(start2, end2);

            Area area = Area.CreateNewBySameDirectionLines(line1, line2);

            Assert.AreEqual(area.OutterLine.Tracks.Count, 4);
            Assert.AreEqual(area.OutterLine.Tracks[1].GetStartPoint().X, 2);
            Assert.AreEqual(area.OutterLine.Tracks[1].GetStartPoint().Y, 0);
            Assert.AreEqual(area.OutterLine.Tracks[1].GetEndPoint().X, 3);
            Assert.AreEqual(area.OutterLine.Tracks[1].GetEndPoint().Y, -1);
            Assert.IsTrue(area.OutterLine.Tracks[2].Equals(line2.Reverse()));
            Assert.AreEqual(area.OutterLine.Tracks[3].GetStartPoint().X, 0);
            Assert.AreEqual(area.OutterLine.Tracks[3].GetStartPoint().Y, 0);
            Assert.AreEqual(area.OutterLine.Tracks[3].GetEndPoint().Y, 0);
        }

        /// <summary>
        /// 通过两条反向直线段构造
        /// （首点与末尾点连接，末尾点与首点链接）区域的方法（区域为矩形或平行四边形）
        /// </summary>
        [TestMethod()]
        public void CreateNewByInverseDirectionLinesTest()
        {
            Point3D start1 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D end1 = new Point3D() { X = 2, Y = 0, Z = 0 };
            LineSegment line1 = new LineSegment(start1, end1);

            Point3D start2 = new Point3D() { X = 3, Y = -1, Z = 0 };
            Point3D end2 = new Point3D() { X = 0, Y = 0, Z = 0 };
            LineSegment line2 = new LineSegment(start2, end2);

            Area area = Area.CreateNewByInverseDirectionLines(line1, line2);

            Assert.AreEqual(area.OutterLine.Tracks.Count, 4);
            Assert.AreEqual(area.OutterLine.Tracks[1].GetStartPoint().X, 2);
            Assert.AreEqual(area.OutterLine.Tracks[1].GetStartPoint().Y, 0);
            Assert.AreEqual(area.OutterLine.Tracks[1].GetEndPoint().X, 3);
            Assert.AreEqual(area.OutterLine.Tracks[1].GetEndPoint().Y, -1);
            Assert.IsTrue(area.OutterLine.Tracks[2].Equals(line2));
            Assert.AreEqual(area.OutterLine.Tracks[3].GetStartPoint().X, 0);
            Assert.AreEqual(area.OutterLine.Tracks[3].GetStartPoint().Y, 0);
            Assert.AreEqual(area.OutterLine.Tracks[3].GetEndPoint().Y, 0);
        }

        /// <summary>
        /// 获取点集合中属于区域内的点的集合
        /// </summary>
        [TestMethod()]
        public void GetIncludedPointsTest()
        {
            List<Point3D> points = new List<Point3D>();
            points.Add(new Point3D() { X = 0, Y = 0, Z = 0 });
            points.Add(new Point3D() { X = 0.5, Y = 0, Z = 0 });
            points.Add(new Point3D() { X = 2.5, Y = 0, Z = 0 });
            points.Add(new Point3D() { X = -0.4, Y = 0.001, Z = 0 });
            points.Add(new Point3D() { X = -0.5, Y = 0, Z = 0 });
            points.Add(new Point3D() { X = -0.6, Y = 0, Z = 0 });
            points.Add(new Point3D() { X = -1, Y = 0, Z = 0 });
            points.Add(new Point3D() { X = -1.5, Y = 0, Z = 0 });
            points.Add(new Point3D() { X = 1.5, Y = 0, Z = 0 });
            points.Add(new Point3D() { X = -0.5, Y = 0.5, Z = 0 });
            points.Add(new Point3D() { X = 3, Y = 3, Z = 0 });
            points.Add(new Point3D() { X = -0.1, Y = 0.5, Z = 0 });
            List<Point3D> getList = this.Area.GetIncludedPoints(points) as List<Point3D>;
            Assert.AreEqual(getList.Count, 8);
        }

        /// <summary>
        /// 点是否在区域内判断
        /// </summary>
        [TestMethod()]
        public void IsIncludeTest()
        {
            IncludeTest_SubMethod(0, 0, true);

            IncludeTest_SubMethod(0.5, 0, true);

            IncludeTest_SubMethod(2.5, 0, false);

            IncludeTest_SubMethod(-0.4, 0.001, false);

            IncludeTest_SubMethod(-0.5, 0, true);

            IncludeTest_SubMethod(-0.6, 0, true);

            IncludeTest_SubMethod(-1, 0, true);

            IncludeTest_SubMethod(-1.5, 0, false);

            IncludeTest_SubMethod(1.5, 0, true);

            IncludeTest_SubMethod(-0.5, 0.5, true);

            IncludeTest_SubMethod(3, 3, false);

            IncludeTest_SubMethod(-0.1, 0.5, true);

        }

        private void IncludeTest_SubMethod(double x, double y, bool result)
        {
            Point3D testPoint = new Point3D() { X = x, Y = y, Z = 0 };
            bool r = this.Area.IsInclude(testPoint);
            Assert.AreEqual(r, result);
        }
    }
}
