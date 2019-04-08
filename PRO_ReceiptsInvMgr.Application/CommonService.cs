using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRO_ReceiptsInvMgr.Model;
using PRO_ReceiptsInvMgr.Core.Utilites;
using Microsoft.Win32;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Domain.Enum;

namespace PRO_ReceiptsInvMgr.Application
{
    public static class CommonService
    {
        public static void ExecTranScript(List<string> commandList)
        {
            using (DataModelContainerEntities db = new DataModelContainerEntities())
            {
                db.Database.Connection.ConnectionString = ConfigHelper.GetConnection("DataModelContainerEntities") + "Password=" + PRO_ReceiptsInvMgr.Resources.Common.DbPwd;
                var trans = db.Database.BeginTransaction();
                try
                {
                    foreach (var item in commandList)
                    {
                        db.Database.ExecuteSqlCommand(item);
                    }

                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("SQL logic error or missing database\r\nduplicate column name"))
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                trans.Commit();
            }
        }

         

    }
}
