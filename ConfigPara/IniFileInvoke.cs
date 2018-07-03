using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace Zdd.Utility.ConfigPara
{
    public class IniFileInvoke
    {
        #region private IniFileInvoke dllImport
        /// <summary>
        /// 最大字符缓冲区
        /// </summary>
        private readonly int MAX_VALUE_LEN = 1024;

        private string m_path;
        public string IniPath
        {
            get { return m_path; }
            set { m_path = value; }
        }

        private Mutex write_mutex;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, uint nSize, string lpFileName);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, IntPtr lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);
        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileSection", CharSet = CharSet.Auto, SetLastError = true)]
        protected internal static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string lpFileName);
        #endregion

        #region IniFileInvoke method

       /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ini_path"></param>
        /// <returns></returns>
        public IniFileInvoke(string ini_path)
        {
            if (null == write_mutex)
            {
                write_mutex =new Mutex(false, "INI_HELPER");
            }
            this.m_path = ini_path;
        }
        ~IniFileInvoke()
        {
            if (null != write_mutex)
            {
                write_mutex.Close();
            }
        }
        /// <summary>
        /// 转换文件编码类型
        /// </summary>
        /// <returns></returns>
        private void ConvertEncoding()
        {
            if (!IniExist())
                return;

            Encoding current_encoding = GetType(m_path);
            if (current_encoding == Encoding.UTF8
                || current_encoding == Encoding.BigEndianUnicode
                || current_encoding == Encoding.Unicode)
            {
                StreamReader reader = new StreamReader(m_path, current_encoding);
                string input = reader.ReadToEnd();
                reader.Close();
                write_mutex.WaitOne();
                StreamWriter writer = new StreamWriter(m_path, false, Encoding.GetEncoding(0x3a8));
                writer.Write(input);
                writer.Close();
                write_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// 获取文件编码类型
        /// </summary>
        /// <param name="ini_path"></param>
        /// <returns></returns>
        private Encoding GetType(string ini_path)
        {
            FileStream fs = new FileStream(ini_path, FileMode.Open, FileAccess.Read);
            Encoding encoding = GetType(fs);
            fs.Close();
            return encoding;
        }

        /// <summary>
        /// 获取文件流的编码类型
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        private Encoding GetType(FileStream fs)
        {
            BinaryReader br = new BinaryReader(fs, Encoding.Default);
            byte[] content_start = br.ReadBytes(4);
            br.Close();

            if (content_start[0] >= 0xEF)
            {
                if (content_start[0] == 0xEF && content_start[1] == 0xBB && content_start[2] == 0xBF)
                    return Encoding.UTF8;
                else if (content_start[0] == 0xFE && content_start[1] == 0xFF)
                    return Encoding.BigEndianUnicode;
                else if (content_start[0] == 0xFF && content_start[1] == 0xFE)
                    return Encoding.Unicode;
                else
                    return Encoding.Default;
            }
            return Encoding.Default;
        }

        /// <summary>
        /// 写入配置信息
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>0 成功</returns>
        public int IniWriteValue(string section, string key, string value)
        {
            ConvertEncoding();
            WritePrivateProfileString(section, key, value, m_path);
            return 0;
        }

        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>-1 读取失败或为空 0读取成功</returns>
        public int IniReadValue(string section, string key, out string value)
        {
            ConvertEncoding();
            StringBuilder temp_value = new StringBuilder(MAX_VALUE_LEN);
            int return_value = GetPrivateProfileString(section, key, "", temp_value, MAX_VALUE_LEN, m_path);
            value = temp_value.ToString();
            if (value == string.Empty)
                return -1;
            else
                return 0;
        }
        /// <summary>
        /// 读取整型值
        /// </summary>
        /// <param name="section"></param>
        /// <param name="skey"></param>
        /// <returns>-1读取错误</returns>
        public int IniReadValueInt(string section, string skey, int defaultVal = -1)
        {
            ConvertEncoding();
            return GetPrivateProfileInt(section, skey, defaultVal, m_path);
        }
        /// <summary>
        /// 判断配置文件是否存在
        /// </summary>
        /// <returns></returns>
        public bool IniExist()
        {
            return File.Exists(m_path);
        }
        /// <summary>
        /// 读取值
        /// </summary>
        /// <param name="argSectionName"></param>
        /// <param name="argKeyName"></param>
        /// <param name="argDefaultValue"></param>
        /// <returns></returns>
        public string GetString(string argSectionName, string argKeyName, string argDefaultValue = "")
        {
            if (!string.IsNullOrEmpty(argSectionName) && !string.IsNullOrEmpty(argKeyName))
            {
                ConvertEncoding();
                StringBuilder stringBuilder = new StringBuilder(MAX_VALUE_LEN);
                GetPrivateProfileString(argSectionName, argKeyName, argDefaultValue, stringBuilder, MAX_VALUE_LEN, this.m_path);
                return stringBuilder.ToString();
            }
            return argDefaultValue;
        }

        /// <summary>
        /// 获取分区的名称和值列表
        /// </summary>
        /// <param name="argSectionName"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetSectionValuesAsList(string argSectionName)
        {
            if (string.IsNullOrEmpty(argSectionName))
            {
                return null;
            }
            ConvertEncoding();
            IntPtr intPtr = Marshal.AllocCoTaskMem(MAX_VALUE_LEN);
            string[] array;
            try
            {
                int privateProfileSection = GetPrivateProfileSection(argSectionName, intPtr, MAX_VALUE_LEN, this.m_path);
                array = ConvertNullSeperatedStringToStringArray(intPtr, privateProfileSection);
            }
            finally
            {
                Marshal.FreeCoTaskMem(intPtr);
            }
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                int num = array[i].IndexOf('=');
                string key = array[i].Substring(0, num);
                string value = array[i].Substring(num + 1, array[i].Length - num - 1);
                list.Add(new KeyValuePair<string, string>(key, value));
            }
            return list;
        }

        /// <summary>
        /// 获取分区的所有值
        /// </summary>
        /// <param name="argSectionName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetSectionValues(string argSectionName)
        {
            ConvertEncoding();
            List<KeyValuePair<string, string>> sectionValuesAsList = this.GetSectionValuesAsList(argSectionName);
            Dictionary<string, string> dictionary = new Dictionary<string, string>(sectionValuesAsList.Count);
            foreach (KeyValuePair<string, string> current in sectionValuesAsList)
            {
                if (!dictionary.ContainsKey(current.Key))
                {
                    dictionary.Add(current.Key, current.Value);
                }
            }
            return dictionary;
        }

        /// <summary>
        /// 获取分区的名称列表
        /// </summary>
        /// <param name="argSectionName"></param>
        /// <returns></returns>
        public string[] GetKeyNames(string argSectionName)
        {
            if (string.IsNullOrEmpty(argSectionName))
            {
                return null;
            }
            ConvertEncoding();
            IntPtr intPtr = Marshal.AllocCoTaskMem(MAX_VALUE_LEN);
            string[] result;
            try
            {
                int privateProfileString = GetPrivateProfileString(argSectionName, null, null, intPtr, MAX_VALUE_LEN, this.m_path);
                result = ConvertNullSeperatedStringToStringArray(intPtr, privateProfileString);
            }
            finally
            {
                Marshal.FreeCoTaskMem(intPtr);
            }
            return result;
        }
        /// <summary>
        /// 获取所有分区名称
        /// </summary>
        /// <returns></returns>
        public string[] GetSectionNames()
        {
            ConvertEncoding();
            IntPtr intPtr = Marshal.AllocCoTaskMem(MAX_VALUE_LEN);
            string[] result;
            try
            {
                int privateProfileSectionNames = GetPrivateProfileSectionNames(intPtr, 32767u, this.m_path);
                result = ConvertNullSeperatedStringToStringArray(intPtr, privateProfileSectionNames);
            }
            finally
            {
                Marshal.FreeCoTaskMem(intPtr);
            }
            return result;
        }

        private static string[] ConvertNullSeperatedStringToStringArray(IntPtr ptr, int valLength)
        {
            string[] result;
            if (valLength == 0)
            {
                result = new string[0];
            }
            else
            {
                string text = Marshal.PtrToStringAuto(ptr, valLength - 1);
                char[] separator = new char[1];
                result = text.Split(separator);
            }
            return result;
        }

        public static void WriteIniValue(string section, string key, string val, string sPath)
        {
            WritePrivateProfileString(section, key, val, sPath);
        }

        public static string ReadIniValue(string section, string key, string sPath)
        {
            System.Text.StringBuilder steb = new System.Text.StringBuilder(255);
            GetPrivateProfileString(section, key, "", steb, 255, sPath);
            return steb.ToString();
        }

        public static int ReadIniValueForInt(string section, string skey, string sPath)
        {
            return GetPrivateProfileInt(section, skey, -1, sPath);
        }

        #endregion
    }
}
