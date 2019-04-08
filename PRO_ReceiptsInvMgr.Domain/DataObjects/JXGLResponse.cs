using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRO_InvoiceDevice.Domain.DataObjects
{
    /// <summary>
    /// 企业当前税款所属期
    /// </summary>
    public class JXGL_1001
    {
        /// <summary>
        /// 企业当前税款所属期
        /// </summary>
        public string currentTaxPeriod { get; set; }
    }

    /// <summary>
    /// 企业当期可勾选发票开票起止日期
    /// </summary>
    public class JXGL_1002
    {
        /// <summary>
        /// 当前税款所属期可勾选发票的起始开票日期
        /// </summary>
        public string selectStartDate { get; set; }

        /// <summary>
        /// 当前税款所属期可勾选发票的截止开票日期
        /// </summary>
        public string selectEndDate { get; set; }
    }

    /// <summary>
    /// 企业当期勾选发票操作截至日期
    /// </summary>
    public class JXGL_1003
    {
        /// <summary>
        /// 当前税款所属期可勾选发票操作截止日期
        /// </summary>
        public string operationEndDate { get; set; }
    }

    /// <summary>
    /// 企业基本信息
    /// </summary>
    public class JXGL_1004
    {
        /// <summary>
        /// 企业当前税号
        /// </summary>
        public string currentTaxNo { get; set; }

        /// <summary>
        /// 企业旧税号
        /// </summary>
        public string oldTaxNo { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string companyName { get; set; }

        /// <summary>
        /// 申报周期，值为quarter-季度/month-月
        /// </summary>
        public string declarePeriod { get; set; }

        /// <summary>
        /// 信用等级，值为A/B/C/D或者空
        /// </summary>
        public string creditRating { get; set; }
    }

     
    /// <summary>
    /// 发票单张勾选响应
    /// </summary>
    public class JXGL_1005_RES
    {
        /// <summary>
        /// 发票代码
        /// </summary>
        public string fpdm { get; set; }

        /// <summary>
        /// 发票号码
        /// </summary>
        public string fphm { get; set; }

        /// <summary>
        /// 发票状态
        /// </summary>
        public string fpzt { get; set}

        /// <summary>
        /// 是否认证
        /// </summary>
        public string sfrz { get; set; }

        /// <summary>
        /// 认证月份
        /// </summary>
        public string rzMonth { get; set; }

        /// <summary>
        /// 确认日期
        /// </summary>
        public string qrDate { get; set; }

        /// <summary>
        /// 勾选标志
        /// </summary>
        public string gxbz { get; set; }

        /// <summary>
        /// 勾选日期
        /// </summary>
        public string gxDate { get; set; }


    }


}
