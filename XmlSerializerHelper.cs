using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing.Imaging;
using System.Drawing;

/********************************************************************
	created:	2017/03/08
	filename: 	XmlSerializerHelper.cs
	file path:	    StampTool
	file base:	XmlSerializerHelper
	file ext:	    cs
	author:		ZDD
	purpose:	XML File Serialize or Deserialize base class
    modify              date
     init version      2017/03/08
*********************************************************************/
namespace Zdd.Utility
{
    public class XmlSerializerHelper<T> where T : class
    {
        private XmlSerializer xmlSer;
        private FileStream fs;
        private StringBuilder buffer;
        private TextWriter writer;
        private TextReader reader;
        public XmlSerializerHelper(string xmlRootName = "")
        {
            if (string.IsNullOrEmpty(xmlRootName))
                this.xmlSer = new XmlSerializer(typeof(T));
            else
            {
                XmlRootAttribute rootAttr = new XmlRootAttribute(xmlRootName);
                rootAttr.Namespace = "";
                this.xmlSer = new XmlSerializer(typeof(T), rootAttr);
            }
            this.buffer = new StringBuilder();
        }

        public bool Serialize(string xmlPath, T t)
        {
            bool result;
            try
            {
                FileInfo fi = new FileInfo(xmlPath);
                if (!Directory.Exists(fi.DirectoryName))
                    Directory.CreateDirectory(fi.DirectoryName);
                //this.fs = new FileStream(xmlPath, FileMode.Create);//==== has lines

                //Create our own namespaces for the output  
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //Add an empty namespace and empty value  
                ns.Add("", "");
               // this.xmlSer.Serialize(this.fs, t, ns);//==== has lines
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(xmlPath, settings))
                {
                    this.xmlSer.Serialize(writer, t, ns);
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.fs != null)
                {
                    this.fs.Close();
                    this.fs = null;
                }
            }
            return result;
        }

