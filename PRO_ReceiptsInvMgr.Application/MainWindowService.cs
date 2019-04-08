using System;
using System.IO;
using System.Text;
using PRO_ReceiptsInvMgr.Core.Utilites;
using PRO_ReceiptsInvMgr.Core.Helper;
using Microsoft.Win32;
using System.Diagnostics;
using PRO_ReceiptsInvMgr.Domain.Enum;
using Newtonsoft.Json;
using System.Threading;
using System.Linq;
using System.Windows;
using PRO_ReceiptsInvMgr.Model.Tables;
using System.Collections.Generic;
using PRO_ReceiptsInvMgr.Resources;
using PRO_ReceiptsInvMgr.Application.Global;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Runtime.InteropServices;
using PRO_ReceiptsInvMgr.Domain.DataObjects;

namespace PRO_ReceiptsInvMgr.Application
{
    public class MainWindowService : BaseService
    {
        /// <summary>
        /// 检查客户端、开票软件升级
        /// </summary>
        /// <returns>true:相同,false:不同</returns>
        public bool CheckClientUpdate(int LBBM, out string serverClientVersion, out bool isForce)
        {
            serverClientVersion = string.Empty;
            string localVersion = string.Empty;
            isForce = false;
            try
            {

                var entity = SoftwareVersionService.LoadEntities(x => x.LBBM == LBBM && x.IsDownComplete == true).OrderByDescending(x => x.ID).FirstOrDefault();
                if (LBBM == (int)LbblmType.Client)
                {
                    localVersion = entity != null ? entity.Version : System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                }

                string strRequest = new JavaScriptSerializer().Serialize(new { LBBM = LBBM.ToString() });
                bool result = false;
                string errorMsg = string.Empty;
                serverClientVersion = WSInterface.GetResponse(strRequest, InterfaceType.CX, ref result, out errorMsg);
                if (result)
                {
                    dynamic serverVersionResponse = new JsonSerializer().Deserialize<dynamic>(new JsonTextReader(new StringReader(serverClientVersion)));
                    if (LBBM == (int)LbblmType.Client &&
                        GetClientVersionToDouble(localVersion) < GetClientVersionToDouble(serverVersionResponse.VERSION.ToString()))
                    {
                        serverClientVersion = serverVersionResponse.VERSION.ToString();
                        if (serverVersionResponse.QZSJBZ.ToString() == ForceUpdate.Force.GetHashCode().ToString())
                        {
                            isForce = true;
                        }
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Logging.Log4NetHelper.Error(typeof(MainWindowService), Message.CheckVersion + e.ToString());
            }
            return true;
        }

        /// <summary>
        /// 客户端版本号转为double
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        private double GetClientVersionToDouble(string version)
        {
            double versionNumber = 0;
            string strVersion = System.Text.RegularExpressions.Regex.Replace(version, @"[^0-9]+", "");
            double.TryParse(strVersion, out versionNumber);

            return versionNumber;
        }

        /// <summary>
        /// 升级服务器下载最新版本软件
        /// </summary>
        /// <param name="LBBM">类别编码， 1：开票软件2：客户端软件</param>
        /// <param name="filePath">客户端更新包路径</param>
        /// <returns></returns>
        public bool DownloadSoftware(int LBBM, ref string filePath)
        {
            bool isNewVersion = false;
            string serverClientVersion;
            bool isForce = false;
            try
            {
                if (!CheckClientUpdate(LBBM, out serverClientVersion, out isForce))
                {
                    string updateDirectory = string.Empty;
                    string RelativeFilePath = string.Empty;
                    if (!string.IsNullOrEmpty(serverClientVersion))
                    {
                        RelativeFilePath = "UpdatePackage" + @"\" + LbblmType.Client.ToString() + @"\" + serverClientVersion;
                 
                        //存放更新包路径
                        updateDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RelativeFilePath);
                    }
                    if (!Directory.Exists(updateDirectory))
                    {
                        Directory.CreateDirectory(updateDirectory);
                    }

                    string strRequest = "{\"LBBM\":\"" + LBBM + "\",\"VERSION\":\"" + serverClientVersion + "\"}";
                    bool result = true;
                    string errorMsg = string.Empty;
                    string ret = WSInterface.GetResponse(strRequest, InterfaceType.DownLoad, ref result, out errorMsg);
                    if (!result)
                    {
                        return false;
                    }
                    string localFileName = string.Empty;
                    string serverFileUrl = string.Empty;
                    string serverMd5 = string.Empty;

                    if (!string.IsNullOrEmpty(ret))
                    {
                        dynamic downLoadResponse = new JsonSerializer().Deserialize<dynamic>(new JsonTextReader(new StringReader(ret)));
                        serverFileUrl = downLoadResponse.FILEURL.ToString();
                        serverMd5 = downLoadResponse.MD5;
                        localFileName = updateDirectory + "\\" + System.IO.Path.GetFileName(serverFileUrl);
                        RelativeFilePath += "\\" + System.IO.Path.GetFileName(serverFileUrl);
                    }
                    bool isFinish = false;

                    DownloadHelper downLoadHelper = new DownloadHelper(serverFileUrl, updateDirectory);
                    downLoadHelper.GetTotalSize();

                    if (!File.Exists(localFileName) || (File.Exists(localFileName) && new FileInfo(localFileName).Length < downLoadHelper.TotalSize))
                    {
                        Logging.Log4NetHelper.Info(typeof(MainWindowService), string.Format(((LbblmType)LBBM).GetDescription() + Message.DownStart, serverClientVersion));

                        string downLoadStep = ConfigHelper.GetAppSettingValue("DownloadStep");

                        downLoadHelper.Step = !string.IsNullOrEmpty(downLoadStep) ? Convert.ToInt32(downLoadStep) : 102400;
                        while (!downLoadHelper.IsFinished)
                        {
                            downLoadHelper.Download();
                        }
                        isFinish = downLoadHelper.IsFinished;
                        if (isFinish)
                        {
                            Logging.Log4NetHelper.Info(typeof(MainWindowService), string.Format(((LbblmType)LBBM).GetDescription() + Message.UpdateSoftSuccessed, serverClientVersion, downLoadHelper.FilePath));
                        }
                    }
                    else
                    {
                        if (File.Exists(localFileName) && new FileInfo(localFileName).Length == downLoadHelper.TotalSize)
                        {
                            isFinish = true;
                        }
                        downLoadHelper.OperateFile.Close();
                        downLoadHelper.OperateFile.Dispose();
                    }

                    if (isFinish)
                    {
                        string localMd5 = CommonHelper.GetMD5HashFromFile(localFileName);

                        if (localMd5 == serverMd5)
                        {
                            var entity = SoftwareVersionService.GetFirstEntity(x => x.LBBM == LBBM && x.Version == serverClientVersion);

                            if (entity != null)
                            {
                                entity.Version = serverClientVersion;
                                entity.OPERATEDATE = DateTime.Now;
                                entity.DowloadPath = RelativeFilePath;
                                entity.IsDownComplete = true;
                                entity.IsForceUpdate = isForce;
                                SoftwareVersionService.UpdateEntities(entity);
                            }
                            else
                            {
                                entity = new SoftwareVersion();
                                entity.LBBM = LBBM;
                                entity.Name = ((LbblmType)LBBM).ToString();
                                entity.Version = serverClientVersion;
                                entity.OPERATEDATE = DateTime.Now;
                                entity.DowloadPath = RelativeFilePath;
                                entity.IsDownComplete = true;
                                entity.IsForceUpdate = isForce;
                                SoftwareVersionService.AddEntities(entity);
                            }
                             
                            filePath = localFileName;
                            isNewVersion = true;
                        }
                        else
                        {
                            Logging.Log4NetHelper.Info(typeof(MainWindowService), PRO_ReceiptsInvMgr.Resources.Message.DownloadMd5Fail);
                            File.Delete(localFileName);
                            isNewVersion = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(MainWindowService), (LBBM == LbblmType.Client.GetHashCode() ? Message.ClientUpdateError : Message.FwkpUpdateFailed) + ex.Message + System.Environment.NewLine + ex.StackTrace);
                isNewVersion = false;
            }
            return isNewVersion;
        }

        /// <summary>
        /// 升级更新下载后未更新，通过读取数据库更新记录进行更新
        /// </summary>
        /// <returns></returns>
        public string UpdateSoftware(out bool isForce)
        {
            string DowloadPath = string.Empty;
            isForce = false;
            var entityList = SoftwareVersionService.LoadEntities(x => x.LBBM == (int)LbblmType.Client && x.IsDownComplete.HasValue && x.IsDownComplete.Value).OrderByDescending(x => x.ID).FirstOrDefault();
            if (entityList != null)
            {
                string clientVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                if (GetClientVersionToDouble(clientVersion) < GetClientVersionToDouble(entityList.Version))
                {
                    DowloadPath = entityList.DowloadPath;
                    isForce = entityList.IsForceUpdate.HasValue && entityList.IsForceUpdate.Value;
                }
            }
            return DowloadPath;
        }


        private double GetVersionToDouble(string version)
        {
            double versionNumber = 0;
            string strVersion = System.Text.RegularExpressions.Regex.Replace(version, @"[^0-9]+", "");
            if (strVersion.Length >= 10)
            {
                double.TryParse(strVersion.Substring(0, 10), out versionNumber);
            }
            return versionNumber;
        }
         
        /// <summary>
        /// 校验当前纳税人是否注册
        /// </summary>
        /// <returns></returns>
        public bool IsRegister(out string errorMsg)
        {
            var ret = false;
            string strRequest = new JavaScriptSerializer().Serialize(new JXIsRegisterRequest { taxno = GlobalInfo.NSRSBH });
            bool result = false;

            var response = WSInterface.GetResponse(strRequest, InterfaceType.JXIsRegister, ref result, out errorMsg);

            if (result)
            {
                var obj = new JsonSerializer().Deserialize<JXIsRegisterResponse>(new JsonTextReader(new StringReader(response)));
                ret = obj.result == "1";
            }
            return ret;
        }

        /// <summary>
        /// 获取应用是否开放
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public AppSate GetAppIsOpen(AppCode appCode, out string errorMsg)
        {
            errorMsg = string.Empty;
            AppSate retState = AppSate.NotOpen;
            string strRequest = new JavaScriptSerializer().Serialize(
                new AppOpenRequest
                {
                    taxno = GlobalInfo.NSRSBH,
                    appId = appCode.GetHashCode().ToString(),
                });
            bool result = false;
            var response = WSInterface.GetResponse(strRequest, InterfaceType.AppOpen, ref result, out errorMsg);

            if (result)
            {
                var obj = new JsonSerializer().Deserialize<AppOpenResponse>(new JsonTextReader(new StringReader(response)));
                if (obj.appType == AppType.Pay)
                {
                    if (obj.appFlag == 1)
                    {
                        if (obj.expireFlag == 0)
                        {
                            retState = AppSate.OverTime;
                        }
                        else
                        {
                            retState = AppSate.Open;
                        }
                    }
                    else
                    {
                        retState = AppSate.NotOpen;
                    }
                }
                else
                {
                    retState = AppSate.Open;
                }
            }

            return retState;
        }
         
        /// <summary>
        /// 获取最新资讯
        /// </summary>
        /// <returns></returns>
        public AdertiseResponse GetAdertiseInfo()
        {
            AdertiseResponse advertiseResponse = new AdertiseResponse();
            string strRequest = new JavaScriptSerializer().Serialize(new { CXLX = AdvertiseCxlx.All.GetHashCode().ToString() });
            bool result = false;
            string errorMsg = string.Empty;
            var response = WSInterface.GetResponse(strRequest, InterfaceType.AdvertiseQuery, ref result, out errorMsg);
            if (result)
            {
                advertiseResponse = JsonConvert.DeserializeObject<AdertiseResponse>(response);
            }

            return advertiseResponse;
        }

        public void DownloadManual()
        {
            ManualResponse manualResponse;
            string strRequest = new JavaScriptSerializer().Serialize(new { SCVERSION = "" });
            bool result = false;
            string errorMsg = string.Empty;

            var fileName = AppDomain.CurrentDomain.BaseDirectory + @"\Init.ini";
            var myIni = new MyIniFile(fileName);
            var manualVersion = myIni.IniReadValue("InitializeInfo", "ManualVersion");
            if (string.IsNullOrEmpty(manualVersion))
            {
                manualVersion = "1.0.1";
                myIni.IniWriteValue("InitializeInfo", "ManualVersion", manualVersion);
            } 

            var response = WSInterface.GetResponse(strRequest, InterfaceType.ManualDownload, ref result, out errorMsg);
            if (result)
            {
                try
                {
                    manualResponse = new JsonSerializer().Deserialize<ManualResponse>(new JsonTextReader(new StringReader(response)));
                    if (manualResponse == null)
                    {
                        return;
                    }
                    if (!string.IsNullOrEmpty(manualResponse.SCWJURL))
                    { 
                        if (GetClientVersionToDouble(manualVersion) >= GetClientVersionToDouble(manualResponse.SCVERSION))
                        {
                            return;
                        }

                        bool isFinish = false;
                        DownloadHelper downLoadHelper = new DownloadHelper(manualResponse.SCWJURL, AppDomain.CurrentDomain.BaseDirectory);
                        downLoadHelper.GetTotalSize();

                        string localFileName = AppDomain.CurrentDomain.BaseDirectory + downLoadHelper.FileName;

                        if (!File.Exists(localFileName) || (File.Exists(localFileName) && new FileInfo(localFileName).Length < downLoadHelper.TotalSize))
                        {
                            string downLoadStep = ConfigHelper.GetAppSettingValue("DownloadStep");

                            downLoadHelper.Step = !string.IsNullOrEmpty(downLoadStep) ? Convert.ToInt32(downLoadStep) : 102400;
                            while (!downLoadHelper.IsFinished)
                            {
                                downLoadHelper.Download();
                            }
                            isFinish = downLoadHelper.IsFinished;
                            if (isFinish)
                            {
                                Logging.Log4NetHelper.Info(typeof(MainWindowService), string.Format(Resources.Message.ManualDownloadSuccess, manualResponse.SCVERSION));
                            }
                        }
                        else
                        {
                            downLoadHelper.OperateFile.Close();
                            downLoadHelper.OperateFile.Dispose();
                        }


                        string localMd5 = CommonHelper.GetMD5HashFromFile(localFileName);

                        if (localMd5 != manualResponse.MD5)
                        {
                            File.Delete(localFileName);
                        }
                        else
                        {

                            string oldChmPath = AppDomain.CurrentDomain.BaseDirectory + PRO_ReceiptsInvMgr.Resources.Common.ManualFilename;
                            string newChmPath = AppDomain.CurrentDomain.BaseDirectory + downLoadHelper.FileName;
                            if (File.Exists(newChmPath))
                            {
                                if (File.Exists(oldChmPath))
                                {
                                    File.Delete(oldChmPath);
                                }

                                FileInfo fileInfo = new FileInfo(newChmPath);
                                fileInfo.MoveTo(oldChmPath);
                            }

                            myIni.IniWriteValue("InitializeInfo", "ManualVersion", manualResponse.SCVERSION);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.Log4NetHelper.Error(typeof(MainWindowService), string.Format(Message.ManualDownloadFail, ex.Message + System.Environment.NewLine + ex.StackTrace));
                }
            }
        }

       
    }
}
