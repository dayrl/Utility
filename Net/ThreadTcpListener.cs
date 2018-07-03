//create by xiaowy 2008-10-8

namespace Zdd.Utility
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// tcp�����̣߳�ר�Ÿ�����������
    /// </summary>
    internal class ThreadTcpListener : ITcpListener
    {
        #region ��Ա����

        private bool isListening;
        private Socket listenSocket;
        private Thread listenThread;
        private int port;

        #endregion

        #region �����¼�

        /// <summary>
        /// �����µĿͻ�����������ʱ����
        /// </summary>
        public event EventHandler<ConnectedEventArgs> Connected;

        #endregion 

        #region ���캯��

        /// <summary>
        /// tcp�����̣߳�ר�Ÿ�����������
        /// </summary>
        /// <param name="port">���ط���˿�</param>
        public ThreadTcpListener(int port)
        {
            this.port = port;
           isListening = false;
           listenSocket = null;
           listenThread = null;
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡ�Ƿ����ڼ���
        /// </summary>
        public bool IsListening
        {
            get { return isListening; }
        }

        /// <summary>
        /// ��ȡ�����˿�
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns>�ɹ�����true,ʧ�ܷ���false</returns>
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
        /// �رռ���
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

        #region ˽�з���

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
