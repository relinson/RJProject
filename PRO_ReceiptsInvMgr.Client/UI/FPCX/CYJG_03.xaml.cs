using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Client.Resources.xskin;
using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using PRO_ReceiptsInvMgr.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRO_ReceiptsInvMgr.Client.UI.FPCX
{
    /// <summary>
    /// Interaction logic for CYJG_01.xaml
    /// </summary>
    public partial class CYJG_03 : BaseWindow
    {

        private string invoiceTitle;

        public FpcyResponse InvoiceData { get; set; }

        private string cyDate;

        public string CyDate
        {
            get
            {
                return cyDate;
            }

            set
            {
                cyDate = value;
                OnPropertyChanged("CyDate");
            }
        }

        public string InvoiceTitle
        {
            get
            {
                return invoiceTitle;
            }

            set
            {
                invoiceTitle = value;
                OnPropertyChanged("InvoiceTitle");
            }
        }

        public CYJG_03()
        {
            InitializeComponent();
        }

        private void cyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (InvoiceData != null)
            {
                var invoiceInfo = InvoiceData.invoiceList[0].invoiceInfo;
                if (invoiceInfo.cancellationMark == "Y")
                {
                    lblZF.Visibility = Visibility.Visible;
                }

                DateTime dt = DateTime.ParseExact(invoiceInfo.invoiceDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                invoiceInfo.invoiceDate = dt.ToString("yyyy年MM月dd日");
                invoiceInfo.checkCount = string.Format("第 {0} 次", invoiceInfo.checkCount);


                if (invoiceInfo.totalAmount != null && invoiceInfo.totalAmount.HasValue)
                {
                    invoiceInfo.DXtotalAmount = CommonHelper.MoneyToUpper(invoiceInfo.totalAmount.ToString());
                }
                
                double dInvoiceAmount;
                Double.TryParse(invoiceInfo.invoiceAmount,out dInvoiceAmount);
                invoiceInfo.dInvoiceAmount = dInvoiceAmount;

                double dTaxAmount;
                Double.TryParse(invoiceInfo.invoiceAmount, out dTaxAmount);
                invoiceInfo.dTaxAmount = dTaxAmount;

                this.DataContext = invoiceInfo;
            }
        }
    } 
}
