using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//Token.dll调用获取token  20190309
//大象动态库接口调用 参数顺序： 证书口令 char*，地区编码 char* ，税号 char* ，版本号 char*，token char*
namespace PRO_ReceiptsInvMgr.Client.Helper
{
    class GetTokenHelper
    {
        [DllImport("Token.dll", EntryPoint = "GetToken", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, 
            CallingConvention = CallingConvention.StdCall)]
        private static extern int GetToken(string taxcode, string certnum, string areacode, string bbh, StringBuilder outtoken);

        public static string ErrorMsg { get; set; }
        public static string retCode { get; set; }

        public static string GetToken_dll(string nsrsbh, string certnum, string areacode, string bbh)
        {
            string rettoken = string.Empty;
            StringBuilder jsonStr = new StringBuilder(512);

            int n = GetToken(certnum, areacode, nsrsbh, bbh, jsonStr);
            //             if (n != 0)
            //             {
            //                 ErrorMsg = "网络通讯失败";
            //                 return rettoken;
            //             }
            
            if (jsonStr.Length > 0)
            {
                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(jsonStr.ToString());
                retCode = jsonObj["retCode"].ToString();
                ErrorMsg = jsonObj["retMess"].ToString();
                rettoken = jsonObj["token"].ToString();
           }

            return rettoken;
        }
    }
}
