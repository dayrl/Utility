namespace Zdd.Utility
{
    /// <summary>
    /// һЩ�ֽڴ�������ֹ���
    /// </summary>
    public static class ByteHelper
    {
        /// <summary>
        /// �Ƚ�����Byte�����ֵ�Ƿ���ȡ�
        /// </summary>
        /// <param name="array1">Ҫ�Ƚϵ�����1</param>
        /// <param name="array2">Ҫ�Ƚϵ�����1</param>
        /// <returns>���������е�ֵ��ȫ��ȷ���True�����򷵻�False</returns>
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