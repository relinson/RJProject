using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PRO_ReceiptsInvMgr.Domain.DataObjects
{
    [Serializable, XmlRoot("FPT")]
    /// <summary>
    ///发票信息
    /// </summary>
    public class FpkjfptDataObject
    {
        /// <summary>
        ///发票代码
        /// </summary>
        public string FPDM { get; set; }
        /// <summary>
        ///发票号码
        /// </summary>
        public string FPHM { get; set; }
        /// <summary>
        ///发票种类
        /// </summary>
        public string FPZL { get; set; }
        /// <summary>
        ///开票机号
        /// </summary>
        public string KPJH { get; set; } 
        /// <summary>
        ///销售单据编号
        /// </summary>
        public string XSDJBH { get; set; } 
        /// <summary>
        ///购买方名称
        /// </summary>
        public string GFMC { get; set; }
        /// <summary>
        ///购买方纳税人识别号
        /// </summary>
        public string GFSH { get; set; }
        /// <summary>
        ///购买方地址、电话
        /// </summary>
        public string GFDZDH { get; set; }
        /// <summary>
        ///购买方银行账号
        /// </summary>
        public string GFYHZH { get; set; }
        /// <summary>
        ///销售方纳税人名称
        /// </summary>
        public string XFMC { get; set; }
        /// <summary>
        ///销售方纳税人识别号
        /// </summary>
        public string XFSH { get; set; }
        /// <summary>
        ///销售方地址、电话
        /// </summary>
        public string XFDZDH { get; set; }
        /// <summary>
        ///销售方银行账号
        /// </summary>
        public string XFYHZH { get; set; }
        /// <summary>
        ///开票日期
        /// </summary>
        public string KPRQ { get; set; }
        /// <summary>
        ///所属月份
        /// </summary>
        public string SSYF { get; set; }
        /// <summary>
        ///合计金额
        /// </summary>
        public string HJJE { get; set; }
        /// <summary>
        ///合计税额
        /// </summary>
        public string HJSE { get; set; }
        /// <summary>
        ///备注
        /// </summary>
        public string BZ { get; set; }
        /// <summary>
        ///开票人
        /// </summary>
        public string KPR { get; set; } 
        /// <summary>
        ///收款人
        /// </summary>
        public string SKR { get; set; } 
        /// <summary>
        ///复核人
        /// </summary>
        public string FHR { get; set; }
        /// <summary>
        ///0-非清单发票，1-清单发票
        /// </summary>
        public string QDBZ { get; set; }
        /// <summary>
        ///作废标志
        /// </summary>
        public string ZFBZ { get; set; } 
        /// <summary>
        ///作废日期
        /// </summary>
        public string ZFRQ { get; set; } 
        /// <summary>
        ///修复标志
        /// </summary>
        public string XFBZ { get; set; } 
        /// <summary>
        ///校验码
        /// </summary>
        public string JYM { get; set; }
        /// <summary>
        ///税控设备编号
        /// </summary>     
        public string JQBH { get; set; }
        /// <summary>
        ///蓝字代码号码
        /// </summary>
        public string LZDMHM { get; set; } 
        /// <summary>
        ///编码表版本号
        /// </summary>
        public string BMBBBH { get; set; }
          /// <summary>
        ///营业税标志
        /// </summary>
        public string YYSBZ { get; set; }
        /// <summary>
        ///发票密文
        /// </summary>
        public string FPMW { get; set; }
        
        /// <summary>
        ///发票购买的详细商品信息
        /// </summary>
        public List<FpkjmxDataObject> FpkjmxDataList { get; set; }

    }
    [Serializable, XmlRoot("COMMON_FPKJ_FPT")]
    /// <summary>
    ///发票信息
    /// </summary>
    public class FpkjfptDataObject2
    {
        /// <summary>
        ///发票密文
        /// </summary>
        public string FPMW { get; set; }
    }
    }
