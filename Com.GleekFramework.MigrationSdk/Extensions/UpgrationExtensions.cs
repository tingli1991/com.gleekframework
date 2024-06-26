﻿using FluentMigrator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 升级拓展类
    /// </summary>
    public static partial class UpgrationExtensions
    {
        /// <summary>
        /// 运行版本升级
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="options">配置选项</param>
        /// <returns></returns>
        public static async Task UpgrationAsync(this IServiceScope scope, MigrationOptions options)
        {
            if (scope == null || !options.UpgrationSwitch)
            {
                return;
            }
            await UpgrationProvider.ExecuteAsync(scope.ServiceProvider);
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="transactionBehavior"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static async Task ExecuteAsync(this TransactionBehavior transactionBehavior, Action callback)
        {
            if (transactionBehavior == TransactionBehavior.None)
            {

                //执行回调函数
                callback();
            }
            else
            {
                var options = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };
                using var scope = new TransactionScope(TransactionScopeOption.Required, options);
                callback();//执行回调函数
                scope.Complete();//提交事务
            }
            await Task.CompletedTask;
        }
    }
}