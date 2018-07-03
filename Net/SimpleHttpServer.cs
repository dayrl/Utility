using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System;
using System.Web;

namespace Zdd.Utility.Net
{
    public delegate void SimpleHttpServerNotify(object sender, string msg);
    public delegate bool AccessVerifyDelegate(Socket clinent, string accessFile);
    /// <summary>
    /// SimpleHttpServer
    /// </summary>
    public class SimpleHttpServer
    {
        /// <summary>
        /// 服务通知
        /// </summary>
        public event SimpleHttpServerNotify ServerNotifyEvent;
        /// <summary>
        /// 用于判断对文件的访问权限
        /// </summary>
        public event AccessVerifyDelegate AccessVerifyEvent;
        private void OnNotifyEvent(object sender, string msg)
        {
            if (null != ServerNotifyEvent)
            {
                ServerNotifyEvent.Invoke(sender, msg);
            }
        }
        private bool OnAccessVerify(Socket client, string fileName)
        {
            if (null != AccessVerifyEvent)
            {
                return AccessVerifyEvent.Invoke(client, fileName);
            }
            return true;
        }
        bool running;
        /// <summary>
        /// Server Running State
        /// </summary>
        public bool RuningState
        {
            get { return running; }
        }
        /// <summary>
        /// Data Transfer Timeout
        /// </summary>
        private const int TransTimeout = 10*1000;
        /// <summary>
        /// String Encoding
        /// </summary>
        private readonly Encoding dataEncoder = Encoding.UTF8;
        private Socket serverSocket;
        private string rootPath;
        /// <summary>
        /// 根目录
        /// </summary>
        public string RootPath
        {
            get { return rootPath; }
        }
        /// <summary>
        /// 根据后缀名返回MIME类型
        /// </summary>
        private Dictionary<string, string> mimeType = new Dictionary<string, string>()
        { 
            { ".htm", "text/html" },
            { ".html", "text/html" },
            { ".xml", "text/xml" },
            { ".txt", "text/plain" },
            { ".css", "text/css" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".jpg", "image/jpg" },
            { ".jpeg", "image/jpeg" },
            { ".ico", "image/x-icon" },
            { ".zip", "application/zip"},
            { ".flv","application/octet-stream"},
            { ".mp4","video/mp4"},//application/octet-stream
            { ".mp3","video/mp3"},
            { ".m3u8","application/octet-stream"},
            { ".ts","application/octet-stream"},
            { ".3gp","application/octet-stream"},
            { ".mov","application/octet-stream"},
            { ".avi","application/octet-stream"},
            { ".wmv","application/octet-stream"},
        };
        /// <summary>
        /// 启动SimpleHttpServer
        /// </summary>
        /// <param name="host">提供的访问IP地址</param>
        /// <param name="port">端口</param>
        /// <param name="backlog">最大连接数</param>
        /// <param name="root">根目录</param>
        /// <returns></returns>
        public bool Start(IPAddress host, int port, int backlog, string root)
        {
            if (running)
            {
                OnNotifyEvent(this, "Service is running.");
                return false;
            }
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(host, port));
                serverSocket.Listen(backlog);
                serverSocket.ReceiveTimeout = TransTimeout;
                serverSocket.SendTimeout = TransTimeout;
                running = true;
                this.rootPath = root;
            }
            catch(Exception ex)
            {
                OnNotifyEvent(this, "Service start exception: " + ex.Message);
                return false; 
            }

            Thread requestThread = new Thread(() =>
            {
                while (running)
                {
                    Socket client;
                    try
                    {
                        client = serverSocket.Accept();
                        Thread requestHandler = new Thread(() =>
                        {
                            client.ReceiveTimeout = TransTimeout;
                            client.SendTimeout = TransTimeout;
                            try 
                            { 
                                handleHttpRequest(client); 
                            }
                            catch(Exception ex)
                            {
                                OnNotifyEvent(this, "client exception: " + ex.Message);
                                try { client.Close(); }
                                catch { }
                            }
                        });
                        requestHandler.Start();
                    }
                    catch(Exception ex)
                    {
                        OnNotifyEvent(this, "server running exception: " + ex.Message);
                    }
                }
            });
            requestThread.Start();

