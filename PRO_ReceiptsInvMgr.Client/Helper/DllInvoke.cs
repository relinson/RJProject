using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PRO_ReceiptsInvMgr.Client.Helper
{

    public class DllInvoke

    {

        [DllImport("kernel32.dll")]

        private extern static IntPtr LoadLibrary(String path);



        [DllImport("kernel32.dll")]

        private extern static IntPtr GetProcAddress(IntPtr lib, String funcName);



        [DllImport("kernel32.dll")]

        private extern static bool FreeLibrary(IntPtr lib);



        private readonly IntPtr hLib;


        public DllInvoke(String DLLPath)
        {

            hLib = LoadLibrary(DLLPath);
        }



        ~DllInvoke()

        {

            FreeLibrary(hLib);

        }

   
        //将要执行的函数转换为委托  

        public Delegate Invoke(String APIName, Type t)

        {

            IntPtr api = GetProcAddress(hLib, APIName);

            return Marshal.GetDelegateForFunctionPointer(api, t);

        }

    }
}
