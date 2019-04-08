using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Component;
using PRO_ReceiptsInvMgr.Application.Global;
using System.IO;
using PRO_ReceiptsInvMgr.Client.UI;
using PRO_ReceiptsInvMgr.Domain.Enum;
using PRO_ReceiptsInvMgr.Resources;
using PRO_ReceiptsInvMgr.Core.Utilites;
using log4net;
using log4net.Appender;
using PRO_ReceiptsInvMgr.Client;
using PRO_ReceiptsInvMgr.Client.UI.FPCX;
using System.Management;
using Microsoft.Win32;

namespace PRO_ReceiptsInvMgr.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        [DllImport("User32.dll")]
        private static extern void SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 根据句柄查找进程ID
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        AppService appService = new AppService();

        /// 税控盘名称
        /// </summary>
        private string SKPName = PRO_ReceiptsInvMgr.Resources.Common.SKPDeviceName;

        /// <summary>
        /// 金税盘名称
        /// </summary>
        private string JSPName = PRO_ReceiptsInvMgr.Resources.Common.JSPDeviceName;
        /// <summary>
        /// 税控盘插件
        /// </summary>
        private string skpTool = PRO_ReceiptsInvMgr.Resources.Common.SKPTool;

        /// <summary>
        /// 金控盘插件
        /// </summary>
        private string jspTool = PRO_ReceiptsInvMgr.Resources.Common.JSPTool;


        /// <summary>
        ///  软件启动初始化
        /// </summary>
        public App()
        {

            Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            System.Windows.Application.Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;

            Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;
        }


        /// <summary>
        /// 在异常由应用程序引发但未进行处理时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var comException = e.Exception as COMException;
            if (comException != null)
            {
                e.Handled = true;

                return;
            }
            e.Handled = true;
        }

        /// <summary>
        /// 检查当前接入的开票设备
        /// </summary>
        private void CheckInvoiceDevice()
        {
            try
            {
                List<string> diskModel = new List<string>();
                ManagementObjectSearcher mosDisks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (var moDisk in mosDisks.Get())
                {
                    if (moDisk["Model"] != null)
                    {
                        diskModel.Add(moDisk["Model"].ToString());
                    }
                }

                var deviceEntities = diskModel.Where(x => x == JSPName || x == SKPName).ToList();

                if (deviceEntities.Count == 0)
                {
                    var dialogResult = MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.NotFindTax, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                    System.Windows.Application.Current.Shutdown();
                }
                else if (deviceEntities.Count > 1)
                {
                    MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.MultiTaxDiskError, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                    System.Windows.Application.Current.Shutdown();
                }
                else
                {
                    if (deviceEntities.FirstOrDefault() == JSPName)
                    {
                        GlobalInfo.DeviceType = DeviceType.JSP.GetHashCode();
                    }
                    else
                    {
                        GlobalInfo.DeviceType = DeviceType.SKP.GetHashCode();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.CheckTaxDeviceError, ex);
            }
        }

        /// <summary>
        /// 启动程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                CheckMultiRunning();

                CheckInvoiceDevice();

                var dbFileName = ConfigHelper.GetAppSettingValue("DbFileName");
                string filePath = AppDomain.CurrentDomain.BaseDirectory + dbFileName;

                var isInit = false;
                if (!File.Exists(filePath))
                {
                    isInit = true;
                }
                else
                {
                    if (!appService.SyncServerDataBase())
                    {
                        MessageBoxEx.Show(PRO_ReceiptsInvMgr.Resources.Message.SyncDataBaseFail, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                        Environment.Exit(1);
                    }

                    if (GlobalInfo.DeviceType == DeviceType.JSP.GetHashCode())
                    {
                        if (!CheckIsInstall(jspTool))
                        {
                            isInit = true;
                        }
                    }
                    else
                    {
                        if (!CheckIsInstall(skpTool))
                        {
                            isInit = true;
                        }
                    }
                }


                if (isInit)
                {
                    Start win = new Start();
                    win.Show();
                }
                else
                {
                    Login win = new Login();
                    win.Show();
                }
               
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, Message.StartError, ex);
            }
        }


        /// <summary>
        /// 检查注册表项判断是否安装
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static bool CheckIsInstall(string node)
        {
            var result = false;

            using (RegistryKey ndpKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\"))
            {
                foreach (var name in ndpKey.GetSubKeyNames())
                {
                    if (name.Equals(node))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }


        private void CheckMultiRunning()
        {
            Process p = null;
            //限制程序不能打开多次
            if (ExistRunningInstance(ref p))
            {
                IntPtr hWnd = p.MainWindowHandle;
                if (hWnd.ToInt32() == 0)
                {

                    var hnd = FindWindow(null, "久易-进项票管家");

                    int id = -1;
                    GetWindowThreadProcessId(hnd, out id);
                    if (id == p.Id)
                    {
                        hWnd = hnd;
                    }
                }

                ShowWindow(hWnd, 2);
                ShowWindow(hWnd, 1);
                Environment.Exit(1);
            }
        }


        /// <summary>
        /// 程序是否已运行
        /// </summary>
        /// <param name="p"></param>
        /// <returns>true：已运行,false：未运行</returns>
        private static bool ExistRunningInstance(ref Process p)
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] procList = Process.GetProcessesByName(currentProcess.ProcessName);

            foreach (Process proc in procList)
            {
                if (proc.Id != currentProcess.Id)
                {
                    p = proc;

                    return true;
                }
            }

            return false;
        }
    }
}
