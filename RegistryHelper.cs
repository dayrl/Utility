using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace Zdd.Utility
{
	/// <summary>
	/// 注册表访问助手类。
	/// </summary>
	public static class RegistryHelper
	{
		#region 读取值

		/// <summary>
		/// 安全的从已打开 RegistryKey 中获取 Int64值。
		/// </summary>
		/// <param name="key">要读取的 RegistryKey。</param>
		/// <param name="name">要读取的键名。</param>
		/// <param name="defaultValue">若值不存在或读取失败返回的默认值。</param>
		/// <returns>返回读取的值，若失败则返回 defaultValue。</returns>
		public static long GetSafeInt64(RegistryKey key, string name, long defaultValue)
		{
			long ret = defaultValue;
			try
			{
				ret = Convert.ToInt64(key.GetValue(name, defaultValue));
			}
			catch (SystemException ex)
			{
				Debug.Fail(ex.Message);
			}

			return ret;
		}

		/// <summary>
		/// 安全的从已打开 RegistryKey 中获取 Int32值。
		/// </summary>
		/// <param name="key">要读取的 RegistryKey。</param>
		/// <param name="name">要读取的键名。</param>
		/// <param name="defaultValue">若值不存在或读取失败返回的默认值。</param>
		/// <returns>返回读取的值，若失败则返回 defaultValue。</returns>
		public static int GetSafeInt32(RegistryKey key, string name, int defaultValue)
		{
			int ret = defaultValue;
			try
			{
				ret = Convert.ToInt32(key.GetValue(name, defaultValue));
			}
			catch (SystemException ex)
			{
				Debug.Fail(ex.Message);
			}

			return ret;
		}

		/// <summary>
		/// 安全的从已打开 RegistryKey 中获取 Double 值。
		/// </summary>
		/// <param name="key">要读取的 RegistryKey。</param>
		/// <param name="name">要读取的键名。</param>
		/// <param name="defaultValue">若值不存在或读取失败返回的默认值。</param>
		/// <returns>返回读取的值，若失败则返回 defaultValue。</returns>
		public static double GetSafeDouble(RegistryKey key, string name, double defaultValue)
		{
			double ret = defaultValue;
			try
			{
				ret = Convert.ToDouble(key.GetValue(name, defaultValue));
			}
			catch (SystemException ex)
			{
				Debug.Fail(ex.Message);
			}

			return ret;
		}

		/// <summary>
		/// 安全的从已打开 RegistryKey 中获取 String 值。
		/// </summary>
		/// <param name="key">要读取的 RegistryKey。</param>
		/// <param name="name">要读取的键名。</param>
		/// <param name="defaultValue">若值不存在或读取失败返回的默认值。</param>
		/// <returns>返回读取的值，若失败则返回 defaultValue。</returns>
		public static string GetSafeString(RegistryKey key, string name, string defaultValue)
		{
			string ret = defaultValue;
			try
			{
				ret = (string)key.GetValue(name, defaultValue);
			}
			catch (SystemException ex)
			{
				Debug.Fail(ex.Message);
			}

			return ret;
		}

		/// <summary>
		/// 安全的从已打开 RegistryKey 中获取 DateTime 值。
		/// </summary>
		/// <param name="key">要读取的 RegistryKey。</param>
		/// <param name="name">要读取的键名。</param>
		/// <param name="defaultValue">若值不存在或读取失败返回的默认值。</param>
		/// <returns>返回读取的值，若失败则返回 defaultValue。</returns>
		public static DateTime GetSafeDateTime(RegistryKey key, string name, DateTime defaultValue)
		{
			DateTime ret = defaultValue;
			try
			{
				ret = Convert.ToDateTime(key.GetValue(name, defaultValue));
			}
			catch (SystemException ex)
			{
				Debug.Fail(ex.Message);
			}

			return ret;
		}

		/// <summary>
		/// 安全的从已打开 RegistryKey 中获取 Boolean 值。
		/// </summary>
		/// <param name="key">要读取的 RegistryKey。</param>
		/// <param name="name">要读取的键名。</param>
		/// <param name="defaultValue">若值不存在或读取失败返回的默认值。</param>
		/// <returns>返回读取的值，若失败则返回 defaultValue。</returns>
		public static bool GetSafeBoolean(RegistryKey key, string name, bool defaultValue)
		{
			bool ret = defaultValue;
			try
			{
				ret = Convert.ToBoolean(key.GetValue(name, defaultValue));
			}
			catch (SystemException ex)
			{
				Debug.Fail(ex.Message);
			}

			return ret;
		}

		#endregion

		#region 打开注册表键

		/// <summary>
		/// 以只读方式打开 HKEY_LOCAL_MACHINE 下的子健。
		/// 若指定的 subKey 不存在，则创建它。
		/// </summary>
		/// <param name="subKey">准备打开的子键。</param>
		/// <returns>返回打开的 RegistryKey ，若打开失败返回 null。</returns>
		public static RegistryKey OpenKeyForRead(string subKey)
		{
			return OpenKeyForRead(Registry.LocalMachine, subKey);
		}

		/// <summary>
		/// 以只读方式打开 RegistryKey 下的子健。
		/// 若指定的 subKey 不存在，则创建它。
		/// </summary>
		/// <param name="key">要开的项的父项。</param>
		/// <param name="subKey">准备打开的子键。</param>
		/// <returns>返回打开的 RegistryKey ，若打开失败返回 null。</returns>
		public static RegistryKey OpenKeyForRead(RegistryKey key, string subKey)
		{
			RegistryKey regKey;

			regKey = key.OpenSubKey(subKey, true);
			if (regKey == null)
			{
				regKey = key.CreateSubKey(subKey);
				regKey.Close();
				regKey = key.OpenSubKey(subKey);
			}

			return regKey;
		}

		/// <summary>
		/// 以读写方式打开 HKEY_LOCAL_MACHINE 下的子健。
		/// 若指定的 subKey 不存在，则创建它。
		/// </summary>
		/// <param name="subKey">准备打开的子键。</param>
		/// <returns>返回打开的 RegistryKey ，若打开失败返回 null。</returns>
		public static RegistryKey OpenKeyForWrite(string subKey)
		{
			return OpenKeyForWrite(Registry.LocalMachine, subKey);
		}

		/// <summary>
		/// 以只读方式打开 RegistryKey 下的子健。
		/// 若指定的 subKey 不存在，则创建它。
		/// </summary>
		/// <param name="key">要开的项的父项。</param>
		/// <param name="subKey">准备打开的子键。</param>
		/// <returns>返回打开的 RegistryKey ，若打开失败返回 null。</returns>
		public static RegistryKey OpenKeyForWrite(RegistryKey key, string subKey)
		{
			RegistryKey regKey;

			regKey = key.OpenSubKey(subKey, true);
		    if (regKey == null)
		        regKey = key.CreateSubKey(subKey);


		    return regKey;
		}
        /// <summary> 
        /// 判断是否已经存在此键值,此处可以在Form_Load中来使用。
        ///如果存在，菜单[开机自动运行]前面可以打上对钩
        ///如果不存在，则不操作
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private static bool IsExistKey(string keyName, string _keyValue)
        {
            bool _exist = false;
            try
            {
                using (RegistryKey hklm = Registry.LocalMachine)
                {
                    using (RegistryKey runs = hklm.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                    {
                        string[] runsName = runs.GetValueNames();
                        foreach (string strName in runsName)
                        {
                            if (strName.ToUpper() == keyName.ToUpper())
                            {
                                string keyValue = runs.GetValue(keyName).ToString();
                                if (keyValue.Equals(_keyValue))
                                {
                                    _exist = true;
                                }
                                return _exist;
                            }
                        }
                    }
                }
            }
            catch { }
            return _exist;
        }
        /// <summary>
        /// 写入键值到注册表
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        private static bool WriteKey(string keyName, string keyValue)
        {
            try
            {
                using (RegistryKey hklm = Registry.LocalMachine)
                {
                    using (RegistryKey run = hklm.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run"))
                    {
                        //将我们的程序加进去
                        run.SetValue(keyName, keyValue);
                        //注意，一定要关闭，注册表应用。
                        hklm.Close();
                        return true;
                    }
                }
            }
            catch //这是捕获异常的 
            {
                return false;
            }
        }

        /// <summary> 删除注册表中键值
        /// </summary>
        /// <param name="keyName"></param>
        private static void DeleteKey(string keyName)
        {
            try
            {
                using (RegistryKey hklm = Registry.LocalMachine)
                {
                    using (RegistryKey runs = hklm.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                    {
                        //注意此处用的是GetValueNames()
                        string[] runsName = runs.GetValueNames();
                        foreach (string strName in runsName)
                        {
                            if (strName.ToUpper() == keyName.ToUpper())
                                runs.DeleteValue(strName, false);
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 写入注册表自动启动项
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appPath"></param>
        public static void WiteRegAutoStart(string appName, string appPath,bool add=true)
        {
            string keyValue = appPath;
            string keyName = appName;
            try
            {
                if (add)
                {
                    WriteKey(keyName, keyValue);//add update
                    return;
                }
                else
                {
                    bool bExist = IsExistKey(keyName, keyValue);
                    if (bExist)
                    {
                        DeleteKey(keyName);
                    }
                }

            }
            catch { }
        }
		#endregion
	}
}
