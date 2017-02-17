using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Common.Datas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ZY.Common.Datas.Tests
{
    [TestClass()]
    public class Point3DTests
    {
        #region 构造函数
        [TestMethod()]
        public void Point3DTest()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void Point3DTest1()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void Point3DTest2()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void Point3DTest3()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void Point3DTest4()
        {
            //Assert.Fail();
        }
        #endregion

        [TestMethod()]
        public void GetNextPointTest()
        {
            Point3D point = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D result = point.GetNextPoint(Math.PI / 2, 1);

            Assert.AreEqual(result.X, 1);
            Assert.AreEqual(Math.Round(result.Y, 8), 0);
            Assert.AreEqual(result.Z, 0);

            //DateTime startTime = DateTime.Now;

            //for (int i = 0; i < 1; i++)
            //{

            //    Point3D point = new Point3D() { X = double.MaxValue / 2, Y = double.MaxValue / 3, Z = double.MaxValue / 4 };
            //    Point3D result = point.GetNextPoint(double.MaxValue / 5, double.MaxValue / 10);
            //}
            ////Assert.AreEqual(result.X, 1);
            ////Assert.AreEqual(Math.Round(result.Y, 8), 0);
            ////Assert.AreEqual(result.Z, 0);
            //DateTime endTime = DateTime.Now;

            //Debug.WriteLine((endTime - startTime).TotalMilliseconds);
        }

        [TestMethod()]
        public void DisToTest()
        {
            Point3D point = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D endPoint = new Point3D() { X = 1, Y = 1, Z = 1 };
            var result = point.DisTo(endPoint);
            Assert.AreEqual(result, Math.Pow(3, 0.5)); //包括Z轴 
        }

        [TestMethod()]
        public void DirToTest()
        {
            Point3D point = new Point3D() { X = 0, Y = 0, Z = 0 };
            var result1 = point.DirTo(null);
            Assert.AreEqual(result1, 0);

            Point3D endPoint = new Point3D() { X = 1, Y = 1, Z = 1 };
            var result2 = point.DirTo(endPoint);
            Assert.AreEqual(result2, 45 * Math.PI / 180); //包括Z轴 
        }

        [TestMethod()]
        public void VectorTest()
        {
            Point3D point = new Point3D() { X = 0, Y = 1, Z = 2 };
            Point3D endPoint = new Point3D() { X = 1, Y = 1, Z = 1 };
            Point3D result = point.Vector(endPoint);
            Assert.AreEqual(result.X, 1);
            Assert.AreEqual(result.Y, 0);
            Assert.AreEqual(result.Z, -1);
        }


        [TestMethod()]
        public void VectorTest1()  //以1公里（1000）为单位
        {
            Point3D point = new Point3D() { X = 0, Y = 1, Z = 0 };
            Point3D result = point.Vector(Math.PI / 2);
            Assert.AreEqual(result.X, 1000);
            Assert.AreEqual(Math.Round(result.Y, 4), 0);
            Assert.AreEqual(result.Z, 0);
        }

        [TestMethod()]
        public void VectorProductTest()
        {
            //二维向量叉乘公式a（x1,y1）,b（x2,y2）,则a×b=（x1y2-x2y1）
            Point3D point = new Point3D() { X = 0, Y = 1, Z = 2 };
            Point3D otherPoint = new Point3D() { X = 1, Y = 1, Z = 1 };
            var result = point.VectorProduct(otherPoint, Types.DimensionType.D3);  //暂不支持三维
            Assert.AreEqual(result, double.NaN);

            var result2 = point.VectorProduct(otherPoint, Types.DimensionType.D2);
            Assert.AreEqual(result2, -1);
        }

        [TestMethod()]
        public void GetModelTest()
        {
            Point3D point = new Point3D() { X = 1, Y = 1, Z = 1 };
            var result = point.GetModel();
            Assert.AreEqual(result, Math.Pow(3, 0.5));
        }

        [TestMethod()]
        public void PointMulityTest()  //向量叉乘 = x1x2+y1y2+z1z2
        {
            Point3D point = new Point3D() { X = 1, Y = 1, Z = 1 };
            Point3D otherPoint = new Point3D() { X = 2, Y = 2, Z = 2 };
            var result = point.PointMulity(otherPoint);
            Assert.AreEqual(result, 6);
        }

        [TestMethod()]
        public void IncludedAngleTest()
        {
            Point3D point = new Point3D() { X = 1, Y = 1, Z = 1 };
            Point3D otherPoint = new Point3D() { X = 2, Y = 2, Z = 2 };
            var result = point.IncludedAngle(otherPoint);
            Assert.AreEqual(result, 0);

            point = new Point3D() { X = 1, Y = 1, Z = 0 };
            Point3D otherPoint2 = new Point3D() { X = 1, Y = 0, Z = 0 };
            var result2 = point.IncludedAngle(otherPoint2);
            Assert.AreEqual(Math.Round(result2, 5), Math.Round(45 * Math.PI / 180, 5));
        }

        [TestMethod()]
        public void IncludeAnglebyTwoDirsTest()
        {
            Point3D point = new Point3D() { X = 0, Y = 0, Z = 0 };
            var result = point.IncludeAnglebyTwoDirs(Math.PI, Math.PI / 8);
            Assert.AreEqual(Math.Round(result, 8), Math.Round(Math.PI * 7 / 8, 8));
        }
    }
}
