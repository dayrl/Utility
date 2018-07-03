#region License and Copyright

/* -------------------------------------------------------------------------
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
 * 
 * Free Software Foundation, Inc., 
 * 59 Temple Place, 
 * Suite 330, 
 * Boston, 
 * MA 02111-1307 
 * USA 
 * 
 * -------------------------------------------------------------------------
 */

#endregion

using System;
using System.Diagnostics;

namespace Zdd.Utility
{
    /// <summary>		
    /// This class converts an object to another taking their Type into consideration.
    /// </summary>
    public static class ConverterHelper
    {
        #region System.Convert supported types

        // The types supported by System.Convert
        private static Type booleanType = Type.GetType("System.Boolean");
        private static Type charType = Type.GetType("System.Char");
        private static Type byteType = Type.GetType("System.Byte");
        private static Type int16Type = Type.GetType("System.Int16");
        private static Type int32Type = Type.GetType("System.Int32");
        private static Type int64Type = Type.GetType("System.Int64");
        private static Type uint16Type = Type.GetType("System.UInt16");
        private static Type uint32Type = Type.GetType("System.UInt32");
        private static Type uint64Type = Type.GetType("System.UInt64");
        private static Type singleType = Type.GetType("System.Single");
        private static Type doubleType = Type.GetType("System.Double");
        private static Type decimalType = Type.GetType("System.Decimal");
        private static Type dateTimeType = Type.GetType("System.DateTime");
        private static Type stringType = Type.GetType("System.String");

        #endregion

