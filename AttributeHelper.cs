#region License and Copyright

/*
 * Dotnet Commons Reflection 
 *
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
 * 
 * 59 Temple Place, 
 * Suite 330, 
 * Boston, 
 * MA 02111-1307 
 * USA 
 * 
 */

#endregion

using System;
using System.Reflection;

namespace Zdd.Utility
{
    /// <summary>
    /// Utility class that wraps around reflection APIs to access <see cref="System.Attribute"/>.
    /// </summary>
    public static class AttributeHelper
    {
        /// <summary>
        /// Get an <see cref="System.Attribute"/> of an object.
        /// </summary>
        /// <param name="srcObj">The SRC obj.</param>
        /// <param name="attributeName">name of <see cref="System.Attribute"/> to search</param>
        /// <param name="ignoreCase">ignore case sensitivity for the name of the <see cref="System.Attribute"/> to search</param>
        /// <returns>
        /// Custom <see cref="System.Attribute"/> if found, null if otherwise
        /// </returns>
        public static Object GetAttribute(object srcObj, string attributeName, bool ignoreCase)
        {
            if (srcObj == null)
                throw new ArgumentNullException("srcObj");

            if (srcObj is MethodInfo)
                return GetMethodAttribute((MethodInfo) srcObj, attributeName, ignoreCase);

            if (srcObj is PropertyInfo)
                return GetPropertyAttribute((PropertyInfo) srcObj, attributeName, ignoreCase);

            if (srcObj is FieldInfo)
                return GetFieldAttribute((FieldInfo) srcObj, attributeName, ignoreCase);

            Type srcType = srcObj.GetType();

            Object[] attributes = srcType.GetCustomAttributes(true);

            return searchForAttribute(attributeName, attributes, ignoreCase);
        }

        /// <summary>
        /// Get an attribute of a method
        /// </summary>
        /// <param name="mi"><see cref="MethodInfo"/> of a method.</param>
        /// <param name="attributeName">name of attribute to search</param>
        /// <param name="ignoreCase">ignore case sensitivity for the name of the attribute to search</param>
        /// <returns>Custom attribute if found, null if otherwise</returns>
        public static Object GetMethodAttribute(MethodInfo mi, string attributeName, bool ignoreCase)
        {
            if (mi == null)
                throw new ArgumentNullException("mi");

            Object[] attributes = mi.GetCustomAttributes(true);

            return searchForAttribute(attributeName, attributes, ignoreCase);
        }

        /// <summary>
        /// Get an attribute of a property
        /// </summary>
        /// <param name="pi"><see cref="PropertyInfo"/> of a property</param>
        /// <param name="attributeName">name of attribute to search</param>
        /// <param name="ignoreCase">ignore case sensitivity for the name of the attribute to search</param>
        /// <returns>Custom attribute if found, null if otherwise</returns>
        public static Object GetPropertyAttribute(PropertyInfo pi, string attributeName, bool ignoreCase)
        {
            if (pi == null)
                throw new ArgumentNullException("pi");

            Object[] attributes = pi.GetCustomAttributes(true);

            return searchForAttribute(attributeName, attributes, ignoreCase);
        }

        /// <summary>
        /// Get an attribute of a field.
        /// </summary>
        /// <param name="fi"><see cref="FieldInfo" /> of a field </param>
        /// <param name="attributeName">name of attribute to search</param>
        /// <param name="ignoreCase">ignore case sensitivity for the name of the attribute to search</param>
        /// <returns>Custom attribute if found, null if otherwise</returns>
        public static Object GetFieldAttribute(FieldInfo fi, string attributeName, bool ignoreCase)
        {
            if (fi == null)
                throw new ArgumentNullException("fi");

            Object[] attributes = fi.GetCustomAttributes(true);

            return searchForAttribute(attributeName, attributes, ignoreCase);
        }

        /// <summary>
        /// Search for an attribute based on the attribute class shortname
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>
        /// Custom attribute if found, null if otherwise
        /// </returns>
        private static object searchForAttribute(string attributeName, object[] attributes, bool ignoreCase)
        {
            foreach (Object attr in attributes)
            {
                string attrClassName = attributeName + "Attribute";

                if (ignoreCase && attr.GetType().Name.ToLower().Equals(attrClassName.ToLower()))
                    return attr;

                if (attr.GetType().Name.Equals(attrClassName))
                    return attr;
            }
            return null;
        }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <param name="srcObj">The SRC obj.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="valuePropertyName">Name of the value property.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static object GetAttributeValue(object srcObj, string attributeName, string valuePropertyName,
                                               bool ignoreCase)
        {
            if (srcObj == null) throw new ArgumentNullException("srcObj");
            Object attr = GetAttribute(srcObj, attributeName, ignoreCase);
            if (attr == null)
                return null;

            return ObjectHelper.GetProperty(attr, valuePropertyName, ignoreCase);
        }

        /// <summary>
        /// Set the value of the attribute only if it has a setter property.
        /// </summary>
        /// <param name="srcObj">The SRC obj.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="valuePropertyName">Name of the value property.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <param name="attrValue">The attr value.</param>
        public static void SetAttributeValue(object srcObj, string attributeName, string valuePropertyName,
                                             bool ignoreCase, object attrValue)
        {
            Object attr = GetAttribute(srcObj, attributeName, ignoreCase);
            if (attr == null)
                return;

            // Ensure Property Is writeable
            if (!PropertyHelper.IsWritable(attr, valuePropertyName, ignoreCase))
                return;

            ObjectHelper.SetProperty(attr, valuePropertyName, attrValue);
        }
    }
}