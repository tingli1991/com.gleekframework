using Com.GleekFramework.CommonSdk;
using Dapper;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// 字段映射工具拓展类
    /// </summary>
    public static partial class MapperHostingExtensions
    {
        /// <summary>
        /// 使用Dapper字段映射
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblyNameList">命名空间名称列表</param>
        public static IHostBuilder UseDapperColumnMap(this IHostBuilder builder, params string[] assemblyNameList)
        {
            if (assemblyNameList.IsNullOrEmpty())
                throw new ArgumentNullException("assemblyNameList");

            // 启用Dapper的下划线到驼峰命名的自动转换
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            var assemblyTypeList = assemblyNameList
                .Select(assemblyName => AssemblyProvider.GetAssembly(assemblyName))
                .Where(e => e != null)
                .SelectMany(e => e.GetTypes())
                .Distinct()
                .ToList();

            if (!assemblyTypeList.IsNotNull())
                return builder;

            assemblyTypeList.ForEach(type => SqlMapper.SetTypeMap(type, new ColumnTypeMapper(type)));
            return builder;
        }
    }
}