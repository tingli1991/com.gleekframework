namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 允许用户授权范围特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class AllowAuthorizeAttribute : Attribute
    {
        /// <summary>
        /// 是否验证令牌过期
        /// </summary>
        public bool VerifyToken { get; set; } = true;

        /// <summary>
        /// 是否验证用户状态(只有验证令牌的的同时才会验证状态)
        /// </summary>
        public bool VerifyStatus { get; set; } = true;
    }
}