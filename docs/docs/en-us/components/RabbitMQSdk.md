## Project

> Com.GleekFramework.RabbitMQSdk

## Dependencies

> Com.GleekFramework.HttpSdk

> Com.GleekFramework.NLogSdk

> Com.GleekFramework.AutofacSdk

> Com.GleekFramework.ConsumerSdk

## Overview

RabbitMQ extension toolkit

- Supports rapid expansion
- Custom connection string
- Supports common modes such as `RPC`, `Work mode`, and `Publish/Subscribe mode`

## Consumer Side

It is recommended to manage all consumer side subscriptions in `subscription.json`.

### Subscription Configuration

```C#
{
  "RabbitConnectionOptions": {               // RabbitMQ consumer configuration node
    "AwaitTask": false,                      // Whether to wait for Task task completion (for distinguishing synchronous from asynchronous consumption)
    "VirtualHost": "/",                      // Virtual host
    "HostOptions": [                         // Host configuration node (can subscribe to multiple RabbitMQs)
      {
        "Port": 5672,                        // Port, default: 5672
        "UserName": "guest",                 // Username, default: guest
        "Host": "192.168.100.29",            // Host address
        "Password": "ChinaNet910111"         // Password, default: guest
      }
    ]
  }
}
```

### RPC Consumption Definition

#### Default Queue Name

```C#
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Queue test method definition class
    /// </summary>
    public class RabbitRpcTestHandler : RabbitRpcHandler
    {
        /// <summary>
        /// Define method name
        /// </summary>
        public override Enum ActionKey => MessageType.TEST_QUERY_RPC_NAME;

        /// <summary>
        /// Execute method
        /// </summary>
        /// <param name="messageBody">Message content</param>
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

#### Custom Queue Name

```C#
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Queue test method definition class
    /// </summary>
    public class RabbitRpcCustomerHandler : RabbitRpcHandler
    {
        /// <summary>
        /// Define method name
        /// </summary>
        public override Enum ActionKey => MessageType.CUSTOMER_QUERY_RPC_NAME;

        /// <summary>
        /// Custom RPC queue name
        /// </summary>
        public override string QueueName => RabbitQueueConstant.RpcCustomerQueue;

        /// <summary>
        /// Execute method
        /// </summary>
        /// <param name="messageBody">Message content</param>
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

### Work Mode Consumption

```C#
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Work mode consumption
    /// </summary>
    public class RabbitWorkTestHandler : RabbitWorkHandler
    {
        /// <summary>
        /// Define method name
        /// </summary>
        public override Enum ActionKey => MessageType.TEST_QUERY_RPC_NAME;

        /// <summary>
        /// Queue name
        /// </summary>
        public override string QueueName => RabbitQueueConstant.WorkCustomerQueue;

        /// <summary>
        /// Execute method
        /// </summary>
        /// <param name="messageBody">Message content</param>
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

### Publish/Subscribe Mode

```C#
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RabbitMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Publish/Subscribe mode consumption
    /// </summary>
    public class RabbitSubscribeTestHandler : RabbitSubscribeHandler
    {
        /// <summary>
        /// Define method name
        /// </summary>
        public override Enum ActionKey => MessageType.TEST_QUERY_RPC_NAME;

        /// <summary>
        /// Exchange name
        /// </summary>
        public override string ExchangeName => RabbitQueueConstant.WorkCustomerExchangeName;

        /// <summary>
        /// Execute method
        /// </summary>
        /// <param name="messageBody">Message content</param>
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

## Producer Side

### Produce RPC Message

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
    /// RabbitMQ test controller
    /// </summary>
    [Route("rabbit")]
    public class RabbitController : BaseController
    {
        /// <summary>
        /// Serial number generator
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// RPC client service
        /// </summary>
        public RabbitRpcClientService RabbitRpcClientService { get; set; }

        /// <summary>
        /// RPC test
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
                var headers = new Dictionary<string, string>() { { "test_header_key", "Invalid header key-value pairs" }, { "test-header-key", "Correct header key-value pairs" } };
                var response = RabbitRpcClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, MessageType.TEST_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"Message start time: {beginTime:yyyy-MM-dd HH:mm:ss}, message processing time: {(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
}
```

### Produce Work Message

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
    /// RabbitMQ test controller
    /// </summary>
    [Route("rabbit")]
    public class RabbitController : BaseController
    {
        /// <summary>
        /// Serial number generator
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// Work mode client
        /// </summary>
        public RabbitWorkClientService RabbitWorkClientService { get; set; }

        /// <summary>
        /// Work test
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
                var headers = new Dictionary<string, string>() { { "test_header_key", "Invalid header key-value pairs" }, { "test-header-key", "Correct header key-value pairs" } };
                var response = RabbitWorkClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, RabbitQueueConstant.WorkCustomerQueue, MessageType.TEST_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"Message start time: {beginTime:yyyy-MM-dd HH:mm:ss}, message processing time: {(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}
```

### Produce Subscribe Message

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
    /// RabbitMQ test controller
    /// </summary>
    [Route("rabbit")]
    public class RabbitController : BaseController
    {
        /// <summary>
        /// Serial number generator
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// Publish/Subscribe mode client
        /// </summary>
        public RabbitSubscribeClientService RabbitSubscribeClientService { get; set; }

        /// <summary>
        /// Subscribe test
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
                var headers = new Dictionary<string, string>() { { "test_header_key", "Invalid header key-value pairs" }, { "test-header-key", "Correct header key-value pairs" } };
                var response = RabbitSubscribeClientService.PublishAsync(RabbitConfig.RabbitDefaultClientHosts, RabbitQueueConstant.WorkCustomerExchangeName, MessageType.TEST_QUERY_RPC_NAME, param, serialNo, headers);
                Console.WriteLine($"Message start time: {beginTime:yyyy-MM-dd HH:mm:ss}, message processing time: {(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}
```
