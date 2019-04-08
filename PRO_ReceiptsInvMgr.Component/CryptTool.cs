using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Core.Utilites;
using PRO_ReceiptsInvMgr.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace PRO_ReceiptsInvMgr.Component
{
    public static class CryptTool
    {
        /// <summary>
        /// COM组件对象
        /// </summary>
        private static dynamic CryptToolInstance;
        static CryptTool()
        {
            try
            {
                CryptToolInstance = Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("3C474273-7F8B-4690-8C34-855C4528658D")));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error(typeof(CryptTool), string.Format("创建CryptTool Component实例出现异常，详情为：{0}", e.Message));
            }
        }

        private static string ymbb = "3.2.01";
        private static string funType = "01";
        private static string loginUrl = PRO_ReceiptsInvMgr.Resources.Common.JXTokenUrl + ConfigHelper.GetAppSettingValue("JXLoginUri");
        private static string queryUrl = PRO_ReceiptsInvMgr.Resources.Common.JXTokenUrl + ConfigHelper.GetAppSettingValue("JXQueryUri");
        private static string serverPacket = string.Empty;
        private static string serverRandom = string.Empty;
        private static string clientAuthCode = string.Empty;

        public static string Ymbb
        {
            get
            {
                return ymbb;
            }

            set
            {
                ymbb = value;
            }
        }

        public static string FunType
        {
            get
            {
                return funType;
            }

            set
            {
                funType = value;
            }
        }

        public static string UserPin
        {
            get; set;
        }

        public static string ErrorMsg
        {
            get; set;
        }

        public static string Nsrsbh
        {
            get; set;
        }

        public static string Nsrmc
        {
            get; set;
        }

        public static string Dqrq
        {
            get; set;
        }

        public static string Dqdm
        {
            get; set;
        }

        public static string Token
        {
            get; set;
        }

        public static string LoginUrl
        {
            get
            {
                return loginUrl;
            }

            set
            {
                loginUrl = value;
            }
        }

        public static string QueryUrl
        {
            get
            {
                return queryUrl;
            }

            set
            {
                queryUrl = value;
            }
        }

        public static int openThisDevice()
        {
            if (CryptToolInstance == null)
            {
                ErrorMsg = "调用组件出现异常！";
                return -1;
            }

            if (CryptToolInstance.IsDeviceOpened() != 0)
            {
                CryptToolInstance.CloseDevice();
            }
            CryptToolInstance.OpenDeviceEx(UserPin);
            if (CryptToolInstance.ErrCode == 0x57)
            {
                CryptToolInstance.OpenDeviceEx(UserPin);
            }
            if (CryptToolInstance.ErrCode != 0 && CryptToolInstance.ErrCode != -1)
            {
                ErrorMsg = CryptToolInstance.ErrMsg;
                Log4NetHelper.Error(typeof(CryptTool), CryptToolInstance.ErrMsg);
                return CryptToolInstance.ErrCode;
            }
            return CryptToolInstance.ErrCode;
        }

        public static void getThisCert()
        {
            var rtn = openThisDevice();

            if (rtn != 0)
            {
                return;
            }

            CryptToolInstance.GetCertInfo("", 71);
            var error = CryptToolInstance.errCode;

            if (error == 0)
            {
                Nsrsbh = CryptToolInstance.strResult;
                CryptToolInstance.GetUserInfo();
                Nsrmc = CryptToolInstance.strResult;
            }
            CryptToolInstance.CloseDevice();
        }



        public static int MakeClientHello()
        {
            var dwFlag = 0;
            CryptToolInstance.ClientHello(dwFlag);
            if (CryptToolInstance.ErrCode != 0)
            {
                ErrorMsg = CryptToolInstance.ErrMsg;
                Log4NetHelper.Error(typeof(CryptTool), CryptToolInstance.ErrMsg);
            }
            return CryptToolInstance.ErrCode;
        }

        public static bool Login(string pwd)
        {
            ErrorMsg = string.Empty;
            UserPin = pwd;
            var rtn = openThisDevice();
            if (rtn != 0)
            {
                return false;
            }

            rtn = MakeClientHello();
            if (rtn != 0)
            {
                return false;
            }
            var clientHello = CryptToolInstance.strResult;
            var param1 = "type=CLIENT-HELLO&clientHello=" + clientHello + "&ymbb=" + Ymbb;
            if (firstLogin(param1) && secondLogin())
            {
                return true;
            }
            return false;
        }


        private static bool firstLogin(string param1)
        {
            var ret = false;
            try
            {
                Log4NetHelper.Info(typeof(CryptTool), "首次访问外网服务...");
                var response = HttpWebResponseUtility.CreatePostHttpResponse(LoginUrl, param1, Encoding.UTF8, 3);

                LoginResponse firstLoginResponse = null;
                Regex reg = new Regex(@"\(([^)]*)\)");
                Match m = reg.Match(response);
                if (m.Success)
                {
                    var result = m.Result("$1");
                    firstLoginResponse = new JavaScriptSerializer().Deserialize<LoginResponse>(result);
                }

                if (firstLoginResponse == null)
                {
                    ErrorMsg = "系统异常！";
                    Log4NetHelper.Error(typeof(CryptTool), "转化FirstLoginResponse实体异常，响应报文：" + response);
                    return false;
                }

                Log4NetHelper.Info(typeof(CryptTool), "首次访问外网服务正常");
                if (firstLoginResponse.key1 == "00")
                {
                    ErrorMsg = "服务器调用身份认证失败！";
                }
                else if (firstLoginResponse.key1 == "01")
                {
                    ret = true;
                    serverPacket = firstLoginResponse.key2;
                    serverRandom = firstLoginResponse.key3;
                    MakeClientAuthCode();
                }
                else
                {
                    ErrorMsg = "系统异常!";
                    Log4NetHelper.Error(typeof(CryptTool), "系统异常！响应报文:" + response);

                }
            }
            catch (Exception ex)
            {
                ret = false;
                ErrorMsg = "首次访问外网服务异常！";
                Log4NetHelper.Error(typeof(CryptTool), "首次访问外网服务异常" + ex.Message);
            }
            return ret;
        }

        private static bool validateNsrsbh(string nsrsbh)
        {
            return Regex.IsMatch(nsrsbh, "^[0-9a-zA-Z]+$");
        }

        private static bool secondLogin()
        {
            var ret = false;
            try
            {
                Log4NetHelper.Info(typeof(CryptTool), "第二次访问外网服务...");
                getThisCert();
                var cert = Nsrsbh;
                if (!validateNsrsbh(cert))
                {
                    ErrorMsg = string.Format("读取到的纳税人信息（纳税人识别号：{0}）不合法", cert);
                    return false;
                }

                //////////////////////////////////////////////////////////////////////////
                var param1 = "cert=" + cert + "&funType=" + FunType;
                var response = HttpWebResponseUtility.CreatePostHttpResponse(QueryUrl, param1, Encoding.UTF8, 3);
               
                QueryResponse queryResponse = null;
                Regex reg = new Regex(@"\(([^)]*)\)");
                Match m = reg.Match(response);
                if (m.Success)
                {
                    var result = m.Result("$1");
                    queryResponse = new JavaScriptSerializer().Deserialize<QueryResponse>(result);
                    var publickey = "";

                    if (queryResponse == null)
                    {
                        ErrorMsg = "系统异常！";
                        Log4NetHelper.Error(typeof(CryptTool), "转化QueryResponse实体异常，响应报文：" + response);
                        return false;
                    }

                    var param2 = "type=CLIENT-AUTH&clientAuthCode=" + clientAuthCode + "&serverRandom=" + serverRandom + "&password=&ts=" + queryResponse.ts + "&publickey=" + publickey + "&cert=" + cert + "&ymbb=" + Ymbb;

                    var secondLoginResponse = HttpWebResponseUtility.CreatePostHttpResponse(LoginUrl, param2, Encoding.UTF8, 3);

                    LoginResponse secondLoginResponseObj = null;
                    m = reg.Match(secondLoginResponse);
                    if (m.Success)
                    {
                        var resResult = m.Result("$1");
                        secondLoginResponseObj = new JavaScriptSerializer().Deserialize<LoginResponse>(resResult);
                    }

                    if (secondLoginResponseObj == null)
                    {
                        ErrorMsg = "系统异常！";
                        Log4NetHelper.Error(typeof(CryptTool), "转化SecondLoginResponse实体异常，响应报文：" + response);
                        return false;
                    }

                    Log4NetHelper.Info(typeof(CryptTool), "第二次访问外网服务正常");

                    var rezt = secondLoginResponseObj.key1;
                    if (rezt == "00")
                    {
                        string msg = secondLoginResponseObj.key2;
                        if (msg.Contains("函数调用失败"))
                        {
                            msg = "网络延迟，请重试";
                        }

                        ErrorMsg = string.Format("登录失败！{0}", msg);
                    }
                    else if (rezt == "03")
                    {
                        Token = secondLoginResponseObj.key2;
                        Nsrmc = HttpUtility.UrlDecode(secondLoginResponseObj.key3);
                        Dqrq = secondLoginResponseObj.key4;
                        ret = true;
                    }
                    else if (rezt == "02")
                    {
                        ErrorMsg = string.Format("纳税人档案（税号：{0}）信息不存在！请确认本企业是否属于取消认证政策的纳税人。如是，则请联系主管税务机关进行核实和补录相关档案信息！", Nsrsbh);
                    }
                    else if (rezt == "12")
                    {
                        var xyjb = secondLoginResponseObj.key5;
                        if (xyjb == "" || xyjb == "null")
                        {
                            xyjb = "未设置";
                        }
                        ErrorMsg = string.Format("纳税人档案信息为（税号：{0}；信用等级：{1}）！请确认本企业是否属于取消认证政策的纳税人。如是，则请联系主管税务机关进行核实和修改相关档案信息！", Nsrsbh, xyjb);
                    }
                    else if (rezt == "13")
                    {
                        ErrorMsg = string.Format("纳税人档案信息为（税号：{0}）为特定企业！特定企业不允许进行网上发票认证！如有疑问，请联系主管税务机关进行核实和修改相关档案信息！", Nsrsbh);
                    }
                    else if (rezt == "06")
                    {
                        ErrorMsg = string.Format("纳税人档案（税号：{0}）当前状态为已注销，请联系主管税务机关核实相关档案信息！", Nsrsbh);
                    }
                    else if (rezt == "21")
                    {
                        var xyjb = secondLoginResponseObj.key4;
                        if (xyjb == "" || xyjb == "null")
                        {
                            xyjb = "未设置";
                        }
                        ErrorMsg = string.Format("纳税人档案信息为(税号：{0})档案信息存在，当前信用级别为：" + xyjb + ",本平台启用状态为：未启用,无权登录此系统，请联系主管税务机关开通权限！", Nsrsbh);
                    }
                    else if (rezt == "22" || rezt == "23" || rezt == "400000" || rezt == "98" || rezt == "99" || rezt == "101")
                    {
                        ErrorMsg = string.Format("系统异常，错误代码为：{0},请重新登录", rezt);
                    }
                    else if (rezt == "400001")
                    {
                        ErrorMsg = string.Format("您为转登记纳税人，未获取到您的认定时间起，请联系您的服务单位！");
                    }
                    else if (rezt == "400002")
                    {
                        ErrorMsg = string.Format("您为转登记纳税人，未获取到您的认定时间止，请联系您的服务单位！");
                    }
                    else if (rezt == "09")
                    {
                        ErrorMsg = string.Format("会话已超时，请重新登陆！");
                    }
                    else if (rezt == "666")
                    {
                        ErrorMsg = string.Format("客户端请求参数不完整，请稍后重试");
                    }
                    else if (rezt == "777")
                    {
                        ErrorMsg = string.Format("客户端请求异常，请稍后重试！");
                    }
                    else if (rezt == "888")
                    {
                        ErrorMsg = string.Format("您操作过于频繁，请稍后再试！");
                    }
                    else
                    {
                        ErrorMsg = string.Format("系统异常，错误代码为：{0}", rezt);
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
                ErrorMsg = "第二次访问外网服务异常！";
                Log4NetHelper.Error(typeof(CryptTool), "第二次访问外网服务异常" + ex.Message);
            }
            return ret;
        }

        private static void MakeClientAuthCode()
        {
            var err = 0;
            err = openThisDevice();
            if (err != 0)
            {
                return;
            }
            CryptToolInstance.ClientAuth(serverPacket);
            if (CryptToolInstance.ErrCode != 0)
            {
                ErrorMsg = CryptToolInstance.ErrMsg;
                Log4NetHelper.Error(typeof(CryptTool), CryptToolInstance.ErrMsg);
            }
            clientAuthCode = CryptToolInstance.strResult;
            CryptToolInstance.CloseDevice();
        }
    }

    public class LoginResponse
    {
        public string key1 { get; set; }
        public string key2 { get; set; }

        public string key3 { get; set; }

        public string key4 { get; set; }

        public string key5 { get; set; }
    }

    public class QueryResponse
    {
        public string page { get; set; }
        public string ts { get; set; }

    }


}
