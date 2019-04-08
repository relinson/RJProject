using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PRO_ReceiptsInvMgr.Client.Helper
{
    public static class JsDiskHelper
    {
        [DllImport("ReadAreaCode.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetRealTaxCode(byte[] taxcode, byte[] path);

        [DllImport("ReadAreaCode.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetKpNo(byte[] machineno, byte[] path);

        [DllImport("ReadAreaCode.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetAreaCode(byte[] areacode, byte[] path);
         
        static string path = AppDomain.CurrentDomain.BaseDirectory + "JSDiskDLL.dll";

        public static string GetNsrsbh()
        {
            string nsrsbh = string.Empty;
            byte[] taxcode = new byte[0x19];
            GetRealTaxCode(taxcode, Encoding.GetEncoding("GB18030").GetBytes(path));

            if (taxcode.Length > 0)
            {
                nsrsbh = Encoding.GetEncoding("GB18030").GetString(taxcode).Trim(new char[1]);
            }
            return nsrsbh;
        }

        public static string GetFjh()
        {
            string fjh = string.Empty;
            byte[] machineno = new byte[5];
            GetKpNo(machineno, Encoding.GetEncoding("GB18030").GetBytes(path));
            if (machineno.Length > 0)
            {
                fjh = Encoding.GetEncoding("GB18030").GetString(machineno).Trim(new char[1]);
            }
            return fjh;
        }

        public static string GetOrgCode()
        {
            string orgCode = string.Empty;
            byte[] areacode = new byte[0x19];
            GetAreaCode(areacode, Encoding.GetEncoding("GB18030").GetBytes(path));
           
            if (areacode.Length > 0)
            {
                orgCode = Encoding.GetEncoding("GB18030").GetString(areacode).Trim(new char[1]);
            }
            return orgCode;
        }
        
    }
}
