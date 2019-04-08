using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PRO_ReceiptsInvMgr.Domain.DataObjects
{
    [Serializable, XmlRoot("RESPONSE_ENTINFO")]
    /// <summary>
    /// 企业信息采集实体类
    /// </summary>
    public class Entinfo : BaseDataObject
    {
        /// <summary>
        /// 纳税人识别号
        /// </summary>
        public string NSRSBH { get; set; }
        /// <summary>
        /// 纳税人名称
        /// </summary>
        public string NSRMC { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string DH { get; set; }
        /// <summary>
        /// 营业地址
        /// </summary>
        public string YYDZ { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string YHZH { get; set; }
        /// <summary>
        /// 开票方地址电话
        /// </summary>
        public string DZDH { get; set; }
        /// <summary>
        /// 分机号
        /// </summary>
        public string FJH { get; set; }
         /// <summary>
        /// 地区编码 
        /// </summary>
        public string DQBM { get; set; }
        /// <summary>
        /// 开票软件版本号
        /// </summary>
        public string KPRJVER { get; set; }
         /// <summary>
        ///备用字段1
        /// </summary>
        public string BYZD1 { get; set; }
         /// <summary>
        ///备用字段2
        /// </summary>
        public string BYZD2 { get; set; } 
         /// <summary>
        ///备用字段3
        /// </summary>
        public string BYZD3 { get; set; } 

    }
}