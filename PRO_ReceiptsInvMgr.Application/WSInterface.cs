using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using static PRO_ReceiptsInvMgr.Domain.DataObjects.UpGlobalDataObject;
using PRO_ReceiptsInvMgr.Core.Helper;
using System.Web.Script.Serialization;
using PRO_ReceiptsInvMgr.Core.Utilites;
using System.IO;
using PRO_ReceiptsInvMgr.Core.Security;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using PRO_ReceiptsInvMgr.Domain.Enum;
using PRO_ReceiptsInvMgr.Resources;
using System.Threading;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Component;

namespace PRO_ReceiptsInvMgr.Application
{
    public class WSInterface
    {
        protected WSInterface() { }

        /// <summary>
        /// 调用服务端接口并返回Content
        /// </summary>
        /// <param name="strRequest">请求Json字符串</param>
        /// <param name="InterfaceType">接口枚举</param>
        /// <param name="result">是否成功</param>
        /// <returns>Content值</returns>
        public static string GetResponse(string strRequest, InterfaceType InterfaceType, ref bool result, out string errorMsg, int timeOut = 60000)
        {
            string strContent = string.Empty;
            errorMsg = string.Empty;
            try
            {
                Logging.Log4NetHelper.Debug(typeof(WSInterface), strRequest);

                string requestDownLoadXML = GlobalBW(strRequest);
                if (!string.IsNullOrEmpty(requestDownLoadXML))
                {
                    requestDownLoadXML = System.Web.HttpUtility.UrlEncode(requestDownLoadXML, System.Text.Encoding.UTF8);
                    string fpqzResponse = CommonHelper.HttpPost(ConfigHelper.GetAppSettingValue(InterfaceType.ToString()), requestDownLoadXML, timeOut);

                    JavaScriptSerializer jserial = new JavaScriptSerializer();
                    GeneralInfo dyfpqzResponse = jserial.Deserialize<GeneralInfo>(fpqzResponse);

                    if (dyfpqzResponse.state.returnCode.ToString() != "0000")
                    {
                        result = false;

                        //errorMsg = string.Format("{0}({1})", dyfpqzResponse.state.returnMessage, dyfpqzResponse.state.returnCode);
                        errorMsg = string.Format("{0}", dyfpqzResponse.state.returnMessage);
                        Logging.Log4NetHelper.Debug(typeof(WSInterface), string.Format(Message.GetResponseFailed, InterfaceType.GetDescription(), dyfpqzResponse.state.returnCode, dyfpqzResponse.state.returnMessage));
                    }
                    else
                    {
                        result = true;
                    }

                    if (!string.IsNullOrEmpty(dyfpqzResponse.content))
                    {
                        strContent = GetRetContent(dyfpqzResponse, InterfaceType);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Logging.Log4NetHelper.Error(typeof(WSInterface), string.Format(Message.GetResponseException, InterfaceType.GetDescription()) + ex.Message + System.Environment.NewLine + ex.StackTrace);
                strContent = ex.Message;

                if (ex.Message.Contains("timeout") ||
                  ex.Message.Contains("timed out"))
                {
                    strContent = PRO_ReceiptsInvMgr.Resources.Message.TimeOutError;
                }
                else if (ex.Message.Contains("No connection could be made") ||
                    ex.Message.Contains("Unable to connect") || ex.Message.Contains("Not Found"))
                {
                    strContent = PRO_ReceiptsInvMgr.Resources.Message.ConnectError;
                }
                else if (ex.Message.Contains("Internal Server Error"))
                {
                    strContent = PRO_ReceiptsInvMgr.Resources.Message.InternalError;
                }
                errorMsg = strContent;
            }

            return strContent;
        }

        /// <summary>
        /// 调用服务端接口并获取返回报文对象
        /// </summary>
        /// <param name="InterfaceType">接口枚举</param>
        /// <param name="result">是否上传或下载成功</param>
        /// <returns></returns>
        public static GeneralInfo GetResponseObject(string strRequest, InterfaceType InterfaceType)
        {
            GeneralInfo dyfpqzResponse = null;
            try
            {
                string requestDownLoadXML = GlobalBW(strRequest);
                if (!string.IsNullOrEmpty(requestDownLoadXML))
                {
                    requestDownLoadXML = System.Web.HttpUtility.UrlEncode(requestDownLoadXML, System.Text.Encoding.UTF8);

                    string fpqzResponse = CommonHelper.HttpPost(ConfigHelper.GetAppSettingValue(InterfaceType.ToString()).ToString(), requestDownLoadXML);
                    JavaScriptSerializer jserial = new JavaScriptSerializer();
                    dyfpqzResponse = jserial.Deserialize<GeneralInfo>(fpqzResponse);

                    if (dyfpqzResponse.state.returnCode.ToString() != "0000")
                    {
                        Logging.Log4NetHelper.Error(typeof(WSInterface), string.Format(Message.GetResponseFailed, InterfaceType.GetDescription(), dyfpqzResponse.state.returnCode, dyfpqzResponse.state.returnMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(WSInterface), string.Format(Message.GetResponseException, InterfaceType.GetDescription()) + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw;
            }
            return dyfpqzResponse;
        }


        /// <summary>
        /// 通用报文
        /// </summary>
        /// <param name="inputErpXmlSource">报文的Content内容</param>
        /// <returns></returns>
        public static string GlobalBW(string erpXmlSource)
        {
            GeneralInfo globalInfo = new GeneralInfo();

            //XML格式化参数
            globalInfo.appId = GlobalInfo.AppId;
            globalInfo.version = "1.0";
            globalInfo.encryptCode = ConfigHelper.GetAppSettingValue("EncryptCode");
            string requestDownLoadXML = string.Empty;
            globalInfo.passWord = CommonHelper.GetPassWord(Resources.Common.PassWord);
           
            try
            {
                //Test temp set to 3DES
                string encodeOrgXMLSource = CommonHelper.Get3DESEncodeContent(globalInfo.passWord, erpXmlSource);
                globalInfo.content = encodeOrgXMLSource;
                globalInfo.state = new ReturnStateInfo();
                requestDownLoadXML = new JavaScriptSerializer().Serialize(globalInfo);
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(WSInterface), Message.GetGlobalFailed + ex.Message + System.Environment.NewLine + ex.StackTrace);

            }
            return requestDownLoadXML;
        }

        /// <summary>
        /// 获取接口返回Content
        /// </summary>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static string GetRetContent(GeneralInfo reponseObj, InterfaceType InterfaceType)
        {
            string strContent = string.Empty;
            try
            {
                if (reponseObj != null && !string.IsNullOrEmpty(reponseObj.content))
                {
                    strContent = CommonHelper.Get3DESDecodeContent(reponseObj.passWord, reponseObj.content);
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(WSInterface), string.Format(Message.DecodeFailed, InterfaceType.GetDescription()) + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return strContent;
        }


        /// <summary>
        /// 调用服务端接口并返回Content
        /// </summary>
        /// <param name="strRequest">请求Json字符串</param>
        /// <param name="InterfaceType">接口枚举</param>
        /// <param name="result">是否上传或下载成功</param>
        /// <returns>Content值</returns>
        public static string GetServerResponse(string strRequest, InterfaceType InterfaceType, ref bool result)
        {
            string strContent = string.Empty;
            try
            {
                string requestDownLoadXML = GlobalBW(strRequest);
                if (!string.IsNullOrEmpty(requestDownLoadXML))
                {
                    requestDownLoadXML = System.Web.HttpUtility.UrlEncode(requestDownLoadXML, System.Text.Encoding.UTF8);

                    int timeout = 10000;

                    string fpqzResponse = CommonHelper.HttpPost(ConfigHelper.GetAppSettingValue(InterfaceType.ToString()).ToString(), requestDownLoadXML, timeout);
                    JavaScriptSerializer jserial = new JavaScriptSerializer();
                    GeneralInfo dyfpqzResponse = jserial.Deserialize<GeneralInfo>(fpqzResponse);

                    if (dyfpqzResponse.state.returnCode.ToString() != "0000")
                    {
                        result = false;
                        Logging.Log4NetHelper.Error(typeof(WSInterface), string.Format(Message.GetResponseFailed, InterfaceType.GetDescription(), dyfpqzResponse.state.returnCode, dyfpqzResponse.state.returnMessage));
                    }
                    else
                    {
                        result = true;
                    }

                    if (!string.IsNullOrEmpty(dyfpqzResponse.content))
                    {
                        strContent = GetRetContent(dyfpqzResponse, InterfaceType);
                    }
                }

            }
            catch (Exception ex)
            {
                result = false;
                Logging.Log4NetHelper.Error(typeof(WSInterface), string.Format(Message.GetResponseException, InterfaceType.GetDescription()) + ex.Message + System.Environment.NewLine + ex.StackTrace);

                if (ex.Message.Contains("timeout") ||
                    ex.Message.Contains("timed out"))
                {
                    strContent = PRO_ReceiptsInvMgr.Resources.Message.TimeOutError;
                }
                else if (ex.Message.Contains("No connection could be made") ||
                    ex.Message.Contains("Unable to connect"))
                {
                    strContent = PRO_ReceiptsInvMgr.Resources.Message.ConnectError;
                }
                else if (ex.Message.Contains("Internal Server Error"))
                {
                    strContent = PRO_ReceiptsInvMgr.Resources.Message.InternalError;
                }

            }
            return strContent;
        }

        private static double GetVersionToDouble(string version)
        {
            double versionNumber = 0;
            try
            {
                string strVersion = System.Text.RegularExpressions.Regex.Replace(version, @"[^0-9]+", "");

                double.TryParse(strVersion.Substring(0, strVersion.Length), out versionNumber);

            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(WSInterface), ex);
                throw;
            }
            return versionNumber;
        }
 
        #region InterfaceTest
        /// <summary>
        /// 调用服务端接口并返回Content
        /// </summary>
        /// <param name="strRequest">请求Json字符串</param>
        /// <param name="InterfaceType">接口枚举</param>
        /// <param name="result">是否成功</param>
        /// <returns>Content值</returns>
        public static string GetResponse2(string strRequest, string url, ref bool result, out string errorMsg, out string response)
        {
            string strContent = string.Empty;
            response = string.Empty;
            errorMsg = string.Empty;
            try
            {
                string requestDownLoadXML = GlobalBW(strRequest);
                if (!string.IsNullOrEmpty(requestDownLoadXML))
                {
                    requestDownLoadXML = System.Web.HttpUtility.UrlEncode(requestDownLoadXML, System.Text.Encoding.UTF8);
                    response = CommonHelper.HttpPost(url, requestDownLoadXML);
                    JavaScriptSerializer jserial = new JavaScriptSerializer();
                    GeneralInfo dyfpqzResponse = jserial.Deserialize<GeneralInfo>(response);

                    if (dyfpqzResponse.state.returnCode.ToString() != "0000")
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }

                    if (!string.IsNullOrEmpty(dyfpqzResponse.content))
                    {
                        strContent = Encoding.UTF8.GetString(Convert.FromBase64String(dyfpqzResponse.content));
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                errorMsg = ex.Message;

                if (ex.Message.Contains("timeout") ||
                  ex.Message.Contains("timed out"))
                {
                    errorMsg = PRO_ReceiptsInvMgr.Resources.Message.TimeOutError;
                }
                else if (ex.Message.Contains("No connection could be made") ||
                    ex.Message.Contains("Unable to connect") || ex.Message.Contains("Not Found"))
                {
                    errorMsg = PRO_ReceiptsInvMgr.Resources.Message.ConnectError;
                }
                else if (ex.Message.Contains("Internal Server Error"))
                {
                    errorMsg = PRO_ReceiptsInvMgr.Resources.Message.InternalError;
                }
            }

            return strContent;
        }

        /// <summary>
        /// 获取接口返回Content
        /// </summary>
        /// <param name="retMessage"></param>
        /// <returns></returns>
        public static string GetRetContent(GeneralInfo reponseObj)
        {
            string strContent = string.Empty;
            try
            {
                if (reponseObj != null && !string.IsNullOrEmpty(reponseObj.content))
                {
                    strContent = CommonHelper.Get3DESDecodeContent(reponseObj.passWord, reponseObj.content);
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(WSInterface), ex);
                throw;
            }
            return strContent;
        }

        #endregion
    }
}
