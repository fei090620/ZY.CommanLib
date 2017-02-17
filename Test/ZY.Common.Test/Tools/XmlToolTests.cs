using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Common.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Serialization;
namespace ZY.Common.Tools.Tests
{
    [TestClass()]
    public class XmlToolTests
    {
        TestObject Test = null;

        [TestInitialize]
        public void Init()
        {
            Test = new TestObject() { Count = 1.23456, id = Guid.NewGuid(), Name = "TestName", Sub = new TestSubObject() { SubName = "SubName" } };
        }

        [TestMethod()]
        public void SerializeTest()
        {
            string xmlText = XmlTool.Serialize(typeof(TestObject), this.Test);
            Assert.IsNotNull(xmlText);
        }

        [TestMethod()]
        public void DeserializeTest()
        {
            //Test1
            string xmlText = XmlTool.Serialize(typeof(TestObject), this.Test);
            TestObject getTest = XmlTool.Deserialize(typeof(TestObject), xmlText) as TestObject;
            Assert.AreEqual(getTest.id, Test.id);
            Assert.AreEqual(getTest.Name, Test.Name);
            Assert.AreEqual(getTest.Count, Test.Count);
            Assert.AreEqual(getTest.Sub.SubName, Test.Sub.SubName);
            //Test2
            TestObject getTest2 = XmlTool.Deserialize(typeof(TestObject), null) as TestObject;
            Assert.IsNull(getTest2);
        }

        [TestMethod()]
        public void DeserializeTest1()
        {
            //Test1
            string xmlText = XmlTool.Serialize(typeof(TestObject), this.Test);
            byte[] array = Encoding.ASCII.GetBytes(xmlText);
            MemoryStream stream = new MemoryStream(array);
            TestObject getTest = XmlTool.Deserialize<TestObject>(stream);
            Assert.AreEqual(getTest.id, Test.id);
            Assert.AreEqual(getTest.Name, Test.Name);
            Assert.AreEqual(getTest.Count, Test.Count);
            Assert.AreEqual(getTest.Sub.SubName, Test.Sub.SubName);
            //Test2
            string xmlText2 = "123";
            byte[] array2 = Encoding.ASCII.GetBytes(xmlText2);
            MemoryStream stream2 = new MemoryStream(array2);
            TestObject getTest2 = XmlTool.Deserialize<TestObject>(stream2);
            Assert.IsNull(getTest2);
        }
    }

    public class TestObject
    {
        public Guid id { get; set; }
        public TestSubObject Sub { get; set; }
        public string Name { get; set; }
        public double Count { get; set; }
    }

    public class TestSubObject
    {
        public string SubName { get; set; }
    }
}
