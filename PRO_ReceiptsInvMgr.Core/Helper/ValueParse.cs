using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using PRO_ReceiptsInvMgr.Core.Security;
using System.Runtime.Serialization.Json;

namespace PRO_ReceiptsInvMgr.Core.Helper
{
    /// <summary>
    /// Class ValueParse
    /// </summary>
    public static class ValueParse
    {
        /// <summary>
        /// Gets the int16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Int16.</returns>
        public static Int16 GetInt16(string value)
        {
            Int16 result;
            Int16.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// Gets the boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static bool GetBoolean(string value)
        {
            bool result;
            bool.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// Gets the boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static bool? GetBoolean(string value, bool? defaultValue)
        {
            bool result;
            if (bool.TryParse(value, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets the IN T16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Nullable{Int16}.</returns>
        public static Int16? GetInt16NullAble(string value)
        {
            Int16 result;
            if (Int16.TryParse(value, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        public static int GetInt(string value)
        {
            int result;
            int.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// Gets the INT.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Nullable{System.Int32}.</returns>
        public static int? GetIntNullAble(string value)
        {
            int result;
            if (int.TryParse(value, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Int64.</returns>
        public static Int64 GetInt64(string value)
        {
            Int64 result;
            Int64.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// Gets the IN T64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Nullable{Int64}.</returns>
        public static Int64? GetInt64NullAble(string value)
        {
            Int64 result;
            if (Int64.TryParse(value, out result))
            {
                return result;
            }
            return null;
        }
        /// <summary>
        /// Gets the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Double.</returns>
        public static double GetDouble(string value)
        {
            double result;
            double.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// Gets the DOUBLE.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Nullable{System.Double}.</returns>
        public static double? GetDoubleNullAble(string value)
        {
            double result;
            if (double.TryParse(value, out result))
            {
                return result;
            }
            return null;
        }
        /// <summary>
        /// Getdecimals the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal GetDecimal(string value)
        {
            decimal result;
            decimal.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// Gets the DECIMAL.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Nullable{System.Decimal}.</returns>
        public static decimal? GetDecimalNullAble(string value)
        {
            decimal result;
            if (decimal.TryParse(value, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Getfloats the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Single.</returns>
        public static float Getfloat(string value)
        {
            float result;
            float.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// Gets the FLOAT.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Nullable{System.Single}.</returns>
        public static float? GetfloatNullAble(string value)
        {
            float result;
            if (float.TryParse(value, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetDateTime(string value)
        {
            DateTime result;
            DateTime.TryParse(value, out result);
            return result;
        }
        /// <summary>
        /// Gets the DATEIME.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Nullable{DateTime}.</returns>
        public static DateTime? GetDateTimeNullAble(string value)
        {
            DateTime result;
            if (DateTime.TryParse(value, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string GetString(int? value)
        {
            return value.HasValue ? value.Value.ToString() : string.Empty;
        }
        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string GetString(Int16? value)
        {
            return value.HasValue ? value.Value.ToString() : string.Empty;
        }
        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string GetString(Int64? value)
        {
            return value.HasValue ? value.Value.ToString() : string.Empty;
        }
        /// <summary>
        /// Gets the bytes form strem.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>System.Byte[][].</returns>
        public static byte[] GetBytesFormStrem(Stream stream)
        {
            byte[] output = null;
            if (stream != null)
            {
                output = new byte[stream.Length];
                stream.Read(output, 0, (int)stream.Length);
            }
            return output;
        }

        /// <summary>
        /// Get the int by string(split with comma)
        /// </summary>
        /// <param name="value">split with comma eg:1,3,5</param>
        /// <returns></returns>
        public static List<int> GetIntList(this string value)
        {
            List<int> output = new List<int>();
            var temp = value.Split(',');
            foreach (var item in temp)
            {
                output.Add(GetInt(item));
            }
            return output;
        }

        /// <summary>
        /// Get the string by int
        /// </summary>
        /// <param name="value">split with comma eg:1,3,5</param>
        /// <returns></returns>
        public static string GetCNInt(this int value)
        {
            string[] cstr = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            string[] wstr = { "", "", "十", "百", "千", "万", "十", "百", "千", "亿", "十", "百", "千" };
            var str = value.ToString();
            int len = str.Length;
            int i;
            string tmpstr, rstr;
            rstr = "";
            for (i = 1; i <= len; i++)
            {
                tmpstr = str.Substring(len - i, 1);
                rstr = string.Concat(cstr[Int32.Parse(tmpstr)] + wstr[i], rstr);
            }
            rstr = rstr.Replace("十零", "十");
            rstr = rstr.Replace("零十", "零");
            rstr = rstr.Replace("零百", "零");
            rstr = rstr.Replace("零千", "零");
            rstr = rstr.Replace("零万", "万");
            for (i = 1; i <= 6; i++)
            {
                rstr = rstr.Replace("零零", "零");
            }
            rstr = rstr.Replace("零万", "零");
            rstr = rstr.Replace("零亿", "亿");
            rstr = rstr.Replace("零零", "零");

            return rstr;
        }
        /// <summary>
        /// Get the string by int
        /// </summary>
        /// <param name="value">split with comma eg:1,3,5</param>
        /// <returns></returns>
        public static int GetIntFromCN(this string value)
        {
            string outputStr = string.Empty;
            foreach (var item in value)
            {
                switch (item)
                {
                    case '零':
                        {
                            outputStr += "0";
                            break;
                        }
                    case '一':
                        {
                            outputStr += "1";
                            break;
                        }
                    case '二':
                        {
                            outputStr += "2";
                            break;
                        }
                    case '三':
                        {
                            outputStr += "3";
                            break;
                        }
                    case '四':
                        {
                            outputStr += "4";
                            break;
                        }
                    case '五':
                        {
                            outputStr += "5";
                            break;
                        }
                    case '六':
                        {
                            outputStr += "6";
                            break;
                        }
                    case '七':
                        {
                            outputStr += "7";
                            break;
                        }
                    case '八':
                        {
                            outputStr += "8";
                            break;
                        }
                    case '九':
                        {
                            outputStr += "9";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            return GetInt(outputStr);
        }
        /// <summary>
        /// Gets the DATEIME.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Nullable{DateTime}.</returns>
        public static DateTime? GetDateTimeFromOADateNullAble(string value)
        {
            DateTime? result = null;
            int v;
            if (int.TryParse(value, out v))
            {
                result = DateTime.FromOADate(v);
            }
            return result;
        }


        /// <summary>
        /// Encryptors the AES to string by decimal.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <returns>System.String.</returns>
        public static string EncryptorAesToString(this Nullable<decimal> inputvalue)
        {
            return Cryptographer.EncryptorAesToString(inputvalue);
        }
        /// <summary>
        /// Decryptors the AES from string to Decimal.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <returns>System.String.</returns>
        public static decimal DecryptorAesToDecimal(this string inputvalue)
        {
            return Cryptographer.DecryptorAesToDecimal(inputvalue);
        }

        /// <summary>
        /// 获取Json的Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="szJson"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(this string szJson)
        {
            T obj = Activator.CreateInstance<T>();  //注意 要有T类型要有无参构造函数
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }

        public static T GetValue<T>(this Nullable<T> inputvalue) where T : struct
        {
            return inputvalue.HasValue ? inputvalue.Value : new T();
        }


        /// <summary>
        /// Decryptors the AES from string to double.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <returns>System.String.</returns>
        public static double DecryptorAesToDouble(this string inputvalue)
        {
            return ValueParse.GetDouble(Cryptographer.DecryptorAesToDecimal(inputvalue).ToString());
        }

        /// <summary>
        /// Encryptors the AES to string by decimal.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <returns>System.String.</returns>
        public static string EncryptorAesToString(this decimal inputvalue)
        {
            return Cryptographer.EncryptorAesToString(inputvalue);
        }

        /// <summary>
        /// 数字数组连续时以特定分隔符省略
        /// </summary>
        /// <param name="inputvalue"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static string ToSimplifyStr(this IEnumerable<object> inputvalue, string split = "-")
        {
            StringBuilder res = new StringBuilder();
            var source = inputvalue.OrderBy(it => it).ToList();
            var strType = typeof(string);
            foreach (var item in source)
            {
                int itemValue = 0;
                var type = item.GetType();
                if (type.IsValueType)
                {
                    itemValue = ValueParse.GetInt(item.ToString());
                }
                else if (type == strType)
                {
                    itemValue = ValueParse.GetInt(item.ToString());
                    if (itemValue == 0)
                    {
                        res.AppendFormat(",{0}", item.ToString());
                        continue;
                    }
                }
                else
                {
                    res.AppendFormat(",{0}", itemValue);
                    continue;
                }
                res.AppendFormat("{0}", itemValue);
            }

            return res.ToString().TrimStart(',');
        }
    }
}
