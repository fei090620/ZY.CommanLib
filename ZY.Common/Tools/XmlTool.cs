using System;
using System.IO;
using System.Xml.Serialization;

namespace ZY.Common.Tools
{
    /// <summary>
    /// xml帮助文件
    /// </summary>
    public static class XmlTool
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(Type type, object obj)
        {
            string str = null;
            MemoryStream stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                xml.Serialize(stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            str = reader.ReadToEnd();
            reader.Dispose();
            stream.Dispose();
            return str;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                StringReader reader = new StringReader(xml);
                XmlSerializer xmldes = new XmlSerializer(type);
                return xmldes.Deserialize(reader);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T Deserialize<T>(Stream stream)
        {
            try
            {
                XmlSerializer xmldes = new XmlSerializer(typeof(T));
                return (T)xmldes.Deserialize(stream);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
