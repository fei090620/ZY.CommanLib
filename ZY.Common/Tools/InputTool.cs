using System;
using System.IO;

namespace ADCC.Common.Tools
{
    public static class InputTool
    { 
        public static void ValidateFile(this Inputer inputer, string xsdFile)
        {
            if (!File.Exists(xsdFile))
                throw new ArgumentNullException("The input file is not exist!");

            string extendString = new FileInfo(xsdFile).Extension;
            if (!string.Equals(extendString, inputer.Extend))
                throw new ArgumentException("Please input .xsd file!");
        }

        public static string GetFileStream(this Inputer inputer, string xsdFile)
        {
            inputer.ValidateFile(xsdFile);
            var file = File.ReadAllText(xsdFile);
            return file.Replace("\n","").Replace("\t","").Replace("\r","");
        }
    }

    public class Inputer
    {
        public string Extend { get; private set; }
        public Inputer(string extend)
        {
            Extend = extend;
        }
    }
}
