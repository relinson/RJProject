using PRO_ReceiptsInvMgr.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Model.Tables
{
    [Table("T_Fpcy")]
    /// <summary>
    /// 查验结果
    /// </summary>
    public class TFpcy : IBaseModel
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
    }

}
