using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace PRO_ReceiptsInvMgr.Domain.DataObjects
{
    public class FpcyDataObject : BaseDataObject
    {
        public int ID { get; set; }
        public string InvoiceCode { get; set; }

        public string InvoiceNo { get; set; }

        public string InvoiceDate { get; set; }

        public double? TotalAmount { get; set; }

        public string CheckCode { get; set; }

        public string InvoiceType { get; set; } 

        public string InvoiceData { get; set; }

        public DateTime? OperateDate { get; set; }

        public string ResultCode { get; set; }

        public System.Windows.Visibility CYSuccessVisible { get; set; }
        public System.Windows.Visibility CYNotFindVisible { get; set; }
        public System.Windows.Visibility CYNotSameVisible { get; set; }

        public ImageSource FpzlImageSource { get; set; }

        public ImageSource FplsImageSource { get; set; }

        public string InvoiceTypeDescription { get; set; }
    }
}
