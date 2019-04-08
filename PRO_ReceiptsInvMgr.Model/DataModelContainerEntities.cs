using PRO_ReceiptsInvMgr.Core.Utilites;
using PRO_ReceiptsInvMgr.Model.Tables;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Model
{
    public class DataModelContainerEntities : DbContext
    {
        public DataModelContainerEntities()
        {
            this.Database.Connection.ConnectionString = ConfigHelper.GetConnection("DataModelContainerEntities") + "Password=" + PRO_ReceiptsInvMgr.Resources.Common.DbPwd;
        }
        /// <summary>
        /// 用户信息数据集
        /// </summary>
        public DbSet<TConfig> UserInfo { get; set; }
        /// <summary>
        /// 数据库版本号数据集
        /// </summary>
        public DbSet<DbVersion> DbVersion { get; set; }
        /// <summary>
        /// 升级软件数据集
        /// </summary>
        public DbSet<SoftwareVersion> SoftwareVersion { get; set; }

        public DbSet<TFpcy> T_Fpcy { get; set; }

        public DbSet<TDqdm> T_Dqdm { get; set; }

    }

}
