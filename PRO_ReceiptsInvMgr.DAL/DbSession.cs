using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TZZJ.JNBA.DAL;
using PRO_ReceiptsInvMgr.Domain.IDAL;
using PRO_ReceiptsInvMgr.Logging;

namespace PRO_ReceiptsInvMgr.DAL
{
    /// <summary>
    /// 数据库交互会话，
    /// 如果操作数据库的话直接从这里来操作
    /// </summary>
    public partial class DbSession:IDbSession //代表的是应用程序跟数据库之间的一次会话，也是数据库访问层的统一入口
    {
        /// <summary>
        /// 代表当前应用程序跟数据库的回话内所有的实体变化，更新会数据库
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()  //UintWork单元工作模式
        {   
            //调用EF上下文的SaveChanges的方法
            try
            {
                return  EFContextFactory.GetCurrentDbContext().SaveChanges();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, "DbContext SaveChanges", ex);
                return -1;
            }

        }

        /// <summary>
        /// Excutes the SQL.
        /// </summary>
        /// <param name="strSql">The string SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int ExcuteSql(string strSql, DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

    }
}
