using System;
using System.Reflection;

namespace Zdd.Utility
{
	/// <summary>
	/// This class contains utility functions that perform operations on Fields in
	/// objects at runtime.	
	/// </summary>
	public static class FieldHelper
	{
		/// <summary>
		/// Determine if a field exists in an object
		/// </summary>
		/// <param name="fieldName">Name of the field </param>
		/// <param name="srcObject">the object to inspect</param>
		/// <param name="bSearchPrivate">flag to indicate if the the search should include private fields</param>
		/// <returns>true if the field exists, false otherwise</returns>
		/// <exception cref="ArgumentNullException">if srcObject is null</exception>
		/// <exception cref="ArgumentException">if fieldName is empty or nukk </exception>
		public static bool Exists(string fieldName, object srcObject, bool bSearchPrivate)
		{
			if (srcObject == null)
				throw new ArgumentNullException("srcObject");

            if (string.IsNullOrEmpty(fieldName))
				throw new ArgumentException("Field name cannot be empty or null.");

			BindingFlags bf = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

			if (bSearchPrivate)
				bf = bf | BindingFlags.NonPublic;
		
			FieldInfo fieldInfoSrcObj = srcObject.GetType().GetField(fieldName, bf);

			return (fieldInfoSrcObj != null);
		}

		/// <summary>
		/// Set a field value if the field exists and is accessible in the object
		/// </summary>				
		/// <param name="fieldName">Name of the field </param>
		/// <param name="fieldValue">Value to set the field to</param>
		/// <param name="srcObject">the object to inspect</param>
		/// <param name="bSetPrivate">flag to indicate if field to set is a private field</param>
		/// <returns></returns>
		public static object SetField(string fieldName, object fieldValue, object srcObject,  bool bSetPrivate)
		{
			BindingFlags bindingflags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

			if (bSetPrivate)
				bindingflags = bindingflags | BindingFlags.NonPublic;

			if (Exists(fieldName, srcObject, bSetPrivate))
			{
				FieldInfo fieldInfo = srcObject.GetType().GetField(fieldName,bindingflags);

				if (fieldInfo != null)
				{
					// Convert if necessary
                    Object destValue = ConverterHelper.Convert(fieldValue, fieldInfo.FieldType);

					// Set the value 
					srcObject.GetType().InvokeMember(fieldInfo.Name,
													BindingFlags.SetField | bindingflags,
													null,
													srcObject,
													new object[]{destValue});
				}
			}

			return srcObject;
		}
	}
}
