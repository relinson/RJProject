using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Core.Security;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using PRO_ReceiptsInvMgr.Domain.Mapper;
using PRO_ReceiptsInvMgr.Model;
using PRO_ReceiptsInvMgr.Model.Tables;
using PRO_ReceiptsInvMgr.Core.Helper;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using PRO_ReceiptsInvMgr.Domain.Enum;

namespace PRO_ReceiptsInvMgr.Application
{
    public class LoginService : BaseService
    {
        /// <summary>
        /// 获取地区列表
        /// </summary>
        /// <returns></returns>
        public List<Area> GetAreaList()
        {
            var areaList = new List<Area>();
            var list = DqdmService.LoadEntities(x => true);
            if (list.Any())
            {
                areaList = list.Select(x => new Area { Name = x.ProviceName, Code = x.DqCode }).ToList();
            }
            return areaList;
        }

        /// <summary>
        /// 获取B盘历史税号
        /// </summary>
        /// <returns></returns>
        public List<string> GetSkpNsrsbhList()
        {
            var nsrsbhList = new List<string>();
            var list = UserService.LoadEntities(x => x.TaxType == DeviceType.SKP.GetHashCode());
            if (list.Any())
            {
                nsrsbhList = list.Select(x => x.NSRSBH).ToList();
            }
            return nsrsbhList;
        }

       
        /// <summary>
        /// 保存用户信息
        /// </summary>
        public void SaveUserInfo(ConfigObject configObject)
        {
            var entity = UserService.GetFirstEntity(x => x.NSRSBH == configObject.NSRSBH);
            if (entity == null)
            {
                TConfig tconfig = CommonMapper.ToEntity<TConfig>(configObject);
                UserService.AddEntities(tconfig);
            }
            
        }

        /// <summary>
        /// 更新新增用户信息
        /// </summary>
        public void UpdateOrSaveUserInfo(ConfigObject configObject)
        {
            var entity = UserService.GetFirstEntity(x => x.NSRSBH == configObject.NSRSBH);

            if (entity != null)
            {
                string pwd = configObject.LoginPwd;
                string certPwd = configObject.CertPwd;
                entity.LoginPwd = pwd;
                entity.CertPwd = certPwd;
                entity.IsRemember = configObject.IsRemember;
                UserService.UpdateEntities(entity);
            }
            else
            {
                TConfig tconfig = CommonMapper.ToEntity<TConfig>(configObject);
                UserService.AddEntities(tconfig);
            }
        }

        /// <summary>
        ///  获取用户登录信息
        /// </summary>
        /// <returns></returns>
        public ConfigObject GetUserLogin(string nsrsbh)
        {
            TConfig userEntity = UserService.GetFirstEntity(x => x.NSRSBH == nsrsbh && x.IsRemember == true);

            if (userEntity != null)
            {
                return CommonMapper.ToDataObject<ConfigObject>(userEntity);
            }
            return null;
        }


        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Register(string appId, string nsrsbh, string nsrmc, string pwd, string areaId, out string errorMsg)
        {
            string strResult = string.Empty;
            errorMsg = string.Empty;
            try
            {
                string strRequest = new JavaScriptSerializer().Serialize(
                    new RegisterRequest
                    {
                        taxNo = nsrsbh,
                        appKey = appId,
                        orgName = nsrmc,
                        pwd = CommonHelper.MD5Encrypt(pwd),
                        areaId = areaId
                    });
                bool result = false;
                var response = WSInterface.GetResponse(strRequest, InterfaceType.Register, ref result, out errorMsg);
                return result;

            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.RegisterError, ex);
                errorMsg = PRO_ReceiptsInvMgr.Resources.Message.RegisterError;
            }
            return false;
        }

        /// <summary>
        /// 登录用户
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public LoginResponse Login(string nsrsbh, string pwd, string ftoken, out string errorMsg)
        {
            string strResult = string.Empty;
            errorMsg = string.Empty;
            try
            {
                string strRequest = new JavaScriptSerializer().Serialize(
                    new LoginRequest
                    {
                        taxNo = nsrsbh,
                        pwd = CommonHelper.MD5Encrypt(pwd),
                        token = ftoken,
                    });
                bool result = false;
                var response = WSInterface.GetResponse(strRequest, InterfaceType.Login, ref result, out errorMsg);

                if (result)
                {
                    var ret = JsonConvert.DeserializeObject<LoginResponse>(response);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.LoginError, ex);
                errorMsg = PRO_ReceiptsInvMgr.Resources.Message.LoginError;
            }
            return null;
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool ModifyPwd(string taxNo, string oldPwd, string newPwd, out string errorMsg)
        {
            string strResult = string.Empty;
            errorMsg = string.Empty;
            try
            {
                string strRequest = new JavaScriptSerializer().Serialize(
                    new ModifyPwdRequest
                    {
                        taxNo = taxNo,
                        oldPwd = CommonHelper.MD5Encrypt(oldPwd),
                        newPwd = CommonHelper.MD5Encrypt(newPwd)
                    });
                bool result = false;
                var response = WSInterface.GetResponse(strRequest, InterfaceType.ModifyPwd, ref result, out errorMsg);
                return result;
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.ModifyPwdError, ex);
                errorMsg = PRO_ReceiptsInvMgr.Resources.Message.ModifyPwdError;
            }
            return false;
        }


        /// <summary>
        /// 修改注册码
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool ModifyZcm(string taxNo, string pwd, string newZcm, out string errorMsg)
        {
            string strResult = string.Empty;
            errorMsg = string.Empty;
            try
            {
                string strRequest = new JavaScriptSerializer().Serialize(
                    new ModifyZcmRequest
                    {
                        taxNo = taxNo,
                        pwd = CommonHelper.MD5Encrypt(pwd),
                        newAppKey = newZcm
                    });
                bool result = false;
                var response = WSInterface.GetResponse(strRequest, InterfaceType.ModifyZcm, ref result, out errorMsg);
                return result;
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.ModifyZcmError, ex);
                errorMsg = PRO_ReceiptsInvMgr.Resources.Message.ModifyZcmError;
            }
            return false;
        }

    }

    public class Area
    {
        public string Name { get; set; }

        public string Code { get; set; }
    }

}
