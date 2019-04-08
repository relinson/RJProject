using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Domain.DataObjects
{
    [Serializable]
    public class UpGlobalDataObject : BaseDataObject
    {
        [Serializable]
        /// <summary>
        ///通用外层报文类
        /// </summary>
        public class GeneralInfo
        {

            /// <summary>
            /// 注册码
            /// </summary>
            public string appId { get; set; }

            /// <summary>
            ///接口版本，默认1.0
            /// </summary>
            public string version { get; set; }

            /// <summary>
            ///密码10位随机数+Base64({（10位随机数+注册码）MD5})
            /// </summary>
            public string passWord { get; set; }

             /// <summary>
            ///(加密方式代码0:不加密（base64）  1: 3DES加密   2：CA加密)
            /// </summary>
            public string encryptCode { get; set; }
            
            /// <summary>
            ///base64请求数据内容或返回数据内容
            /// </summary>
            public string content { get; set; }

            /// <summary>
            ///返回上传状态
            /// </summary>
            public ReturnStateInfo state { get; set; }

       }

        

        [Serializable]
        public class ReturnStateInfo
        {
            
            /// <summary>
            ///返回代码
            /// </summary>
            public String returnCode { get; set; }


            /// <summary>
            ///返回描述
            /// </summary>
            public String returnMessage { get; set; }
        }
       

    }
}
