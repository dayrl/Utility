using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Zdd.Utility
{
    /// <summary>
    /// ���л�������
    /// </summary>
    /// <typeparam name="T">���л���������</typeparam>
    public static class SerializerHelper<T>
    {
        /// <summary>
        /// ��һ��������xml��ʽ�л����ŵ�ָ�����ļ�.
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
        /// ��ָ����xml�ļ���ԭһ������.
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
