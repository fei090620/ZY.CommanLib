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
    public class CurveTests
    {
        Curve Curve = null;
        ArcSegment Arc1 = null;
        LineSegment Line1 = null;
        ArcSegment Arc2 = null;
        LineSegment Line2 = null;

        Curve Curve2 = null;
        ArcSegment Arc22 = null;
        LineSegment Line22 = null;

        LineSegment AddLine_End = null;
        LineSegment AddLine_Begin = null;

        [TestInitialize]
        public void Init()
        {
            //Curve1 
            List<CurveSegment> list = new List<CurveSegment>();

            Point3D center = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D start = new Point3D() { X = Math.Pow(0.5, 0.5), Y = Math.Pow(0.5, 0.5), Z = 0 };
            Point3D end = new Point3D() { X = 1, Y = 0, Z = 0 };
            this.Arc1 = new ArcSegment(center, start, end, ArcDirctionType.CLOCK_WISE);

            Point3D start1 = new Point3D() { X = 1, Y = 0, Z = 0 };
            Point3D end1 = new Point3D() { X = 0, Y = -1, Z = 0 };
            this.Line1 = new LineSegment(start1, end1);

            Point3D center2 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D start2 = new Point3D() { X = 0, Y = -1, Z = 0 };
            Point3D end2 = new Point3D() { X = -1, Y = 0, Z = 0 };
            this.Arc2 = new ArcSegment(center2, start2, end2, ArcDirctionType.CLOCK_WISE);

            Point3D start3 = new Point3D() { X = -1, Y = 0, Z = 0 };
            Point3D end3 = new Point3D() { X = 0, Y = 1, Z = 0 };
            this.Line2 = new LineSegment(start3, end3);

            list.Add(this.Arc1);
            list.Add(this.Line1);
            list.Add(this.Arc2);
            list.Add(this.Line2);

            Curve = new Curve(list);

            //Curve2
            List<CurveSegment> list2 = new List<CurveSegment>();

            Point3D center21 = new Point3D() { X = 0, Y = 3, Z = 0 };
            Point3D start21 = new Point3D() { X = 0, Y = 2, Z = 0 };
            Point3D end21 = new Point3D() { X = -1, Y = 3, Z = 0 };
            this.Arc22 = new ArcSegment(center21, start21, end21, ArcDirctionType.CLOCK_WISE);

            Point3D start22 = new Point3D() { X = -1, Y = 3, Z = 0 };
            Point3D end22 = new Point3D() { X = -1, Y = 4, Z = 0 };
            this.Line22 = new LineSegment(start22, end22);

            list2.Add(this.Arc22);
            list2.Add(this.Line22);

            this.Curve2 = new Curve(list2);

            //算法自动创建的线段
            //1.第一个曲线集合之后接另一个曲线集合或曲线段
            this.AddLine_End = new LineSegment(new Point3D() { X = 0, Y = 1, Z = 0 }, new Point3D() { X = 0, Y = 2, Z = 0 });

            //2.第一个曲线集合之前接另一个曲线集合或曲线段
            this.AddLine_Begin = new LineSegment(new Point3D() { X = -1, Y = 4, Z = 0 }, new Point3D() { X = Math.Pow(0.5, 0.5), Y = Math.Pow(0.5, 0.5), Z = 0 });
        }

        /// <summary>
        /// 根据精度取点列
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public void ToListTest()
        {
            List<Point3D> list = this.Curve2.ToList(0.1);
            List<Point3D> subList1 = this.Arc22.ToList(0.1) as List<Point3D>;
            List<Point3D> subList2 = this.Line22.ToList(0.1) as List<Point3D>;
            Assert.AreEqual(list.Count, subList1.Count + subList2.Count);//包含了重合相交的点
        }

        /// <summary>
        /// 获取第一个直线段
        /// </summary>
        [TestMethod()]
        public void GetFirstLineSegmentTest()
        {
            LineSegment getLine = this.Curve.GetFirstLineSegment();
            Assert.IsTrue(getLine.Equals(this.Line1));
        }

        /// <summary>
        /// 获取第一个圆弧段
        /// </summary>
        [TestMethod()]
        public void GetFirstArcSegmentTest()
        {
            ArcSegment getArc = this.Curve.GetFirstArcSegment();
            Assert.IsTrue(getArc.Equals(this.Arc1));
        }

        /// <summary>
        /// 获取最后一个直线段
        /// </summary>
        [TestMethod()]
        public void GetLastLineSegmentTest()
        {
            LineSegment getLine = this.Curve.GetLastLineSegment();
            Assert.IsTrue(getLine.Equals(this.Line2));
        }

        /// <summary>
        /// 获取最后一个圆弧段
        /// </summary>
        [TestMethod()]
        public void GetLastArcSegmentTest()
        {
            ArcSegment getArc = this.Curve.GetLastArcSegment();
            Assert.IsTrue(getArc.Equals(this.Arc2));
        }

        /// <summary>
        /// 获取起始点方向（直线段的方向or圆弧中心点到起点的方向）
        /// </summary>
        [TestMethod()]
        public void GetStartTangentRadianTest()
        {
            double? radian = this.Curve.GetStartTangentRadian();
            Assert.AreEqual(radian, Math.PI / 4);
        }

        /// <summary>
        /// 获取起始点方向（直线段的方向or圆弧中心点到起点的方向）
        /// </summary>
        [TestMethod()]
        public void GetEndTangentRadianTest()
        {
            double? radian = this.Curve.GetEndTangentRadian();
            Assert.AreEqual(radian, Math.PI / 4);
        }

        /// <summary>
        /// 在曲线的后端连接另一条曲线
        /// （如果末尾端点和另一条曲线的首点不是同一个点，则用直线段连接）
        /// </summary>
        [TestMethod()]
        public void ConnectBackWithTest()
        {
            Curve result = this.Curve.ConnectBackWith(this.Curve2);
            Assert.AreEqual(result.Tracks.Count, 7);
            Assert.IsTrue(result.Tracks[4].Equals(this.AddLine_End));
        }

        /// <summary>
        /// 在曲线的后端连接另一条曲线段
        /// （如果末尾端点和另一条曲线的首点不是同一个点，则用直线段连接）
        /// </summary>
        [TestMethod()]
        public void ConnectBackWithTest1()
        {
            Curve result = this.Curve.ConnectBackWith(this.Arc22);
            Assert.AreEqual(result.Tracks.Count, 6);
            Assert.IsTrue(result.Tracks[4].Equals(this.AddLine_End));
        }

        /// <summary>
        /// 在曲线的前端连接另一条曲线
        /// （如果起始端点和另一条曲线的末尾点不是同一个点，则用直线段连接）
        /// </summary>
        [TestMethod()]
        public void ConnectFrontWithTest()
        {
            Curve result = this.Curve.ConnectFrontWith(this.Curve2);
            Assert.AreEqual(result.Tracks.Count, 7);
            Assert.IsTrue(result.Tracks[2].Equals(this.AddLine_Begin));
        }

        /// <summary>
        /// 在曲线的前端连接另一条曲线段
        /// （如果起始端点和另一条曲线段的末尾点不是同一个点，则用直线段连接）
        /// </summary>
        [TestMethod()]
        public void ConnectFrontWithTest1()
        {
            Curve result = this.Curve.ConnectFrontWith(this.Line22);
            Assert.AreEqual(result.Tracks.Count, 6);
            Assert.IsTrue(result.Tracks[1].Equals(this.AddLine_Begin));
        }

        /// <summary>
        /// 翻转曲线（曲线段顺序翻转，曲线段方向翻转）
        /// </summary>
        [TestMethod()]
        public void ReverseTest()
        {
            Curve newCurve = this.Curve2.Reverse();
            Assert.AreNotEqual(this.Curve2.GetHashCode(), newCurve.GetHashCode());
            Assert.IsTrue(newCurve.Tracks[0].GetStartPoint().Equals(this.Line22.EndPoint));
            Assert.IsTrue(newCurve.Tracks[1].GetEndPoint().Equals(this.Arc22.BeginPoint));
        }
    }
}
