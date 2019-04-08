using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_ReceiptsInvMgr.Domain.IDAL
{
    //添加接口，起约束作用
    public partial interface IDbSession
    {
        /// <summary>
        /// 保存修改
        /// </summary>
        /// <returns>System.Int32.</returns>
        int SaveChanges();

        /// <summary>
        /// /执行sql
        /// </summary>
        /// <param name="strSql">The string SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Int32.</returns>
        int ExcuteSql(string strSql, DbParameter[] parameters);


    }
}
