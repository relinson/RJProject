using Newtonsoft.Json;
using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Client.Helper;
using PRO_ReceiptsInvMgr.Client.Resources.xskin;
using PRO_ReceiptsInvMgr.Component;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using PRO_ReceiptsInvMgr.Domain.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
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
    public partial class JXRegister : Window
    {
        public RelayCommand SetUserName { get; private set; }
        private bool isBack = true;

        public JXRegister()
        {
            InitializeComponent();

            this.SetUserName = new RelayCommand(DoSetUserName);

        }
        private void DoSetUserName()
        {
            if (string.IsNullOrEmpty(pwd.Password))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PwdNotEmpty, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            CryptTool.ErrorMsg = string.Empty;
            CryptTool.UserPin = pwd.Password;
            CryptTool.getThisCert();

            if (string.IsNullOrEmpty(CryptTool.ErrorMsg))
            {
                txtNsrsbh.Text = CryptTool.Nsrsbh;
                txtNsrName.Text = CryptTool.Nsrmc;
                pwd.IsEnabled = false;
            }
            else
            {
                MessageBoxEx.Show(this, CryptTool.ErrorMsg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
            }
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
            if (pwd.Password != "")
            {
                pwd.SetValue(ControlAttachProperty.WatermarkProperty, "");
            }
            else
            {
                pwd.SetValue(ControlAttachProperty.WatermarkProperty, "请输入税盘口令");
            }
        }


        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pwd.Password))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PwdNotEmpty, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtNsrsbh.Text))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.NsrNotEmpty, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            try
            {
                DeviceHelper.matchDriveLetterWithSerial();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(this, ex.Message, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            string deviceID = string.Empty;
            if (DeviceHelper.SerialNumber.Any())
            {
                deviceID = DeviceHelper.SerialNumber[0];
            }
            else
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.GetDeviceError, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            string strRequest = new JavaScriptSerializer().Serialize(
                new JXRegisterRequest { taxno = txtNsrsbh.Text, orgName = txtNsrName.Text, area = GlobalInfo.Dqdm, deviceId = deviceID });
             bool result = false;
            string errorMsg = string.Empty;
            var response = WSInterface.GetResponse(strRequest, InterfaceType.JXRegister, ref result, out errorMsg);

            if (result)
            {
                var obj = new JsonSerializer().Deserialize<JXIsRegisterResponse>(new JsonTextReader(new StringReader(response)));
                if (obj.result == "1")
                {
                    JXLogin jxLogin = new JXLogin();
                    jxLogin.Show();

                    isBack = false;
                    this.Close();
                }
                else
                {
                    MessageBoxEx.Show(obj.message, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                }
            }
            else
            {
                MessageBoxEx.Show(errorMsg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
            }
        }

        private void RegisterPage_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isBack)
            {
                MainWindow.GetMainWindowInstance.NotifyMenuItemRestore_Click(null, null);
            }
        }


    }
}
