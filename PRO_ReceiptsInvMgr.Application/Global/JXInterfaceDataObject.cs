using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Domain.DataObjects
{
    /// <summary>
    /// 进项登录请求报文
    /// </summary>
    public class JXRegisterRequest
    {
        public string taxno { get; set; }
        public string orgName { get; set; }
        public string area { get; set; }
        public string deviceId { get; set; }
    }

    /// <summary>
    /// 校验是否注册请求报文
    /// </summary>
    public class JXIsRegisterRequest
    {
        public string taxno { get; set; }
    }

    /// <summary>
    /// 校验是否注册响应报文
    /// </summary>
    public class JXIsRegisterResponse
    {
        public string result { get; set; }
        public string message { get; set; }
    }


    /// <summary>
    /// 税款所属期请求报文
    /// </summary>
    public class JXSkssqRequest
    {
        public string taxno { get; set; }
        public string area { get; set; }
        public string token { get; set; }
    }

    /// <summary>
    /// 税款所属期响应报文
    /// </summary>
    public class JXSkssqResponse
    {
        public string skssq { get; set; }
    }

    /// <summary>
    /// 获取可勾选发票开票起止日期请求报文
    /// </summary>
    public class JXStartEndDateRequest
    {
        public string taxno { get; set; }
        public string area { get; set; }
        public string token { get; set; }
    }

    /// <summary>
    /// 获取可勾选发票开票起止日期响应
    /// </summary>
    public class JXStartEndDateResponse
    {
        public string selectStartDate { get; set; }
        public string selectEndDate { get; set; }
    }

    /// <summary>
    /// 勾选认证查询请求报文
    /// </summary>
    public class JXGxrzRequest
    {
        public string taxno { get; set; }
        public string area { get; set; }
        public string token { get; set; }
        public string skssq { get; set; }
        public string kprqBegin { get; set; }
        public string kprqEnd { get; set; }
        public string invoiceCode { get; set; }
        public string fpzt { get; set; }
        public string invoiceNo { get; set; }
        public string sellerTaxNo { get; set; }
        public string taxAmount { get; set; }
        public string gxrqBegin { get; set; }
        public string gxrqEnd { get; set; }
    }


    /// <summary>
    /// 勾选认证查询响应报文
    /// </summary>
    public class JXGxrzResponse
    {
        public string result { get; set; }
        public List<JXGxrzContent> data { get; set; }
        public string message { get; set; }

    }

    public class JXGxrzContent
    {
        public string kprq { get; set; }
        public string invoiceNo { get; set; }
        public string invoiceCode { get; set; }
        public string sellerName { get; set; }
        public string invoiceAmount { get; set; }
        public string taxAmount { get; set; }
        public string invoiceStatus { get; set; }
        public string fpzt { get; set; }
        public string sellerTaxNo { get; set; }
        public string fplx { get; set; }
        public string gxbz { get; set; }
        public string warnFlag { get; set; }
    }

    /// <summary>
    /// 勾选认证
    /// </summary>
    public class JXGxrzSubmitRequest
    {
        public string taxno { get; set; }
        public string area { get; set; }
        public string token { get; set; }
        public string skssq { get; set; }
        public string fplb { get; set; }
    }

    /// <summary>
    /// 勾选认证响应报文
    /// </summary>
    public class JXGxrzSubmitResponse
    {
         public List<JXGxrzSubmitContent> data { get; set; }
    }

    public class JXGxrzSubmitContent
    {
        public string result { get; set; }
        public string kprq { get; set; }
        public string invoiceNo { get; set; }
        public string invoiceCode { get; set; }
        public string message { get; set; }
    }

    /// <summary>
    /// 逾期提醒月数请求报文
    /// </summary>
    public class JxyqWarnMonthRequest
    {
        public string taxno { get; set; }
    }

    /// <summary>
    /// 税款所属期响应报文
    /// </summary>
    public class JxyqWarnMonthResponse
    {
        public string warnMonth { get; set; }
    }

    /// <summary>
    /// 设置逾期提醒月数请求报文
    /// </summary>
    public class JxyqSetWarnMonthRequest
    {
        public string overWarnMonth { get; set; }
        public string taxno { get; set; }
    }

    /// <summary>
    /// 逾期预警请求报文
    /// </summary>
    public class JXWarnRequest
    {
        public string taxno { get; set; }
        public int overWarnMonth { get; set; }
    }

    /// <summary>
    /// 逾期预警响应报文
    /// </summary>
    public class JXWarnResponse
    {
        public string result { get; set; }
        public List<JXWarnContent> data { get; set; }
        public string message { get; set; }
    }

    /// <summary>
    /// 逾期预警响应报文Content
    /// </summary>
    public class JXWarnContent
    {
        public string kprq { get; set; }
        public string invoiceNo { get; set; }
        public string invoiceCode { get; set; }
        public string sellerName { get; set; }
        public string invoiceAmount { get; set; }
        public string taxAmount { get; set; }
        public string invoiceStatus { get; set; }
    }

    /// <summary>
    /// 认证清单请求报文
    /// </summary>
    public class JXCertificateRequest
    {
        public string taxno { get; set; }

        public string area { get; set; }

        public string token { get; set; }

        public string skssq { get; set; }
    }

    /// <summary>
    /// 认证清单响应报文
    /// </summary>
    public class JXCertificateResponse
    {

        public string result { get; set; }
        public List<JXCertificateContent> data { get; set; }
        public string message { get; set; }
       
    }

    public class JXCertificateContent
    {
        public string kprq { get; set; }

        public string invoiceNo { get; set; }

        public string invoiceCode { get; set; }

        public string sellerTaxNo { get; set; }

        public string sellerName { get; set; }

        public string invoiceAmount { get; set; }

        public string taxAmount { get; set; }

        public string invoiceStatus { get; set; }
    }


    /// <summary>
    /// 勾选认证发票状态请求
    /// </summary>
    public class JXInvoiceStatusRequest
    {
        public string fplb { get; set; }
    }
     
    public class JXInvoiceStatusContent
    {
        public string invoiceCode { get; set; }

        public string invoiceNo { get; set; }

        public string fpzt { get; set; }
    }


}
