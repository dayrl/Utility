using System.Text.RegularExpressions;

namespace Zdd.Utility
{
	/// <summary>
	/// 字符串验证工具。
	/// </summary>
	public static class StringValidator 
	{
		/// <summary>
		/// 判断指定的字符串是否仅仅包含数字。
		/// </summary>
		/// <param name="input">要检查的字符串。</param>
		/// <returns>只包含数字返回True，否则False。</returns>
		public static bool IsNumber(string input)
		{
			return Regex.IsMatch(input, @"^\d+$");
		}
		
		/// <summary>
		/// 判断给定的一个字符串是否为IP v4地址。
		/// </summary>
		/// <param name="address">要检查的字符串。</param>
		/// <returns>如果是IPV4地址则为True，否则为False。</returns>
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
        /// 检查给定的路径(包括字符串)是否合法
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

        /// <summary>按照文件夹命名规则检查文件夹命名是否合法
        /// 正则表达式：按照文件夹命名规则检查文件夹命名是否合法
        /// 当文件夹名称包含如下字符为不合法命名：\ / : * ? " < > | 
        /// </summary>
        /// <param name="name">文件夹名称</param>
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