## Project

> Com.GleekFramework.ConsumerSdk

## Dependencies

[Common Contract](/docs/en-us/components/ContractSdk.md)

## Overview

Consumer Development Toolkit, which is the foundational component used across the consumer side to define consumers, such as local queues, Kafka, RabbitMQ, etc.

- Supports property injection.
- Unifies the definition format of the consumer side.
- Supports AOP aspect-oriented programming (the system has uniquely designed `CustomActionAttribute` and `CustomAuthorizeAttribute` for all consumer-side services, facilitating the implementation of aspect-oriented business more conveniently on the consumer side).

## Demonstration

The following code essentially uses the `CoustomExecuteExtensions.ExecuteAsync` from the ConsumerSdk to define the final consumption format through the `HostingQueueExtensions.SubscribeQueue` logic. The effects achieved can be referred to in the `HostingQueueExtensions.SubscribeQueue` logic. Moreover, `CoustomExecuteExtensions.ExecuteAsync` can be extended to include other types of consumer middleware.

```C#
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Custom queue consumer handler class
    /// </summary>
    public class QueueCustomerHandler : QueueHandler
    {
        /// <summary>
        /// Method name
        /// </summary>
        public override Enum ActionKey => MessageType.CUSTOMER_TEST_QUEUE_NAME;

        /// <summary>
        /// Execution method
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            var beginTime = messageBody.TimeStamp.ToDateTime();
            var totalMilliseconds = (DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
            Console.WriteLine($"Topic: {Topic}, Method Name: {ActionKey}, Time Taken: {totalMilliseconds}ms, Parameters: {JsonConvert.SerializeObject(param)}");
            return await Task.FromResult(new ContractResult().SetSuccessful(messageBody.SerialNo));
        }
    }
}
```

## Aspects

!> **Note:** The sequence of `CustomActionAttribute` and `CustomAuthorizeAttribute` is such that `CustomAuthorizeAttribute` takes precedence over `CustomActionAttribute`. These two classes are somewhat similar to filters in WebAPI. For AOP aspect implementation, our custom attributes need to inherit from them and override corresponding methods. Another important point is that derived attributes from `CustomActionAttribute` and `CustomAuthorizeAttribute` can only be placed on classes, methods, and interfaces.

### Application Example

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
    /// Custom queue consumer handler class
    /// </summary>
    public class QueueCustomerHandler : QueueHandler
    {
        /// <summary>
        /// Method name
        /// </summary>
        public override Enum ActionKey => MessageType.CUSTOMER_TEST_QUEUE_NAME;

        /// <summary>
        /// Execution method
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        [TestCustomAction, TestCustomAuthorize]
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            var beginTime = messageBody.TimeStamp.ToDateTime();
            var totalMilliseconds = (DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
            Console.WriteLine($"Topic: {Topic}, Method Name: {ActionKey}, Time Taken: {totalMilliseconds}ms, Parameters: {JsonConvert.SerializeObject(param)}");
            return await Task.FromResult(new ContractResult().SetSuccessful(messageBody.SerialNo));
        }
    }
}
```

### `CustomActionAttribute`

?> Multiple CustomActionAttribute derivations can be defined. When multiple instances exist, the program will execute them per the Order value in ascending sequence.

```C#
using Com.GleekFramework.ConsumerSdk;
using Com.GleekFramework.ContractSdk;

namespace Com.GleekFramework.AppSvc.Attributes
{
    /// <summary>
    /// Test
    /// </summary>
    public class TestCustomActionAttribute : CustomActionAttribute
    {
        /// <summary>
        /// Order for execution (ascending order)
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// Called before method execution
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ContractResult> OnActionExecutingAsync(CustomActionExecutingContext context)
        {
            return base.OnActionExecutingAsync(context);
        }

        /// <summary>
        /// Called after method execution is complete
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

?> Multiple CustomAuthorizeAttribute derivations can be defined. When multiple instances exist, the program will execute them per the Order value in ascending sequence.

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
        /// Order for execution (ascending order)
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// Called before method invocation, earlier than CustomActionAttribute
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
