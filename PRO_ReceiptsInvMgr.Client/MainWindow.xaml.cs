using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.BLL;
using PRO_ReceiptsInvMgr.Client.Resources.xskin;
using PRO_ReceiptsInvMgr.Logging;
using System.ComponentModel;
using System.Diagnostics;
using PRO_ReceiptsInvMgr.Client.UI;
using System.Threading;
using PRO_ReceiptsInvMgr.Core.Utilites;
using PRO_ReceiptsInvMgr.Core.Helper;
using System.IO;
using PRO_ReceiptsInvMgr.WebService;
using Microsoft.Win32;
using System.Windows.Threading;
using PRO_ReceiptsInvMgr.Domain.Enum;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net;
using PRO_ReceiptsInvMgr.Resources;
using System.Management;
using PRO_ReceiptsInvMgr.Client.UserControls;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using log4net;
using log4net.Appender;
using PRO_ReceiptsInvMgr.Domain;
using PRO_ReceiptsInvMgr.Client.UI.FPCX;
using PRO_ReceiptsInvMgr.Client.UI.JXGL;
using PRO_ReceiptsInvMgr.Component;
using PRO_ReceiptsInvMgr.Client.Helper;

namespace PRO_ReceiptsInvMgr.Client
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow
    {
        #region WindowsApi
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool ShowWindow(IntPtr hWnd, short State);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam, ref bool ret);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll ", SetLastError = true)]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        private const int SendMessageValue = 99999;
        #endregion

        private readonly DispatcherTimer adverTimer = new DispatcherTimer();
        private readonly DispatcherTimer kpTimer = new DispatcherTimer();

        private static MainWindow MainWindowInstance = null;
        private MainWindowService mainWindowService = new MainWindowService();

        List<Button> btnList = new List<Button>();
        Process helperProcess;
        private QA qaWindow;
        private FPCY fpcyWin = null;
        private JXRegister jxRegister = null;
        DispatcherTimer appTimer = new DispatcherTimer();
        PDFMessage PDFMessage = new PDFMessage();

        private List<NewestInfo> _newestList;
        private List<NewestInfo> _newestList2;

        private List<GgInfo> _ggInfoList1;
        private List<GgInfo> _ggInfoList2;

        // 最新资讯
        public List<NewestInfo> NewestInfoList1
        {
            get
            {
                return _newestList;
            }
            set
            {
                _newestList = value;

                OnPropertyChanged("NewestInfoList1");
            }
        }

        public List<NewestInfo> NewestInfoList2
        {
            get
            {
                return _newestList2;
            }
            set
            {
                _newestList2 = value;

                OnPropertyChanged("NewestInfoList2");
            }
        }

        // 重要通知
        public List<GgInfo> GgInfoList1
        {
            get
            {
                return _ggInfoList1;
            }
            set
            {
                _ggInfoList1 = value;

                OnPropertyChanged("GgInfoList1");
            }
        }

        public List<GgInfo> GgInfoList2
        {
            get
            {
                return _ggInfoList2;
            }
            set
            {
                _ggInfoList2 = value;

                OnPropertyChanged("GgInfoList2");
            }
        }

        // 公告展示
        public List<GgInfo> GgInfo { get; set; }


        /// <summary>
        /// 当前窗体实例
        /// </summary>
        public static MainWindow GetMainWindowInstance
        {
            get
            {
                return MainWindowInstance;
            }
        }

        public static IntPtr Formhwnd
        {
            get; set;
        }

        public string QdUrl
        {
            get; set;
        }

        public MainWindow()
        {
            InitializeComponent();

            MainWindowInstance = this;

            if (GlobalInfo.ExistTax)
            {
                lblNsrInfo.Visibility = Visibility.Hidden;
                panelNsrInfo.Visibility = Visibility.Visible;
                tbNsrsbh.Text = GlobalInfo.NSRSBH;

                CryptTool.UserPin = GlobalInfo.JxPwd;
                CryptTool.getThisCert();
                tbNsrmc.Text = CryptTool.Nsrmc;// "北京聚达创通科技有限公司";
                GlobalInfo.NSRMC = CryptTool.Nsrmc;
            }
            else
            {
                GlobalInfo.NSRSBH = "0";
                GlobalInfo.FJH = "0";
                GlobalInfo.orgCode = "0";

                lblNsrInfo.Visibility = Visibility.Visible;
                panelNsrInfo.Visibility = Visibility.Hidden;
                lblNsrInfo.Content = PRO_ReceiptsInvMgr.Resources.Message.NotConnTaxDisk;
            }
        }

        /// <summary>
        /// 页面初始化
        /// </summary>
        private void Init()
        {
            try
            {
                this.Activate();

                GetAppInfo();

                GetAdvertiseInfo();

                DownLoadManual();

                this.lblVersion.Content = string.Format(PRO_ReceiptsInvMgr.Resources.Common.ClientVersion, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

                LoadNotifyIcon();

                CheckUpdate();

                IntervalUpdateClient();

                var btnQA = (Button)windowTemplate.FindName("btnQA", this);
                btnQA.Click += new RoutedEventHandler((o, e) =>
                {

                    if (qaWindow == null)
                    {
                        qaWindow = new QA();
                        qaWindow.Show();
                        qaWindow.Closing += delegate
                        {
                            qaWindow = null;
                        };
                    }
                    else
                    {
                        qaWindow.Activate();
                        qaWindow.WindowState = WindowState.Normal;
                    }
                });

            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(MainWindow), Message.ErrorMsg + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// 广告信息
        /// </summary>
        private void GetAdvertiseInfo()
        {
            try
            {
                adverTimer.Interval = TimeSpan.FromSeconds(double.Parse(ConfigHelper.GetAppSettingValue("AdvertiseInterval")));
                adverTimer.Tick += GetAdertiseHandler;
                adverTimer.Start();
                GetAdertiseHandler(null, null);
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(MainWindow), Message.GetAdertiesError + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }



        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetAdertiseHandler(object sender, EventArgs e)
        {
            try
            {
                AdertiseResponse advertiseResponse = mainWindowService.GetAdertiseInfo();

                var NewestInfoList = advertiseResponse.ZXZXS;
                var ggInfoList = advertiseResponse.GGZS;
                GgInfo = ggInfoList;

                if (ggInfoList != null && ggInfoList.Any())
                {
                    ggInfoList.ForEach((o) =>
                    {
                        o.GG_TITLE = CommonHelper.CutStringByte(o.GG_TITLE, 50);
                        DateTime dt;
                        var b = DateTime.TryParse(o.GG_TIME, out dt);
                        if (b)
                        {
                            o.GG_TIME = dt.ToString("yyyy-MM-dd");
                        }
                    });

                    GgInfoList1 = ggInfoList.Take(3).ToList();

                    GgInfoList2 = ggInfoList.Skip(3).Take(3).ToList();

                }

                if (NewestInfoList != null && NewestInfoList.Any())
                {
                    NewestInfoList.ForEach((o) =>
                    {
                        o.ZXMC = CommonHelper.CutStringByte(o.ZXMC, 50);
                    });

                    NewestInfoList1 = NewestInfoList.Take(3).ToList();

                    NewestInfoList2 = NewestInfoList.Skip(3).Take(3).ToList();
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(MainWindow), Message.GetAdertiesError + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// 软件升级进程
        /// </summary>
        private void IntervalUpdateClient()
        {
            Task.Factory.StartNew(() =>
            {
                UpdateClient();
            });
        }

        /// <summary>
        /// 更新客户端
        /// </summary>
        private void UpdateClient()
        {
            bool result = true;
            try
            {
                int delayTimes = Convert.ToInt32(ConfigHelper.GetAppSettingValue("UpdateCheckInterval") ?? "60") * 1000;
                while (true)
                {
                    string filePath = string.Empty;
                    result = mainWindowService.DownloadSoftware((int)LbblmType.Client, ref filePath);
                    if (result)
                    {
                        UpdateSoftware(filePath);
                    }

                    Thread.Sleep(delayTimes);
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(MainWindow), Message.ClientUpdateError + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }


        /// <summary>
        /// 更新检查
        /// </summary>
        private void CheckUpdate()
        {
            bool isForce = false;
            string downloadpath = mainWindowService.UpdateSoftware(out isForce);
            if (!string.IsNullOrEmpty(downloadpath))
            {
                var msgShow = MessageBoxEx.Show(this, Message.DownloadFile, Message.Tips, MessageBoxExButtons.YesNo, MessageBoxExIcon.Question);
                if (msgShow.HasValue && msgShow.Value)
                {
                    OpenUpdateProcess(AppDomain.CurrentDomain.BaseDirectory + downloadpath);
                }
                else
                {
                    if (isForce)
                    {
                        Environment.Exit(1);
                    }
                }
            }
        }


        /// <summary>
        /// 存在新版本，更新新版本客户端
        /// </summary>
        /// <param name="localPath"></param>
        public void UpdateSoftware(string localPath)
        {
            bool isForce = false;
            string downloadpath = mainWindowService.UpdateSoftware(out isForce);
            if (!string.IsNullOrEmpty(downloadpath))
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var msgShow = MessageBoxEx.Show(this, Message.DownloadFile, Message.Tips, MessageBoxExButtons.YesNo, MessageBoxExIcon.Question);
                    if (msgShow.HasValue && msgShow.Value)
                    {
                        OpenUpdateProcess(localPath);
                    }
                    else
                    {
                        if (isForce)
                        {
                            Environment.Exit(1);
                        }
                    }
                }));
            }
        }

        private void OpenUpdateProcess(string updateFilePath)
        {
            try
            {
                string updateResultPath = AppDomain.CurrentDomain.BaseDirectory + "Update.ini";
                MyIniFile ini = new MyIniFile(updateResultPath);
                ini.IniWriteValue("Common", "UpdateFile", updateFilePath);
                ini.IniWriteValue("UpdateResult", updateFilePath, "0");
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\PRO_ReceiptsInvMgr.UpdateApp.exe", "\"" + updateFilePath + "\"");
                CloseClient();
                System.Windows.Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(MainWindow), Message.ExitError + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }


        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Init();

            CheckInvoiceDeviceTask();
        }

        public void CheckInvoiceDeviceTask()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
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

                        string JSPName = PRO_ReceiptsInvMgr.Resources.Common.JSPDeviceName;
                        string SKPName = PRO_ReceiptsInvMgr.Resources.Common.SKPDeviceName;
                        var deviceEntities = diskModel.Where(x => x == JSPName || x == SKPName).ToList();

                        if (deviceEntities.Count == 0)
                        {
//                            var dialogResult = MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.NotFindTax, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                            Environment.Exit(1);
                        }
                        else if (deviceEntities.Count > 1)
                        {
//                            MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.MultiTaxDiskError, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                            Environment.Exit(1);
                        }
//                         else
//                         {
//                             if (deviceEntities.FirstOrDefault() == JSPName)
//                             {
//                                 GlobalInfo.DeviceType = DeviceType.JSP.GetHashCode();
//                             }
//                             else
//                             {
//                                 GlobalInfo.DeviceType = DeviceType.SKP.GetHashCode();
//                             }
//                         }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.CheckTaxDeviceError, ex);
                    }
                    Thread.Sleep(new TimeSpan(0, 0, 10));

                }
            });
        }
        /// <summary>
        /// 窗体拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }


        /// <summary>
        /// 程序状态显示最大化，最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowPage_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                NotifyMenuItemMin_Click(null, null);
            }
            else
            {
                NotifyMenuItemDisplay_Click(null, null);
            }

        }

        /// <summary>
        /// 关闭主页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowPage_Closing(object sender, CancelEventArgs e)
        {
            this.Hide();
            NotifyMenuItemMin_Click(null, null);
            e.Cancel = true;
        }


        /// <summary>
        ///  获取应用信息
        /// </summary>
        private void GetAppInfo()
        {
            stpanelApp.Children.Clear();
            GetInnerApp();

            var btnManager = new Button();
            btnManager.Style = this.FindResource("MenuButton") as Style;
            btnManager.Tag = PRO_ReceiptsInvMgr.Resources.Common.AppManger;
            btnManager.Cursor = Cursors.Hand;
            btnManager.Width = 90;
            btnManager.Height = 105;
            btnManager.Click += btnManage_Click;
            btnManager.Background = new ImageBrush(new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoAddApp)));
