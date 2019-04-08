using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Model
{
    [Table("DbVersion")]
    /// <summary>
    ///数据库版本表
    /// </summary>
    public class DbVersion : IBaseModel
    {
        public DbVersion() { }
        /// <summary>
        ///表ID
        /// </summary>
        public Int64 ID { get; set; }

        /// <summary>
        ///版本号
        /// </summary>
        public string Version { get; set; }
    }
}
