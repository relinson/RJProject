using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Client.Resources.xskin;
using PRO_ReceiptsInvMgr.Application.ViewModel;
using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using PRO_ReceiptsInvMgr.Domain.Enum;
using PRO_ReceiptsInvMgr.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Client.Helper;

namespace PRO_ReceiptsInvMgr.Client.UI.JXGL
{
    /// <summary>
    /// Interaction logic for JXYqyj.xaml
    /// </summary>
    public partial class JXYqyj : Page
    {
        public YqyjViewModel YqyjViewModelInstance { get; set; }
        List<JXInvoiceInfo> invoiceList = new List<JXInvoiceInfo>();
        JXManagerService service = new JXManagerService();

        public JXYqyj()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            YqyjViewModelInstance.IsAllChecked = false;
            imgTip.Visibility = Visibility.Collapsed;
            pcPage.Visibility = Visibility.Collapsed;
            pcPage.CrrentPage = 1; //每次查询时返回第一页
            gifLoading.Visibility = Visibility.Visible;
            YqyjViewModelInstance.InvoiceList = new ObservableCollection<JXInvoiceInfo>();
            Task.Factory.StartNew(() =>
            {
                string msg = string.Empty;
                int totalCount = 0;

                invoiceList = service.GetJXYQData(YqyjViewModelInstance.QueryModel, out totalCount, out msg);
                YqyjViewModelInstance.ListCount = totalCount;

                YqyjViewModelInstance.InvoiceList = new ObservableCollection<JXInvoiceInfo>(invoiceList);

                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    pcPage.TotalCount = totalCount;

                    if (!string.IsNullOrEmpty(msg))
                    {
                        MessageBoxEx.Show(JXManager.JXManagerInstance, msg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                    }

                    gifLoading.Visibility = Visibility.Collapsed;
                    if (invoiceList.Count > 0)
                    {
                        pcPage.Visibility = Visibility.Visible;
                        pcPage_DataSourcePageSize(null, null);
                    }
                    else
                    {
                        imgTip.Visibility = Visibility.Visible;
                    }
                }));
            });
        }
        private void pcPage_DataSourcePageSize(object sender, EventArgs e)
        {
            var list = invoiceList.Skip((pcPage.CrrentPage - 1) * pcPage.PageSize).Take(pcPage.PageSize).ToList();
            YqyjViewModelInstance.InvoiceList = new ObservableCollection<JXInvoiceInfo>(list);
        }

        private void grdList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var row = grdList.SelectedItem as JXInvoiceInfo;
            row.IsChecked = !row.IsChecked;
        }

        private void YQYJPage_Loaded(object sender, RoutedEventArgs e)
        {
            int[] monthArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            cbxMonth.ItemsSource = monthArray;

            imgTip.Visibility = Visibility.Visible;

            YqyjViewModelInstance = new YqyjViewModel();
            YqyjViewModelInstance.QueryModel = new YqyjQueryViewModel();
            this.DataContext = YqyjViewModelInstance;

            string errMsg = string.Empty;
            int warnMonth = service.GetYqWarnMonth(out errMsg);
            if (warnMonth > 0)
            {
                YqyjViewModelInstance.QueryModel.Month = warnMonth;
                btnQuery_Click(null, null);
            }
        }

        private void cbxSelectAll_CheckChanged(object sender, RoutedEventArgs e)
        {
            var cbx = sender as CheckBox;
            if (cbx != null)
            {
                invoiceList.ForEach(x => x.IsChecked = cbx.IsChecked.Value);
            }
        }
        private void btnGXRZ_Click(object sender, RoutedEventArgs e)
        {
            int count = invoiceList.Count(x => x.IsChecked);
            if (count == 0)
            {
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.SelectGXInvoice, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Information);
                return;
            }
            else if (count > 20)
            {
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.OutOfFpCount, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            var checkedList = invoiceList.Where(x => x.IsChecked).ToList();
            var totalAmount = checkedList.Sum(x => x.HJBHSJE).Value;
            var totalSe = checkedList.Sum(x => x.SE).Value;

            YqyjViewModelInstance.TotalAmount = totalAmount.ToString("f2");
            YqyjViewModelInstance.TotalSE = totalSe.ToString("f2");

            string confirmTip = string.Format("勾选总张数：{0} 张  总金额：{1} 元  总税额：{2} 元", count, YqyjViewModelInstance.TotalAmount, YqyjViewModelInstance.TotalSE);
            GxrzConfirm confirmWin = new GxrzConfirm();
            confirmWin.ConfirmTip = confirmTip;
            confirmWin.SelectedList = checkedList;
            confirmWin.DoRzAction = () =>
            {
                Task.Factory.StartNew(() =>
                {
                    string msg = string.Empty;
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        WaitingBox.Show(() =>
                        {
                            service.GXRZ(checkedList, out msg);
                            //20190409 取数据失败时，如果是token过期则重新获取并重试
                            if (msg.Contains("(token过期)"))
                            {
                                int retryCount = 3;
                                do
                                {
                                    --retryCount;
                                    GlobalInfo.token = GetTokenHelper.GetToken_dll(GlobalInfo.NSRSBH, GlobalInfo.JxPwd, GlobalInfo.Dqdm);
                                } while (GlobalInfo.token.Length == 0 && retryCount > 0);

                                service.GXRZ(checkedList, out msg);
                            }
                        }, PRO_ReceiptsInvMgr.Resources.Message.GxrzWait);
                    }));
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!string.IsNullOrEmpty(msg))
                        {
                            MessageBoxEx.Show(JXManager.JXManagerInstance, msg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                        }
                        btnQuery_Click(null, null);
                    }));
                });
            };
            confirmWin.Owner = JXManager.JXManagerInstance;
            confirmWin.ShowDialog();
             
        }

        private void cbxMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbx = sender as ComboBox;
            if (cbx != null && cbx.SelectedValue != null)
            {
                service.SetYqWarnMonth(cbx.SelectedValue.ToString());
            }
        }
    }

}
