using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Common.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
namespace ZY.Common.Tools.Tests
{
    [TestClass()]
    public class GenericToolTests
    {
        [TestMethod()]
        public void DistinctValueTest() //Value中无重复的Dictionary
        {
            //Test1
            Dictionary<int, int> dic1 = new Dictionary<int, int>();
            dic1.Add(1, 1);
            dic1.Add(2, 2);
            dic1.Add(3, 1);
            var dicResult1 = GenericTool.DistinctValue(dic1);
            Assert.AreEqual(dicResult1.Count, 2);

            //Test2
            Test t = new Test() { Id = "1" };
            Test t1 = t;
            Test t2 = t;
            Dictionary<int, object> dic2 = new Dictionary<int, object>();
            dic2.Add(1, t1);
            dic2.Add(2, new Test() { Id = "2" });
            dic2.Add(3, t2);
            var dicResult2 = GenericTool.DistinctValue(dic2);
            Assert.AreEqual(dicResult2.Count, 2);
        }

        private class Test
        {
            public string Id { get; set; }
        }

        [TestMethod()]
        public void GetCellValuebyPartConlumeNameTest()
        {
            DataTable table = new DataTable("table");
            table.Columns.Add("Name");
            table.Columns.Add("Id");
            table.Columns.Add("Year");
            DataRow row = table.NewRow();
            row[0] = "name1";
            row[1] = "id1";
            row[2] = "year1";
            table.Rows.Add(row);
            var result = GenericTool.GetCellValuebyPartConlumeName(row, "Id");
            Assert.AreEqual(result, "id1");

            var result2 = GenericTool.GetCellValuebyPartConlumeName(row, "Id2");
            Assert.AreEqual(result2, string.Empty);
        }

        [TestMethod()]
        public void ToListTest()
        {
            List<string> list = new List<string>();
            list.Add("123");
            var resultList = GenericTool.ToList(list, new Func<string, int>((x) => { return int.Parse(x); }));
            Assert.AreEqual(resultList[0], 123);

            var resultList2 = GenericTool.ToList(null, new Func<string, int>((x) => { return int.Parse(x); }));
            Assert.AreEqual(resultList2, null);
        }

        [TestMethod()]
        public void GetPartialTest()
        {
            List<string> list = new List<string>();
            list.Add("111");
            list.Add("123");
            list.Add("222");
            var resultList = GenericTool.GetPartial(list, new Func<string, bool>((x) => { return x.Contains("1"); }));

            Assert.AreEqual(resultList.Count, 2);
        }

        [TestMethod]
        public void Test_NotSatisifyContract()
        {
            //var result = GenericTool.GetCellValuebyPartConlumeName(null, null);
        }
    }
}
