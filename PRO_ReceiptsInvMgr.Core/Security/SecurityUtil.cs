using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Text.RegularExpressions;

namespace PRO_ReceiptsInvMgr.Core.Security
{
    public static class SecurityUtil
    {
        /// <summary>
        /// 将普通字符串转换为Base64字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StringToBase64(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            byte[] b = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// 将Base64字符串转换为普通字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Base64ToString(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            byte[] b = Convert.FromBase64String(source);
            return Encoding.UTF8.GetString(b);
        }


        #region Base64加密
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        /// <returns></returns>
        public static string EncodeBase64(string text)
        {
            //如果字符串为空，则返回
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            try
            {
                char[] Base64Code = new char[]{'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T',
                                            'U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n',
                                            'o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
                                            '8','9','+','/','='};
                byte empty = (byte)0;
                ArrayList byteMessage = new ArrayList(Encoding.Default.GetBytes(text));
                StringBuilder outmessage;
                int messageLen = byteMessage.Count;
                int page = messageLen / 3;
                int use = 0;
                use = messageLen % 3;
                if (use > 0)
                {
                    for (int i = 0; i < 3 - use; i++)
                    {
                        byteMessage.Add(empty);
                    }
                    page++;
                }
                outmessage = new System.Text.StringBuilder(page * 4);
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[3];
                    instr[0] = (byte)byteMessage[i * 3];
                    instr[1] = (byte)byteMessage[i * 3 + 1];
                    instr[2] = (byte)byteMessage[i * 3 + 2];
                    int[] outstr = new int[4];
                    outstr[0] = instr[0] >> 2;
                    outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                    if (!instr[1].Equals(empty))
                    {
                        outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                    }
                    else
                    {
                        outstr[2] = 64;
                    }
                    if (!instr[2].Equals(empty))
                    {
                        outstr[3] = (instr[2] & 0x3f);
                    }
                    else
                    {
                        outstr[3] = 64;
                    }
                    outmessage.Append(Base64Code[outstr[0]]);
                    outmessage.Append(Base64Code[outstr[1]]);
                    outmessage.Append(Base64Code[outstr[2]]);
                    outmessage.Append(Base64Code[outstr[3]]);
                }
                return outmessage.ToString();
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(SecurityUtil), ex);
                throw;
            }
        }
        #endregion

        #region Base64解密
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="text">要解密的字符串</param>
        public static string DecodeBase64(string inputText)
        {
            var text = inputText;
            //如果字符串为空，则返回
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            //将空格替换为加号
            text = text.Replace(" ", "+");

            try
            {
                if ((text.Length % 4) != 0)
                {
                    return "包含不正确的BASE64编码";
                }
                if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
                {
                    return "包含不正确的BASE64编码";
                }
                string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                int page = text.Length / 4;
                ArrayList outMessage = new ArrayList(page * 3);
                char[] message = text.ToCharArray();
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[4];
                    instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                    instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                    instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                    instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                    byte[] outstr = new byte[3];
                    outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                    if (instr[2] != 64)
                    {
                        outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    if (instr[3] != 64)
                    {
                        outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    outMessage.Add(outstr[0]);
                    if (outstr[1] != 0)
                    {
                        outMessage.Add(outstr[1]);
                    }
                    if (outstr[2] != 0)
                    {
                        outMessage.Add(outstr[2]);
                    }
                }
                byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
                return Encoding.UTF8.GetString(outbyte);
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(SecurityUtil), ex);
                throw;
            }
        }
        #endregion
    }
}
