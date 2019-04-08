using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using PRO_ReceiptsInvMgr.Model;
using PRO_ReceiptsInvMgr.Domain.DataObjects;

namespace PRO_ReceiptsInvMgr.Domain.Mapper
{
   public static class CommonMapper
   {
        #region EntityToObject

        /// <summary>
        /// EF实体映射业务实体
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="entity">EF实体对象</param>
        /// <returns></returns>
        public static T  ToDataObject<T>(this IBaseModel entity) where T : BaseDataObject, new()
        {
            var o = new T();
            AutoMapper.Mapper.DynamicMap(entity, o, entity.GetType(), typeof(T));
            return o;
        }

        /// <summary>
        /// EF实体列表映射业务实体列表
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="entityList">EF实体对象列表</param>
        /// <returns></returns>
        public static List<T> ToDataObjectList<T>(this IQueryable<IBaseModel> entityList) where T : BaseDataObject, new()
        {
            List<T> list = new List<T>();
            foreach (var item in entityList)
            {
                var o = new T();
                AutoMapper.Mapper.DynamicMap(item, o, item.GetType(), typeof(T));
                list.Add(o);
            }
            return list;
        }
        #endregion

        #region ObjectToEntity

        /// <summary>
        /// 业务实体映射EF实体
        /// </summary>
        /// <typeparam name="T">EF实体类型</typeparam>
        /// <param name="dataObj">业务实体对象</param>
        /// <returns></returns>
        public static T ToEntity<T>(this BaseDataObject dataObj) where T : IBaseModel, new()
        {
            var o = new T();
            AutoMapper.Mapper.DynamicMap(dataObj, o, dataObj.GetType(), typeof(T));
            return o;
        }

        /// <summary>
        /// 业务实体列表映射EF实体列表
        /// </summary>
        /// <typeparam name="T">EF实体类型</typeparam>
        /// <param name="dataObjList">业务实体对象列表</param>
        /// <returns></returns>
        public static List<T> ToEntityList<T>(this IQueryable<BaseDataObject> dataObjList) where T : IBaseModel, new()
        {
            List<T> list = new List<T>();
            foreach (var item in dataObjList)
            {
                var o = new T();
                AutoMapper.Mapper.DynamicMap(item, o, item.GetType(), typeof(T));
                list.Add(o);
            }
            return list;
        }
        #endregion

    }
}
