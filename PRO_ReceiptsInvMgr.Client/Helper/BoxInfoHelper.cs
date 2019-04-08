using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PRO_ReceiptsInvMgr.Client.Helper
{
    /// <summary>
    /// 盒子信息
    /// </summary>
    class BoxInfoHelper
    {
        protected BoxInfoHelper() { }

        [DllImport("AisinoJsbDll.dll")]
        private static extern bool Aisino_GetJQBH(byte[] jqbh);

        /// <summary>
        /// 获取盒子编号
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceNo()
        {
            string retStr = string.Empty;
            byte[] deviceNo = new byte[24];

            try
            {
                Aisino_GetJQBH(deviceNo);
                if (deviceNo.Length > 0)
                {
                    string strResult = Encoding.UTF8.GetString(deviceNo);
                    if (strResult.Contains("-"))
                    {
                        retStr = strResult.Split('-')[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(BoxInfoHelper), PRO_ReceiptsInvMgr.Resources.Message.GetDeviceError, ex);
            }

            return retStr;
        }
    }
}
