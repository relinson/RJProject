using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_ReceiptsInvMgr.Model.Tables
{
    [Table("T_Config")]
    public class TConfig: IBaseModel
    {
        /// <summary>
        ///登录纳税人账号管理表
        /// </summary>
        public TConfig() { }
        /// <summary>
        ///主键ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        ///纳税人识别号
        /// </summary>
        public string NSRSBH { get; set; }
         
        /// <summary>
        /// 纳税人名称
        /// </summary>
        public string NSRMC { get; set; }

        /// <summary>
        /// 地区代码
        /// </summary>
        public string DQDM { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPwd { get; set;}

        /// <summary>
        /// 证书密码
        /// </summary>
        public string CertPwd { get; set; }

        /// <summary>
        /// 税盘类型
        /// </summary>
        public int? TaxType { get; set; }

        /// <summary>
        /// 是否记住密码
        /// </summary>
        public bool? IsRemember { get; set; }
    }
}
