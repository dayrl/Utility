using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;

//Description : 类辅助工具

namespace Zdd.Utility
{
    /// <summary>
    /// 类辅助工具
    /// <example>
    /// </example>
    /// </summary>
    public static class ClassHelper
    {
        /// <summary>
        /// 将对象的所有可读的Public属性值格式化为字符串表示形式
        /// </summary>
        /// <param name="obj">要格式化的对象</param>
        /// <param name="separator">分隔字符，没有为null</param>
        /// <returns></returns>
        public static string GetPropertiesValue(object obj, string separator)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            string[] values = new string[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].CanRead && properties[i].PropertyType.IsPublic)
                    values[i] = (properties[i].GetValue(obj, null) == null)
                                    ? string.Empty
                                    : properties[i].GetValue(obj, null).ToString();
            }

            return string.Join(separator, values);
        }

        /// <summary>
        /// 用一组字符为该对象可写的Public属性赋值（一般以GetPropertiesValues的结果为输入）
        /// </summary>
        /// <param name="obj">要赋值的对象</param>
        /// <param name="propertiesValue">属性值字符串</param>
        /// <param name="separator">字符串中的分隔符</param>
        public static void SetPropertiesValue(object obj, string propertiesValue, string separator)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (propertiesValue == null)
                throw new ArgumentNullException("separator");

            string[] values = propertiesValue.Split(new string[] { separator }, StringSplitOptions.None);
            PropertyInfo[] properties = obj.GetType().GetProperties();
            int count = Math.Min(values.Length, properties.Length);

            for (int i = 0; i < count; i++)
            {
                object o = Convert.ChangeType(values[i], properties[i].PropertyType);
                if (properties[i].CanWrite && properties[i].PropertyType.IsPublic)
                    properties[i].SetValue(obj, o, null);
            }
        }

        /// <summary>
        /// 将对象的所有的可读的Public属性描述格式化为字符串表示形式
        /// </summary>
        /// <param name="classType">要得到属性描述类的类型</param>
        /// <param name="separator">字符串分隔符</param>
        /// <returns></returns>
        public static string GetPropertiesDescription(Type classType, string separator)
        {
            PropertyInfo[] properties = classType.GetProperties();
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(classType);
            Hashtable propertieNames = new Hashtable();
            foreach (PropertyDescriptor p in pdc)
            {
                propertieNames.Add(p.Name, p);
            }

            string[] descriptions = new string[pdc.Count];
            for (int i = 0; i < properties.Length; i++)
            {
                if (propertieNames.Contains(properties[i].Name) && properties[i].CanRead &&
                    properties[i].PropertyType.IsPublic)
                {
                    AttributeCollection attributes = pdc[properties[i].Name].Attributes;
                    DescriptionAttribute attribute = (DescriptionAttribute)attributes[typeof(DescriptionAttribute)];
                    descriptions[i] = attribute.Description;
                }
            }

            return string.Join(separator, descriptions);
        }

        public static Object ConvertToEntity(DataRow pDataRow, Type pType)
        {
            Object entity = null;
            Object proValue = null;
            PropertyInfo propertyInfo = null;
            try
            {
                if (pDataRow != null)
                {
                    //动态创建类的实例
                    entity = Activator.CreateInstance(pType);
                    foreach (DataColumn dc in pDataRow.Table.Columns)
                    {
                        //忽略绑定时的大小写
                        propertyInfo = pType.GetProperty(dc.ColumnName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        proValue = pDataRow[dc];
                        //当值不为空时
                        if (proValue != DBNull.Value)
                        {
                            try
                            {   //给属性赋值
                                propertyInfo.SetValue(entity, Convert.ChangeType(proValue, dc.DataType), null);
                            }
                            catch //如果有错误,继续下一个属性的赋值
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            catch
            {
                entity = null;
            }
            return entity;
        } 
    }
}