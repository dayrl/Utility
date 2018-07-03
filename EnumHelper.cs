using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Zdd.Utility
{
    /// <summary>
    /// 枚举值辅助工具
    /// </summary>
    /// <example>
    /// private enum Test
    /// {
    /// 	[Description("测试1")]
    ///		Enum1,
    /// 	[Description("测试2")]
    /// 	Enum2,
    /// 	[Description("测试3")]
    /// 	Enum3
    /// }
    /// 
    /// EnumHelper.GetDescription(Test.Enum2);
    /// </example>
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举值的的描述信息。
        /// </summary>
        /// <param name="type">枚举类型。</param>
        /// <param name="fieldName">枚举项。</param>
        /// <returns>返回枚举项的Description信息，如果该项没有包含Description则返回枚举项的名称。</returns>
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
        /// 获取枚举值的的描述信息。
        /// </summary>
        /// <param name="obj">枚举对象。</param>
        /// <returns>返回枚举项的Description信息，如果该项没有包含Description则返回枚举项的ToString()内容。</returns>
        public static string GetDescription(object obj)
        {
            string desc = String.Empty;

            string fieldName = obj.ToString();
            return GetDescription(obj.GetType(), obj.ToString());
        }

        /// <summary>
        /// 获取给定枚举的所有值的描述信息。
        /// </summary>
        /// <param name="type">枚举对象。</param>
        /// <returns>返回枚举对象所有值的描述信息集合。</returns>
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