using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Zdd.Utility
{
    /// <summary>
    /// ö��ֵ��������
    /// </summary>
    /// <example>
    /// private enum Test
    /// {
    /// 	[Description("����1")]
    ///		Enum1,
    /// 	[Description("����2")]
    /// 	Enum2,
    /// 	[Description("����3")]
    /// 	Enum3
    /// }
    /// 
    /// EnumHelper.GetDescription(Test.Enum2);
    /// </example>
    public static class EnumHelper
    {
        /// <summary>
        /// ��ȡö��ֵ�ĵ�������Ϣ��
        /// </summary>
        /// <param name="type">ö�����͡�</param>
        /// <param name="fieldName">ö���</param>
        /// <returns>����ö�����Description��Ϣ���������û�а���Description�򷵻�ö��������ơ�</returns>
        public static string GetDescription(Type type, string fieldName)
        {
            string desc = String.Empty;

            FieldInfo[] fields = type.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.IsSpecialName)
                    continue;

                if (field.Name != fieldName)
                    continue;

                object[] attrs = field.GetCustomAttributes(typeof (DescriptionAttribute), false);
                if (attrs.Length > 0)
                    desc = ((DescriptionAttribute) attrs[0]).Description;
            }
            if (string.IsNullOrEmpty(desc))
                desc = fieldName;
            return desc;
        }


        /// <summary>
        /// ��ȡö��ֵ�ĵ�������Ϣ��
        /// </summary>
        /// <param name="obj">ö�ٶ���</param>
        /// <returns>����ö�����Description��Ϣ���������û�а���Description�򷵻�ö�����ToString()���ݡ�</returns>
        public static string GetDescription(object obj)
        {
            string desc = String.Empty;

            string fieldName = obj.ToString();
            return GetDescription(obj.GetType(), obj.ToString());
        }

        /// <summary>
        /// ��ȡ����ö�ٵ�����ֵ��������Ϣ��
        /// </summary>
        /// <param name="type">ö�ٶ���</param>
        /// <returns>����ö�ٶ�������ֵ��������Ϣ���ϡ�</returns>
        public static string[] GetDescriptions(Type type)
        {
            FieldInfo[] fields = type.GetFields();
            ArrayList temp = new ArrayList();
            string desc = String.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.IsSpecialName)
                    continue;

                object[] attrs = field.GetCustomAttributes(typeof (DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    desc = ((DescriptionAttribute) attrs[0]).Description;
                    if (string.IsNullOrEmpty((desc)))
                    {
                        desc = attrs[0].ToString();
                    }
                    temp.Add(desc);
                }
            }
            string[] descs = new string[temp.Count];
            temp.CopyTo(descs);

            return descs;
        }
    }
}