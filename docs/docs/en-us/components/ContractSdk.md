## Project

> Com.GleekFramework.ContractSdk

## Overview

A public contract extension toolkit, which is mainly used to define common data models (enumerations, cache models, message models, etc.) and the core classes and methods on which the models depend.

## Dependency

> Com.GleekFramework.AutofacSdk

## Directory Introduction

```text
Com.GleekFramework.ContractSdk/
├── Enums/                   -> Enumeration directory
│   ├── GlobalMessageCode.cs -> Global message error code
├── Extensions/              -> Extension directory
│   ├── MessageExtensions.cs -> Message extension class
│   └── ResultExtensions.cs  -> General return model extension class
├── Models/                  -> Data model
│   ├── CacheModel.cs        -> Collection data cache model │
├── Params/                  -> Parameter directory
│   ├── MessageBody.cs/      -> Model of the content of the message
├── Providers/               -> Implementation class directory (accessed through the object method, not recommended)
│   ├── SnowflakeProvider.cs -> Snowflake algorithm implementation class
├── Results/                 -> Return model directory
│   ├── ContractResult.cs    -> Data return contract model (usually used for the outermost return contract of interfaces, services, or domains)
│   ├── PageDataResult.cs    -> Pagination return contract model
└── Services/                -> Service implementation class directory (accessed through attribute injection, recommended)
    ├── SnowflakeService.cs  -> Snowflake algorithm service implementation
```

## Important Contracts

