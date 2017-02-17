using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Common.Datas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
namespace ZY.Common.Datas.Tests
{
    [TestClass()]
    public class FeatureObjectTests
    {
        [TestMethod()]
        public void EqualsTest()
        {
            FeatureObject featureObject = new FeatureObject() { Coordinates = null };
            FeatureObject featureObjectCompare = new FeatureObject() { Coordinates = null };

            //类型不匹配测试
            var result1 = featureObject.Equals("");
            Assert.AreEqual(result1, false);

            //Coordinates相等且为null测试
            var result2 = featureObject.Equals(featureObjectCompare);
            Assert.AreEqual(result2, true);
        }

        [TestMethod()]
        public void EqualsTest2()//LayerName不同，FeatureAttribute为空等测试
        {
            Point3D testPoint1 = new Point3D { X = 0, Y = 0, Z = 0 };
            Point3D testPoint2 = new Point3D { X = 0, Y = 0, Z = 0 };

            FeatureObject featureObject = new FeatureObject() { Coordinates = new List<Point3D>() { testPoint1 }, LayerName = "test1", FeatureAttribute = null };
            FeatureObject featureObjectCompare = new FeatureObject() { Coordinates = new List<Point3D>() { testPoint2 }, LayerName = null, FeatureAttribute = null };

            var result = featureObject.Equals(featureObjectCompare);
            Assert.AreEqual(result, false);
        }

        [TestMethod()]
        public void EqualsTest3()//Point3D不同测试  //只比较X和Y
        {
            DataTable table = new DataTable();
            table.Columns.Add("column1");
            table.Columns.Add("column2");
            DataRow row = table.NewRow();
            row[0] = 111;
            row[1] = 222;
            table.Rows.Add(row);

            Point3D testPoint1 = new Point3D { X = 0, Y = 0, Z = 0 };
            Point3D testPoint2 = new Point3D { X = 0, Y = 1, Z = 0 };

            FeatureObject featureObject = new FeatureObject() { Coordinates = new List<Point3D>() { testPoint1 }, LayerName = "test1", FeatureAttribute = row };
            FeatureObject featureObjectCompare = new FeatureObject() { Coordinates = new List<Point3D>() { testPoint2 }, LayerName = "test1", FeatureAttribute = row };

            var result = featureObject.Equals(featureObjectCompare);
            Assert.AreEqual(result, false);
        }

        [TestMethod()]
        public void EqualsTest4()//FeatureAttribute不同测试  
        {
            DataTable table = new DataTable();
            table.Columns.Add("column1");
            table.Columns.Add("column2");
            DataRow row = table.NewRow();
            row[0] = 111;
            row[1] = 222;
            table.Rows.Add(row);

            DataRow row2 = table.NewRow();
            row2[0] = 111;
            row2[1] = 333;
            table.Rows.Add(row2);

            Point3D testPoint1 = new Point3D { X = 0, Y = 0, Z = 0 };

            FeatureObject featureObject = new FeatureObject() { Coordinates = new List<Point3D>() { testPoint1 }, LayerName = "test1", FeatureAttribute = row };
            FeatureObject featureObjectCompare = new FeatureObject() { Coordinates = new List<Point3D>() { testPoint1 }, LayerName = "test1", FeatureAttribute = row2 };

            var result = featureObject.Equals(featureObjectCompare);
            Assert.AreEqual(result, false);
        }

        [TestMethod()]
        public void EqualsTest5()//完全相同测试
        {
            DataTable table = new DataTable();
            table.Columns.Add("column1");
            table.Columns.Add("column2");
            DataRow row = table.NewRow();
            row[0] = 111;
            row[1] = 222;
            table.Rows.Add(row);

            Point3D testPoint1 = new Point3D { X = 0, Y = 0, Z = 0 };

            FeatureObject featureObject = new FeatureObject() { Coordinates = new List<Point3D>() { testPoint1 }, LayerName = "test1", FeatureAttribute = row };
            FeatureObject featureObjectCompare = new FeatureObject() { Coordinates = new List<Point3D>() { testPoint1 }, LayerName = "test1", FeatureAttribute = row };

            var result = featureObject.Equals(featureObjectCompare);
            Assert.AreEqual(result, true);
        }

        [TestMethod()]
        public void DeepCloneTest()//深拷贝测试
        {
            DataTable table = new DataTable();
            table.Columns.Add("column1");
            table.Columns.Add("column2");
            DataRow row = table.NewRow();
            row[0] = 111;
            row[1] = 222;
            table.Rows.Add(row);
            Point3D testPoint1 = new Point3D { X = 0, Y = 0, Z = 0 };
            FeatureObject featureObject = new FeatureObject() { Coordinates = new List<Point3D>() { testPoint1 }, LayerName = "test1", FeatureAttribute = row };

            var obj = featureObject.Clone();
            Assert.AreNotEqual(featureObject.GetHashCode(), obj.GetHashCode());

            var result = featureObject.Equals(obj);
            Assert.AreEqual(result, true);
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            DataTable table = new DataTable();
            table.Columns.Add("column1");
            table.Columns.Add("column2");
            DataRow row = table.NewRow();
            row[0] = 111;
            row[1] = 222;
            table.Rows.Add(row);
            Point3D testPoint1 = new Point3D { X = 0, Y = 0, Z = 0 };
            List<Point3D> list = new List<Point3D>() { testPoint1 };
            FeatureObject featureObject = new FeatureObject() { Coordinates = list, LayerName = "test1", FeatureAttribute = row };
            FeatureObject featureObject2 = new FeatureObject() { Coordinates = list, LayerName = "test1", FeatureAttribute = row };

            Assert.AreEqual(featureObject.GetHashCode(), featureObject2.GetHashCode());

            var obj = featureObject.Clone();
            Assert.AreNotEqual(featureObject.GetHashCode(), obj.GetHashCode());
        }
    }
}