            return true;
        }

        public void Stop()
        {
            if (running)
            {
                OnNotifyEvent(this, "server stopped.");
                running = false;
                try { serverSocket.Close(); }
                catch { }
                serverSocket = null;
            }
        }
        /// <summary>
        /// 处理HTTP请求
        /// </summary>
        /// <param name="client"></param>
        private void handleHttpRequest(Socket client)
        {
            if (null == client)
            {
                return;
            }
            byte[] buffer = new byte[10240]; //GET请求最大1024K,使用10K以防万一
            int rcvBuf = client.Receive(buffer); // Receive the request
            string strReceived = dataEncoder.GetString(buffer, 0, rcvBuf);
            string[] lines = strReceived.Split(new char[] { '\r', '\n' });
            OnNotifyEvent(this, string.Format("client [{0}] conncted [{1}]", client.RemoteEndPoint, lines[0]));
            //GET /abc HTTP/1.1
            if (lines.Length == 0)
            {
                badRequest(client);
                return;
            }
            string[] httpProtocolLine = lines[0].Split(' ');
            if (httpProtocolLine.Length != 3)
            {
                badRequest(client);
                return;
            }
            // Parse the method of the request
            string httpMethod = httpProtocolLine[0].ToUpperInvariant();//GET POST
            if (httpMethod != "GET")
            {
                notImplemented(client);
                return;
            }
            string protocol = httpProtocolLine[2];//协议HTTP/1.1
            string requestedUrl = httpProtocolLine[1];
            string requestedFile = requestedUrl.Split('?')[0];
            requestedFile = requestedFile.Replace("/", "\\").Replace("\\..", ""); //there must be some bad guy
            string requestedFileName = System.IO.Path.GetFileName(requestedFile);
            requestedFileName = UrlDecoder.UrlDecode(requestedFileName, dataEncoder);
            if (requestedUrl == "/")
            {
                welcome(client);
                return;
            }
            FileInfo fi = new FileInfo(Path.Combine(rootPath, requestedFileName));
            if (requestedUrl == "/favicon.ico")
            {
                if (!fi.Exists)
                {
                    fi = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, requestedFileName));
                }
            }
            //favicon.ico

            string fullPath = fi.FullName;
            string fileExt = fi.Extension.ToLowerInvariant();
            if (!httpMethod.Equals("GET"))/*GET only*/
            {
                notImplemented(client);
                return;
            }

            if (!mimeType.ContainsKey(fileExt))
            {
                badRequest(client);//invalid mime type request
                return;
            }
            //check 
            if (!fi.Exists)
            {
                notFound(client);//invalid right
                return;
            }

            if (!OnAccessVerify(client, fullPath))
            {
                forbidden(client);
            }
            else
            {
                sendOk(client, File.ReadAllBytes(fi.FullName), mimeType[fileExt]);
            }
            return;
        }
        /// <summary>
        /// welcome
        /// </summary>
        /// <param name="client"></param>
        private void welcome(Socket client)
        {
            sendResponse(client,
                ("<html>" +
                "<head>" +
                "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                "</head>" +
                "<body>" +
                "<h2>Simple Web Server</h2>" +
                "<div>Welcome!</div>" +
                "</body>" +
                 "</html>"),
                "200 OK",
                "text/html");
        }
        /// <summary>
        /// 403 Forbidden 
        /// </summary>
        /// <param name="client"></param>
        private void forbidden(Socket client)
        {
            sendResponse(client,
                ("<html>" +
                "<head>" +
                "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                "</head>" +
                "<body>" +
                "<h2>Simple Web Server</h2>" +
                "<div>403 -Forbidden, access denied!</div>" +
                "</body>" +
                 "</html>"),
                "403 Forbidden",
                "text/html");
        }
        /// <summary>
        /// 501 Not Implemented
        /// </summary>
        /// <param name="client"></param>
        private void notImplemented(Socket client)
        {
            sendResponse(client,
                ("<html>" +
                "<head>" +
                "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                "</head>" +
                "<body>" +
                "<h2>Simple Web Server</h2>" +
                "<div>501 Method Not Implemented</div>" +
                "</body>" +
                 "</html>"),
                "501 NotImplemented",
                "text/html");
        }
        /// <summary>
        /// send bad request
        /// </summary>
        /// <param name="client"></param>
        private void badRequest(Socket client)
        {
            sendResponse(client, 
                ("<html>" +
                "<head>"+
                "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                "</head>"+
                "<body>"+
                "<h2>Simple Web Server</h2>"+
                "<div>400 - Bad Request</div>" +
                "</body>"+
                 "</html>"),
                "400 BadRequest", 
                "text/html");
        }
        /// <summary>
        /// 404
        /// </summary>
        /// <param name="client"></param>
        private void notFound(Socket client)
        {
            sendResponse(client,
                ("<html>" +
                "<head>" +
                "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">" +
                "</head>" +
                "<body>" +
                "<h2>Simple Web Server</h2>" +
                "<div>404 - You Know That!</div>" +
                "</body>" +
                 "</html>"),
                "400 NotFound ",
                "text/html");
        }
        /// <summary>
        /// sendOk
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bContent"></param>
        /// <param name="contentType"></param>
        private void sendOk(Socket client, byte[] bContent, string contentType)
        {
            sendResponse(client, bContent, "200 OK", contentType);
        }

        /// <summary>
        /// strings response
        /// </summary>
        /// <param name="client"></param>
        /// <param name="strContent"></param>
        /// <param name="responseCode"></param>
        /// <param name="contentType"></param>
        private void sendResponse(Socket client, string strContent, string responseCode, string contentType)
        {
            byte[] bContent = dataEncoder.GetBytes(strContent);
            sendResponse(client, bContent, responseCode, contentType);
        }

        /// <summary>
        /// byte response
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bContent"></param>
        /// <param name="responseCode"></param>
        /// <param name="contentType"></param>
        private void sendResponse(Socket client, byte[] bContent, string responseCode, string contentType)
        {
            try
            {
                byte[] bHeader = dataEncoder.GetBytes(
                                    "HTTP/1.1 " + responseCode + "\r\n"
                                  + "Server: Simple Web Server\r\n"
                                  + "Content-Length: " + bContent.Length.ToString() + "\r\n"
                                  + "Connection: close\r\n"
                                  + "Content-Type: " + contentType + "; charset=gb2312\r\n\r\n");
                client.Send(bHeader);
                client.Send(bContent);
                client.Close();
            }
            catch(Exception ex)
            {
                OnNotifyEvent(this, "Response Exception: " + ex.Message);
            }
        }
    }
}