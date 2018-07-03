using System;
using System.Collections.Generic;

namespace Zdd.Utility
{
    /// <summary>
    /// 泛型单件容器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static class Singleton<T> where T : class, new()
    {
        private static T _instance;

        /// <summary>获取特定类型的单件实例
        /// </summary>
        internal static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _instance, new T(), null);
                }
                return _instance;
            }
        }
    }
}