        public T Deserialize(string xmlPath)
        {
            T result = default(T);
            try
            {
                if (File.Exists(xmlPath))
                {
                    this.fs = new FileStream(xmlPath, FileMode.Open);
                    T t = (T)((object)this.xmlSer.Deserialize(this.fs));
                    result = t;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.fs != null)
                {
                    this.fs.Close();
                    this.fs = null;
                }
            }
            return result;
        }
        public string XMLSerialize(T entity)
        {
            string result;
            try
            {
                /*
                this.buffer.Clear();
                this.writer = new StringWriter(this.buffer);
                this.xmlSer.Serialize(this.writer, entity);
                result = this.buffer.ToString();
                 */
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                string temp = Path.GetTempFileName();
                using (XmlWriter writer = XmlWriter.Create(temp, settings))
                {
                    this.xmlSer.Serialize(writer, entity, ns);
                }
                result = File.ReadAllText(temp);
                File.Delete(temp);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.writer != null)
                {
                    this.writer.Close();
                    this.writer.Dispose();
                    this.writer = null;
                }
                if (this.buffer != null)
                {
                    this.buffer.Clear();
                }
            }
            //return result;
        }

        public T DeXMLSerialize(string xmlString)
        {
            T result;
            try
            {
                this.buffer.Append(xmlString);
                this.reader = new StringReader(this.buffer.ToString());
                using (XmlReader rder = XmlReader.Create(reader))
                {
                    result = (T)((object)this.xmlSer.Deserialize(rder));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.reader != null)
                {
                    this.reader.Close();
                    this.reader.Dispose();
                    this.reader = null;
                }
                if (this.buffer != null)
                {
                    this.buffer.Clear();
                }
            }
            return result;
        }
    }
    public class XmlHelper
    {
        /// <summary>
        /// 从xml文件读取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="xmlRootName"></param>
        /// <returns></returns>
        public static T LoadFromXml<T>(string fileName, string xmlRootName = "")
        {
            T t = default(T);

            var relName = fileName;
            Stream stream = null;
            try
            {
                FileInfo fi = new FileInfo(relName);
                if (!fi.Exists)
                {
                    return t;
                }
                stream = new FileStream(relName, FileMode.Open);
                XmlSerializer xs = String.IsNullOrEmpty(xmlRootName) ?
                    new XmlSerializer(typeof(T)) :
                    new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootName));

                t = (T)(xs.Deserialize(stream));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
            return t;
        }

        /// <summary>
        /// 保存对象到XML文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="xmlRootName"></param>
        /// <param name="t"></param>
        public static bool SaveToXml<T>(string fileName, string xmlRootName, T t)
        {
            FileStream stream = null;
            try
            {
                FileInfo fi = new FileInfo(fileName);
                if (!fi.Directory.Exists)
                {
                    Directory.CreateDirectory(fi.Directory.FullName);
                }
                stream = new FileStream(fileName, FileMode.Create);

                XmlSerializer xs = String.IsNullOrEmpty(xmlRootName) ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootName));
                xs.Serialize(stream, t);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
            return true;
        }

        /// <summary>
        /// 将对象保存到流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="xmlRootName"></param>
        /// <param name="t"></param>
        public static bool SaveToXml<T>(Stream stream, string xmlRootName, T t) where T : class
        {
            try
            {
                XmlSerializer xs = String.IsNullOrEmpty(xmlRootName) ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootName));

                xs.Serialize(stream, t);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
            return true;
        }

        /// <summary>
        /// 从流中读取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="xmlRootName"></param>
        /// <returns></returns>
        public static T LoadFromXml<T>(Stream stream, string xmlRootName) where T : class
        {
            T t = default(T);
            try
            {
                var xs = String.IsNullOrEmpty(xmlRootName) ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootName));

                t = xs.Deserialize(stream) as T;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
            return t;
        }

        /// <summary>
        /// 获取xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlRootName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ObjectToXmlString<T>(string xmlRootName, T t) where T : class
        {
            string str = "";
            try
            {
                if (t == null) return string.Empty;
                if (string.IsNullOrEmpty(xmlRootName))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    using (MemoryStream memoryStream = new System.IO.MemoryStream())
                    {
                        using (XmlTextWriter xtw = new System.Xml.XmlTextWriter(memoryStream, Encoding.UTF8))
                        {
                            XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
                            xns.Add("", "");
                            xmlSerializer.Serialize(xtw, t, xns);
                            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(memoryStream, Encoding.UTF8))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
                else
                {
                    using (var writer = new StringWriter())
                    {
                        var xs = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootName));
                        xs.Serialize(writer, t);
                        str = writer.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return str;
        }

        /// <summary>
        /// 获取xml文件中某个节点的某个属性的值
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="NodeName"></param>
        /// <param name="AttributeName"></param>
        /// <returns></returns>
        public static string GetXmlNodeValue(string xmlPath, string NodeName, string AttributeName)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Path.Combine(xmlPath));
                XmlNodeList XE = xmlDoc.GetElementsByTagName(NodeName);
                if (XE != null && XE.Count > 0)
                {
                    var xNode = XE[0];
                    string passString = xNode.Attributes[AttributeName].InnerText;
                    return passString;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        /// <summary>
        /// 设置xml文件中某个节点的某个属性的值
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="NodeName"></param>
        /// <param name="AttributeName"></param>
        /// <returns></returns>
        public static bool SetXmlNodeValue(string xmlPath, string NodeName, string AttributeName, string svalue)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Path.Combine(xmlPath));
                XmlNodeList XE = xmlDoc.GetElementsByTagName(NodeName);
                if (XE != null && XE.Count > 0)
                {
                    var xNode = XE[0];
                    xNode.Attributes[AttributeName].InnerText = svalue;
                    xmlDoc.Save(xmlPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T XmlStringToObject<T>(string str, string xmlRootName) where T : class
        {
            T t = default(T);
            try
            {
                using (var reader = new StringReader(str))
                {
                    var xs = String.IsNullOrEmpty(xmlRootName) ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootName));

                    t = xs.Deserialize(reader) as T;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return t;
        }
        /// <summary>
        /// 根据图片文件,生成在HTML上显示的img标记
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public static string GetImageTag(string imgPath)
        {
            if (string.IsNullOrEmpty(imgPath))
                return "";
            FileInfo fi = new FileInfo(imgPath);
            if (!fi.Exists)
            {
                return "";
            }
            string htmlTag = "<image src=\"data:image/{0};base64,{1}\" />";
            string base64 = ImageToBase64(imgPath);

            string strExt = fi.Extension.Substring(1);
            return string.Format(htmlTag, strExt, base64);
        }
        /// <summary>
        /// 将图片文件转换为base64字符串
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public static string ImageToBase64(string imgPath)
        {
            if (string.IsNullOrEmpty(imgPath))
                return "";
            FileInfo fi = new FileInfo(imgPath);
            if (!fi.Exists)
                return "";
            imgPath = imgPath.ToLower();
            try
            {
                ImageFormat imgFormat = ImageFormat.Jpeg;
                if (imgPath.EndsWith("bmp"))
                    imgFormat = ImageFormat.Bmp;
                if (imgPath.EndsWith("png"))
                    imgFormat = ImageFormat.Png;
                if (imgPath.EndsWith("jpg") || imgPath.EndsWith("jpeg"))
                    imgFormat = ImageFormat.Jpeg;
                if (imgPath.EndsWith("gif"))
                    imgFormat = ImageFormat.Gif;
                using (Bitmap bmp = new Bitmap(imgPath))
                {
                    MemoryStream ms = new MemoryStream();
                    bmp.Save(ms, imgFormat);
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    ms.Close();
                    String strbaser64 = Convert.ToBase64String(arr);
                    return strbaser64;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    /// <summary>
    /// JSON序列化
    /// </summary>
    public class JsonHelper
    {
        public static T JsonDeserialize<T>(string jsonString)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                T obj = (T)ser.ReadObject(ms);
                return obj;
            }
        }
        public static string JsonSerialize<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                serializer.WriteObject(stream, obj);
                byte[] dataBytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(dataBytes, 0, (int)stream.Length);
                return Encoding.Default.GetString(dataBytes);
            }
        }
    }
}
