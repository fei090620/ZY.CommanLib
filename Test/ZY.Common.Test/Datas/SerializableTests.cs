using ZY.Common.Datas;
using ZY.Common.Datas.Tests;
using ZY.Common.Tools;
using ZY.Common.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Common.Test.Datas
{
    [TestClass()]
    public class SerializableTests
    {
        /// <summary>
        /// 点序列化和反序列化
        /// </summary>
        [TestMethod()]
        public void Point3D_SerializableTest()
        {
            Point3D p = new Point3D() { X = 1, Y = 2, Z = 3 };
            string xmlText = XmlTool.Serialize(typeof(Point3D), p);
            Assert.IsNotNull(xmlText);
            Point3D pd = XmlTool.Deserialize(typeof(Point3D), xmlText) as Point3D;
            Assert.IsTrue(p.Equals(pd));
        }

        /// <summary>
        /// 线段序列化和反序列化
        /// </summary>
        [TestMethod()]
        public void LineSegment_Serializable()
        {
            Point3D start = new Point3D() { X = 1, Y = 2, Z = 3 };
            Point3D end = new Point3D() { X = 4, Y = 5, Z = 6 };
            LineSegment line = new LineSegment(start, end);
            string xmlText = XmlTool.Serialize(typeof(LineSegment), line);
            Assert.IsNotNull(xmlText);
            LineSegment ld = XmlTool.Deserialize(typeof(LineSegment), xmlText) as LineSegment;
            Assert.IsTrue(line.Equals(ld));
        }

        /// <summary>
        /// 圆弧序列化和反序列化
        /// </summary>
        [TestMethod()]
        public void ArcSegment_Serializable()
        {
            Point3D center = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D start = new Point3D() { X = 0, Y = 1, Z = 0 };
            Point3D end = new Point3D() { X = 1, Y = 0, Z = 0 };
            ArcSegment arc = new ArcSegment(center, start, end, ArcDirctionType.CLOCK_WISE);
            string xmlText = XmlTool.Serialize(typeof(ArcSegment), arc);
            Assert.IsNotNull(xmlText);
            ArcSegment ad = XmlTool.Deserialize(typeof(ArcSegment), xmlText) as ArcSegment;
            Assert.IsTrue(arc.Equals(ad));
        }

        /// <summary>
        /// 曲线段序列化和反序列化
        /// </summary>
        [TestMethod()]
        public void CurveSegment_Serializable()
        {
            List<CurveSegment> list = new List<CurveSegment>();

            Point3D center = new Point3D() { X = 0, Y = 0, Z = 0 };
            Point3D start = new Point3D() { X = 0, Y = 1, Z = 0 };
            Point3D end = new Point3D() { X = 1, Y = 0, Z = 0 };
            ArcSegment arc = new ArcSegment(center, start, end, ArcDirctionType.CLOCK_WISE);

            Point3D startL = new Point3D() { X = 1, Y = 2, Z = 3 };
            Point3D endL = new Point3D() { X = 4, Y = 5, Z = 6 };
            LineSegment line = new LineSegment(startL, endL);

            list.Add(arc);
            list.Add(line);

            string xmlText = XmlTool.Serialize(typeof(List<CurveSegment>), list);
            Assert.IsNotNull(xmlText);
            List<CurveSegment> cd = XmlTool.Deserialize(typeof(List<CurveSegment>), xmlText) as List<CurveSegment>;

            for (int i = 0; i < cd.Count; i++)
            {
                Assert.IsTrue(list[i].Equals(cd[i]));
            }
        }

    }
}
