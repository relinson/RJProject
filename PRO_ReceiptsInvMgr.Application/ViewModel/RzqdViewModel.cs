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
    public class RzqdQueryViewModel : ViewModelBase
    {
        private DateTime? skssq;
        public DateTime? Skssq
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
    }
    public class RzqdViewModel : ViewModelBase
    {
        public RzqdViewModel()
        { }
         
        private int listCount;
        private RzqdQueryViewModel queryModel;
        private ObservableCollection<JXInvoiceInfo> invoiceList;
        private string totalAmount;
        private string totalSE;


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

        public RzqdQueryViewModel QueryModel
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
    }
}
