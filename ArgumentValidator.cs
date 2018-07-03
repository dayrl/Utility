using System;

namespace Zdd.Utility
{
    /// <summary>
    /// ����������֤���ߡ�
    /// </summary>
    public static class ArgumentValidator
    {
        /// <summary>
        /// ������Ϊ�յĲ�����֤��
        /// </summary>
        /// <param name="parameter">Ҫ��֤�Ĳ�����</param>
        /// <param name="parameterName">��֤�Ĳ������ơ�</param>
        public static void NotNullValidator(object parameter, string parameterName)
        {
            if (null == parameter)
                throw new ArgumentNullException(parameterName, "��������Ϊ�ա�");
        }

        /// <summary>
        /// ����������Χ��֤��
        /// </summary>
        /// <param name="minValue">�������������Сֵ��</param>
        /// <param name="maxValue">��������������ֵ��</param>
        /// <param name="actualValue">Ҫ��֤�Ĳ���ʵ��ֵ��</param>
        /// <param name="parameterName">��֤�Ĳ������ơ�</param>
        public static void ValueRangeValidator(int minValue, int maxValue, int actualValue, string parameterName)
        {
            if (actualValue < minValue || actualValue > maxValue)
                throw new ArgumentOutOfRangeException(parameterName, actualValue, "����������ɵķ�Χ��");
        }

        /// <summary>
        /// �����Ͳ�����Χ��֤��
        /// </summary>
        /// <param name="minValue">�������������Сֵ��</param>
        /// <param name="maxValue">��������������ֵ��</param>
        /// <param name="actualValue">Ҫ��֤�Ĳ���ʵ��ֵ��</param>
        /// <param name="parameterName">��֤�Ĳ������ơ�</param>
        public static void ValueRangeValidator(long minValue, long maxValue, long actualValue, string parameterName)
        {
            if (actualValue < minValue || actualValue > maxValue)
                throw new ArgumentOutOfRangeException(parameterName, actualValue, "����������ɵķ�Χ��");
        }

        /// <summary>
        /// �����Ͳ�����Χ��֤��
        /// </summary>
        /// <param name="minValue">�������������Сֵ��</param>
        /// <param name="maxValue">��������������ֵ��</param>
        /// <param name="actualValue">Ҫ��֤�Ĳ���ʵ��ֵ��</param>
        /// <param name="parameterName">��֤�Ĳ������ơ�</param>
        public static void ValueRangeValidator(float minValue, float maxValue, float actualValue, string parameterName)
        {
            if (actualValue < minValue || actualValue > maxValue)
                throw new ArgumentOutOfRangeException(parameterName, actualValue, "����������ɵķ�Χ��");
        }

        /// <summary>
        /// ����ʱ�������Χ��֤��
        /// </summary>
        /// <param name="minValue">�������������Сֵ��</param>
        /// <param name="maxValue">��������������ֵ��</param>
        /// <param name="actualValue">Ҫ��֤�Ĳ���ʵ��ֵ��</param>
        /// <param name="parameterName">��֤�Ĳ������ơ�</param>
        public static void ValueRangeValidator(DateTime minValue, DateTime maxValue, DateTime actualValue,
                                               string parameterName)
        {
            if (actualValue < minValue || actualValue > maxValue)
                throw new ArgumentOutOfRangeException(parameterName, actualValue, "����������ɵķ�Χ��");
        }

        /// <summary>
        /// ʱ����������Χ��֤��
        /// </summary>
        /// <param name="minValue">�������������Сֵ��</param>
        /// <param name="maxValue">��������������ֵ��</param>
        /// <param name="actualValue">Ҫ��֤�Ĳ���ʵ��ֵ��</param>
        /// <param name="parameterName">��֤�Ĳ������ơ�</param>
        public static void ValueRangeValidator(TimeSpan minValue, TimeSpan maxValue, TimeSpan actualValue,
                                               string parameterName)
        {
            if (actualValue < minValue || actualValue > maxValue)
                throw new ArgumentOutOfRangeException(parameterName, actualValue, "����������ɵķ�Χ��");
        }
    }
}