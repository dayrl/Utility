using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Zdd.Utility
{
    /// <summary>
    /// �ṹ������
    /// </summary>
    public class StructHelper
    {
        /// <summary>
        /// �ṹ��ת����bytes
        /// </summary>
        public static byte[] StructToBytes<T>(T structure) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr handle = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, handle, true);
                byte[] buffer = new byte[size];
                Marshal.Copy(handle, buffer, 0, size);
                return buffer;
            }
            finally
            {
                Marshal.FreeHGlobal(handle);
            }
            
        }
            
        /// <summary>
        /// �ṹ��ת����bytes
        /// </summary>
        /// <typeparam name="T">�ṹ��</typeparam>
        /// <param name="structure">�ṹ��</param>
        /// <param name="buffer">����</param>
        /// <param name="startIndex">��ʼλ��</param>
        /// <returns>�ɹ�</returns>
        public static bool StructToBytes<T>(T structure, byte[] buffer, int startIndex) where T : struct
        {
            if (buffer == null || buffer.Length == 0)
                return false;

            int size = Marshal.SizeOf(typeof(T));
            IntPtr handle = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, handle, true);
                if (buffer.Length - startIndex >= size)
                {
                    Marshal.Copy(handle, buffer, startIndex, size);
                    return true;
                }
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(handle);
            }


        }

      

        /// <summary>
        /// �ṹ��ת����bytes
        /// </summary>
        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structObj, buffer, true);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        ///  bytesת�����ṹ��
        /// </summary>
        /// <typeparam name="T">�ṹ��</typeparam>
        /// <param name="buffer">����</param>
        /// <param name="startIndex">��ʼλ��</param>
        /// <returns></returns>
        public static T BytesToStruct<T>(byte[] buffer, int startIndex) where T : struct
        {
            IntPtr handle = IntPtr.Zero;
            T structClass = new T();
            try
            {
                int size = Marshal.SizeOf(typeof(T));
                handle = Marshal.AllocHGlobal(size);
                Marshal.Copy(buffer, startIndex, handle, size);
                structClass = (T)Marshal.PtrToStructure(handle, typeof(T));
            }
            catch (ArgumentException)
            {

            }
            catch (Exception)
            {

            }
            finally
            {
                if (handle != IntPtr.Zero)
                {
                    try
                    {
                        //������ܻ���������Ԥ�ϵĴ���
                        Marshal.FreeHGlobal(handle);
                    }
                    catch
                    {
                    }
                }
            }
            return structClass;
        }
        /// <summary>
        /// bytesת�����ṹ��
        /// </summary>
        public static T BytesToStruct<T>(byte[] buffer) where T : struct
        {
            return BytesToStruct<T>(buffer, 0);
        }

        /// <summary>
        /// bytesת�����ṹ��
        /// </summary>
        public static object BytesToStruct(byte[] bytes, Type strcutType)
        {
            int size = Marshal.SizeOf(strcutType);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure(buffer, strcutType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// Objects to bytes.
        /// </summary>
        /// <typeparam name="T">T����֧�ֿ����л�</typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static byte[] ObjectToBytes(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            byte[] data = new byte[0];

            try
            {
                formatter.Serialize(stream, obj);
                data = stream.ToArray();
            }
            catch
            {
                return data;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return data;
        }

        /// <summary>
        /// Byteses to object.
        /// </summary>
        /// <typeparam name="T">T����֧�ֿ����л�</typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static T BytesToObject<T>(byte[] data)
        {
            T t;
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                stream.Write(data, 0, data.Length);
                stream.Seek(0, SeekOrigin.Begin);
                t = (T)formatter.Deserialize(stream);
            }
            catch
            {
                return default(T);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return t;

        }

        /// <summary>
        /// Byteses to object.
        /// </summary>
        /// <typeparam name="T">T����֧�ֿ����л�</typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static object BytesToObject(byte[] data)
        {
            object t;
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                stream.Write(data, 0, data.Length);
                stream.Seek(0, SeekOrigin.Begin);
                t = formatter.Deserialize(stream);
            }
            catch
            {
              return  null;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return t;

        }
    }
}
