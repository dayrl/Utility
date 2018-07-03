using System.Text.RegularExpressions;

namespace Zdd.Utility
{
	/// <summary>
	/// �ַ�����֤���ߡ�
	/// </summary>
	public static class StringValidator 
	{
		/// <summary>
		/// �ж�ָ�����ַ����Ƿ�����������֡�
		/// </summary>
		/// <param name="input">Ҫ�����ַ�����</param>
		/// <returns>ֻ�������ַ���True������False��</returns>
		public static bool IsNumber(string input)
		{
			return Regex.IsMatch(input, @"^\d+$");
		}
		
		/// <summary>
		/// �жϸ�����һ���ַ����Ƿ�ΪIP v4��ַ��
		/// </summary>
		/// <param name="address">Ҫ�����ַ�����</param>
		/// <returns>�����IPV4��ַ��ΪTrue������ΪFalse��</returns>
		public static bool IsIPv4(string address)
		{
			if (address == null)
				return false;

			string pattern = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
			return Regex.IsMatch(address, pattern);
		}

        /// <summary>
        /// Determines whether [is valid email] [the specified s].
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid email] [the specified s]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmail(string s)
        {
            return Regex.IsMatch(s, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        }
        /// <summary>
        /// Determines whether the specified s is numeric.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// 	<c>true</c> if the specified s is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(string s)
        {
            return Regex.IsMatch(s, @"^(-?\d+)(\.\d+)?$");

        }
        /// <summary>
        /// Determines whether [is zip code] [the specified s].
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// 	<c>true</c> if [is zip code] [the specified s]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsZipCode(string s)
        {
            return Regex.IsMatch(s, @"^\\d{6}$");
        }
        /// <summary>
        /// Determines whether the specified s is chinese.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// 	<c>true</c> if the specified s is chinese; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsChinese(string s)
        {
            return Regex.IsMatch(s, @"^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$");
        }
        /// <summary>
        /// Determines whether the specified s is HTTP.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// 	<c>true</c> if the specified s is HTTP; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsHttp(string s)
        {
            return Regex.IsMatch(s, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }

        /// <summary>
        /// ��������·��(�����ַ���)�Ƿ�Ϸ�
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="IsPathRooted">if set to <c>true</c> [is path rooted].</param>
        /// <returns></returns>
        public static bool PathValidity(string path, bool IsPathRooted)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            try
            {
                if (IsPathRooted && System.IO.Path.IsPathRooted(path) == false)
                    return false;

                string device = path.Substring(0, path.IndexOf(@"\") + 1);
                if (System.IO.Directory.Exists(device) == false)
                    return false;

                System.IO.Path.GetFullPath(path);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>�����ļ��������������ļ��������Ƿ�Ϸ�
        /// ������ʽ�������ļ��������������ļ��������Ƿ�Ϸ�
        /// ���ļ������ư��������ַ�Ϊ���Ϸ�������\ / : * ? " < > | 
        /// </summary>
        /// <param name="name">�ļ�������</param>
        /// <returns></returns>
        public static bool FolderAndFileNameValidity(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            Regex reg = new Regex(@"^[^\\\/\:\*\?\<\>\|\u0022]+$");
            return reg.IsMatch(name);
        }
	}
}