using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PRO_ReceiptsInvMgr.Domain.DataObjects
{
    /// <summary>
    ///发票商品明细类
    /// </summary>
    [Serializable, XmlRoot("SPH")]
      public class FpkjmxDataObject
    {
        /// <summary>
        ///发票明细序号
        /// </summary>
        public string FPMXXH { get; set; }
        /// <summary>
        ///发票行性质 接口开票必填 0正常行、1折扣行、2被折扣行
        /// </summary>
        public string FPHXZ { get; set; }
        /// <summary>
        ///金额
        /// </summary>
        public string JE { get; set; }
        /// <summary>
        ///税率
        /// </summary>
        public string SLV { get; set; }
        /// <summary>
        ///税额
        /// </summary>
        public string SE { get; set; } 
        /// <summary>
        ///商品名称
        /// </summary>
        public string SPMC { get; set; }
        /// <summary>
        ///规格型号
        /// </summary>
        public string GGXH { get; set; } 
        /// <summary>
        ///计量单位
        /// </summary>
        public string JLDW { get; set; } 
        /// <summary>
        ///数量
        /// </summary>
        public string SL { get; set; } 
        /// <summary>
        ///单价
        /// </summary>
        public string DJ { get; set; } 
        /// <summary>
        ///含税标志
        /// </summary>
        public string HSJBZ { get; set; }
        /// <summary>
        ///商品编码
        /// </summary>
        public string SPBH { get; set; }
        /// <summary>
        ///分类编码
        /// </summary>
        public string FLBM { get; set; }
        /// <summary>
        ///享受优惠标识。0-不享受，1-享受
        /// </summary>
        public string XSYH { get; set; }
        /// <summary>
        ///优惠说明
        /// </summary>
        public string YHSM { get; set; }
        /// <summary>
        ///零税率标识
        /// </summary>
        public string LSLVBS { get; set; }


    }
}

