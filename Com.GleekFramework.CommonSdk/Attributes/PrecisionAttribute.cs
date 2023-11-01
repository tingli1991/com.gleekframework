using System;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 精度特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class PrecisionAttribute : Attribute
    {
        /// <summary>
        /// 保留位数
        /// </summary>
        public byte Scale { get; set; }

        /// <summary>
        /// 精确度
        /// </summary>
        public byte Precision { get; set; }

        /// <summary>
        /// <para>自定义Decimal类型的精确度属性</para>
        /// </summary>
        /// <param name="scale">小数位数（默认2）</param>
        /// <param name="precision">精度（默认18）</param>
        public PrecisionAttribute(byte scale = 2, byte precision = 18)
        {
            Scale = scale;
            Precision = precision;
        }
    }
}