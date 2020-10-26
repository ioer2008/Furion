﻿using Fur.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fur.DatabaseAccessor
{
    /// <summary>
    /// 数据库公开类
    /// </summary>
    [SkipScan]
    public static class Db
    {
        /// <summary>
        /// 迁移类库名称
        /// </summary>
        internal static string MigrationAssemblyName = "Fur.Database.Migrations";

        /// <summary>
        /// 是否启用自定义租户类型
        /// </summary>
        internal static bool CustomizeMultiTenants;

        /// <summary>
        /// 基于表的多租户外键名
        /// </summary>
        internal static string OnTableTenantId = nameof(Entity.TenantId);

        /// <summary>
        /// 未找到服务错误消息
        /// </summary>
        private const string NotFoundServiceErrorMessage = "{0} Service not registered or uninstalled.";

        /// <summary>
        /// 获取非泛型仓储
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        public static IRepository GetRepository()
        {
            return App.GetRequestService<IRepository>()
                ?? App.GetService<IRepository>()
                ?? throw new NotSupportedException(string.Format(NotFoundServiceErrorMessage, nameof(IRepository)));
        }

        /// <summary>
        /// 获取实体仓储
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns>IRepository<TEntity></returns>
        public static IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IPrivateEntity, new()
        {
            return App.GetRequestService<IRepository<TEntity>>()
                ?? App.GetService<IRepository<TEntity>>()
                ?? throw new NotSupportedException(string.Format(NotFoundServiceErrorMessage, nameof(IRepository<TEntity>)));
        }

        /// <summary>
        /// 获取实体仓储
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <returns>IRepository<TEntity, TDbContextLocator></returns>
        public static IRepository<TEntity, TDbContextLocator> GetRepository<TEntity, TDbContextLocator>()
            where TEntity : class, IPrivateEntity, new()
            where TDbContextLocator : class, IDbContextLocator
        {
            return App.GetRequestService<IRepository<TEntity, TDbContextLocator>>()
                ?? App.GetService<IRepository<TEntity, TDbContextLocator>>()
                ?? throw new NotSupportedException(string.Format(NotFoundServiceErrorMessage, nameof(IRepository<TEntity, TDbContextLocator>)));
        }

        /// <summary>
        /// 获取Sql仓储
        /// </summary>
        /// <returns>ISqlRepository</returns>
        public static ISqlRepository GetSqlRepository()
        {
            return App.GetRequestService<ISqlRepository>()
                ?? App.GetService<ISqlRepository>()
                ?? throw new NotSupportedException(string.Format(NotFoundServiceErrorMessage, nameof(ISqlRepository)));
        }

        /// <summary>
        /// 获取Sql仓储
        /// </summary>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <returns>ISqlRepository<TDbContextLocator></returns>
        public static ISqlRepository<TDbContextLocator> GetSqlRepository<TDbContextLocator>()
            where TDbContextLocator : class, IDbContextLocator
        {
            return App.GetRequestService<ISqlRepository<TDbContextLocator>>()
                ?? App.GetService<ISqlRepository<TDbContextLocator>>()
                ?? throw new NotSupportedException(string.Format(NotFoundServiceErrorMessage, nameof(ISqlRepository<TDbContextLocator>)));
        }

        /// <summary>
        /// 获取Sql代理
        /// </summary>
        /// <returns>ISqlRepository</returns>
        public static TSqlDispatchProxy GetSqlDispatchProxy<TSqlDispatchProxy>()
            where TSqlDispatchProxy : class, ISqlDispatchProxy
        {
            return App.GetRequestService<TSqlDispatchProxy>()
                ?? App.GetService<TSqlDispatchProxy>()
                ?? throw new NotSupportedException(string.Format(NotFoundServiceErrorMessage, nameof(ISqlDispatchProxy)));
        }

        /// <summary>
        /// 获取瞬时数据库上下文
        /// </summary>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <returns></returns>
        public static DbContext GetDbContext<TDbContextLocator>()
            where TDbContextLocator : class, IDbContextLocator
        {
            // 判断是否注册了数据库上下文
            if (!Penetrates.DbContextWithLocatorCached.ContainsKey(typeof(TDbContextLocator))) return default;

            var dbContextResolve = App.GetService<Func<Type, ITransient, DbContext>>();
            return dbContextResolve(typeof(TDbContextLocator), default);
        }

        /// <summary>
        /// 获取作用域数据库上下文
        /// </summary>
        /// <typeparam name="TDbContextLocator">数据库上下文定位器</typeparam>
        /// <returns></returns>
        public static DbContext GetRequestDbContext<TDbContextLocator>()
            where TDbContextLocator : class, IDbContextLocator
        {
            var dbContextResolve = App.GetRequestService<Func<Type, IScoped, DbContext>>();
            return dbContextResolve(typeof(TDbContextLocator), default);
        }
    }
}