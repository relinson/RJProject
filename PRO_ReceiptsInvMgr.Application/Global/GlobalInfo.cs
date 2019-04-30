using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using Microsoft.Win32;
using PRO_ReceiptsInvMgr.Core.Helper;

namespace PRO_ReceiptsInvMgr.Application.Global
{
    /// <summary>
    /// 全局信息
    /// </summary>
    public static class GlobalInfo
    {
        /// <summary>
        /// 纳税人识别号
        /// </summary>
        public static string NSRSBH
        {
            get;
            set;
        }

        /// <summary>
        /// 纳税人名称
        /// </summary>
        public static string NSRMC
        {
            get;
            set;
        }

        /// <summary>
        /// 分机号
        /// </summary>
        public static string FJH
        {
            get;
            set;
        }

        /// <summary>
        /// 地区编号
        /// </summary>
        public static string orgCode { get; set; }


        /// <summary>
        ///  进项应用token
        /// </summary>
        public static string token { get; set; }

        /// <summary>
        /// 证书密码
        /// </summary>
        public static string JxPwd { get; set; }

        /// <summary>
        /// 税款所属期
        /// </summary>
        public static string skssq { get; set; }

        /// <summary>
        /// 可勾选发票开始日期
        /// </summary>
        public static string selectStartDate { get; set; }

        /// <summary>
        /// 可勾选发票截至日期
        /// </summary>
        public static string selectEndDate { get; set; }
  
        /// <summary>
        /// 地区代码
        /// </summary>
        public static string Dqdm { get; set; }


        /// <summary>
        /// 开票设备类型（1:金税盘 2:税控盘）
        /// </summary>
        public static int DeviceType { get; set; }

        public static string LogPath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables("%SystemDrive%") + PRO_ReceiptsInvMgr.Resources.Common.SoftLogPath;
            }
        }

        /// <summary>
        /// 下载本地路径
        /// </summary>
        public static string DownloadPath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables("%SystemDrive%") + PRO_ReceiptsInvMgr.Resources.Common.DownLoadPath;
            }
        }
         
        /// <summary>
        ///  是否插入税盘
        /// </summary>
        public static bool ExistTax
        {
            get;
            set;

        }

     
        /// <summary>
        /// 数据库路径
        /// </summary>
        public static string DbPath
        {
            get;
            set;
        }


        /// <summary>
        /// 注册码
        /// </summary>
        public static string AppId { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public static string ExpiredTime { get; set; }
    }
}
