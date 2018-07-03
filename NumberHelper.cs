using System;

namespace Zdd.Utility
{
	/// <summary>
	/// 数值处理相关的助手类。
	/// </summary>
	public static class NumberHelper
	{
		/// <summary>
		/// 获取采用最佳单位描述的数值。例如 1024 转换后为 1K
		/// </summary>
		/// <param name="size">要转换的数值。</param>
		/// <returns>表示该加上单位信息的字符串。</returns>
		public static string GetOptimalSize(long size)
		{
			string desc;

			if (size >= 1099511627776L)
			{
				//转换单位为T
				desc = String.Format("{0:F}TB", (float)size / 1099511627776F);
			}
			else if (size >= 1073741824L)
			{
				//转换单位为G
				desc = String.Format("{0:F}GB", (float)size / 1073741824F);
			}
			else if (size >= 1048576L)
			{
				//转换单位为M
				desc = String.Format("{0:F}MB", (float)size / 1048576F);
			}
			else if (size >= 1024L)
			{
				//转换单位为K
				desc = String.Format("{0:F}KB", (float)size / 1024F);
			}
			else
			{
				//转换单位为B
				desc = size.ToString() + "B";
			}
			return desc;
		}

       
	}
}
