using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Core.Utilites;
using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Model.Tables;
using System.Configuration;
using Microsoft.Win32;
using PRO_ReceiptsInvMgr.Resources;
using PRO_ReceiptsInvMgr.Domain.Enum;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using System.Web.Script.Serialization;
using PRO_ReceiptsInvMgr.Domain.Mapper;
using System.Windows.Media.Imaging;

namespace PRO_ReceiptsInvMgr.Application
{
    public class FpcyService : BaseService
    {
        /// <summary>
        /// 发票查验，成功保存数据库
        /// </summary>
        /// <returns></returns>
        public FpcyResult DoFPZY(string request, ref string errorMsg)
        {
            var ret = FpcyResult.FailNotSave;
            bool result = false;

            var response = WSInterface.GetResponse(request, InterfaceType.FPCY, ref result,out errorMsg);
            if (!result)
            {
                return ret;
            }

            var responseObj = new JavaScriptSerializer().Deserialize<FpcyResponse>(response);
            if (responseObj.invoiceList != null && responseObj.invoiceList.Any())
            {
                var invoiceInfo = responseObj.invoiceList[0].invoiceInfo;
                if (invoiceInfo != null)
                {
                    var resultCode = (FpcyResultCode)Convert.ToInt32(invoiceInfo.resultCode);

                    if (resultCode == FpcyResultCode.CheckSuccess || resultCode == FpcyResultCode.CheckInvoiceNotSame || resultCode == FpcyResultCode.CheckInvoiceNotExist)
                    {
                        FpcyDataObject fpcyObject = new FpcyDataObject();
                        fpcyObject.InvoiceCode = invoiceInfo.invoiceCode;
                        fpcyObject.InvoiceNo = invoiceInfo.invoiceNo;
                        fpcyObject.InvoiceDate = invoiceInfo.invoiceDate;
                        fpcyObject.TotalAmount = invoiceInfo.totalAmount;
                        fpcyObject.CheckCode = invoiceInfo.checkCode;
                        fpcyObject.InvoiceType = invoiceInfo.invoiceType;
                        fpcyObject.InvoiceData = response;
                        fpcyObject.OperateDate = DateTime.Now;
                        fpcyObject.ResultCode = invoiceInfo.resultCode;
                        var entity = fpcyObject.ToEntity<TFpcy>();
                        FpcyService.AddEntities(entity);

                        if (resultCode == FpcyResultCode.CheckSuccess)
                        {
                            ret = FpcyResult.SuccessAndSave;
                        }
                        else
                        {
                            errorMsg = string.Format("{0}({1})", invoiceInfo.resultTip, invoiceInfo.resultCode);
                            ret = FpcyResult.FailAndSave;
                        }
                    }
                    else
                    {
                        errorMsg = string.Format("{0}({1})", invoiceInfo.resultTip,invoiceInfo.resultCode);
                        ret = FpcyResult.FailNotSave;
                    }
                }
            }

            return ret;
        }

        public List<FpcyDataObject> GetFPCYData(ref string errorMsg)
        {
          
            List<FpcyDataObject> fpcyList = new List<FpcyDataObject>();
            try
            {
                var entities = FpcyService.LoadEntities(x => true).OrderByDescending(x => x.OperateDate).Take(20);
                fpcyList = entities.ToDataObjectList<FpcyDataObject>();
                fpcyList.ForEach(((o) =>
                {
                    DateTime dt = DateTime.ParseExact(o.InvoiceDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    o.InvoiceDate = dt.ToString("yyyy-MM-dd");
                    var invoiceType = (FpcyInvoiceType)Convert.ToInt32(o.InvoiceType);
                    o.InvoiceTypeDescription = invoiceType.GetDescription();
                    if (invoiceType == FpcyInvoiceType.ZYFP)
                    {
                        o.FpzlImageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoFpcyZhuan,UriKind.Relative));
                    }
                    else if (invoiceType == FpcyInvoiceType.PTFP)
                    {
                        o.FpzlImageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoFpcyPu, UriKind.Relative));
                    }
                    else if (invoiceType == FpcyInvoiceType.DZFP)
                    {
                        o.FpzlImageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoFpcyDian, UriKind.Relative));
                    }
                    else if (invoiceType == FpcyInvoiceType.GSFP)
                    {
                        o.FpzlImageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoFpcyJuan, UriKind.Relative));
                    }
                    else if (invoiceType == FpcyInvoiceType.TXFFP)
                    {
                        o.FpzlImageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoFpcyTong, UriKind.Relative));
                    }
                    else if (invoiceType == FpcyInvoiceType.JDC)
                    {
                        o.FpzlImageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoFpcyJDC, UriKind.Relative));
                    }
                    //else if (invoiceType == FpcyInvoiceType.HYYS)
                    //{
                    //    o.FpzlImageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoFpcyHYYS, UriKind.Relative));
                    //}
                     
                    var resultCode = (FpcyResultCode)Convert.ToInt32(o.ResultCode);
                    if (resultCode == FpcyResultCode.CheckSuccess)
                    {
                        o.CYSuccessVisible = System.Windows.Visibility.Visible;
                        o.CYNotFindVisible = System.Windows.Visibility.Hidden;
                        o.CYNotSameVisible = System.Windows.Visibility.Hidden;
                      
                    }
                    else if (resultCode == FpcyResultCode.CheckInvoiceNotExist)
                    {
                        o.CYSuccessVisible = System.Windows.Visibility.Hidden;
                        o.CYNotFindVisible = System.Windows.Visibility.Visible;
                        o.CYNotSameVisible = System.Windows.Visibility.Hidden;
                    }
                    else if (resultCode == FpcyResultCode.CheckInvoiceNotSame)
                    {
                        o.CYSuccessVisible = System.Windows.Visibility.Hidden;
                        o.CYNotFindVisible = System.Windows.Visibility.Hidden;
                        o.CYNotSameVisible = System.Windows.Visibility.Visible;
                    }
                   
                }));
            }
            catch (Exception ex)
            {
                errorMsg = Resources.Message.GetInvoiceDataError;
                Logging.Log4NetHelper.Error(this, Resources.Message.GetInvoiceDataError, ex);
            }

            return fpcyList;
        }
    }
}
