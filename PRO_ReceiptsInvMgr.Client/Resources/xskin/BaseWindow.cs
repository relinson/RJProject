using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;
using PRO_ReceiptsInvMgr.Client.UI;
using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Core.Utilites;
using System.Windows.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using PRO_ReceiptsInvMgr.Client.UserControls;
using System.ComponentModel;
using PRO_ReceiptsInvMgr.Domain.Enum;

namespace PRO_ReceiptsInvMgr.Client.Resources.xskin
{
    /// <summary>
    /// 功能描述：窗体基础类
    /// 创建日期：2015-02-06
    /// </summary>
    public class BaseWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 动态加载页面属性值
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        } 

        private bool isDisplayMin = true;
        /// <summary>
        /// 是否显示最小化按钮
        /// </summary>
        public bool IsDisplayMin
        {
            get { return isDisplayMin; }
            set { isDisplayMin = value; }
        }

        private bool isDisplayClose = true;
        /// <summary>
        /// 是否显示关闭
        /// </summary>
        public bool IsDisplayClose
        {
            get { return isDisplayClose; }
            set { isDisplayClose = value; }
        }

        private bool isDisplayMax = true;
        /// <summary>
        /// 是否显示最大化按钮
        /// </summary>
        public bool IsDisplayMax
        {
            get { return isDisplayMax; }
            set { isDisplayMax = value; }
        }


        private bool isDisplayQA = true;
        /// <summary>
        /// 是否显示最大化按钮
        /// </summary>
        public bool IsDisplayQA
        {
            get { return isDisplayQA; }
            set { isDisplayQA = value; }
        }

        private bool isShowTitle = true;
        /// <summary>
        /// 是否显示Title
        /// </summary>
        public bool IsShowTitle
        {
            get { return isShowTitle; }
            set { isShowTitle = value; }
        }

        public System.Windows.Forms.NotifyIcon MyNotifyIcon
        {
            get
            {
                return myNotifyIcon;
            }

            set
            {
                myNotifyIcon = value;
            }
        }

        public System.Windows.Forms.ToolStripMenuItem NotifySetup
        {
            get
            {
                return notifySetup;
            }

            set
            {
                notifySetup = value;
            }
        }

        public System.Windows.Forms.ToolStripMenuItem NotifyMenuItemMin
        {
            get
            {
                return notifyMenuItemMin;
            }

            set
            {
                notifyMenuItemMin = value;
            }
        }

        public System.Windows.Forms.ToolStripMenuItem NotifyMenuItemMax
        {
            get
            {
                return notifyMenuItemMax;
            }

            set
            {
                notifyMenuItemMax = value;
            }
        }

        public System.Windows.Forms.ToolStripMenuItem NotifyMenuItemRestore
        {
            get
            {
                return notifyMenuItemRestore;
            }

            set
            {
                notifyMenuItemRestore = value;
            }
        }

        public System.Windows.Forms.ToolStripMenuItem NotifyMenuItemExit
        {
            get
            {
                return notifyMenuItemExit;
            }

            set
            {
                notifyMenuItemExit = value;
            }
        }

        public System.Windows.Forms.ToolStripMenuItem NotifyMenuItemDisplay
        {
            get
            {
                return notifyMenuItemDisplay;
            }

            set
            {
                notifyMenuItemDisplay = value;
            }
        }

        protected Rect RectSizeMax;     //定义一个全局rect记录最大状态下窗口的位置和大小。
        protected Rect RectSizeRestore; //定义一个全局rect记录还原状态下窗口的位置和大小。
        protected BaseWindowState MyWindowState = BaseWindowState.Normal;
        protected ControlTemplate windowTemplate = (ControlTemplate)App.Current.Resources["BaseWindowControlTemplate"];

        /// <summary>
        /// 加载托盘
        /// </summary>
        public void LoadNotifyIcon()
        {
            var uri = new Uri(@"Resources\image\icon\2.ico", UriKind.Relative);
            var info = System.Windows.Application.GetResourceStream(uri);
            if (info.Stream.Length > 0)
            {
                this.MyNotifyIcon = new System.Windows.Forms.NotifyIcon();
                this.MyNotifyIcon.BalloonTipText = "久易-进项票管家";         //设置程序启动时显示的文本
                this.MyNotifyIcon.Text = "久易-进项票管家";                   //最小化到托盘时，鼠标点击时显示的文本
                System.Drawing.Icon icon = new System.Drawing.Icon(info.Stream);   //程序图标 
                this.MyNotifyIcon.Icon = icon;
                this.MyNotifyIcon.Visible = true;
                this.MyNotifyIcon.MouseClick += OnNotifyIconClick;
                this.MyNotifyIcon.ShowBalloonTip(1000);
            }

            LoadNotifyContextMenu();
        }

        readonly System.Windows.Forms.ContextMenuStrip contextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

        /// <summary>
        /// 加载托盘菜单
        /// </summary>
        private void LoadNotifyContextMenu()
        {
            this.MyNotifyIcon.ContextMenuStrip = contextMenuStrip;

            NotifyMenuItemExit.Text = "退出";
            NotifyMenuItemExit.Click += new EventHandler(NotifyMenuItemExit_Click);

            NotifyMenuItemMin.Text = "隐藏主界面";
            NotifyMenuItemMin.Click += new EventHandler(NotifyMenuItemMin_Click);

            NotifyMenuItemRestore.Text = "还原";
            NotifyMenuItemRestore.Click += new EventHandler(NotifyMenuItemRestore_Click);

            NotifyMenuItemDisplay.Text = "显示主界面";
            NotifyMenuItemDisplay.Click += new EventHandler(NotifyMenuItemDisplay_Click);

            contextMenuStrip.Items.Add(NotifyMenuItemMin);
            contextMenuStrip.Items.Add(NotifyMenuItemExit);
        }

        /// <summary>
        /// 双击托盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyMenuDB_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            //SetMyWindowState(BaseWindowState.Maximized);
            contextMenuStrip.Items.Remove(NotifyMenuItemDisplay);
            contextMenuStrip.Items.Insert(0, NotifyMenuItemMin);
            this.Activate();
        }

        /// <summary>
        /// 托盘右键菜单（最小化/隐藏）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NotifyMenuItemMin_Click(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;

            contextMenuStrip.Items.Remove(NotifyMenuItemMin);
            contextMenuStrip.Items.Insert(0,NotifyMenuItemDisplay);
            if (OnChildWindowShowOrHide != null)
            {
                OnChildWindowShowOrHide(false, e);
            }
        }

        /// <summary>
        /// 托盘右键菜单（最大化）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyMenuItemMax_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            SetMyWindowState(BaseWindowState.Normal);
            this.Activate();
            if (OnChildWindowShowOrHide != null)
            {
                OnChildWindowShowOrHide(true, e);
            }
        }

        /// <summary>
        /// 托盘右键菜单（还原）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NotifyMenuItemRestore_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;

            this.Activate();
            if (OnChildWindowShowOrHide != null)
            {
                OnChildWindowShowOrHide(true, e);
            }
        }


        /// <summary>
        /// 托盘右键菜单（退出）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyMenuItemExit_Click(object sender, EventArgs e)
        {
            CloseClient();
        }

        public void CloseClient()
        {
            if (this.MyNotifyIcon != null)
            {
                this.MyNotifyIcon.Dispose();//手动释放通知栏图标
            }

            Environment.Exit(1);
        }


        public void NotifyMenuItemDisplay_Click(object sender, EventArgs e)
        {
            NotifyMenuDB_Click(sender, e);
        }

        /// <summary>
        /// 构造实例
        /// </summary>
        public BaseWindow()
        {
            InitializeStyle();
            this.Loaded += delegate
            {
                InitializeEvent(base.Icon, base.Title);
            };
        }

        private System.Windows.Forms.NotifyIcon myNotifyIcon;
        public event EventHandler OnChildWindowShowOrHide = null;  //如果子窗体处于打开状态，通过Notify隐藏主窗体时隐藏子窗体
        private System.Windows.Forms.ToolStripMenuItem notifySetup = new System.Windows.Forms.ToolStripMenuItem();
        private System.Windows.Forms.ToolStripMenuItem notifyMenuItemMin = new System.Windows.Forms.ToolStripMenuItem();
        private System.Windows.Forms.ToolStripMenuItem notifyMenuItemMax = new System.Windows.Forms.ToolStripMenuItem();
        private System.Windows.Forms.ToolStripMenuItem notifyMenuItemRestore = new System.Windows.Forms.ToolStripMenuItem();
        private System.Windows.Forms.ToolStripMenuItem notifyMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
        private System.Windows.Forms.ToolStripMenuItem notifyMenuItemDisplay = new System.Windows.Forms.ToolStripMenuItem();

        /// <summary>
        /// 设置窗体状态
        /// </summary>o
        public void InitialWindowState(double width=1170, double height = 750,bool isMax = true)
        {
            //设置页面最大化最小化
            this.Left = 0;
            this.Top = 0;
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            RectSizeMax = new Rect(0, 0, this.Width, this.Height);
            double restoreLeft = (this.Width - width) / 2;
            double restoreTop = (this.Height - height) / 2;
            RectSizeRestore = new Rect(restoreLeft, restoreTop, width, height);
            if (isMax)
            {
                SetMyWindowState(BaseWindowState.Maximized);
            }
            else
            {
                SetMyWindowState(BaseWindowState.Normal);
            }
        }
         
        /// <summary>
        /// 双击托盘图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNotifyIconClick(object sender,System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                NotifyMenuDB_Click(sender, e);
            }
        }

      

        /// 样式初始
        /// </summary>
        private void InitializeStyle()
        {
            this.Style = (Style)App.Current.Resources["BaseWindowStyle"];
        }


        /// <summary>
        /// 窗口初始相关
        /// </summary>
        /// <param name="title"></param>
        private void InitializeEvent(ImageSource icon, string title)
        {
            //窗口图标
            var imgWinIcon = (Image)windowTemplate.FindName("imgWinIcon", this);
            imgWinIcon.Source = icon;
            imgWinIcon.Visibility = icon == null ? Visibility.Hidden : Visibility.Visible;

            //窗口标题
            if (IsShowTitle)
            {
                var txtWinTitle = (TextBlock)windowTemplate.FindName("txtWinTitle", this);
                txtWinTitle.Text = title;
                txtWinTitle.Margin = icon == null ? new Thickness(4, 0, 0, 0) : txtWinTitle.Margin;

            }

            //窗口最小化
            var btnMin = (System.Windows.Controls.Button)windowTemplate.FindName("btnMin", this);
            btnMin.Click += delegate
            {
                this.WindowState = WindowState.Minimized;
            };

            
            //窗口最大化或还原
            var btnMax = (System.Windows.Controls.Button)windowTemplate.FindName("btnMax", this);
            btnMax.Click += delegate
            {
                this.WindowState = System.Windows.WindowState.Normal;
                SetMyWindowState(MyWindowState);
            };

            //窗口关闭
            var btnClose = (System.Windows.Controls.Button)windowTemplate.FindName("btnClose", this);
            btnClose.Click += delegate
            {
                if (MainWindow.GetMainWindowInstance == null)
                {
                    this.Close();
                }
                if (this.Name == MainWindow.GetMainWindowInstance.Name)
                {
                    var result = MessageBoxEx.Show(this,"确认要退出吗？","提示",MessageBoxExButtons.YesNo,MessageBoxExIcon.Question);
                    if (result.HasValue && result.Value)
                    {
                        NotifyMenuItemExit_Click(null, null);
                    }
                }
                else
                {
                    this.Close();
                }
            };

            //窗口标题栏
            var borderTitleBar = (Border)windowTemplate.FindName("borderTitleBar", this);
            borderTitleBar.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };

            //是否显示最大化与最小化
            if (!IsDisplayMin)
            {
                btnMin.Visibility = System.Windows.Visibility.Hidden;
            }

            if (!IsDisplayMax)
            {
                btnMax.Visibility = System.Windows.Visibility.Collapsed;
            }

            var btnQA = (Button)windowTemplate.FindName("btnQA", this);

            if (!IsDisplayQA)
            {
                btnQA.Visibility = System.Windows.Visibility.Hidden;
            }

            if (!IsDisplayClose)
            {
                btnClose.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        /// <summary>
        /// 最大化或还原
        /// </summary>
        /// <param name="btnMax"></param>
        public void SetMyWindowState(BaseWindowState windowState)
        {
            var btnMax = (Button)windowTemplate.FindName("btnMax", this);
            MyWindowState = windowState;
            switch (MyWindowState)
            {
                case BaseWindowState.Normal:
                    this.Top = SystemParameters.WorkArea.Top;
                    this.Left = SystemParameters.WorkArea.Left;
                    this.Width = SystemParameters.WorkArea.Width;
                    this.Height = SystemParameters.WorkArea.Height;
                     btnMax.Background = new ImageBrush(GetImageSource(@"Resources/xskin/icon/rest.png"));
                    MyWindowState = BaseWindowState.Maximized;
                    break;
                case BaseWindowState.Maximized:
                    this.Left = RectSizeRestore.Left;
                    this.Top = RectSizeRestore.Top;
                    this.Width = RectSizeRestore.Width;
                    this.Height = RectSizeRestore.Height;
                    btnMax.Background = new ImageBrush(GetImageSource(@"Resources/xskin/icon/max.png"));
                    MyWindowState = BaseWindowState.Normal;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 图片资源转换
        /// </summary>
        /// <param name="sourceUri"></param>
        /// <returns></returns>
        private ImageSource GetImageSource(string sourceUri)
        {
            var uri = new Uri(sourceUri, UriKind.Relative);
            var info = System.Windows.Application.GetResourceStream(uri);
            var convert = new ImageSourceConverter();
            var imageSource = (ImageSource)convert.ConvertFrom(info.Stream);

            return imageSource;
        }
    }

    /// <summary>
    /// 自定义窗口状态
    /// </summary>
    public enum BaseWindowState
    {
        Normal,
        //Minimized,
        Maximized,
    }
}
