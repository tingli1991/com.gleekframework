using Com.GleekFramework.AutofacSdk;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// 配置文件依赖注入实现类
    /// </summary>
    public partial class DependencyService : IBaseAutofac
    {
        /// <summary>
        /// 注册(或刷新)配置特性对照的配置值
        /// </summary>
        public void RefreshConfigAttribute() => DependencyProvider.RefreshConfigAttribute();
    }
}