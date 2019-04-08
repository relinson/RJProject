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

namespace PRO_ReceiptsInvMgr.Client.UI.FPCX
{
    /// <summary>
    /// Interaction logic for FPCY_CX.xaml
    /// </summary>
    public partial class FPCY : BaseWindow
    {
        public FpcyViewModel fpcyViewModelInstance { get; set; }
        List<FpcyDataObject> list;
        QueryModel queryModel = new QueryModel();
        FpcyService fpcyService = new FpcyService();
        CYJG_01 win1 = null;
        CYJG_02 win2 = null;
        CYJG_03 win3 = null;
//        CYJG_04 win4 = null;
        CYJG_JS winJs = null;

        public FPCY()
        {
            InitializeComponent();

            list = new List<FpcyDataObject>();
            this.DataContext = new FpcyViewModel();
            fpcyViewModelInstance = this.DataContext as FpcyViewModel;

            List<InvoiceType> invoiceTypeList = EnumHelper.EnumToList<FpcyInvoiceType>().
                    Select(x => new InvoiceType { Name = x.Desction.Replace("（", "(").Replace("）", ")"), Value = x.EnumValue.ToString().PadLeft(2, '0') }).ToList();

            cbxType.ItemsSource = invoiceTypeList;
            cbxType.DisplayMemberPath = "Name";
            cbxType.SelectedValuePath = "Value";

            string errorMsg = string.Empty;
            list = fpcyService.GetFPCYData(ref errorMsg);
            this.grdList.ItemsSource = list;
            fpcyViewModelInstance.ListCount = list.Count;
            fpcyViewModelInstance.QueryModel = queryModel;

            if (list.Count == 0)
            {
                imgTip.Visibility = Visibility.Visible;
            }
            else
            {
                imgTip.Visibility = Visibility.Hidden;
            }

            this.DateInvoice.BlackoutDates.Add(new CalendarDateRange(DateTime.Now, DateTime.MaxValue));
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {

            var btn = sender as Button;
            btn.IsEnabled = false;

            try
            {
                if (!ValidQuery())
                {
                    btn.IsEnabled = true;
                    return;
                }

                fpcyViewModelInstance.QueryModel.taxNo = " ";
                string invoiceDate = fpcyViewModelInstance.QueryModel.InvoiceDate;
                fpcyViewModelInstance.QueryModel.InvoiceDate = DateTime.Parse(invoiceDate).ToString("yyyyMMdd");
                string strRequest = new JavaScriptSerializer().Serialize(fpcyViewModelInstance.QueryModel);
                fpcyViewModelInstance.QueryModel.InvoiceDate = invoiceDate;

                string retMsg = string.Empty;
                var fpcyResult = FpcyResult.FailNotSave;
                Task.Factory.StartNew(() =>
                 {
                     this.Dispatcher.Invoke(new Action(() =>
                     {
                         WaitingBox.Show(() =>
                         {
                             fpcyResult = fpcyService.DoFPZY(strRequest, ref retMsg);
                         }, PRO_ReceiptsInvMgr.Resources.Message.SearchWaitTip);
                     }));

                     if (fpcyResult == FpcyResult.FailAndSave || fpcyResult == FpcyResult.FailNotSave)
                     {
                         this.Dispatcher.Invoke(new Action(() =>
                         {
                             MessageBoxEx.Show(this, retMsg, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                         }));
                     }

                     if (fpcyResult == FpcyResult.SuccessAndSave || fpcyResult == FpcyResult.FailAndSave)
                     {
                         string errorMsg = string.Empty;
                         this.Dispatcher.Invoke(new Action(() =>
                         {
                             list = fpcyService.GetFPCYData(ref errorMsg);

                             this.grdList.ItemsSource = list;
                             fpcyViewModelInstance.ListCount = list.Count;

                             if (list.Count == 0)
                             {
                                 imgTip.Visibility = Visibility.Visible;
                             }
                             else
                             {
                                 imgTip.Visibility = Visibility.Hidden;
                             }

                             if (!string.IsNullOrEmpty(errorMsg))
                             {
                                 MessageBoxEx.Show(this, errorMsg, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                             }
                             else if (fpcyResult == FpcyResult.SuccessAndSave)
                             {
                                 this.grdList.SelectedIndex = 0;
                                 BtnFPDetail_Click(null, null);
                             }
                         }));
                     }

                     this.Dispatcher.BeginInvoke(new Action(() =>
                     {
                         btn.IsEnabled = true;
                     }));
                 });
            }
            catch
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.FpcySearchError, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
            }
        }

        public bool ValidQuery()
        {
            if (string.IsNullOrEmpty(fpcyViewModelInstance.QueryModel.InvoiceType))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.InvoiceTypeNotEmpty, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(fpcyViewModelInstance.QueryModel.InvoiceCode))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.InvoiceCodeNotEmpty, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(fpcyViewModelInstance.QueryModel.InvoiceNo))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.InvoiceNoNotEmpty, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(fpcyViewModelInstance.QueryModel.InvoiceDate))
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.InvoiceDateNotEmpty, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(fpcyViewModelInstance.QueryModel.InvoiceAmount) && fpcyViewModelInstance.QueryModel.InvoiceAmountVisibility == Visibility.Visible)
            {
                MessageBoxEx.Show(this, PRO_ReceiptsInvMgr.Resources.Message.InvoiceAmountNotEmpty, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(fpcyViewModelInstance.QueryModel.CheckCode) && fpcyViewModelInstance.QueryModel.CheckCodeVisibility == Visibility.Visible)
            {
                MessageBoxEx.Show(PRO_ReceiptsInvMgr.Resources.Message.CheckCodeNotEmpty, Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return false;
            }
            return true;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            fpcyViewModelInstance.QueryModel.InvoiceAmount = "";
            fpcyViewModelInstance.QueryModel.InvoiceCode = "";
            fpcyViewModelInstance.QueryModel.InvoiceDate = "";
            fpcyViewModelInstance.QueryModel.InvoiceNo = "";
            fpcyViewModelInstance.QueryModel.InvoiceType = "";
            fpcyViewModelInstance.QueryModel.CheckCode = "";
            DateInvoice.Text = "";
        }

        private void BtnFPDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var index = this.grdList.SelectedIndex;
                var rowData = list[index];
                var invoiceType = (FpcyInvoiceType)Convert.ToInt32(rowData.InvoiceType);

                if (win1 != null)
                {
                    win1.Close();
                }
                if (win2 != null)
                {
                    win2.Close();
                }
                if (win3 != null)
                {
                    win3.Close();
                }
                if (winJs != null)
                {
                    winJs.Close();
                }

                if (invoiceType == FpcyInvoiceType.ZYFP ||
                        invoiceType == FpcyInvoiceType.PTFP ||
                        invoiceType == FpcyInvoiceType.DZFP)
                {

                    win1 = new CYJG_01();
                    win1.InvoiceTitle = invoiceType.GetDescription();
                    win1.InvoiceData = new JavaScriptSerializer().Deserialize<FpcyResponse>(rowData.InvoiceData);
                    win1.CyDate = rowData.OperateDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    win1.Show();
                }
                else if (invoiceType == FpcyInvoiceType.TXFFP)
                {
                    win2 = new CYJG_02();
                    win2.InvoiceTitle = invoiceType.GetDescription();
                    win2.InvoiceData = new JavaScriptSerializer().Deserialize<FpcyResponse>(rowData.InvoiceData);
                    win2.CyDate = rowData.OperateDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    win2.Show();
                }
                else if (invoiceType == FpcyInvoiceType.GSFP)
                {
                    winJs = new CYJG_JS();
                    winJs.InvoiceData = new JavaScriptSerializer().Deserialize<FpcyResponse>(rowData.InvoiceData);
                    winJs.CyDate = rowData.OperateDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    winJs.Show();
                }
                else if (invoiceType == FpcyInvoiceType.JDC)
                {
                    win3 = new CYJG_03();
                    win3.InvoiceData = new JavaScriptSerializer().Deserialize<FpcyResponse>(rowData.InvoiceData);
                    win3.CyDate = rowData.OperateDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    win3.Show();
                }
                //else if (invoiceType == FpcyInvoiceType.HYYS)
                //{
                //    win4 = new CYJG_04();
                //    win4.InvoiceData = new JavaScriptSerializer().Deserialize<FpcyResponse>(rowData.InvoiceData);
                //    win4.CyDate = rowData.OperateDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                //    win4.Show();
                //}
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.ShowInvoiceDetailError, ex);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var dateTxt = this.DateInvoice.Template.FindName("PART_TextBox", this.DateInvoice) as TextBox;
            dateTxt.PreviewMouseLeftButtonUp += dateTxt_Click;
        }

        private void dateTxt_Click(object sender, MouseButtonEventArgs e)
        {
            DateInvoice.IsDropDownOpen = true;
        }

        private void BaseWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DateInvoice.IsDropDownOpen = false;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DateInvoice.IsDropDownOpen = false;
        }

    }

    public class InvoiceType
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

}
