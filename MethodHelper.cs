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
using System.Reflection;

namespace Zdd.Utility
{
	/// <summary>
	/// This class contains utility methods that 
	/// perform operations on object methods at runtime. 
	/// </summary>
	/// <remarks> 
	/// ?Copyright 2006 by Edward Lim.
	/// All rights reserved.
	/// </remarks>
	public static class MethodHelper
	{
		/// <summary>
		/// Determine if a method exists in an object
		/// </summary>
		/// <param name="srcObj"></param>
		/// <param name="methodName"></param>
		/// <returns>true if a method exists in an object, false otherwise</returns>
		/// <exception cref="ArgumentNullException">if srcobj is null</exception>
		public static bool Exists(object srcObj, string methodName)
		{
			if (srcObj == null)
				throw new ArgumentNullException("srcObj");

			MethodInfo methodInfo = srcObj.GetType().GetMethod(methodName);

			return (methodInfo != null);			
		}
		
		/// <summary>
		/// Invoke an method of an object if it exists
		/// </summary>
		/// <param name="srcObj">the object in which the method to invoke</param>
		/// <param name="methodName">method name to invoke</param>
		/// <returns>the value return after invoking a method</returns>
		/// <exception cref="ArgumentNullException">if srcobj is null</exception>
		/// <exception cref="ArgumentException">if methodName is null or empty</exception>
		public static object InvokeMethod(Object srcObj, string methodName)
		{

			if (srcObj == null)
				throw new ArgumentNullException("srcObj");

			if ((methodName == null) || (methodName.Length < 1))
				throw new ArgumentException("methodName cannot be null or empty");

			MethodInfo methodInfo = srcObj.GetType().GetMethod(methodName);

			if (methodInfo == null)
				return null;

			return methodInfo.Invoke(srcObj, new object[]{});
		}
	}
}
