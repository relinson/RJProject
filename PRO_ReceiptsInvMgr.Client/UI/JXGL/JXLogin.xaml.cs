using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Client.Helper;
using PRO_ReceiptsInvMgr.Client.Resources.xskin;
using PRO_ReceiptsInvMgr.Component;
using PRO_ReceiptsInvMgr.Core.Utilites;
using PRO_ReceiptsInvMgr.Domain.Enum;
using PRO_ReceiptsInvMgr.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Util.Controls;

namespace PRO_ReceiptsInvMgr.Client.UI.JXGL
{
    /// <summary>
    /// Interaction logic for JXLogin.xaml
    /// </summary>
    public partial class JXLogin : Window
    {
        private bool isBack = true;
        JXManagerService service = new JXManagerService();

        public JXLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetMainWindowInstance.Show();
            this.Close();
        }

        private void top_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pwd.Password == string.Empty)
            {
                pwd.SetValue(ControlAttachProperty.WatermarkProperty, "请输入税盘口令");
            }
            else
            {
                pwd.SetValue(ControlAttachProperty.WatermarkProperty, string.Empty);
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pwd.Password))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PwdNotEmpty, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            var deviceId = BoxInfoHelper.GetDeviceNo();
            if (string.IsNullOrEmpty(deviceId))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.GetDeviceNoError, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            var isSuccess = false;
            string getDqdmError = string.Empty;
            this.Dispatcher.Invoke(new Action(() =>
            {
                WaitingBox.Show(() =>
                {
                    string netLocation = service.GetNetLocation(GlobalInfo.Dqdm);

                    if (!string.IsNullOrEmpty(netLocation))
                    {
                        CryptTool.LoginUrl = netLocation + ConfigHelper.GetAppSettingValue("JXLoginUri");
                        CryptTool.QueryUrl = netLocation + ConfigHelper.GetAppSettingValue("JXQueryUri");
                    }
                 
                    if (!string.IsNullOrEmpty(getDqdmError))
                    {
                        return;
                    }

                    var loop = 0;
                    while ((loop < 5 && !string.IsNullOrEmpty(CryptTool.ErrorMsg) && CryptTool.ErrorMsg.Contains(PRO_ReceiptsInvMgr.Resources.Message.JXCertError)) || loop == 0)
                    {
                        isSuccess = CryptTool.Login(pwd.Password);
                        GlobalInfo.NSRMC = CryptTool.Nsrmc;

                        loop++;
                        if (isSuccess)
                        {
                            break;
                        }
                    }
                }, PRO_ReceiptsInvMgr.Resources.Message.loginWait);
            }));

            if (!string.IsNullOrEmpty(getDqdmError))
            {
                MessageBoxEx.Show(this, getDqdmError, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            else
            {
                Logging.Log4NetHelper.Info(this, "地区代码：" + GlobalInfo.Dqdm);
            }

            if (!isSuccess)
            {
                MessageBoxEx.Show(this, CryptTool.ErrorMsg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
            }
            else
            {
                GlobalInfo.JxPwd = pwd.Password;
                GlobalInfo.token = CryptTool.Token;

                JXManager win = new JXManager();
                win.Show();

                isBack = false;
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isBack)
            {
                MainWindow.GetMainWindowInstance.NotifyMenuItemRestore_Click(null, null);
            }
        }
 
         
    }
}
