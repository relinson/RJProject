﻿//#define USE_DEB

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
using PRO_ReceiptsInvMgr.Core.Utilites;
using System.Text.RegularExpressions;
using PRO_ReceiptsInvMgr.Client.Helper;

namespace PRO_ReceiptsInvMgr.Client.UI.JXGL
{
    public partial class JXSmrz : Page
    {
        public GxrzViewModel SmrzViewModelInstance { get; set; }
        List<JXInvoiceInfo> invoiceList = new List<JXInvoiceInfo>();
        List<JXInvoiceInfo> pageList = new List<JXInvoiceInfo>();
        JXManagerService service = new JXManagerService();

        public JXSmrz()
        {
            InitializeComponent();
        }

        private void GXRZPage_Loaded(object sender, RoutedEventArgs e)
        {
/*            this.DateInvoiceStart.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(1), DateTime.MaxValue));
            this.DateInvoiceEnd.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(1), DateTime.MaxValue));
*/
            SmrzViewModelInstance = new GxrzViewModel();
            SmrzViewModelInstance.QueryModel = new GxrzQueryModel();
            this.DataContext = SmrzViewModelInstance;

            //发票状态初始化
 /*           string[] combValues = { "正常", "失控", "作废", "红冲", "异常", "全部" };
            cbxInvoiceState.ItemsSource = combValues;
            cbxInvoiceState.SelectedIndex = 0;*/


            Task.Factory.StartNew(() =>
            {
                string errorMsg = string.Empty;
                GlobalInfo.skssq = service.GetJXSsq(out errorMsg);

                //20190409 取数据失败时，如果是token过期则重新获取并重试
                if (errorMsg.Contains("token过期"))
                {
                    int retryCount = 3;
                    do
                    {
                        --retryCount;
                        GlobalInfo.token = GetTokenHelper.GetToken_dll(GlobalInfo.NSRSBH, GlobalInfo.JxPwd, GlobalInfo.Dqdm);
                    } while (GlobalInfo.token.Length == 0 && retryCount > 0);

                    GlobalInfo.skssq = service.GetJXSsq(out errorMsg);
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        MessageBoxEx.Show(JXManager.JXManagerInstance, errorMsg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                    }));
                    return;
                }
                else
                {
                    SmrzViewModelInstance.Skssq = GlobalInfo.skssq;
                }

                string[] strDateArray = service.GetJXKpStartEndDate(out errorMsg);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        MessageBoxEx.Show(JXManager.JXManagerInstance, errorMsg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                    }));
                }
                else
                {
                    SmrzViewModelInstance.SelectStartDate = strDateArray[0];
                    SmrzViewModelInstance.SelectEndDate = strDateArray[1];

/*                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                         btnQuery_Click(null, null);
                    }));*/
                }
            });

            //GetInvoiceStatusTask();

        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SmrzViewModelInstance.Skssq))
            {
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.SkssqNotLoaded, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(SmrzViewModelInstance.SelectStartDate))
            {
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.SelectStartDateNotLoaded, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(SmrzViewModelInstance.SelectEndDate))
            {
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.SelectEndDateNotLoaded, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

/*            SmrzViewModelInstance.QueryModel.FPZT = cbxInvoiceState.SelectedIndex.ToString();
            if(5 == cbxInvoiceState.SelectedIndex)
            {
                SmrzViewModelInstance.QueryModel.FPZT = "9";
            }*/

            if (!string.IsNullOrEmpty(SmrzViewModelInstance.QueryModel.SE))
            {
                Regex reg = new Regex(@"^\d+(\.\d+)?$");
                if (!reg.IsMatch(SmrzViewModelInstance.QueryModel.SE))
                {
                    MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.QuerySeError, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                    return;
                }
            }

/*            if (DateInvoiceStart.SelectedDate.HasValue && DateInvoiceEnd.SelectedDate.HasValue &&
                DateInvoiceStart.SelectedDate.Value.CompareTo(DateInvoiceEnd.SelectedDate.Value) > 0)
            {
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.QueryDateError, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }*/

            SmrzViewModelInstance.IsAllChecked = false;
            imgTip.Visibility = Visibility.Collapsed;
            pcPage.Visibility = Visibility.Collapsed;
            pcPage.CrrentPage = 1; //每次查询时返回第一页
            gifLoading.Visibility = Visibility.Visible;
            SmrzViewModelInstance.InvoiceList = new ObservableCollection<JXInvoiceInfo>();
            Task.Factory.StartNew(() =>
            {
                string msg = string.Empty;
                int totalCount = 0;

                invoiceList = service.GetJXData(SmrzViewModelInstance.QueryModel, out totalCount, out msg);
                //20190324 取数据失败时，如果是token过期则重新获取并重试
                if (msg.Contains("token过期"))
                {
                    int retryCount = 3;
                    do
                    {
                        --retryCount;
                        GlobalInfo.token = GetTokenHelper.GetToken_dll(GlobalInfo.NSRSBH, GlobalInfo.JxPwd, GlobalInfo.Dqdm);
                    } while (GlobalInfo.token.Length == 0 && retryCount > 0);

                    invoiceList = service.GetJXData(SmrzViewModelInstance.QueryModel, out totalCount, out msg);
                }

                SmrzViewModelInstance.InvoiceList = new ObservableCollection<JXInvoiceInfo>(invoiceList);
                SmrzViewModelInstance.YqTipCounts = SmrzViewModelInstance.InvoiceList.Count(x => x.YQTXBZ);

                this.Dispatcher.BeginInvoke(new Action(() =>
                {

                    splYqRemain.Visibility = SmrzViewModelInstance.YqTipCounts > 0 ? Visibility.Visible : Visibility.Collapsed;

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
                    CalcTotalChecked();
                }));
            });
        }


        private void cbxSelectAll_CheckChanged(object sender, RoutedEventArgs e)
        {
            var cbx = sender as CheckBox;
            if (cbx != null)
            {
                pageList.ForEach(x => x.IsChecked = cbx.IsChecked.Value);
            }
            CalcTotalChecked();
        }

        private void pcPage_DataSourcePageSize(object sender, EventArgs e)
        {
            pageList = invoiceList.Skip((pcPage.CrrentPage - 1) * pcPage.PageSize).Take(pcPage.PageSize).ToList();
            SmrzViewModelInstance.InvoiceList = new ObservableCollection<JXInvoiceInfo>(pageList);
        }

        private void grdList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var row = grdList.SelectedItem as JXInvoiceInfo;
            row.IsChecked = !row.IsChecked;
        }

        /// <summary>
        /// 计算已选发票总额
        /// </summary>
        private void CalcTotalChecked()
        {
            var checkedList = invoiceList.Where(x => x.IsChecked);
            var totalAmount = checkedList.Sum(x => x.HJBHSJE).Value;
            var totalSe = checkedList.Sum(x => x.SE).Value;
            SmrzViewModelInstance.TotalAmount = totalAmount.ToString("f2");
            SmrzViewModelInstance.TotalSE = totalSe.ToString("f2");
        }

        private void btnGXRZ_Click(object sender, RoutedEventArgs e)
        {
            var checkedList = invoiceList.Where(x => x.IsChecked).ToList();
            int count = checkedList.Count;
            if (count == 0)
            {
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.SelectGXInvoice, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Information);
                return;
            }
            else if (count > 60)
            {
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.OutOfFpCount, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            string confirmTip = string.Format("勾选总张数：{0} 张  总金额：{1} 元  总税额：{2} 元", count, SmrzViewModelInstance.TotalAmount, SmrzViewModelInstance.TotalSE);
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
                            if (msg.Contains("token过期"))
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

        private void grdChx_CheckChanged(object sender, RoutedEventArgs e)
        {
            CalcTotalChecked();
        }


        /// <summary>
        /// 获取发票状态
        /// </summary>
        public void GetInvoiceStatusTask()
        {

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    string fplb = string.Empty;
                    foreach (var item in invoiceList)
                    {
                        fplb += string.Format("{0},{1};", item.InvoiceCode, item.InvoiceNo);
                    }

                    if (!string.IsNullOrEmpty(fplb))
                    {
                        fplb = fplb.Substring(0, fplb.Length - 1);
                        List<JXInvoiceStatusContent> invoiceStatusList = service.GetInvoiceStatus(fplb);
                        foreach (var item in invoiceStatusList)
                        {
                            var data = invoiceList.FirstOrDefault(x => x.InvoiceCode == item.invoiceCode && x.InvoiceNo == item.invoiceNo);
                            if (data != null)
                            {
                                if (!string.IsNullOrEmpty(item.fpzt) && item.fpzt != "-1")
                                {
                                    int fpzt = 0;
                                    Int32.TryParse(item.fpzt, out fpzt);
                                    data.InvoiceStateDesc = ((InvoiceStatus)fpzt).GetDescription();

                                }
                            }
                        }
                    }
                    Thread.Sleep(new TimeSpan(0, 0, 10));

                }
            });
        }

/*        private void cbxInvoiceState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxInvoiceState.SelectedIndex != -1)
            {
                string fpzt = cbxInvoiceState.SelectedValue.ToString();
            }
            else
            {
                cbxInvoiceState.SelectedIndex = 0;
            }
        }*/
    }
}