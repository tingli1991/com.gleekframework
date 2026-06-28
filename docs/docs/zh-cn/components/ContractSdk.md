## 项目

> Com.GleekFramework.ContractSdk

### 概述

公共契约拓展工具包，它主要用于定义通用的数据模型（枚举、缓存模型、消息模型等）以及模型所依赖的核心类跟方法

### 依赖

> Com.GleekFramework.AutofacSdk

## 目录介绍

```text
Com.GleekFramework.ContractSdk/
├── Enums/                   -> 枚举目录
│   ├── GlobalMessageCode.cs -> 全局消息错误码
├── Extensions/              -> 拓展目录
│   ├── MessageExtensions.cs -> 消息拓展类
│   └── ResultExtensions.cs  -> 通用返回模型拓展类
├── Models/                  -> 数据模型
│   ├── CacheModel.cs        -> 集合数据缓存模型│
├── Params/                  -> 参数目录
│   ├── MessageBody.cs/      -> 消息的内容参数模型
├── Providers/               -> 实现类目录(通过对象的方式进行访问，不推荐)
│   ├── SnowflakeProvider.cs -> 雪花算法实现类
├── Results/                 -> 返回模型目录
│   ├── ContractResult.cs    -> 数据返回契约模型(通常用于接口、服务或者领域的最外层返回契约)
│   ├── PageDataResult.cs    -> 分页返回契约模型
└── Services/                -> 服务实现类目录(通过属性注入的方式进行访问，推荐)
    ├── SnowflakeService.cs  -> 雪花算法服务实现
```

## 重要契约

