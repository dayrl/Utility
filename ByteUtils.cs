/*************************************************************************
// Copyright (C) 2017  Qinchuan IoT Technology Co Ltd. 
// All Rights Reserved 
//
// FileName: D:\workspace\GMIS7\QCWebAPI\QCWebAPI\Models\Util\ByteUtils.cs
// Function Description：
//			byte code operate util
// 
// Creator:	zlj (Administrator)
// Create Time:	6:11:2017   10:53
//	
// Change History:
// Editor:
// Time:
// Comment:
//
// Version: V1.0.0
/*************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Zdd.Utility
{
    /// <summary>
    /// ByteUtils
    /// </summary>
    public class ByteUtils
    {
        /// <summary>
        /// ctr.
        /// </summary>
        public ByteUtils()
        {
        }
        /// <summary>
        /// short2Bytes
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static byte[] short2Bytes(short f)
        {
            return long2Bytes(f, longRealLen(f));
        }
        /// <summary>
        /// int2Bytes
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static byte[] int2Bytes(int f)
        {
            return long2Bytes(f, longRealLen(f));
        }
        /// <summary>
        /// long2Bytes
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static byte[] long2Bytes(long f)
        {
            return long2Bytes(f, longRealLen(f));
        }
        /// <summary>
        /// long2Bytes
        /// </summary>
        /// <param name="f"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] long2Bytes(long f, int length)
        {
            byte[] ret = new byte[length];
            int len = longRealLen(f);
            for (int i = 0; i < len; i++)
                ret[(length - len) + i] = (byte)(int)(f >> (len - 1 - i) * 8 & 255L);

            return ret;
        }
        /// <summary>
        /// longRealLen
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int longRealLen(long f)
        {
            long s = f;
            int i;
            for (i = 0; s > 0L; i++)
                s >>= 8;

            return i;
        }
        /// <summary>
        /// bytes2Short
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static short bytes2Short(byte[] bs)
        {
            return (short)(int)bytes2Long(bs);
        }
        /// <summary>
        /// bytes2Int
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static int bytes2Int(byte[] bs)
        {
            return (int)bytes2Long(bs);
        }
        /// <summary>
        /// bytes2Long
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static long bytes2Long(byte[] bs)
        {
            long ret = 0L;
            int length = bs.Length;
            for (int i = 0; i < length; i++)
                ret += (long)(bs[i] & 255) << (length - 1 - i) * 8;

            return ret;
        }
        /// <summary>
        /// char2Bytes
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static byte[] char2Bytes(char ch)
        {
            return BitConverter.GetBytes(ch);
        }
        /// <summary>
        /// bytes2Char
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static char bytes2Char(byte[] bs)
        {
            return BitConverter.ToChar(bs, 0);
        }
        /// <summary>
        /// str2Bytes
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] str2Bytes(String str, String encoding = "GBK")
        {
            try
            {
                return Encoding.GetEncoding(encoding).GetBytes(str);
            }
            catch (Exception )
            {
                return Encoding.Default.GetBytes(str);
            }
        }
        /// <summary>
        /// bytes2Str
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static String bytes2Str(byte[] bytes, String encoding="GBK")
        {
            try
            {
                return Encoding.GetEncoding(encoding).GetString(bytes);
            }
            catch (Exception )
            {
                return Encoding.Default.GetString(bytes);
            }
        }
        /// <summary>
        /// toBinaryStr
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static String toBinaryStr(byte[] bs)
        {
            StringBuilder sb = new StringBuilder();
            int tmp = 0;
            for (int i = 0; i < bs.Length; i++)
            {
                tmp = bs[i] & 255;
                sb.Append(Convert.ToString(tmp, 2).PadLeft(8, '0'));
            }

            int end = -1;
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == '0')
                    continue;
                end = i;
                break;
            }

            if (end != -1)
                sb.Remove(0, end);
            return sb.ToString();
        }
        /// <summary>
        /// 将二进制数组转换为16进制字符串表示
        /// 恢复时使用hexStr2Bytes
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static String toHexStr(byte[] buf)
        {
            return toHexStr(buf, false);
        }
        /// <summary>
        /// toHexStr
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static String toHexStr(byte[] buf, bool sep)
        {
            if (null == buf || buf.Length == 0) return "";
            StringBuilder sb = new StringBuilder(buf.Length * (sep ? 3 : 2));
            for (int i = 0; i < buf.Length; i++)
            {
                byte byte0 = buf[i];
                sb.Append(hexDigits[byte0 >> 4 & 0x0F]);
                sb.Append(hexDigits[byte0 & 0x0F]);
                if (sep && (i != buf.Length - 1))
                    sb.Append(' ');
            }

            return sb.ToString();
        }

        /// <summary>
        /// 将byte数组转换为十六进制字符串,输入001,输出:001(0x30 30 31)
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static String toHexAsciiStr(byte[] buf)
        {
            StringBuilder ret = new StringBuilder(Encoding.Default.GetString(buf));
            ret.Append(" (0X").Append(toHexStr(buf, true).Trim()).Append(')');
            return ret.ToString();
        }
        /// <summary>
        /// 将十六进制字符串转换为byte数组输出,长度必须为偶数,
        /// 只能包含数字字母，否则使用Encoding.Default.GetBytes
        /// 恢复时使用toHexStr
        /// @parm String hex : 30303131
        /// </summary>
        /// <param name="hex">如30303130</param>
        /// <returns></returns>
        public static byte[] hexStr2Bytes(String hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return null;
            }
            if (hex.Length % 2 != 0 || !System.Text.RegularExpressions.Regex.IsMatch(hex, @"^[0-9A-Fa-f]+$"))
            {
                return Encoding.Default.GetBytes(hex);
            }
            int i = 0;
            byte[] data = new byte[hex.Length / 2];
            for (int n = 0; n < hex.Length; n += 2)
            {
                String temp = hex.Substring(n, 2);
                int tt = Convert.ToInt32(temp, 16);
                data[i] = (byte)tt;
                i++;
            }

            return data;
        }
        /// <summary>
        /// 对两个16进制字符串进行异域 (两个字符串必须长度相等,长度必须为偶数,必须为16进制字符串)
        /// </summary>
        /// <param name="strhex1"></param>
        /// <param name="strhex2"></param>
        /// <returns></returns>
        public static byte[] XOR(string strhex1, string strhex2)
        {
            if(string.IsNullOrEmpty(strhex1) || string.IsNullOrEmpty(strhex2) || 
                strhex1.Length != strhex2.Length ||
            strhex1.Length%2!=0 || strhex2.Length%2 !=0)
            {
                return null;
            }
            byte[] buf1 = hexStr2Bytes(strhex1);
            byte[] buf2 = hexStr2Bytes(strhex2);
            
            if(null == buf1 || null == buf2 || buf1.Length != buf2.Length)
            {
                return null;
            }
            byte[] finalData = new byte[strhex1.Length/2];
            for(int i=0; i<buf1.Length; i++)
            {
                finalData[i] = (byte)(buf1[i] ^ buf2[i]);
            }
            return finalData;
        }
        /// <summary>
        /// 导出byte数组的字符串对比(每行输出32字节数据)
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String dumphex(byte[] bytes)
        {
            int bsLen = bytes.Length;
            String head = "-Location- -0--1--2--3--4--5--6--7--8--9--A--B--C--D--E--F--0--1--2--3--4--5--6--7--8--9--A--B--C--D--E--F- ---ASCII Code---"+Environment.NewLine;
            StringBuilder ret = new StringBuilder(head.Length + bsLen * 3);
            ret.Append(head);
            for (int i = 0; i < bsLen; i += 32)
            {
                ret.Append(lpadding(string.Format("{0:X}", i), 4, '0')).Append('(');
                ret.Append(lpadding(i.ToString(), 4, '0')).Append(") ");
                for (int j = 0; j < 32; j++)
                {
                    String hex = i + j < bsLen ? string.Format("{0:X}", (bytes[i + j] & 255)) : "..";
                    if (hex.Length < 2)
                        ret.Append("0");
                    ret.Append(hex).Append(' ');
                }

                ret.Append(' ');
                for (int j = 0; j < 32; j++)
                    if (i + j >= bsLen)
                        ret.Append('.');
                    else
                        if (bytes[i + j] < 20 && bytes[i + j] >= 0)
                            ret.Append('*');
                        else
                            if (bytes[i + j] > 0)
                                ret.Append((char)bytes[i + j]);
                            else
                                if (bsLen > i + j + 1)
                                {
                                    String s = Encoding.Default.GetString(bytes, i + j, 2);
                                    ret.Append(s);
                                    j++;
                                }
                                else
                                {
                                    ret.Append((char)bytes[i + j]);
                                }

                ret.Append(Environment.NewLine);
            }

            return ret.ToString();
        }
        /// <summary>
        /// 把指定的二进制值显示为适合记入日志的文本串(每行输出16字节数据)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string dumphexlite(byte[] data)
        {
            if (data == null) return "";
            string result = "";
            int start = 0;
            result += "-0--1--2--3--4--5--6--7--8--9--A--B--C--D--E--F- ---ASCII Code---"+Environment.NewLine;
            while (start < data.Length)
            {
                //if (!result.Equals("")) result += "\r\n";
                int len = data.Length - start;
                if (len > 16) len = 16;
                string strByte = BitConverter.ToString(data, start, len);
                strByte = strByte.Replace('-', ' ');
                if (len > 8)
                {
                    strByte = strByte.Substring(0, 23) + " " + strByte.Substring(24);
                }
                if (len < 16)
                {
                    strByte += new string(' ', (16 - len) * 3);
                }
                strByte += "  ";
                for (int i = 0; i < len; i++)
                {
                    char c = Convert.ToChar(data[start + i]);
                    if (char.IsControl(c))
                    {
                        c = '.';
                    }
                    strByte += c;
                }
                result += strByte;
                result+=Environment.NewLine;
                start += len;
            }
            return result;
        }
        /// <summary>
        /// 左填充
        /// </summary>
        /// <param name="s"></param>
        /// <param name="n"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        private static string lpadding(string s, int n, char padding)
        {
            return s.PadLeft(n, padding);
//             StringBuilder strbuf = new StringBuilder();
//             for (int i = 0; i < n - s.Length; i++)
//                 strbuf.Append(padding);
// 
//             strbuf.Append(s);
//             return strbuf.ToString();
        }
        /// <summary>
        /// 右填充
        /// </summary>
        /// <param name="s"></param>
        /// <param name="n"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        private static string rpadding(String s, int n, char padding)
        {
            return s.PadRight(n, padding);
        }
        /// <summary>
        /// 填充字节
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="ch"></param>
        /// <param name="num"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static byte[] fillByte(byte[] rs, byte ch, int num, bool left)
        {
            int rsLen = rs.Length;
            byte[] ret = new byte[Math.Abs(num)];
            if (left)
            {
                if (num >= rsLen)
                    Array.Copy(rs, 0, ret, num - rsLen, rsLen);
                else
                    Array.Copy(rs, 0, ret, 0, ret.Length);
            }
            else
                if (num >= rsLen)
                    Array.Copy(rs, 0, ret, 0, rsLen);
                else
                    Array.Copy(rs, 0, ret, 0, ret.Length);
            return ret;
        }

        public static byte[] removeFillByte(byte[] rs, byte ch, bool left)
        {
            int idx;
            byte[] ret;
            if (left)
            {
                if (rs[0] != ch)
                    return rs;
                idx = rs.Length;
                for (int i = 0; i < rs.Length; i++)
                {
                    if (rs[i] == ch)
                        continue;
                    idx = i;
                    break;
                }

                ret = new byte[rs.Length - idx];
                Array.Copy(rs, idx, ret, 0, ret.Length);
                return ret;
            }
            if (rs[rs.Length - 1] != ch)
                return rs;
            idx = -1;
            for (int i = rs.Length - 1; i >= 0; i--)
            {
                if (rs[i] == ch)
                    continue;
                idx = i;
                break;
            }

            ret = new byte[idx + 1];
            Array.Copy(rs, 0, ret, 0, ret.Length);
            return ret;
        }

        public static Byte[] wraps(byte[] bs)
        {
            Byte[] ret = new Byte[bs.Length];
            for (int i = 0; i < bs.Length; i++)
                ret[i] = bs[i];

            return ret;
        }

        public static byte[] unwraps(Byte[] bs)
        {
            byte[] ret = new byte[bs.Length];
            for (int i = 0; i < bs.Length; i++)
                ret[i] = bs[i];
            return ret;
        }

        public static byte[] reverse(byte[] bs)
        {
            int len = bs.Length;
            byte[] ret = new byte[len];
            for (int i = 0; i < len; i++)
                ret[len - 1 - i] = bs[i];

            return ret;
        }

        public static byte[] copyOf(byte[] original, int newLength)
        {
            byte[] copy = new byte[newLength];
            Array.Copy(original, 0, copy, 0, Math.Min(original.Length, newLength));
            return copy;
        }

        public static int indexOf(byte[] source, int fromIndex, byte[] dst)
        {
            if (fromIndex >= source.Length)
                return dst.Length != 0 ? -1 : source.Length;
            if (fromIndex < 0)
                fromIndex = 0;
            if (dst.Length == 0)
                return fromIndex;
            byte first = dst[0];
            int max = source.Length - dst.Length;
            for (int i = fromIndex; i <= max; i++)
            {
                if (source[i] != first)
                    while (++i <= max && source[i] != first) ;
                if (i <= max)
                {
                    int j = i + 1;
                    int end = (j + dst.Length) - 1;
                    for (int k = 1; j < end && source[j] == dst[k]; k++)
                        j++;

                    if (j == end)
                        return i;
                }
            }

            return -1;
        }

        public static String bytes2MockStr(byte[] bs)
        {
            int length = bs.Length;
            StringBuilder sb = new StringBuilder(length + 2);
            int visible = 0;
            int invisible = 0;
            for (int k = 0; k < length; k++)
            {
                int byteToInt = bs[k] & 255;
                if (byteToInt >= 129 && byteToInt <= 254 && k + 1 < length)
                {
                    int byteToInt2 = bs[k + 1] & 255;
                    if (byteToInt2 >= 64 && byteToInt2 <= 254)
                    {
                        if (visible == 0 && invisible == 0)
                            sb.Append("[");
                        else
                            if (visible == 0 && invisible > 0)
                                sb.Append("][");
                        sb.Append(Encoding.Default.GetString(bs, k, 2));
                        k++;
                        visible++;
                        invisible = 0;
                        if (k == length - 1)
                            sb.Append("]");
                        continue;
                    }
                }
                if (byteToInt == 9 || byteToInt == 10 || byteToInt == 13 || byteToInt >= 32 && byteToInt <= 126)
                {
                    if (visible == 0 && invisible == 0)
                        sb.Append("[");
                    else
                        if (visible == 0 && invisible > 0)
                            sb.Append("][");
                    sb.Append((char)byteToInt);
                    visible++;
                    invisible = 0;
                }
                else
                {
                    if (invisible == 0 && visible == 0)
                        sb.Append("[0X");
                    else
                        if (invisible == 0 && visible > 0)
                            sb.Append("][0X");
                    String t = string.Format("{0:X}", byteToInt);
                    if (t.Length == 1)
                        t = (new StringBuilder("0")).Append(t).ToString();
                    sb.Append(t.ToUpper());
                    invisible++;
                    visible = 0;
                }
                if (k == length - 1)
                    sb.Append("]");
            }

            return sb.ToString();
        }

        public static char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        ////
        /// <summary>
        /// 十进制转换为二进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string DecToBin(string x)
        {
            string z = null;
            int X = Convert.ToInt32(x);
            int i = 0;
            long a, b = 0;
            while (X > 0)
            {
                a = X % 2;
                X = X / 2;
                b = b + a * Pow(10, i);
                i++;
            }
            z = Convert.ToString(b);
            return z;
        }

        /// <summary>
        /// 16进制转ASCII码
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static string HexToAscii(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexString.Length - 2; i += 2)
            {
                sb.Append(
                    Convert.ToString(
                        Convert.ToChar(Int32.Parse(hexString.Substring(i, 2),
                                                   System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 十进制转换为八进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string DecToOtc(string x)
        {
            string z = null;
            int X = Convert.ToInt32(x);
            int i = 0;
            long a, b = 0;
            while (X > 0)
            {
                a = X % 8;
                X = X / 8;
                b = b + a * Pow(10, i);
                i++;
            }
            z = Convert.ToString(b);
            return z;
        }

        /// <summary>
        /// 十进制转换为十六进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string DecToHex(string x)
        {
            if (string.IsNullOrEmpty(x))
            {
                return "0";
            }
            string z = null;
            int X = Convert.ToInt32(x);
            Stack a = new Stack();
            int i = 0;
            while (X > 0)
            {
                a.Push(Convert.ToString(X % 16));
                X = X / 16;
                i++;
            }
            while (a.Count != 0)
                z += ToHex(Convert.ToString(a.Pop()));
            if (string.IsNullOrEmpty(z))
            {
                z = "0";
            }
            return z;
        }

        /// <summary>
        /// 二进制转换为十进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string BinToDec(string x)
        {
            string z = null;
            int X = Convert.ToInt32(x);
            int i = 0;
            long a, b = 0;
            while (X > 0)
            {
                a = X % 10;
                X = X / 10;
                b = b + a * Pow(2, i);
                i++;
            }
            z = Convert.ToString(b);
            return z;
        }

        /// <summary>
        /// 二进制转换为十进制，定长转换
        /// </summary>
        /// <param name="x"></param>
        /// <param name="iLength"></param>
        /// <returns></returns>
        public static string BinToDec(string x, short iLength)
        {
            StringBuilder sb = new StringBuilder();
            int iCount = 0;

            iCount = x.Length / iLength;

            if (x.Length % iLength > 0)
            {
                iCount += 1;
            }

            int X = 0;

            for (int i = 0; i < iCount; i++)
            {
                if ((i + 1) * iLength > x.Length)
                {
                    X = Convert.ToInt32(x.Substring(i * iLength, (x.Length - iLength)));
                }
                else
                {
                    X = Convert.ToInt32(x.Substring(i * iLength, iLength));
                }
                int j = 0;
                long a, b = 0;
                while (X > 0)
                {
                    a = X % 10;
                    X = X / 10;
                    b = b + a * Pow(2, j);
                    j++;
                }
                sb.AppendFormat("{0:D2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 二进制转换为十六进制，定长转换
        /// </summary>
        /// <param name="x"></param>
        /// <param name="iLength"></param>
        /// <returns></returns>
        public static string BinToHex(string x, short iLength)
        {
            StringBuilder sb = new StringBuilder();
            int iCount = 0;

            iCount = x.Length / iLength;

            if (x.Length % iLength > 0)
            {
                iCount += 1;
            }

            int X = 0;

            for (int i = 0; i < iCount; i++)
            {
                if ((i + 1) * iLength > x.Length)
                {
                    X = Convert.ToInt32(x.Substring(i * iLength, (x.Length - iLength)));
                }
                else
                {
                    X = Convert.ToInt32(x.Substring(i * iLength, iLength));
                }
                int j = 0;
                long a, b = 0;
                while (X > 0)
                {
                    a = X % 10;
                    X = X / 10;
                    b = b + a * Pow(2, j);
                    j++;
                }
                //前补0
                sb.Append(DecToHex(b.ToString()));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 八进制转换为十进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string OctToDec(string x)
        {
            string z = null;
            int X = Convert.ToInt32(x);
            int i = 0;
            long a, b = 0;
            while (X > 0)
            {
                a = X % 10;
                X = X / 10;
                b = b + a * Pow(8, i);
                i++;
            }
            z = Convert.ToString(b);
            return z;
        }


        /// <summary>
        /// 十六进制转换为十进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string HexToDec(string x)
        {
            if (string.IsNullOrEmpty(x))
            {
                return "0";
            }
            int n1 = Convert.ToInt32(x, 16);
            return n1.ToString();
            /*
            string z = null;
            Stack a = new Stack();
            int i = 0, j = 0, l = x.Length;
            long Tong = 0;
            while (i < l)
            {
                a.Push(ToDec(Convert.ToString(x[i])));
                i++;
            }
            while (a.Count != 0)
            {
                Tong = Tong + Convert.ToInt64(a.Pop()) * Pow(16, j);
                j++;
            }
            z = Convert.ToString(Tong);
            return z;*/
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static long Pow(long x, long y)
        {
            int i = 1;
            long X = x;
            if (y == 0)
                return 1;
            while (i < y)
            {
                x = x * X;
                i++;
            }
            return x;
        }

        /// <summary>
        /// 将16进制字符串转换为十进制字符串
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static string ToDec(string x)
        {
            switch (x)
            {
                case "A":
                    return "10";
                case "B":
                    return "11";
                case "C":
                    return "12";
                case "D":
                    return "13";
                case "E":
                    return "14";
                case "F":
                    return "15";
                default:
                    return x;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static string ToHex(string x)
        {
            switch (x)
            {
                case "10":
                    return "A";
                case "11":
                    return "B";
                case "12":
                    return "C";
                case "13":
                    return "D";
                case "14":
                    return "E";
                case "15":
                    return "F";
                default:
                    return x;
            }
        }

        /// <summary>
        /// 将16进制BYTE数组转换成16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
        /////
        /// <summary>
        /// 由结构体转换为byte数组
        /// </summary>
        public static  byte[] StructureToByte<T>(T structure)
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[size];
            IntPtr bufferIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, bufferIntPtr, true);
                Marshal.Copy(bufferIntPtr, buffer, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(bufferIntPtr);
            }
            return buffer;
        }

        /// <summary>
        /// 由byte数组转换为结构体
        /// </summary>
        public static T ByteToStructure<T>(byte[] dataBuffer)
        {
            object structure = null;
            int size = Marshal.SizeOf(typeof(T));
            IntPtr allocIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(dataBuffer, 0, allocIntPtr, size);
                structure = Marshal.PtrToStructure(allocIntPtr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(allocIntPtr);
            }
            return (T)structure;
        }
        /// <summary>
        /// 对象序列化成byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ObjectToBytes(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// byte[]序列化成对象
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        public static object BytesToObject(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(ms);
            }
        }
