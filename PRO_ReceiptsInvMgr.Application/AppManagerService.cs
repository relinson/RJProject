using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Core.Utilites;
using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Model.Tables;
using System.Configuration;
using Microsoft.Win32;
using PRO_ReceiptsInvMgr.Resources;
using PRO_ReceiptsInvMgr.Domain.Enum;
using PRO_ReceiptsInvMgr.Model;
using PRO_ReceiptsInvMgr.Domain.Mapper;
using PRO_ReceiptsInvMgr.Domain;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace PRO_ReceiptsInvMgr.Application
{
    public class AppManagerService : BaseService
    {
     
        /// <summary>
        /// 获取常用应用
        /// </summary>
        /// <returns></returns>
        public AppResponse GetAppInfoResponse(out bool isSuccess)
        {
            AppResponse appResponse = new AppResponse();
            string strRequest = new JavaScriptSerializer().Serialize(new { YYRJLX = DownloadType.App.GetHashCode().ToString() });
            bool result = false;

            string errorMsg = string.Empty;
            var response = WSInterface.GetResponse(strRequest, InterfaceType.AppDownload, ref result,out errorMsg);
            isSuccess = result;
          
            if (result)
            {
                appResponse = new JsonSerializer().Deserialize<AppResponse>(new JsonTextReader(new StringReader(response)));
            }
            return appResponse;
        }


        /// <summary>
        /// 版本比较
        /// </summary>
        /// <param name="version1"></param>
        /// <param name="version2"></param>
        /// <returns></returns>
        public int compareVersion(string version1, string version2)
        {
            if (string.IsNullOrEmpty(version1))
            {
                return -1;
            }
            if (version1 == version2)
            {
                return 0;
            }
            string[] version1Array = version1.Split('.');
            string[] version2Array = version2.Split('.');
            int index = 0;
            //获取最小长度值
            int minLen = Math.Min(version1Array.Length, version2Array.Length);
            int diff = 0;
            //循环判断每位的大小
            while (index < minLen && (diff = Int32.Parse(version1Array[index]) - Int32.Parse(version2Array[index])) == 0)
            {
                index++;
            }
            if (diff == 0)
            {
                //如果位数不一致，比较多余位数
                for (int i = index; i < version1Array.Length; i++)
                {
                    if (Int32.Parse(version1Array[i]) > 0)
                    {
                        return 1;
                    }
                }

                for (int i = index; i < version2Array.Length; i++)
                {
                    if (Int32.Parse(version2Array[i]) > 0)
                    {
                        return -1;
                    }
                }
                return 0;
            }
            else
            {
                return diff > 0 ? 1 : -1;
            }
        }

    }
}
