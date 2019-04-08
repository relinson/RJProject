using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;
using PRO_ReceiptsInvMgr.Resources;

namespace PRO_ReceiptsInvMgr.Core.Utilites
{
    public class RegisterHelper
    {
        protected RegisterHelper() { }

        public static void RegisterDll(string strDll)
        {
            Process p = new Process();
            p.StartInfo.FileName = "Regsvr32.exe";
            p.StartInfo.Arguments = " /s " + strDll;
            p.Start();
            p.Close();
        }

        /// <summary>
        /// 检查指定的 COM 组件是否已注册到系统中
        /// </summary>
        /// <param name="clsid">指定 COM 组件的Class Id</param>
        /// <returns>true: 表示已注册；false: 表示未注册</returns>
        public static System.Boolean IsRegistered(String clsid)
        {

            //参数检查
            System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(clsid), Message.CheckClassId);

            //设置返回值
            Boolean result = false;

            //检查方法，查找注册表是否存在指定的clsid
            String key = String.Format(@"CLSID\{{{0}}}", clsid);
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(key);
            if (regKey != null)
            {
                result = true;
            }

            return result;
        }//end method


        /// <summary>
        /// 注册指定的 COM 组件到系统中
        /// </summary>
        /// <param name="file">指定的 COM 组件</param>
        /// <returns>true: 表示已注册；false: 表示未注册</returns>
        public static System.Boolean Register(String file)
        {

            //参数检查
            System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(file), Message.ChekckRegistry);

            //设置返回值
            Boolean result = false;

            //检查方法，查找注册表是否存在指定的clsid
            string fileFullName = "\"" + file + "\"";
            System.Diagnostics.Process p = System.Diagnostics.Process.Start("regsvr32", fileFullName + " /s");

            if (p != null && p.HasExited)
            {
                Int32 exitCode = p.ExitCode;
                if (exitCode == 0)
                {
                    result = true;
                }
            }
            return result;
        }//end method

        /// <summary>
        /// 反注册指定的 COM 组件
        /// </summary>
        /// <param name="file">指定的 COM 组件</param>
        /// <returns>true: 表示反注册成功；false: 表示反注册失败</returns>
        public static System.Boolean UnRegister(String file)
        {

            //参数检查
            System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(file), Message.ChekckRegistry);

            //设置返回值
            Boolean result = false;

            //检查方法，查找注册表是否存在指定的clsid
            string fileFullName = "\"" + file + "\"";
            System.Diagnostics.Process p = System.Diagnostics.Process.Start("regsvr32", fileFullName + " /s /u");

            if (p != null && p.HasExited)
            {
                Int32 exitCode = p.ExitCode;
                if (exitCode == 0)
                {
                    result = true;
                }
            }
            return result;
        }//end method

        /// <summary>
        /// 读取注册中开票软件信息
        /// </summary>
        /// <returns></returns>
        public static List<SBConfigInfo> ReadPath(string NSRSBH, string FJH)
        {
            bool isInReg = false;
            List<SBConfigInfo> list = new List<SBConfigInfo>();
            // 读取注册表 
            try
            {
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                {
                    string[] strs = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\fwkp.exe\", RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl).GetSubKeyNames();
                    if (strs != null)
                    {
                        foreach (string name in strs)
                        {
                            SBConfigInfo sb = new SBConfigInfo();
                            isInReg = true;
                            RegistryKey regRead;
                            regRead = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\fwkp.exe\" + name, RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl);
                            if (regRead.GetValue("code") != null && regRead.GetValue("machine") != null && regRead.GetValue("code").ToString().ToUpper().Trim().Equals(NSRSBH.ToUpper().Trim()) && regRead.GetValue("machine").ToString().Trim().Equals(FJH.Trim()) &&
                                regRead.GetValue("Path") != null)
                            {
                                sb.Path = regRead.GetValue("Path").ToString();
                                if (regRead.GetValue("orgcode") != null)
                                {
                                    sb.DQBM = regRead.GetValue("orgcode").ToString();
                                }
                                else
                                {
                                    sb.DQBM = "";
                                }
                                if (regRead.GetValue("Version") != null)
                                {
                                    sb.KPVersion = regRead.GetValue("Version").ToString();
                                }
                                else
                                {
                                    sb.KPVersion = "";
                                }
                                list.Add(sb);
                            }
                        }
                    }
                    else
                    {
                        Logging.Log4NetHelper.Error(typeof(RegisterHelper), Message.ReadRegistryFailed);
                        return new List<SBConfigInfo>();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(RegisterHelper), Message.GetRegistryFailed + ex.Message + System.Environment.NewLine + ex.StackTrace);
                return new List<SBConfigInfo>();
            }

            if (list.Count == 0 && isInReg)
            {
                Logging.Log4NetHelper.Error(typeof(RegisterHelper), Message.AdministratorFailed);
            }
            return list;
        }
        public class SBConfigInfo
        {
            //"终端类型标识(0:B/S请求来源;1:C/S请求来源)"
            public string Path { get; set; }
            //发票请求流水号
            public string DQBM { get; set; }
            //开票软件版本号
            public string KPVersion { get; set; }
        }
    }
}

