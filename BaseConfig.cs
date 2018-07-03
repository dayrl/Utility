using System;
using System.IO;

namespace Zdd.Utility
{
    /// <summary>
    /// 本地配置文件的基础类，提供泛型的读取保存配置文件方法
    /// </summary>
    [Serializable]
    public class BaseConfig
    {
        /// <summary>
        /// 读取配置文件的方法
        /// 注意：不可以在子类的构造函数中调用此方法
        /// </summary>
        /// <typeparam name="TOutput">返回类型</typeparam>
        /// <param name="strCfgFileName">读取配置文件的文件名</param>
        /// <returns></returns>
        protected static TOutput GetConfig<TOutput>(string strCfgFileName)
            where TOutput : class
        {
            FileStream fs = null;
            try
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                //XmlSerializer xs = new XmlSerializer(typeof(TOutput));
                fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + strCfgFileName, FileMode.Open, FileAccess.Read);
                //TOutput config = (TOutput)xs.Deserialize(fs);
                TOutput config = (TOutput)formatter.Deserialize(fs);
                fs.Close();
                return config;
            }
            catch
            {
                if (fs != null)
                    fs.Close();
                return null;
            }
        }

        /// <summary>
        /// 保存配置文件的方法
        /// </summary>
        /// <param name="cfg">配置文件的实例化对象</param>
        /// <param name="strCfgFileName">保存配置文件的文件名</param>
        protected static void SetConfig(object cfg, string strCfgFileName)
        {
            FileStream fs = null;
            try
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                //XmlSerializer xs = new XmlSerializer(cfg.GetType());
                fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + strCfgFileName, FileMode.Create, FileAccess.Write);
                //xs.Serialize(fs, cfg);
                formatter.Serialize(fs, cfg);
                fs.Close();
            }
            catch
            {
                if (fs != null)
                    fs.Close();
                throw new Exception("Xml serialization failed!");
            }
        }
    }
}
