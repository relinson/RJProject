using PRO_ReceiptsInvMgr.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Application.Global
{
    #region ��Ʊ����
    /// <summary>
    /// ������
    /// </summary>
    public class FpcyResponse
    {
        public List<FpcyInvoice> invoiceList { get; set; }
    }

    /// <summary>
    /// ��Ʊ��Ϣ
    /// </summary>
    public class FpcyInvoice
    {
        public FpcyInvoiceInfo invoiceInfo { get; set; }
    }

    /// <summary>
    /// ��Ʊ������Ϣ
    /// </summary>
    public class FpcyInvoiceInfo
    {
        public string resultCode { get; set; }
        public string resultTip { get; set; }
        public string invoiceType { get; set; }
        public string invoiceCode { get; set; }
        public string invoiceNo { get; set; }
        public string checkCount { get; set; }
        public string salerName { get; set; }
        public string salerTaxNo { get; set; }
        public string salerAddressPhone { get; set; }
        public string salerAccount { get; set; }
        public string buyerTaxNo { get; set; }
        public string buyerName { get; set; }
        public string buyerAddressPhone { get; set; }
        public string buyerAccount { get; set; }
        public string invoiceDate { get; set; }
        public string invoiceAmount { get; set; }
        public string taxAmount { get; set; }
        public double? totalAmount { get; set; }
        public string remark { get; set; }
        public string machineNo { get; set; }
        public string drawer { get; set; }
        public string payee { get; set; }
        public string reviewer { get; set; }
        public string checkCode { get; set; }
        public string blueInvoiceCode { get; set; }
        public string blueInvoiceNo { get; set; }
        public string cancellationMark { get; set; }
        public string idNo { get; set; }
        public string vehicleType { get; set; }
        public string bandModel { get; set; }
        public string produceArea { get; set; }
        public string qualifiedNo { get; set; }
        public string commodityInspectionNo { get; set; }
        public string engineNo { get; set; }
        public string vehicleIdentificationNo { get; set; }
        public string certificateOfImport { get; set; }
        public string taxAuthorityCode { get; set; }
        public string taxPaymentCertificateNo { get; set; }
        public string limitedPeopleCount { get; set; }
        public string taxAuthorityName { get; set; }
        public string tonnage { get; set; }
        public string taxRate { get; set; }
        public string salerAddress { get; set; }
        public string salerPhone { get; set; }
        public string salerBankName { get; set; }
        public string salerBankAccount { get; set; }
        public string carrierName { get; set; }
        public string carrierTaxNo { get; set; }
        public string draweeName { get; set; }
        public string draweeTaxNo { get; set; }
        public string receiveName { get; set; }
        public string receiveTaxNo { get; set; }
        public string consignorName { get; set; }
        public string consignorTaxNo { get; set; }
        public string transportGoodsInfo { get; set; }
        public string throughAddress { get; set; }
        public string taxDiskNumber { get; set; }
        public string carNumber { get; set; }
        public string vehicleTonnage { get; set; }
        public string trafficFeeFlag { get; set; }
        public string zeroTaxRateFlag { get; set; }

        public List<FpcyDetail> detailList { get; set; }

        public string DXtotalAmount { get; set; }

        public double dInvoiceAmount { get; set; }

        public double dTaxAmount { get; set; }
    }

    /// <summary>
    /// ��Ʒ����Ϣ
    /// </summary>
    public class FpcyDetail
    {
        public string detailNo { get; set; }
        public string goodsName { get; set; }
        public string detailAmount { get; set; }
        public string num { get; set; }
        public string taxRate { get; set; }
        public string taxAmount { get; set; }
        public string taxUnitPrice { get; set; }
        public string taxDetailAmount { get; set; }
        public string unitPrice { get; set; }
        public string specificationModel { get; set; }
        public string unit { get; set; }
        public string expenseItem { get; set; }
        public string plateNo { get; set; }
        public string type { get; set; }
        public string trafficDateStart { get; set; }
        public string trafficDateEnd { get; set; }
    }
    #endregion
     
    #region Ӧ���Ƿ񿪷�
    public class AppOpenRequest
    {
        public string taxno { get; set; }
        public string appId { get; set; }
    }

    public class AppOpenResponse
    {
        public AppType appType { get; set; }

        public int appFlag { get; set; }

        public int expireFlag { get; set; }
    }
    #endregion

    /// <summary>
    /// ע���������
    /// </summary>
    public class RegisterRequest
    {
        public string taxNo { get; set; }

        public string appKey { get; set; }

        public string orgName { get; set; }

        public string pwd { get; set; }

        public string areaId { get; set; }
    }

    /// <summary>
    /// ��¼�������
    /// </summary>
    public class LoginRequest
    {
        public string taxNo { get; set; }

        public string pwd { get; set; }

        public string token { get; set; }
    }

    /// <summary>
    /// ��¼������Ӧ
    /// </summary>
    public class LoginResponse
    {
        public string areaId { get; set; }

        public string appKey { get; set; }

        public string expiredTime { get; set; }
    }

    #region ����Ӧ�����ز�ѯ
    public class AppResponse
    {
        public List<WSAppInfo> CYYYS { get; set; }
    }

    public class WSAppInfo
    {
        public int ID { get; set; }
        public string YYNAME { get; set; }
        public string YYMS { get; set; }
        public string YYEXEXDLJ { get; set; }
        public string MD5 { get; set; }
        public string YYTPIO { get; set; }
        public string YYVERSION { get; set; }
        public string CYYYURL { get; set; }
    }
    #endregion

    #region ����Ӧ������
    public class AppUpdateResponse
    {
        public List<AppUpdate> YYUPDATES { get; set; }
    }

    public class AppUpdate
    {
        public string YYNAME { get; set; }
        public string YYMS { get; set; }
        public string YYPATH { get; set; }
        public string YYVERSION { get; set; }
        public string MD5 { get; set; }
        public string YY_ID { get; set; }
    }
    #endregion

    #region ������ѯ�͹���չʾ
    public class AdertiseResponse
    {
        public List<NewestInfo> ZXZXS { get; set; }
        public List<GgInfo> GGZS { get; set; }
    }

    public class NewestInfo
    {
        public string ZXMC { get; set; }
        public string ZXURL { get; set; }
    }

    public class GgInfo
    {
        public string ID { get; set; }
        public string GG_TITLE { get; set; }
        public string GG_AUTHOR { get; set; }
        public string GG_TIME { get; set; }
        public string GG_CONTENT { get; set; }
    }
    #endregion

    /// <summary>
    /// �޸������������
    /// </summary>
    public class ModifyPwdRequest
    {
        public string taxNo { get; set; }

        public string oldPwd { get; set; }

        public string newPwd { get; set; }
    }

    /// <summary>
    /// �޸�ע�����������
    /// </summary>
    public class ModifyZcmRequest
    {
        public string taxNo { get; set; }

        public string pwd { get; set; }

        public string newAppKey { get; set; }
    }

    #region �������Խӿ�
    public class QAInfo
    {
        public List<HotIssuesInfo> HOTISSUES { get; set; }

        public List<OwnIssuesInfo> OWNISSUES { get; set; }
    }

    public class HotIssuesInfo
    {
        public string ID { get; set; }
        public string Question { get; set; }
        public List<QAResonse> RESPONSES { get; set; }
    }

    public class OwnIssuesInfo
    {
        public string ID { get; set; }
        public string Question { get; set; }
        public string NSRSBH { get; set; }
        public List<QAResonse> RESPONSES { get; set; }
    }

    public class QAResonse
    {
        public string RESPONSE { get; set; }

        public string RTIME { get; set; }
    }
    #endregion

    #region �û��ֲ�����
    public class ManualResponse
    {
        public string SCNAME { get; set; }
        public string SCVERSION { get; set; }
        public string MD5 { get; set; }
        public string SCWJURL { get; set; }
    }
    #endregion
}
