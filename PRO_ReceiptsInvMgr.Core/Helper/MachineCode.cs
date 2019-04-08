using Microsoft.Win32;
using PRO_ReceiptsInvMgr.Resources;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace PRO_ReceiptsInvMgr.Core.Helper
{
    public class MachineCode
    {
        static MachineCode machineCode;
        /// <summary>
        /// 获取机器代码
        /// </summary>
        /// <returns></returns>
        public static string GetMachineCodeString()
        {
            string machineCodeString = string.Empty;
            if (machineCode == null)
            {
                machineCode = new MachineCode();
            }
            machineCodeString = "PC." + machineCode.GetCpuInfo() + "." +
                                machineCode.GetHDid() + "." +
                                machineCode.GetMoAddress();
            return machineCodeString;
        }

        ///   <summary> 
        ///   获取cpu序列号     
        ///   </summary> 
        ///   <returns> string </returns> 
        public string GetCpuInfo()
        {
            string cpuInfo = "";
            using (ManagementClass cimobject = new ManagementClass("Win32_Processor"))
            {
                ManagementObjectCollection moc = cimobject.GetInstances();

                foreach (var mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                    mo.Dispose();
                }
            }
            return cpuInfo.ToString();
        }

        ///   <summary> 
        ///   获取硬盘ID     
        ///   </summary> 
        ///   <returns> string </returns> 
        public string GetHDid()
        {
            string HDid = "";
            using (ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive"))
            {
                ManagementObjectCollection moc1 = cimobject1.GetInstances();
                foreach (var mo in moc1)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                    mo.Dispose();
                }
            }
            return HDid.ToString();
        }

        ///   <summary> 
        ///   获取网卡硬件地址 
        ///   </summary> 
        ///   <returns> string </returns> 
        public string GetMoAddress()
        {
            string MoAddress = "";
            using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                ManagementObjectCollection moc2 = mc.GetInstances();
                foreach (var mo in moc2)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        MoAddress = mo["MacAddress"].ToString();
                    }
                    mo.Dispose();
                }
            }
            return MoAddress.ToString();
        }

        /// <summary>
        /// 获取开票软件路径
        /// </summary>
        /// <param name="nsrsbh">纳税人识别号</param>
        /// <param name="fjh">分机号</param>
        /// <returns></returns>
        public static string GetKwfpAddress(string nsrsbh, string fjh)
        {
            string path = "";
            try
            {
                RegistryKey hkml = Registry.LocalMachine;
                string regeditFwkp = Resources.Common.RegeditFwkp;
                RegistryKey subKey = hkml.OpenSubKey(regeditFwkp).OpenSubKey(nsrsbh + "." + fjh);
                path = (string)subKey.GetValue("Path");
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(MachineCode), Message.GetKPPathFailed + ex.Message.ToString());
            }
            return path;
        }
    }
}