For the examination of more contracts, please refer to [Directory Introduction](#directory-introduction).

### ContractResult

During the project development process, `ContractResult` will serve as an important contract model for representing the results returned by the protocol. It has the following characteristics and advantages:

###### Characteristics:

- It has multiple properties to represent different return result information, including the result of business processing, error status code, business serial number, error message, and timestamp.
- The `ContractResult<T>` class inherits from the `ContractResult` class and adds a data details property Data for storing data of a specific type.

###### Advantages:

- Time stamp recording: Includes time stamp information for easy tracking and analysis of the time sequence of operations.
- Serializable: Ensure that it can be serialized and deserialized for easy transmission and storage in different environments.
- Clear structure and definition: Clearly divide different properties, making the structure of the return result clear and easy to understand and process.
- Error handling: Through the error status code and error message, it can clearly indicate the success or failure of business processing and provide detailed error descriptions.
- Standardized contract: Provides a unified contract model for the return results of interfaces, which helps to ensure consistent understanding and use of return data by all parties.
- Flexible data bearing: Through inheritance and extension, specific types of data details can be added, increasing the flexibility of the model.

###### Source code

```C#
using Com.GleekFramework.CommonSdk;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// Contract return result
    /// </summary>
    [Serializable]
    public partial class ContractResult
    {
        /// <summary>
        /// The result of business processing (true indicates success, false indicates failure)
        /// </summary>
        [JsonProperty("success"), JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Error status code
        /// </summary>
        [JsonProperty("code"), JsonPropertyName("code")]
        public string Code { get; set; } = $"{GlobalMessageCode.FAIL.GetHashCode()}";

        /// <summary>
        /// Business serial number
        /// </summary>
        [JsonProperty("serial_no"), JsonPropertyName("serial_no")]
        public string SerialNo { get; set; } = SnowflakeProvider.GetSerialNo();

        /// <summary>
        /// Error message, and it will return empty when successful
        /// </summary>
        [JsonProperty("message"), JsonPropertyName("message")]
        public string Message { get; set; } = GlobalMessageCode.FAIL.GetDescription();

        /// <summary>
        /// Time stamp
        /// </summary>
        [JsonProperty("timestamp"), JsonPropertyName("timestamp")]
        public long TimeStamp { get; set; } = DateTime.Now.ToCstTime().ToUnixTimeForMilliseconds();
    }

    /// <summary>
    /// Return result contract
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ContractResult<T> : ContractResult
    {
        /// <summary>
        /// Data details
        /// </summary>
        [JsonProperty("data"), JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
```

### MessageBody

`MessageBody` is used to represent the message body in the message queue. It contains some key attributes such as the message type (method name), business serial number, timestamp, and request header information. In addition, there is a generic subclass `MessageBody<T>`, which is used to contain specific data details.

###### Characteristics

- Clear structure: By defining clear attributes, it provides a structured way to describe the message.
- Serializable: It has the ability to be serialized, which is convenient for transmission and storage between different systems.
- Universal design: It is applicable to various types of data details and can be extended through the generic type T.
- Standardized: Using common attribute names and conventions, it is convenient for integration with other systems or components.

###### Advantages

- Improve readability: The clear structure makes the meaning and purpose of the message easier to understand.
- Facilitate processing: It provides a unified interface and structure, which is convenient for the production and consumption ends of the message queue to process.
- Easy to extend: The generic subclass allows handling different types of data details and has good extensibility.
- Enhance interoperability: Based on the standardized design, it can better interact with other systems or components.
- Improve maintainability: The clear definition and structure is beneficial for the maintenance and update of the code.

###### Source Code

```C#
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// Receives the content of the message
    /// </summary>
    [Serializable]
    public partial class MessageBody
    {
        /// <summary>
        /// Message type (method name)
        /// </summary>
        [JsonProperty("action_key"), JsonPropertyName("action_key")]
        public string ActionKey { get; set; }

        /// <summary>
        /// Business serial number
        /// </summary>
        [JsonProperty("serial_no"), JsonPropertyName("serial_no")]
        public string SerialNo { get; set; }

        /// <summary>
        /// Time stamp
        /// </summary>
        [JsonProperty("timestamp"), JsonPropertyName("timestamp")]
        public long TimeStamp { get; set; }

        /// <summary>
        /// Request header information
        /// </summary>
        [JsonProperty("headers"), JsonPropertyName("headers")]
        public Dictionary<string, string> Headers { get; set; }
    }

    /// <summary>
    /// Message content
    /// </summary>
    [Serializable]
    public partial class MessageBody<T> : MessageBody where T : class
    {
        /// <summary>
        /// Data details
        /// </summary>
        [JsonProperty("data"), JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
```

## Important Expansion

For more extensions, please refer to [Directory Introduction](#directory-introduction).

### ResultExtensions

The extended class ResultExtensions mainly extends the return models `ContractResult` and `ContractResult<T>`, which can help us quickly set the return values, and at the same time strictly constrain that our error codes and error messages must be defined within the enum.

```C#
using Com.GleekFramework.CommonSdk;
using System;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// General return model
    /// </summary>
    public static partial class ResultExtensions
    {
        /// <summary>
        /// Check if it is successful
        /// </summary>
        /// <param name="source">Data source</param>
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
        /// Set the error message
        /// </summary>
        /// <param name="source"></param>
        /// <param name="error">Error code</param>
        /// <param name="serialNo">Serial number</param>
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
        /// Set the error message
        /// </summary>
        /// <param name="source"></param>
        /// <param name="error">Error code</param>
        /// <param name="serialNo">Serial number</param>
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
        /// Set the successful return message
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
        /// Set the successful return message
        /// </summary>
        /// <param name="source">Data source</param>
        /// <param name="data"></param>
        /// <param name="serialNo"></param>
        public static ContractResult<T> SetSuceccful<T>(this ContractResult<T> source, T data = default, string serialNo = "")
        {
            source.Success = true;
            if (data!= null)
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

The extended class `MessageExtensions` is mainly used to obtain the Data data content of `MessageBody<T>,` making it convenient for message users to obtain the required attribute values through custom models.

```C#
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// The extended class for message types
    /// </summary>
    public static partial class MessageExtensions
    {
        /// <summary>
        /// Obtain message content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageBody">Message content</param>
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
        /// Obtain message content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageBody">Message content</param>
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

## Important Services

### SnowflakeService

`SnowflakeService` This service I define it as the "snowflake algorithm service", its main function is to help us generate a 32-bit serial number according to the snowflake algorithm. This service relies on `SnowflakeProvider`. All the implementations of the snowflake algorithm are uniformly in the `SnowflakeProvider` (`SnowflakeProvider` is mainly used for non-IOC implementation classes, such as the SerialNo of `ContractResult`).
!> Note: The machine code of the snowflake algorithm here is replaced by a random number from 1-999 for me, the purpose is to reduce complexity, and from a probabilistic point of view, it is indeed possible to repeat, but this logic has been running for several years, and no duplicate cases have been found. If a duplicate is encountered, please contact me.

```C#
using Com.GleekFramework.AutofacSdk;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// Snowflake algorithm implementation class
    /// </summary>
    public partial class SnowflakeService : IBaseAutofac
    {
        /// <summary>
        /// Get the serial number
        /// </summary>
        /// <param name="suffix">Serial number prefix</param>
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
    /// Snowflake algorithm implementation class
    /// </summary>
    public static class SnowflakeProvider
    {
        /// <summary>
        /// Self-increasing starting position
        /// </summary>
        private static long Sequence = 100000000L;

        /// <summary>
        /// Object lock
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// Random factor
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Machine code
        /// </summary>
        private static readonly string PersonCode = $"{Random.NextLong(1, 999)}".PadLeft(3, '0');

        /// <summary>
        /// Time point for re-counting
        /// </summary>
        private static long RefSequence = long.Parse(DateTime.Now.Date.ToCstToday().AddDays(1).ToString("yyyyMMddHHmmssffff"));

        /// <summary>
        /// Get the serial number
        /// </summary>
        /// <param name="suffix">Serial number prefix</param>
        /// <returns></returns>
        public static string GetSerialNo(int suffix = 1)
        {
            var serialNo = NextSnowflakeNo(suffix);
            if (string.IsNullOrWhiteSpace(serialNo))
            {
                throw new ArgumentNullException($"GetSerialNo({suffix}): Failed to obtain the sequence number!!!");
            }
            return serialNo;
        }

        /// <summary>
        /// Generate the snowflake number
        /// </summary>
        /// <param name="suffix">Serial number prefix</param>
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
                Interlocked.Increment(ref Sequence);//Atomic increment
                return $"{currentTimeSpan}{PersonCode}{Sequence}{suffix.ToString().PadLeft(2, '0').Substring(0, 2)}";
            }
        }

        /// <summary>
        /// Generate the current time stamp
        /// </summary>
        /// <returns></returns>
        private static long GetCurrentTimeSpan()
        {
            return long.Parse(DateTime.Now.ToCstTime().ToString("yyyyMMddHHmmssffff"));
        }
    }
}
```
