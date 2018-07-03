using System;

namespace Zdd.Utility
{
    /// <summary>
    /// 方法参数验证工具。
    /// </summary>
    public static class ArgumentValidator
    {
        /// <summary>
        /// 不允许为空的参数验证。
        /// </summary>
        /// <param name="parameter">要验证的参数。</param>
        /// <param name="parameterName">验证的参数名称。</param>
        public static void NotNullValidator(object parameter, string parameterName)
        {
            if (null == parameter)
                throw new ArgumentNullException(parameterName, "参数不能为空。");
        }

        /// <summary>
        /// 整数参数范围验证。
        /// </summary>
        /// <param name="minValue">参数所允许的最小值。</param>
        /// <param name="maxValue">参数所允许的最大值。</param>
        /// <param name="actualValue">要验证的参数实际值。</param>
        /// <param name="parameterName">验证的参数名称。</param>
        public static void ValueRangeValidator(int minValue, int maxValue, int actualValue, string parameterName)
        {
            if (actualValue < minValue || actualValue > maxValue)
                throw new ArgumentOutOfRangeException(parameterName, actualValue, "参数超过许可的范围。");
        }

        /// <summary>
        /// 长整型参数范围验证。
        /// </summary>
        /// <param name="minValue">参数所允许的最小值。</param>
        /// <param name="maxValue">参数所允许的最大值。</param>
        /// <param name="actualValue">要验证的参数实际值。</param>
        /// <param name="parameterName">验证的参数名称。</param>
        public static void ValueRangeValidator(long minValue, long maxValue, long actualValue, string parameterName)
        {
            if (actualValue < minValue || actualValue > maxValue)
                throw new ArgumentOutOfRangeException(parameterName, actualValue, "参数超过许可的范围。");
        }

        /// <summary>
        /// 浮点型参数范围验证。
        /// </summary>
        /// <param name="minValue">参数所允许的最小值。</param>
        /// <param name="maxValue">参数所允许的最大值。</param>
        /// <param name="actualValue">要验证的参数实际值。</param>
        /// <param name="parameterName">验证的参数名称。</param>
        public static void ValueRangeValidator(float minValue, float maxValue, float actualValue, string parameterName)
        {
            if (actualValue < minValue || actualValue > maxValue)
                throw new ArgumentOutOfRangeException(parameterName, actualValue, "参数超过许可的范围。");
        }

        /// <summary>
        /// 日期时间参数范围验证。
        /// </summary>
        /// <param name="minValue">参数所允许的最小值。</param>
        /// <param name="maxValue">参数所允许的最大值。</param>
        /// <param name="actualValue">要验证的参数实际值。</param>
        /// <param name="parameterName">验证的参数名称。</param>
        public static void ValueRangeValidator(DateTime minValue, DateTime maxValue, DateTime actualValue,
                                               string parameterName)
        {
            if (actualValue < minValue || actualValue > maxValue)
                throw new ArgumentOutOfRangeException(parameterName, actualValue, "参数超过许可的范围。");
        }

        /// <summary>
        /// 时间间隔参数范围验证。
        /// </summary>
        /// <param name="minValue">参数所允许的最小值。</param>
        /// <param name="maxValue">参数所允许的最大值。</param>
        /// <param name="actualValue">要验证的参数实际值。</param>
        /// <param name="parameterName">验证的参数名称。</param>
        public static void ValueRangeValidator(TimeSpan minValue, TimeSpan maxValue, TimeSpan actualValue,
                                               string parameterName)
        {
            if (actualValue < minValue || actualValue > maxValue)
                throw new ArgumentOutOfRangeException(parameterName, actualValue, "参数超过许可的范围。");
        }
    }
}