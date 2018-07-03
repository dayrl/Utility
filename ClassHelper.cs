using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;

//Description : �ศ������

namespace Zdd.Utility
{
    /// <summary>
    /// �ศ������
    /// <example>
    /// </example>
    /// </summary>
    public static class ClassHelper
    {
        /// <summary>
        /// ����������пɶ���Public����ֵ��ʽ��Ϊ�ַ�����ʾ��ʽ
        /// </summary>
        /// <param name="obj">Ҫ��ʽ���Ķ���</param>
        /// <param name="separator">�ָ��ַ���û��Ϊnull</param>
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
        /// ��һ���ַ�Ϊ�ö����д��Public���Ը�ֵ��һ����GetPropertiesValues�Ľ��Ϊ���룩
        /// </summary>
        /// <param name="obj">Ҫ��ֵ�Ķ���</param>
        /// <param name="propertiesValue">����ֵ�ַ���</param>
        /// <param name="separator">�ַ����еķָ���</param>
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
        /// ����������еĿɶ���Public����������ʽ��Ϊ�ַ�����ʾ��ʽ
        /// </summary>
        /// <param name="classType">Ҫ�õ����������������</param>
        /// <param name="separator">�ַ����ָ���</param>
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
                    //��̬�������ʵ��
                    entity = Activator.CreateInstance(pType);
                    foreach (DataColumn dc in pDataRow.Table.Columns)
                    {
                        //���԰�ʱ�Ĵ�Сд
                        propertyInfo = pType.GetProperty(dc.ColumnName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        proValue = pDataRow[dc];
                        //��ֵ��Ϊ��ʱ
                        if (proValue != DBNull.Value)
                        {
                            try
                            {   //�����Ը�ֵ
                                propertyInfo.SetValue(entity, Convert.ChangeType(proValue, dc.DataType), null);
                            }
                            catch //����д���,������һ�����Եĸ�ֵ
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