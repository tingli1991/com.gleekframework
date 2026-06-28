using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.CommonSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Attribute
{
    /// <summary>
    /// 授权模式枚举单元测试
    /// </summary>
    public class GrantTypeTests : BaseUnitTest
    {
        /// <summary>
        /// GrantType.PASSWORD 值为 1
        /// </summary>
        [Fact(DisplayName = "PASSWORD值为1")]
        public void PasswordValueIs1()
        {
            Assert.Equal(1, (int)GrantType.PASSWORD);
        }

        /// <summary>
        /// GrantType.AUTHORIZATION_CODE 值为 2
        /// </summary>
        [Fact(DisplayName = "AUTHORIZATION_CODE值为2")]
        public void AuthorizationCodeValueIs2()
        {
            Assert.Equal(2, (int)GrantType.AUTHORIZATION_CODE);
        }

        /// <summary>
        /// GrantType.CLIENT_CREDENTIALS 值为 3
        /// </summary>
        [Fact(DisplayName = "CLIENT_CREDENTIALS值为3")]
        public void ClientCredentialsValueIs3()
        {
            Assert.Equal(3, (int)GrantType.CLIENT_CREDENTIALS);
        }

        /// <summary>
        /// GrantType.PASSWORD 的 Description 为"密码模式"
        /// </summary>
        [Fact(DisplayName = "PASSWORD描述为密码模式")]
        public void PasswordDescriptionIsCorrect()
        {
            Assert.Equal("密码模式", GrantType.PASSWORD.GetDescription());
        }

        /// <summary>
        /// GrantType.AUTHORIZATION_CODE 的 Description 为"授权码模式"
        /// </summary>
        [Fact(DisplayName = "AUTHORIZATION_CODE描述为授权码模式")]
        public void AuthorizationCodeDescriptionIsCorrect()
        {
            Assert.Equal("授权码模式", GrantType.AUTHORIZATION_CODE.GetDescription());
        }

        /// <summary>
        /// RouteRequiredAttribute 构造设置错误消息
        /// </summary>
        [Fact(DisplayName = "RouteRequiredAttribute设置错误消息")]
        public void RouteRequiredAttributeSetsErrorMessage()
        {
            var attr = new RouteRequiredAttribute("参数必填");
            Assert.Equal("参数必填", attr.ErrorMessage);
        }

        /// <summary>
        /// RouteRequiredAttribute 构造设置路由名称和错误消息
        /// </summary>
        [Fact(DisplayName = "RouteRequiredAttribute设置名称和错误消息")]
        public void RouteRequiredAttributeWithNameSetsProperties()
        {
            var attr = new RouteRequiredAttribute("app_id", "应用Id不能为空");
            Assert.Equal("app_id", attr.Name);
            Assert.Equal("应用Id不能为空", attr.ErrorMessage);
        }

        /// <summary>
        /// RouteRequiredAttribute 默认绑定源为 Path
        /// </summary>
        [Fact(DisplayName = "RouteRequiredAttribute默认绑定为Path")]
        public void RouteRequiredAttributeBindingSourceIsPath()
        {
            var attr = new RouteRequiredAttribute("test", "错误");
            Assert.Equal(Microsoft.AspNetCore.Mvc.ModelBinding.BindingSource.Path, attr.BindingSource);
        }
    }
}
