using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PRO_ReceiptsInvMgr.WebService
{
    public class WebServiceCaller
    {
        /// <summary>
        /// 需要WebService支持Get调用
        /// </summary>
        public static XmlDocument QueryGetWebService(String URL, String MethodName, Hashtable Pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            return ReadXmlResponse(request.GetResponse());
        }

        private static XmlDocument ReadXmlResponse(WebResponse response)
        {
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(retXml);
            return doc;
        }
        /// <summary>
        /// 获取或设置请求cookie
        /// </summary>
        public static List<string> Cookies
        {
            get;
            set;
        }

        /// <summary>
        /// 获取请求返回的 HTTP 头部内容
        /// </summary>
        public static HttpHeader HttpHeaders
        {
            get;
            internal set;
        }

        /// <summary>
        /// 代理类型
        /// </summary>
        public enum ProxyType
        {
            None = 0,
            Http = 1,
            Socks4 = 2,
            Socks5 = 3
        }
         
        private static ProxyType _proxyType;
        private static string _proxyServer;
        private static int _proxyPort; 
        private static string _proxyAccount;
        private static string _proxyPassword;


        /// <summary>
        /// 调WebServe接口
        /// </summary>
        /// <param name="webServiceUrl"></param>
        /// <param name="proxyType"></param>
        /// <param name="proxyServer"></param>
        /// <param name="proxyPort"></param>
        /// <param name="isNeedCertificate"></param>
        /// <param name="proxyAccount"></param>
        /// <param name="proxyPassword"></param>
        /// <param name="MethodName">方法名</param>
        /// <param name="paramName">参数名</param>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public static string QuerySoapWebService(
            string webServiceUrl,
            int proxyType,
            string proxyServer,
            int proxyPort,
            bool isNeedCertificate,
            string proxyAccount,
            string proxyPassword,
            string methodName, string paramName, string paramValue)
        {
            string _webServiceUrl = webServiceUrl;
            _proxyType = (ProxyType)proxyType;
            _proxyServer = proxyServer;
            _proxyPort = proxyPort;
            _proxyAccount = proxyAccount;
            _proxyPassword = proxyPassword;
            return QuerySoapWebService(_webServiceUrl, methodName, paramName, paramValue);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="webServiceUrl"></param>
        /// <param name="proxyType"></param>
        /// <param name="proxyServer"></param>
        /// <param name="proxyPort"></param>
        /// <param name="isNeedCertificate"></param>
        /// <param name="proxyAccount"></param>
        /// <param name="proxyPassword"></param>
        /// <param name="url"></param>
        /// <param name="file"></param>
        public static void DownLoadFile(
            string webServiceUrl,
            int proxyType,
            string proxyServer,
            int proxyPort,
            bool isNeedCertificate,
            string proxyAccount,
            string proxyPassword,
            string url, string file)
        {
            _proxyType = (ProxyType)proxyType;
            _proxyServer = proxyServer;
            _proxyPort = proxyPort;
            _proxyAccount = proxyAccount;
            _proxyPassword = proxyPassword;
            DownLoadFile(url, file);
        }

        /// <summary>
        /// 通用WebService调用(Soap)
        /// </summary>
        /// <param name="inputURL"></param>
        /// <param name="methodName">方法名</param>
        /// <param name="paramName">参数名</param>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        private static string QuerySoapWebService(String inputURL, String methodName, string paramName, string paramValue)
        {
           
            var url = inputURL;
            if (url.ToUpper().Contains("?WSDL"))
            {
                url = url.Substring(0, url.Length - 5);
            }
            if (_xmlNamespaces.ContainsKey(url))
            {
                return QuerySoapWebService(url, methodName, paramName, paramValue, _xmlNamespaces[url].ToString());
            }
            else
            {
                return QuerySoapWebService(url, methodName, paramName, paramValue, GetNamespace(url));
            }
        }

        /// <summary>
        /// 调用Service方法
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="MethodName"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <param name="XmlNs"></param>
        /// <returns></returns>
        private static string QuerySoapWebService(String URL, String MethodName, string paramName, string paramValue, string XmlNs)
        {
            _xmlNamespaces[URL] = XmlNs;//加入缓存，提高效率
            String retXml;
            String retX;
            XmlDocument doc = new XmlDocument();
            try
            {
                //设置SOAP信息  
                doc.LoadXml("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"></soap:Envelope>");

                AddDelaration(doc);
                XmlElement soapBody = doc.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");

                //设置SOAP数据
                XmlElement soapMethod = doc.CreateElement(MethodName);
                soapMethod.SetAttribute("xmlns", XmlNs);
                if (!string.IsNullOrEmpty(paramName))
                {
                    XmlElement soapPar = doc.CreateElement(paramName);
                    soapPar.InnerXml = ObjectToSoapXml(paramValue);
                    soapMethod.AppendChild(soapPar);
                }
                soapBody.AppendChild(soapMethod);
                doc.DocumentElement.AppendChild(soapBody);

                string postData = doc.OuterXml;
                //HTTP代理或无代理方式
                if (_proxyType == ProxyType.None || _proxyType == ProxyType.Http)
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
                    request.Method = "POST";
                    request.ContentType = "text/xml; charset=utf-8";
                    request.Headers.Add("SOAPAction", "\"" + XmlNs + (XmlNs.EndsWith("/") ? "" : "/") + MethodName + "\"");
                    SetWebRequest(request);

                    byte[] data = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = data.Length;

                    //POST数据
                    Stream writer = request.GetRequestStream();
                    writer.Write(data, 0, data.Length);
                    writer.Close();

                    //取得调用结果
                    WebResponse response = request.GetResponse();
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    retX = sr.ReadToEnd();

                    doc.LoadXml(retX);
                    sr.Close();
                }
                else
                {
                    Socket sRH = null;

                    byte[] buffer;
                    int rlength = 0;
                    string sr;

                    Uri uri = new Uri(URL + "?WSDL");

                    //连接SOCKS代理取得套接字
                    if (_proxyType == ProxyType.Socks4)
                    {
                        sRH = SocksProxy.ConnectSocks4Server(
                            _proxyServer,
                           _proxyPort,
                            uri.Host,
                            uri.Port);
                    }
                    else
                    {
                        sRH = SocksProxy.ConnectSocks5Server(
                            _proxyServer,
                            _proxyPort,
                            uri.Host,
                            uri.Port,
                            _proxyAccount,
                            _proxyPassword);
                    }
                    try
                    {
                        sRH.SendTimeout = 10000;
                        sRH.ReceiveTimeout = 10000;
                        //发送数据
                        SSend("POST " + uri.AbsolutePath + " HTTP/1.0\r\n", sRH);
                        SSend("Connection:close\r\n", sRH);
                        SSend("Host:" + uri.Host + "\r\n", sRH);
                        SSend("Content-Type: text/xml; charset=utf-8\r\n", sRH);
                        SSend("User-agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0)\r\n", sRH);
                        SSend("Accept-language:zh-cn\r\n", sRH);
                        SSend("Content-Length:" + postData.Length + "\r\n", sRH);
                        SSend("SOAPAction:" + "\"" + XmlNs + (XmlNs.EndsWith("/") ? "" : "/") + MethodName + "\"\r\n", sRH);
                        SSend("\r\n", sRH);
                        SSend(postData, sRH);

                        //接收数据
                        buffer = new byte[1024];
                        rlength = sRH.Receive(buffer);
                        sr = Encoding.Default.GetString(buffer, 0, rlength);
                        string ret = sr;
                        while (sr.Length > 0)
                        {
                            rlength = sRH.Receive(buffer);
                            sr = Encoding.Default.GetString(buffer, 0, rlength);
                            ret += sr;
                        }
                        if (ret.Length > 0)
                        {
                            string b = ret.Substring(ret.IndexOf("<soap:"));
                            doc.LoadXml(b);
                        }
                    }
                    finally
                    {
                        sRH.Shutdown(SocketShutdown.Both);
                        sRH.Close();
                    }
                }
                XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
                mgr.AddNamespace("soap", PRO_ReceiptsInvMgr.Resources.Common.WebServiceSoapUri);
                retXml = doc.SelectSingleNode("//soap:Body/*/*", mgr).InnerXml;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("timeout") ||
                    ex.Message.Contains("timed out"))
                {
                    throw new Exception("连接服务器超时，请重试！");
                }
                else if (ex.Message.Contains("No connection could be made") ||
                    ex.Message.Contains("Unable to connect"))
                {
                    throw new Exception("连接服务器失败，请稍候重试！");
                }
                else if (ex.Message.Contains("Internal Server Error"))
                {
                    throw new Exception("服务器端程序运行出现错误，请联系服务器端管理员！");
                }
                else
                {
                    throw;
                }
            }

            return retXml;
        }


        /// <summary>
        /// 获取命名空间
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        private static string GetNamespace(String URL)
        {
            string ret;
            XmlDocument doc = new XmlDocument();
            try
            {
                //HTTP代理或无代理方式
                if (_proxyType == ProxyType.None || _proxyType == ProxyType.Http)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL + "?WSDL");
                    SetWebRequest(request);
                    WebResponse response = request.GetResponse();
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    doc.LoadXml(sr.ReadToEnd());
                    sr.Close();
                }
                else//SOCKS4/5代理
                {
                    Socket sRH = null;

                    byte[] buffer;
                    int rlength = 0;
                    string sr;

                    Uri uri = new Uri(URL + "?WSDL");

                    //连接SOCKS代理取得套接字
                    if (_proxyType == ProxyType.Socks4)
                    {
                        sRH = SocksProxy.ConnectSocks4Server(
                            _proxyServer,
                            _proxyPort,
                            uri.Host,
                            uri.Port);
                    }
                    else
                    {
                        sRH = SocksProxy.ConnectSocks5Server(
                            _proxyServer,
                            _proxyPort,
                            uri.Host,
                            uri.Port,
                            _proxyAccount,
                            _proxyPassword);
                    }
                    try
                    {
                        sRH.SendTimeout = 10000;
                        sRH.ReceiveTimeout = 10000;
                        SSend("GET " + uri.PathAndQuery + " HTTP/1.0\r\n", sRH);
                        SSend("Connection:close\r\n", sRH);
                        SSend("Host:" + uri.Host + "\r\n", sRH);
                        SSend("Content-Type: text/xml; charset=utf-8\r\n", sRH);
                        SSend("User-agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0)\r\n", sRH);
                        SSend("Accept-language:zh-cn\r\n", sRH);
                        SSend("\r\n", sRH);

                        buffer = new byte[1024];
                        rlength = sRH.Receive(buffer);
                        sr = Encoding.Default.GetString(buffer, 0, rlength);
                        ret = sr;
                        while (sr.Length > 0)
                        {
                            rlength = sRH.Receive(buffer);
                            sr = Encoding.Default.GetString(buffer, 0, rlength);
                            ret += sr;
                        }
                        if (ret.Length > 0)
                        {
                            string response = ret.Substring(ret.IndexOf("<?xml"));
                            doc.LoadXml(response);
                        }
                    }
                    finally
                    {
                        sRH.Shutdown(SocketShutdown.Both);
                        sRH.Close();
                    }
                }
                XmlNode node = doc.SelectSingleNode("//@targetNamespace");
                if (node == null)
                {
                    throw new Exception("服务地址可能有误，请检查地址！");
                }
                else
                {
                    ret = doc.SelectSingleNode("//@targetNamespace").Value;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("timeout") ||
                    ex.Message.Contains("timed out"))
                {
                    throw new Exception(PRO_ReceiptsInvMgr.Resources.Message.TimeOutError);
                }
                else if (ex.Message.Contains("No connection could be made") ||
                    ex.Message.Contains("Unable to connect"))
                {
                    throw new Exception(PRO_ReceiptsInvMgr.Resources.Message.ConnectError);
                }
                else if (ex.Message.Contains("Internal Server Error"))
                {
                    throw new Exception(PRO_ReceiptsInvMgr.Resources.Message.InternalError);
                }
                else
                {
                    throw;
                }
            }
            return ret;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sRH"></param>
        private static void SSend(string str, Socket sRH)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            sRH.Send(b);
        }

        /// <summary>
        /// 转换为xml
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string ObjectToSoapXml(object o)
        {
            XmlSerializer mySerializer = new XmlSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            mySerializer.Serialize(ms, o);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(ms.ToArray()));
            ms.Dispose();
            if (doc.DocumentElement != null)
            {
                return doc.DocumentElement.InnerXml;
            }
            else
            {
                return o.ToString();
            }
        }

        /// <summary>
        /// 设置凭证与超时时间
        /// </summary>
        /// <param name="request"></param>
        private static void SetWebRequest(HttpWebRequest request)
        {
            if (_proxyType == ProxyType.Http)//Http方式代理
            {
                WebProxy proxy = new WebProxy(_proxyServer + ":" + _proxyPort, true);
                if (_proxyAccount.Trim().Length > 0 && _proxyPassword.Trim().Length > 0)
                {
                    proxy.Credentials = new System.Net.NetworkCredential(_proxyAccount, _proxyPassword, "");
                }
                else
                {
                    proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                }
                request.Proxy = proxy;
            }
            else
            {
                request.Credentials = CredentialCache.DefaultCredentials;
            }
            request.Timeout = 30000;
            request.ReadWriteTimeout = 30000000;
        }
       
        /// <summary>
        /// 加声明头部
        /// </summary>
        /// <param name="doc"></param>
        private static void AddDelaration(XmlDocument doc)
        {
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.InsertBefore(decl, doc.DocumentElement);
        }

        private static Hashtable _xmlNamespaces = new Hashtable();//缓存xmlNamespace，避免重复调用GetNamespace

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file"></param>
        private static void DownLoadFile(string url, string file)
        {
            try
            {
                //HTTP代理或无代理方式
                if (_proxyType == ProxyType.None)
                {
                    WebClient client = new WebClient();
                    client.DownloadFile(url, file);
                    client.Dispose();
                }
                else if (_proxyType == ProxyType.Http)
                {
                    System.Net.WebProxy proxy = new System.Net.WebProxy(_proxyServer + ":" + _proxyPort, true);
                    if (_proxyAccount.Trim().Length > 0 && _proxyPassword.Trim().Length > 0)
                    {
                        proxy.Credentials = new System.Net.NetworkCredential(_proxyAccount, _proxyPassword, "");
                    }
                    else
                    {
                        proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    }

                    WebClient client = new WebClient();
                    client.Proxy = proxy;
                    client.DownloadFile(url, file);
                    client.Dispose();
                }
                else
                {
                    Socket sRH = null;

                    Uri uri = new Uri(url);

                    //连接SOCKS代理取得套接字
                    if (_proxyType == ProxyType.Socks4)
                    {
                        sRH = SocksProxy.ConnectSocks4Server(
                            _proxyServer,
                           _proxyPort,
                            uri.Host,
                            uri.Port);
                    }
                    else
                    {
                        sRH = SocksProxy.ConnectSocks5Server(
                            _proxyServer,
                            _proxyPort,
                            uri.Host,
                            uri.Port,
                            _proxyAccount,
                            _proxyPassword);
                    }

                    MemoryStream result = new MemoryStream(10240);
                    try
                    {
                        byte[] send = GetSendHeaders(uri, "", "");

                        if (sRH.Connected)
                        {
                            sRH.Send(send, SocketFlags.None);
                            ProcessData(sRH, ref result);
                        }

                        result.Flush();
                    }
                    finally
                    {
                        sRH.Shutdown(SocketShutdown.Both);
                        sRH.Close();
                        result.Dispose();
                    }

                    result.Seek(0, SeekOrigin.Begin);

                    FileStream fs = File.Create(file);
                    try
                    {
                        byte[] data = result.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Close();
                    }
                    catch (Exception ex)
                    {
                        Logging.Log4NetHelper.Error(typeof(WebServiceCaller),ex);
                    }

                    sRH.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("timeout") ||
                    ex.Message.Contains("timed out"))
                {
                    throw new Exception(PRO_ReceiptsInvMgr.Resources.Message.TimeOutError);
                }
                else if (ex.Message.Contains("No connection could be made") ||
                    ex.Message.Contains("Unable to connect"))
                {
                    throw new Exception(PRO_ReceiptsInvMgr.Resources.Message.ConnectError);
                }
                else if (ex.Message.Contains("Internal Server Error"))
                {
                    throw new Exception(PRO_ReceiptsInvMgr.Resources.Message.InternalError);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 返回请求的头部内容
        /// </summary>
        /// <param name="uri">请求绝对地址</param>
        /// <param name="referer">请求来源地址,可为空</param>
        /// <param name="postData">post请求参数. 设置空值为get方式请求</param>
        /// <returns>请求头部数据</returns>
        private static byte[] GetSendHeaders(Uri uri, string referer, string postData)
        {
            string sendString = @"{0} {1} HTTP/1.1
                Accept: text/html, application/xhtml+xml, */*
                Referer: {2}
                Accept-Language: zh-CN
                User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)
                Accept-Encoding: gzip, deflate
                Host: {3}
                Connection: Keep-Alive
                Cache-Control: no-cache
                ";

            sendString = string.Format(sendString, string.IsNullOrWhiteSpace(postData) ? "GET" : "POST", uri.PathAndQuery
                , string.IsNullOrWhiteSpace(referer) ? uri.AbsoluteUri : referer, uri.Host);

            if (Cookies != null && Cookies.Count > 0)
            {
                sendString += string.Format("Cookie: {0}\r\n", string.Join("; ", Cookies.ToArray()));
            }

            if (string.IsNullOrWhiteSpace(postData))
            {
                sendString += "\r\n";
            }
            else
            {
                sendString += string.Format(@"Content-Type: application/x-www-form-urlencoded
                    Content-Length: {0} {1}", postData.Length, postData);
            }

            return Encoding.UTF8.GetBytes(sendString);
        }

        /// <summary>
        /// 处理请求返回的数据.
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="reader">数据源实例</param>
        /// <param name="body">保存数据的流</param>
        private static void ProcessData<T>(T reader, ref MemoryStream body)
        {
            byte[] data = new byte[10240];
            int readLength = 0;

            int bodyStart = GetHeaders(reader, ref data, ref readLength);

            if (bodyStart >= 0)
            {
                if (HttpHeaders.IsChunk)
                {
                    GetChunkData(reader, ref data, ref bodyStart, ref readLength, ref body);
                }
                else
                {
                    GetBodyData(reader, ref data, bodyStart, readLength, ref body);
                }
            }
        }

        /// <summary>
        /// 取得返回的http头部内容,并设置相关属性.
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="reader">数据源实例</param>
        /// <param name="data">待处理的数据</param>
        /// <param name="readLength">读取的长度</param>
        /// <returns>数据内容的起始位置,返回-1表示未读完头部内容</returns>
        private static int GetHeaders<T>(T reader, ref byte[] data, ref int readLength)
        {
            int result = -1;
            StringBuilder sb = new StringBuilder(1024);

            do
            {
                readLength = ReadData(reader, ref data);

                if (result < 0)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        char c = (char)data[i];
                        sb.Append(c);

                        if (c == '\n' && string.Concat(sb[sb.Length - 4], sb[sb.Length - 3], sb[sb.Length - 2], sb[sb.Length - 1]).Contains("\r\n\r\n"))
                        {
                            result = i + 1;
                            SetThisHeaders(sb.ToString());
                            break;
                        }
                    }
                }

                if (result >= 0)
                {
                    break;
                }
            }
            while (readLength > 0);

            return result;
        }

        /// <summary>
        /// 设置此类的字段
        /// </summary>
        /// <param name="headText">头部文本</param>
        private static void SetThisHeaders(string headText)
        {
            if (string.IsNullOrWhiteSpace(headText))
            {
                throw new ArgumentNullException("headText");
            }

            string[] headers = headText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (headers == null || headers.Length == 0)
            {
                throw new ArgumentException("'WithHeadersText' param format error.");
            }

            HttpHeaders = new HttpHeader();

            foreach (string head in headers)
            {
                if (head.StartsWith("HTTP", StringComparison.OrdinalIgnoreCase))
                {
                    string[] ts = head.Split(' ');
                    if (ts.Length > 1)
                    {
                        HttpHeaders.ResponseStatusCode = ts[1];
                    }
                }
                else if (head.StartsWith("Set-Cookie:", StringComparison.OrdinalIgnoreCase))
                {
                    Cookies = Cookies ?? new List<string>();
                    string tCookie = head.Substring(11, head.IndexOf(";") < 0 ? head.Length - 11 : head.IndexOf(";") - 10).Trim();

                    if (!Cookies.Exists(f => f.Split('=')[0] == tCookie.Split('=')[0]))
                    {
                        Cookies.Add(tCookie);
                    }
                }
                else if (head.StartsWith("Location:", StringComparison.OrdinalIgnoreCase))
                {
                    HttpHeaders.Location = head.Substring(9).Trim();
                }
                else if (head.StartsWith("Content-Encoding:", StringComparison.OrdinalIgnoreCase))
                {
                    if (head.IndexOf("gzip", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        HttpHeaders.IsGzip = true;
                    }
                }
                else if (head.StartsWith("Content-Type:", StringComparison.OrdinalIgnoreCase))
                {
                    string[] types = head.Substring(13).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string t in types)
                    {
                        if (t.IndexOf("charset=", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            HttpHeaders.Charset = t.Trim().Substring(8);
                        }
                        else if (t.IndexOf('/') >= 0)
                        {
                            HttpHeaders.ContentType = t.Trim();
                        }
                    }
                }
                else if (head.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase))
                {
                    HttpHeaders.ContentLength = long.Parse(head.Substring(15).Trim());
                }
                else if (head.StartsWith("Transfer-Encoding:", StringComparison.OrdinalIgnoreCase) && head.EndsWith("chunked", StringComparison.OrdinalIgnoreCase))
                {
                    HttpHeaders.IsChunk = true;
                }
            }
        }

        /// <summary>
        /// 取得未分块数据的内容
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="reader">数据源实例</param>
        /// <param name="data">已读取未处理的字节数据</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="readLength">读取的长度</param>
        /// <param name="body">保存块数据的流</param>
        private static void GetBodyData<T>(T reader, ref byte[] data, int startIndex, int readLength, ref MemoryStream body)
        {
            int contentTotal = 0;

            if (startIndex < data.Length)
            {
                int count = readLength - startIndex;
                body.Write(data, startIndex, count);
                contentTotal += count;
            }

            int tlength = 0;

            do
            {
                tlength = ReadData(reader, ref data);
                contentTotal += tlength;
                body.Write(data, 0, tlength);

                if (HttpHeaders.ContentLength > 0 && contentTotal >= HttpHeaders.ContentLength)
                {
                    break;
                }
            }
            while (tlength > 0);
        }

        /// <summary>
        /// 取得分块数据
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="reader">Socket实例</param>
        /// <param name="data">已读取未处理的字节数据</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="readLength">读取的长度</param>
        /// <param name="body">保存块数据的流</param>
        private static void GetChunkData<T>(T reader, ref byte[] data, ref int startIndex, ref int readLength, ref MemoryStream body)
        {
            int chunkSize = -1;//每个数据块的长度,用于分块数据.当长度为0时,说明读到数据末尾.

            while (true)
            {
                chunkSize = GetChunkHead(reader, ref data, ref startIndex, ref readLength);
                GetChunkBody(reader, ref data, ref startIndex, ref readLength, ref body, chunkSize);

                if (chunkSize <= 0)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 取得分块数据的数据长度
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="reader">Socket实例</param>
        /// <param name="data">已读取未处理的字节数据</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="readLength">读取的长度</param>
        /// <returns>块长度,返回0表示已到末尾.</returns>
        private static int GetChunkHead<T>(T reader, ref byte[] data, ref int startIndex, ref int readLength)
        {
            int chunkSize = -1;
            List<char> tChars = new List<char>();//用于临时存储块长度字符

            if (startIndex >= data.Length || startIndex >= readLength)
            {
                readLength = ReadData(reader, ref data);
                startIndex = 0;
            }

            do
            {
                for (int i = startIndex; i < readLength; i++)
                {
                    char c = (char)data[i];

                    if (c == '\n')
                    {
                        try
                        {
                            chunkSize = Convert.ToInt32(new string(tChars.ToArray()).TrimEnd('\r'), 16);
                            startIndex = i + 1;
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Maybe exists 'chunk-ext' field.", e);
                        }

                        break;
                    }

                    tChars.Add(c);
                }

                if (chunkSize >= 0)
                {
                    break;
                }

                startIndex = 0;
                readLength = ReadData(reader, ref data);
            }
            while (readLength > 0);

            return chunkSize;
        }

        /// <summary>
        /// 取得分块传回的数据内容
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="reader">Socket实例</param>
        /// <param name="data">已读取未处理的字节数据</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="readLength">读取的长度</param>
        /// <param name="body">保存块数据的流</param>
        /// <param name="chunkSize">块长度</param>
        private static void GetChunkBody<T>(T reader, ref byte[] data, ref int startIndex, ref int readLength, ref MemoryStream body, int chunkSize)
        {
            if (chunkSize <= 0)
            {
                return;
            }

            int chunkReadLength = 0;//每个数据块已读取长度

            if (startIndex >= data.Length || startIndex >= readLength)
            {
                readLength = ReadData(reader, ref data);
                startIndex = 0;
            }

            do
            {
                int owing = chunkSize - chunkReadLength;
                int count = Math.Min(readLength - startIndex, owing);

                body.Write(data, startIndex, count);
                chunkReadLength += count;

                if (owing <= count)
                {
                    startIndex += count + 2;
                    break;
                }

                startIndex = 0;
                readLength = ReadData(reader, ref data);
            }
            while (readLength > 0);
        }

        /// <summary>
        /// 从数据源读取数据
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <param name="reader">数据源</param>
        /// <param name="data">用于存储读取的数据</param>
        /// <returns>读取的数据长度,无数据为-1</returns>
        private static int ReadData<T>(T reader, ref byte[] data)
        {
            int result = -1;

            if (reader is Socket)
            {
                result = (reader as Socket).Receive(data, SocketFlags.None);
            }
            else if (reader is SslStream)
            {
                result = (reader as SslStream).Read(data, 0, data.Length);
            }

            return result;
        }

    }

    public class HttpHeader
    {
        /// <summary>
        /// 获取请求回应状态码
        /// </summary>
        public string ResponseStatusCode
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取跳转url
        /// </summary>
        public string Location
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取是否由Gzip压缩
        /// </summary>
        public bool IsGzip
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取返回的文档类型
        /// </summary>
        public string ContentType
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取内容使用的字符集
        /// </summary>
        public string Charset
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取内容长度
        /// </summary>
        public long ContentLength
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取是否分块传输
        /// </summary>
        public bool IsChunk
        {
            get;
            internal set;
        }
    }
}
