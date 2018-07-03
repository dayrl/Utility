namespace Zdd.Utility
{
    using System;

    public class IP2Net
    {
        /// <summary>
        /// ͬ����
        /// </summary>
        private readonly static object m_lock_string = new object();

        /// <summary>
        /// ͬ����
        /// </summary>
        private readonly static object m_lock_long = new object();
        /// <summary>
        /// ����IP��ַת��Ϊ����
        /// </summary>
        /// <param name="_dotIP"></param>
        /// <returns></returns>
        public static long Dot2LongIP(string _dotIP)
        {
            string dotIP = _dotIP;
            lock (m_lock_string)
            {
                string[] subIP = dotIP.Split('.');
                long ip = 16777216*Convert.ToInt64(subIP[0]) + 65536*Convert.ToInt64(subIP[1]) +
                          256*Convert.ToInt64(subIP[2]) + Convert.ToInt64(subIP[3]);
                return ip;
            }
        }

        /// <summary>
        /// ����������IP��ַ��ת��
        /// </summary>
        /// <param name="_longIP"></param>
        /// <returns></returns>
        public static string LongIP2Dot(long _longIP)
        {
            long longIP = _longIP;
            lock (m_lock_long)
            {
                System.Net.IPAddress ipaddr = new System.Net.IPAddress(longIP);
                byte[] bip = ipaddr.GetAddressBytes();
                long temp = BitConverter.ToUInt32(bip, 0);
                ipaddr = new System.Net.IPAddress(temp);
                return ipaddr.ToString();
            }
        }
    }
}
