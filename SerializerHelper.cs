using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Zdd.Utility
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    /// <typeparam name="T">序列化对象类型</typeparam>
    public static class SerializerHelper<T>
    {
        /// <summary>
        /// 将一个对象序按xml方式列化后存放到指定的文件.
        /// </summary>
        public static bool SaveAsXml(T t, string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (string.IsNullOrEmpty(fileName))
                return false;

            Stream stream = null;
            XmlSerializer xs = new XmlSerializer(typeof(T));

            try
            {
                stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                xs.Serialize(stream, t);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return true;
        }

        /// <summary>
        /// 从指定的xml文件还原一个对象.
        /// </summary>
        public static T LoadFromXml(string fileName)
        {
            T t;
            if (string.IsNullOrEmpty(fileName))
                return default(T);

            if (!File.Exists(fileName))
                return default(T);

            Stream stream = null;
            XmlSerializer xs = new XmlSerializer(typeof(T));

            try
            {
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                t = (T)xs.Deserialize(stream);
            }
            catch
            {
                return default(T);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return t;
        }
    }
}
