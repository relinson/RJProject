using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PRO_ReceiptsInvMgr.Core.Helper
{
    public static class HttpWebResponseUtility
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";


        /// <summary>
        /// get请求带重试
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public static string CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies, int sum)
        {
            string temp = null;
            for (int i = 0; i < sum; i++)
            {
                if (string.IsNullOrEmpty(temp) || string.IsNullOrEmpty(temp.Trim()))
                {
                    temp = CreateGetHttpResponse(url, timeout, userAgent, cookies);
                }
                else
                {
                    break;
                }
            }
            return temp;
        }
        /// <summary>
        /// post请求带重试
        /// </summary>
        /// <returns></returns>
        public static string CreatePostHttpResponse(string url, string d, Encoding requestEncoding, int sum, int? timeout = null, string userAgent = "", CookieCollection cookies = null)
        {
            string temp = null;
            for (int i = 0; i < sum; i++)
            {
                if (string.IsNullOrEmpty(temp) || string.IsNullOrEmpty(temp.Trim()))
                {
                    temp = CreatePostHttpResponse(url, d, requestEncoding, timeout, userAgent, cookies);
                }
                else
                {
                    break;
                }
            }
            return temp;
        }

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static string CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
        {
            var retStr = string.Empty;
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            var httpWebResponse = request.GetResponse() as HttpWebResponse;
            using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("GBK")))
            {
                retStr = sr.ReadToEnd();
            }

            return retStr;
        }
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="d"></param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static string CreatePostHttpResponse(string url, string d, Encoding requestEncoding, int? timeout=null, string userAgent="",  CookieCollection cookies= null)
        {
            HttpWebResponse httpWebResponse = null;
            HttpWebRequest request = null;
            string retStr = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException("url");
                }
                if (requestEncoding == null)
                {
                    throw new ArgumentNullException("requestEncoding");
                }
                //如果是发送HTTPS请求  
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

                if (!string.IsNullOrEmpty(userAgent))
                {
                    request.UserAgent = userAgent;
                }
                else
                {
                    request.UserAgent = DefaultUserAgent;
                }

                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                }
                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }
                //如果需要POST数据  
                if (!string.IsNullOrEmpty(d))
                {

                    byte[] data = requestEncoding.GetBytes(d);
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                 httpWebResponse = request.GetResponse() as HttpWebResponse;
                 
                using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream(),Encoding.GetEncoding("GBK")))
                {
                    retStr = sr.ReadToEnd();
                }
            }
            finally
            {
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
            }

            return retStr;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }
}
