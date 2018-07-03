#region License and Copyright
/*
 * Dotnet Commons Reflection 
 *
 * Copyright ?2005. EDWARD LIM
 *
 * This library is free software; you can redistribute it and/or modify it 
 * under the terms of the GNU Lesser General Public License as published by 
 * the Free Software Foundation; either version 2.1 of the License, or 
 * (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License 
 * for more details. 
 *
 * You should have received a copy of the GNU Lesser General Public License 
 * along with this library; if not, write to the 
 * Free Software Foundation, Inc., 
 * 59 Temple Place, 
 * Suite 330, 
 * Boston, 
 * MA 02111-1307 
 * USA 
 * 
 */
#endregion

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;

namespace Zdd.Utility
{
    /// <summary>
    /// This class contains utility methods that perform operations on object properties. 
    /// </summary>
    /// <remarks> 
    /// ?Copyright 2006 by Edward Lim.
    /// All rights reserved.
    /// </remarks>
    public static class PropertyHelper
    {
        /// <summary>
        /// Get the names of all the properties of an object
        /// </summary>
        /// <param name="srcObj">the object to retreive the properties' names from</param>
        /// <returns>an array of property names</returns>
        public static string[] GetPropertyNames(object srcObj)
        {
            if (srcObj == null)
                throw new ArgumentNullException("srcObj");

            return GetPropertyNames(srcObj.GetType());
        }

        /// <summary>
        /// Get the names of all the properties of a type
        /// </summary>
        /// <param name="objType">the type to retreive the properties' names from</param>
        /// <returns>an array of property names</returns>
        public static string[] GetPropertyNames(Type objType)
        {

            if (objType == null)
                throw new ArgumentNullException("objType");

            string[] propertyNames = new string[objType.GetProperties().Length];

            for (int i = 0; i < objType.GetProperties().Length; i++)
            {
                propertyNames[i] = ((PropertyInfo)objType.GetProperties().GetValue(i)).Name;
            }

            return propertyNames;
        }
        
        /// <summary>
        /// Determine if a property exists in an object
        /// </summary>
        /// <param name="propertyName">Name of the property </param>
        /// <param name="srcObject">the object to inspect</param>
        /// <returns>true if the property exists, false otherwise</returns>
        /// <exception cref="ArgumentNullException">if srcObject is null</exception>
        /// <exception cref="ArgumentException">if propertName is empty or null </exception>
        public static bool Exists(string propertyName, object srcObject)
        {
            if (srcObject == null)
                throw new ArgumentNullException("srcObject");

            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Property name cannot be empty or null.");

            PropertyInfo propInfoSrcObj = srcObject.GetType().GetProperty(propertyName);

            return (propInfoSrcObj != null);
        }
        
        /// <summary>
        /// Determine if a property exists in an object
        /// </summary>
        /// <param name="propertyName">Name of the property </param>
        /// <param name="srcObject">the object to inspect</param>
        /// <param name="ignoreCase">ignore case sensitivity</param>
        /// <returns>true if the property exists, false otherwise</returns>
        /// <exception cref="ArgumentNullException">if srcObject is null</exception>
        /// <exception cref="ArgumentException">if propertName is empty or null </exception>
        public static bool Exists(string propertyName, object srcObject, bool ignoreCase)
        {
            if (!ignoreCase)
                return Exists(propertyName, srcObject);

            if (srcObject == null)
                throw new ArgumentNullException("srcObject");

            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Property name cannot be empty or null.");


            PropertyInfo[] propertyInfos = srcObject.GetType().GetProperties();

            propertyName = propertyName.ToLower();
            foreach (PropertyInfo propInfo in propertyInfos)
            {
                if (propInfo.Name.ToLower().Equals(propertyName))
                    return true;
            }
            return false;
        }
        
        /// <summary>
        /// Copy <i>Dynamic</i> properties from a Dictionary to an object. Each of the Key
        /// in the Dynamic property Dictionary will be assumed to be the property name to
        /// be copied over.
        /// </summary>
        /// <param name="targetObject">the object on which setting is to be performed</param>
        /// <param name="dynamicProperties">the dictionary that contains Properties and their values</param>
        public static Object Copy(object targetObject, IDictionary dynamicProperties)
        {
            if (targetObject == null)
                throw new ArgumentNullException("targetObject");

            if ((dynamicProperties == null) || (dynamicProperties.Count < 1))
                return targetObject;

            foreach (string propertyName in dynamicProperties.Keys)
            {
                if (Exists(propertyName, targetObject))
                {
                    // set the property
                    targetObject = ObjectHelper.SetProperty(targetObject, propertyName, dynamicProperties[propertyName]);
                }
            }

            return targetObject;
        }
        	
