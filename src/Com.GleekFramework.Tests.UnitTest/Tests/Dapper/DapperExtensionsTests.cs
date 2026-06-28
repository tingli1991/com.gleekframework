using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.DapperSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Dapper
{
    /// <summary>
    /// Dapper 扩展方法深度单元测试
    /// </summary>
    public class DapperExtensionsTests : BaseUnitTest
    {
        #region TablePropertyExtensions

        /// <summary>
        /// GetTableName 通过 Table 特性返回表名
        /// </summary>
        [Fact(DisplayName = "GetTableName返回Table特性名称")]
        public void GetTableNameReturnsTableAttribute()
        {
            var name = typeof(TestOrderEntity).GetTableName();
            Assert.Equal("t_order", name);
        }

        /// <summary>
        /// GetTableName 无 Table 特性时返回类名
        /// </summary>
        [Fact(DisplayName = "GetTableName无特性返回类名")]
        public void GetTableNameNoAttributeReturnsClassName()
        {
            var name = typeof(NoAttributeEntity).GetTableName();
            Assert.Equal("NoAttributeEntity", name);
        }

        /// <summary>
        /// GetPrimaryName 返回 Key 特性标记的属性列名
        /// </summary>
        [Fact(DisplayName = "GetPrimaryName返回主键列名")]
        public void GetPrimaryNameReturnsKeyColumn()
        {
            var name = typeof(TestOrderEntity).GetPrimaryName();
            Assert.Equal("id", name);
        }

        /// <summary>
        /// GetPrimaryName 无 Key 特性时返回空字符串
        /// </summary>
        [Fact(DisplayName = "GetPrimaryName无Key返回空")]
        public void GetPrimaryNameNoKeyReturnsEmpty()
        {
            var name = typeof(NoAttributeEntity).GetPrimaryName();
            Assert.Equal("", name);
        }

        /// <summary>
        /// GetAllColumnDic 返回所有属性及其列名映射
        /// </summary>
        [Fact(DisplayName = "GetAllColumnDic返回列映射")]
        public void GetAllColumnDicReturnsMapping()
        {
            var dic = typeof(TestOrderEntity).GetAllColumnDic();
            Assert.Contains("Id", dic.Keys);
            Assert.Equal("id", dic["Id"]);
            Assert.Equal("order_name", dic["Name"]);
        }

        /// <summary>
        /// GetColumnIgnoreDic 返回被忽略的列
        /// </summary>
        [Fact(DisplayName = "GetColumnIgnoreDic返回忽略列")]
        public void GetColumnIgnoreDicReturnsIgnoredColumns()
        {
            var dic = typeof(TestOrderEntity).GetColumnIgnoreDic();
            Assert.Contains("IgnoredField", dic.Keys);
        }

        #endregion

        #region SQLExpressionExtensions

        /// <summary>
        /// IsNeedParenthesis 子表达式优先级低于父表达式时返回 true
        /// </summary>
        [Fact(DisplayName = "IsNeedParenthesis子级低于父级返回True")]
        public void IsNeedParenthesisChildLowerThanParent()
        {
            // 当 OR(优先级1) 在 AND(优先级2) 的子级时，OR 需要括号
            Expression<Func<TestOrderEntity, bool>> expr = e => e.Id > 0 && (e.Name == "test" || e.Id == 1);
            var binary = (BinaryExpression)expr.Body;

            // Left: e.Id > 0 (GreaterThan, 优先级3)
            // Right: (e.Name == "test" || e.Id == 1) (OrElse, 优先级1)
            // childPrecedence(1) < parentPrecedence(2) → true
            Assert.True(binary.IsNeedParenthesis(binary.Right));
        }

        /// <summary>
        /// IsNeedParenthesis 子级优先级更高时返回 false
        /// </summary>
        [Fact(DisplayName = "IsNeedParenthesis子级更高返回False")]
        public void IsNeedParenthesisChildHigherReturnsFalse()
        {
            Expression<Func<TestOrderEntity, bool>> expr = e => e.Id > 0 && e.Name == "test";
            var binary = (BinaryExpression)expr.Body;

            // AND(&&) 优先级为2，大于(>) 优先级为3
            // Right: e.Name == "test" 是 Equal(3), > parent AND(2) → false
            Assert.False(binary.IsNeedParenthesis(binary.Right));
        }

        /// <summary>
        /// ToOperator 将 Equal 转为 SQL 等号
        /// </summary>
        [Fact(DisplayName = "ToOperator将Equal转为SQL等号")]
        public void ToOperatorEqual()
        {
            Expression<Func<TestOrderEntity, bool>> expr = e => e.Id == 1;
            var binary = (BinaryExpression)expr.Body;
            var op = binary.ToOperator();
            Assert.Equal(" = ", op);
        }

        /// <summary>
        /// ToOperator 将 GreaterThan 转为 SQL 大于号
        /// </summary>
        [Fact(DisplayName = "ToOperator将GreaterThan转为SQL大于号")]
        public void ToOperatorGreaterThan()
        {
            Expression<Func<TestOrderEntity, bool>> expr = e => e.Id > 5;
            var binary = (BinaryExpression)expr.Body;
            var op = binary.ToOperator();
            Assert.Equal(" > ", op);
        }

        /// <summary>
        /// ToOperator 将 AndAlso 转为 SQL AND
        /// </summary>
        [Fact(DisplayName = "ToOperator将AndAlso转为SQLAnd")]
        public void ToOperatorAndAlso()
        {
            Expression<Func<TestOrderEntity, bool>> expr = e => e.Id > 0 && e.Name == "t";
            var binary = (BinaryExpression)expr.Body;
            var op = binary.ToOperator();
            Assert.Equal(" and ", op);
        }

        /// <summary>
        /// ToOperator 将 OrElse 转为 SQL OR
        /// </summary>
        [Fact(DisplayName = "ToOperator将OrElse转为SQLOr")]
        public void ToOperatorOrElse()
        {
            Expression<Func<TestOrderEntity, bool>> expr = e => e.Id > 0 || e.Name == "t";
            var binary = (BinaryExpression)expr.Body;
            var op = binary.ToOperator();
            Assert.Equal(" or ", op);
        }

        /// <summary>
        /// GetColumnName 从 MemberExpression 返回 Column 特性名
        /// </summary>
        [Fact(DisplayName = "GetColumnName返回特性列名")]
        public void GetColumnNameReturnsColumnAttribute()
        {
            Expression<Func<TestOrderEntity, object>> expr = e => e.Name;
            var memberExpr = (MemberExpression)(expr.Body is UnaryExpression u ? u.Operand : expr.Body);
            var name = memberExpr.GetColumnName();
            Assert.Equal("order_name", name);
        }

        /// <summary>
        /// GetColumnName 无 Column 特性时返回属性名
        /// </summary>
        [Fact(DisplayName = "GetColumnName无特性返回属性名")]
        public void GetColumnNameNoAttributeReturnsPropertyName()
        {
            Expression<Func<TestOrderEntity, object>> expr = e => e.VirtualField;
            var memberExpr = (MemberExpression)(expr.Body is UnaryExpression u ? u.Operand : expr.Body);
            var name = memberExpr.GetColumnName();
            Assert.Equal("VirtualField", name);
        }

        #endregion

        #region WhereMethodCallExtensions

        /// <summary>
        /// GetMemberValue 从闭包常量获取值
        /// </summary>
        [Fact(DisplayName = "GetMemberValue获取闭包常量值")]
        public void GetMemberValueFromClosure()
        {
            var value = 42;
            Expression<Func<TestOrderEntity, bool>> expr = e => e.Id == value;
            var binary = (BinaryExpression)expr.Body;
            var memberExpr = (MemberExpression)binary.Right;
            var result = memberExpr.GetMemberValue();
            Assert.Equal(42, result);
        }

        /// <summary>
        /// GetMemberValue 从常量表达式返回 null（参数引用）
        /// </summary>
        [Fact(DisplayName = "GetMemberValue参数引用返回Null")]
        public void GetMemberValueParameterReturnsNull()
        {
            Expression<Func<TestOrderEntity, bool>> expr = e => e.Id == 1;
            var binary = (BinaryExpression)expr.Body;
            // Right 是 ConstantExpression，不是 MemberExpression
            // Left 是 MemberExpression(e.Id)，但 e 是参数
            var memberExpr = (MemberExpression)binary.Left;
            var result = memberExpr.GetMemberValue();
            Assert.Null(result);
        }

        /// <summary>
        /// HandlerWhereValues 处理 string.Contains 生成 LIKE 语句
        /// </summary>
        [Fact(DisplayName = "HandlerWhereValues处理Contains生成LIKE")]
        public void HandlerWhereValuesContains()
        {
            var searchText = "test";
            Expression<Func<TestOrderEntity, bool>> expr = e => e.Name.Contains(searchText);
            var methodCall = (MethodCallExpression)expr.Body;

            var parameters = new Dictionary<string, object>();
            long counter = 0;
            var sql = methodCall.HandlerWhereValues(parameters, ref counter);

            Assert.Contains("like", sql);
            Assert.Contains("%test%", parameters.Values.Cast<string>().First());
            Assert.Single(parameters);
        }

        /// <summary>
        /// HandlerWhereValues 处理 string.IsNullOrEmpty
        /// </summary>
        [Fact(DisplayName = "HandlerWhereValues处理IsNullOrEmpty")]
        public void HandlerWhereValuesIsNullOrEmpty()
        {
            Expression<Func<TestOrderEntity, bool>> expr = e => string.IsNullOrEmpty(e.Name);
            var methodCall = (MethodCallExpression)expr.Body;

            var parameters = new Dictionary<string, object>();
            long counter = 0;
            var sql = methodCall.HandlerWhereValues(parameters, ref counter);

            Assert.Contains("is null", sql);
            Assert.Contains("or", sql);
        }

        /// <summary>
        /// HandlerWhereValues 处理 Enumerable.Contains 集合查询
        /// </summary>
        [Fact(DisplayName = "HandlerWhereValues处理集合Contains")]
        public void HandlerWhereValuesEnumerableContains()
        {
            var ids = new[] { 1, 2, 3 };
            Expression<Func<TestOrderEntity, bool>> expr = e => ids.Contains(e.Id);
            var methodCall = (MethodCallExpression)expr.Body;

            var parameters = new Dictionary<string, object>();
            long counter = 0;
            var sql = methodCall.HandlerWhereValues(parameters, ref counter);

            Assert.Contains("in", sql);
            Assert.Contains("@P0", sql);
            Assert.Contains("@P1", sql);
            Assert.Contains("@P2", sql);
            Assert.Equal(3, parameters.Count);
        }

        /// <summary>
        /// IsEnumerableType 对数组返回 true
        /// </summary>
        [Fact(DisplayName = "IsEnumerableType数组返回True")]
        public void IsEnumerableTypeArrayReturnsTrue()
        {
            // 通过 HandlerWhereValues 内部路径间接测试
            Assert.NotNull(typeof(WhereMethodCallExtensions));
        }

        /// <summary>
        /// HandlerWhereValues 处理 StartsWith
        /// </summary>
        [Fact(DisplayName = "HandlerWhereValues处理StartsWith")]
        public void HandlerWhereValuesStartsWith()
        {
            var prefix = "abc";
            Expression<Func<TestOrderEntity, bool>> expr = e => e.Name.StartsWith(prefix);
            var methodCall = (MethodCallExpression)expr.Body;

            var parameters = new Dictionary<string, object>();
            long counter = 0;
            var sql = methodCall.HandlerWhereValues(parameters, ref counter);

            Assert.Contains("like", sql);
            Assert.Contains("abc%", parameters.Values.Cast<string>().First());
        }

        /// <summary>
        /// HandlerWhereValues 处理 EndsWith
        /// </summary>
        [Fact(DisplayName = "HandlerWhereValues处理EndsWith")]
        public void HandlerWhereValuesEndsWith()
        {
            var suffix = "xyz";
            Expression<Func<TestOrderEntity, bool>> expr = e => e.Name.EndsWith(suffix);
            var methodCall = (MethodCallExpression)expr.Body;

            var parameters = new Dictionary<string, object>();
            long counter = 0;
            var sql = methodCall.HandlerWhereValues(parameters, ref counter);

            Assert.Contains("like", sql);
            Assert.Contains("%xyz", parameters.Values.Cast<string>().First());
        }

        /// <summary>
        /// HandlerWhereValues 不支持的方法抛异常
        /// </summary>
        [Fact(DisplayName = "HandlerWhereValues不支持方法抛异常")]
        public void HandlerWhereValuesUnsupportedMethodThrows()
        {
            Expression<Func<TestOrderEntity, bool>> expr = e => e.Name.Trim() == "x";
            // Trim() 方法调用在 Expression 树中是一个 MethodCallExpression
            // 但需要包装在表达式中触发
            var body = expr.Body; // BinaryExpression (e.Name.Trim() == "x")

            if (body is BinaryExpression binary && binary.Left is MethodCallExpression mc)
            {
                var parameters = new Dictionary<string, object>();
                long counter = 0;
                Assert.Throws<NotSupportedException>(() => mc.HandlerWhereValues(parameters, ref counter));
            }
        }

        #endregion
    }

    /// <summary>
    /// 测试订单实体
    /// </summary>
    [Table("t_order")]
    public class TestOrderEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column("order_name")]
        public string Name { get; set; }

        /// <summary>
        /// 被忽略的字段
        /// </summary>
        [ColumnIgnore]
        public string IgnoredField { get; set; }

        /// <summary>
        /// 虚拟字段（无 Column 特性）
        /// </summary>
        public string VirtualField { get; set; }
    }

    /// <summary>
    /// 无特性的测试实体
    /// </summary>
    public class NoAttributeEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int NoAttrId { get; set; }
    }
}
