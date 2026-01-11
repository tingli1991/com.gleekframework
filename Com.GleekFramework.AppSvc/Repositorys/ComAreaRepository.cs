using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.DapperSdk;
using Com.GleekFramework.Models;

namespace Com.GleekFramework.AppSvc.Repositorys
{
    /// <summary>
    /// 区域仓储
    /// </summary>
    public class ComAreaRepository : IBaseAutofac
    {
        /// <summary>
        /// 流水号生成服务
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// 默认测试仓储(读写)
        /// </summary>
        public DefaultRepository<ComArea> DefaultRepository { get; set; }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public async Task<PageDataResult<ComAreaModel>> GetPageListAsync(ComAreaPageParam param)
        {
            //新增
            var comAreaInfo = new ComArea()
            {
                Code = "test",
                IsDeleted = false,
                Name = "测试名称",
                Extend = "拓展字段",
                Remark = "测试备注",
                ParentId = 0,
                Lat = "",
                Lng = "",
                Level = AreaLevel.Province
            };
            comAreaInfo = DefaultRepository.Insert(comAreaInfo);
            DefaultRepository.InsertMany([comAreaInfo]);
            comAreaInfo = await DefaultRepository.InsertAsync(comAreaInfo);
            await DefaultRepository.InsertManyAsync([comAreaInfo]);

            //修改
            comAreaInfo.Name = "测试名称修改";
            DefaultRepository.Update(comAreaInfo);
            DefaultRepository.Update(new { Name = "测试名称修改1" }, comAreaInfo.Id);
            DefaultRepository.UpdateMany(new Dictionary<object, object>() { { comAreaInfo.Id, new { Name = "测试名称修改2" } } });
            DefaultRepository.UpdateMany([comAreaInfo]);

            comAreaInfo.Name = "测试名称修改异步";
            await DefaultRepository.UpdateAsync(comAreaInfo);
            await DefaultRepository.UpdateAsync(new { Name = "测试名称修改异步1" }, comAreaInfo.Id);
            await DefaultRepository.UpdateManyAsync(new Dictionary<object, object>() { { comAreaInfo.Id, new { Name = "测试名称修改异步2" } } });
            await DefaultRepository.UpdateManyAsync([comAreaInfo]);



            //排序参数集合
            var orders = new Dictionary<string, string>()
            {
                { "create_time", "asc"},
                { "update_time", "desc" },
                { "version", "desc" }
            };

            //使用示例
            var columnName = "北京";//变量值
            var ids = new long[] { 1, 2 };
            var names = new string[] { "北京市", "京", "市" };
            var enums = new AreaLevel[] { AreaLevel.Province, AreaLevel.District, AreaLevel.City, AreaLevel.Street };
            var query = new QueryableBuilder<ComArea, ComAreaModel>()
                .Order(orders)
                //.Where(e => !e.IsDeleted)

                //组合条件
                //.Where(e => e.Level == AreaLevel.Province)
                //.Where(e => e.Id == 1 || (!e.IsDeleted && e.Id == 1))
                //.Where(e => e.Id == 1 || (!e.IsDeleted && e.Id == 1) && ids.Contains(e.Id))

                ////IN 和 NOT IN 场景
                //.Where(e => ids.Contains(e.Id))
                //.Where(e => !ids.Contains(e.Id))
                //.Where(e => names.Contains(e.Name))
                //.Where(e => !names.Contains(e.Name))
                //.Where(e => enums.Contains(e.Level))
                //.Where(e => !enums.Contains(e.Level))

                ////空和非空
                .Where(e => string.IsNullOrEmpty(e.Name))
                //.Where(e => !string.IsNullOrEmpty(e.Name))

                ////LIKE 查询场景
                //.Where(e => e.Name.Contains(columnName))
                //.Where(e => !e.Name.Contains(columnName))
                //.Where(e => e.Name.Contains("北京"))
                //.Where(e => !e.Name.Contains("北京"))

                //.Where(e => e.Name.StartsWith(columnName))
                //.Where(e => !e.Name.StartsWith(columnName))
                //.Where(e => e.Name.StartsWith("北京"))
                //.Where(e => !e.Name.StartsWith("北京"))

                //.WhereIf(param.CreateBeginTime.HasValue, e => e.CreateTime >= param.CreateBeginTime)
                //.WhereIf(param.CreateEndTime.HasValue, e => e.CreateTime >= param.CreateEndTime.Value)
                //.WhereIf(param.Keywords.IsNotNull(), e => e.Code.Contains(param.Keywords) || e.Name.Contains(param.Keywords))
                //.Where(e => e.Name.EndsWith("北京"))

                ////排序
                .OrderBy(e => e.Version)
                .OrderBy(e => e.CreateTime)
                .OrderByDescending(e => e.UpdateTime);

            var testComAreaInfo = DefaultRepository.GetFirstOrDefault(query);//查询单条记录
            var testComAreaInfo1 = await DefaultRepository.GetFirstOrDefaultAsync(query);//查询单条记录

            var comAreaInfoList = DefaultRepository.GetList(query);//获取列表
            var comAreaInfoList1 = await DefaultRepository.GetListAsync(query);//查询列表

            var comAreaInfoPageList = DefaultRepository.GetPageList(query);//查询分页列表
            var comAreaInfoPageList1 = await DefaultRepository.GetPageListAsync(query);//查询分页列表
            return comAreaInfoPageList;
        }
    }
}