        /// <summary>
        /// Convert a value to another <see cref="System.Type"/>.
        /// </summary>
        /// <param name="srcValue"></param>
        /// <param name="destType"></param>
        /// <returns></returns>
        public static Object Convert(Object srcValue, Type destType)
        {
            // Handle bad parameters
            if ((srcValue == null) || (destType == null))
            {
                return (null);
            }

            Type srcType = srcValue.GetType();
            Object destValue = null;

            // If the source and destination types match, //
            // we don't have to perform any conversion.   //
            if (srcType == destType)
            {
                // We just need to try and copy it as best we can

                // Deep-copy cloneable objects
                if (srcValue is ICloneable)
                {
                    destValue = ((ICloneable) srcValue).Clone();
                }
                    // Shallow-copy references
                else if (srcValue is ValueType)
                {
                    destValue = srcValue;
                }
                    // If we can't copy, just return a reference
                else
                {
                    // destValue = srcValue;
                    destValue = null;
                }

                return (destValue);
            }


            //--------------------------------------------//
            // If the source is the string type and the   //
            // destination is an Enum, we simply call     //
            // Enum.Parse to do the conversion.	          //			
            //--------------------------------------------//
            if (srcValue is string && destType.IsEnum)
            {
                try
                {
                    return Enum.Parse(destType, (string) srcValue, true);
                }
                catch (Exception ex)
                {
                    Trace.Fail("An exception occurred while converting a string to the enum type " + destType.Name,
                               ex.ToString());
                    return null;
                }
            }

            //----------------------------------------------//
            // If the source is an enum and the destination //
            // type is a string, we simply get the name of  //
            // the enum value and set it to the dest value. //
            //----------------------------------------------//
            if (srcType.IsEnum && destType.Equals(typeof (string)))
            {
                try
                {
                    return Enum.GetName(srcType, srcValue);
                }
                catch (Exception ex)
                {
                    Trace.Fail(
                        String.Format(
                            "An exception occurred while converting a enum value {0} (of type '{1}') to a string.\n{2}",
                            srcValue, srcType.Name, ex.ToString()));
                    return null;
                }
            }


            //----------------------------------------------//
            // If the source is an enum and the destination //
            // type is an integral type, we simpy use the   //
            // standard System.Convert to set the			//
            // destination value.							//
            //----------------------------------------------//
            if (srcType.IsEnum && destType.Equals(typeof (Int32)))
                return System.Convert.ToInt32(srcValue);
            else if (srcType.IsEnum && destType.Equals(typeof (Int16)))
                return System.Convert.ToInt16(srcValue);
            else if (srcType.IsEnum && destType.Equals(typeof (Int64)))
                return System.Convert.ToInt64(srcValue);
            else if (srcType.IsEnum && destType.Equals(typeof (SByte)))
                return System.Convert.ToSByte(srcValue);


            //----------------------------------------------//
            // If the destination is an enum and the source //
            // type is an integral type, we simpy use the   //
            // Enum.Parse to set the destination value.		//
            //----------------------------------------------//
            if (destType.IsEnum &&
                (srcValue is Int32 ||
                 srcValue is Int16 ||
                 srcValue is Int64 ||
                 srcValue is SByte
                )
                )
                return Enum.Parse(destType, srcValue.ToString());


            //---------------------------------------------------------//
            // Convert using System.Convert if the types are supported //
            //---------------------------------------------------------//
            if ((TypeSupportedBySystemConvert(srcType)) && (TypeSupportedBySystemConvert(destType)))
            {
                // If the conversion can occur without an exception being thrown,
                // invoke System.Convert (note that a precision-related exception
                // can still occur
                if (ValidSystemConvertConversion(srcType, destType))
                {
                    try
                    {
                        #region System.Convert invocation

                        if (destType == stringType)
                        {
                            destValue = System.Convert.ToString(srcValue);
                        }
                        else if (destType == int16Type)
                        {
                            destValue = System.Convert.ToInt16(srcValue);
                        }
                        else if (destType == int32Type)
                        {
                            destValue = System.Convert.ToInt32(srcValue);
                        }
                        else if (destType == int64Type)
                        {
                            destValue = System.Convert.ToInt64(srcValue);
                        }
                        else if (destType == booleanType)
                        {
                            destValue = System.Convert.ToBoolean(srcValue);
                        }
                        else if (destType == charType)
                        {
                            destValue = System.Convert.ToChar(srcValue);
                        }
                        else if (destType == byteType)
                        {
                            destValue = System.Convert.ToByte(srcValue);
                        }
                        else if (destType == uint16Type)
                        {
                            destValue = System.Convert.ToUInt16(srcValue);
                        }
                        else if (destType == uint32Type)
                        {
                            destValue = System.Convert.ToUInt32(srcValue);
                        }
                        else if (destType == uint64Type)
                        {
                            destValue = System.Convert.ToUInt64(srcValue);
                        }
                        else if (destType == singleType)
                        {
                            destValue = System.Convert.ToSingle(srcValue);
                        }
                        else if (destType == doubleType)
                        {
                            destValue = System.Convert.ToDouble(srcValue);
                        }
                        else if (destType == decimalType)
                        {
                            destValue = System.Convert.ToDecimal(srcValue);
                        }
                        else if (destType == dateTimeType)
                        {
                            destValue = System.Convert.ToDateTime(srcValue);
                        }

                        return (destValue);

                        #endregion
                    } // try

                        // A precision or other unexpected exception might still occur
                    catch (Exception ex)
                    {
                        String errMsg = "An exception occurred while converting " + srcValue + "[" + srcType +
                                        "] to the type " + destType;
                        Trace.Fail(errMsg, ex.ToString());
                        throw new Exception(errMsg + ex.ToString());
                    }
                }
            } // If we can use System.Convert to Convert

            // If we get this far, we don't know what to do
            return (null);
        }

