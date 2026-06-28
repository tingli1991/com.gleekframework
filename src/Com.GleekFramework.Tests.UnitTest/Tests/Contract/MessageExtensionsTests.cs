using Com.GleekFramework.ContractSdk;
using Newtonsoft.Json;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Contract
{
    /// <summary>
    /// 消息扩展方法单元测试
    /// </summary>
    public class MessageExtensionsTests : BaseUnitTest
    {
        /// <summary>
        /// 测试用人员类
        /// </summary>
        public class Person
        {
            /// <summary>
            /// 姓名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 年龄
            /// </summary>
            public int Age { get; set; }
        }

        /// <summary>
        /// GetData 从 MessageBody(object) 中提取 JSON 字符串数据
        /// </summary>
        [Fact(DisplayName = "GetData从MessageBody提取数据")]
        public void GetDataFromMessageBodyObjectReturnsData()
        {
            var expected = new Person { Name = "张三", Age = 30 };
            var json = JsonConvert.SerializeObject(expected);
            var messageBody = new MessageBody<object> { Data = json, SerialNo = "SN123", ActionKey = "test_action" };
            var result = messageBody.GetData<Person>();
            Assert.NotNull(result);
            Assert.Equal(expected.Name, result.Name);
            Assert.Equal(expected.Age, result.Age);
        }

        /// <summary>
        /// GetData 从 MessageBody(T) 中提取数据
        /// </summary>
        [Fact(DisplayName = "GetData从泛型MessageBody提取数据")]
        public void GetDataFromMessageBodyGenericReturnsData()
        {
            var expected = new Person { Name = "李四", Age = 25 };
            var messageBody = new MessageBody<Person> { Data = expected, SerialNo = "SN456", ActionKey = "test_action" };
            var result = messageBody.GetData<Person>();
            Assert.NotNull(result);
            Assert.Equal(expected.Name, result.Name);
        }

        /// <summary>
        /// GetData Data 为 null 时返回 null
        /// </summary>
        [Fact(DisplayName = "GetData为Null返回Null")]
        public void GetDataNullDataReturnsNull()
        {
            var messageBody = new MessageBody<object> { Data = null, SerialNo = "SN789" };
            var result = messageBody.GetData<Person>();
            Assert.Null(result);
        }

        /// <summary>
        /// GetDataAsync 异步获取数据
        /// </summary>
        [Fact(DisplayName = "GetDataAsync异步获取数据")]
        public async Task GetDataAsyncReturnsData()
        {
            var expected = new Person { Name = "赵六", Age = 22 };
            var messageBody = new MessageBody<Person> { Data = expected, SerialNo = "SN345" };
            var result = await messageBody.GetDataAsync<Person>();
            Assert.NotNull(result);
            Assert.Equal(expected.Name, result.Name);
        }

        /// <summary>
        /// GetData 非 MessageBody(object) 或 MessageBody(T) 时抛异常
        /// </summary>
        [Fact(DisplayName = "GetData非正确类型抛异常")]
        public void GetDataInvalidTypeThrowsException()
        {
            var messageBody = new MessageBody { SerialNo = "SN678" };
            Assert.Throws<InvalidOperationException>(() => messageBody.GetData<Person>());
        }

        /// <summary>
        /// MessageBody 属性赋值正确
        /// </summary>
        [Fact(DisplayName = "MessageBody属性赋值正确")]
        public void MessageBodyPropertiesSetCorrectly()
        {
            var headers = new Dictionary<string, string> { { "key1", "value1" } };
            var messageBody = new MessageBody<object>
            {
                ActionKey = "my_action",
                SerialNo = "SN_001",
                TimeStamp = 1234567890L,
                Headers = headers
            };
            Assert.Equal("my_action", messageBody.ActionKey);
            Assert.Equal("SN_001", messageBody.SerialNo);
            Assert.Equal(1234567890L, messageBody.TimeStamp);
            Assert.Equal("value1", messageBody.Headers["key1"]);
        }
    }
}
