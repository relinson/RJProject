using System;
using System.Collections.Generic;
using System.Linq;
using PRO_ReceiptsInvMgr.DAL;
using PRO_ReceiptsInvMgr.Domain.IBLL;
using PRO_ReceiptsInvMgr.Domain.IDAL;
using PRO_ReceiptsInvMgr.Logging;

namespace PRO_ReceiptsInvMgr.BLL
{
    /// <summary>
    /// 基础service类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> : IBaseService<T>, IDisposable where T : class, new()
    {
        //在调用这个方法的时候必须给他赋值
        private readonly BaseRepository<T> baseRepository = new BaseRepository<T>();
        public IBaseRepository<T> CurrentRepository
        {
            get
            {
                return baseRepository;
            }
        }

        /// <summary>
        ///新增单条数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public T AddEntities(T entity)
        {
            //如果在这里操作多个表的话，实现批量的操作
            //调用T对应的仓储来添加
            var addentity = CurrentRepository.AddEntities(entity);

            return addentity;
        }

        /// <summary>
        /// 新增多条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool AddEntities(List<T> entitys)
        {
            var addEntity = CurrentRepository.AddEntities(entitys);

            return addEntity;
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateEntities(T entity)
        {
            var updateEntity = CurrentRepository.UpdateEntities(entity);

            return updateEntity;
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool UpdateEntities(List<T> entitys)
        {
            var updateEntity = CurrentRepository.UpdateEntities(entitys);

            return updateEntity;
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntities(T entity)
        {
            var deleteEntity = CurrentRepository.DeleteEntities(entity);

            return deleteEntity;
        }

       /// <summary>
       /// 删除多条数据
       /// </summary>
       /// <param name="entitys"></param>
       /// <returns></returns>
        public bool DeleteEntities(List<T> entitys)
        {
            var deleteEntitys = CurrentRepository.DeleteEntities(entitys);

            return deleteEntitys;
        }

       /// <summary>
       /// 获取满足条件的第一条数据
       /// </summary>
       /// <param name="wherelambda"></param>
       /// <returns></returns>
        public T GetFirstEntity(Func<T, bool> wherelambda)
        {
            try
            {
                return CurrentRepository.GetFirstEntity(wherelambda);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Get First Entity", "Database Error", ex);
                return new T();
            }
        }
       
        public T GetFirstEntity(Func<T, bool> wherelambda, bool isNoTracking)
        {
            try
            {
                return CurrentRepository.GetFirstEntity(wherelambda,isNoTracking);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Get First Entity", "Database Error", ex);
                return new T();
            }
        }

        /// <summary>
        /// 获取满足条件的所有数据
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(Func<T, bool> wherelambda)
        {
            return CurrentRepository.LoadEntities(wherelambda);
        }
        public IQueryable<T> LoadEntities(Func<T, bool> wherelambda,bool isNoTracking)
        {
            return CurrentRepository.LoadEntities(wherelambda,isNoTracking);
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">查询的起始下标</param>
        /// <param name="total">满足条件的总条数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="isAsc">升降序</param>
        /// <param name="orderByLambda">排序字段</param>
        /// <returns></returns>
        public IQueryable<T> LoadPagerEntities<S>(int pageSize, int pageIndex,
             out int total, Func<T, bool> whereLambda, bool isAsc, Func<T, S> orderByLambda)
        {
            return CurrentRepository.LoadPagerEntities(pageSize, pageIndex, out total, whereLambda, isAsc, orderByLambda);
        }


        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            CurrentRepository.Dispose();
        }
    }
}
