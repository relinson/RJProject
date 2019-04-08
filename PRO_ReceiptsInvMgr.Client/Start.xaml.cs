using Microsoft.Win32;
using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Client.Helper;
using PRO_ReceiptsInvMgr.Client.UI;
using PRO_ReceiptsInvMgr.Core.Utilites;
using PRO_ReceiptsInvMgr.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRO_ReceiptsInvMgr.Client
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 税控盘插件
        /// </summary>
        private string skpTool = PRO_ReceiptsInvMgr.Resources.Common.SKPTool;

        /// <summary>
        /// 金控盘插件
        /// </summary>
        private string jspTool = PRO_ReceiptsInvMgr.Resources.Common.JSPTool;

        AppService appService = new AppService();



        private void Start_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Init();
            });

        }

        private void Init()
        {
            var dbFileName = ConfigHelper.GetAppSettingValue("DbFileName");
            string filePath = AppDomain.CurrentDomain.BaseDirectory + dbFileName;

            if (!File.Exists(filePath))
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    tbTip.Text = "初始化数据库...";
                }));
                if (!appService.InitializeDataBase())
                {
                    MessageBoxEx.Show(PRO_ReceiptsInvMgr.Resources.Message.DataFail, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                    System.Windows.Application.Current.Shutdown();
                }
            }


            this.Dispatcher.Invoke(new Action(() =>
            {
                tbTip.Text = "检测并安装相关驱动...";
            }));

            
            if (!CheckIsInstall(skpTool) && !CheckIsInstall(jspTool))
            {
                string filebase = AppDomain.CurrentDomain.BaseDirectory + "Driver\\base_driver.exe";
                //SilentInstall(filebase);
                ProcessHelper proc = new ProcessHelper();
                proc.PrintDoc(filebase, "");
            }

            if (GlobalInfo.DeviceType == DeviceType.JSP.GetHashCode())
            {
                if (!CheckIsInstall(jspTool))
                {
                    string filename = AppDomain.CurrentDomain.BaseDirectory + "Driver\\jspDriver1.1.1.12.exe";
                    SilentInstall(filename);
                }
                else
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        Login win = new Login();
                        win.Show();
                        this.Close();
                    }));
                }
            }
            else
            {
                if (!CheckIsInstall(skpTool))
                {
                    string filename = AppDomain.CurrentDomain.BaseDirectory + "Driver\\nisecinstaller_V1.0.8.7.1.exe";
                    SilentInstall(filename);
                }
                else
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        Login win = new Login();
                        win.Show();
                        this.Close();
                    }));
                }
            }
        }

        /// <summary>
        ///  静默安装
        /// </summary>
        /// <param name="filename"></param>
        private void SilentInstall(string filename)
        {
            ProcessHelper proc = new ProcessHelper();
            proc.SetExitHandler((s, e1) =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    Login win = new Login();
                    win.Show();
                    this.Close();
                }));
            });
            proc.PrintDoc(filename, "/S");
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



    }

}
