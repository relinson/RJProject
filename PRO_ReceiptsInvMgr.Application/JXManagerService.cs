using Newtonsoft.Json;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Application.ViewModel;
using PRO_ReceiptsInvMgr.Component;
using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using PRO_ReceiptsInvMgr.Domain.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace PRO_ReceiptsInvMgr.Application
{
    public class JXManagerService : BaseService
    {

        /// <summary>
        /// 获取税款所属期
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public string GetJXSsq(out string errorMsg, int reTimes = 1)
        {
            string strResult = string.Empty;
            errorMsg = string.Empty;
            try
            {
                string strRequest = new JavaScriptSerializer().Serialize(
                    new JXSkssqRequest
                    {
                        taxno = GlobalInfo.NSRSBH,
                        area = GlobalInfo.Dqdm,
                        token = GlobalInfo.token,
                    });
                bool result = false;
                var response = WSInterface.GetResponse(strRequest, InterfaceType.JXSkssq, ref result, out errorMsg);

                if (result)
                {
                    var obj = new JsonSerializer().Deserialize<JXSkssqResponse>(new JsonTextReader(new StringReader(response)));
                    strResult = GetFormatStr(obj.skssq);
                }
                else
                {
                    // token 过期重试处理
                    if (IsReTry(ref errorMsg, reTimes))
                    {
                        strResult = GetJXSsq(out errorMsg, 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.GetJXSkssqError, ex);
                errorMsg = PRO_ReceiptsInvMgr.Resources.Message.GetJXSkssqError;
            }
            return strResult;
        }

        /// <summary>
        /// 获取开票起止日期
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public string[] GetJXKpStartEndDate(out string errorMsg, int reTimes = 1)
        {

            string[] strArray = new string[2];
            errorMsg = string.Empty;
            try
            {
                string strRequest = new JavaScriptSerializer().Serialize(
                    new JXSkssqRequest
                    {
                        taxno = GlobalInfo.NSRSBH,
                        area = GlobalInfo.Dqdm,
                        token = GlobalInfo.token,
                    });
                bool result = false;

                var response = WSInterface.GetResponse(strRequest, InterfaceType.JXKpStartEndDate, ref result, out errorMsg);

                if (result)
                {
                    var obj = new JsonSerializer().Deserialize<JXStartEndDateResponse>(new JsonTextReader(new StringReader(response)));
                    GlobalInfo.selectStartDate = GetFormatStr(obj.selectStartDate, "yyyyMMdd", "yyyy-MM-dd");
                    GlobalInfo.selectEndDate = GetFormatStr(obj.selectEndDate, "yyyyMMdd", "yyyy-MM-dd");

                    strArray[0] = GetFormatStr(obj.selectStartDate, "yyyyMMdd", "yyyy年MM月dd日");
                    strArray[1] = GetFormatStr(obj.selectEndDate, "yyyyMMdd", "yyyy年MM月dd日");
                }
                else
                {
                    if (IsReTry(ref errorMsg, reTimes))
                    {
                        strArray = GetJXKpStartEndDate(out errorMsg, 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.GetJXSkssqError, ex);
                errorMsg = PRO_ReceiptsInvMgr.Resources.Message.GetJXSkssqError;
            }
            return strArray;
        }

        public string GetFormatStr(string strDt, string strFormat = "yyyyMM", string resultFormat = "yyyy-MM")
        {
            DateTime dt = DateTime.ParseExact(strDt, strFormat, System.Globalization.CultureInfo.CurrentCulture);
            return dt.ToString(resultFormat);
        }

        public List<JXInvoiceInfo> GetJXData(GxrzQueryModel queryModel, out int totalCount, out string errorMsg, int reTimes = 1)
        {
            totalCount = 0;
            errorMsg = string.Empty;
            var invoiceList = new List<JXInvoiceInfo>();
            try
            {
                string strRequest = new JavaScriptSerializer().Serialize(
                    new JXGxrzRequest
                    {
                        taxno = GlobalInfo.NSRSBH,
                        area = GlobalInfo.Dqdm,
                        token = GlobalInfo.token,
                        skssq = GlobalInfo.skssq.Replace("-", ""),
                        invoiceCode = queryModel.InvoiceCode,
                        invoiceNo = queryModel.InvoiceNo,
                        fpzt = queryModel.FPZT,
                        sellerTaxNo = queryModel.XSFSH,
                        taxAmount = queryModel.SE,
                        kprqBegin = queryModel.InvoiceDateStart.HasValue ? queryModel.InvoiceDateStart.Value.ToString("yyyy-MM-dd") : null,
                        kprqEnd = queryModel.InvoiceDateEnd.HasValue ? queryModel.InvoiceDateEnd.Value.ToString("yyyy-MM-dd") : null,
                        gxrqBegin = GlobalInfo.selectStartDate,
                        gxrqEnd = GlobalInfo.selectEndDate
                    });


                //Logging.Log4NetHelper.Error(this, "获取进项数据request:" + strRequest);
                bool result = false;
                var response = WSInterface.GetResponse(strRequest, InterfaceType.JXGXRZSearch, ref result, out errorMsg);

                if (result)
                {
                    var obj = new JsonSerializer().Deserialize<JXGxrzResponse>(new JsonTextReader(new StringReader(response)));
                    if (obj.result == "0")
                    {
                        errorMsg = obj.message;
                        return invoiceList;
                    }
                    else
                    {
                        var content = obj.data;

                        if (content != null && content.Any())
                        {
                            invoiceList = ToView(content);
                        }
                    }
                }
//                else
//                 {
//                     if (IsReTry(ref errorMsg, reTimes))
//                     {
//                         invoiceList = GetJXData(queryModel, out totalCount, out errorMsg, 2);
//                     }
//                 }

                totalCount = invoiceList.Count;
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.GetJXDataError, ex);
                errorMsg = PRO_ReceiptsInvMgr.Resources.Message.GetJXDataError;
            }

            totalCount = invoiceList.Count;
            return invoiceList;
        }

        /// <summary>
        /// 勾选认证
        /// </summary>
        /// <param name="fplb"></param>
        /// <param name="errorMsg"></param>
        /// <param name="reTimes"></param>
        /// <returns></returns>
        public bool GXRZ(List<JXInvoiceInfo> checkedList, out string errorMsg, int reTimes = 1)
        {
            bool ret = false;
            errorMsg = string.Empty;
            try
            {
                JXGxrzSubmitRequest request = new JXGxrzSubmitRequest();
                request.taxno = GlobalInfo.NSRSBH;
                request.area = GlobalInfo.Dqdm;
                request.skssq = GlobalInfo.skssq.Replace("-", "");
                request.token = GlobalInfo.token;

                List<string> strList = new List<string>();
                for (var j = 0; j < checkedList.Count; j++)
                {
                    if (j < checkedList.Count)
                    {
                        var item = checkedList[j];
                        strList.Add(string.Format("{0},{1},{2}", item.InvoiceCode, item.InvoiceNo, item.InvoiceDate.ToString("yyyy-MM-dd")));
                    }
                }
                if (strList.Any())
                {
                    request.fplb = string.Join(";", strList);
                }

                string strRequest = new JavaScriptSerializer().Serialize(request);
                bool result = false;
                var response = WSInterface.GetResponse(strRequest, InterfaceType.JXColCert, ref result, out errorMsg, 120000);

                if (result)
                {
                    var obj = new JsonSerializer().Deserialize<JXGxrzSubmitResponse>(new JsonTextReader(new StringReader(response)));
                    var data = obj.data;
                    int successCount = data.Count(x => x.result == "1");
                    int failCount = data.Count(x => x.result == "0");
                    int unExecCount = data.Count(x => x.result == "-1");

                    StringBuilder sb = new StringBuilder();

                    if (successCount > 0)
                    {
                        sb.Append(string.Format("{0}张发票勾选认证执行成功。\r\n", successCount));
                    }

                    if (failCount > 0)
                    {
                        sb.Append(string.Format("--------{0}张发票勾选认证执行失败--------\r\n", failCount));
                        foreach (var item in data.Where(x => x.result == "0"))
                        {
                            sb.Append(string.Format("发票代码:{0},发票号码:{1}\r\n", item.invoiceCode, item.invoiceNo));
                        }

                    }
                    if (unExecCount > 0)
                    {
                        sb.Append(string.Format("--------{0}张发票未执行--------\r\n", unExecCount));
                        foreach (var item in data.Where(x => x.result == "-1"))
                        {
                            sb.Append(string.Format("发票代码:{0},发票号码:{1}\r\n", item.invoiceCode, item.invoiceNo));
                        }
                    }
                    errorMsg = sb.ToString();
                }
                else
                {
                    IsReTry(ref errorMsg, reTimes);
                }

            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.GxrzSubmitError, ex);
                errorMsg = PRO_ReceiptsInvMgr.Resources.Message.GxrzSubmitError;
            }
            return ret;
        }



        /// <summary>
        /// 获取逾期提醒月数
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public int GetYqWarnMonth(out string errorMsg)
        {
            int month = 0;
            errorMsg = string.Empty;
            try
            {
                string strRequest = new JavaScriptSerializer().Serialize(
                    new JxyqWarnMonthRequest
                    {
                        taxno = GlobalInfo.NSRSBH,
                    });
                bool result = false;
                var response = WSInterface.GetResponse(strRequest, InterfaceType.JXYQWarnMonth, ref result, out errorMsg);

                if (result)
                {
                    var obj = new JsonSerializer().Deserialize<JxyqWarnMonthResponse>(new JsonTextReader(new StringReader(response)));
                    Int32.TryParse(obj.warnMonth, out month);
                }

            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.GetJXWarnMonthError, ex);
                errorMsg = PRO_ReceiptsInvMgr.Resources.Message.GetJXWarnMonthError;
            }
            return month;
        }


        /// <summary>
        /// 设置逾期提醒月数
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public void SetYqWarnMonth(string month)
        {
            try
            {
                string strRequest = new JavaScriptSerializer().Serialize(
                    new JxyqSetWarnMonthRequest
                    {
                        overWarnMonth = month,
                        taxno = GlobalInfo.NSRSBH,
                    });
                bool result = false;
                string errorMsg = string.Empty;
                WSInterface.GetResponse(strRequest, InterfaceType.JXYQSetWarnMonth, ref result, out errorMsg);
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.SetJXWarnMonthError, ex);
            }
        }

        /// <summary>
        /// 获取逾期提醒发票数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="totalCount"></param>
        /// <param name="errorMsg"></param>
        /// <param name="reTimes"></param>
        /// <returns></returns>
        public List<JXInvoiceInfo> GetJXYQData(YqyjQueryViewModel query, out int totalCount, out string errorMsg)
        {
            totalCount = 0;
            errorMsg = string.Empty;
            var invoiceList = new List<JXInvoiceInfo>();
            try
            {
                string strRequest = new JavaScriptSerializer().Serialize(
                    new JXWarnRequest
                    {
                        taxno = GlobalInfo.NSRSBH,
                        overWarnMonth = query.Month,
                    });
                bool result = false;
                var response = WSInterface.GetResponse(strRequest, InterfaceType.JXYQYJ, ref result, out errorMsg);

                if (result)
                {
                    var obj = new JsonSerializer().Deserialize<JXWarnResponse>(new JsonTextReader(new StringReader(response)));

                    var content = obj.data;

                    if (content != null && content.Any())
                    {
                        invoiceList = ToView(content);
                    }
                }
                totalCount = invoiceList.Count;
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.GetYqyjError, ex);
                errorMsg = PRO_ReceiptsInvMgr.Resources.Message.GetYqyjError;
            }

            totalCount = invoiceList.Count;
            return invoiceList;
        }

        /// <summary>
        /// 重试获取token调用接口
        /// </summary>
        private bool IsReTry(ref string errorMsg, int reTime)
        {
            var result = false;

            if (reTime > 1)
            {
                return result;
            }
            if (errorMsg.Contains("(J04)") || errorMsg.Contains("(J99)"))
            {
                string tokenMsg = string.Empty;
                result = GetNewToken(out tokenMsg);
                if (result)
                {
                    GlobalInfo.token = CryptTool.Token;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取token值
        /// </summary>
        private bool GetNewToken(out string errorMsg)
        {
            errorMsg = string.Empty;
            var loop = 0;
            bool isSuccess = false;
            //while ((loop < 5 && !string.IsNullOrEmpty(CryptTool.ErrorMsg) && CryptTool.ErrorMsg.Contains(PRO_ReceiptsInvMgr.Resources.Message.JXCertError)) || loop == 0)
            while (loop < 5)
            {
                isSuccess = CryptTool.Login(GlobalInfo.JxPwd);
                loop++;
                if (isSuccess)
                {
                    break;
                }
            }

            if (!isSuccess)
            {
                errorMsg = CryptTool.ErrorMsg;
            }
            else
            {
                GlobalInfo.token = CryptTool.Token;
            }
            return isSuccess;
        }


        /// <summary>
        /// 获取认证清单发票数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="totalCount"></param>
        /// <param name="errorMsg"></param>
        /// <param name="reTimes"></param>
        /// <returns></returns>
        public List<JXInvoiceInfo> GetRzqdData(RzqdQueryViewModel query, out int totalCount, out string errorMsg, int reTimes = 1)
        {
            totalCount = 0;
            errorMsg = string.Empty;
            var invoiceList = new List<JXInvoiceInfo>();
            try
            {
                string strRequest = new JavaScriptSerializer().Serialize(
                    new JXCertificateRequest
                    {
                        taxno = GlobalInfo.NSRSBH,
                        area = GlobalInfo.Dqdm,
                        token = GlobalInfo.token,
                        skssq = query.Skssq.Value.ToString("yyyyMM")
                    });
                bool result = false;
                var response = WSInterface.GetResponse(strRequest, InterfaceType.JXRZQD, ref result, out errorMsg);

                if (result)
                {
                    var obj = new JsonSerializer().Deserialize<JXCertificateResponse>(new JsonTextReader(new StringReader(response)));
                    if (obj.result == "0")
                    {
                        errorMsg = obj.message;
                        return invoiceList;
                    }
                    else
                    {
                        var content = obj.data;

                        if (content != null && content.Any())
                        {
                            content = content.OrderBy(x => x.kprq).ToList();
                            invoiceList = ToView(content);
                        }
                    }
                }
                else
                {
                    if (IsReTry(ref errorMsg, reTimes))
                    {
                        invoiceList = GetRzqdData(query, out totalCount, out errorMsg, 2);
                    }
                }
                totalCount = invoiceList.Count;
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.GetRzqdError, ex);
                errorMsg = PRO_ReceiptsInvMgr.Resources.Message.GetRzqdError;
            }

            totalCount = invoiceList.Count;
            return invoiceList;
        }

        private List<JXInvoiceInfo> ToView(List<JXWarnContent> list)
        {
            var invoiceList = new List<JXInvoiceInfo>();
            foreach (var item in list)
            {
                var invoice = new JXInvoiceInfo();
                invoice.InvoiceCode = item.invoiceCode;
                invoice.InvoiceNo = item.invoiceNo;
                invoice.InvoiceDate = DateTime.ParseExact(item.kprq, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                invoice.XSFMC = item.sellerName;
                invoice.SE = ValueParse.GetDouble(item.taxAmount);
                invoice.HJBHSJE = ValueParse.GetDouble(item.invoiceAmount);

                invoiceList.Add(invoice);
            }
            return invoiceList;
        }

        private List<JXInvoiceInfo> ToView(List<JXGxrzContent> list)
        {
            var invoiceList = new List<JXInvoiceInfo>();
            foreach (var item in list)
            {
                var invoice = new JXInvoiceInfo();
                invoice.InvoiceCode = item.invoiceCode;
                invoice.InvoiceNo = item.invoiceNo;
                invoice.InvoiceDate = DateTime.ParseExact(item.kprq, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                invoice.XSFSH = item.sellerTaxNo;
                invoice.XSFMC = item.sellerName;
                invoice.YQTXBZ = item.warnFlag == "true";
                invoice.SE = ValueParse.GetDouble(item.taxAmount);
                invoice.HJBHSJE = ValueParse.GetDouble(item.invoiceAmount);
               
                int nfpzt = 0;
                Int32.TryParse(item.fpzt, out nfpzt);
                invoice.InvoiceStateDesc = ((InvoiceStatus)nfpzt).GetDescription();//"加载中...";

                invoiceList.Add(invoice);
            }
            return invoiceList;
        }

        private List<JXInvoiceInfo> ToView(List<JXCertificateContent> list)
        {
            var invoiceList = new List<JXInvoiceInfo>();
            foreach (var item in list)
            {
                var invoice = new JXInvoiceInfo();
                invoice.InvoiceCode = item.invoiceCode;
                invoice.InvoiceNo = item.invoiceNo;
                invoice.InvoiceDate = DateTime.ParseExact(item.kprq, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                invoice.XSFSH = item.sellerTaxNo;
                invoice.XSFMC = item.sellerName;
                invoice.SE = ValueParse.GetDouble(item.taxAmount);
                invoice.HJBHSJE = ValueParse.GetDouble(item.invoiceAmount);

                invoiceList.Add(invoice);
            }
            return invoiceList;
        }

        /// <summary>
        /// 获取地区进项网址
        /// </summary>
        /// <returns></returns>
        public string GetNetLocation(string dqCode)
        {
            string ret = string.Empty;

            try
            {
                var entity = DqdmService.GetFirstEntity(x => x.DqCode == dqCode);
                if (entity != null)
                {
                    ret = entity.NetLocation;
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.GetDqNetLocationError, ex);
            }
            return ret;
        }


        /// <summary>
        /// 批量获取发票状态
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public List<JXInvoiceStatusContent> GetInvoiceStatus(string fplist)
        {
            string errorMsg = string.Empty;
            try
            {
                string strRequest = new JavaScriptSerializer().Serialize(
                    new JXInvoiceStatusRequest
                    {
                        fplb = fplist,
                    });
                bool result = false;
                var response = WSInterface.GetResponse(strRequest, InterfaceType.JXInvoiceStatus, ref result, out errorMsg);

                if (result)
                {
                    var list = new JsonSerializer().Deserialize<List<JXInvoiceStatusContent>>(new JsonTextReader(new StringReader(response)));
                    return list;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.GetJXWarnMonthError, ex);
            }
            return null;
        }

    }
}
