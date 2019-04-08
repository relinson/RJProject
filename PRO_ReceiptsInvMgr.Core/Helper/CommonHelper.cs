using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Xml;
using PRO_ReceiptsInvMgr.Core.Security;
using PRO_ReceiptsInvMgr.WebService;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Windows.Media.Imaging;
using System.Reflection;

namespace PRO_ReceiptsInvMgr.Core.Helper
{
    public class CommonHelper
    {
        protected CommonHelper()
        { }

        static CommonHelper()
        {
        }

        /// <summary>
        /// 将Content进行3DES加密
        /// </summary>
        /// <param name="password"></param>
        /// <param name="contentXml"></param>
        /// <returns>3DES加密后的Content</returns>
        public static string Get3DESEncodeContent(string password, string contentXml)
        {
            try
            {
                byte[] key = Encoding.UTF8.GetBytes((password.Length > 10) ? password.Substring(10) : password);//updated by Peter.wu 2014-11-20

                byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };      //当模式为ECB时，IV无用
                byte[] data = Encoding.UTF8.GetBytes(contentXml);
                byte[] desData = EncryptionLib.Des3EncodeECB(key, iv, data);
                return Convert.ToBase64String(desData);
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(CommonHelper), "3DES加密异常:" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// 将Content进行3DES解密
        /// </summary>
        /// <param name="password"></param>
        /// <param name="contentXml"></param>
        /// <returns>3DES解密后的Content</returns>
        public static string Get3DESDecodeContent(string password, string contentXml)
        {
            try
            {
                byte[] key = Encoding.UTF8.GetBytes((password.Length > 10) ? password.Substring(10) : password);//updated by Peter.wu 2014-11-20
                byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };      //当模式为ECB时，IV无用
                byte[] data = Convert.FromBase64String(contentXml);

                byte[] desData = EncryptionLib.Des3DecodeECB(key, iv, data);
                return Encoding.UTF8.GetString(desData).TrimEnd('\0');
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(CommonHelper), "3DES解密密异常:" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// 将Content进行Base64解密
        /// </summary>
        /// <param name="contentXml">The content XML.</param>
        /// <returns>System.String.</returns>
        public static byte[] GetBase64Content(string contentXml)
        {
            try
            {
                byte[] data = Convert.FromBase64String(contentXml);
                return data;
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(CommonHelper), "Base64解密密异常:" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string source)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            UTF8Encoding encoding = new UTF8Encoding();
            var hashedBytes = md5Hasher.ComputeHash(encoding.GetBytes(source));
            md5Hasher.Dispose();
            return Convert.ToBase64String(hashedBytes);
        }

        /// <summary>
        /// 发起一个HTTP请求（以POST方式）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string param = "", int timeout = 10000)
        {
            param = "param=" + param;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = timeout;
            request.KeepAlive = false;
            request.AllowAutoRedirect = true;
            request.Proxy = null;

            WebResponse response = null;
            string responseStr = null;
            Stream reqStream = null;
            try
            {
                reqStream = request.GetRequestStream();
                using (StreamWriter sw = new StreamWriter(reqStream, Encoding.ASCII))
                {
                    sw.Write(param);
                    sw.Close();
                }
                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(CommonHelper), ex);
                throw;
            }
            finally
            {
                if (reqStream != null)
                {
                    reqStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                request.Abort();
            }
            return responseStr;
        }



        /// <summary>
        /// 人民币阿拉伯数字转大写
        /// </summary>
        /// <param name="strAmount"></param>
        /// <returns></returns>
        public static string MoneyToUpper(string inputAmount)
        {
            string strAmount = inputAmount;

            bool IsNegative = false; // 是否是负数
            if (strAmount.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                strAmount = strAmount.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strUpart = null;
            // 保留两位小数 123.489→123.49　　123.4→123.4
            strAmount = Math.Round(double.Parse(strAmount), 2).ToString();
            if (strAmount.IndexOf(".") > -1)
            {
                if (strAmount.IndexOf(".") == strAmount.Length - 2)
                {
                    strAmount = strAmount + "0";
                }
            }
            else
            {
                strAmount = strAmount + ".00";
            }
            string strLower = strAmount;
            int iTemp = 1;
            string strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                    default:
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "万";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "亿";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "万";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }

                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理
            if (strUpper.Substring(0, 1) == "圆")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零圆整";
            }
            string functionReturnValue = strUpper;

            if (IsNegative)
            {
                return "负" + functionReturnValue;
            }
            else
            {
                return functionReturnValue;
            }
        }

        /// <summary> 
        /// RSA 加密
        /// </summary> 
        /// <param name="sSource" >明文</param> 
        /// <param name="sPublicKey" >公钥</param> 
        public static byte[] EncryptString(string sSource, string sPublicKey)
        {
            var sPublicKeys = @"<RSAKeyValue><Modulus>iDGr0/JUEob5sbr424Oz5Ozq5Lzkf96s8RTsEqzoMUK9TkYSraVBFuj2PJwmenaVhCvdQodoeGYqMLQjrnZBzW726pU+18Ol8KGhack8CsiQtNy7U9q3dheo/AzQmEWrlWHKLiXIxjY2X5HaxCfyg90inz/rk6tt1fBpuumrqs8=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            RSACryptoServiceProvider.UseMachineKeyStore = true;
            string plaintext = sSource;
            rsa.FromXmlString(sPublicKeys);
            int keySize = rsa.KeySize / 8;
            int bufferSize = keySize - 11;
            byte[] buffer = new byte[bufferSize];
            MemoryStream msInput = new
                MemoryStream(Encoding.UTF8.GetBytes(plaintext));
            MemoryStream msOutput = new MemoryStream();
            int readLen = msInput.Read(buffer, 0, bufferSize);
            while (readLen > 0)
            {
                byte[] dataToEnc = new byte[readLen];
                Array.Copy(buffer, 0, dataToEnc, 0, readLen);
                byte[] encData = rsa.Encrypt(dataToEnc, false);
                msOutput.Write(encData, 0, encData.Length);
                readLen = msInput.Read(buffer, 0, bufferSize);
            }
            msInput.Close();

            byte[] result = msOutput.ToArray();
            rsa.Clear();
            return result;
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public static byte[] ReadFile(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] file = new byte[fileStream.Length];
            fileStream.Read(file, 0, file.Length);
            fileStream.Dispose();
            fileStream.Close();
            return file;
        }

        /// <summary>
        /// 调WebServe接口
        /// </summary>
        /// <param name="WebServiceInfo"></param>
        /// <param name="MethodName">方法名</param>
        /// <param name="paramName">参数名</param>
        /// <param name="paramValue">参数值</param>
        /// <returns></returns>
        public static string QuerySoapWebService(string url, string MethodName, string paramName, string paramValue)
        {
            return WebServiceCaller.QuerySoapWebService(url,
                0, null, 0, false, null, null,
                MethodName, paramName, paramValue);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="WebServiceInfo"></param>
        /// <param name="url"></param>
        /// <param name="file"></param>
        public static void DownLoadFile(string serviceUrl, string url, string file)
        {
            WebServiceCaller.DownLoadFile(serviceUrl,
                0, null, 0, false, null, null, url, file);
        }

        /// <summary>
        /// XML转字符串
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static string XMLToString(XmlDocument xmlDoc)
        {
            string xmlString = string.Empty;

            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, null);

            xmlDoc.Save(xmlTextWriter);
            StreamReader streamReader = new StreamReader(memoryStream, System.Text.Encoding.UTF8);
            memoryStream.Position = 0;
            xmlString = streamReader.ReadToEnd();
            streamReader.Close();
            memoryStream.Close();
            streamReader.Dispose();
            memoryStream.Dispose();
            return xmlString;
        }

        /// <summary>
        /// 生成Password:10位随机数+Base64({（10位随机数+注册码）MD5})
        /// </summary>
        /// <returns></returns>
        public static string GetPassWord(string strZCM)
        {
            var randomNumber = GetRandomString(10);
            return randomNumber + Convert.ToBase64String(EncryptionLib.MD5EncryptBytes(randomNumber + strZCM));
        }
        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <param name="RandomLength"></param>
        /// <returns></returns>
        public static string GetRandomString(int RandomLength)
        {
            string returnValue = string.Empty;

            //0123456789ABCDEFGHIJKMLNOPQRSTUVWXYZ
            string RandomString = "0123456789";
            Random myRandom = new Random();

            for (int i = 0; i < RandomLength; i++)
            {
                int r = myRandom.Next(0, RandomString.Length - 1);
                returnValue += RandomString[r];
            }
            return returnValue;
        }

        /// <summary>
        /// 获取文件MD5码
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                return Convert.ToBase64String(retVal);
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(CommonHelper), ex);
                throw;
            }
        }

        /// <summary>
        /// 按指定(字节)长度截取字符串
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>string</returns>
        public static string CutStringByte(string str, int byteLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (System.Text.Encoding.Default.GetByteCount(str) < byteLength)
            {
                return str;
            }
            int i = 0;//字节数
            int j = 0;//实际截取长度
            foreach (char newChar in str)
            {
                if ((int)newChar > 127)
                {
                    //汉字
                    i += 2;
                }
                else
                {
                    i++;
                }

                if (i < byteLength)
                {
                    j++;
                }
                else
                {
                    break;
                }
            }
            str = str.Substring(0, j) + "...";
            return str;
        }


        /// <summary>
        /// 传入类型B的对象b，将b与a相同名称的值进行赋值给创建的a中
        /// </summary>
        /// <typeparam name="A">类型A</typeparam>
        /// <typeparam name="B">类型B</typeparam>
        /// <param name="b">类型为B的参数b</param>
        /// <returns>拷贝b中相同属性的值的a</returns>
        public static A Mapper<A, B>(B b)
        {
            A a = Activator.CreateInstance<A>();
            try
            {
                Type Typeb = b.GetType();//获得类型  
                Type Typea = typeof(A);
                foreach (PropertyInfo sp in Typeb.GetProperties())//获得类型的属性字段  
                {
                    foreach (PropertyInfo ap in Typea.GetProperties())
                    {
                        if (ap.Name == sp.Name)//判断属性名是否相同  
                        {
                            ap.SetValue(a, sp.GetValue(b, null), null);//获得b对象属性的值复制给a对象的属性  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(CommonHelper), ex);
                throw;
            }
            return a;
        }

    }
}
