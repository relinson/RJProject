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
    public class QueryModel : ViewModelBase
    {
        private string invoiceType;

        private string invoiceCode;

        private string invoiceNo;

        private string invoiceDate;

        private string invoiceAmount;

        private string checkCode;

        public string taxNo { get; set; }

        private Visibility checkCodeVisibility = Visibility.Hidden;

        private Visibility invoiceAmountVisibility = Visibility.Visible;

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

        public string InvoiceDate
        {
            get
            {
                return invoiceDate;
            }

            set
            {
                invoiceDate = value;
                OnPropertyChanged("InvoiceDate");
            }
        }

        public string CheckCode
        {
            get
            {
                return checkCode;
            }

            set
            {
                checkCode = value;
                OnPropertyChanged("CheckCode");
            }
        }

        public string InvoiceType
        {
            get
            {
                return invoiceType;
            }

            set
            {
                invoiceType = value;
                OnPropertyChanged("InvoiceType");
                if (!string.IsNullOrEmpty(invoiceType))
                {
                    var type = (FpcyInvoiceType)Convert.ToInt32(invoiceType);
                    if (type == FpcyInvoiceType.ZYFP
                        || type == FpcyInvoiceType.JDC
                        //|| type == FpcyInvoiceType.HYYS
                        )
                    {
                        CheckCodeVisibility = Visibility.Hidden;
                        InvoiceAmountVisibility = Visibility.Visible;
                    }
                    else
                    {
                        CheckCodeVisibility = Visibility.Visible;
                        InvoiceAmountVisibility = Visibility.Hidden;
                    }
                } 
            }
        }

        public string InvoiceAmount
        {
            get
            {
                return invoiceAmount;
            }

            set
            {
                invoiceAmount = value;
                OnPropertyChanged("InvoiceAmount");
            }
        }

        public Visibility CheckCodeVisibility
        {
            get
            {
                return checkCodeVisibility;
            }

            set
            {
                checkCodeVisibility = value;
                OnPropertyChanged("CheckCodeVisibility");
            }
        }

        public Visibility InvoiceAmountVisibility
        {
            get
            {
                return invoiceAmountVisibility;
            }

            set
            {
                invoiceAmountVisibility = value;
                OnPropertyChanged("InvoiceAmountVisibility");
            }
        }
    }
    public class FpcyViewModel : ViewModelBase
    {
        
        public FpcyViewModel()
        { }

        private int listCount;
        private QueryModel queryModel;

        public QueryModel QueryModel
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
    }
}
