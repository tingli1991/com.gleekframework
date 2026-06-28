## Project

> Com.GleekFramework.AssemblySdk

## Overview

The assembly development toolkit, which mainly outputs the query ability for assemblies and class libraries to the business. Currently, it only involves these two parts. Its core actually relies on the Providers of the Com.GleekFramework.CommonSdk component.

## Dependency

GleekFramework.AutofacSdk
Com.GleekFramework.CommonSdk

## Directory Introduction

```text
Com.GleekFramework.AssemblySdk/
└── Services/                 -> Service Implementation Directory (Access via property injection, recommended)
    ├── AssemblyService.cs    -> Assembly Service
    ├── DocumentService.cs    -> Document Service
    ├── LibraryService.cs     -> Compilation Library Service
```

## Explanation

!> Attention: All the Services in the components of gleekframework are invoked using IOC property injection

### Invocation Example

```C#
using Com.GleekFramework.AutofacSdk;

namespace Com.GleekFramework.KafkaSdk
{
    /// <summary>
    /// Kafka Client Service
    /// </summary>
    public class KafkaClientService : IBaseAutofac
    {
        /// <summary>
        /// Environment Variables Service
        /// </summary>
        public EnvironmentService EnvironmentService { get; set; }

        /// <summary>
        /// Http Request Context
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }
    }
}
```
