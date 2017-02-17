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
    public class CurveSegmentTests
    {
        ArcSegment ArcSegment = null;
        LineSegment Line = null;

        [TestInitialize]
        public void Init()
        {
            Point3D center = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D start = new Point3D() { X = 0, Y = 1, Z = 0 };
            Point3D end = new Point3D() { X = 1, Y = 0, Z = 0 };
            this.ArcSegment = new ArcSegment(center, start, end, ArcDirctionType.CLOCK_WISE);

            Point3D start1 = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D end1 = new Point3D() { X = 1, Y = 0, Z = 0 };
            this.Line = new LineSegment(start1, end1);
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void ToListTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// 拷贝，并平移拷贝的曲线段
        /// </summary>
        [TestMethod()]
        public void CopyMoveTest()
        {
            //Case1:直线段
            LineSegment newLine = this.Line.CopyMove(Math.PI / 2, 1) as LineSegment;
            Assert.AreNotEqual(this.Line.GetHashCode(), newLine.GetHashCode());
            Assert.AreEqual(Math.Round(newLine.BeginPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(newLine.BeginPoint.Y, 8), 0);
            Assert.AreEqual(Math.Round(newLine.EndPoint.X, 8), 2);
            Assert.AreEqual(Math.Round(newLine.EndPoint.Y, 8), 0);

            //Case2:圆弧
            ArcSegment newArc = this.ArcSegment.CopyMove(Math.PI / 2, 1) as ArcSegment;
            Assert.AreNotEqual(this.ArcSegment.GetHashCode(), newArc.GetHashCode());
            Assert.AreEqual(Math.Round(newArc.BeginPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(newArc.BeginPoint.Y, 8), 1);
            Assert.AreEqual(Math.Round(newArc.EndPoint.X, 8), 2);
            Assert.AreEqual(Math.Round(newArc.EndPoint.Y, 8), 0);
            Assert.AreEqual(newArc.ArcDirection, ArcDirctionType.CLOCK_WISE);
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void MoveTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// 拷贝，并将拷贝的线段绕某一点旋转一定弧度（顺时针或逆时针）
        /// </summary>
        [TestMethod()]
        public void CopyRotateTest()
        {
            Point3D referencePoint = new Point3D() { X = 1, Y = 1, Z = 0 };
            //Case1:直线段
            LineSegment newLine = this.Line.CopyRotate(referencePoint, Math.PI / 2, ArcDirctionType.CLOCK_WISE) as LineSegment;
            Assert.AreNotEqual(this.Line.GetHashCode(), newLine.GetHashCode());
            Assert.AreEqual(Math.Round(newLine.BeginPoint.X, 8), 0);
            Assert.AreEqual(Math.Round(newLine.BeginPoint.Y, 8), 2);
            Assert.AreEqual(Math.Round(newLine.EndPoint.X, 8), 0);
            Assert.AreEqual(Math.Round(newLine.EndPoint.Y, 8), 1);

            //Case2:圆弧
            ArcSegment newArc = this.ArcSegment.CopyRotate(referencePoint, Math.PI / 2, ArcDirctionType.CLOCK_WISE) as ArcSegment;
            Assert.AreNotEqual(this.ArcSegment.GetHashCode(), newArc.GetHashCode());
            Assert.AreEqual(Math.Round(newArc.BeginPoint.X, 8), 1);
            Assert.AreEqual(Math.Round(newArc.BeginPoint.Y, 8), 2);
            Assert.AreEqual(Math.Round(newArc.EndPoint.X, 8), 0);
            Assert.AreEqual(Math.Round(newArc.EndPoint.Y, 8), 1);
            Assert.AreEqual(newArc.ArcDirection, ArcDirctionType.CLOCK_WISE);
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void RotateTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// 根据曲线段上的一点截取点之前的曲线段,点不在曲线上则返回曲线本身
        /// </summary>
        [TestMethod()]
        public void CutOutPreviousParagraphbyPointTest()
        {
            //Case1:直线段

            //点在线外 //返回自身
            Point3D other = new Point3D() { X = 3, Y = 0, Z = 0 };
            CurveSegment result1 = this.Line.CutOutPreviousParagraphbyPoint(other);
            Assert.IsTrue(this.Line.Equals(result1));

            //点在线上
            Point3D other2 = new Point3D() { X = 0.5, Y = 0, Z = 0 };
            CurveSegment result2 = this.Line.CutOutPreviousParagraphbyPoint(other2);
            Assert.IsNotNull(result2);
            Assert.AreEqual((result2 as LineSegment).BeginPoint.X, 0);
            Assert.AreEqual((result2 as LineSegment).BeginPoint.Y, 0);
            Assert.AreEqual((result2 as LineSegment).EndPoint.X, 0.5);
            Assert.AreEqual((result2 as LineSegment).EndPoint.Y, 0);

            //Case2:圆弧

            //点在弧外 //返回自身
            CurveSegment result3 = this.ArcSegment.CutOutPreviousParagraphbyPoint(other);
            Assert.IsTrue(this.ArcSegment.Equals(result3));

            //点在弧上
            Point3D other3 = new Point3D() { X = Math.Pow(0.5, 0.5), Y = Math.Pow(0.5, 0.5), Z = 0 };
            CurveSegment result4 = this.ArcSegment.CutOutPreviousParagraphbyPoint(other3);
            Assert.IsNotNull(result4);
            Assert.AreEqual((result4 as ArcSegment).BeginPoint.X, 0);
            Assert.AreEqual((result4 as ArcSegment).BeginPoint.Y, 1);
            Assert.AreEqual((result4 as ArcSegment).EndPoint.X, Math.Pow(0.5, 0.5));
            Assert.AreEqual((result4 as ArcSegment).EndPoint.Y, Math.Pow(0.5, 0.5));
            Assert.AreEqual((result4 as ArcSegment).ArcDirection, ArcDirctionType.CLOCK_WISE);
        }

        /// <summary>
        /// 根据曲线段上的一点截取点之后的曲线段,点不在曲线上则返回曲线本身
        /// </summary>
        [TestMethod()]
        public void CuteOutAfterParagraphbyPointTest()
        {
            //Case1:直线段

            //点在线外 //返回自身
            Point3D other = new Point3D() { X = 3, Y = 0, Z = 0 };
            CurveSegment result1 = this.Line.CuteOutAfterParagraphbyPoint(other);
            Assert.IsTrue(this.Line.Equals(result1));

            //点在线上
            Point3D other2 = new Point3D() { X = 0.5, Y = 0, Z = 0 };
            CurveSegment result2 = this.Line.CuteOutAfterParagraphbyPoint(other2);
            Assert.IsNotNull(result2);
            Assert.AreEqual((result2 as LineSegment).BeginPoint.X, 0.5);
            Assert.AreEqual((result2 as LineSegment).BeginPoint.Y, 0);
            Assert.AreEqual((result2 as LineSegment).EndPoint.X, 1);
            Assert.AreEqual((result2 as LineSegment).EndPoint.Y, 0);

            //Case2:圆弧

            //点在弧外 //返回自身
            CurveSegment result3 = this.ArcSegment.CuteOutAfterParagraphbyPoint(other);
            Assert.IsTrue(this.ArcSegment.Equals(result3));

            //点在弧上
            Point3D other3 = new Point3D() { X = Math.Pow(0.5, 0.5), Y = Math.Pow(0.5, 0.5), Z = 0 };
            CurveSegment result4 = this.ArcSegment.CuteOutAfterParagraphbyPoint(other3);
            Assert.IsNotNull(result4);
            Assert.AreEqual((result4 as ArcSegment).BeginPoint.X, Math.Pow(0.5, 0.5));
            Assert.AreEqual((result4 as ArcSegment).BeginPoint.Y, Math.Pow(0.5, 0.5));
            Assert.AreEqual((result4 as ArcSegment).EndPoint.X, 1);
            Assert.AreEqual((result4 as ArcSegment).EndPoint.Y, 0);
            Assert.AreEqual((result4 as ArcSegment).ArcDirection, ArcDirctionType.CLOCK_WISE);
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void CuteOutbyPointTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void GetCurveCurveRelationShipTypeTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void GetCrossPointsTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void GetStartCourseTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void GetEndCourseTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void GetStartPointTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void ReverseTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void GetEndPointTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void EqualsTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void GetHashCodeTest()
        {
            //Abstract Method
        }

        /// <summary>
        /// Abstract Method
        /// </summary>
        [TestMethod()]
        public void ToStringTest()
        {
            //Abstract Method
        }
    }
}
