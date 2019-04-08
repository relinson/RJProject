using System;
using System.Collections.Generic;
using System.Linq;

namespace PRO_ReceiptsInvMgr.Domain.IDAL
{
    /// <summary>
    /// 基仓储实现的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class, new()
    {
        /// <summary>
        /// 添加 entities.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>T.</returns>
        T AddEntities(T entity);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entitys">The entitys.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        bool AddEntities(List<T> entitys);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        bool UpdateEntities(T entity);

        /// <summary>
        ///批量修改
        /// </summary>
        /// <param name="entitys">The entitys.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        bool UpdateEntities(List<T> entitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        bool DeleteEntities(T entity);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entitys">The entitys.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        bool DeleteEntities(List<T> entitys);

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="wherelambda">The wherelambda.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        IQueryable<T> LoadEntities(Func<T, bool> wherelambda);
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="wherelambda">The wherelambda.</param>
        /// <param name="isNoTracking">set isNoTracking.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        IQueryable<T> LoadEntities(Func<T, bool> wherelambda,bool isNoTracking);

        /// <summary>
        /// Loads the pager entities.分页查询
        /// </summary>
        /// <typeparam name="S">排序字段类型</typeparam>
        /// <param name="pageSize">Size of the page.分页条数</param>
        /// <param name="pageIndex">Index of the page.当前页0开始</param>
        /// <param name="total">The total.总数 返回</param>
        /// <param name="whereLambda">The where lambda.过滤条件</param>
        /// <param name="isAsc">if set to <c>true</c> [is asc].是否升序</param>
        /// <param name="orderByLambda">The order by lambda.排序字段</param>
        /// <returns>IQueryable&lt;T&gt;.返回结果列表</returns>
        IQueryable<T> LoadPagerEntities<S>(int pageSize, int pageIndex,
            out int total, Func<T, bool> whereLambda, bool isAsc, Func<T, S> orderByLambda);

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        void Dispose();

        /// <summary>
        /// 查询第一条符合条件数据
        /// </summary>
        /// <param name="wherelambda">The wherelambda.</param>
        /// <returns>T.</returns>
        T GetFirstEntity(Func<T, bool> wherelambda);
        /// <summary>
        /// 查询第一条符合条件数据
        /// </summary>
        /// <param name="wherelambda">The wherelambda.</param>
        /// <returns>T.</returns>
        T GetFirstEntity(Func<T, bool> wherelambda, bool isNoTracking);
    }
}