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
        public DefaultRepository DefaultRepository { get; set; }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public async Task<PageDataResult<ComAreaModel>> GetPageListAsync(ComAreaPageParam param)
        {
            var version = SnowflakeService.GetVersionNo();

            ////1.创建顶层条件组（AND逻辑）
            //var predicateGroup = new PredicateGroup { Operator = GroupOperator.And, Predicates = [] };

            ////2.添加is_deleted=false条件
            //predicateGroup.Predicates.Add(Predicates.Field<ComArea>(a => a.IsDeleted, Operator.Eq, false));

            ////3. 创建嵌套条件组（OR逻辑，处理 app_id/name LIKE）
            //if (param.Keywords.IsNotNull())
            //{
            //    var keywordPredicateGroup = new PredicateGroup { Operator = GroupOperator.Or, Predicates = [] };
            //    keywordPredicateGroup.Predicates.Add(Predicates.Field<ComArea>(a => a.Code, Operator.Like, $"%{param.Keywords}%"));
            //    keywordPredicateGroup.Predicates.Add(Predicates.Field<ComArea>(a => a.Name, Operator.Like, $"%{param.Keywords}%"));
            //    predicateGroup.Predicates.Add(keywordPredicateGroup);
            //}
            //var response = await DefaultRepository.GetPageListAsync<ComArea>(param.PageIndex, param.PageSize, predicateGroup);

            /// <summary>
            /// 排序参数集合
            /// </summary>
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
                .Where(e => !e.IsDeleted)

                //组合条件
                .Where(e => e.Id == 1 || (!e.IsDeleted && e.Id == 1) && ids.Contains(e.Id))

                //IN 和 NOT IN 场景
                .Where(e => ids.Contains(e.Id))
                .Where(e => !ids.Contains(e.Id))
                .Where(e => names.Contains(e.Name))
                .Where(e => !names.Contains(e.Name))
                .Where(e => enums.Contains(e.Level))
                .Where(e => !enums.Contains(e.Level))

                //空和非空
                .Where(e => string.IsNullOrEmpty(e.Name))
                .Where(e => !string.IsNullOrEmpty(e.Name))

                //LIKE 查询场景
                .Where(e => e.Name.Contains(columnName))
                .Where(e => !e.Name.Contains(columnName))
                .Where(e => e.Name.Contains("北京"))
                .Where(e => !e.Name.Contains("北京"))

                .Where(e => e.Name.StartsWith(columnName))
                .Where(e => !e.Name.StartsWith(columnName))
                .Where(e => e.Name.StartsWith("北京"))
                .Where(e => !e.Name.StartsWith("北京"))

                .WhereIf(param.CreateBeginTime.HasValue, e => e.CreateTime >= param.CreateBeginTime)
                .WhereIf(param.CreateEndTime.HasValue, e => e.CreateTime >= param.CreateEndTime.Value)
                .WhereIf(param.Keywords.IsNotNull(), e => e.Code.Contains(param.Keywords) || e.Name.Contains(param.Keywords))
                .Where(e => e.Name.EndsWith("北京"))

                //排序
                .OrderBy(e => e.Version)
                .OrderBy(e => e.CreateTime)
                .OrderByDescending(e => e.UpdateTime);

            var dataList = await DefaultRepository.GetListAsync(query);//查询列表
            var pageList = await DefaultRepository.GetPageListAsync(query);//查询分页列表
            var fistInfo = await DefaultRepository.GetFirstOrDefaultAsync(query);//查询单条数据
            return pageList;
        }
    }
}