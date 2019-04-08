using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using PRO_ReceiptsInvMgr.Domain.IDAL;
using PRO_ReceiptsInvMgr.Logging;
using PRO_ReceiptsInvMgr.Model;
using PRO_ReceiptsInvMgr.Core.Utilites;

namespace PRO_ReceiptsInvMgr.DAL
{
    /// <summary>
    /// 仓储实现类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BaseRepository<T> : IBaseRepository<T>, IDisposable where T : class, new()
    {

        //EF上下文的实例保证，线程内唯一
        //实例化EF框架
        //获取的实当前线程内部的上下文实例，而且保证了线程内上下文实例唯一
       
        private DbContext db = new DataModelContainerEntities();
        private DbContext OrcDB
        { 
            get
            {
                if (isDisposed.HasValue && isDisposed.Value)
                {
                    db = new DataModelContainerEntities();
                    isDisposed = false;
                }
                return db;
            }
            set
            {
                db.Dispose();
                OrcDB = value;
            }
        }
        private bool? isDisposed = null;

        /// <summary>
        ///新增单条数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public T AddEntities(T entity)
        {
            try
            {
                OrcDB.Entry<T>(entity).State = EntityState.Added;
                OrcDB.SaveChanges();
                Dispose();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, "DB Add Save Error", ex);
                return new T();
            }
            return entity;
        }

        /// <summary>
        /// 新增多条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool AddEntities(List<T> entitys)
        {
            int saveCount = 0;
            try
            {
                foreach (T entity in entitys)
                {
                    OrcDB.Entry<T>(entity).State = EntityState.Added;
                }
                saveCount = OrcDB.SaveChanges();
                Dispose();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, "DB Add Save Error", ex);
            }
            return saveCount > 0;
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateEntities(T entity)
        {
            int saveCount = 0;
            try
            {
                OrcDB.Set<T>().Attach(entity);
                OrcDB.Entry<T>(entity).State = EntityState.Modified;
                saveCount = OrcDB.SaveChanges();
                Dispose();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, "DB Update Save Error", ex);
            }
            return saveCount > 0;
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool UpdateEntities(List<T> entitys)
        {
            int saveCount = 0;
            try
            {
                Dispose();
                foreach (T entity in entitys)
                {
                    OrcDB.Set<T>().Attach(entity);
                    OrcDB.Entry<T>(entity).State = EntityState.Modified;
                }
                saveCount = OrcDB.SaveChanges();
                Dispose();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, "DB Update Save Error", ex);
            }
            return saveCount > 0;
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntities(T entity)
        {
            int saveCount = 0;
            try
            {
                OrcDB.Set<T>().Attach(entity);
                OrcDB.Entry<T>(entity).State = EntityState.Deleted;
                saveCount = OrcDB.SaveChanges();
                Dispose();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, "DB Delete Save Error", ex);
            }
            return saveCount > 0;
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool DeleteEntities(List<T> entitys)
        {
            int saveCount = 0;
            try
            {
                foreach (T entity in entitys)
                {
                    OrcDB.Set<T>().Attach(entity);
                    OrcDB.Entry<T>(entity).State = EntityState.Deleted;
                }
                saveCount = OrcDB.SaveChanges();
                Dispose();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, "DB Delete Save Error", ex);
            }
            return saveCount > 0;
        }

        /// <summary>
        /// 获取满足条件的所有数据
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(Func<T, bool> wherelambda)
        {
            return OrcDB.Set<T>().Where<T>(wherelambda).AsQueryable();
        }
        public IQueryable<T> LoadEntities(Func<T, bool> wherelambda,bool isNoTracking)
        {
            if (isNoTracking)
            {
                return OrcDB.Set<T>().AsNoTracking().Where<T>(wherelambda).AsQueryable();
            }
            else
            {
                return OrcDB.Set<T>().Where<T>(wherelambda).AsQueryable();
            }
        }


        /// <summary>
        /// 获取满足条件的第一条数据
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>
        public T GetFirstEntity(Func<T, bool> wherelambda)
        {
            Dispose();
            T entity = OrcDB.Set<T>().FirstOrDefault(wherelambda);
            return entity;
        }
        public T GetFirstEntity(Func<T, bool> wherelambda, bool isNoTracking)
        {
            Dispose();
            if (isNoTracking)
            {
                Dispose();
                T entity = OrcDB.Set<T>().AsNoTracking().FirstOrDefault(wherelambda);
                return entity;
            }
            else {
                Dispose();
                T entity = OrcDB.Set<T>().FirstOrDefault(wherelambda);
                return entity;
            }
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
        public IQueryable<T> LoadPagerEntities<S>(int pageSize, int pageIndex, out int total,
            Func<T, bool> whereLambda, bool isAsc, Func<T, S> orderByLambda)
        {
            var tempData = OrcDB.Set<T>().Where<T>(whereLambda);
            total = tempData.Count();

            //排序获取当前页的数据
            if (isAsc)
            {
                tempData = tempData.OrderBy<T, S>(orderByLambda).
                      Skip<T>(pageSize * (pageIndex)).
                      Take<T>(pageSize).AsQueryable();
            }
            else
            {
                tempData = tempData.OrderByDescending<T, S>(orderByLambda).
                     Skip<T>(pageSize * (pageIndex)).
                     Take<T>(pageSize).AsQueryable();
            }
            return tempData.AsQueryable();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            db.Dispose();
            isDisposed = true;
        }
    }
}
