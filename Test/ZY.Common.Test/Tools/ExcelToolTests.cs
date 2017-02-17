using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Common.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.IO;
namespace ZY.Common.Tools.Tests
{


    [TestClass()]
    public class ExcelToolTests
    {
        private string path;
        [TestInitialize]
        public void Initialize()
        {
            path = AppDomain.CurrentDomain.BaseDirectory + "\\TestCSV.csv";
        }

        [TestMethod()]
        public void CSVWriterTest()
        {
            DataTable table = new DataTable("sheet1");
            table.Columns.Add("ColumnA");
            table.Columns.Add("ColumnB");
            for (int i = 0; i < 100; i++)
            {
                var row = table.NewRow();
                row[0] = "111";
                row[1] = 222;
                table.Rows.Add(row);
            }
            ExcelTool.CSVWriter(table, path);
            bool exists = File.Exists(path);
            Assert.AreEqual(exists, true);
        }

        [TestMethod()]
        public void CSVReaderTest()
        {
            CSVWriterTest();
            DataTable table = ExcelTool.CSVReader(path);
            Assert.AreEqual(table.Rows.Count, 100);
            Assert.AreEqual(table.Columns.Count, 2);
        }
    }
}
