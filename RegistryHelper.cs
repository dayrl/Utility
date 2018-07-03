using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace Zdd.Utility
{
	/// <summary>
	/// ע�����������ࡣ
	/// </summary>
	public static class RegistryHelper
	{
		#region ��ȡֵ

		/// <summary>
		/// ��ȫ�Ĵ��Ѵ� RegistryKey �л�ȡ Int64ֵ��
		/// </summary>
		/// <param name="key">Ҫ��ȡ�� RegistryKey��</param>
		/// <param name="name">Ҫ��ȡ�ļ�����</param>
		/// <param name="defaultValue">��ֵ�����ڻ��ȡʧ�ܷ��ص�Ĭ��ֵ��</param>
		/// <returns>���ض�ȡ��ֵ����ʧ���򷵻� defaultValue��</returns>
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
		/// ��ȫ�Ĵ��Ѵ� RegistryKey �л�ȡ Int32ֵ��
		/// </summary>
		/// <param name="key">Ҫ��ȡ�� RegistryKey��</param>
		/// <param name="name">Ҫ��ȡ�ļ�����</param>
		/// <param name="defaultValue">��ֵ�����ڻ��ȡʧ�ܷ��ص�Ĭ��ֵ��</param>
		/// <returns>���ض�ȡ��ֵ����ʧ���򷵻� defaultValue��</returns>
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
		/// ��ȫ�Ĵ��Ѵ� RegistryKey �л�ȡ Double ֵ��
		/// </summary>
		/// <param name="key">Ҫ��ȡ�� RegistryKey��</param>
		/// <param name="name">Ҫ��ȡ�ļ�����</param>
		/// <param name="defaultValue">��ֵ�����ڻ��ȡʧ�ܷ��ص�Ĭ��ֵ��</param>
		/// <returns>���ض�ȡ��ֵ����ʧ���򷵻� defaultValue��</returns>
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
		/// ��ȫ�Ĵ��Ѵ� RegistryKey �л�ȡ String ֵ��
		/// </summary>
		/// <param name="key">Ҫ��ȡ�� RegistryKey��</param>
		/// <param name="name">Ҫ��ȡ�ļ�����</param>
		/// <param name="defaultValue">��ֵ�����ڻ��ȡʧ�ܷ��ص�Ĭ��ֵ��</param>
		/// <returns>���ض�ȡ��ֵ����ʧ���򷵻� defaultValue��</returns>
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
		/// ��ȫ�Ĵ��Ѵ� RegistryKey �л�ȡ DateTime ֵ��
		/// </summary>
		/// <param name="key">Ҫ��ȡ�� RegistryKey��</param>
		/// <param name="name">Ҫ��ȡ�ļ�����</param>
		/// <param name="defaultValue">��ֵ�����ڻ��ȡʧ�ܷ��ص�Ĭ��ֵ��</param>
		/// <returns>���ض�ȡ��ֵ����ʧ���򷵻� defaultValue��</returns>
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
		/// ��ȫ�Ĵ��Ѵ� RegistryKey �л�ȡ Boolean ֵ��
		/// </summary>
		/// <param name="key">Ҫ��ȡ�� RegistryKey��</param>
		/// <param name="name">Ҫ��ȡ�ļ�����</param>
		/// <param name="defaultValue">��ֵ�����ڻ��ȡʧ�ܷ��ص�Ĭ��ֵ��</param>
		/// <returns>���ض�ȡ��ֵ����ʧ���򷵻� defaultValue��</returns>
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

		#region ��ע����

		/// <summary>
		/// ��ֻ����ʽ�� HKEY_LOCAL_MACHINE �µ��ӽ���
		/// ��ָ���� subKey �����ڣ��򴴽�����
		/// </summary>
		/// <param name="subKey">׼���򿪵��Ӽ���</param>
		/// <returns>���ش򿪵� RegistryKey ������ʧ�ܷ��� null��</returns>
		public static RegistryKey OpenKeyForRead(string subKey)
		{
			return OpenKeyForRead(Registry.LocalMachine, subKey);
		}

		/// <summary>
		/// ��ֻ����ʽ�� RegistryKey �µ��ӽ���
		/// ��ָ���� subKey �����ڣ��򴴽�����
		/// </summary>
		/// <param name="key">Ҫ������ĸ��</param>
		/// <param name="subKey">׼���򿪵��Ӽ���</param>
		/// <returns>���ش򿪵� RegistryKey ������ʧ�ܷ��� null��</returns>
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
		/// �Զ�д��ʽ�� HKEY_LOCAL_MACHINE �µ��ӽ���
		/// ��ָ���� subKey �����ڣ��򴴽�����
		/// </summary>
		/// <param name="subKey">׼���򿪵��Ӽ���</param>
		/// <returns>���ش򿪵� RegistryKey ������ʧ�ܷ��� null��</returns>
		public static RegistryKey OpenKeyForWrite(string subKey)
		{
			return OpenKeyForWrite(Registry.LocalMachine, subKey);
		}

		/// <summary>
		/// ��ֻ����ʽ�� RegistryKey �µ��ӽ���
		/// ��ָ���� subKey �����ڣ��򴴽�����
		/// </summary>
		/// <param name="key">Ҫ������ĸ��</param>
		/// <param name="subKey">׼���򿪵��Ӽ���</param>
		/// <returns>���ش򿪵� RegistryKey ������ʧ�ܷ��� null��</returns>
		public static RegistryKey OpenKeyForWrite(RegistryKey key, string subKey)
		{
			RegistryKey regKey;

			regKey = key.OpenSubKey(subKey, true);
		    if (regKey == null)
		        regKey = key.CreateSubKey(subKey);


		    return regKey;
		}
        /// <summary> 
        /// �ж��Ƿ��Ѿ����ڴ˼�ֵ,�˴�������Form_Load����ʹ�á�
        ///������ڣ��˵�[�����Զ�����]ǰ����Դ��϶Թ�
        ///��������ڣ��򲻲���
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
        /// д���ֵ��ע���
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
                        //�����ǵĳ���ӽ�ȥ
                        run.SetValue(keyName, keyValue);
                        //ע�⣬һ��Ҫ�رգ�ע���Ӧ�á�
                        hklm.Close();
                        return true;
                    }
                }
            }
            catch //���ǲ����쳣�� 
            {
                return false;
            }
        }

        /// <summary> ɾ��ע����м�ֵ
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
                        //ע��˴��õ���GetValueNames()
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
        /// д��ע����Զ�������
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
