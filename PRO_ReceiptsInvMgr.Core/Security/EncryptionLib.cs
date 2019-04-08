using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace PRO_ReceiptsInvMgr.Core.Security
{
    public class EncryptionLib
    {
        protected EncryptionLib() { }

        //密钥
        private static byte[] arrDESKey = new byte[] { 42, 16, 93, 156, 78, 4, 218, 32 };
        private static byte[] arrDESIV = new byte[] { 55, 103, 246, 79, 36, 99, 167, 3 };

        /// <summary>
        /// 加密。
        /// </summary>
        /// <param name="m_Need_Encode_String"></param>
        /// <returns></returns>
        public static string Encode(string m_Need_Encode_String)
        {
            if (m_Need_Encode_String == null)
            {
                throw new ArgumentNullException(nameof(m_Need_Encode_String));
            }
            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
            MemoryStream objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(arrDESKey, arrDESIV), CryptoStreamMode.Write);
            StreamWriter objStreamWriter = new StreamWriter(objCryptoStream);
            objStreamWriter.Write(m_Need_Encode_String);
            objStreamWriter.Flush();
            objCryptoStream.FlushFinalBlock();
            objMemoryStream.Flush();
            objDES.Dispose();
            objMemoryStream.Dispose();
            objCryptoStream.Dispose();
            objStreamWriter.Dispose();
            return Convert.ToBase64String(objMemoryStream.GetBuffer(), 0, (int)objMemoryStream.Length);
        }

        /// <summary>
        /// 解密。
        /// </summary>
        /// <param name="m_Need_Encode_String"></param>
        /// <returns></returns>
        public static string Decode(string m_Need_Encode_String)
        {
            if (m_Need_Encode_String == null)
            {
                throw new ArgumentNullException(nameof(m_Need_Encode_String));
            }
            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
            byte[] arrInput = Convert.FromBase64String(m_Need_Encode_String);
            MemoryStream objMemoryStream = new MemoryStream(arrInput);
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateDecryptor(arrDESKey, arrDESIV), CryptoStreamMode.Read);
            StreamReader objStreamReader = new StreamReader(objCryptoStream);
            objDES.Dispose();
            objMemoryStream.Dispose();
            objCryptoStream.Dispose();
            objStreamReader.Dispose();
            return objStreamReader.ReadToEnd();
        }

        /// <summary>
        /// md5
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        public static string Md5(string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();
            byte[] inputBye;
            byte[] outputBye;
            inputBye = System.Text.Encoding.ASCII.GetBytes(encypStr);
            outputBye = m5.ComputeHash(inputBye);
            retStr = Convert.ToBase64String(outputBye);
            m5.Dispose();
            return (retStr);
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
        /// MD5解密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] MD5EncryptBytes(string source)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            UTF8Encoding encoding = new UTF8Encoding();
            var hashedBytes = md5Hasher.ComputeHash(encoding.GetBytes(source));
            md5Hasher.Dispose();
            return hashedBytes;
        }

        #region MD5加密
        /// <summary> 
        /// MD5加密 
        /// </summary> 
        /// <param name="input">需要加密的字符串</param> 
        /// <returns></returns> 
        public static string MD5Encrypt2(string input)
        {
            return MD5Encrypt(input, new UTF8Encoding());
        }

        /// <summary> 
        /// MD5加密 
        /// </summary> 
        /// <param name="input">需要加密的字符串</param> 
        /// <param name="encode">字符的编码</param> 
        /// <returns></returns> 
        public static string MD5Encrypt(string input, Encoding encode)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(encode.GetBytes(input));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            md5.Dispose();
            return sb.ToString();
        }

        /// <summary> 
        /// MD5对文件流加密 
        /// </summary> 
        /// <param name="sr"></param> 
        /// <returns></returns> 
        public static string MD5Encrypt(Stream stream)
        {
            MD5 md5serv = MD5CryptoServiceProvider.Create();
            byte[] buffer = md5serv.ComputeHash(stream);
            StringBuilder sb = new StringBuilder();
            foreach (byte var in buffer)
            {
                sb.Append(var.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary> 
        /// MD5加密(返回16位加密串) 
        /// </summary> 
        /// <param name="input"></param> 
        /// <param name="encode"></param> 
        /// <returns></returns> 
        public static string MD5Encrypt16(string input, Encoding encode)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string result = BitConverter.ToString(md5.ComputeHash(encode.GetBytes(input)), 4, 8);
            result = result.Replace("-", "");
            md5.Dispose();
            return result;
        }
        #endregion 

        #region DES加密解密
        /// <summary> 
        /// DES加密 
        /// </summary> 
        /// <param name="data">加密数据</param> 
        /// <param name="key">8位字符的密钥字符串</param> 
        /// <param name="iv">8位字符的初始化向量字符串</param> 
        /// <returns></returns> 
        public static string DESEncrypt(string data, string key, string iv)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(iv);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            //int i = cryptoProvider.KeySize
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            cryptoProvider.Dispose();
            ms.Dispose();
            cst.Dispose();
            sw.Dispose();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        /// <summary> 
        /// DES解密 
        /// </summary> 
        /// <param name="data">解密数据</param> 
        /// <param name="key">8位字符的密钥字符串(需要和加密时相同)</param> 
        /// <param name="iv">8位字符的初始化向量字符串(需要和加密时相同)</param> 
        /// <returns></returns> 
        public static string DESDecrypt(string data, string key, string iv)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(iv);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            cryptoProvider.Dispose();
            ms.Dispose();
            cst.Dispose();
            sr.Dispose();
            return sr.ReadToEnd();
        }
        #endregion

        #region 3DES 加密解密

        #region CBC模式
        /// <summary>
        /// DES3 CBC模式加密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV</param>
        /// <param name="data">明文的byte数组</param>
        /// <returns>密文的byte数组</returns>
        public static byte[] Des3EncodeCBC(byte[] key, byte[] iv, byte[] data)
        {
            //复制于MSDN
            try
            {
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;             //默认值
                tdsp.Padding = PaddingMode.PKCS7;       //默认值
                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);
                // Write the byte array to the crypto stream and flush it.
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                // Get an array of bytes from the 
                // MemoryStream that holds the 
                // encrypted data.
                byte[] ret = mStream.ToArray();
                // Close the streams.
                cStream.Close();
                mStream.Close();
                mStream.Dispose();
                tdsp.Dispose();
                cStream.Dispose();
                // Return the encrypted buffer.
                return ret;
            }
            catch (CryptographicException ex)
            {
                Logging.Log4NetHelper.Error(typeof(EncryptionLib), ex);
                throw;
            }
        }
        /// <summary>
        /// DES3 CBC模式解密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV</param>
        /// <param name="data">密文的byte数组</param>
        /// <returns>明文的byte数组</returns>
        public static byte[] Des3DecodeCBC(byte[] key, byte[] iv, byte[] data)
        {
            try
            {
                // Create a new MemoryStream using the passed 
                // array of encrypted data.
                MemoryStream msDecrypt = new MemoryStream(data);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read);
                // Create buffer to hold the decrypted data.
                byte[] fromEncrypt = new byte[data.Length];
                // Read the decrypted data out of the crypto stream
                // and place it into the temporary buffer.
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                //Convert the buffer into a string and return it.
                msDecrypt.Dispose();
                tdsp.Dispose();
                csDecrypt.Dispose();
                return fromEncrypt;
            }
            catch (CryptographicException ex)
            {
                Logging.Log4NetHelper.Error(typeof(EncryptionLib), ex);
                throw;
            }
        }
        #endregion

        #region ECB模式
        /// <summary>
        /// DES3 ECB模式加密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV(当模式为ECB时，IV无用)</param>
        /// <param name="str">明文的byte数组</param>
        /// <returns>密文的byte数组</returns>
        public static byte[] Des3EncodeECB(byte[] key, byte[] iv, byte[] data)
        {
            try
            {
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.ECB;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);
                // Write the byte array to the crypto stream and flush it.
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                // Get an array of bytes from the 
                // MemoryStream that holds the 
                // encrypted data.
                byte[] ret = mStream.ToArray();
                // Close the streams.
                cStream.Close();
                mStream.Close();
                mStream.Dispose();
                tdsp.Dispose();
                cStream.Dispose();
                // Return the encrypted buffer.
                return ret;
            }
            catch (CryptographicException ex)
            {
                Logging.Log4NetHelper.Error(typeof(EncryptionLib), ex);
                throw;
            }
        }
        /// <summary>
        /// DES3 ECB模式解密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="iv">IV(当模式为ECB时，IV无用)</param>
        /// <param name="str">密文的byte数组</param>
        /// <returns>明文的byte数组</returns>
        public static byte[] Des3DecodeECB(byte[] key, byte[] iv, byte[] data)
        {
            try
            {
                // Create a new MemoryStream using the passed 
                // array of encrypted data.
                MemoryStream msDecrypt = new MemoryStream(data);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.ECB;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).

                if (data.Length % 8 != 0)
                {
                    throw new ArgumentException("data");
                }

                byte[] fromEncrypt = new byte[data.Length];
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                     tdsp.CreateDecryptor(key, iv),
                     CryptoStreamMode.Read))
                {
                    // Create buffer to hold the decrypted data.
                    // Read the decrypted data out of the crypto stream
                    // and place it into the temporary buffer.
                    csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                    //Convert the buffer into a string and return it.
                }
                msDecrypt.Dispose();
                tdsp.Dispose();
                return fromEncrypt;
            }
            catch (CryptographicException ex)
            {
                Logging.Log4NetHelper.Error(typeof(EncryptionLib), ex);
                throw;
            }
        }
        #endregion


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DES3Encrypt(string data, string key)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

            DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
            DES.Mode = CipherMode.CBC;
            DES.Padding = PaddingMode.PKCS7;

            ICryptoTransform DESEncrypt = DES.CreateEncryptor();

            byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(data);
            DES.Dispose();
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DES3Decrypt(string data, string key)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

            DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
            DES.Mode = CipherMode.CBC;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            ICryptoTransform DESDecrypt = DES.CreateDecryptor();

            string result = "";
            try
            {
                byte[] Buffer = Convert.FromBase64String(data);
                result = ASCIIEncoding.ASCII.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(EncryptionLib), ex);
                throw;
            }
            DES.Dispose();
            return result;
        }
        #endregion



    }
}