        /// <summary>
        /// Determines if the specified type supported by <see cref="System.Convert"/>.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Boolean TypeSupportedBySystemConvert(Type t)
        {
            // If the type is supported by System.Convert, return true
            if ((t == stringType) || (t == booleanType) || (t == byteType) || (t == int16Type) ||
                (t == int32Type) || (t == int64Type) || (t == uint16Type) || (t == uint32Type) ||
                (t == uint64Type) || (t == singleType) || (t == doubleType) || (t == decimalType) ||
                (t == dateTimeType) || (t == charType))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        /// <summary>
        /// Determines if <see cref="System.Convert"/> convert between the two specified types without
        /// throwing an exception.
        /// </summary>
        /// <param name="srcType"></param>
        /// <param name="destType"></param>
        /// <returns></returns>
        private static Boolean ValidSystemConvertConversion(Type srcType, Type destType)
        {
            // Char to Boolean, Single, Double, Decimal or DateTime fails
            if ((srcType == charType) && ((destType == booleanType) || (destType == singleType)
                                          || (destType == doubleType) || (destType == decimalType) ||
                                          (destType == dateTimeType)))
            {
                return false;
            }

                // Boolean, Single, Double, Decimal or DateTime to Char fails
            else if ((destType == charType) && ((srcType == booleanType) || (srcType == singleType)
                                                || (srcType == doubleType) || (srcType == decimalType) ||
                                                (srcType == dateTimeType)))
            {
                return false;
            }

                // Anything but String to DateTime fails
            else if ((srcType != stringType) && (destType == dateTimeType))
            {
                return false;
            }

                // DateTime to anything but String fails
            else if ((srcType == dateTimeType) && (destType != stringType))
            {
                return false;
            }

            // If we get this far, System.Convert will convert without throwing an exception
            return (true);
        }

        /// <summary>
        /// Determines whether type1 can be converted to type2. Check only for primitive types. 
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public static bool CanConvertFrom(Type type1, Type type2)
        {
            if (type1.IsPrimitive && type2.IsPrimitive)
            {
                TypeCode typeCode1 = Type.GetTypeCode(type1);
                TypeCode typeCode2 = Type.GetTypeCode(type2);

                // If both type1 and type2 have the same type, return true.
                if (typeCode1 == typeCode2)
                    return true;

                // Possible conversions from Char follow.
                if (typeCode1 == TypeCode.Char)
                    switch (typeCode2)
                    {
                        case TypeCode.UInt16:
                            return true;
                        case TypeCode.UInt32:
                            return true;
                        case TypeCode.Int32:
                            return true;
                        case TypeCode.UInt64:
                            return true;
                        case TypeCode.Int64:
                            return true;
                        case TypeCode.Single:
                            return true;
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                // Possible conversions from Byte follow.
                if (typeCode1 == TypeCode.Byte)
                    switch (typeCode2)
                    {
                        case TypeCode.Char:
                            return true;
                        case TypeCode.UInt16:
                            return true;
                        case TypeCode.Int16:
                            return true;
                        case TypeCode.UInt32:
                            return true;
                        case TypeCode.Int32:
                            return true;
                        case TypeCode.UInt64:
                            return true;
                        case TypeCode.Int64:
                            return true;
                        case TypeCode.Single:
                            return true;
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                // Possible conversions from SByte follow.
                if (typeCode1 == TypeCode.SByte)
                    switch (typeCode2)
                    {
                        case TypeCode.Int16:
                            return true;
                        case TypeCode.Int32:
                            return true;
                        case TypeCode.Int64:
                            return true;
                        case TypeCode.Single:
                            return true;
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                // Possible conversions from UInt16 follow.
                if (typeCode1 == TypeCode.UInt16)
                    switch (typeCode2)
                    {
                        case TypeCode.UInt32:
                            return true;
                        case TypeCode.Int32:
                            return true;
                        case TypeCode.UInt64:
                            return true;
                        case TypeCode.Int64:
                            return true;
                        case TypeCode.Single:
                            return true;
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                // Possible conversions from Int16 follow.
                if (typeCode1 == TypeCode.Int16)
                    switch (typeCode2)
                    {
                        case TypeCode.Int32:
                            return true;
                        case TypeCode.Int64:
                            return true;
                        case TypeCode.Single:
                            return true;
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                // Possible conversions from UInt32 follow.
                if (typeCode1 == TypeCode.UInt32)
                    switch (typeCode2)
                    {
                        case TypeCode.UInt64:
                            return true;
                        case TypeCode.Int64:
                            return true;
                        case TypeCode.Single:
                            return true;
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                // Possible conversions from Int32 follow.
                if (typeCode1 == TypeCode.Int32)
                    switch (typeCode2)
                    {
                        case TypeCode.Int64:
                            return true;
                        case TypeCode.Single:
                            return true;
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                // Possible conversions from UInt64 follow.
                if (typeCode1 == TypeCode.UInt64)
                    switch (typeCode2)
                    {
                        case TypeCode.Single:
                            return true;
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                // Possible conversions from Int64 follow.
                if (typeCode1 == TypeCode.Int64)
                    switch (typeCode2)
                    {
                        case TypeCode.Single:
                            return true;
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                // Possible conversions from Single follow.
                if (typeCode1 == TypeCode.Single)
                    switch (typeCode2)
                    {
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
            }
            return false;
        }
    }
}