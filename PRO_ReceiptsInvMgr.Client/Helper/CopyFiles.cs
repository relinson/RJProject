

using System;
using System.Management;
using System.Configuration;
using System.IO;
using log4net;
using PRO_ReceiptsInvMgr.Core.Utilites;
using PRO_ReceiptsInvMgr.Application.Global;
using System.Diagnostics;

namespace PRO_ReceiptsInvMgr.Client.Helper
{
    public static class CopyFiles
    {

        public static void MoveDirectory(string srcPath, string destPath)
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)
                {
                    if (!Directory.Exists(destPath + PRO_ReceiptsInvMgr.Resources.Common.Slash + i.Name))
                    {
                        Directory.CreateDirectory(destPath + PRO_ReceiptsInvMgr.Resources.Common.Slash + i.Name);
                    }
                    MoveDirectory(i.FullName, destPath + PRO_ReceiptsInvMgr.Resources.Common.Slash + i.Name);
                }
                else
                {
                    if (!File.Exists(destPath + PRO_ReceiptsInvMgr.Resources.Common.Slash + i.Name))
                    {
                        File.Move(i.FullName, destPath + PRO_ReceiptsInvMgr.Resources.Common.Slash + i.Name);
                    }
                }
            }
        }
        /// <summary>
        /// 复制文件到系统文件夹
        /// </summary>
        public static void CopySysWOW64(string nsrsbh, string fjh, string drive)
        {

            string InitSettingFilePath = AppDomain.CurrentDomain.BaseDirectory + "Init";
            MyIniFile ini = new MyIniFile(InitSettingFilePath);

            string systemPath = string.Empty;
            //判断当前系统是否是64位系统
            var bit = GetOSBit();
            if (bit == 64)
            {
                systemPath = Environment.ExpandEnvironmentVariables("%SYSTEMROOT%") + @"\SysWOW64\";
            }
            else
            {
                systemPath = Environment.ExpandEnvironmentVariables("%SYSTEMROOT%") + @"\System32\";
            }
            var filePath = drive + @"SystemFile\";

            if (!Directory.Exists(systemPath))
            {
                return;
            }
            //判断目标路径中是否还有AisinoCSP文件夹，禁止重复复制
            var configValue = ini.IniReadValue("CopyFile", "SystemFile");
            if (Directory.Exists(systemPath + @"\AisinoCSP\") && configValue == "1")
            {
                return;
            }

            CopyFolder(nsrsbh, fjh, filePath, systemPath);
            ini.IniWriteValue("CopyFile", "SystemFile", "1");
        }

        /// <summary>
        /// 获取操作系统
        /// </summary>
        /// <returns>返回当前电脑是32位系统还是64位系统</returns>
        private static int GetOSBit()
        {
            string addressWidth = String.Empty;
            ConnectionOptions mConnOption = new ConnectionOptions();
            ManagementScope mMs = new ManagementScope(PRO_ReceiptsInvMgr.Resources.Common.LocalHost, mConnOption);
            ObjectQuery mQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
            ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(mMs, mQuery);
            ManagementObjectCollection mObjectCollection = mSearcher.Get();
            foreach (var mObject in mObjectCollection)
            {
                addressWidth = mObject["AddressWidth"].ToString();
            }
            return Int32.Parse(addressWidth);
        }

        /// <summary>
        /// 复制开票软件文件夹
        /// </summary>
        public static void CopyKP(string nsrsbh, string fjh, string drive)
        {
            string InitSettingFilePath = AppDomain.CurrentDomain.BaseDirectory + "Init";
            MyIniFile ini = new MyIniFile(InitSettingFilePath);

            var filePath = drive + @"\fwkpBase\";
            //获取系统盘符 
            var systemPath = string.Format(@"{0}\fwkp\", drive);
            //目标路径中包含该纳税人，禁止重复复制
            if (Directory.Exists(systemPath))
            {
                var configValue = ini.IniReadValue("CopyFile", nsrsbh + "." + fjh);
                if (Directory.Exists(systemPath + nsrsbh + "." + fjh) && configValue == "1")
                {
                    return;
                }
            }
            ini.IniWriteValue("CopyFile", nsrsbh + "." + fjh, "0");

            CopyFolder(nsrsbh, fjh, filePath, systemPath);

            ini.IniWriteValue("CopyFile", nsrsbh + "." + fjh, "1");

        }

        /// <summary>
        /// 复制子文件夹和文件
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private static void CopyFolder(string nsrsbh, string fjh, string from, string to)
        {
            if (!Directory.Exists(to))
            {
                Directory.CreateDirectory(to);
            }

            // 子文件夹
            foreach (string sub in Directory.GetDirectories(from))
            {
                var filename = Path.GetFileName(sub);
                //如果是开票sbh.fjh，更换为配置文件的纳税人和分机号
                if (filename.Equals("sbh.fjh"))
                {
                    filename = nsrsbh + "." + fjh;
                }
                CopyFolder(nsrsbh, fjh, sub + "\\", to + filename + "\\");
            }

            // 文件
            foreach (string file in Directory.GetFiles(from))
            {
                if (!File.Exists(to + Path.GetFileName(file)))
                {
                    File.Copy(file, to + Path.GetFileName(file));
                }
            }
        }
    }
}