        /// <summary>
        /// Get the type of the Property.
        /// </summary>
        /// <param name="targetObj">the object to investigate</param>
        /// <param name="property">name of the property</param>
        /// <param name="searchPrivate">flag to indicate if the the search should include private properties</param>
        /// <returns>the Type of the property</returns>
        public static Type GetPropertyType(object targetObj, string property, bool searchPrivate)
        {
            if (targetObj == null)
                throw new ArgumentNullException("targetObj");

            if ((property == null) || (property.Length < 1))
                return null;

            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            if (searchPrivate)
                flags = flags | BindingFlags.NonPublic;

            PropertyInfo propInfo = targetObj.GetType().GetProperty(property);

            return propInfo.PropertyType;
        }
        
        /// <summary>
        /// Set a value to a property of an object using a Name-Value pair supplied.
        /// </summary>
        /// <param name="targetObject">object to set the property</param>
        /// <param name="nvp">name-value pair. Comma delimeter is used as the separator to separate the property Name from its Value.</param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// obj = PropertyUtils.SetPropertyFromNVP(employee, "LastName,Smith");
        /// </code>
        /// </example>
        /// <exception cref="System.ArgumentNullException">if targetObject is null</exception>
        public static object SetPropertyFromNVP(object targetObject, string nvp)
        {
            return SetPropertyFromNVP(targetObject, nvp, ",");
        }
        
        /// <summary>
        /// Set a value to a property of an object using a Name-Value pair supplied.
        /// </summary>
        /// <param name="delimeter">delimeter used to separate the property Name from its value</param>
        /// <param name="targetObject">object to set the property</param>
        /// <param name="nvp">name-value pair</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">if targetObject is null</exception>
        /// <example>
        /// <code>
        /// obj = PropertyUtils.SetPropertyFromNVP(employee, "LastName=Smith", "=");
        /// </code>
        /// </example>
        public static object SetPropertyFromNVP(object targetObject, string nvp, string delimeter)
        {
            if (targetObject == null)
                throw new ArgumentNullException("targetObject");

            if (nvp == null || nvp.Length < 3)
                return targetObject;

            if (delimeter == null || delimeter.Length < 1)
            {
                delimeter = ",";
            }

            string[] nvpParts = nvp.Split(delimeter.ToCharArray());

            if (nvpParts.Length != 2)
                return targetObject;

            targetObject = ObjectHelper.SetProperty(targetObject, nvpParts[0], nvpParts[1]);

            return targetObject;
        }


        /// <summary>
        /// Sets the property from NVP.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        /// <param name="nvpObj">The NVP obj.</param>
        /// <returns></returns>
        public static object SetPropertyFromNVP(object targetObject, object nvpObj)
        {
            return SetPropertyFromNVP(targetObject, nvpObj, false);
        }
        
        /// <summary>
        /// Set the Property of an object from a NVP object. A NVP object must have a <b>Name</b>
        /// property that contains the name of the property to set, and a <b>Value</b> property,
        /// which contains the value of the property to set.
        /// </summary>
        /// <param name="targetObject">target object to set the property value</param>
        /// <param name="nvpObj">NVP object that contains the property and its value</param>
        /// <param name="bUpperFirstLetter">Flag to determine if the first letter case has to be set to upper case</param>
        public static object SetPropertyFromNVP(object targetObject, object nvpObj, bool bUpperFirstLetter)
        {
            if (targetObject == null)
                throw new ArgumentNullException("targetObject");

            if (nvpObj == null)
                return targetObject;

            string propertyName = (ObjectHelper.GetProperty(nvpObj, "Name")) as String;

            if (propertyName == null)
                return targetObject;

            targetObject = ObjectHelper.SetProperty(targetObject,
                                    StringHelper.SetFirstLetterUpperCase(propertyName),
                                    ObjectHelper.GetProperty(nvpObj, "Value"));

            return targetObject;
        }

        /// <summary>
        /// Copy properties contain in a list of NVP objects into an object
        /// </summary>
        /// <param name="targetObject"></param>
        /// <param name="nvpObjList"></param>
        public static object SetPropertiesFromNVPList(object targetObject, IList nvpObjList)
        {
            return SetPropertiesFromNVPList(targetObject, nvpObjList, false);
        }

        /// <summary>
        /// Copy properties contain in a list of NVP objects into an object
        /// </summary>
        /// <param name="targetObject"></param>
        /// <param name="nvpObjList"></param>
        /// <param name="bUpperFirstLetter">Flag to determine if the first letter case 
        /// has to be set to upper case</param>
        /// <exception cref="ArgumentNullException">if the targetObject is null</exception>
        public static object SetPropertiesFromNVPList(object targetObject, IList nvpObjList, bool bUpperFirstLetter)
        {
            if (targetObject == null)
                throw new ArgumentNullException("targetObject");

            if ((nvpObjList == null) || (nvpObjList.Count < 1))
                return targetObject;


