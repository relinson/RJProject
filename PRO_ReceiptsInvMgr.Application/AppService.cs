using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Core.Utilites;
using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Model.Tables;
using System.Configuration;
using Microsoft.Win32;
using PRO_ReceiptsInvMgr.Resources;
using PRO_ReceiptsInvMgr.Domain.Enum;
using PRO_ReceiptsInvMgr.Model;
using System.ComponentModel;

namespace PRO_ReceiptsInvMgr.Application
{
    public class AppService : BaseService
    {
        /// <summary>
        /// 数据库版本号
        /// </summary>
        public string DbVersion
        {
            get
            {
                var versionEntity = DBVersionService.GetFirstEntity(x => true);
                return versionEntity.Version;
            }
        }

        
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns></returns>
        public bool InitializeDataBase()
        {
            var result = true;
            var dbFileName = ConfigHelper.GetAppSettingValue("DbFileName");
            string filePath = AppDomain.CurrentDomain.BaseDirectory + dbFileName;
            GlobalInfo.DbPath = filePath;
            ConfigHelper.SetConnection("DataModelContainerEntities", filePath);

            var isDBNewCreated = false;
            if (!File.Exists(filePath))
            {
                string path = filePath.Substring(0, filePath.LastIndexOf("\\"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!File.Exists(filePath))
                {
                    isDBNewCreated = true;
                    System.Data.SQLite.SQLiteConnection.CreateFile(filePath);
                }

                try
                {
                    //读取配置信息，看数据库是否已经加密；如果没加密，则给数据库设置密码
                    var fileName = AppDomain.CurrentDomain.BaseDirectory + @"\Init.ini";
                    var myIni = new MyIniFile(fileName);
                    var isEncrypted = myIni.IniReadValue("InitializeInfo", "DbIsEncrypted");
                    //对已存在数据库，但没经过加密的（isEncrypted == ""）；及新建的数据库进行加密
                    if (string.IsNullOrEmpty(isEncrypted) || isDBNewCreated)
                    {
                        System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("Data Source=" + filePath);
                        con.Open();
                        con.ChangePassword(PRO_ReceiptsInvMgr.Resources.Common.DbPwd);
                        myIni.IniWriteValue("InitializeInfo", "DbIsEncrypted", "True");//标示已经加密
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log4NetHelper.Error(this, Message.DBEncryptFail + ex.Message + ex.InnerException + System.Environment.NewLine + ex.StackTrace);
                }

                try
                {
                    ExecSqlList(new List<string> { ConfigHelper.GetAppSettingValue("InitializeScriptName") });
                }
                catch (Exception ex)
                {
                    result = false;
                    Logging.Log4NetHelper.Error(this, Message.CreateDataBase + ex.Message + ex.InnerException + System.Environment.NewLine + ex.StackTrace);
                }
            }

            return result;
        }

        /// <summary>
        /// 同步数据库版本
        /// </summary>
        /// <returns></returns>
        public bool SyncServerDataBase()
        {
            var result = true;
            var isUpdate = false;
            var fileList = new List<string>();
            foreach (var item in UpdateResourece.DicUpdateRes)
            {
                if (!isUpdate)
                {
                    if (item.Key == DbVersion)
                    {
                        isUpdate = true;
                    }
                    continue;
                }
                fileList.AddRange(item.Value);
            }

            try
            {
                ExecSqlList(fileList);
            }
            catch (Exception ex)
            {
                result = false;
                Logging.Log4NetHelper.Error(typeof(AppService), Message.SyncDataBase + ex.Message + ex.InnerException + System.Environment.NewLine + ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// 执行数据库文件
        /// </summary>
        /// <param name="fileList">文件列表</param>
        private void ExecSqlList(List<string> fileList)
        {
            if (!fileList.Any())
            {
                return;
            }
            var sqlList = new List<string>();
            var ass = Assembly.GetEntryAssembly();
            foreach (var resourceName in ass.GetManifestResourceNames())
            {

                if (fileList.Any(x => resourceName.Contains(x)))
                {
                    var stream = ass.GetManifestResourceStream(resourceName);
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        sqlList.Add(reader.ReadToEnd());
                    }
                }
            }
            CommonService.ExecTranScript(sqlList);
        }
  
        
    }
}
