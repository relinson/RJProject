using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using PRO_ReceiptsInvMgr.Core.Helper;

namespace PRO_ReceiptsInvMgr.Core.Security
{
    public  static class Cryptographer
    {
        /// <summary>
        /// The SH a256
        /// </summary>
        private static readonly SHA256Managed SHA256 = new SHA256Managed();
        /// <summary>
        /// The AES
        /// </summary>
        private static readonly RijndaelManaged AES = new RijndaelManaged();
        /// <summary>
        /// The CODETYPE
        /// </summary>
        private static Encoding CODETYPE = new UTF8Encoding();
        /// <summary>
        /// The KEYARRY
        /// </summary>
        private static readonly CryptogramConfig KEYARRY = CryptogramConfig.GetCryptogramNode();

        /// <summary>
        /// Initializes static members of the <see cref="Cryptographer"/> class.
        /// </summary>
        static Cryptographer()
        {
            AES.Mode = CipherMode.ECB;
            AES.Padding = PaddingMode.PKCS7;

        }

        /// <summary>
        /// Creates the SH a256 code.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <returns>System.Byte[][].</returns>
        public static byte[] CreateSha256Code(string inputvalue)
        {
            byte[] valuetosha = CODETYPE.GetBytes(inputvalue);
            byte[] result = SHA256.ComputeHash(valuetosha);
            return result;
        }

        /// <summary>
        /// Compares the SH a256.
        /// </summary>
        /// <param name="inputvale">The inputvale.</param>
        /// <param name="comparevalue">The comparevalue.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static bool CompareSha256(string inputvale, string comparevalue)
        {
            byte[] input = CODETYPE.GetBytes(inputvale);
            byte[] compare = CODETYPE.GetBytes(comparevalue);


            return (CompareSha256(input, compare));
        }

        /// <summary>
        /// Compares the SH a256.
        /// </summary>
        /// <param name="inputvale">The inputvale.</param>
        /// <param name="comparevalue">The comparevalue.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public static bool CompareSha256(byte[] inputvale, byte[] comparevalue)
        {
            byte[] inputhash = SHA256.ComputeHash(inputvale);
            byte[] comparehash = SHA256.ComputeHash(comparevalue);

            return (inputhash == comparehash);
        }

        /// <summary>
        /// Encryptors the AES.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <param name="keyArray">The key array.</param>
        /// <returns>System.Byte[][].</returns>
        /// <exception cref="System.ArgumentException">
        /// inputvalue can not be null
        /// or
        /// keyArray value was wrong
        /// </exception>
        public static byte[] EncryptorAes(string inputvalue, byte[] keyArray)
        {           
            if (string.IsNullOrEmpty(inputvalue))
            {
                throw new ArgumentException("inputvalue can not be null");
            }
            if ((keyArray == null) || (keyArray.Length < 1))
            {
                throw new ArgumentException("keyArray value was wrong");
            }


            byte[] toEncryptArray = CODETYPE.GetBytes(inputvalue);
            AES.Key = keyArray;
            ICryptoTransform cTransform = AES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return resultArray;
        }

        /// <summary>
        /// Decryptors the AES.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <param name="keyArray">The key array.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentException">
        /// inputvalue can not be null
        /// or
        /// keyArray value was wrong
        /// </exception>
        public static string DecryptorAes(byte[] inputvalue, byte[] keyArray)
        {
            if ((inputvalue == null) || (inputvalue.Length < 1))
            {
                throw new ArgumentException("inputvalue can not be null");
            }
            if ((keyArray == null) || (keyArray.Length < 1))
            {
                throw new ArgumentException("keyArray value was wrong");
            }

            AES.Key = keyArray;
            ICryptoTransform cTransform = AES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputvalue, 0, inputvalue.Length);
            return CODETYPE.GetString(resultArray);
        }

        /// <summary>
        /// Encryptors the AES to string.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <returns>System.String.</returns>
        public static string EncryptorAesToString(string inputvalue)
        {
            return Convert.ToBase64String(EncryptorAes(inputvalue, CODETYPE.GetBytes(KEYARRY.CryptogramKey)));
        }

        /// <summary>
        /// Encryptors the AES to string.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <returns>System.String.</returns>
        public static string EncryptorAesToString(string inputvalue,string key)
        {
            return Convert.ToBase64String(EncryptorAes(inputvalue, CODETYPE.GetBytes(key)));
        }

        /// <summary>
        /// Decryptors the AES from string.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <returns>System.String.</returns>
        public static string DecryptorAesFromString(string inputvalue)
        {
            return DecryptorAes(Convert.FromBase64String(inputvalue), CODETYPE.GetBytes(KEYARRY.CryptogramKey));
        }

        /// <summary>
        /// Decryptors the aes from string dynamic.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <returns>System.String.</returns>
        public static string DecryptorAesFromStringDynamic(string inputvalue)
        {
            return DecryptorAes(Convert.FromBase64String(inputvalue.Substring(6, inputvalue.Length - 6)), CODETYPE.GetBytes(inputvalue.Substring(0,6) + KEYARRY.CryptogramKey.Substring(6, KEYARRY.CryptogramKey.Length-6)));
        }


        /// <summary>
        /// Encryptors the aes to string dynamic.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <returns>System.String.</returns>
        public static string EncryptorAesToStringDynamic(string inputvalue)
        {
            string s = "ABCDEF0123456789";
            string randomString = string.Empty;
            Random random = new Random();
            Enumerable.Repeat<int>(0, 6).ToList().ForEach(x => randomString += s[random.Next(s.Length)]);

            return randomString + Convert.ToBase64String(EncryptorAes(inputvalue, CODETYPE.GetBytes(randomString + KEYARRY.CryptogramKey.Substring(6, KEYARRY.CryptogramKey.Length - 6))));
        }


        /// <summary>
        /// Encryptors the AES to string by decimal.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <returns>System.String.</returns>
        public static string EncryptorAesToString(Nullable<decimal> inputvalue)
        {
            if (inputvalue.HasValue)
            {
                var value = inputvalue.ToString();
                return Convert.ToBase64String(EncryptorAes(value, CODETYPE.GetBytes(KEYARRY.CryptogramKey)));
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Decryptors the AES from string to Decimal.
        /// </summary>
        /// <param name="inputvalue">The inputvalue.</param>
        /// <returns>System.String.</returns>
        public static decimal DecryptorAesToDecimal(string inputvalue)
        {
            if (string.IsNullOrWhiteSpace(inputvalue)) {
                return 0;
            }
            var str = DecryptorAes(Convert.FromBase64String(inputvalue), CODETYPE.GetBytes(KEYARRY.CryptogramKey));
            var res = ValueParse.GetDecimal(str);
            return res;
        }
    }
}
