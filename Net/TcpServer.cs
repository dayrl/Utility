//create by xiaowy 2008-10-8

namespace Zdd.Utility
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// ����һ��tcp���ӿͻ��� 
    /// </summary>
    public class TcpServer : IDisposable
    {
        #region ��Ա����

        private DateTime connectedTime;
        private DateTime lastReceiveTime;
        private int port;
        private byte[] receiveBuffer;
        private IPAddress remoteIP;
        private Socket socket;
        private int transfromRate;

        #endregion

        #region �����¼�

        /// <summary>
        /// �������ӶϿ���ʱ����
        /// </summary>
        public event EventHandler Disconnected;
        /// <summary>
        /// ���ݵ���ʱ����
        /// </summary>
        public event EventHandler<DataArrivedEventArgs> DataArrived;
        /// <summary>
        /// ���ݷ������ʱ����
        /// </summary>
        public event EventHandler<SendCompleteEventArgs> SendComplete;

        #endregion

        #region ���캯��

        /// <summary>
        /// ����һ��tcp���ӿͻ���
        /// </summary>
        /// <param name="socket">�׽���</param>
        public TcpServer(Socket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket", "�׽��ֲ���Ϊ�ա�");
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

        #region ��������

        /// <summary>
        /// ��ȡ����ʱ�� 
        /// </summary>
        public DateTime ConnectedTime
        {
            get { return connectedTime; }
        }

        /// <summary>
        /// ��ȡԶ��IPAddress
        /// </summary>
        public IPAddress IPAddress
        {
            get { return remoteIP; }
        }

        /// <summary>
        /// ��ȡ����״̬
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
        /// ��ȡ���Ӷ˿�
        /// </summary>
        public int Port
        {
            get { return  port; }
        }

        /// <summary>
        /// ��ȡ���ݴ�������
        /// </summary>
        public int TransfromRate
        {
            get { return transfromRate; }
        }

        #endregion

        #region ��������

        /// <summary>
        /// �첽��������
        /// </summary>
        /// <param name="data">Ҫ���͵�����</param>
        /// <returns>�ɹ�����true,ʧ�ܷ���false</returns>
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
        /// �ͷ���Դ
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
        /// �Ͽ�����
        /// </summary>
        protected virtual void OnDisconnected()
        {
            if (Disconnected != null)
            {
               Disconnected(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// �������
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

        #region ˽�з���

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

        #region ��������

        /// <summary>
        /// �Ͽ���������
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
        /// �ͷ���Դ
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        #endregion
    }

    #region �¼�����

    /// <summary>
    /// ���ݵ����¼�����
    /// </summary>
    public class DataArrivedEventArgs : EventArgs
    {
        private readonly byte[] data;

        /// <summary>
        /// ���ݵ����¼�����
        /// </summary>
        /// <param name="data">����</param>
        public DataArrivedEventArgs(byte[] data)
        {
            this.data = data;
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        public byte[] Data
        {
            get { return data; }
        }
    }

    /// <summary>
    /// ���������¼����� 
    /// </summary>
    public class TcpServerEventArgs : EventArgs
    {
        private readonly TcpServer server;

        /// <summary>
        /// ���������¼�����
        /// </summary>
        /// <param name="server">tcp�ͻ��˶���</param>
        public TcpServerEventArgs(TcpServer server)
        {
            this.server = server;
        }

        /// <summary>
        /// ��ȡһ��tcp�ͻ��˶���
        /// </summary>
        public TcpServer Server
        {
            get { return server; }
        }
    }

    /// <summary>
    /// ���ݷ�������¼����� 
    /// </summary>
    public class SendCompleteEventArgs : EventArgs
    {
        private readonly object key;
        private readonly bool success;

        /// <summary>
        /// ���ݷ�������¼�����
        /// </summary>
        public SendCompleteEventArgs(bool result, object key)
        {
            success = result;
            this.key = key;
        }

        /// <summary>
        /// ��ȡ�Զ���Ĳ���
        /// </summary>
        public object Key
        {
            get { return key; }
        }

        /// <summary>
        /// ��ȡ�Ƿ��ͳɹ����ɹ�Ϊtrue
        /// </summary>
        public bool Success
        {
            get { return success; }
        }
    }

    /// <summary>
    /// �ͻ����������Ƴ��¼����� 
    /// </summary>
    public class ServerRemovedEventArgs : EventArgs
    {
        private readonly int index;

        /// <summary>
        /// �ͻ����������Ƴ��¼�����
        /// </summary>
        /// <param name="index">�ͻ����ڿͻ����б��е�����</param>
        public ServerRemovedEventArgs(int index)
        {
            this.index = index;
        }

        /// <summary>
        /// ��ȡ�ͻ������б��е�����
        /// </summary>
        public int Index
        {
            get { return index; }
        }
    }

    /// <summary>
    /// �ͻ����������¼����� 
    /// </summary>
    public class ConnectedEventArgs : EventArgs
    {
        private readonly Socket socket;
        /// <summary>
        /// �ͻ����������¼�����
        /// </summary>
        /// <param name="socket">�׽���</param>
        public ConnectedEventArgs(Socket socket)
        {
            this.socket = socket;
        }

        /// <summary>
        /// ��ȡ�׽���
        /// </summary>
        public Socket Socket
        {
            get { return socket; }
        }
    }

    #endregion
}

