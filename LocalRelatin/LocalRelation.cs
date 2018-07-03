namespace Zdd.Utility.LocalRelation
{
    using System;
    using System.Net;
using System.Collections.Generic;
  /********************************************************************
	created:	    2009/04/23
	created:	    23:4:2009   11:50
	filename: 	E:\code\c# code\stivs-v3.0\ClassLibrary\Utility\LocalRelatin\LocalRelation.cs
	file path:	E:\code\c# code\stivs-v3.0\ClassLibrary\Utility\LocalRelatin
	file base:	LocalRelation
	file ext:	    cs
	author:		zdd(0438)	
	purpose:	    获取本地信息的一些相关函数接口
    *********************************************************************/
    public class LocaNetwork
    {
        /// <summary>
        /// 获取本地主机的IPV4地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP4Address()
        {
            string Ip4Addr = string.Empty;
            foreach (IPAddress Ipa in Dns.GetHostAddresses(Environment.MachineName))
            {
                if (0 == string.CompareOrdinal(Ipa.AddressFamily.ToString(), "InterNetwork"))
                {
                    Ip4Addr = Ipa.ToString();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(Ip4Addr))
                return Ip4Addr;

            foreach (IPAddress Ipa in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (0 == string.CompareOrdinal(Ipa.AddressFamily.ToString(), "InterNetwork"))
                {
                    Ip4Addr = Ipa.ToString();
                    break;
                }
            }
            return Ip4Addr;
        }

        public static List<string> GetLocalIP4AddressList()
        {
            List<string> lsResult = new List<string>();

            foreach (IPAddress Ipa in Dns.GetHostAddresses(Environment.MachineName))
            {
                if (0 == string.CompareOrdinal(Ipa.AddressFamily.ToString(), "InterNetwork"))
                {
                    lsResult.Add(Ipa.ToString());
                }
            }

            if (lsResult.Count > 0)
                return lsResult;

            foreach (IPAddress Ipa in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (0 == string.CompareOrdinal(Ipa.AddressFamily.ToString(), "InterNetwork"))
                {
                    lsResult.Add(Ipa.ToString());
                }
            }
            return lsResult;
        }
    }
}
