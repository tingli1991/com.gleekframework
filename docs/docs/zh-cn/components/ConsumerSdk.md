## 项目

> Com.GleekFramework.ConsumerSdk

## 依赖

[公共契约](/docs/zh-cn/components/ContractSdk.md)

## 概述

消费者开发工具包，它是整个消费端用来定义消费者的基础组件，例如：本地队列、Kafka、RabbitMQ 等。

- 支持属性注入。
- 统一规范消费端的定义格式。
- 支持 AOP 切面编程(系统为所有的消费端服务统一设计了`CustomActionAttribute`和`CustomAuthorizeAttribute`特性，方便大家在消费端也能更加方便的实现切面业务)。

## 效果展示

如下代码实际就是本地对了组件通过调用 ConsumerSdk 里面的 `CoustomExecuteExtensions.ExecuteAsync` 来实现最终的消费格式定义，具体怎么达到的效果可以参考`HostingQueueExtensions.SubscribeQueue`的相关逻辑，同时也可以通过`CoustomExecuteExtensions.ExecuteAsync`方法，拓展其他的消费类型中间件。

```C#
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 自定义队列消费处理类
    /// </summary>
    public class QueueCustomerHandler : QueueHandler
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public override Enum ActionKey => MessageType.CUSTOMER_TEST_QUEUE_NAME;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            var beginTime = messageBody.TimeStamp.ToDateTime();
            var totalMilliseconds = (DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
            Console.WriteLine($"主题：{Topic}，方法名称：{ActionKey}，耗时：{totalMilliseconds}，参数：{JsonConvert.SerializeObject(param)}");
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}
```

## 切面

!> **注意：**`CustomActionAttribute`和`CustomAuthorizeAttribute` 的顺序是`CustomAuthorizeAttribute`优先于`CustomActionAttribute`，这两个类有点类似于 WebAPI 里面的过滤器，在做 AOP 切面的时候，我们的自定义特性子需要继承它，并重写对应的方法即可，还有一个比较重要的点就是`CustomActionAttribute`和`CustomAuthorizeAttribute`派生的特性只能放在类、方法和接口上面。

### 应用示例

```C#
using Com.GleekFramework.AppSvc.Attributes;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 自定义队列消费处理类
    /// </summary>
    public class QueueCustomerHandler : QueueHandler
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public override Enum ActionKey => MessageType.CUSTOMER_TEST_QUEUE_NAME;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        [TestCustomAction, TestCustomAuthorize]
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            var beginTime = messageBody.TimeStamp.ToDateTime();
            var totalMilliseconds = (DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
            Console.WriteLine($"主题：{Topic}，方法名称：{ActionKey}，耗时：{totalMilliseconds}，参数：{JsonConvert.SerializeObject(param)}");
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}
```

### `CustomActionAttribute`

?> `CustomActionAttribute`可以定义多个派生，当出现多个的时候，程序会按照 Order 升序的规则进行执行。

```C#
using Com.GleekFramework.ConsumerSdk;
using Com.GleekFramework.ContractSdk;

namespace Com.GleekFramework.AppSvc.Attributes
{
    /// <summary>
    /// 测试
    /// </summary>
    public class TestCustomActionAttribute : CustomActionAttribute
    {
        /// <summary>
        /// 排序(升序排序)
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// 方法执行之前调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ContractResult> OnActionExecutingAsync(CustomActionExecutingContext context)
        {
            return base.OnActionExecutingAsync(context);
        }

        /// <summary>
        /// 方法执行完成之后调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ContractResult> OnActionExecutedAsync(CustomActionExecutedContext context)
        {
            return base.OnActionExecutedAsync(context);
        }
    }
}
```

### `CustomAuthorizeAttribute`

?> `CustomAuthorizeAttribute`可以定义多个派生，当出现多个的时候，程序会按照 Order 升序的规则进行执行。

```C#
using Com.GleekFramework.ConsumerSdk;
using Com.GleekFramework.ContractSdk;

namespace Com.GleekFramework.AppSvc.Attributes
{
    /// <summary>
    ///
    /// </summary>
    public class TestCustomAuthorizeAttribute : CustomAuthorizeAttribute
    {
        /// <summary>
        /// 排序(升序排序)
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// 方法调用之前，比 CustomActionAttribute 更早
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ContractResult> OnAuthorizationAsync(CustomAuthorizationContext context)
        {
            return base.OnAuthorizationAsync(context);
        }
    }
}
```
