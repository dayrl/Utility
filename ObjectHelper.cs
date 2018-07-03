#region License and Copyright
/*
 * Dotnet Commons Reflection
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
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Zdd.Utility
{
    /// <summary>	
    /// This utility class contains a rich sets of utility methods that perform operations 
    /// on objects during runtime such as copying of property and field values
    /// between 2 objects, deep cloning of objects, etc.	
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// Clone a bean based on the available property accessors (getters and setters),
        /// even if the <i>Value Object</i> class itself does not implement ICloneable.
        /// </summary>
        /// <param name="srcObject">Source object</param>
        /// <returns>Cloned object</returns>
        public static object Clone(object srcObject)
        {
            if (srcObject == null) return null;

            Type objType = srcObject.GetType();
            object clonedObj = Activator.CreateInstance(objType);

            return CopyProperties(srcObject, clonedObj);

        }

        /// <summary>
        /// Clone a <see cref="IDictionary" /> collection.
        /// </summary>
        /// <param name="srcDict">source dictionary</param>
        /// <param name="IsToDeepClone">true if deep cloning, false for shallow cloning</param>
        /// <returns>cloned dictionary collection</returns>
        public static IDictionary CloneDictionary(IDictionary srcDict, bool IsToDeepClone)
        {

            if (srcDict == null)
                return null;

            IDictionary clonedDict = Activator.CreateInstance(srcDict.GetType()) as IDictionary;

            if (srcDict.Count == 0)
                return clonedDict;

            foreach (object key in srcDict.Keys)
            {
                object clonedElement = null;

                if (IsToDeepClone)
                    clonedElement = DeepClone(srcDict[key]);
                else
                    clonedElement = Clone(srcDict[key]);

                clonedDict.Add(key, clonedElement);

            }
            return clonedDict;
        }

        /// <summary>
        /// Clone a <see cref="IList" /> collection.
        /// </summary>
        /// <param name="srcList"></param>
        /// <param name="IsToDeepClone"></param>
        /// <returns></returns>
        public static IList CloneList(IList srcList, bool IsToDeepClone)
        {
            if (srcList == null)
                return null;

            IList clonedList = (IList)Activator.CreateInstance(srcList.GetType());

            if (srcList.Count == 0)
                return clonedList;

            foreach (object element in srcList)
            {
                object clonedElement = null;

                if (IsToDeepClone)
                    clonedElement = DeepClone(element);
                else
                    clonedElement = Clone(element);


                clonedList.Add(clonedElement);
            }

            return clonedList;
        }

        /// <summary>
        /// Deep clone an <see cref="System.Object"/>.
        /// </summary>
        /// <param name="srcObject">Source <see cref="System.Object"/>  to clone from</param>
        /// <returns>Clone <see cref="System.Object"/> </returns>
        public static object DeepClone(object srcObject)
        {
            if (srcObject == null) return null;



            // If object implements ISerializable, then call the DeepCloneSerializable			

            Type ISerializableType = srcObject.GetType().GetInterface("ISerializable", true);

            if (ISerializableType != null)
                return DeepCloneSerializable(srcObject);



            // If object implements IDictionary, then call the 
            // CloneDictionary method.

            Type IDictionaryType = srcObject.GetType().GetInterface("IDictionary", true);

            if (IDictionaryType != null)
                return CloneDictionary((IDictionary)srcObject, true);



            // If object implements IList, then call the 
            // CloneDictionary method.

            Type IListType = srcObject.GetType().GetInterface("IList", true);

            if (IListType != null)
                return CloneList((IList)srcObject, true);


            //Firstly, create an instance of this specific type.
            object newObject = Activator.CreateInstance(srcObject.GetType());


            // get the array of fields for the new type instance.
            FieldInfo[] fields = newObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            int i = 0;

            foreach (FieldInfo fi in srcObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                // query if the fiels support the ICloneable interface.
                Type cloneType = fi.FieldType.GetInterface("ICloneable", true);

                //Getting the ICloneable interface from the object.
                ICloneable cloneObj = null;

                if (cloneType != null)
                    cloneObj = (ICloneable)fi.GetValue(srcObject);

                if (cloneType != null && cloneObj != null)
                {
                    //use the clone method to set the new value to the field.
                    fields[i].SetValue(newObject, cloneObj.Clone());
                }
                else
                {
                    Type fieldType = fi.DeclaringType;
                    Type typeIDictionary = fi.FieldType.GetInterface("IDictionary", true);
                    Type typeIList = fi.FieldType.GetInterface("IList", true);


                    // Field is one of the Primitive type - just set it.
                    if ((fieldType.IsPrimitive) || (fieldType.IsArray))
                    {
                        fields[i].SetValue(newObject, fi.GetValue(srcObject));
                    }
                    // Field is a dictionary
                    else if (typeIDictionary != null)
                    {
                        fields[i].SetValue(newObject, CloneDictionary((IDictionary)fi.GetValue(srcObject), true));
                    }
                    // field is a list
                    else if (typeIList != null)
                    {
                        fields[i].SetValue(newObject, CloneList((IList)fi.GetValue(srcObject), true));
                    }

                        // field is an object - deep clone it
                    else
                        fields[i].SetValue(newObject, DeepClone(fi.GetValue(srcObject)));
                }

                i++;
            }//for

            return newObject;

        }

        /// <summary>
        /// Deep cloning an <see cref="System.Object"/>. If the <see cref="System.Object"/>
        /// is serializable, ie, an <see cref="System.Object"/> 
        /// which implements <see cref="System.Runtime.Serialization.ISerializable" /> 
        /// interface or its class haivng marked
        /// with the [Serializable] <see cref="System.Attribute"/>.
        /// </summary>
        /// <param name="srcObject"></param>
        /// <returns></returns>
        public static object DeepCloneSerializable(object srcObject)
        {
            if (srcObject == null) return null;

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, srcObject);
                //stream.Position = 0;
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 1 level Deep copying of an <see cref="System.Object"/> properties and 
        /// fields to another.
        /// Both private and public fields will be copied.
        /// </summary>
        /// <param name="srcObject">Source <see cref="System.Object"/> to copy from</param>
        /// <param name="destObject">Destination <see cref="System.Object"/> to copy to</param>
        /// <returns>Copied destination object</returns>
        public static object Copy(object srcObject, object destObject)
        {
            // null objects cannot be reflected
            if (srcObject == null)
                return destObject;

            // --------------------------------
            // copy public properties
            // --------------------------------
            foreach (PropertyInfo propInfoFromObj in srcObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                PropertyInfo propInfoDestObj = destObject.GetType().GetProperty(propInfoFromObj.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (propInfoDestObj != null)
                {
                    object srcValue = srcObject.GetType().InvokeMember(propInfoFromObj.Name,
                                                        BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                                                        null,
                                                        srcObject,
                                                        null);


                    // Convert if necessary
                    Object destValue = ConverterHelper.Convert(srcValue, propInfoDestObj.PropertyType);


                    destObject.GetType().InvokeMember(propInfoDestObj.Name,
                                                        BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                                                        null,
                                                        destObject,
                                                        new object[] { destValue });
                }
            }

            // --------------------------------
            // copy private & public fields
            // --------------------------------
            foreach (FieldInfo fieldInfoFromObj in srcObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                FieldInfo fieldInfoDestObj = destObject.GetType().GetField(fieldInfoFromObj.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (fieldInfoDestObj != null)
                {
                    Object srcValue = srcObject.GetType().InvokeMember(fieldInfoFromObj.Name,
                                                                            BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                                                                            null,
                                                                            srcObject,
                                                                            null);

                    // Convert if necessary
                    Object destValue = ConverterHelper.Convert(srcValue, fieldInfoDestObj.FieldType);


                    destObject.GetType().InvokeMember(fieldInfoDestObj.Name,
                                                        BindingFlags.SetField | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                                                        null,
                                                        destObject,
                                                        new object[] { destValue });
                }

            }

            return destObject;
        }

        /// <summary>
        /// Copy property values from the origin value object (VO) to the destination VO.
        /// </summary>
        /// <param name="propertyName">Name of the property to copy from</param>
        /// <param name="srcObj">source object to copy from</param>
        /// <param name="destObj">destination object to copy to</param>
        /// <returns>returned value object copied</returns>
        /// <exception cref="System.ArgumentNullException">thrown if either the source 
        /// or the destination objects are not supplied.</exception>
        /// <exception cref="System.ArgumentException">thrown if the property
        /// to be copied does not exists in the source
        /// </exception>
        public static object CopyProperty(string propertyName, object srcObj, object destObj)
        {
            if (srcObj == null)
                throw new System.ArgumentNullException("srcObj");

            if (destObj == null)
                throw new System.ArgumentNullException("destObj");

            if ((propertyName == null) || (propertyName.Length < 1))
                throw new System.ArgumentNullException("propertyName is null or empty");

            PropertyInfo propInfoSrcObj = srcObj.GetType().GetProperty(propertyName);

            if (propInfoSrcObj == null)
                throw new System.ArgumentException("The class '" + srcObj.GetType().Name + "' does not have the property '" + propertyName + "'");

            PropertyInfo propInfoDestObj = destObj.GetType().GetProperty(propertyName);

            if (propInfoDestObj == null || !propInfoDestObj.CanWrite)
                throw new System.ArgumentException("The class '" + destObj.GetType().Name + "' does not have the property '" + propertyName + "' or the property cannot be set.");

            // Get the value from property.
            object srcValue = srcObj.GetType()
                                .InvokeMember(propInfoSrcObj.Name,
                                                BindingFlags.GetProperty,
                                                null,
                                                srcObj,
                                                null);

            // Convert if necessary
            Object destValue = ConverterHelper.Convert(srcValue, propInfoDestObj.PropertyType);


            // Set the value 
            destObj.GetType().InvokeMember(propInfoDestObj.Name,
                                            BindingFlags.SetProperty,
                                            null,
                                            destObj,
                                            new object[] { destValue });

            return destObj;

        }

        /// <summary>
        /// Similar to the <see cref="CopyProperty"/> method, but will return the original 
        /// destination object if the copy fails because of the property to copy to the 
        /// destination object does not exist or is readonly.
        /// </summary>
        /// <param name="propertyName">Name of the property to copy from</param>
        /// <param name="srcObj">source object to copy from</param>
        /// <param name="destObj">destination object to copy to</param>
        /// <returns>returned value object copied</returns>        
        public static object CopyPropertyIfExists(string propertyName, object srcObj, object destObj)
        {
            try
            {
                return CopyProperty(propertyName, srcObj, destObj);
            }
            catch (ArgumentException)
            {
                // do nothing just return
            }
            catch (System.Reflection.TargetException)
            {
                // do nothing just return
            }
            catch (System.MissingMethodException)
            {
                // do nothing just return
            }
            return destObj;
        }

        /// <summary>
        /// Copy property values from the origin value object (VO) to the destination VO
        /// for all cases where the property names are the same.  For each
        /// property, a conversion is attempted as necessary.  
        /// 
        /// All combinations of standard value objects (.Net Property) and 
        /// javabean like value objects (property getters and setters) are supported.  
        /// 
        /// Properties that exist in the origin VO, but do not exist
        /// in the destination VO (or are read-only in the destination bean) are
        /// silently ignored.
        /// </summary>
        /// <remarks>
        ///		Note that the JavaBean VO standards mandates that the getters MUST have
        ///		the method names beginning with a lowercase 'get' and the setters names MUST
        ///		begin with a lowercase 'set'. Eg. getAddress() and setAddress() are
        ///		valid getter and setter names for the Address property. 
        /// </remarks>
        /// <param name="srcObject">Value object to be copied from</param>
        /// <param name="destObject">Value object to be copied to</param>
        /// <returns>returned value object copied from original to destination</returns>
        public static object CopyProperties(object srcObject, object destObject)
        {
            // null objects cannot be reflected
            if (srcObject == null)
                return destObject;

            // --------------------------------
            // copy public properties
            // --------------------------------
            foreach (PropertyInfo propInfoFromObj in srcObject.GetType().GetProperties())
            {
                PropertyInfo propInfoDestObj = destObject.GetType().GetProperty(propInfoFromObj.Name);

                // Ensure that the property exists in the destination object
                // and that it can be written, ie. it has the setter.
                if (propInfoDestObj == null || !propInfoDestObj.CanWrite)
                    continue;

                object srcValue = srcObject.GetType().InvokeMember(propInfoFromObj.Name,
                                                                BindingFlags.GetProperty,
                                                                null,
                                                                srcObject,
                                                                null);


                // Convert if necessary
                Object destValue = ConverterHelper.Convert(srcValue, propInfoDestObj.PropertyType);


                destObject.GetType().InvokeMember(propInfoDestObj.Name,
                                                BindingFlags.SetProperty,
                                                null,
                                                destObject,
                                                new object[] { destValue });

            }

            // --------------------------------
            // copy public fields
            // --------------------------------
            foreach (FieldInfo fieldInfoFromObj in srcObject.GetType().GetFields())
            {
                FieldInfo fieldInfoDestObj = destObject.GetType().GetField(fieldInfoFromObj.Name);

                // Ensure field exists in the destination object
                if (fieldInfoDestObj == null) continue;

                Object srcValue = srcObject.GetType().InvokeMember(fieldInfoFromObj.Name,
                                                                BindingFlags.GetField,
                                                                null,
                                                                srcObject,
                                                                null);

                // Convert if necessary
                Object destValue = ConverterHelper.Convert(srcValue, fieldInfoDestObj.FieldType);


                destObject.GetType().InvokeMember(fieldInfoDestObj.Name,
                                                BindingFlags.SetField,
                                                null,
                                                destObject,
                                                new object[] { destValue });

            }

            // ------------------------------------------
            // copy getter and setter type properties
            // ------------------------------------------
            foreach (MethodInfo fromObjMethodInfo in srcObject.GetType().GetMethods())
            {
                if (fromObjMethodInfo.Name.StartsWith("get") && !fromObjMethodInfo.Name.StartsWith("get_"))
                {
                    string setterMethodName = "set" + fromObjMethodInfo.Name.Substring(3, fromObjMethodInfo.Name.Length - 3);
                    MethodInfo methodInfoDestobj = destObject.GetType().GetMethod(setterMethodName);

                    if (methodInfoDestobj != null)
                    {
                        try
                        {
                            object srcValue = srcObject
                                                    .GetType()
                                                    .InvokeMember(fromObjMethodInfo.Name,
                                                                    BindingFlags.InvokeMethod,
                                                                    null,
                                                                    srcObject,
                                                                    null);


                            destObject
                                .GetType()
                                .InvokeMember(methodInfoDestobj.Name,
                                                BindingFlags.InvokeMethod,
                                                null,
                                                destObject,
                                                new object[] { srcValue });
                        }
                        catch
                        {
                            //ignore and continue							
                        }
                    }

                }
            }
            // return the updated object
            return destObject;
        }

        /// <summary>
        /// Copy an array into another array.
        /// </summary>
        /// <param name="src">source array</param>
        /// <param name="dest">destination array</param>
        /// <returns>copied array</returns>
        public static object[] CopyArray(object[] src, object[] dest)
        {
            for (int i = 0; i < src.Length; i++)
            {

                Type ICloneType = src[i].GetType().GetInterface("ICloneable", true);
                MemberInfo[] mi = src[i].GetType().GetMember("Clone");

                if (src[i].GetType().IsPrimitive)
                    dest[i] = src[i];

                // reference type
                else if (ICloneType != null)
                {
                    //Getting the ICloneable interface from the object.
                    ICloneable IClone = (ICloneable)src[i];

                    dest[i] = IClone.Clone();

                }
                else if ((mi != null) && (mi.Length > 0))
                {
                    dest[i] = src[i].GetType().InvokeMember("Clone", BindingFlags.InvokeMethod, null, src[i], null);
                }
                else  // copy reference
                    dest[i] = src[i];

            }
            return dest;
        }

        /// <summary>
        /// Helper to display the "contents" of the Value Object
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public static string ConvertToString(object valueObject)
        {
            StringBuilder buffy = new StringBuilder(valueObject.GetType().FullName);


            // null objects cannot be reflected
            if (valueObject == null)
            {
                buffy.Append(" is null.");
                return buffy.ToString();
            }

            buffy.Append("[\n");
            foreach (PropertyInfo objProperty in valueObject.GetType().GetProperties())
            {
                string nvp;

                nvp = "  <<Property>> " + "<" + objProperty.PropertyType + "> " + objProperty.Name + "=";

                if (objProperty != null)
                {
                    object value = valueObject
                                     .GetType()
                                     .InvokeMember(objProperty.Name,
                                                    BindingFlags.GetProperty,
                                                    null,
                                                    valueObject,
                                                    null);

                    buffy.Append(nvp + value.ToString() + "\n");
                }
                else
                {
                    buffy.Append(nvp + "<null>\n");
                }
            }

            foreach (FieldInfo objField in valueObject.GetType().GetFields())
            {
                string nvp;

                nvp = "  <<Field>> " + "<" + objField.FieldType + "> " + objField.Name + "=";

                if (objField != null)
                {
                    object value = valueObject.GetType().InvokeMember(objField.Name,
                                                                        BindingFlags.GetField,
                                                                        null,
                                                                        valueObject,
                                                                        null);

                    buffy.Append(nvp + value.ToString() + "\n");
                }
                else
                {
                    buffy.Append(nvp + "<null>\n");
                }
            }
            // ----------- End ---------
            buffy.Append("]");
            return buffy.ToString();
        }

        /// <summary>
        /// Get a <b>public</b> field value given its name
        /// </summary>
        /// <param name="srcObj">object to inspect</param>
        /// <param name="fieldName">Name of the field to retrieve the value from</param>
        /// <returns>property value</returns>
        public static Object GetFieldValue(object srcObj, string fieldName)
        {

            FieldInfo fieldInfoObj = srcObj.GetType().GetField(fieldName);

            if (fieldInfoObj == null)
                return null;

            // Get the value from property.
            object srcValue = srcObj.GetType()
                                    .InvokeMember(fieldInfoObj.Name,
                                    BindingFlags.GetField,
                                    null,
                                    srcObj,
                                    null);

            return srcValue;
        }

        /// <summary>
        /// Get a property value given its name
        /// </summary>
        /// <param name="srcObj">object to inspect</param>
        /// <param name="propertyName">Name of the property to retrieve the value from</param>
        /// <returns>property value</returns>
        public static Object GetProperty(object srcObj, string propertyName)
        {

            PropertyInfo propInfoObj = srcObj.GetType().GetProperty(propertyName);

            if (propInfoObj == null)
                return null;

            // Get the value from property.
            object srcValue = srcObj
                                .GetType()
                                .InvokeMember(propInfoObj.Name,
                                                BindingFlags.GetProperty,
                                                null,
                                                srcObj,
                                                null);

            return srcValue;
        }

        /// <summary>
        /// Get a property value given its name. 
        /// </summary>
        /// <param name="srcObj">object to inspect</param>
        /// <param name="propertyName">Name of the property to retrieve the value from</param>
        /// <param name="ignoreCase">ignore case sensitivity with the supplied property name.</param>
        /// <returns>value of the Property request if found, <b>null</b> otherwise.</returns>
        /// <remarks>This method will get property in any scope, including public and non public,
        /// instance and static.</remarks>
        public static Object GetProperty(object srcObj, string propertyName, bool ignoreCase)
        {
            PropertyInfo propInfoObj;

            BindingFlags bindingAttrs = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            bindingAttrs = (ignoreCase) ? bindingAttrs | BindingFlags.IgnoreCase : bindingAttrs;

            propInfoObj = srcObj.GetType().GetProperty(propertyName, bindingAttrs);

            if (propInfoObj == null)
                return null;

            // Get the value from property.
            object srcValue = srcObj
                                .GetType()
                                .InvokeMember(propInfoObj.Name,
                                                BindingFlags.GetProperty,
                                                null,
                                                srcObj,
                                                null);

            return srcValue;
        }

        /// <summary>
        /// Deep Comparison two objects if they are alike. The objects are consider alike if 
        /// they are:
        /// <list type="ordered">
        ///		<item>of the same <see cref="System.Type"/>,</item>
        ///		<item>have the same number of methods, properties and fields</item>
        ///		<item>the public and private properties and fields values reflect each other's. </item>
        /// </list>
        /// </summary>
        /// <param name="original"></param>
        /// <param name="comparedToObject"></param>
        /// <returns></returns>
        public static bool IsALike(object original, object comparedToObject)
        {

            if (original.GetType() != comparedToObject.GetType())
                return false;


            // Compare Number of Private and public Methods

            MethodInfo[] originalMethods = original
                .GetType()
                .GetMethods(BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public);

            MethodInfo[] comparedMethods = comparedToObject
                .GetType()
                .GetMethods(BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public);

            if (comparedMethods.Length != originalMethods.Length)
                return false;


            // Compare Number of Private and public Properties

            PropertyInfo[] originalProperties = original
                .GetType()
                .GetProperties(BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public);

            PropertyInfo[] comparedProperties = comparedToObject
                .GetType()
                .GetProperties(BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public);


            if (comparedProperties.Length != originalProperties.Length)
                return false;



            // Compare number of public and private fields

            FieldInfo[] originalFields = original
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            FieldInfo[] comparedToFields = comparedToObject
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);


            if (comparedToFields.Length != originalFields.Length)
                return false;


            // compare field values

            foreach (FieldInfo fi in originalFields)
            {

                // check to see if the object to contains the field					
                FieldInfo fiComparedObj = comparedToObject.GetType().GetField(fi.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (fiComparedObj == null)
                    return false;

                // Get the value of the field from the original object				
                Object srcValue = original.GetType().InvokeMember(fi.Name,
                    BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null,
                    original,
                    null);



                // Get the value of the field
                object comparedObjFieldValue = comparedToObject
                    .GetType()
                    .InvokeMember(fiComparedObj.Name,
                    BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null,
                    comparedToObject,
                    null);


                // -------------------------------
                // now compare the field values
                // -------------------------------

                if (srcValue == null)
                {
                    if (comparedObjFieldValue != null)
                        return false;
                    else
                        return true;
                }

                if (srcValue.GetType() != comparedObjFieldValue.GetType())
                    return false;

                if (!srcValue.ToString().Equals(comparedObjFieldValue.ToString()))
                    return false;
            }


            // compare each Property values

            foreach (PropertyInfo pi in originalProperties)
            {

                // check to see if the object to contains the field					
                PropertyInfo piComparedObj = comparedToObject
                    .GetType()
                    .GetProperty(pi.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (piComparedObj == null)
                    return false;

                // Get the value of the property from the original object				
                Object srcValue = original
                    .GetType()
                    .InvokeMember(pi.Name,
                    BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null,
                    original,
                    null);

                // Get the value of the property
                object comparedObjValue = comparedToObject
                    .GetType()
                    .InvokeMember(piComparedObj.Name,
                    BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null,
                    comparedToObject,
                    null);


                // -------------------------------
                // now compare the property values
                // -------------------------------
                if (srcValue.GetType() != comparedObjValue.GetType())
                    return false;

                if (!srcValue.ToString().Equals(comparedObjValue.ToString()))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Set the specified <b>public</b> field value of an object
        /// </summary>
        /// <param name="vo">the Value Object on which setting is to be performed</param>
        /// <param name="fieldName">Field name</param>
        /// <param name="fieldValue">Value to be set</param>
        public static object SetField(object vo, string fieldName, Object fieldValue)
        {
            if (vo == null)
                throw new System.ArgumentNullException("No object specified to set.");

            if (fieldName == null)
                throw new System.ArgumentNullException("No field name specified.");

            if (string.IsNullOrEmpty(fieldName) || (fieldName.Length < 1))
                throw new System.ArgumentException("Field name cannot be empty.");


            FieldInfo fieldInfo = vo.GetType().GetField(fieldName);

            if (fieldInfo == null)
                throw new System.ArgumentException("The class '" + vo.GetType().Name + "' does not have the field '" + fieldName + "'");

            // Set the value 
            vo.GetType().InvokeMember(fieldInfo.Name,
                                        BindingFlags.SetField,
                                        null,
                                        vo,
                                        new object[] { fieldValue });
            return vo;
        }

        /// <summary>
        /// Set the specified property value of an object
        /// </summary>
        /// <param name="vo">the Value Object on which setting is to be performed</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyValue">Value to be set</param>
        public static Object SetProperty(object vo, string propertyName, Object propertyValue)
        {
            if (vo == null)
                throw new System.ArgumentException("No object specified to set.");

            if ((propertyName == null) || (propertyName.Length < 1))
                throw new System.ArgumentException("No property specified to set.");


            PropertyInfo propInfo = vo.GetType().GetProperty(propertyName);

            if (propInfo == null)
                throw new System.ArgumentException("The class '" + vo.GetType().Name + "' does not have the property '" + propertyName + "'");

            Object destValue;

            if (propInfo.PropertyType.IsEnum)
                vo = PropertyHelper.SetEnumTypeProperty(vo, propInfo.Name, propertyValue.ToString(), true);
            else
            {
                // Convert if necessary
                destValue = ConverterHelper.Convert(propertyValue, propInfo.PropertyType);


                // Set the value 
                vo.GetType().InvokeMember(propInfo.Name,
                                            BindingFlags.SetProperty,
                                            null,
                                            vo,
                                            new object[] { destValue });

            }
            return vo;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> representation of the value object
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns>string that represents the <see cref="System.Object"/></returns>
        public static string ToString(object valueObject)
        {
            StringBuilder buffy = new StringBuilder();

            buffy.Append("[");
            buffy.Append(valueObject.GetType().FullName);
            buffy.Append("]\n");

            // null objects cannot be reflected
            if (valueObject == null)
            {
                buffy.Append(" is null.");
                return buffy.ToString();
            }

            foreach (PropertyInfo objProperty in valueObject.GetType().GetProperties())
            {
                string propName;

                propName = objProperty.Name + "=";
                buffy.Append(propName);

                if (objProperty != null)
                {
                    object propValue = valueObject.GetType().InvokeMember(objProperty.Name,
                        BindingFlags.GetProperty,
                        null,
                        valueObject,
                        null);

                    if (propValue != null)
                        buffy.Append(propValue.ToString() + "\n");
                    else
                        buffy.Append("<null>\n");
                }
                else
                    buffy.Append("<null>\n");
            }

            foreach (FieldInfo objField in valueObject.GetType().GetFields())
            {
                string fieldName;

                fieldName = objField.Name + "=";
                buffy.Append(fieldName);

                if (objField != null)
                {
                    object fieldValue = valueObject.GetType().InvokeMember(objField.Name,
                        BindingFlags.GetField,
                        null,
                        valueObject,
                        null);

                    buffy.Append(fieldValue.ToString() + "\n");
                }
                else
                    buffy.Append("<null>\n");

            }
            // ----------- End ---------			
            return buffy.ToString();
        }
    }
}
