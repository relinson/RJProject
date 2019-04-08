
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRO_ReceiptsInvMgr.UpdateApp
{
    /// <summary>
    /// Interaction logic for Decompression.xaml
    /// </summary>
    public partial class Decompression : Window
    {
        public string localFileName { get; set; }
        public Decompression()
        {
            InitializeComponent();
            PictureOfGif.Image = PRO_ReceiptsInvMgr.UpdateApp.Properties.Resources.appLoading;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread td = new Thread(() => Update());
            td.IsBackground = true;
            td.Start();
        }

        private void Update()
        {
            try
            {
                Thread.Sleep(1000);
                ProcessHelper proc = new ProcessHelper();
                proc.SetExitHandler((s, e1) =>
                {
                    string updateResultPath = System.Windows.Forms.Application.StartupPath + "\\Update.ini";
                    MyIniFile ini = new MyIniFile(updateResultPath);
                    ini.IniWriteValue("UpdateResult", localFileName, "1");
                    CheckRegAndRun();
                });
                proc.PrintDoc(localFileName,"/S");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("升级失败，请联系管理员", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log4NetHelper.Error(this, "升级失败:", ex);
                Environment.Exit(1);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            e.Cancel = true;
        }

        /// <summary>
        /// 检查注册表并启动客户端
        /// </summary>
        private bool CheckRegAndRun()
        {
            bool ret = false;
            string exePath = GetExePath();
            if (!string.IsNullOrEmpty(exePath) && File.Exists(exePath))
            {
                Process pr = new Process();
                pr.StartInfo.FileName = exePath;
                pr.StartInfo.WorkingDirectory = exePath.Substring(0, exePath.LastIndexOf("\\"));
                pr.Start();
                ret = true;
                Environment.Exit(1);
            }

            return ret;
        }


        /// <summary>
        /// 获取启动软件路径
        /// </summary>
        /// <returns></returns>
        private string GetExePath()
        {
            string strPath = string.Empty;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = hkml.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\PRO_ReceiptsInvMgr.Client.exe");
            if (software != null)
            {
                strPath = software.GetValue("").ToString();
            }
            hkml.Close();

            return strPath;
        }
    }
}
