namespace Zdd.Utility
{
    /// <summary>
    /// 一些字节处理的助手工具
    /// </summary>
    public static class ByteHelper
    {
        /// <summary>
        /// 比较两个Byte数组的值是否相等。
        /// </summary>
        /// <param name="array1">要比较的数组1</param>
        /// <param name="array2">要比较的数组1</param>
        /// <returns>两个数组中的值完全相等返回True，否则返回False</returns>
        public static bool CompareByteArray(byte[] array1, byte[] array2)
        {
            if (array1 == null || array2 == null)
                return false;

            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }
            return true;
        }
    }
}