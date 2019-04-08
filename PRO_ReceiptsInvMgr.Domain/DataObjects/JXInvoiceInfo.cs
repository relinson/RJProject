using PRO_ReceiptsInvMgr.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PRO_ReceiptsInvMgr.Domain.DataObjects
{
    public class JXInvoiceInfo : BaseDataObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 动态加载页面属性值
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int ID { get; set; }

        public string InvoiceCode { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime InvoiceDate { get; set; }
         
        public string XSFSH { get; set; }

        public string XSFMC { get; set; }

        public double? SE { get; set; }

        public double? HJBHSJE { get; set; }

        private string  invoiceStateDesc;
        public string InvoiceStateDesc
        {
            get
            {
                return invoiceStateDesc;
            }

            set
            {
                invoiceStateDesc = value;
                OnPropertyChanged("InvoiceStateDesc");
            }
        }


        private bool isChecked;

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public bool YQTXBZ { get; set; }
    }
}
