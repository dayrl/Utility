namespace Zdd.Utility
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// ASPHelper
    /// </summary>
    public class ASPHelper
    {
        private static Guid IID_IObjectContext = new Guid("51372ae0-cae7-11cf-be81-00aa00a2fa25");

        /// <summary>
        /// Initializes a new instance of the <see cref="ASPHelper"/> class.
        /// </summary>
        private ASPHelper()
        {}

        /// <summary>
        /// Coes the get object context.
        /// </summary>
        /// <param name="iid">The iid.</param>
        /// <param name="g">The g.</param>
        /// <returns></returns>
        [DllImport("ole32.dll")]
        private static extern int CoGetObjectContext(ref Guid iid, out IObjectContext g);
        public static IApplicationObject GetApplicationObject()
        {
            IApplicationObject property = null;
            IObjectContext context;
            if (CoGetObjectContext(ref IID_IObjectContext, out context) == 0)
            {
                IGetContextProperties o = (IGetContextProperties)context;
                if (o != null)
                {
                    property = (IApplicationObject)o.GetProperty("Application");
                    Marshal.ReleaseComObject(o);
                }
                Marshal.ReleaseComObject(context);
            }
            return property;
        }

        /// <summary>
        /// Gets the COM default property.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static object GetComDefaultProperty(object o)
        {
            if (o == null)
            {
                return null;
            }
            return o.GetType().InvokeMember("", BindingFlags.GetProperty, null, o, new object[0]);
        }

        /// <summary>
        /// Gets the request object.
        /// </summary>
        /// <returns></returns>
        public static IRequest GetRequestObject()
        {
            IRequest property = null;
            IObjectContext context;
            if (CoGetObjectContext(ref IID_IObjectContext, out context) == 0)
            {
                IGetContextProperties o = (IGetContextProperties)context;
                if (o != null)
                {
                    property = (IRequest)o.GetProperty("Request");
                    Marshal.ReleaseComObject(o);
                }
                Marshal.ReleaseComObject(context);
            }
            return property;
        }

        /// <summary>
        /// Gets the response object.
        /// </summary>
        /// <returns></returns>
        public static IResponse GetResponseObject()
        {
            IResponse property = null;
            IObjectContext context;
            if (CoGetObjectContext(ref IID_IObjectContext, out context) == 0)
            {
                IGetContextProperties o = (IGetContextProperties)context;
                if (o != null)
                {
                    property = (IResponse)o.GetProperty("Response");
                    Marshal.ReleaseComObject(o);
                }
                Marshal.ReleaseComObject(context);
            }
            return property;
        }

        /// <summary>
        /// Gets the session object.
        /// </summary>
        /// <returns></returns>
        public static ISessionObject GetSessionObject()
        {
            ISessionObject property = null;
            IObjectContext context;
            if (CoGetObjectContext(ref IID_IObjectContext, out context) == 0)
            {
                IGetContextProperties o = (IGetContextProperties)context;
                if (o != null)
                {
                    property = (ISessionObject)o.GetProperty("Session");
                    Marshal.ReleaseComObject(o);
                }
                Marshal.ReleaseComObject(context);
            }
            return property;
        }

        /// <summary>
        /// IApplicationObject
        /// </summary>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("D97A6DA0-A866-11cf-83AE-10A0C90C2BD8")]
        public interface IApplicationObject
        {
            object GetValue(string name);
            void PutValue(string name, object val);
        }

        /// <summary>
        /// IDispatch
        /// </summary>
        [ComImport, Guid("00020400-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IDispatch
        {
        }

        /// <summary>
        /// IGetContextProperties
        /// </summary>
        [ComImport, Guid("51372af4-cae7-11cf-be81-00aa00a2fa25"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IGetContextProperties
        {
            int Count();
            object GetProperty(string name);
        }

        /// <summary>
        /// IObjectContext
        /// </summary>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("51372ae0-cae7-11cf-be81-00aa00a2fa25")]
        public interface IObjectContext
        {
        }

        /// <summary>
        /// IReadCookie
        /// </summary>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("71EAF260-0CE0-11D0-A53E-00A0C90C2091")]
        public interface IReadCookie
        {
            void GetItem(object key, out object val);
            object HasKeys();
            void GetNewEnum();
            void GetCount(out int count);
            object GetKey(object key);
        }

        /// <summary>
        /// IRequest
        /// </summary>
        [ComImport, Guid("D97A6DA0-A861-11cf-93AE-00A0C90C2BD8"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
        public interface IRequest
        {
            ASPHelper.IDispatch GetItem(string name);
            ASPHelper.IRequestDictionary GetQueryString();
            ASPHelper.IRequestDictionary GetForm();
            ASPHelper.IRequestDictionary GetBody();
            ASPHelper.IRequestDictionary GetServerVariables();
            ASPHelper.IRequestDictionary GetClientCertificates();
            ASPHelper.IRequestDictionary GetCookies();
            int GetTotalBytes();
            void BinaryRead();
        }

        /// <summary>
        /// IRequestDictionary
        /// </summary>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("D97A6DA0-A85F-11df-83AE-00A0C90C2BD8")]
        public interface IRequestDictionary
        {
            object GetItem(object var);
            object NewEnum();
            int GetCount();
            object Key(object varKey);
        }

        /// <summary>
        /// IResponse
        /// </summary>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("D97A6DA0-A864-11cf-83BE-00A0C90C2BD8")]
        public interface IResponse
        {
            void GetBuffer();
            void PutBuffer();
            void GetContentType();
            void PutContentType();
            void GetExpires();
            void PutExpires();
            void GetExpiresAbsolute();
            void PutExpiresAbsolute();
            void GetCookies();
            void GetStatus();
            void PutStatus();
            void Add();
            void AddHeader();
            void AppendToLog();
            void BinaryWrite();
            void Clear();
            void End();
            void Flush();
            void Redirect();
            void Write(object text);
        }

        /// <summary>
        /// ISessionObject
        /// </summary>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("D97A6DA0-A865-11cf-83AF-00A0C90C2BD8")]
        public interface ISessionObject
        {
            string GetSessionID();
            object GetValue(string name);
            void PutValue(string name, object val);
            int GetTimeout();
            void PutTimeout(int t);
            void Abandon();
            int GetCodePage();
            void PutCodePage(int cp);
            int GetLCID();
            void PutLCID();
        }

        /// <summary>
        /// IStringList
        /// </summary>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("D97A6DA0-A85D-11cf-83AE-00A0C90C2BD8")]
        public interface IStringList
        {
            object GetItem(object key);
            int GetCount();
            object NewEnum();
        }
    }
}
