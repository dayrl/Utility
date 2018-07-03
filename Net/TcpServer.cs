//create by xiaowy 2008-10-8

namespace Zdd.Utility
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// 代表一个tcp连接客户端 
    /// </summary>
    public class TcpServer : IDisposable
    {
        #region 成员变量

        private DateTime connectedTime;
        private DateTime lastReceiveTime;
        private int port;
        private byte[] receiveBuffer;
        private IPAddress remoteIP;
        private Socket socket;
        private int transfromRate;

        #endregion

        #region 公共事件

        /// <summary>
        /// 网络连接断开后时发生
        /// </summary>
        public event EventHandler Disconnected;
        /// <summary>
        /// 数据到达时发生
        /// </summary>
        public event EventHandler<DataArrivedEventArgs> DataArrived;
        /// <summary>
        /// 数据发送完成时发生
        /// </summary>
        public event EventHandler<SendCompleteEventArgs> SendComplete;

        #endregion

        #region 构造函数

        /// <summary>
        /// 代表一个tcp连接客户端
        /// </summary>
        /// <param name="socket">套接字</param>
        public TcpServer(Socket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket", "套结字不能为空。");
            }

            this.socket = socket;
            transfromRate = 0;
            lastReceiveTime = DateTime.Now;
            connectedTime = DateTime.Now;
            receiveBuffer = new byte[0x2000];
            remoteIP = ((IPEndPoint)socket.RemoteEndPoint).Address;
            port = ((IPEndPoint)socket.RemoteEndPoint).Port;
            BeginReceive();
        }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取连接时间 
        /// </summary>
        public DateTime ConnectedTime
        {
            get { return connectedTime; }
        }

        /// <summary>
        /// 获取远端IPAddress
        /// </summary>
        public IPAddress IPAddress
        {
            get { return remoteIP; }
        }

        /// <summary>
        /// 获取连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (socket == null)
                    return false;
                return socket.Connected;
            }
        }

        /// <summary>
        /// 获取连接端口
        /// </summary>
        public int Port
        {
            get { return  port; }
        }

        /// <summary>
        /// 获取数据传输速率
        /// </summary>
        public int TransfromRate
        {
            get { return transfromRate; }
        }

        #endregion

        #region 保护方法

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <returns>成功返回true,失败返回false</returns>
        protected bool BeginSend(byte[] data)
        {
            if ((socket == null) || !socket.Connected)
            {
                return false;
            }

            if (data.Length != 0)
            {
                try
                {
                    socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
                }
                catch (Exception)
                {
                    Disconnect();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect();
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        protected virtual void OnDisconnected()
        {
            if (Disconnected != null)
            {
               Disconnected(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 发送完成
        /// </summary>
        protected virtual void OnSendComplete(bool success, object key)
        {
            if (SendComplete != null)
            {
                SendCompleteEventArgs e = new SendCompleteEventArgs(success, key);
                SendComplete(this, e);
            }
        }

        #endregion

        #region 私有方法

        private void BeginReceive()
        {
            try
            {
                socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), receiveBuffer);
            }
            catch
            {
                Disconnect();
            }
        }

        private void ReceiveCallback(IAsyncResult iar)
        {
            byte[] asyncState = (byte[])iar.AsyncState;
            int size = 0;
            try
            {
                size = socket.EndReceive(iar);
            }
            catch (Exception)
            {
                Disconnect();
                return;
            }
            if (size != 0)
            {
                OnDataArrived(asyncState);
                BeginReceive();
            }
            else
            {
                Disconnect();
            }
        }

        protected virtual void OnDataArrived(byte[] data)
        {
            if (DataArrived != null)
                DataArrived(this, new DataArrivedEventArgs(data));
        }

        private void SendCallback(IAsyncResult iar)
        {
            try
            {
                socket.EndSend(iar);
            }
            catch (Exception)
            {
                OnSendComplete(false, iar.AsyncState);
                Disconnect();
                return;
            }
            OnSendComplete(true, iar.AsyncState);
        }

        ~TcpServer()
        {
            Dispose(false);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 断开网络连接
        /// </summary>
        public virtual void Disconnect()
        {
            lock (this)
            {
                if (socket != null)
                {
                    try
                    {
                        socket.Shutdown(SocketShutdown.Receive);
                        socket.Close();
                    }
                    catch (Exception)
                    {}
                    socket = null;
                    OnDisconnected();
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        #endregion
    }

    #region 事件参数

    /// <summary>
    /// 数据到达事件参数
    /// </summary>
    public class DataArrivedEventArgs : EventArgs
    {
        private readonly byte[] data;

        /// <summary>
        /// 数据到达事件参数
        /// </summary>
        /// <param name="data">数据</param>
        public DataArrivedEventArgs(byte[] data)
        {
            this.data = data;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public byte[] Data
        {
            get { return data; }
        }
    }

    /// <summary>
    /// 网络连接事件参数 
    /// </summary>
    public class TcpServerEventArgs : EventArgs
    {
        private readonly TcpServer server;

        /// <summary>
        /// 网络连接事件参数
        /// </summary>
        /// <param name="server">tcp客户端对象</param>
        public TcpServerEventArgs(TcpServer server)
        {
            this.server = server;
        }

        /// <summary>
        /// 获取一个tcp客户端对象
        /// </summary>
        public TcpServer Server
        {
            get { return server; }
        }
    }

    /// <summary>
    /// 数据发送完成事件参数 
    /// </summary>
    public class SendCompleteEventArgs : EventArgs
    {
        private readonly object key;
        private readonly bool success;

        /// <summary>
        /// 数据发送完成事件参数
        /// </summary>
        public SendCompleteEventArgs(bool result, object key)
        {
            success = result;
            this.key = key;
        }

        /// <summary>
        /// 获取自定义的参数
        /// </summary>
        public object Key
        {
            get { return key; }
        }

        /// <summary>
        /// 获取是否发送成功，成功为true
        /// </summary>
        public bool Success
        {
            get { return success; }
        }
    }

    /// <summary>
    /// 客户端连接已移除事件参数 
    /// </summary>
    public class ServerRemovedEventArgs : EventArgs
    {
        private readonly int index;

        /// <summary>
        /// 客户端连接已移除事件参数
        /// </summary>
        /// <param name="index">客户端在客户端列表中的索引</param>
        public ServerRemovedEventArgs(int index)
        {
            this.index = index;
        }

        /// <summary>
        /// 获取客户端在列表中的索引
        /// </summary>
        public int Index
        {
            get { return index; }
        }
    }

    /// <summary>
    /// 客户端已连接事件参数 
    /// </summary>
    public class ConnectedEventArgs : EventArgs
    {
        private readonly Socket socket;
        /// <summary>
        /// 客户端已连接事件参数
        /// </summary>
        /// <param name="socket">套接字</param>
        public ConnectedEventArgs(Socket socket)
        {
            this.socket = socket;
        }

        /// <summary>
        /// 获取套接字
        /// </summary>
        public Socket Socket
        {
            get { return socket; }
        }
    }

    #endregion
}

