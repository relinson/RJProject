using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PRO_ReceiptsInvMgr.Client.Helper
{
    public class ProcessHelper
    {
        private Process myProcess = new Process();

        public void SetExitHandler(EventHandler eventHandler)
        {
            MyProcess.Exited += eventHandler;
        }

        public Process MyProcess
        {
            get
            {
                return myProcess;
            }
            set
            {
                myProcess = value;
            }
        }
        /// <summary>
        /// 打开新进程
        /// </summary>
        /// <param name="fileName"></param>
        public void PrintDoc(string fileName, string argument = "")
        {
            MyProcess.StartInfo.FileName = fileName;
            if (!string.IsNullOrEmpty(argument))
            {
                MyProcess.StartInfo.Arguments = argument;
            }
            
            MyProcess.EnableRaisingEvents = true;
            MyProcess.Start();
            MyProcess.WaitForExit();
           
            while (!MyProcess.HasExited)
            {
                Thread.Sleep(1);
            }
        }
    }
}
