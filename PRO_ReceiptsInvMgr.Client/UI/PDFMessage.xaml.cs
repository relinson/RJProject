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
using System.Windows.Threading;
using PRO_ReceiptsInvMgr.Client.Resources.xskin;

namespace PRO_ReceiptsInvMgr.Client.UI
{
    /// <summary>
    /// Interaction logic for MessageBoxEx.xaml
    /// </summary>
    public partial class PDFMessage : BaseWindow
    {
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="type">0为pdf，1为退出</param>
        /// <param name="owner">父窗体,默认为null,设置此参数可更改消息框的背景图与父窗体一致</param>
        /// <param name="msgText">提示文本</param>
        /// <param name="caption">消息框的标题</param>
        /// <param name="msgBoxIcon">消息框的图标枚举</param>
        /// <param name="msgBoxButtons">消息框的按钮,此值可为MessageBoxButtons.OK,MessageBoxButtons.OKCancelMessageBoxButtons.RetryCancel</param>
        public bool? ShowDlog()
        {
            PDFMessage msgBox = new PDFMessage();
            msgBox.Topmost = true;
            msgBox.ShowDialog();
            return msgBox.DialogResult;
        }

        public bool? ShowWin()
        {
            this.Topmost = true;
            this.Show();
            return this.DialogResult;
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <param name="msgBoxIcon"></param>
        /// <param name="msgBoxButtons"></param>
        public PDFMessage()
        {
            InitializeComponent();
            this.Owner = MainWindow.GetMainWindowInstance;
        }

        /// <summary>
        /// 设置子窗体topmost,防止点击任务导致子窗体不显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GetMainWindowInstance_OnChildWindowTopMost(object sender, EventArgs e)
        {
            this.Topmost = ((bool)sender);
        }

        /// <summary>
        /// 如果当前子窗体打开，父窗体隐藏时同时隐藏子窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GetMainWindowInstance_OnChildWindowShowOrHide(object sender, EventArgs e)
        {
            if (((bool)sender))
            {
                if (this.Visibility == Visibility.Hidden)
                {
                    this.ShowDialog();
                }
                this.Activate();
            }
            else
            {
                this.Hide();
            }
        }

          
    }

}
