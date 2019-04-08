using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Client.UI;
using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Core.Utilites;
using PRO_ReceiptsInvMgr.Domain;
using PRO_ReceiptsInvMgr.Domain.Enum;
using PRO_ReceiptsInvMgr.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRO_ReceiptsInvMgr.Client
{
    /// <summary>
    /// Interaction logic for ProcessBarTest.xaml
    /// </summary>
    public partial class AppDownload : Page, INotifyPropertyChanged
    {
        public Action GetAppAction { get; set; }
        private Dictionary<int, Thread> dicMap = new Dictionary<int, Thread>();
        private Dictionary<int, bool> dicIsStop = new Dictionary<int, bool>();
        AppManagerService service = new AppManagerService();
        private List<AppInfoObject> _appList = new List<AppInfoObject>();
        public event PropertyChangedEventHandler PropertyChanged;
        public List<AppInfoObject> AppList
        {
            get
            {
                return _appList;
            }
            set
            {
                _appList = value;

                //触发事件
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("AppList"));
                }
            }
        }

        public Dictionary<int, Thread> DicMap
        {
            get
            {
                return dicMap;
            }

            set
            {
                dicMap = value;
            }
        }

        public Dictionary<int, bool> DicIsStop
        {
            get
            {
                return dicIsStop;
            }

            set
            {
                dicIsStop = value;
            }
        }

        public AppDownload()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            scrollViewer1.RaiseEvent(eventArg);
        }


        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {

            DataGridTemplateColumn column = this.grdList.Columns[0] as DataGridTemplateColumn;
            //获取指定的行与列相交位置的单元格
            FrameworkElement element = column.GetCellContent(this.grdList.Items[this.grdList.SelectedIndex]);
            var bar = column.CellTemplate.FindName("progressCtl", element) as ProgressBar;
            var btnShow = column.CellTemplate.FindName("btnShow", element) as Button;
            var btnDownload = sender as Button;
            Border b = btnDownload.Template.FindName("ContentContainer", btnDownload) as Border;
            Button btnCancel = column.CellTemplate.FindName("btnCancel", element) as Button;

            string filePath = btnShow.Tag != null ? btnShow.Tag.ToString() : string.Empty;

            if (string.IsNullOrEmpty(filePath))
            {
                var m_Dialog = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = m_Dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                filePath = m_Dialog.SelectedPath.Trim();
            }

            var index = this.grdList.SelectedIndex;
            var rowData = AppList[index];

            (sender as Button).IsEnabled = false;

            string downFoldar = filePath;
            btnShow.Tag = filePath;
            if (!Directory.Exists(downFoldar))
            {
                Directory.CreateDirectory(downFoldar);
            }

            Thread td = new Thread(() =>
            {
                DownloadHelper downLoadHelper = null;
                try
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        btnCancel.Visibility = Visibility.Visible;
                        btnDownload.Visibility = Visibility.Hidden;
                    }));

                    string serverFileUrl = rowData.DownUrl;
                    downLoadHelper = new DownloadHelper(serverFileUrl, downFoldar);
                    downLoadHelper.GetTotalSize();
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        bar.Maximum = downLoadHelper.TotalSize;
                    }));

                    string downLoadStep = ConfigHelper.GetAppSettingValue("DownloadStep");

                    downLoadHelper.Step = !string.IsNullOrEmpty(downLoadStep) ? Convert.ToInt32(downLoadStep) : 102400;

                    while (!downLoadHelper.IsFinished && !DicIsStop[index])
                    {
                        downLoadHelper.Download();
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            bar.Visibility = Visibility.Visible;
                            bar.Value = downLoadHelper.CurrentSize;
                        }));
                    }

                    if (downLoadHelper.IsFinished)
                    {
                        string localFileName = downLoadHelper.FilePath;
                        string localMd5 = CommonHelper.GetMD5HashFromFile(localFileName);

                        if (localMd5 == rowData.MD5)
                        {
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    btnDownload.Visibility = Visibility.Hidden;
                                    btnShow.Visibility = Visibility.Visible;
                                    btnCancel.Visibility = Visibility.Hidden;
                                }));
                            }));

                        }
                        else
                        {
                            File.Delete(localFileName);
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                MessageBoxEx.Show(this, string.Format(Message.DownFail, rowData.AppName), Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);

                                btnDownload.Visibility = Visibility.Visible;
                                b.Background = new ImageBrush
                                {
                                    ImageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoDownRestart))
                                };
                                btnDownload.IsEnabled = true;
                                btnShow.Visibility = Visibility.Hidden;
                                btnCancel.Visibility = Visibility.Hidden;
                                bar.Visibility = Visibility.Hidden;
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is System.Threading.ThreadAbortException))
                    {
                        string errMsg = PRO_ReceiptsInvMgr.Resources.Message.NetworkError;

                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            MessageBoxEx.Show(this, errMsg, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);

                            btnCancel.Visibility = Visibility.Hidden;
                            bar.Visibility = Visibility.Hidden;
                            btnDownload.Visibility = Visibility.Visible;
                            btnDownload.IsEnabled = true;
                        }));
                        Logging.Log4NetHelper.Error(typeof(AppDownload), string.Format(PRO_ReceiptsInvMgr.Resources.Message.DownAppFail, rowData.AppName) + ex.Message + System.Environment.NewLine + ex.StackTrace);
                    }
                }
                finally
                {
                    if (DicMap.Keys.Contains(index))
                    {
                        DicMap.Remove(index);
                    }
                    if (downLoadHelper != null)
                    {
                        downLoadHelper.Dispose();
                    }
                }
            });

            td.IsBackground = true;
            td.Start();

            DicMap.Add(index, td);

            if (!DicIsStop.ContainsKey(index))
            {
                DicIsStop.Add(index, false);
            }
            else
            {
                DicIsStop[index] = false;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    bool isSuccess = false;
                    var appResponse = service.GetAppInfoResponse(out isSuccess);
                    if (isSuccess)
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            gifLoading.Visibility = Visibility.Collapsed;
                            imgTip.Visibility = Visibility.Hidden;
                            grdList.Visibility = Visibility.Visible;
                        }));

                        var wsAppList = appResponse.CYYYS;
                        if (wsAppList != null && wsAppList.Any())
                        {
                            AppList = ResponseToData(wsAppList);
                            AppList.ForEach((app) =>
                            {
                                app.btnImgUrl = PRO_ReceiptsInvMgr.Resources.Common.IcoDown;
                            });
                        }
                        else
                        {
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                imgTip.Visibility = Visibility.Visible;
                                grdList.Visibility = Visibility.Hidden;
                                imgTip.Source = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoDownNoExist, UriKind.Relative));
                            }));
                        }
                    }
                    else
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            gifLoading.Visibility = Visibility.Collapsed;
                            imgTip.Visibility = Visibility.Visible;
                            grdList.Visibility = Visibility.Hidden;
                            imgTip.Source = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoDownNetError, UriKind.Relative));
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log4NetHelper.Error(typeof(AppDownload), Message.GetAppFail + ex.Message + System.Environment.NewLine + ex.StackTrace);
                }
            });
        }


        private List<AppInfoObject> ResponseToData(List<WSAppInfo> listResponse)
        {
            List<AppInfoObject> appInfoObjectList = new List<AppInfoObject>();
            if (listResponse != null)
            {
                foreach (var item in listResponse)
                {
                    var appInfoObject = new AppInfoObject();
                    appInfoObject.AppCode = item.ID.ToString();
                    appInfoObject.AppName = item.YYNAME;
                    appInfoObject.Ico = SaveIco(item.ID.ToString(), item.YYTPIO);
                    appInfoObject.AppDescription = item.YYMS;
                    appInfoObject.DownUrl = item.CYYYURL;
                    appInfoObject.MD5 = item.MD5;
                    appInfoObjectList.Add(appInfoObject);
                }
            }
            return appInfoObjectList;
        }

        /// <summary>
        /// 保存ICO
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="baseString">base64文件流</param>
        /// <returns></returns>
        private string SaveIco(string id, string baseString)
        {
            string filename = AppDomain.CurrentDomain.BaseDirectory + "AppIco\\" + id + ".ico";
            byte[] b = Convert.FromBase64String(baseString);
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "AppIco"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "AppIco");
            }
            if (!File.Exists(filename))
            {
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                fs.Write(b, 0, b.Length);
                fs.Close();
                fs.Dispose();
            }
            return filename;
        }


        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DataGridTemplateColumn column = this.grdList.Columns[0] as DataGridTemplateColumn;
            FrameworkElement element = column.GetCellContent(this.grdList.Items[this.grdList.SelectedIndex]);
            var bar = column.CellTemplate.FindName("progressCtl", element) as ProgressBar;
            var btnCancel = sender as Button;
            Button btnDownload = column.CellTemplate.FindName("btnDownload", element) as Button;
            var lblCancel = column.CellTemplate.FindName("lblCancel", element) as Label;

            var index = this.grdList.SelectedIndex;
            Task.Factory.StartNew(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    btnCancel.Visibility = Visibility.Hidden;
                    lblCancel.Visibility = Visibility.Visible;
                    bar.Visibility = Visibility.Hidden;
                }));

                if (DicMap.Keys.Contains(index))
                {
                    Thread td = DicMap[index];
                    DicMap.Remove(index);
                    DicIsStop[index] = true;
                    Thread.Sleep(500);
                    td.Abort();
                }

                this.Dispatcher.Invoke(new Action(() =>
                {
                    lblCancel.Visibility = Visibility.Hidden;
                    btnDownload.Visibility = Visibility.Visible;
                    btnDownload.IsEnabled = true;
                }));
            });
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn.Tag != null)
            {
                Process.Start(btn.Tag.ToString());
            }
        }
    }
}
