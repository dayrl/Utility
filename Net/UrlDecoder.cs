using System;
using System.Text;
using System.Globalization;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

namespace Zdd.Utility.Net
{
    public class UrlDecoder
    {
        private static char[] s_entityEndingChars;
        static UrlDecoder()
        {
            s_entityEndingChars = new char[] { ';', '&' };
        }
        private int _bufferSize;
        private int _numChars;
        private char[] _charBuffer;
        private int _numBytes;
        private byte[] _byteBuffer;
        private Encoding _encoding;
        private void FlushBytes()
        {
            if (this._numBytes > 0)
            {
                this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
                this._numBytes = 0;
            }
        }
        internal UrlDecoder(int bufferSize, Encoding encoding)
        {
            this._bufferSize = bufferSize;
            this._encoding = encoding;
            this._charBuffer = new char[bufferSize];
        }

        internal void AddChar(char ch)
        {
            if (this._numBytes > 0)
            {
                this.FlushBytes();
            }
            this._charBuffer[this._numChars++] = ch;
        }

        internal void AddByte(byte b)
        {
            if (this._byteBuffer == null)
            {
                this._byteBuffer = new byte[this._bufferSize];
            }
            this._byteBuffer[this._numBytes++] = b;
        }

        internal string GetString()
        {
            if (this._numBytes > 0)
            {
                this.FlushBytes();
            }
            if (this._numChars > 0)
            {
                return new string(this._charBuffer, 0, this._numChars);
            }
            return string.Empty;
        }
        private static int HexToInt(char h)
        {
            if (h >= '0' && h <= '9')
            {
                return (int)(h - '0');
            }
            if (h >= 'a' && h <= 'f')
            {
                return (int)(h - 'a' + '\n');
            }
            if (h < 'A' || h > 'F')
            {
                return -1;
            }
            return (int)(h - 'A' + '\n');
        }
        internal static bool IsSafe(char ch)
        {
            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
            {
                return true;
            }
            if (ch != '!')
            {
                switch (ch)
                {
                    case '\'':
                    case '(':
                    case ')':
                    case '*':
                    case '-':
                    case '.':
                        return true;
                    case '+':
                    case ',':
                        break;
                    default:
                        if (ch == '_')
                        {
                            return true;
                        }
                        break;
                }
                return false;
            }
            return true;
        }
        internal static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 48);
            }
            return (char)(n - 10 + 97);
        }
        public static string UrlDecode(string str)
        {
            if (str == null)
            {
                return null;
            }
            return UrlDecode(str, Encoding.UTF8);
        }
        public static string UrlDecode(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            return UrlDecodeStringFromStringInternal(str, e);
        }
        /// <summary>
        /// 对Url编码
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string UrlDecodeStringFromStringInternal(string s, Encoding e)
        {
            int length = s.Length;
            UrlDecoder urlDecoder = new UrlDecoder(length, e);
            int i = 0;
            while (i < length)
            {
                char c = s[i];
                if (c == '+')
                {
                    c = ' ';
                    goto IL_106;
                }
                if (c != '%' || i >= length - 2)
                {
                    goto IL_106;
                }
                if (s[i + 1] == 'u' && i < length - 5)
                {
                    int num = HexToInt(s[i + 2]);
                    int num2 = HexToInt(s[i + 3]);
                    int num3 = HexToInt(s[i + 4]);
                    int num4 = HexToInt(s[i + 5]);
                    if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0)
                    {
                        goto IL_106;
                    }
                    c = (char)(num << 12 | num2 << 8 | num3 << 4 | num4);
                    i += 5;
                    urlDecoder.AddChar(c);
                }
                else
                {
                    int num5 = HexToInt(s[i + 1]);
                    int num6 = HexToInt(s[i + 2]);
                    if (num5 < 0 || num6 < 0)
                    {
                        goto IL_106;
                    }
                    byte b = (byte)(num5 << 4 | num6);
                    i += 2;
                    urlDecoder.AddByte(b);
                }
            IL_120:
                i++;
                continue;
            IL_106:
                if ((c & 'ﾀ') == '\0')
                {
                    urlDecoder.AddByte((byte)c);
                    goto IL_120;
                }
                urlDecoder.AddChar(c);
                goto IL_120;
            }
            return urlDecoder.GetString();
        }
        public static string UrlEncode(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            return Encoding.ASCII.GetString(UrlEncodeToBytes(str, e));
        }
        public static byte[] UrlEncodeToBytes(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            byte[] bytes = e.GetBytes(str);
            return UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
        }
        private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
        {
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < count; i++)
            {
                char c = (char)bytes[offset + i];
                if (c == ' ')
                {
                    num++;
                }
                else if (!IsSafe(c))
                {
                    num2++;
                }
            }
            if (!alwaysCreateReturnValue && num == 0 && num2 == 0)
            {
                return bytes;
            }
            byte[] array = new byte[count + num2 * 2];
            int num3 = 0;
            for (int j = 0; j < count; j++)
            {
                byte b = bytes[offset + j];
                char c2 = (char)b;
                if (IsSafe(c2))
                {
                    array[num3++] = b;
                }
                else if (c2 == ' ')
                {
                    array[num3++] = 43;
                }
                else
                {
                    array[num3++] = 37;
                    array[num3++] = (byte)IntToHex(b >> 4 & 15);
                    array[num3++] = (byte)IntToHex((int)(b & 15));
                }
            }
            return array;
        }
        public static string UrlEncode(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            return Encoding.ASCII.GetString(UrlEncodeToBytes(bytes));
        }
        public static byte[] UrlEncodeToBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            return UrlEncodeToBytes(bytes, 0, bytes.Length);
        }
        public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
        {
            if (bytes == null && count == 0)
            {
                return null;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (offset < 0 || offset > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0 || offset + count > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return UrlEncodeBytesToBytesInternal(bytes, offset, count, true);
        }
        /// <summary>
        /// UrlEncode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            if (str == null)
            {
                return null;
            }
            return UrlEncode(str, Encoding.UTF8);
        }

        /// <summary>
        /// 将url进行解码
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ignoreAscii"></param>
        /// <returns></returns>
        private static string UrlEncodeUnicodeStringToStringInternal(string s, bool ignoreAscii)
        {
            if (s == null)
            {
                return null;
            }
            int length = s.Length;
            StringBuilder stringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                char c = s[i];
                if ((c & 'ﾀ') == '\0')
                {
                    if (ignoreAscii || IsSafe(c))
                    {
                        stringBuilder.Append(c);
                    }
                    else if (c == ' ')
                    {
                        stringBuilder.Append('+');
                    }
                    else
                    {
                        stringBuilder.Append('%');
                        stringBuilder.Append(IntToHex((int)(c >> 4 & '\u000f')));
                        stringBuilder.Append(IntToHex((int)(c & '\u000f')));
                    }
                }
                else
                {
                    stringBuilder.Append("%u");
                    stringBuilder.Append(IntToHex((int)(c >> 12 & '\u000f')));
                    stringBuilder.Append(IntToHex((int)(c >> 8 & '\u000f')));
                    stringBuilder.Append(IntToHex((int)(c >> 4 & '\u000f')));
                    stringBuilder.Append(IntToHex((int)(c & '\u000f')));
                }
            }
            return stringBuilder.ToString();
        }
        public static byte[] UrlDecodeToBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            return UrlDecodeToBytes(bytes, 0, (bytes != null) ? bytes.Length : 0);
        }
        public static byte[] UrlDecodeToBytes(byte[] bytes, int offset, int count)
        {
            if (bytes == null && count == 0)
            {
                return null;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (offset < 0 || offset > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0 || offset + count > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return UrlDecodeBytesFromBytesInternal(bytes, offset, count);
        }
        private static byte[] UrlDecodeBytesFromBytesInternal(byte[] buf, int offset, int count)
        {
            int num = 0;
            byte[] array = new byte[count];
            for (int i = 0; i < count; i++)
            {
                int num2 = offset + i;
                byte b = buf[num2];
                if (b == 43)
                {
                    b = 32;
                }
                else if (b == 37 && i < count - 2)
                {
                    int num3 = HexToInt((char)buf[num2 + 1]);
                    int num4 = HexToInt((char)buf[num2 + 2]);
                    if (num3 >= 0 && num4 >= 0)
                    {
                        b = (byte)(num3 << 4 | num4);
                        i += 2;
                    }
                }
                array[num++] = b;
            }
            if (num < array.Length)
            {
                byte[] array2 = new byte[num];
                Array.Copy(array, array2, num);
                array = array2;
            }
            return array;
        }

        public static byte[] UrlDecodeToBytes(string str)
        {
            if (str == null)
            {
                return null;
            }
            return UrlDecodeToBytes(str, Encoding.UTF8);
        }
        public static byte[] UrlDecodeToBytes(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            return UrlDecodeToBytes(e.GetBytes(str));
        }
    }
}
