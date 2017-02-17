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
    public class RotateMatrixTests
    {
        double[,] data1 = new double[2, 2];
        double[,] data2 = new double[2, 2];
        double[,] data3 = new double[1, 2];
        RotateMatrix matrix1;
        RotateMatrix matrix2;
        RotateMatrix matrix3;
        public RotateMatrixTests()
        {
            data1[0, 0] = 0;
            data1[0, 1] = 1;
            data1[1, 0] = 2;
            data1[1, 1] = 3;

            data2[0, 0] = 10;
            data2[0, 1] = 11;
            data2[1, 0] = 12;
            data2[1, 1] = 13;

            data3[0, 0] = 1;
            data3[0, 1] = 1;
        }

        [TestMethod()]
        public void AddTest()
        {
            matrix1 = new RotateMatrix(data1);
            matrix2 = new RotateMatrix(data2);
            double[,] dou = matrix1.Add(matrix2).m_data;
            Assert.AreEqual(dou[0, 0], 0 + 10);
            Assert.AreEqual(dou[0, 1], 1 + 11);
            Assert.AreEqual(dou[1, 0], 2 + 12);
            Assert.AreEqual(dou[1, 1], 3 + 13);
        }

        [TestMethod()]
        public void SubtractTest()
        {
            matrix1 = new RotateMatrix(data1);
            matrix2 = new RotateMatrix(data2);
            double[,] dou = matrix1.Subtract(matrix2).m_data;
            Assert.AreEqual(dou[0, 0], 0 - 10);
            Assert.AreEqual(dou[0, 1], 1 - 11);
            Assert.AreEqual(dou[1, 0], 2 - 12);
            Assert.AreEqual(dou[1, 1], 3 - 13);
        }

        [TestMethod()]
        public void MultiplyTest()
        {
            matrix1 = new RotateMatrix(data1);
            double[,] dou = matrix1.Multiply(1.1).m_data;
            Assert.AreEqual(dou[0, 0], data1[0, 0] * 1.1);
            Assert.AreEqual(dou[0, 1], data1[0, 1] * 1.1);
            Assert.AreEqual(dou[1, 0], data1[1, 0] * 1.1);
            Assert.AreEqual(dou[1, 1], data1[1, 1] * 1.1);
        }

        [TestMethod()]
        public void MultiplyTest1()
        {
            matrix1 = new RotateMatrix(data1);
            matrix2 = new RotateMatrix(data2);
            double[,] dou = matrix1.Multiply(matrix2).m_data;
            Assert.AreEqual(dou[0, 0], 12);
            Assert.AreEqual(dou[0, 1], 13);
            Assert.AreEqual(dou[1, 0], 56);
            Assert.AreEqual(dou[1, 1], 61);
        }

        [TestMethod()]
        public void Rotate2DTest()
        {
            //二维测试1
            matrix3 = new RotateMatrix(data3);
            double[,] dou = matrix3.Rotate2D(90, Types.ArcDirctionType.CLOCK_WISE).m_data;
            Assert.AreEqual(Math.Round(dou[0, 0], 8), 1);
            Assert.AreEqual(Math.Round(dou[0, 1], 8), -1);

            //二维测试1
            double d1 = Math.Cos(75 * Math.PI / 180) * Math.Pow(2, 0.5);
            double d2 = Math.Sin(75 * Math.PI / 180) * Math.Pow(2, 0.5);
            matrix3 = new RotateMatrix(data3);
            double[,] dou2 = matrix3.Rotate2D(30, Types.ArcDirctionType.UNCLOCK_WISE).m_data;
            Assert.AreEqual(Math.Round(dou2[0, 0], 8), Math.Round(d1, 8));
            Assert.AreEqual(Math.Round(dou2[0, 1], 8), Math.Round(d2, 8));

            //二维测试3
            matrix3 = new RotateMatrix(data3);
            double[,] dou3 = matrix3.Rotate2D(360, Types.ArcDirctionType.UNCLOCK_WISE).m_data;
            Assert.AreEqual(Math.Round(dou3[0, 0], 8), data3[0, 0]);
            Assert.AreEqual(Math.Round(dou3[0, 1], 8), data3[0, 1]);

            //二维测试4
            string exceptionText = "";
            try
            {
                matrix3 = new RotateMatrix(data3);
                double[,] dou4 = matrix3.Rotate2D(360, Types.ArcDirctionType.UNKONW).m_data;
            }
            catch (Exception ex)
            {
                exceptionText += ex.ToString();
            }
            Assert.AreNotEqual(exceptionText, "");
        }

        [TestMethod()]
        public void RotateAt2DTest()
        {
            //二维测试1
            matrix3 = new RotateMatrix(data3);
            double[,] dou = matrix3.RotateAt2D(90, new Point3D() { X = 0.5, Y = 1, Z = 1 }, Types.ArcDirctionType.UNCLOCK_WISE).m_data;
            Assert.AreEqual(Math.Round(dou[0, 0], 8), 0.5);
            Assert.AreEqual(Math.Round(dou[0, 1], 8), 1.5);
        }
    }
}
