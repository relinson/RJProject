using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using PRO_ReceiptsInvMgr.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PRO_ReceiptsInvMgr.Application.ViewModel
{
    public class GxrzQueryModel : ViewModelBase
    {
        private DateTime? invoiceDateStart;

        private DateTime? invoiceDateEnd;

        private string invoiceCode;

        private string invoiceNo;

        private string xSFSH;

        private string sE;

        private string fpzt;

        public DateTime? InvoiceDateStart
        {
            get
            {
                return invoiceDateStart;
            }

            set
            {
                invoiceDateStart = value;
                OnPropertyChanged("InvoiceDateStart");
            }
        }

        public DateTime? InvoiceDateEnd
        {
            get
            {
                return invoiceDateEnd;
            }

            set
            {
                invoiceDateEnd = value;
                OnPropertyChanged("InvoiceDateEnd");
            }
        }

        public string InvoiceCode
        {
            get
            {
                return invoiceCode;
            }

            set
            {
                invoiceCode = value;
                OnPropertyChanged("InvoiceCode");
            }
        }

        public string XSFSH
        {
            get
            {
                return xSFSH;
            }

            set
            {
                xSFSH = value;
                OnPropertyChanged("XSFSH");
            }
        }

        public string SE
        {
            get
            {
                return sE;
            }
            set
            {
                sE = value;
                OnPropertyChanged("SE");
            }
        }

        public string FPZT
        {
            get
            {
                return fpzt;
            }

            set
            {
                fpzt = value;
                OnPropertyChanged("fpzt");
            }
        }

        public string InvoiceNo
        {
            get
            {
                return invoiceNo;
            }

            set
            {
                invoiceNo = value;
                OnPropertyChanged("InvoiceNo");
            }
        }
    }
    public class GxrzViewModel : ViewModelBase
    {
        public GxrzViewModel()
        { }

        private string skssq;

        private string selectStartDate;

        private string selectEndDate;

        private int listCount;

        private string totalAmount;

        private string totalSE;

        private GxrzQueryModel queryModel;

        private string taxPeriod;

        private string invoiceStartDate;

        private string invoiceEndDate;

        private ObservableCollection<JXInvoiceInfo> invoiceList;

        private double yqTipCounts;
         
        private bool isAllChecked;

        public ObservableCollection<JXInvoiceInfo> InvoiceList
        {
            get
            {
                return invoiceList;
            }

            set
            {
                invoiceList = value;
                OnPropertyChanged("InvoiceList");
            }
        }

        public GxrzQueryModel QueryModel
        {
            get
            {
                return queryModel;
            }

            set
            {
                queryModel = value;
                OnPropertyChanged("QueryModel");
            }
        }

        public int ListCount
        {
            get
            {
                return listCount;
            }

            set
            {
                listCount = value;
                OnPropertyChanged("ListCount");
            }
        }

        public string TotalAmount
        {
            get
            {
                if (string.IsNullOrEmpty(totalAmount))
                {
                    totalAmount = "0.00";
                }
                return totalAmount;
            }
            set
            {
                totalAmount = value;
                OnPropertyChanged("TotalAmount");
            }
        }

        public string TotalSE
        {
            get
            {
                if (string.IsNullOrEmpty(totalSE))
                {
                    totalSE = "0.00";
                }
                return totalSE;
            }

            set
            {
                totalSE = value;
                OnPropertyChanged("TotalSE");
            }
        }

        public bool IsAllChecked
        {
            get
            {
                return isAllChecked;
            }

            set
            {
                isAllChecked = value;
                OnPropertyChanged("IsAllChecked");
            }
        }

        public string InvoiceStartDate
        {
            get
            {
                return invoiceStartDate;
            }

            set
            {
                invoiceStartDate = value;
                OnPropertyChanged("InvoiceStartDate");
            }
        }

        public string InvoiceEndDate
        {
            get
            {
                return invoiceEndDate;
            }

            set
            {
                invoiceEndDate = value;
                OnPropertyChanged("InvoiceEndDate");
            }
        }

        public string TaxPeriod
        {
            get
            {
                return taxPeriod;
            }

            set
            {
                taxPeriod = value;
                OnPropertyChanged("TaxPeriod");
            }
        }

         
        public double YqTipCounts
        {
            get
            {
                return yqTipCounts;
            }

            set
            {
                yqTipCounts = value;
                OnPropertyChanged("YqTipCounts");
            }
        }

        public string Skssq
        {
            get
            {
                return skssq;
            }

            set
            {
                skssq = value;
                OnPropertyChanged("Skssq");
            }
        }

        public string SelectStartDate
        {
            get
            {
                return selectStartDate;
            }

            set
            {
                selectStartDate = value;
                OnPropertyChanged("SelectStartDate");
            }
        }

        public string SelectEndDate
        {
            get
            {
                return selectEndDate;
            }

            set
            {
                selectEndDate = value;
                OnPropertyChanged("SelectEndDate");
            }
        }
    }
}
