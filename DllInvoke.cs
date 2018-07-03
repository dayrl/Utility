namespace Zdd.Utility
{
    using System;
    using System.Runtime.InteropServices;

    public class DllInvoke
    {
        #region Windows API Invoke
       [DllImport("kernel32.dll")] 
　　 private extern static IntPtr LoadLibrary(string path); 
　　[DllImport("kernel32.dll")]
        private extern static IntPtr GetProcAddress(IntPtr lib, string funcName);
　　[DllImport("kernel32.dll")]
        private extern static bool FreeLibrary(IntPtr lib);
        #endregion

        #region DllInvoke private Para
        /// <summary>
        /// DLL模块句柄
        /// </summary>
        private IntPtr hLib;

        /// <summary>
        /// DLL绝对路径
        /// </summary>
        private readonly string dllPath;
        #endregion

        #region DllInvoke Attribute
        public string DllPath
        {
            get { return dllPath; }
        }
        #endregion

        #region DllInvoke method
        public DllInvoke(string _dllPath) 
　　 {
              dllPath = _dllPath;
              hLib = LoadLibrary(dllPath);
　　 }

        ~DllInvoke()
        {
            FreeLibrary(hLib);
        }

        //将要执行的函数转换为委托 
　　 public Delegate Invoke(string APIName, Type t) 
　　 { 
　　    IntPtr api = GetProcAddress(hLib, APIName);
             return Marshal.GetDelegateForFunctionPointer(api, t); 
　　 } 
        #endregion

    }
}
