//create by xiaowy 2008-10-8

namespace Zdd.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// tcp连接管理类，自动为客户端建立连接对象
    /// 暂不支持IP限制
    /// </summary>
    public class TcpServerManager
    {
        #region 成员变量

        private int port;
        private ITcpListener listener;
        private List<TcpServer> servers;

        #endregion 

        #region 公共事件

        /// <summary>
        /// 客户端连接已建立时发生
        /// </summary>
        public event EventHandler<TcpServerEventArgs> ServerCreated;
        /// <summary>
        /// 客户端连接移除后发生
        /// </summary>
        public event EventHandler<ServerRemovedEventArgs> ServerRemoved;
        /// <summary>
        /// 客户端连接移除前发生
        /// </summary>
        public event EventHandler<TcpServerEventArgs> ServerRemoving;

        #endregion

        #region 构造函数

        /// <summary>
        /// tcp连接管理
        /// </summary>
        public TcpServerManager()
            : this(0)
        {
            servers = new List<TcpServer>();
        }

        /// <summary>
        /// tcp连接管理
        /// </summary>
        /// <param name="port">本地服务端口</param>
        public TcpServerManager(int port)
        {
            this.port = port;
        }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取或设置本地服务端口
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// 获取已连接的客户端列表
        /// </summary>
        public ReadOnlyCollection<TcpServer> Servers
        {
            get { return servers.AsReadOnly(); }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <returns>成功返回true,失败返回false</returns>
        public virtual bool Start()
        {
            if (listener == null)
            {
                listener = new ThreadTcpListener(port);
                listener.Connected += new EventHandler<ConnectedEventArgs>(listener_Connected);
            }
            if (listener.IsListening)
            {
                listener.Close();
            }
            return listener.Start();
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        public virtual void Stop()
        {
            if (listener != null)
            {
                listener.Close();
                listener = null;
            }
            if (servers.Count != 0)
            {
                TcpServer[] array = new TcpServer[servers.Count];
                lock (servers)
                {
                    for (int j = 0; j < servers.Count; j++)
                    {
                        array[j] = servers[j];
                    }
                }
                for (int i = 0; i < array.Length; i++)
                {
                    array[i].Disconnect();
                }
            }
        }

        #endregion

        #region 私有方法

        private void listener_Connected(object sender, ConnectedEventArgs e)
        {
            TcpServer server = new TcpServer(e.Socket);
            server.Disconnected += new EventHandler(server_Disconnected);

            lock (servers)
            {
                servers.Add(server);
            }

            OnServerCreated(server);

            if (!server.IsConnected)
                server.Disconnect();
        }

        private void server_Disconnected(object sender, EventArgs e)
        {
            lock (servers)
            {
                TcpServer server = sender as TcpServer;
                int index = servers.IndexOf(server);
                if (index != -1)
                {
                    OnServerRemoving(server);
                    servers.Remove(server);
                    if (null != server)
                        server.Disconnected -= new EventHandler(this.server_Disconnected);
                    OnServerRemoved(index);
                }
            }
        }

        #endregion

        #region 保护方法

        /// <summary>
        /// 引发客户端已连接事件
        /// </summary>
        /// <param name="server">已连接的客户端</param>
        protected virtual void OnServerCreated(TcpServer server)
        {
            if (ServerCreated != null)
            {
                TcpServerEventArgs e = new TcpServerEventArgs(server);
                ServerCreated(this, e);
            }
        }

        /// <summary>
        /// 引发客户端已移除事件
        /// </summary>
        /// <param name="index">移除前在客户端列表中的索引</param>
        protected virtual void OnServerRemoved(int index)
        {
            if (ServerRemoved != null)
            {
                ServerRemoved(this, new ServerRemovedEventArgs(index));
            }
        }

        /// <summary>
        /// 引发客户端移除前事件
        /// </summary>
        /// <param name="server">将要被也称的客户端</param>
        protected virtual void OnServerRemoving(TcpServer server)
        {
            if (ServerRemoving != null)
            {
                TcpServerEventArgs e = new TcpServerEventArgs(server);
                ServerRemoving(this, e);
            }
        }

        #endregion

    }
}
