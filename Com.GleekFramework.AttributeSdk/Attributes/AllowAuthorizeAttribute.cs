namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 允许用户授权范围特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class AllowAuthorizeAttribute : Attribute
    {
        /// <summary>
        /// 是否验证用户必填
        /// </summary>
        public bool VerifyUser { get; set; } = false;

        /// <summary>
        /// 是否验证令牌过期
        /// </summary>
        public bool VerifyToken { get; set; } = true;

        /// <summary>
        /// 验证用户状态
        /// </summary>
        public bool VerifyStatus { get; set; } = false;
    }
}