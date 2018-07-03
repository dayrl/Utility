//create by xiaowy 2008-10-8

namespace Zdd.Utility
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// tcp监听线程，专门负责连接请求
    /// </summary>
    internal class ThreadTcpListener : ITcpListener
    {
        #region 成员变量

        private bool isListening;
        private Socket listenSocket;
        private Thread listenThread;
        private int port;

        #endregion

        #region 公共事件

        /// <summary>
        /// 当有新的客户端请求连接时发生
        /// </summary>
        public event EventHandler<ConnectedEventArgs> Connected;

        #endregion 

        #region 构造函数

        /// <summary>
        /// tcp监听线程，专门负责连接请求
        /// </summary>
        /// <param name="port">本地服务端口</param>
        public ThreadTcpListener(int port)
        {
            this.port = port;
           isListening = false;
           listenSocket = null;
           listenThread = null;
        }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取是否正在监听
        /// </summary>
        public bool IsListening
        {
            get { return isListening; }
        }

        /// <summary>
        /// 获取监听端口
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 启动监听
        /// </summary>
        /// <returns>成功返回true,失败返回false</returns>
        public bool Start()
        {
            if (isListening)
            {
                Close();
            }
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
            try
            {
                listenSocket.Bind(localEP);
            }
            catch (Exception)
            {
                return false;
            }

            int maxConnection;
            try
            {
                maxConnection = int.Parse(ConfigurationManager.AppSettings["MaxConnection"].ToString());
            }
            catch (Exception)
            {
                maxConnection = 100;
            }

            listenSocket.Listen(maxConnection);
            isListening = true;
            listenThread = new Thread(new ThreadStart(Listening));
            listenThread.IsBackground = true;
            listenThread.Name = "Tcp listen thread";
            listenThread.Start();
            return true;
        }

        /// <summary>
        /// 关闭监听
        /// </summary>
        public void Close()
        {
            isListening = false;
            if (listenSocket != null)
            {
                listenSocket.Close();
                listenSocket = null;

                if (listenThread != null)
                {
                    listenThread.Join();
                    listenThread = null;
                }
            }
        }

        #endregion

        #region 私有方法

        private void Listening()
        {
            while (isListening)
            {
                Socket socket = null;
                try
                {
                    socket = listenSocket.Accept();
                }
                catch (Exception)
                {
                    break;
                }
                OnConnected(socket);
            }
        }

        private void OnConnected(Socket socket)
        {
            if (Connected != null)
            {
                ConnectedEventArgs e = new ConnectedEventArgs(socket);
                Connected(this, e);
            }
        }

        #endregion
    }
}
