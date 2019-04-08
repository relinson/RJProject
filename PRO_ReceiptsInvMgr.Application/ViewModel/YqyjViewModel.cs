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
    public class YqyjQueryViewModel : ViewModelBase
    {
        private int month;

        public int Month
        {
            get
            {
                return month;
            }

            set
            {
                month = value;
                OnPropertyChanged("Month");
            }
        }
    }
    public class YqyjViewModel : ViewModelBase
    {
        public YqyjViewModel()
        { }
         
        private int listCount;
        private YqyjQueryViewModel queryModel;
        private ObservableCollection<JXInvoiceInfo> invoiceList;
        private bool isAllChecked;
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

        public YqyjQueryViewModel QueryModel
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

        public string TotalAmount
        {
            get
            {
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