查阅更多的契约请参见[目录介绍](#目录介绍)

### ContractResult

项目开发过程中，`ContractResult`将作为一个重要的契约模型，用于表示协议返回的结果。它具有以下几个特点和优势：

###### 特点

- 具有多个属性来表示不同的返回结果信息，包括业务处理结果、错误状态码、业务流水号、错误信息和时间戳。
- `ContractResult<T>` 类继承自 `ContractResult` 类，并添加了一个数据详情属性 Data，用于存储特定类型的数据。

###### 优势

- 时间戳记录：包含时间戳信息，便于追踪和分析操作的时间顺序。
- 可序列化：确保可以进行序列化和反序列化，方便在不同环境中传输和存储。
- 清晰的结构和定义：明确划分了不同的属性，使得返回结果的结构清晰，易于理解和处理。
- 错误处理：通过错误状态码和错误信息，能够明确指示业务处理的成功或失败情况，并提供详细的错误描述。
- 标准化的契约：为接口的返回结果提供了统一的契约模型，有助于确保各方对返回数据的理解和使用一致。
- 灵活的数据承载：通过继承和扩展，可以根据具体需求添加特定类型的数据详情，增加了模型的灵活性。

###### 源码

```C#
using Com.GleekFramework.CommonSdk;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 协议返回结果
    /// </summary>
    [Serializable]
    public partial class ContractResult
    {
        /// <summary>
        /// 业务处理结果(true 表示成功  false 表示失败)
        /// </summary>
        [JsonProperty("success"), JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// 错误状态码
        /// </summary>
        [JsonProperty("code"), JsonPropertyName("code")]
        public string Code { get; set; } = $"{GlobalMessageCode.FAIL.GetHashCode()}";

        /// <summary>
        /// 业务流水号
        /// </summary>
        [JsonProperty("serial_no"), JsonPropertyName("serial_no")]
        public string SerialNo { get; set; } = SnowflakeProvider.GetSerialNo();

        /// <summary>
        /// 错误信息，成功将返回空
        /// </summary>
        [JsonProperty("message"), JsonPropertyName("message")]
        public string Message { get; set; } = GlobalMessageCode.FAIL.GetDescription();

        /// <summary>
        /// 时间戳
        /// </summary>
        [JsonProperty("timestamp"), JsonPropertyName("timestamp")]
        public long TimeStamp { get; set; } = DateTime.Now.ToCstTime().ToUnixTimeForMilliseconds();
    }

    /// <summary>
    /// 返回结果契约
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ContractResult<T> : ContractResult
    {
        /// <summary>
        /// 数据详情
        /// </summary>
        [JsonProperty("data"), JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
```

### MessageBody

`MessageBody`用于表示消息队列中的消息体。它包含了一些关键的属性，如消息类型（方法名称）、业务流水号、时间戳和请求头信息。此外，还有一个泛型子类 `MessageBody<T>`，用于包含具体的数据详情。

###### 特点

- 清晰的结构：通过定义明确的属性，提供了一种结构化的方式来描述消息。
- 可序列化：具备序列化能力，便于在不同系统之间进行传输和存储。
- 通用设计：适用于各种类型的数据详情，通过泛型 T 进行扩展。
- 标准化：使用了常见的属性名称和约定，便于与其他系统或组件进行集成。

###### 优势

- 提高可读性：结构清晰，使得消息的含义和用途更容易理解。
- 便于处理：提供了统一的接口和结构，方便消息队列的生产和消费端进行处理。
- 易于扩展：泛型子类允许处理不同类型的数据详情，具有很好的扩展性。
- 增强互操作性：基于标准化的设计，能够更好地与其他系统或组件进行交互。
- 提高维护性：清晰的定义和结构有利于代码的维护和更新。

###### 源码

```C#
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 接受消息的内容
    /// </summary>
    [Serializable]
    public partial class MessageBody
    {
        /// <summary>
        /// 消息类型(方法名称)
        /// </summary>
        [JsonProperty("action_key"), JsonPropertyName("action_key")]
        public string ActionKey { get; set; }

        /// <summary>
        /// 业务流水号
        /// </summary>
        [JsonProperty("serial_no"), JsonPropertyName("serial_no")]
        public string SerialNo { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [JsonProperty("timestamp"), JsonPropertyName("timestamp")]
        public long TimeStamp { get; set; }

        /// <summary>
        /// 请求头信息
        /// </summary>
        [JsonProperty("headers"), JsonPropertyName("headers")]
        public Dictionary<string, string> Headers { get; set; }
    }

    /// <summary>
    /// 消息内容
    /// </summary>
    [Serializable]
    public partial class MessageBody<T> : MessageBody where T : class
    {
        /// <summary>
        /// 数据详情
        /// </summary>
        [JsonProperty("data"), JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
```

## 重要拓展

更多拓展请参考[目录介绍](#目录介绍)

### ResultExtensions

拓展类`ResultExtensions`，主要是对返回模型`ContractResult`和`ContractResult<T>`的拓展，它能帮助我们快速设置返回值，同时严格约束我们错误码和错误消息的定义必须在枚举内完成

```C#
using Com.GleekFramework.CommonSdk;
using System;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 通用返回模型
    /// </summary>
    public static partial class ResultExtensions
    {
        /// <summary>
        /// 检查是否成功
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static bool IsSuceccful(this ContractResult source)
        {
            if (source == null)
            {
                return false;
            }
            return source.Success;
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="error">错误码</param>
        /// <param name="serialNo">流水号</param>
        public static ContractResult SetError(this ContractResult source, Enum error, string serialNo = "")
        {
            source.Success = false;
            source.Code = error.GetDescription();
            source.Message = $"{error.GetHashCode()}";
            if (!string.IsNullOrWhiteSpace(serialNo))
            {
                source.SerialNo = serialNo;
            }
            return source;
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="error">错误码</param>
        /// <param name="serialNo">流水号</param>
        public static ContractResult<T> SetError<T>(this ContractResult<T> source, Enum error, string serialNo = "")
        {
            source.Success = false;
            source.Code = error.GetDescription();
            source.Message = $"{error.GetHashCode()}";
            if (!string.IsNullOrWhiteSpace(serialNo))
            {
                source.SerialNo = serialNo;
            }
            return source;
        }

        /// <summary>
        /// 设置成功返回信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static ContractResult SetSuceccful(this ContractResult source, string serialNo = "")
        {
            source.Success = true;
            if (!string.IsNullOrWhiteSpace(serialNo))
            {
                source.SerialNo = serialNo;
            }
            source.Message = GlobalMessageCode.SUCCESS.GetDescription();
            source.Code = GlobalMessageCode.SUCCESS.GetHashCode().ToString();
            return source;
        }

        /// <summary>
        /// 设置成功返回信息
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="data"></param>
        /// <param name="serialNo"></param>
        public static ContractResult<T> SetSuceccful<T>(this ContractResult<T> source, T data = default, string serialNo = "")
        {
            source.Success = true;
            if (data != null)
            {
                source.Data = data;
            }

            if (!string.IsNullOrWhiteSpace(serialNo))
            {
                source.SerialNo = serialNo;
            }

            source.Code = $"{GlobalMessageCode.SUCCESS.GetHashCode()}";
            source.Message = GlobalMessageCode.SUCCESS.GetDescription();
            return source;
        }
    }
}
```

### MessageExtensions

拓展类`MessageExtensions` 主要用于获取`MessageBody<T>`的`Data`数据内容，方便消息使用者通过自定义模型获取到所需的属性值。

```C#
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 消息类型拓展类
    /// </summary>
    public static partial class MessageExtensions
    {
        /// <summary>
        /// 获取消息内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        public static T GetData<T>(this MessageBody messageBody)
            where T : class
        {
            T result = default;
            if (messageBody is MessageBody<object> messageValue)
            {
                if (messageValue?.Data == null)
                {
                    return result;
                }

                var jsonValue = messageValue.Data.ToString();
                result = JsonConvert.DeserializeObject<T>(jsonValue);
            }
            else if (messageBody is MessageBody<T> messageValues)
            {
                result = messageValues.Data;
            }
            else
            {
                throw new InvalidOperationException(nameof(messageBody));
            }
            return result;
        }

        /// <summary>
        /// 获取消息内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        public static async Task<T> GetDataAsync<T>(this MessageBody messageBody)
            where T : class
        {
            var data = messageBody.GetData<T>();
            return await Task.FromResult(data);
        }
    }
}
```

## 重要服务

### SnowflakeService

`SnowflakeService` 该服务我把它定义为"雪花算法服务"，它主要的作用就是帮助我们按照雪花算法给我们生成长度为 32 为的流水号，该服务依赖于`SnowflakeProvider`，所有雪花算法的实现统一都在`SnowflakeProvider`里(`SnowflakeProvider`主要用于非 IOC 实现类，例如：`ContractResult`的`SerialNo`)

!>注意：雪花算法的机器码这里是被我用`1-999`的随机数替代了，目的是减少复杂度，同时从概率学的角度来说它确实有可能重复，但结合 机器码+时间(yyyyMMddHHmmssffff)+自增数，几乎不太可能，这个逻辑我们跑了好几年，没有发现过重复的情况，如果遇到重复，请联系我。

```C#
using Com.GleekFramework.AutofacSdk;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 雪花算法实现类
    /// </summary>
    public partial class SnowflakeService : IBaseAutofac
    {
        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <param name="suffix">流水号前缀</param>
        /// <returns></returns>
        public string GetSerialNo(int suffix = 100)
        {
            return SnowflakeProvider.GetSerialNo(suffix);
        }
    }
}
```

```C#
using Com.GleekFramework.CommonSdk;
using System;
using System.Threading;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 雪花算法实现类
    /// </summary>
    public static class SnowflakeProvider
    {
        /// <summary>
        /// 自增开始位置
        /// </summary>
        private static long Sequence = 100000000L;

        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 随机因子
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// 机器码
        /// </summary>
        private static readonly string PersonCode = $"{Random.NextLong(1, 999)}".PadLeft(3, '0');

        /// <summary>
        /// 重新计数的时间点
        /// </summary>
        private static long RefSequence = long.Parse(DateTime.Now.Date.ToCstToday().AddDays(1).ToString("yyyyMMddHHmmssffff"));

        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <param name="suffix">流水号前缀</param>
        /// <returns></returns>
        public static string GetSerialNo(int suffix = 1)
        {
            var serialNo = NextSnowflakeNo(suffix);
            if (string.IsNullOrWhiteSpace(serialNo))
            {
                throw new ArgumentNullException($"GetSerialNo({suffix}): 获取序列号失败!!!");
            }
            return serialNo;
        }

        /// <summary>
        /// 生成雪花编号
        /// </summary>
        /// <param name="suffix">流水号前缀</param>
        /// <returns></returns>
        private static string NextSnowflakeNo(int suffix)
        {
            lock (@lock)
            {
                var currentTimeSpan = GetCurrentTimeSpan();
                if (currentTimeSpan >= RefSequence)
                {
                    Sequence = 100000000L;
                    RefSequence = long.Parse(DateTime.Now.Date.ToCstToday().AddDays(1).ToString("yyyyMMddHHmmssffff"));
                }
                Interlocked.Increment(ref Sequence);//原子递增
                return $"{currentTimeSpan}{PersonCode}{Sequence}{suffix.ToString().PadLeft(2, '0').Substring(0, 2)}";
            }
        }

        /// <summary>
        /// 生成当前时间戳
        /// </summary>
        /// <returns></returns>
        private static long GetCurrentTimeSpan()
        {
            return long.Parse(DateTime.Now.ToCstTime().ToString("yyyyMMddHHmmssffff"));
        }
    }
}
```
