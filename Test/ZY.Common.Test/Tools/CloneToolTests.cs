using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Common.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Reflection;

namespace ZY.Common.Tools.Tests
{
    [TestClass()]
    public class CloneToolTests
    {
        #region For Test
        public int Count = 0;
        public CloneToolTests()
        {
        }

        public CloneToolTests(int count)
        {
            this.Count = count;
        }
        #endregion

        [TestMethod()]
        public void DeepCloneTest()
        {
            //对象为null情况
            var obj = CloneTool.DeepClone(null, DeepCloneType.Serialize);
            Assert.AreEqual(obj, null);


            //1. 序列化
            ClassB objB1 = new ClassB();
            objB1.RefClass = new RefClass();
            objB1.RefClass.Field = 20;
            ClassB objB2 = CloneTool.DeepClone(objB1, DeepCloneType.Serialize) as ClassB;
            ClassB objB3 = objB1;

            Assert.AreEqual(objB1.RefClass.GetHashCode(), objB3.RefClass.GetHashCode());
            Assert.AreNotEqual(objB1.RefClass.GetHashCode(), objB2.RefClass.GetHashCode());


            //2. 反射实例化
            var resultR = CloneTool.DeepClone(false, DeepCloneType.Refactor); //值类型测试
            Assert.AreEqual(resultR.GetHashCode(), 0);

            var resultR2 = CloneTool.DeepClone(12, DeepCloneType.Refactor); //值类型测试
            Assert.AreEqual(resultR2.GetHashCode(), 12);

            //Assembly assembly = Assembly.LoadFile("程序集路径，不能是相对路径"); // 加载外部程序集（EXE 或 DLL）
            Assembly assembly = Assembly.GetExecutingAssembly(); // 加载当前程序集
            object o = Assembly.GetExecutingAssembly().CreateInstance("ZY.Common.Tools.Tests.CloneToolTests", true, System.Reflection.BindingFlags.Default, null, new object[1] { 12 }, null, null);
            var resultR3 = CloneTool.DeepClone(o, DeepCloneType.Refactor);
            Assert.AreNotEqual(o.GetHashCode(), resultR3.GetHashCode());
            Assert.AreEqual((o as CloneToolTests).Count, (resultR3 as CloneToolTests).Count);

            //3. DataRow
            DataTable table = new DataTable("sheet1");
            table.Columns.Add("ColumnA");
            table.Columns.Add("ColumnB");
            var row = table.NewRow();
            row[0] = "111";
            row[1] = 222;
            table.Rows.Add(row);
            var result = CloneTool.DeepClone(row, DeepCloneType.DataRow);
            var copyRow = row;
            Assert.AreEqual(row.GetHashCode(), copyRow.GetHashCode());
            Assert.AreNotEqual(row.GetHashCode(), result.GetHashCode());

            foreach (DataColumn column in (result as DataRow).Table.Columns)
            {
                var clone = (result as DataRow)[column.ColumnName];
                var source = row[column.ColumnName];
                Assert.AreEqual(clone, source);
            }
        }
    }

    #region For Test
    [Serializable]
    class RefClass
    {
        public int Field { get; set; }
    }

    [Serializable]
    class ClassB
    {
        public RefClass RefClass { get; set; }
    }
    #endregion
}
