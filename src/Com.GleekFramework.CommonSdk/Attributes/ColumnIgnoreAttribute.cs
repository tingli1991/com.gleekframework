using System;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 忽略列属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class ColumnIgnoreAttribute : Attribute
    {

    }
}