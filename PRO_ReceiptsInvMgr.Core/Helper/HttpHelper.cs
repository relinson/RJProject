using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace PRO_ReceiptsInvMgr.Core.Helper
{
    public static class HttpHelper
    {
        /// <summary>
        /// get请求带重试
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public static string HttpGet(string Url, string postDataStr, int sum)
        {
            string temp = null;
            for (int i = 0; i < sum; i++)
            {
                if (string.IsNullOrEmpty(temp) || string.IsNullOrEmpty(temp.Trim()))
                {
                    temp = HttpGet(Url, postDataStr);
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
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public static string HttpPost(string Url, string postDataStr, int sum)
        {
            string temp = null;
            for (int i = 0; i < sum; i++)
            {
                if (string.IsNullOrEmpty(temp) || string.IsNullOrEmpty(temp.Trim()))
                {
                    temp = HttpPost(Url, postDataStr);
                }
                else
                {
                    break;
                }
            }
            return temp;
        }
        /// <summary>
        /// GET请求与获取结果
        /// </summary>
        public static string HttpGet(string Url, string postDataStr)
        {
            WebResponse response = null;
            Stream myResponseStream = null;
            StreamReader myStreamReader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.Timeout = 10000;
                response = (HttpWebResponse)request.GetResponse();
                myResponseStream = response.GetResponseStream();
                myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(HttpHelper), ex);
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (myResponseStream != null)
                {
                    myResponseStream.Close();
                }
                if (myStreamReader != null)
                {
                    myStreamReader.Close();
                }
            }
        }
        /// <summary>
        /// POST请求与获取结果
        /// </summary>
        public static string HttpPost(string Url, string postDataStr)
        {
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.Timeout = 10000;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postDataStr.Length;
                StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
                writer.Write(postDataStr);
                writer.Flush();
                response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码
                }
                reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                string retString = reader.ReadToEnd();
                return retString;
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(HttpHelper), ex);
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
