using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;

namespace Zdd.Utility
{
    /// <summary>
    /// 网络通讯事件模型委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">TcpClient</param>
    public delegate void NetEventHandler(IDataTransmit sender, NetEventArgs e);

    /// <summary>
    /// 网络事件参数
    /// </summary>
    public class NetEventArgs : EventArgs
    {
        private object eventArg;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="EventArg"></param>
        public NetEventArgs(object EventArg)
        {
            eventArg = EventArg;
        }
        /// <summary>
        /// 事件参数
        /// </summary>
        public object EventArg
        {
            get { return eventArg; }
            set { eventArg = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (eventArg != null)
            {
                return eventArg.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
    /// <summary>
    /// TCP连接服务器端,接受多客户的TCP连接
    /// </summary>
    public class TcpService<T>
         where T : class, IDataTransmit, new()
    {
        #region 事件定义

        /// <summary>
        /// 客户端连接事件
        /// </summary>
        public event NetEventHandler Connected;
        /// <summary>
        /// 客户端断开事件
        /// </summary>
        public event NetEventHandler DisConnect;
        #endregion

        #region 字段
        private readonly int maxsockets;            //最大客户连接数
        private int backlog;                        //最大挂起连接数
        private int port;                           //监听端口
        private TcpListener listener;               //监听类
        private Dictionary<EndPoint, T> session;    //保存连接的客户端
        private bool active = false;                //是否处于活动状态
        #endregion

        #region 属性
        /// <summary>
        /// 是否处于活动状态，即自动接收客户端连接
        /// </summary>
        public bool Active
        {
            get { return this.active; }
        }
        /// <summary>
        /// 获取监听端口号
        /// </summary>
        public int ListenPort
        {
            get { return this.port; }
        }
        /// <summary>
        /// 获取当前客户连接数
        /// </summary>
        public int ConnectCount
        {
            get { return session.Count; }
        }

        /// <summary>
        /// 获取与客户连接的所有Socket
        /// </summary>
        public Dictionary<EndPoint, T> Session
        {
            get { return session; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 使用指定端口、最大客户连接数、IP地址构造实例
        /// </summary>
        /// <param name="port">监听的端口号</param>
        /// <param name="maxsockets">最大客户连接量</param>
        /// <param name="ip">IP地址</param>
        public TcpService(int port, int maxsockets, string ip)
        {
            if (maxsockets < 1)
            {
                throw new ArgumentOutOfRangeException("maxsockets", "最大连接数不能小于1");
            }
            this.port = port;
            this.maxsockets = maxsockets;
            this.listener = new TcpListener(new IPEndPoint(IPAddress.Parse(ip), port));
            this.session = new Dictionary<EndPoint, T>();
        }

        /// <summary>
        /// 使用指定端口构造实例
        /// </summary>
        /// <param name="port">监听的端口</param>
        public TcpService(int port)
            : this(port, 1000, "0.0.0.0")
        {
        }
        #endregion

        #region 公用方法
        /// <summary>
        /// 启动服务器程序,开始监听客户端请求
        /// </summary>
        /// <param name="backlog">挂起连接队列的最大长度。</param>
        public void Start(int backlog)
        {
            this.backlog = backlog;
            listener.Start(backlog);
            //监听客户端连接请求 
            listener.BeginAcceptSocket(clientConnect, listener);
            this.active = true;
        }

        /// <summary> 
        /// 启动服务器程序,开始监听客户端请求
        /// </summary> 
        public void Start()
        {
            Start(10);
        }
        /// <summary>
        /// 关闭侦听器。
        /// </summary>
        public void Stop()
        {
            listener.Stop();
            this.active = false;
        }
        /// <summary>
        /// 断开所有客户端连接
        /// </summary>
        public void DisConnectAll()
        {
            foreach (KeyValuePair<EndPoint, T> kvp in this.session)
            {
                kvp.Value.DisConnected -= new NetEventHandler(work_DisConnect);
                kvp.Value.Stop();
                //触发客户断开事件
                NetEventHandler handler = DisConnect;
                if (handler != null)
                {
                    handler(kvp.Value, new NetEventArgs(new SocketException((int)SocketError.Success)));
                }
            }
            this.session.Clear();
        }
        /// <summary>
        /// 关闭侦听器并断开所有客户端连接
        /// </summary>
        public void Close()
        {
            this.Stop();
            this.DisConnectAll();
        }

        private void clientConnect(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            //接受客户的连接,得连接Socket
            try
            {
                Socket client = listener.EndAcceptSocket(ar);
                client.IOControl(IOControlCode.KeepAliveValues, Keepalive(0, 60000, 5000), null);

                T work = new T();
                work.TcpSocket = client;
                work.DisConnected += new NetEventHandler(work_DisConnect);

                EndPoint socketPoint = client.RemoteEndPoint;
                if (session.ContainsKey(socketPoint))
                {
                    session[socketPoint] = work;
                }
                else
                {
                    session.Add(socketPoint, work);
                }

                if (ConnectCount < maxsockets)
                {
                    //继续监听客户端连接请求 
                    IAsyncResult iar = listener.BeginAcceptSocket(clientConnect, listener);
                }
                else
                {   //达到最大连接客户数,则关闭监听.
                    listener.Stop();
                    this.active = false;
                }

                //客户端连接成功事件
                NetEventHandler handler = Connected;
                if (handler != null)
                {
                    handler(work, new NetEventArgs("接受客户的连接请求"));
                }
                Debug.WriteLine(socketPoint.ToString() + " is Connection...Num" + ConnectCount);
            }
            catch
            {
            }
        }

        //客户端断开连接
        private void work_DisConnect(IDataTransmit work, NetEventArgs e)
        {
            EndPoint socketPoint = work.RemoteEndPoint;
            session.Remove(socketPoint);

            //如果已关闭侦听器,则打开,继续监听
            if (ConnectCount == maxsockets)
            {
                listener.Start(this.backlog);
                IAsyncResult iar = listener.BeginAcceptSocket(clientConnect, listener);
                this.active = true;
            }

            //触发客户断开事件
            NetEventHandler handler = DisConnect;
            if (handler != null)
            {
                handler(work, e);
            }
            Debug.WriteLine(socketPoint.ToString() + " is OnDisConnected...Num" + ConnectCount);
        }
        #endregion

        /// <summary>
        ///  得到tcp_keepalive结构值
        /// </summary>
        /// <param name="onoff">是否启用Keep-Alive</param>
        /// <param name="keepalivetime">多长时间后开始第一次探测（单位：毫秒）</param>
        /// <param name="keepaliveinterval">探测时间间隔（单位：毫秒）</param>
        /// <returns></returns>
        public static byte[] Keepalive(int onoff, int keepalivetime, int keepaliveinterval)
        {
            byte[] inOptionValues = new byte[12];
            BitConverter.GetBytes(onoff).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes(keepalivetime).CopyTo(inOptionValues, 4);
            BitConverter.GetBytes(keepaliveinterval).CopyTo(inOptionValues, 8);
            return inOptionValues;
        }
    }
    /// <summary>
    /// 传输对象接口
    /// </summary>
    public interface IDataTransmit
    {
        /// <summary>
        /// 是否已连接
        /// </summary>
        bool Connected { get; }
        /// <summary>
        /// 连接失败事件
        /// </summary>
        event NetEventHandler ConnectFail;
        /// <summary>
        /// 连接成功事件
        /// </summary>
        event NetEventHandler ConnectSucceed;
        /// <summary>
        /// 断开连接事件
        /// </summary>
        event NetEventHandler DisConnected;
        /// <summary>
        /// 接收到数据事件
        /// </summary>
        event NetEventHandler ReceiveData;
        /// <summary>
        /// 获取远程终结点
        /// </summary>
        System.Net.EndPoint RemoteEndPoint { get; }
        /// <summary>
        /// 发送二进制数据
        /// </summary>
        /// <param name="bin">二进制数据</param>
        /// <returns></returns>
        bool Send(byte[] bin);
        /// <summary>
        /// 发送文本
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <returns></returns>
        bool Send(string text);
        /// <summary>
        /// 开始接收数据
        /// </summary>
        void Start();
        /// <summary>
        /// 停止并断开连接
        /// </summary>
        void Stop();
        /// <summary>
        /// Socket对象.
        /// </summary>
        System.Net.Sockets.Socket TcpSocket { get; set; }
    }
    /// <summary>
    /// 辅助传输对象
    /// </summary>
    public class DataTransmit : IDataTransmit
    {
        #region 事件定义
        /// <summary>
        /// 连接成功事件
        /// </summary>
        public event NetEventHandler ConnectSucceed;
        /// <summary>
        /// 连接失败事件
        /// </summary>
        public event NetEventHandler ConnectFail;
        /// <summary>
        /// 断开连接事件
        /// </summary>
        public event NetEventHandler DisConnected;
        /// <summary>
        /// 接收到数据事件
        /// </summary>
        public event NetEventHandler ReceiveData;
        #endregion

        #region 字段
        private Socket socket;                  //连接的Socket
        private EndPoint iep;                   //网络终节点,用于标识不同的用户
        private byte[] buffer;                  //接收数据缓存
        private SocketError errorCode;          //错误代码
        /// <summary>
        /// 缓存大小
        /// </summary>
        public const int BagSize = 8192;        //缓存大小8K
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置 Socket 对象
        /// </summary>
        public Socket TcpSocket
        {
            get
            {
                return socket;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("client");
                }
                this.socket = value;
                this.socket.ReceiveBufferSize = BagSize;
                this.iep = value.RemoteEndPoint;
            }
        }
        /// <summary>
        /// 获取远程终结点
        /// </summary>
        public EndPoint RemoteEndPoint
        {
            get { return iep; }
        }
        /// <summary>
        /// Socket是否已连接
        /// </summary>
        public bool Connected
        {
            get
            {
                if (socket == null)
                {
                    return false;
                }
                else
                {
                    return this.socket.Connected;
                }
            }
        }
        /// <summary>
        /// Socket错误代码
        /// </summary>
        public SocketError ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DataTransmit()
        {
            errorCode = SocketError.Success;
            buffer = new byte[BagSize];
        }
        /// <summary>
        /// 使用指定的IP地址和端口构造实例
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口</param>
        public DataTransmit(string ip, int port)
            : this(new IPEndPoint(IPAddress.Parse(ip), port))
        {
        }

        /// <summary>
        /// 客户端调用此构造函数
        /// </summary>
        /// <param name="ipEndPoint">在连接的服务器端网络地址</param>
        public DataTransmit(EndPoint ipEndPoint)
            : this()
        {
            iep = ipEndPoint;
        }

        /// <summary>
        /// 服务器端调用
        /// </summary>
        /// <param name="client">服务器监听连接得到的Socket对象</param>
        public DataTransmit(Socket client)
            : this()
        {
            TcpSocket = client;
        }
        #endregion

        /// <summary>
        /// 停止传输，断开连接
        /// </summary>
        public void Stop()
        {
            if (socket != null)
            {
                try
                {
                    if (socket.Connected)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Disconnect(false);
                        OnDisConnected(new SocketException((int)SocketError.Success));
                    }
                    socket.Close();
                }
                catch { }
                socket = null;
            }
        }

        /// <summary>
        /// 开始接收数据
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            if (socket != null && socket.Connected)
            {
                receiveData();
            }
            else
            {
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.socket.ReceiveBufferSize = BagSize;
                this.socket.BeginConnect(iep, connectCallback, socket);
            }
        }

        private void connectCallback(IAsyncResult ar)
        {
            try
            {
                this.socket.EndConnect(ar);
            }
            catch (Exception err)
            {
                OnConnectFail(err);
                return;
            }
            //连接成功,开始接收数据
            OnConnectSucceed();
            receiveData();
        }

        private void receiveData()
        {
            // 调用异步方法 BeginReceive 来告知 socket 如何接收数据
            if (socket != null && socket.Connected)
            {
                IAsyncResult iar = socket.BeginReceive(buffer, 0, BagSize, SocketFlags.None, out errorCode, receiveCallback, buffer);
                if ((errorCode != SocketError.Success) && (errorCode != SocketError.IOPending))
                {
                    OnDisConnected(new SocketException((int)errorCode));
                }
            }
        }

        /// <summary>
        /// 接收数据回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void receiveCallback(IAsyncResult ar)
        {
            if (socket == null || !socket.Connected) return;

            //接收到的数据长度．
            int receLen = 0;
            try
            {
                receLen = socket.EndReceive(ar, out errorCode);
            }
            catch (Exception err)
            {
                OnDisConnected(err);
                return;
            }
            if (errorCode == SocketError.Success)
            {
                if (receLen > 0)
                {
                    byte[] currentBin = new byte[receLen];
                    Buffer.BlockCopy(buffer, 0, currentBin, 0, receLen);
                    OnReceiveData(currentBin);
                    receiveData();
                }
                else
                {
                    OnDisConnected(new SocketException((int)SocketError.NotConnected));
                }
            }
            else
            {
                OnDisConnected(new SocketException((int)errorCode));
            }
        }

        /// <summary>
        /// 发送文本
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <returns></returns>
        public virtual bool Send(string text)
        {
            byte[] bin = Encoding.Default.GetBytes(text);
            return Send(bin);
        }

        /// <summary>
        /// 发送二进制数据
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns></returns>
        public virtual bool Send(byte[] data)
        {
            if (Connected)
            {
                this.socket.BeginSend(data, 0, data.Length, SocketFlags.None, out errorCode, sendCallBack, socket);
                if (errorCode == SocketError.Success)
                {
                    return true;
                }
            }
            return false;
        }

        private void sendCallBack(IAsyncResult ar)
        {
            if (socket == null) return;
            try
            {
                this.socket.EndSend(ar, out errorCode);
            }
            catch (Exception err)
            {
                OnDisConnected(err);
                return;
            }
            if (errorCode != SocketError.Success)
            {
                OnDisConnected(new SocketException((int)errorCode));
            }
        }

        #region 受保护的事件处理方法
        /// <summary>
        /// 触发连接成功事件
        /// </summary>
        protected virtual void OnConnectSucceed()
        {
            NetEventHandler hander = ConnectSucceed;
            if (hander != null)
            {
                ConnectSucceed(this, new NetEventArgs("成功连接到服务器"));
            }
        }

        /// <summary>
        /// 触发连接失败事件
        /// </summary>
        /// <param name="err"></param>
        protected virtual void OnConnectFail(Exception err)
        {
            NetEventHandler hander = ConnectFail;   //连接服务器失败事件
            if (hander != null)
            {
                ConnectFail(this, new NetEventArgs(err));
            }
        }

        /// <summary>
        /// 触发连接断开事件
        /// </summary>
        /// <param name="err"></param>
        protected virtual void OnDisConnected(Exception err)
        {
            //Stop();
            NetEventHandler hander = DisConnected;  //断开连接事件
            if (hander != null)
            {
                hander(this, new NetEventArgs(err));
            }
        }

        /// <summary>
        /// 触发接收数据事件
        /// </summary>
        /// <param name="bin"></param>
        protected virtual void OnReceiveData(object bin)
        {
            NetEventHandler hander = ReceiveData;   //接收到消息事件
            if (hander != null)
            {
                hander(this, new NetEventArgs(bin));
            }
        }
        #endregion
    }
    /// <summary>
    /// TCP连接服务器端,接受多客户的TCP连接
    /// </summary>
    public class TcpServiceLite : TcpService<DataTransmit>
    {
        #region 构造函数
        /// <summary>
        /// 使用指定端口、最大客户连接数、IP地址构造实例
        /// </summary>
        /// <param name="port">监听的端口号</param>
        /// <param name="maxsockets">最大客户连接量</param>
        /// <param name="ip">IP地址</param>
        public TcpServiceLite(int port, int maxsockets, string ip)
            : base(port, maxsockets, ip)
        {
        }
        /// <summary>
        /// 使用指定端口构造实例
        /// </summary>
        /// <param name="port">监听的端口</param>
        public TcpServiceLite(int port)
            : base(port, 1000, "0.0.0.0")
        {
        }
        #endregion
    }

}
