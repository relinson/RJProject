using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PRO_ReceiptsInvMgr.Domain.DataObjects
{
    [Serializable, XmlRoot("RESPONSE_JL")]
    /// <summary>
    ///发票组件查询类
    /// </summary>
    public class Responsejl
    {
        /// <summary>
        /// 发票代码
        /// </summary>
        public string FPDM
        {
            get;
            set;
        }
        /// <summary>
        /// 发票号码
        /// </summary>
        public string FPHM
        {
            get;
            set;
        }
        /// <summary>
        /// 发票种类
        /// </summary>
        public string FPZL
        {
            get;
            set;
        }
        /// <summary>
        /// 开票日期
        /// </summary>
        public string KPRQ
        {
            get;
            set;
        }
        /// <summary>
        /// 合计金额
        /// </summary>
        public string HJJE
        {
            get;
            set;
        }
        /// <summary>
        /// 合计税额
        /// </summary>
        public string HJSE
        {
            get;
            set;
        }
        /// <summary>
        /// 作废标志
        /// </summary>
        public string ZFBZ
        {
            get;
            set;
        }
        /// <summary>
        /// 修复标志
        /// </summary>
        public string XFBZ
        {
            get;
            set;
        }
        /// <summary>
        ///备用字段1
        /// </summary>
        public string BYZD1
        {
            get;
            set;
        }
        /// <summary>
        /// 备用字段2
        /// </summary>
        public string BYZD2
        {
            get;
            set;
        }
        /// <summary>
        /// 备用字段3
        /// </summary>
        public string BYZD3
        {
            get;
            set;
        }

    }
}
