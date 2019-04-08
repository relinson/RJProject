using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace PRO_ReceiptsInvMgr.Client.Helper
{
    public static class DeviceHelper
    {
        public static List<string> SerialNumber
        {
            get;set;
        }

        /// <summary>
        /// 调用这个函数将本机所有U盘序列号存储到_serialNumber中
        /// </summary>
        public static void matchDriveLetterWithSerial()
        {
            string[] diskArray;
            string driveNumber;
            SerialNumber = new List<string>();
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDiskToPartition");
            foreach (var dm in searcher.Get())
            {
                getValueInQuotes(dm["Dependent"].ToString());
                diskArray = getValueInQuotes(dm["Antecedent"].ToString()).Split(',');
                driveNumber = diskArray[0].Remove(0, 6).Trim();
                var disks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (var disk in disks.Get())
                {
                    if (disk["Name"].ToString() == ("\\\\.\\PHYSICALDRIVE" + driveNumber) && disk["InterfaceType"].ToString() == "USB")
                    {
                        SerialNumber.Add(parseSerialFromDeviceID(disk["PNPDeviceID"].ToString()));
                    }
                }
            }
        }
        private static string parseSerialFromDeviceID(string deviceId)
        {
            var splitDeviceId = deviceId.Split('\\');
            var arrayLen = splitDeviceId.Length - 1;
            var serialArray = splitDeviceId[arrayLen].Split('&');
            var serial = serialArray[0];
            return serial;
        }

        private static string getValueInQuotes(string inValue)
        {
            var posFoundStart = inValue.IndexOf("\"");
            var posFoundEnd = inValue.IndexOf("\"", posFoundStart + 1);
            var parsedValue = inValue.Substring(posFoundStart + 1, (posFoundEnd - posFoundStart) - 1);
            return parsedValue;
        }
    }
}
