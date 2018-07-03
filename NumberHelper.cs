using System;

namespace Zdd.Utility
{
	/// <summary>
	/// ��ֵ������ص������ࡣ
	/// </summary>
	public static class NumberHelper
	{
		/// <summary>
		/// ��ȡ������ѵ�λ��������ֵ������ 1024 ת����Ϊ 1K
		/// </summary>
		/// <param name="size">Ҫת������ֵ��</param>
		/// <returns>��ʾ�ü��ϵ�λ��Ϣ���ַ�����</returns>
		public static string GetOptimalSize(long size)
		{
			string desc;

			if (size >= 1099511627776L)
			{
				//ת����λΪT
				desc = String.Format("{0:F}TB", (float)size / 1099511627776F);
			}
			else if (size >= 1073741824L)
			{
				//ת����λΪG
				desc = String.Format("{0:F}GB", (float)size / 1073741824F);
			}
			else if (size >= 1048576L)
			{
				//ת����λΪM
				desc = String.Format("{0:F}MB", (float)size / 1048576F);
			}
			else if (size >= 1024L)
			{
				//ת����λΪK
				desc = String.Format("{0:F}KB", (float)size / 1024F);
			}
			else
			{
				//ת����λΪB
				desc = size.ToString() + "B";
			}
			return desc;
		}

       
	}
}