//            stpanelApp.Children.Add(btnManager);   wming 20190301  取消应用管理显示
        }

        private AppManager appManagerWin = null;
        /// <summary>
        /// 应用管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnManage_Click(object sender, RoutedEventArgs e)
        {
            if (appManagerWin == null)
            {
                appManagerWin = new AppManager();
                appManagerWin.GetAppAction = GetAppInfo;
                appManagerWin.Show();
                appManagerWin.Closing += new CancelEventHandler((s, args) =>
                {
                    appManagerWin = null;
                });
            }
            else
            {
                appManagerWin.Activate();
                appManagerWin.WindowState = WindowState.Normal;
            }

        }



        /// <summary>
        /// 加载固定应用
        /// </summary>
        private void GetInnerApp()
        {
            btnList = new List<Button>();
             

            var btnJXGLClient = new Button();
            btnJXGLClient.Style = this.FindResource("MenuButton") as Style;
            btnJXGLClient.Tag = "进项管理";
            btnJXGLClient.Cursor = Cursors.Hand;
            btnJXGLClient.Width = 90;
            btnJXGLClient.Height = 105;
            btnJXGLClient.Background = new ImageBrush(new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoJxglApp)));
            btnJXGLClient.UseLayoutRounding = true;

            stpanelApp.Children.Add(btnJXGLClient);

            btnJXGLClient.Click += new RoutedEventHandler((s, e) =>
            {
                if (!GlobalInfo.ExistTax)
                {
                    MessageBoxEx.Show(this, Message.NoTaxDiskTip2, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Information);
                    return;
                }

                if (JXManager.JXManagerInstance != null)
                {
                    JXManager.JXManagerInstance.Activate();
                    JXManager.JXManagerInstance.WindowState = WindowState.Normal;
                    return;
                }

                Task.Factory.StartNew(() =>
                {
                    var isRegister = false;
                    string errorMsg = string.Empty;

                    bool isOpen = false;
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        WaitingBox.Show(() =>
                        {
                            isOpen = ValieIsOpen(AppCode.JXGL);

                            if (!isOpen)
                            {
                                return;
                            }
                            isRegister = mainWindowService.IsRegister(out errorMsg);
                        }, PRO_ReceiptsInvMgr.Resources.Message.LoadingWait);

                    }));

                    if (!isOpen)
                    {
                        return;
                    }

                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (isRegister)
                        {
                            JxLogin();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(errorMsg))
                            {
                                MessageBoxEx.Show(errorMsg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                            }
                            else
                            {
                                if (jxRegister == null)
                                {
                                    jxRegister = new JXRegister();
                                    jxRegister.Show();
                                    NotifyMenuItemMin_Click(null, null);
                                    jxRegister.Closing += new CancelEventHandler((ss, args) =>
                                    {
                                        jxRegister = null;
                                    });
                                }
                                else
                                {
                                    jxRegister.Activate();
                                    jxRegister.WindowState = WindowState.Normal;
                                }
                            }
                        }
                    }));
                });
            });
            btnList.Add(btnJXGLClient);

            var btnJXClient = new Button();
            btnJXClient.Style = this.FindResource("MenuButton") as Style;
            btnJXClient.Tag = "发票查验";
            btnJXClient.Cursor = Cursors.Hand;
            btnJXClient.Width = 90;
            btnJXClient.Height = 105;
            btnJXClient.Background = new ImageBrush(new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoFpcyApp)));
            btnJXClient.UseLayoutRounding = true;

            stpanelApp.Children.Add(btnJXClient);

            btnJXClient.Click += new RoutedEventHandler((s, e) =>
            {
                if (fpcyWin == null)
                {
                    bool isOpen = false;
                    WaitingBox.Show(() =>
                    {
                        isOpen = ValieIsOpen(AppCode.FPCY);
                    }, PRO_ReceiptsInvMgr.Resources.Message.LoadingWait);

                    if (!isOpen)
                    {
                        return;
                    }
                    fpcyWin = new FPCY();
                    fpcyWin.Show();
                    NotifyMenuItemMin_Click(null, null);
                    fpcyWin.Closing += new CancelEventHandler((ss, args) =>
                    {
                        NotifyMenuItemRestore_Click(null, null);
                        fpcyWin = null;
                    });
                }
                else
                {
                    fpcyWin.Activate();
                    fpcyWin.WindowState = WindowState.Normal;
                }
            });
            btnList.Add(btnJXClient);

         
        }

        private JXManagerService jxManageService = new JXManagerService();

        /// <summary>
        /// 进项管理登录
        /// </summary>
        private void JxLogin()
        {
            string token = string.Empty, ErrorMsg = string.Empty, errCode = string.Empty;

            this.Dispatcher.Invoke(new Action(() =>
            {
                WaitingBox.Show(() =>
                {
                    //新版本调用大象token.dll取token 20190309
                    int retryCount = 3;

                    do 
                    {
                        --retryCount;
                        token = GetTokenHelper.GetToken_dll(GlobalInfo.NSRSBH, GlobalInfo.JxPwd, GlobalInfo.Dqdm, "3.2.01");

                        errCode = GetTokenHelper.retCode;
                        ErrorMsg = GetTokenHelper.ErrorMsg;
                    } while (errCode!="0000" && retryCount>0);

                }, PRO_ReceiptsInvMgr.Resources.Message.loginWait);
            }));

            if (errCode != "0000")
            {
                if (ErrorMsg.Contains("0xA7"))
                {
                    ErrorMsg = PRO_ReceiptsInvMgr.Resources.Message.NotFindTax;
                }
                MessageBoxEx.Show(this, ErrorMsg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
            }
            else
            {
                GlobalInfo.token = token;
                JXManager win = new JXManager();
                win.Show();
            }
        }

        private bool ValieIsOpen(AppCode appcode)
        {
            string errorMsg = string.Empty;

            AppSate appState = mainWindowService.GetAppIsOpen(appcode, out errorMsg);

            if (!string.IsNullOrEmpty(errorMsg))
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBoxEx.Show(this, errorMsg, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                }));

                return false;
            }

            if (appState == AppSate.NotOpen)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.AppNotOpen, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                }));
                return false;
            }
            else if (appState == AppSate.OverTime)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.AppOverTime, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                }));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 应用启动Timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppTimer_Tick(object sender, EventArgs e)
        {
            Window win = null;
            try
            {
                foreach (Window w in App.Current.Windows)
                {
                    if (w is PDFMessage)
                    {
                        win = w;
                    }
                }
                //关闭客户端
                Process[] ps = Process.GetProcessesByName(appTimer.Tag.ToString());
                if (ps.Length > 0)
                {
                    if (win != null)
                    {
                        win.Close();
                    }
                    appTimer.Stop();
                }
            }
            catch
            {
                if (win != null)
                {
                    win.Close();
                    appTimer.Stop();
                }

            }

        }



        /// <summary>
        /// 相关链接点击事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_Click(object sender, EventArgs e)
        {
            var hyperLink = sender as Hyperlink;
            Process.Start(hyperLink.NavigateUri.ToString());
        }

        /// <summary>
        /// 重要通知点击事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Gginfo_Click(object sender, EventArgs e)
        {
            var obj = sender as Hyperlink;
            var selectId = obj.Tag as string;

            var ggInfo = GgInfo.FirstOrDefault(x => x.ID == selectId);
            if (ggInfo != null)
            {
                NoticeWindow noticeWindow = new NoticeWindow();
                noticeWindow.GgInfo = ggInfo;
                noticeWindow.Owner = this;
                noticeWindow.ShowDialog();
            }
        }


        /// <summary>
        /// 资讯、公告Menu点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Advertise_Click(object sender, RoutedEventArgs e)
        {
            InitMenu();

            var btn = sender as Button;
            Border b = btn.Template.FindName("ContentContainer", btn) as Border;
            b.BorderThickness = new Thickness(0, 2, 0, 0);
            b.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0xCB, 0xA4, 0x60));
            Label lbl = b.Child as Label;
            lbl.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xCB, 0xA4, 0x60));
            if (btn == btnNewest)
            {
                newestGrid.Visibility = Visibility.Visible;
                ggGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                newestGrid.Visibility = Visibility.Hidden;
                ggGrid.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 设置资讯、公告Menu
        /// </summary>
        /// <param name="btn"></param>
        private void setMenuImage(Button btn)
        {
            Border b = btn.Template.FindName("ContentContainer", btn) as Border;
            b.BorderThickness = new Thickness(0, 2, 0, 0);
            b.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xCB, 0xA4, 0x60));
            Label lbl = b.Child as Label;
            lbl.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x9B, 0xA3, 0xB0));
        }

        /// <summary>
        /// 初始化资讯、公告Menu
        /// </summary>
        /// <param name="btn"></param>
        private void InitMenu()
        {
            setMenuImage(btnNewest);
            setMenuImage(btnGg);
        }


        /// <summary>
        /// 咨询链接鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hyperLink_MouseEnter(object sender, MouseEventArgs e)
        {
            var tb = sender as Hyperlink;
            TextDecoration myUnderline = new TextDecoration();
            Pen myPen = new Pen();
            myPen.Brush = new SolidColorBrush(Color.FromArgb(0xff, 0, 0, 0));
            TextDecorationCollection myCollection = new TextDecorationCollection();
            myCollection.Add(myUnderline);
            myUnderline.Pen = myPen;
            tb.TextDecorations = myCollection;
        }

        /// <summary>
        /// 咨询链接鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hyperLink_MouseLeave(object sender, MouseEventArgs e)
        {
            var tb = sender as Hyperlink;
            tb.TextDecorations = null;
        }


        /// <summary>
        /// 打开用户手册页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hlk_Click(object sender, RoutedEventArgs e)
        {

            bool isOpen = false;
            if (helperProcess != null && !helperProcess.HasExited)
            {
                isOpen = true;
                IntPtr handle = helperProcess.MainWindowHandle;
                SwitchToThisWindow(handle, true);
            }

            if (!isOpen)
            {
                helperProcess = Process.Start(AppDomain.CurrentDomain.BaseDirectory + PRO_ReceiptsInvMgr.Resources.Common.ManualFilename);
            }
        }


        /// <summary>
        /// 鼠标滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var viewer = sender as ScrollViewer;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            viewer.RaiseEvent(eventArg);
        }

        /// <summary>
        /// 用户手册下载
        /// </summary>
        private void DownLoadManual()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    mainWindowService.DownloadManual();
                    Thread.Sleep(new TimeSpan(1, 0, 0));
                }
            });
        }
    }
}