            foreach (object nvp in nvpObjList)
            {
                targetObject = SetPropertyFromNVP(targetObject, nvp);
            }

            return targetObject;
        }
        
        /// <summary>
        /// Set the properties of an object with the property Name-Value pairs
        /// contained in a <see cref="StringDictionary"/>.
        /// </summary>
        /// <param name="targetObject">object to set the properties</param>
        /// <param name="stringDict">String Dictionary that contains the NVPs for 
        /// setting the property values of the object</param>
        /// <returns></returns>
        public static object SetPropertiesFromCollection(object targetObject,
            StringDictionary stringDict)
        {

            if (targetObject == null)
                throw new ArgumentNullException("targetObject");

            if ((stringDict == null) || (stringDict.Count < 1))
                return targetObject;

            foreach (string key in stringDict.Keys)
            {
                targetObject = ObjectHelper.SetProperty(targetObject, key, stringDict[key]);
            }

            return targetObject;
        }
        
        /// <summary>
        /// Set the properties of an object with the property Name-Value pairs
        /// contained in a <see cref="IDictionary"/>.
        /// </summary>
        /// <param name="targetObject">object to set the properties</param>
        /// <param name="nvpDict">Dictionary that contains the NVPs for 
        /// setting the property values of the object</param>
        /// <returns></returns>
        public static object SetPropertiesFromCollection(object targetObject,
            IDictionary nvpDict)
        {
            if (targetObject == null)
                throw new ArgumentNullException("targetObject");

            if ((nvpDict == null) || (nvpDict.Count < 1))
                return targetObject;

            foreach (string key in nvpDict.Keys)
            {
                targetObject = ObjectHelper.SetProperty(targetObject, key, nvpDict[key]);
            }

            return targetObject;
        }
        
        /// <summary>
        /// Copy a property if exists.
        /// </summary>
        /// <param name="propertyName">Property to be copied</param>
        /// <param name="srcObj">source object</param>
        /// <param name="targetObj">target object</param>
        /// <exception cref="ArgumentException">if the propertyName is null or empty</exception>
        /// <exception cref="ArgumentNullException">if the srcObj is null</exception>
        public static object CopyPropertyIfExists(string propertyName, object srcObj, object targetObj)
        {
            if ((propertyName == null) || (propertyName.Length < 1))
            {
                throw new ArgumentException("propertyName cannot be null or empty.");
            }

            if (srcObj == null)
                throw new ArgumentNullException("srcObj");

            if (targetObj == null)
                return null;


            if (srcObj.GetType().GetProperty(propertyName) == null)
                return null;

            if (targetObj.GetType().GetProperty(propertyName) == null)
                return null;

            targetObj = ObjectHelper.SetProperty(targetObj, propertyName, ObjectHelper.GetProperty(srcObj, propertyName));

            return targetObj;
        }

        /// <summary>
        /// Set the value of a property that has been declared as an Enum type using reflection
        /// </summary>
        /// <param name="targetObj">target object in which property is to be set</param>
        /// <param name="propertyName">name of the property</param>
        /// <param name="propertyValue">value of the object to be set</param>
        /// <param name="ignoreCase">ignore case sensitivity</param>
        /// <returns>null if target object or is null, else target object with property set if property if found</returns>
        /// <exception cref="ArgumentException">if the propertyName is null or empty</exception>		
        public static object SetEnumTypeProperty(object targetObj, string propertyName, object propertyValue, bool ignoreCase)
        {
            if (targetObj == null)
                return null;

            if ((propertyName == null) || (propertyName.Length < 1))
            {
                throw new ArgumentException("propertyName cannot be null or empty.");
            }

            if (propertyValue == null)
                return targetObj;

            PropertyInfo propInfo = targetObj.GetType().GetProperty(propertyName);

            if (propInfo == null)
                return targetObj;


            if (!propInfo.PropertyType.IsEnum)
                throw new ArgumentException("property " + propertyName + " is not an Enum type");

            if (propertyValue is String)
                propertyValue = Enum.Parse(propInfo.PropertyType, (string)propertyValue, ignoreCase);

            propInfo.SetValue(targetObj, Enum.ToObject(propInfo.PropertyType, Convert.ToUInt64(propertyValue)), null);

            return targetObj;
        }
        
