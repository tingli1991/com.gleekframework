## 项目

> Com.GleekFramework.RabbitMQSdk

## 依赖

> Com.GleekFramework.HttpSdk

> Com.GleekFramework.NLogSdk

> Com.GleekFramework.AutofacSdk

> Com.GleekFramework.ConsumerSdk

## 概述

RabbitMQ 拓展工具包

- 支持快速拓展
- 自定义链接字符串
- 支持`RPC`、`工作模式`、`发布订阅模式`等常用模式

## 消费端

建议所有的消费端订阅统一放到`subscription.json`中进行管理

### 订阅配置

```C#
{
  "RabbitConnectionOptions": {               //RabbitMQ消费端配置节点
    "AwaitTask": false,                      // 是否需要等待Task任务完成（用于区分同步消费还是一部消费）
    "VirtualHost": "/",                      // 虚拟主机
    "HostOptions": [                         //主机配置节点(可以订阅多个RabbitMQ)
      {
        "Port": 5672,                        //端口，默认：5672
        "UserName": "guest",                 //账号，默认：guest
        "Host": "192.168.100.29",            //主机地址
        "Password": "ChinaNet910111"         //密码，默认：guest
      }
    ]
  }
}
```

### RPC 消费定义

#### 默认队列名称

```C#
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 队列测试方法定义类
    /// </summary>
    public class RabbitRpcTestHandler : RabbitRpcHandler
    {
        /// <summary>
        /// 定义方法名称
        /// </summary>
        public override Enum ActionKey => MessageType.TEST_QUERY_RPC_NAME;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            Console.WriteLine(JsonConvert.SerializeObject(param));
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}
```

#### 自定义队列名

```C#
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 队列测试方法定义类
    /// </summary>
    public class RabbitRpcCustomerHandler : RabbitRpcHandler
    {
        /// <summary>
        /// 定义方法名称
        /// </summary>
        public override Enum ActionKey => MessageType.CUSTOMER_QUERY_RPC_NAME;

        /// <summary>
        /// 自定义RPC队列名称
        /// </summary>
        public override string QueueName => RabbitQueueConstant.RpcCustomerQueue;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            Console.WriteLine(JsonConvert.SerializeObject(param));
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}
```

### 工作模式消费

```C#
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 工作模式消费
    /// </summary>
    public class RabbitWorkTestHandler : RabbitWorkHandler
    {
        /// <summary>
        /// 定义方法名称
        /// </summary>
        public override Enum ActionKey => MessageType.TEST_QUERY_RPC_NAME;

        /// <summary>
        /// 队列名称
        /// </summary>
        public override string QueueName => RabbitQueueConstant.WorkCustomerQueue;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            Console.WriteLine(JsonConvert.SerializeObject(param));
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}
```

### 发布订阅模式

```C#
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 发布订阅模式消费
    /// </summary>
    public class RabbitSubscribeTestHandler : RabbitSubscribeHandler
    {
        /// <summary>
        /// 定义方法名称
        /// </summary>
        public override Enum ActionKey => MessageType.TEST_QUERY_RPC_NAME;

        /// <summary>
        /// 交换机名称
        /// </summary>
        public override string ExchangeName => RabbitQueueConstant.WorkCustomerExchangeName;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            Console.WriteLine(JsonConvert.SerializeObject(param));
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}
```

## 生产端

### 生产 RPC 消息

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// RabbitMQ测试控制器
    /// </summary>
    [Route("rabbit")]
    public class RabbitController : BaseController
    {
        /// <summary>
        /// 流水号生成器
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// RPC客户端服务
        /// </summary>
        public RabbitRpcClientService RabbitRpcClientService { get; set; }

        /// <summary>
        /// RPC测试
        /// </summary>
        /// <returns></returns>
        [HttpPost("rpc")]
        public async Task RpcAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var beginTime = DateTime.Now.ToCstTime();
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"张三_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "不符合规则的头部键值对" }, { "test-header-key", "正确的头部键值对" } };
                var response = RabbitRpcClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, MessageType.TEST_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"消息开始时间：{beginTime:yyyy-MM-dd HH:mm:ss}，消息处理耗时：{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
}
```

### 生产 Work 消息

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// RabbitMQ测试控制器
    /// </summary>
    [Route("rabbit")]
    public class RabbitController : BaseController
    {
        /// <summary>
        /// 流水号生成器
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// 工作模式可客户端
        /// </summary>
        public RabbitWorkClientService RabbitWorkClientService { get; set; }

        /// <summary>
        /// Work测试
        /// </summary>
        /// <returns></returns>
        [HttpPost("work")]
        public async Task WorkAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var beginTime = DateTime.Now.ToCstTime();
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"张三_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "不符合规则的头部键值对" }, { "test-header-key", "正确的头部键值对" } };
                var response = RabbitWorkClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, RabbitQueueConstant.WorkCustomerQueue, MessageType.TEST_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"消息开始时间：{beginTime:yyyy-MM-dd HH:mm:ss}，消息处理耗时：{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}
```

### 生产 Subscribe 消息

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// RabbitMQ测试控制器
    /// </summary>
    [Route("rabbit")]
    public class RabbitController : BaseController
    {
        /// <summary>
        /// 流水号生成器
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// 发布订阅模式客户端
        /// </summary>
        public RabbitSubscribeClientService RabbitSubscribeClientService { get; set; }

        /// <summary>
        /// Subscribe测试
        /// </summary>
        /// <returns></returns>
        [HttpPost("subscribe")]
        public async Task SubscribeAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var beginTime = DateTime.Now.ToCstTime();
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"张三_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "不符合规则的头部键值对" }, { "test-header-key", "正确的头部键值对" } };
                var response = RabbitSubscribeClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, RabbitQueueConstant.WorkCustomerExchangeName, MessageType.TEST_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"消息开始时间：{beginTime:yyyy-MM-dd HH:mm:ss}，消息处理耗时：{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}
```
