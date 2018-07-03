//create by xiaowy 2008-10-8

namespace Zdd.Utility
{
    using System;

    /// <summary>
    /// tcp连接监听接口
    /// </summary>
    internal interface ITcpListener
    {
        /// <summary>
        /// 有新的客户端请求连接时发生
        /// </summary>
        event EventHandler<ConnectedEventArgs> Connected;
        /// <summary>
        /// 关闭监听
        /// </summary>
        void Close();
        /// <summary>
        /// 开始监听
        /// </summary>
        /// <returns>成功返回true，失败返回false</returns>
        bool Start();
        /// <summary>
        /// 获取是否正在监听
        /// </summary>
        bool IsListening { get; }
        /// <summary>
        /// 获取监听端口
        /// </summary>
        int Port { get; set; }
    }

}
