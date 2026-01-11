namespace Com.GleekFramework.AutofacSdk
{
    /// <summary>
    /// 依赖注入接口，IOC都要实现这个接口
    /// </summary>
    public interface IBaseAutofac { }


    /// <summary>
    /// 依赖注入接口，IOC都要实现这个接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseAutofac<T> : IBaseAutofac { }
}