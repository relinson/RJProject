using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Model.Tables
{
    [Table("SoftwareVersion")]
    public class SoftwareVersion : IBaseModel
    {
        /// <summary>
        ///软件升级表
        /// </summary>
        public SoftwareVersion() { }
        /// <summary>
        ///主键ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        ///软件类别
        /// </summary>
        public Int64 LBBM { get; set; }
        /// <summary>
        ///软件当前版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        ///软件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///已下载路径
        /// </summary>
        public string DowloadPath { get; set; }
        /// <summary>
        ///操作时间
        /// </summary>
        public Nullable<DateTime> OPERATEDATE { get; set; }
        /// <summary>
        ///是否下载完成
        /// </summary>
        public Nullable<bool> IsDownComplete { get; set; }
        /// <summary>
        ///是否强制升级
        /// </summary>
        public Nullable<bool> IsForceUpdate { get; set; }
    }
}
