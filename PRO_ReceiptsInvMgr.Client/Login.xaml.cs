using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Client.Helper;
using PRO_ReceiptsInvMgr.Client.Resources.xskin;
using PRO_ReceiptsInvMgr.Client.UI;
using PRO_ReceiptsInvMgr.Component;
using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using PRO_ReceiptsInvMgr.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
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

namespace PRO_ReceiptsInvMgr.Client
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        /// <summary>
        /// 税控盘名称
        /// </summary>
        private string SKPName = PRO_ReceiptsInvMgr.Resources.Common.SKPDeviceName;

        /// <summary>
        /// 金税盘名称
        /// </summary>
        private string JSPName = PRO_ReceiptsInvMgr.Resources.Common.JSPDeviceName;

        public LoginService loginService = new LoginService();
        [DllImport("Cryp_Ctl.ocx", EntryPoint = "DllRegisterServer")]
        private static extern int Cryp_CtlRegisterServer();

        public RelayCommand SetUserName { get; private set; }

        private string LoginPwd { get; set; }

        private string CertPwd { get; set; }

        public static Login LoginWinInstance { get; set; }
        public Login()
        {
            InitializeComponent();

            DoSetUserName();
            LoginWinInstance = this;
            Cryp_CtlRegisterServer();
            this.SetUserName = new RelayCommand(DoSetUserName);
        }
        private void DoSetUserName()
        {
//             if (string.IsNullOrEmpty(tbTaxKey.Password))
//             {
//                 MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PwdNotEmpty, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
//                 return;
//             }

            CryptTool.ErrorMsg = string.Empty;
            CryptTool.UserPin = tbTaxKey.Password;
            CryptTool.getThisCert();

            if (string.IsNullOrEmpty(CryptTool.ErrorMsg))
            {
                tbRegNsrsbh.Text = CryptTool.Nsrsbh;
                tbRegNsrmc.Text = CryptTool.Nsrmc;
                tbTaxKey.IsEnabled = false;
            }
            else
            {
                MessageBoxEx.Show(this, CryptTool.ErrorMsg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void top_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textblock = sender as TextBlock;
            if (textblock == tbLogin)
            {
                tbLogin.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0xff, 0xff));
                tbRegister.Foreground = new SolidColorBrush(Color.FromRgb(0xa9, 0xc9, 0xff));
                imgArrow.Margin = new Thickness(150, 0, 0, 0);

                loginPanel.Visibility = Visibility.Visible;
                registPanel.Visibility = Visibility.Hidden;
                this.Height = 360;
            }
            else
            {
                tbLogin.Foreground = new SolidColorBrush(Color.FromRgb(0xa9, 0xc9, 0xff));
                tbRegister.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0xff, 0xff));
                imgArrow.Margin = new Thickness(240, 0, 0, 0);

                loginPanel.Visibility = Visibility.Hidden;
                registPanel.Visibility = Visibility.Visible;
                this.Height = 450;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pwd = sender as PasswordBox;
            if (pwd.Password == string.Empty)
            {
                pwd.SetValue(ControlAttachProperty.WatermarkProperty, pwd.Tag);
            }
            else
            {
                pwd.SetValue(ControlAttachProperty.WatermarkProperty, string.Empty);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalInfo.ExistTax = true;
            var areaList = loginService.GetAreaList();
            cbxArea.ItemsSource = areaList;
            cbxArea.DisplayMemberPath = "Name";
            cbxArea.SelectedValuePath = "Code";

            if (GlobalInfo.DeviceType == DeviceType.JSP.GetHashCode())
            {
                tbloginNsrsbh.Text = JsDiskHelper.GetNsrsbh();
                tbRegNsrsbh.Text = JsDiskHelper.GetNsrsbh();

                tbloginNsrsbh.IsReadOnly = true;
                tbRegNsrsbh.IsReadOnly = true;
            }
            else
            {
                cbxSkpLoginNsrsbh.Text = CryptTool.Nsrsbh;
                tbloginNsrsbh.Visibility = Visibility.Collapsed;
                cbxSkpLoginNsrsbh.Visibility = Visibility.Visible;

                cbxSkpLoginNsrsbh.ItemsSource = loginService.GetSkpNsrsbhList();

            }

            BindUserInfo(CryptTool.Nsrsbh);
        }

        private void BindUserInfo(string nsrsbh)
        {
            if (!string.IsNullOrEmpty(nsrsbh))
            {
                var userInfo = loginService.GetUserLogin(nsrsbh);
                if (userInfo != null)
                {
                    LoginPwd = userInfo.LoginPwd;
                    CertPwd = userInfo.CertPwd;

                    loginPwd.Password = userInfo.LoginPwd;
                    taxKey.Password = userInfo.CertPwd;
                    chbRemember.IsChecked = userInfo.IsRemember;
                }
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pwd.Password))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PormptSetLoginPwd, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            if (pwd.Password != confirmPwd.Password)
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.NotSamePwd, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(tbRegNsrsbh.Text))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.NsrsbhNotEmpty, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(tbRegNsrmc.Text))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.NsrsmcNotEmpty, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            if (cbxArea.SelectedIndex < 0)
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PromptSelectArea, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(tbRegCode.Text))
            {
                //如果注册码为空，则上传试用注册码  20190419
                tbRegCode.Text = "RJTECH2019"; 
//                 MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PormptInputZcm, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
//                 return;
            }

            string errorMsg = string.Empty;
            bool isRegisterSuccess = loginService.Register(tbRegCode.Text, tbRegNsrsbh.Text, tbRegNsrmc.Text, pwd.Password, cbxArea.SelectedValue.ToString(), out errorMsg);
            if (!isRegisterSuccess)
            {
                MessageBoxEx.Show(this, errorMsg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            else
            {
                ConfigObject configObject = new ConfigObject();
                configObject.NSRSBH = tbRegNsrsbh.Text;
                configObject.NSRMC = tbRegNsrmc.Text;
                configObject.DQDM = cbxArea.SelectedValue.ToString();
                configObject.TaxType = GlobalInfo.DeviceType;
                loginService.SaveUserInfo(configObject);
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.RegisterSuccess, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);

                ResetRegistControl();
                tbTaxKey.IsEnabled = true;
                TextBlock_MouseLeftButtonDown(tbLogin, null);
            }

        }

        private void ResetRegistControl()
        {
            foreach (var control in registPanel.Children)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Text = string.Empty;
                }
                if (control is PasswordBox)
                {
                    ((PasswordBox)control).Password = string.Empty;
                }
                if (control is ComboBox)
                {
                    ((ComboBox)control).SelectedIndex = -1;
                }
            }
        }

        public class InvoiceType
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        { 

            if (GlobalInfo.DeviceType == DeviceType.JSP.GetHashCode())
            {
                if (string.IsNullOrEmpty(tbloginNsrsbh.Text))
                {
                    MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PromptInputNsrsbh, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(cbxSkpLoginNsrsbh.Text))
                {
                    MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PromptInputNsrsbh, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                    return;
                }
            }


            if (string.IsNullOrEmpty(loginPwd.Password))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PromptInputPwd, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(taxKey.Password))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PromptInputKey, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            CryptTool.UserPin = taxKey.Password;
            var ret = CryptTool.openThisDevice();
            if (ret != 0)
            {
                string msg = CryptTool.ErrorMsg;
                if (msg.Contains("0xA7"))
                {
                    msg = PRO_ReceiptsInvMgr.Resources.Message.NotFindTax;
                }
                MessageBoxEx.Show(this, msg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            string errorMsg = string.Empty;
            string nsrsbh = GlobalInfo.DeviceType == DeviceType.JSP.GetHashCode() ? tbloginNsrsbh.Text : cbxSkpLoginNsrsbh.Text.ToString();

            //登陆时取token   20190324
//             int retryCount = 3;
//             do
//             {
//                 --retryCount;
//                 GlobalInfo.token = GetTokenHelper.GetToken_dll(nsrsbh, CryptTool.UserPin, GlobalInfo.Dqdm);
//             } while (GlobalInfo.token.Length == 0 && retryCount > 0);

            var retObj = loginService.Login(nsrsbh, loginPwd.Password, GlobalInfo.token, out errorMsg);
            if (retObj == null)
            {
                MessageBoxEx.Show(this, errorMsg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            else
            {
                GlobalInfo.NSRSBH = nsrsbh;
                GlobalInfo.Dqdm = retObj.areaId;
                GlobalInfo.AppId = retObj.appKey;
                GlobalInfo.ExpiredTime = retObj.expiredTime;
                GlobalInfo.JxPwd = taxKey.Password;

                ConfigObject configObject = new ConfigObject();
                configObject.NSRSBH = nsrsbh;
                configObject.LoginPwd = loginPwd.Password;
                configObject.CertPwd = taxKey.Password;
                configObject.TaxType = GlobalInfo.DeviceType;

                if (chbRemember.IsChecked.HasValue)
                {
                    configObject.IsRemember = chbRemember.IsChecked.Value;
                    loginService.UpdateOrSaveUserInfo(configObject);
                }


                MainWindow win = new MainWindow();
                this.Close();
                win.Show();
                LoginWinInstance = null;

            }

        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void pwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (tbTaxKey.Password != "")
            {
                tbTaxKey.SetValue(ControlAttachProperty.WatermarkProperty, "");
            }
            else
            {
                tbTaxKey.SetValue(ControlAttachProperty.WatermarkProperty, tbTaxKey.Tag);
            }
        }

        private void menuModifyPwd_Click(object sender, RoutedEventArgs e)
        {
            ModifyPwd win = new ModifyPwd();
            win.Show();
            this.Hide();
        }

        private void menuModifyZcm_Click(object sender, RoutedEventArgs e)
        {
            ModifyZcm win = new ModifyZcm();
            win.Show();
            this.Hide();
        }

        private void cbxSkpLoginNsrsbh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxSkpLoginNsrsbh.SelectedValue != null)
            {
                string nsrsbh = cbxSkpLoginNsrsbh.SelectedValue.ToString();
                BindUserInfo(nsrsbh);
            }
            else
            {
                loginPwd.Password = string.Empty;
                taxKey.Password = string.Empty;
            }
        }
 
    }
}
