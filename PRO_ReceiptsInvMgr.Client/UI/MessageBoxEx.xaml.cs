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
    public partial class MessageBoxEx : Window
    {
        #region Public
        private double _endTop;
        private double _screenHeight;
        private DateTime _startTime;
        private DispatcherTimer _timer;
        private void Init()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(0) };
            _screenHeight = SystemParameters.WorkArea.Height;
            _startTime = DateTime.Now;
            _endTop = _screenHeight - Height - 10;
            double screenWidth = SystemParameters.WorkArea.Width;
            Left = screenWidth - Width - 10;
            Top = _screenHeight;
            _timer.Tick += Start_Tick;
            _timer.Start();
        }

        private void Start_Tick(object sender, EventArgs e)
        {
            if (Top > _endTop)
            {
                Top -= 5;
            }
            if ((DateTime.Now - _startTime).Seconds >= 3)
            {
                _timer.Stop();
                _timer.Interval = TimeSpan.FromMilliseconds(100);
                _timer.Tick -= Start_Tick;
                _timer = null;
                Close_Window();
            }
        }


        /// <summary>
        ///     窗体定时关闭
        /// </summary>
        private void Close_Window()
        {
            this.DialogResult = true;

        }
        /// <summary>
        ///     窗体渐入
        /// </summary>
        private void Open_Window()
        {
            if (Visibility == Visibility.Hidden)
            {
                Visibility = Visibility.Visible;
            }
            Init();
        }
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="owner">父窗体,默认为null,设置此参数可更改消息框的背景图与父窗体一致</param>
        /// <param name="msgText">提示文本</param>
        /// <param name="caption">消息框的标题</param>
        /// <param name="msgBoxIcon">消息框的图标枚举</param>
        /// <param name="msgBoxButtons">消息框的按钮,此值可为MessageBoxButtons.OK,MessageBoxButtons.OKCancelMessageBoxButtons.RetryCancel</param>
        public static void InfoShow(object obj,
            string msgText,
            string caption,
            MessageBoxExButtons msgBoxButtons,
            MessageBoxExIcon msgBoxIcon)
        {

            MessageBoxEx msgBox = new MessageBoxEx(msgText, caption, msgBoxIcon, msgBoxButtons);
            msgBox.Height = 180;
            msgBox.Width = 330;
            msgBox.Open_Window();
            msgBox.WindowStartupLocation = WindowStartupLocation.Manual;
            //Always set top most
            msgBox.Topmost = true;
            msgBox.ShowDialog();


        }
        /// <summary>
        /// 无返回值，定时消失的消息框
        /// </summary>
        /// <param name="msgText"></param>
        /// <param name="caption"></param>
        /// <param name="msgBoxButtons"></param>
        /// <param name="msgBoxIcon"></param>
        /// <returns></returns>
        public static void InfoShow(string msgText = "请输入提示信息",
            string caption = "提示",
            MessageBoxExButtons msgBoxButtons = MessageBoxExButtons.OK,
            MessageBoxExIcon msgBoxIcon = MessageBoxExIcon.Information)
        {
            InfoShow(null, msgText, caption, msgBoxButtons, msgBoxIcon);

        }


        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="owner">父窗体,默认为null,设置此参数可更改消息框的背景图与父窗体一致</param>
        /// <param name="msgText">提示文本</param>
        /// <param name="caption">消息框的标题</param>
        /// <param name="msgBoxIcon">消息框的图标枚举</param>
        /// <param name="msgBoxButtons">消息框的按钮,此值可为MessageBoxButtons.OK,MessageBoxButtons.OKCancelMessageBoxButtons.RetryCancel</param>
        public static bool? Show(object obj,
            string msgText,
            string caption,
            MessageBoxExButtons msgBoxButtons,
            MessageBoxExIcon msgBoxIcon, string confirmContent = "")
        {
            MessageBoxEx msgBox = new MessageBoxEx(msgText, caption, msgBoxIcon, msgBoxButtons, confirmContent);
            msgBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //Always set top most
            msgBox.Topmost = true;
            msgBox.Owner = obj as Window;
            msgBox.ShowDialog();
            return msgBox.DialogResult;
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="msgText"></param>
        /// <param name="caption"></param>
        /// <param name="msgBoxButtons"></param>
        /// <param name="msgBoxIcon"></param>
        /// <returns></returns>
        public static bool? Show(string msgText = "请输入提示信息",
            string caption = "提示",
            MessageBoxExButtons msgBoxButtons = MessageBoxExButtons.OK,
            MessageBoxExIcon msgBoxIcon = MessageBoxExIcon.Information)
        {
            return Show(null, msgText, caption, msgBoxButtons, msgBoxIcon);

        }



        #endregion

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        /// <param name="msgBoxIcon"></param>
        /// <param name="msgBoxButtons"></param>
        public MessageBoxEx(string msg, string caption, MessageBoxExIcon msgBoxIcon, MessageBoxExButtons msgBoxButtons, string confirmContent = "")
        {
            InitializeComponent();
            this.Title = caption;

            msgBlock.Text = msg;

            if (msgBoxButtons == MessageBoxExButtons.OK)
            {
                btnNo.Visibility = Visibility.Collapsed;
            }
            if (msgBoxButtons == MessageBoxExButtons.Cancel)
            {
                btnNo.Visibility = Visibility.Collapsed;
                btnYes.Visibility = Visibility.Collapsed;
            }

            if (!string.IsNullOrEmpty(confirmContent))
            {
                tbConfirm.Visibility = Visibility.Visible;
                tbConfirm.Content = confirmContent;
            }
            else
            {
                tbConfirm.Visibility = Visibility.Collapsed;
                tbConfirm.Content = string.Empty;
            }
        }

        /// <summary>
        /// 下拉事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            scrollViewer1.RaiseEvent(eventArg);
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

        /// <summary>
        /// 确定关闭提示框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// 取消关闭提示框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }

    /// <summary>
    /// MessageBoxExButtons
    /// </summary>
    public enum MessageBoxExButtons
    {
        /// <summary>
        /// 消息框包含“确定”按钮
        /// </summary>
        OK,
        ///// <summary>
        ///// 消息框包含“确定”与“取消”按钮
        ///// </summary>
        //OKCancel,
        ///// <summary>
        ///// 消息框包含“重试”与“取消”按钮
        ///// </summary>
        //RetryCancel,

        YesNo,
        /// <summary>
        /// 取消，不需要任何按钮
        /// </summary>
        Cancel
    }

    /// <summary>
    /// MessageBoxExIcon
    /// </summary>
    public enum MessageBoxExIcon
    {
        Error,
        Information,
        OK,
        Question,
        Warning
    }



}

