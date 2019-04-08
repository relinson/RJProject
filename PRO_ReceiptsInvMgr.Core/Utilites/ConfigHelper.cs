using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;
using PRO_ReceiptsInvMgr.Resources;

namespace PRO_ReceiptsInvMgr.Core.Utilites
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// 获取AppSetting配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettingValue(string key)
        {
            try
            {
                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] != null)
                {
                    return config.AppSettings.Settings[key].Value;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(ConfigHelper), Message.ReadConfigFail+ ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return null;
        }

        /// <summary>
        /// 设置AppSetting配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int? SetAppSettingValue(string key, string value)
        {
            System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);

            int saveTag = 0;
            if (config.AppSettings.Settings[key] != null)
            {
                config.AppSettings.Settings[key].Value = value;
                saveTag = 0;
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
                saveTag = 1;
            }

            config.Save(System.Configuration.ConfigurationSaveMode.Modified);
            return saveTag;
        }

         
        /// <summary>
        /// 设置数据库连接字符串
        /// </summary>
        /// <param name="conKey"></param>
        /// <param name="dataSource"></param>
        /// <param name="initialCatalog"></param>
        /// <returns></returns>
        public static int? SetConnection(string conKey, string dataSource)
        {
            if (string.IsNullOrEmpty(conKey))
            {
                return null;
            }

            if (string.IsNullOrEmpty(dataSource))
            {
                return null;
            }
            
            int saveTag = 0;
            System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            string conncectionStr = string.Format("Data Source={0};", dataSource);
            if (config.ConnectionStrings.ConnectionStrings[conKey] == null)
            {
                System.Configuration.ConnectionStringSettings csSettings = new System.Configuration.ConnectionStringSettings(conKey, conncectionStr, "System.Data.SqlClient");
                config.ConnectionStrings.ConnectionStrings.Add(csSettings);
                saveTag = 0;
            }
            else
            {
                config.ConnectionStrings.ConnectionStrings[conKey].ConnectionString = conncectionStr;
                saveTag = 1;
            }
            config.Save(System.Configuration.ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
            return saveTag;
        }

        /// <summary>
        /// 读取数据库连接字符串
        /// </summary>
        /// <param name="conKey"></param>
        /// <returns></returns>
        public static string GetConnection(string conKey)
        {
            if (string.IsNullOrEmpty(conKey))
            {
                return null;
            }

            System.Configuration.ConnectionStringSettings cons = System.Configuration.ConfigurationManager.ConnectionStrings[conKey];
            if (cons == null)
            {
                return null;
            }

            return cons.ConnectionString;
        }

        /// <summary>
        /// 获取AppSetting配置信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettingValue(string path, string key)
        {
            System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(path);
            if (config.AppSettings.Settings[key] != null)
            {
                return config.AppSettings.Settings[key].Value;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 设置AppSetting配置信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int? SetAppSettingValue(string path,string key, string value)
        {
            System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(path);

            int saveTag = 0;
            if (config.AppSettings.Settings[key] != null)
            {
                config.AppSettings.Settings[key].Value = value;
                saveTag = 0;
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
                saveTag = 1;
            }

            config.Save(System.Configuration.ConfigurationSaveMode.Modified);
            return saveTag;
        }
    }
}
