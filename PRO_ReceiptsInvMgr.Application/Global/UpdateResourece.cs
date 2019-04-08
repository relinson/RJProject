using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Application.Global
{
    public static class UpdateResourece
    {
        
        public static Dictionary<string, List<string>> DicUpdateRes
        {
            get;set;
        }

        /// <summary>
        /// 通过SQL文件更新数据库
        /// </summary>
        static UpdateResourece()
        {
            DicUpdateRes = new Dictionary<string,List<string>>();
            DicUpdateRes.Add("1.0.0", new List<string>());
        }

    }
}
