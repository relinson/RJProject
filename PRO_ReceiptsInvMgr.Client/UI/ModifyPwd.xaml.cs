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
    public partial class ModifyPwd : Window
    {
        LoginService loginService = new LoginService();
        public ModifyPwd()
        {
            InitializeComponent();
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

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (Login.LoginWinInstance != null)
            {
                Login.LoginWinInstance.Show();
            }
        }

        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(oldPwd.Password))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PormptSetOldPwd, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(newPwd.Password))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.PormptSetNewPwd, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            if (newPwd.Password != confirmNewPwd.Password)
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.NotSameNewPwd, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            string errorMsg = string.Empty;
            string nsrsbh = GlobalInfo.DeviceType == DeviceType.JSP.GetHashCode() ? tbloginNsrsbh.Text : cbxSkpLoginNsrsbh.Text.ToString();
            var result = loginService.ModifyPwd(nsrsbh, oldPwd.Password, newPwd.Password, out errorMsg);
            if (!result)
            {
                MessageBoxEx.Show(this, errorMsg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
            }
            else
            {
                MessageBoxEx.Show(this, "修改成功", PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                menuClose_Click(null,null);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (GlobalInfo.DeviceType == DeviceType.JSP.GetHashCode())
            {
                tbloginNsrsbh.Text = JsDiskHelper.GetNsrsbh();
                tbloginNsrsbh.IsReadOnly = true;
            }
            else
            {
                tbloginNsrsbh.Visibility = Visibility.Collapsed;
                cbxSkpLoginNsrsbh.Visibility = Visibility.Visible;

                cbxSkpLoginNsrsbh.ItemsSource = loginService.GetSkpNsrsbhList();
            }
        }

         
    }


}
