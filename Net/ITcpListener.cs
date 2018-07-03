//create by xiaowy 2008-10-8

namespace Zdd.Utility
{
    using System;

    /// <summary>
    /// tcp���Ӽ����ӿ�
    /// </summary>
    internal interface ITcpListener
    {
        /// <summary>
        /// ���µĿͻ�����������ʱ����
        /// </summary>
        event EventHandler<ConnectedEventArgs> Connected;
        /// <summary>
        /// �رռ���
        /// </summary>
        void Close();
        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns>�ɹ�����true��ʧ�ܷ���false</returns>
        bool Start();
        /// <summary>
        /// ��ȡ�Ƿ����ڼ���
        /// </summary>
        bool IsListening { get; }
        /// <summary>
        /// ��ȡ�����˿�
        /// </summary>
        int Port { get; set; }
    }

}
