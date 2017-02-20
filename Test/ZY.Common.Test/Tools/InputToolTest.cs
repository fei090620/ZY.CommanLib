using System;
using System.IO;
using ADCC.Common.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZY.Common.Tools.Test
{

    [TestClass]
    public class InputToolTest
    {
        private Inputer _inputer;
        [TestInitialize]
        public void Init()
        {
            _inputer = new Inputer(".xsd");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void when_input_not_exist_file_throw_out_ArgumentNullException()
        {
            var notExistFile = @"../../notExistFile.xsd";
            Assert.IsFalse(File.Exists(notExistFile));

            _inputer.ValidateFile(notExistFile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void when_input_not_xsd_file_throw_ArgumentException()
        {
            var notXsdFile = @"..\Test.resourse\notXsdFile.txt";
            Assert.IsTrue(File.Exists(notXsdFile));

            _inputer.ValidateFile(notXsdFile);
        }

        [TestMethod]
        public void when_input_point_xsd_file_not_throw_ArgumentException()
        {
            var xsdFile = @"..\Test.resourse\xsdFile.xsd";
            Assert.IsTrue(File.Exists(xsdFile));
            _inputer.ValidateFile(xsdFile);
        }

        [TestMethod]
        public void when_input_point_xsd_file_GetFileStream_is_not_null_or_empty()
        {
            var xsdFile = @"..\Test.resourse\xsdFile.xsd";
            Assert.IsTrue(File.Exists(xsdFile));
            var fileStream = _inputer.GetFileStream(xsdFile);
            Assert.IsFalse(string.IsNullOrEmpty(fileStream));
        }
    }

}
