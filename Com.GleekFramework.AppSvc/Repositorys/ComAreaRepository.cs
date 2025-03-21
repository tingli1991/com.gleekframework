using Com.GleekFramework.AutofacSdk;
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
        /// 默认测试仓储(读写)
        /// </summary>
        public DefaultRepository DefaultRepository { get; set; }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public async Task<PageDataResult<ComArea>> GetPageListAsync(ComAreaPageParam param)
        {
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

            //使用示例
            var ids = new long[] { 1, 2 };
            var query = new QueryableBuilder<ComArea>()
                .Where(e => e.Id == 1 || (!e.IsDeleted && e.Id == 1) && ids.Contains(e.Id))
                .Where(e => ids.Contains(e.Id))
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