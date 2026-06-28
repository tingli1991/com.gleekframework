## 项目

> Com.GleekFramework.AssemblySdk

### 概述

程序集开发工具包，它主要是给业务输出对程序集和类库的查询能力，目前只涉及到这两块，他的核心实际还是依赖 Com.GleekFramework.CommonSdk 组件的 Providers。

### 依赖

> GleekFramework.AutofacSdk

> Com.GleekFramework.CommonSdk

## 目录介绍

```text
Com.GleekFramework.AssemblySdk/
└── Services/                 -> 服务实现类目录(通过属性注入的方式进行访问，推荐)
    ├── AssemblyService.cs    -> 程序集服务
    ├── DocumentService.cs    -> 文档服务
    ├── LibraryService.cs     -> 编译库服务

```

## 说明

!> 注意：[gleekframework](https://www.gleekframework.com/)所有组件内的 Service 都是基于 IOC 的属性注入方式进行调用

### 调用示例

```C#
using Com.GleekFramework.AutofacSdk;

namespace Com.GleekFramework.KafkaSdk
{
    /// <summary>
    /// Kafka客户端服务
    /// </summary>
    public class KafkaClientService : IBaseAutofac
    {
        /// <summary>
        /// 环境变量服务
        /// </summary>
        public EnvironmentService EnvironmentService { get; set; }

        /// <summary>
        /// Http请求上下文
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }
    }
}
```