#region Util
        /// <summary>  
        /// 把int32类型的数据转存到2个字节的byte数组中  
        /// 用于存放报文长度
        /// </summary>  
        /// <param name="m">int32类型的数据</param>  
        /// <returns>2个字节大小的byte数组</returns>  
        public static byte[] IntToByteArray(Int32 m)
        {
            byte[] arry = new byte[2];
            arry[0] = (byte)((m & 0xFF00)>>8);
            arry[1] = (byte)((m & 0x00FF));

            return arry;
        }
        /// <summary>
        /// 将2个字节的二进制缓冲区还原为整型数据
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        public static int ByteArrayToInt(byte[] buf)
        {
            if (null == buf || buf.Length < 2) return 0;
            return (buf[0] << 8) + buf[1];
        }
        /// <summary>
        /// BCD压缩(将两位十进制数字压缩为1位BCD),如12压缩为0x12
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte DEC2BCD(byte b)
        {
            //高四位
            byte b1 = (byte)(b / 10);
            //低四位
            byte b2 = (byte)(b % 10);
            return (byte)((b1 << 4) | b2);
        }

        /// <summary>
        /// 将BCD一字节数据还原为 十进制数据,如将0x12还原为十进制数12
        /// </summary>
        /// <param name="b" />字节数
        /// <returns>返回转换后的10进制数</returns>
        public static byte BCD2DEC(byte b)
        {
            //高四位
            byte b1 = (byte)((b >> 4) & 0xF);
            //低四位
            byte b2 = (byte)(b & 0xF);
            return (byte)(b1 * 10 + b2);
        }
        /// <summary>
        /// 将10进制数转换为16进制数,如12转换为0x12
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int DecToHex(byte val)
        {
            int res = 0;
            int bit = 0;
            while (val >= 10)
            {
                res |= (val % 10 << bit);
                val /= 10;
                bit += 4;
            }
            res |= val << bit;
            return res;
        }
        /// <summary>
        /// 将16进制转换为10进制,如0x12转换为12
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        public static byte HexToDec(int vals)
        {
            int c = 1;
            byte b = 0;
            while (vals > 0)
            {
                b += (byte)((vals & 0xf) * c);
                c *= 10;
                vals >>= 4;
            }
            return b;
        }
        /// <summary>
        /// BCD转字符串
        /// </summary>
        /// <param name="bcd"></param>
        /// <returns></returns>
        public static string BCD2String(byte[] bcd)
        {
            if (null == bcd) return "";
            byte[] ascBuf = new byte[bcd.Length * 2];
            for (int i = 0; i < bcd.Length; i++)
            {
                byte c=bcd[i];
		        byte b1 = (((c>>4) + '0') > '9' )? (byte)((c>>4) - 10 + 'A'):  (byte)((c>>4) + '0' );
                ascBuf[i * 2] = b1;
                byte b2 = ((c & 0x0f) + '0' > '9') ? (byte)((c & 0x0f) - 10 + 'A') : (byte)((c & 0x0f) + '0');
                ascBuf[i * 2 + 1] += b2;
            }
            return Encoding.Default.GetString(ascBuf);
        }
        /// <summary>
        /// 字符串BCD压缩,不足偶数位前补0
        /// </summary>
        /// <param name="strTemp"></param>
        /// <returns></returns>
        private static Byte[] String2BCD(string strTemp)
        {
            try
            {
                string asc = strTemp;
                int len = asc.Length;
                if (len % 2 != 0)
                {
                    asc = ("0"+asc);//位数不够偶数位前补零
                    len += 1;
                }
                
                Byte[] p = new Byte[len];
                Byte[] bcd = new Byte[len / 2];
                for (int i = 0; i < len; i++)
                {
                    if ((asc[i] >= '0') && (asc[i] <= '9'))
                    {
                        p[i] = (byte)(asc[i] - '0');
                    }
                    else if ((asc[i] >= 'a') && (asc[i] <= 'f'))
                    {
                        p[i] = (byte)(asc[i] - 'a' + 10);
                    }
                    else if ((asc[i] >= 'A') && (asc[i] <= 'f'))
                    {
                        p[i] = (byte)(asc[i] - 'A' + 10);
                    }
                    else
                    {
                        p[i] = 0;
                    }
                }
                int j = (len + len % 2) / 2;
                if (len % 2 != 0)
                {
                    p[len + 1] = 0;
                }

                for (int i = 0; i < j; i++)
                {
                    bcd[i] = (byte)((p[2 * i] & 0x0f) << 4);
                    bcd[i] |= (byte)(p[2 * i + 1] & 0x0f);
                }
                return bcd;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 压缩BCD
        /// </summary>
        /// <param name="strTemp">要压缩的16进制字符串,长度为偶数位,否则会前补0</param>
        /// <param name="IntLen">要前补齐0的位数</param>
        /// <returns></returns>
        public  static Byte[] String2BCD(string strTemp, int IntLen=0)
        {
            try
            {
                Byte[] Temp = String2BCD(strTemp.Trim());
                if (IntLen != 0)
                {
                    Byte[] return_Byte = new Byte[IntLen];
                    if (Temp.Length < IntLen)
                    {
                        for (int i = 0; i < IntLen - Temp.Length; i++)
                        {
                            return_Byte[i] = 0x00;
                        }
                    }
                    Array.Copy(Temp, 0, return_Byte, IntLen - Temp.Length, Temp.Length);
                    return return_Byte;
                }
                else
                {
                    return Temp;
                }
            }
            catch
            { return null; }
        }
        /// <summary>
        /// BCD压缩
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte ConvertBCD(byte b)
        {
            //高四位
            byte b1 = (byte)(b / 10);
            //低四位
            byte b2 = (byte)(b % 10);
            return (byte)((b1 << 4) | b2);
        }

        /// <summary>
        /// 将BCD一字节数据转换到byte 十进制数据
        /// </summary>
        /// <param name="b" />字节数
        /// <returns>返回转换后的BCD码</returns>
        public static byte BCDToInt(byte b)
        {
            //高四位
            byte b1 = (byte)((b >> 4) & 0xF);
            //低四位
            byte b2 = (byte)(b & 0xF);
            return (byte)(b1 * 10 + b2);
        }

        public static int ToBCD(byte val)
        {
            int res = 0;
            int bit = 0;
            while (val >= 10)
            {
                res |= (val % 10 << bit);
                val /= 10;
                bit += 4;
            }
            res |= val << bit;
            return res;
        }
        /// <summary>
        /// 根据PIN明文,获取8字节的PINBLOCK
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        public static byte[] GetPinBlock(string pin,string pan="")
        {
            if (string.IsNullOrEmpty(pin)) return null;
            bool bFlag = false;//寄数位标志
            if (Convert.ToBoolean(pin.Length & 1))//数字的二进制码最后1位是1则为奇数
            {
                pin += "0";//数位为奇数时后面补0
                bFlag = true;
            }
            Byte[] bcd = new Byte[pin.Length / 2];
            for (int i = 0; i < (pin.Length / 2); i++)
            {
                bcd[i] = (Byte)(((pin[i * 2] - '0') << 4) | (pin[i * 2 + 1] - '0'));
            }
            if (bFlag)
            {
                bcd[bcd.Length - 1] |= 0X0F;
            }
            byte[] block1 = new byte[8] { 0X0F, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF };
            block1[0] = Convert.ToByte(pin.Length);
            Array.Copy(bcd, 0, block1, 1, bcd.Length);
            if(string.IsNullOrEmpty(pan))
            {
                return block1;
            }
            string trimPan = pan.Remove(pan.Length-1);//取主账号的右12位(不包括最右边的校验位),主账号不足12位左补0
            if (pan.Length < 12)
            {
                trimPan.PadLeft(12, '0');
            }
            else
            {
                trimPan = trimPan.Substring(trimPan.Length - 12, 12);
            }
            byte[] block2 = String2BCD(trimPan, 8);
            byte[] finalBlock = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                finalBlock[i] = (byte)(block1[i] ^ block2[i]);
            }
            return finalBlock;
        }
        /// <summary>
        /// SM4算法:根据PIN明文,获取8字节的PINBLOCK
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        public static byte[] GetPinBlockSM4(string pin, string pan = "")
        {
            if (string.IsNullOrEmpty(pin)) return null;
            bool bFlag = false;//寄数位标志
            if (Convert.ToBoolean(pin.Length & 1))//数字的二进制码最后1位是1则为奇数
            {
                pin += "0";//数位为奇数时后面补0
                bFlag = true;
            }
            Byte[] bcd = String2BCD(pin);
//             for (int i = 0; i < (pin.Length / 2); i++)
//             {
//                 bcd[i] = (Byte)(((pin[i * 2] - '0') << 4) | (pin[i * 2 + 1] - '0'));
//             }
            if (bFlag)
            {
                bcd[bcd.Length - 1] |= 0X0F;
            }
            byte[] block1 = new byte[16] { 0X0F, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF, 0XFF };
            block1[0] = Convert.ToByte(pin.Length);
            Array.Copy(bcd, 0, block1, 1, bcd.Length);
            if (string.IsNullOrEmpty(pan))
            {
                return block1;
            }
            string trimPan = pan.Remove(pan.Length - 1);//取主账号的右12位(不包括最右边的校验位),主账号不足12位左补0
            if (pan.Length < 12)
            {
                trimPan.PadLeft(12, '0');
            }
            else
            {
                trimPan = trimPan.Substring(trimPan.Length - 12, 12);
            }
            byte[] blockpan = String2BCD(trimPan);
            byte[] block2 = new byte[16];
            Array.Copy(blockpan, 0, block2, 10, 6);
            byte[] finalBlock = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                finalBlock[i] = (byte)(block1[i] ^ block2[i]);
            }
            return finalBlock;
            /*
            byte[] btLen = String2BCD(finalBlock.Length.ToString(), 1);
            byte[] finalPin = new byte[64] { btLen[0], 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            Array.Copy(finalPin, 1, finalBlock, 0, finalBlock.Length);
            return finalPin;*/
        }
        /// <summary>
        /// 组合成要参与MAC计算的源字符串
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static string GetMacStr(List<string> fields)
        {
            if (null == fields || fields.Count == 0) return "";
            string strMAC = "";
            for (int i = 0; i < fields.Count;  i++)
            {
                strMAC += fields[i];
                /*//银联标准
                strMAC += ProcessMacStr(fields[i]);
                if (i < fields.Count - 1)
                {
                    strMAC += " ";//在域和域之间插入一个空格;
                }*/
            }
            return strMAC;
        }
        /// <summary>
        /// 处理要进行MAC计算的字符串域
        /// a) 带长度值的域在计算 MAC 时应包含其长度值信息;
        /// b) 在域和域之间插入一个空格;
        /// c) 所有的小写字母转换成大写字母;
        /// d) 除了字母(A-Z),数字(0-9),空格,逗号(,)和点号(.)以外的字符都删去;
        /// e) 删去所有域的起始空格和结尾空格;
        /// f) 多于一个的连续空格,由一个空格代替.
        /// </summary>
        /// <param name="src">要处理的字符串,假如已经完成a b两步</param>
        /// <returns></returns>
        public static string ProcessMacStr(string src)
        {
            if (string.IsNullOrEmpty(src)) return "";
            string strTmp = src.ToUpper();
            if (strTmp.StartsWith(" "))//去掉首部空格
            {
                strTmp = strTmp.Substring(1);
            }
            if (strTmp.EndsWith(" "))
            {
                strTmp = strTmp.Substring(0, strTmp.Length - 1);
            }
            string strFinal = "";
            char chLast = strTmp[0];
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (i == 0)
                {
                    if (IsValidMac(strTmp[i]))
                    {
                        strFinal += strTmp[i];
                    }
                    continue;
                }
                if (strTmp[i] == ' ' && chLast == ' ')
                {
                    continue;
                }
                if (IsValidMac(strTmp[i]))
                {
                    strFinal += strTmp[i];
                }
                chLast = strTmp[i];
            }
            return strFinal;
        }
        private static bool IsValidMac(char c)
        {
            return c == ' ' || c == ',' || c == '.' || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');
        }
        #endregion
    }
}