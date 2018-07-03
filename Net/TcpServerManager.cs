//create by xiaowy 2008-10-8

namespace Zdd.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// tcp���ӹ����࣬�Զ�Ϊ�ͻ��˽������Ӷ���
    /// �ݲ�֧��IP����
    /// </summary>
    public class TcpServerManager
    {
        #region ��Ա����

        private int port;
        private ITcpListener listener;
        private List<TcpServer> servers;

        #endregion 

        #region �����¼�

        /// <summary>
        /// �ͻ��������ѽ���ʱ����
        /// </summary>
        public event EventHandler<TcpServerEventArgs> ServerCreated;
        /// <summary>
        /// �ͻ��������Ƴ�����
        /// </summary>
        public event EventHandler<ServerRemovedEventArgs> ServerRemoved;
        /// <summary>
        /// �ͻ��������Ƴ�ǰ����
        /// </summary>
        public event EventHandler<TcpServerEventArgs> ServerRemoving;

        #endregion

        #region ���캯��

        /// <summary>
        /// tcp���ӹ���
        /// </summary>
        public TcpServerManager()
            : this(0)
        {
            servers = new List<TcpServer>();
        }

        /// <summary>
        /// tcp���ӹ���
        /// </summary>
        /// <param name="port">���ط���˿�</param>
        public TcpServerManager(int port)
        {
            this.port = port;
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡ�����ñ��ط���˿�
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// ��ȡ�����ӵĿͻ����б�
        /// </summary>
        public ReadOnlyCollection<TcpServer> Servers
        {
            get { return servers.AsReadOnly(); }
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns>�ɹ�����true,ʧ�ܷ���false</returns>
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
        /// ֹͣ����
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

        #region ˽�з���

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

        #region ��������

        /// <summary>
        /// �����ͻ����������¼�
        /// </summary>
        /// <param name="server">�����ӵĿͻ���</param>
        protected virtual void OnServerCreated(TcpServer server)
        {
            if (ServerCreated != null)
            {
                TcpServerEventArgs e = new TcpServerEventArgs(server);
                ServerCreated(this, e);
            }
        }

        /// <summary>
        /// �����ͻ������Ƴ��¼�
        /// </summary>
        /// <param name="index">�Ƴ�ǰ�ڿͻ����б��е�����</param>
        protected virtual void OnServerRemoved(int index)
        {
            if (ServerRemoved != null)
            {
                ServerRemoved(this, new ServerRemovedEventArgs(index));
            }
        }

        /// <summary>
        /// �����ͻ����Ƴ�ǰ�¼�
        /// </summary>
        /// <param name="server">��Ҫ��Ҳ�ƵĿͻ���</param>
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
