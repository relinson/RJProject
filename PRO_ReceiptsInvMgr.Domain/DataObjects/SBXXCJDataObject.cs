using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Domain.DataObjects
{
    [Serializable]
    /// <summary>
    ///企业信息采集上传报文类
    /// </summary>
    public class SbxxcjDataObject : BaseDataObject
    {
        /// <summary>
        ///企业信息采集纳税人信息类
        /// </summary>
        public Sbxxcj NSRXX { get; set; }
        
        /// <summary>
        ///开票软件版本号
        /// </summary>
        public String KPRJ_VER { get; set; }
        
        /// <summary>
        ///MAC地址
        /// </summary>
        public String MAC { get; set; }
        
        /// <summary>
        ///IP地址
        /// </summary>
        public String IP { get; set; }
        
        /// <summary>
        ///终端类型
        /// </summary>
        public String ZDLX { get; set; }
        
        /// <summary>
        ///客户端版本号
        /// </summary>
        public String ZD_VER { get; set; }
        
        /// <summary>
        ///操作系统版本号
        /// </summary>
        public String OS_VER { get; set; }
        
        /// <summary>
        ///操作类型
        /// </summary>
        public String CZLX { get; set; }

        /// <summary>
        ///备用字段
        /// </summary>
        public String BYZD { get; set; }
    }

    [Serializable]
    /// <summary>
    ///纳税人信息类
    /// </summary>
    public class Sbxxcj
    {
        
        /// <summary>
        ///纳税人识别号
        /// </summary>
        public String NSRSBH { get; set; }
        
        /// <summary>
        ///纳税人名称
        /// </summary>
        public String NSRMC { get; set; }

        /// <summary>
        ///地区编码
        /// </summary>
        public String DQBM { get; set; }

        /// <summary>
        ///开票方地址电话
        /// </summary>
        public String DZDH { get; set; }

        /// <summary>
        ///开票银行账号
        /// </summary>
        public String YHZH { get; set; }

        /// <summary>
        ///分机号
        /// </summary>
        public String FJH { get; set; }
      
       
    }
}
