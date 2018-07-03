using System;
using System.Text;

namespace Zdd.Utility
{
    /// <summary>
    /// һЩ�ַ�����������ֹ���
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// ��Unicode�ַ���ת��ΪANSI���飬����������'\0'
        /// </summary>
        /// <param name="s">Unicode�ַ���</param>
        /// <returns>ANSI����</returns>
        public static UInt16[] GetAnsiChars(string s)
        {
            if (s == null)
                return new ushort[0];

            byte[] bs = Encoding.Default.GetBytes(s);
            char[] cs = Encoding.Default.GetChars(bs);

            UInt16[] chars = new UInt16[cs.Length + 1];
            chars[chars.Length - 1] = 0;
            int bsPoint = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (cs[i] > 127)
                {
                    chars[i] = (UInt16)(bs[bsPoint] * (UInt16)256 + bs[bsPoint + 1]);
                    bsPoint++;
                }
                else
                    chars[i] = bs[bsPoint];
                bsPoint++;
            }
            return chars;
        }

        /// <summary>
        /// ��ȡƴ����
        /// </summary>
        /// <param name="s">�ַ���</param>
        /// <example>
        /// ���룺"�Ұ��й�" �����"wazg"
        /// </example>
        /// <returns>ÿ���ַ���Ӧƴ��������ַ�(Сд)��ɵ��ַ�����û�ж�Ӧ��ƴ���ַ�����*����</returns>
        public static string GetSpellCode(string s)
        {
            string tempStr = "";
            foreach (char c in s)
            {
                tempStr += GetSpell(c);
            }

            return tempStr;
        }

        /// <summary>
        /// ��ȡ�ַ���Ӧ��ƴ�������ַ���
        /// </summary>
        /// <example>
        /// ���룺'��' ��� "w"
        /// </example>
        /// <param name="c">�����ַ�</param>
        /// <returns>����ĸ(Сд)��û�ж�Ӧ��ƴ���ַ�����*����</returns>
        public static string GetSpell(char c)
        {
            if ((int)c >= 0 && (int)c <= 126)
                return c.ToString();

            byte[] array = System.Text.Encoding.Default.GetBytes(new char[] { c });
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));

            if (i < 0xB0A1) return "*";
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "g";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";
            return "*";
        }

        /// <summary>
        /// �������ַ���ת��Ϊ������ʽ
        /// </summary>
        /// <param name="s">�����ַ���</param>
        /// <returns>������ʽ</returns>
        public static string GetRegexString(string s)
        {
            s = s.Replace(@"\", "\\");
            s = s.Replace(".", @"\.");
            s = s.Replace("*", @"\*");
            s = s.Replace("+", @"\+");
            s = s.Replace("{", @"\{");
            s = s.Replace("}", @"\}");
            s = s.Replace("(", @"\(");
            s = s.Replace(")", @"\)");
            s = s.Replace("^", @"\^");
            s = s.Replace("$", @"\$");
            s = s.Replace("[", @"\[");
            s = s.Replace("]", @"\]");
            s = s.Replace("|", @"\|");
            s = s.Replace("<", @"\<");
            s = s.Replace(">", @"\>");
            s = s.Replace(" ", @"\s");

            return s;
        }

        /// <summary>
        /// �滻�����ļ�ϵͳ·����������ͬ�ȵ�ȫ���ַ���
        /// </summary>
        public static string ReplaceInvalidPathChars(string source)
        {
            string ret = source;
            ret = ret.Replace("\\", "��");
            ret = ret.Replace("/", "��");
            ret = ret.Replace(":", "��");
            ret = ret.Replace("|", "��");

            return ret;
        }

        /// <summary>
        /// Sets the first letter upper case.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static String SetFirstLetterUpperCase(String s)
        {
            if (s.Length == 1)
                return s.ToUpper();

            if (s.Length > 1)
                return (s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length - 1));

            return null;
        }

        /// <summary>
        /// Endses the with ignore case.
        /// </summary>
        /// <param name="sSource">The s source.</param>
        /// <param name="sPattern">The s pattern.</param>
        /// <returns></returns>
        public static bool EndsWithIgnoreCase(string s, string pattern)
        {
            return (s.ToUpper().EndsWith(pattern.ToUpper()));
        }

        public static string XmlDecode(string s)
        {
            if (s == null)
            {
                s = "";
            }
            string str2 = s;
            return str2.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&apos;", "'").Replace("&quot;", "\"");
        }
        public static string XmlEncode(string s)
        {
            if (s == null)
            {
                s = "";
            }
            string str2 = s;
            return str2.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
        }

        public static string SafeGetString(string strin)
        {
            string strout = "";
            if (strin != null)
            {
                strout = strin.ToString().Trim();
            }
            return strout;
        }
        public static string GetLimitedLengthString(string s, int length)
        {

            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] data = ascii.GetBytes(s);
            for (int i = 0; i < data.Length; i++)
            {
                if ((int)data[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += s.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > length)
                    break;
            }
            //����ع�����ϰ��ʡ�Ժ�
            byte[] mybyte = Encoding.Default.GetBytes(s);
            if (mybyte.Length > length)
                tempString += "��";

            return tempString;
        }


        /// <summary>
        /// Safes the get int from string.
        /// </summary>
        /// <param name="textvalue">The textvalue.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static int SafeGetIntFromString(string textvalue, int defaultvalue)
        {
            int num = defaultvalue;
            if (textvalue != null)
            {
                try
                {
                    num = Convert.ToInt32(textvalue);
                }
                catch (Exception)
                {
                }
            }
            return num;
        }
        /// <summary>
        /// Safes the get byte from object.
        /// </summary>
        /// <param name="textvalue">The textvalue.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static byte SafeGetByteFromObject(object textvalue, byte defaultvalue)
        {
            byte num = defaultvalue;
            if (textvalue != null)
            {
                try
                {
                    num = Convert.ToByte(textvalue.ToString());
                }
                catch (Exception)
                {
                }
            }
            return num;
        }

        private static DateTime DateTimeNull = DateTime.Now;
        /// <summary>
        /// Safes the get date from string.
        /// </summary>
        /// <param name="textvalue">The textvalue.</param>
        /// <returns></returns>
        public static DateTime SafeGetDateFromString(string textvalue)
        {
            DateTime dateTimeNull = DateTimeNull;
            try
            {
                dateTimeNull = DateTime.Parse(textvalue);
            }
            catch (Exception)
            {
            }
            return dateTimeNull;
        }
        /// <summary>
        /// Safes the get date string by format.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string SafeGetDateStringByFormat(DateTime dt, string format)
        {
            if (dt == DateTimeNull)
            {
                return "";
            }
            return dt.ToString(format);
        }
        /// <summary>
        /// Safes the get double from string.
        /// </summary>
        /// <param name="textvalue">The textvalue.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static double SafeGetDoubleFromString(string textvalue, double defaultvalue)
        {
            double num = defaultvalue;
            try
            {
                num = Convert.ToDouble(textvalue);
            }
            catch (Exception)
            {
            }
            return num;
        }
        /// <summary>
        /// Safes the get float from string.
        /// </summary>
        /// <param name="textvalue">The textvalue.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static float SafeGetFloatFromString(string textvalue, float defaultvalue)
        {
            float num = defaultvalue;
            try
            {
                num = (float)Convert.ToDouble(textvalue);
            }
            catch (Exception)
            {
            }
            return num;
        }
        /// <summary>
        /// Safes the get long from string.
        /// </summary>
        /// <param name="textvalue">The textvalue.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static long SafeGetLongFromString(string textvalue, int defaultvalue)
        {
            long num = defaultvalue;
            if (textvalue != null)
            {
                try
                {
                    num = Convert.ToInt64(textvalue);
                }
                catch (Exception)
                {
                }
            }
            return num;
        }
        /// <summary>
        /// Safes the get U int from string.
        /// </summary>
        /// <param name="textvalue">The textvalue.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static uint SafeGetUIntFromString(string textvalue, uint defaultvalue)
        {
            uint num = defaultvalue;
            if (textvalue != null)
            {
                try
                {
                    num = Convert.ToUInt32(textvalue);
                }
                catch (Exception)
                {
                }
            }
            return num;
        }

        #region ȫ��(SBC case)  ���(DBC case)  �жϺ�ת��

        /************************************************************************/
        /* ȫ��(SBC case)  
         * ���(DBC case)
         * �жϺ�ת��
         * 2009-11-4
         * wangzq  add
        /************************************************************************/

        /************************************************************************/
        /*  ���(DBC case) �ж� */
        /************************************************************************/

        /// <summary>
        /// �ж��ַ��Ƿ�Ӣ�İ���ַ�����
        /// 32    �ո�
        /// 33-47    ���
        /// 48-57    0~9
        /// 58-64    ���
        /// 65-90    A~Z
        /// 91-96    ���
        /// 97-122    a~z
        /// 123-126  ���
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsDBCChar(char c)
        {
            int i = (int)c;
            return i >= 32 && i <= 126;
        }

        /************************************************************************/
        /*  ȫ��(SBC case) �ж�  */
        /************************************************************************/

        /// <summary>
        /// �ж��ַ��Ƿ�ȫ���ַ�����
        /// ȫ���ַ� - 65248 = ����ַ�
        /// ȫ�ǿո�����
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsSBCChar(char c)
        {
            if (c == '\u3000')
            {
                return true;
            }
            int i = (int)c - 65248;
            if (i < 32)
            {
                return false;
            }
            else
            {
                return IsDBCChar((char)i);
            }
        }

        /************************************************************************/
        /*  ���(DBC case) ת�� */
        /************************************************************************/

        /// <summary>
        /// ���ַ����е�ȫ���ַ�ת��Ϊ���
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToBj(string s)
        {
            if (s == null || s.Trim() == string.Empty)
            {
                return s;
            }
            else
            {
                StringBuilder sb = new StringBuilder(s.Length);
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '\u3000')
                    {
                        sb.Append('\u0020');
                    }
                    else if (IsSBCChar(s[i]))
                    {
                        sb.Append((char)((int)s[i] - 65248));
                    }
                    else
                    {
                        sb.Append(s[i]);
                    }
                }
                return sb.ToString();
            }
        }

        /// <summary>
        ///ת��ǵĺ���(DBC case) 
        ///�����ַ��� 
        ///����ַ��� 
        ///ȫ�ǿո�Ϊ12288����ǿո�Ϊ32
        ///�����ַ����(33-126)��ȫ��(65281-65374)�Ķ�Ӧ��ϵ�ǣ������65248 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32; continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /************************************************************************/
        /*  ȫ��(SBC case) ת�� */
        /************************************************************************/

        /// <summary>
        /// תȫ�ǵĺ���(SBC case)
        /// �����ַ��� 
        /// ȫ���ַ��� ///
        /// ȫ�ǿո�Ϊ12288,��ǿո�Ϊ32 
        /// �����ַ����(33-126)��ȫ��(65281-65374)�Ķ�Ӧ��ϵ�ǣ������65248 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            //���תȫ�ǣ�
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127) c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        #endregion
    }
}