        /// <summary>
        /// Determine if a property's Type is an enum
        /// </summary>
        /// <param name="srcObj">object to inspect</param>
        /// <param name="propertyName">name of the property</param>
        /// <param name="ignoreCase">ignore case sensitivity</param>
        /// <returns>true if property is of Enum type, false otherwise</returns>
        /// <exception cref="ArgumentNullException">if the srcObj is null</exception>
        /// <exception cref="ArgumentException">if the propertyName is null or empty</exception>		
        /// <exception cref="ArgumentException">if the property does not exists</exception>		
        public static bool IsPropertyEnumType(object srcObj, string propertyName, bool ignoreCase)
        {
            if (srcObj == null)
                throw new ArgumentNullException("srcObj");

            if ((propertyName == null) || (propertyName.Length < 1))
            {
                throw new ArgumentException("propertyName cannot be null or empty.");
            }

            if (!ignoreCase)
                return IsPropertyEnumType(srcObj, propertyName);

            PropertyInfo[] propInfos = srcObj.GetType().GetProperties();

            foreach (PropertyInfo propInfo in propInfos)
            {
                if (propInfo.Name.ToLower().Equals(propertyName.ToLower()))
                    return IsPropertyEnumType(propInfo);
            }

            throw new ArgumentException("The source object (of the Type '" + srcObj.GetType().FullName + "') does not have a '" + propertyName + "' property");
        }
        
        /// <summary>
        /// Determine if a property's Type is an enum
        /// </summary>
        /// <param name="srcObj">object to inspect</param>
        /// <param name="propertyName">name of the property</param>
        /// <returns>true if property is of Enum type, false otherwise</returns>
        /// <exception cref="ArgumentNullException">if the srcObj is null</exception>
        /// <exception cref="ArgumentException">if the propertyName is null or empty</exception>
        /// <exception cref="ArgumentException">if the property does not exists</exception>				
        public static bool IsPropertyEnumType(object srcObj, string propertyName)
        {
            if (srcObj == null)
                throw new ArgumentNullException("srcObj");

            if ((propertyName == null) || (propertyName.Length < 1))
            {
                throw new ArgumentException("propertyName cannot be null or empty.");
            }

            PropertyInfo propInfo = srcObj.GetType().GetProperty(propertyName);

            if (propInfo == null)
                throw new ArgumentException("The source object (of the Type '" + srcObj.GetType().FullName + "') does not have a '" + propertyName + "' property");

            return IsPropertyEnumType(propInfo);
        }
        
        /// <summary>
        /// Determine if a property's Type is an enum
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns>true if property is of Enum type, false otherwise</returns>
        public static bool IsPropertyEnumType(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            return propertyInfo.PropertyType.IsEnum;
        }

        /// <summary>
        ///  Return <b>true</b> if the specified property name identifies a 
        /// readable property on the specified object; 
        /// otherwise, return <b>false</b>.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="ignoreCase">ignore case sensitivity</param>
        /// <returns></returns>
        public static bool IsReadable(Object obj, string propertyName, bool ignoreCase)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            BindingFlags bindingAttrs = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            bindingAttrs = (ignoreCase) ? bindingAttrs | BindingFlags.IgnoreCase : bindingAttrs;

            PropertyInfo pi = obj.GetType().GetProperty(propertyName, bindingAttrs);

            if (pi == null)
                throw new ArgumentException(string.Format("The '{0}' object does not have a '{1}' property", obj.GetType().FullName, propertyName));

            return pi.CanRead;
        }
        
        /// <summary>
        ///  Return <b>true</b> if the specified property name identifies a 
        /// writeable property on the specified object; 
        /// otherwise, return <b>false</b>.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="ignoreCase">ignore case sensitivity</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If obj parameter is null</exception>
        /// <exception cref="ArgumentException">if the propertyName supplied 
        /// cannot be found in the object</exception>
        public static bool IsWritable(Object obj, string propertyName, bool ignoreCase)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            BindingFlags bindingAttrs = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            bindingAttrs = (ignoreCase) ? bindingAttrs | BindingFlags.IgnoreCase : bindingAttrs;

            PropertyInfo pi = obj.GetType().GetProperty(propertyName, bindingAttrs);

            if (pi == null)
                throw new ArgumentException(string.Format("The '{0}' object does not have a '{1}' property", obj.GetType().FullName, propertyName));

            return pi.CanWrite;
        }
        		
        /// <summary>
        /// Return all the properties of an object and their values as Name 
        /// Value pairs separated by a comma delimeter.
        /// </summary>
        /// <param name="obj">object to retrieve the properties from</param>
        /// <param name="delimeter">delimeter to be used for separating name 
        /// and value. Default to comma if String.Empty is passed in.</param>
        /// <returns>An array containinh the object's properties as Name-Value
        /// pairs</returns>
        /// <remarks>This method is extremely useful when the properties and
        /// their values are required as name value pairs. 
        /// </remarks>
        public static string[] ToStringNVPs(object obj, string delimeter)
        {
            string[] propNames = GetPropertyNames(obj);

            string[] nvps = new string[propNames.Length];

            if (string.IsNullOrEmpty(delimeter))
                delimeter = ",";

            for (int i = 0; i < nvps.Length; i++)
            {
                object value = ObjectHelper.GetProperty(obj,
                    propNames[i]);

                nvps[i] = propNames[i] + delimeter + value.ToString();
            }

            return nvps;
        }
    }
}
