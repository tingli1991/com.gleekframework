## 项目

> Com.GleekFramework.NLogSdk

### 概述

NLog 拓展工具包，它主要是利用 NLog 组件，对其进行二次封装，从而规范我们的日志格式和定义了在微服务架构中，日志所应当包含的一些核心字段。

### 依赖

> Com.GleekFramework.AutofacSdk

## NLog.config

这里 NLog.config 原生的配置定义是没有做调整的，约束的仅仅是它输出的字段，当然这里我是配置成了输出到 File(我们也可以配置让其输出到控制台)

!> NLog.config 必须放在启动项目的根目录，并设置它的属性为"始终复制"

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog autoReload="true" xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

  <variable name="projectLogDir" value="logs" />
  <targets async="true">
    <default-target-parameters xsi:type="File" createDirs="true" autoFlush="false" keepFileOpen="true" maxArchiveFiles="3" openFileFlushTimeout="10"
                               openFileCacheTimeout="30" archiveAboveSize="104857600" archiveNumbering="Sequence" concurrentWrites="true" encoding="UTF-8" />

    <target xsi:type="File"
            name="GlobalTargetJson"
            fileName="${projectLogDir}/${level}/${shortdate}.log"
            archiveFileName="${projectLogDir}/${level}/${shortdate}.{#####}.log">

      <layout xsi:type="JsonLayout">
        <attribute name="level" layout="${level}" />
        <attribute name="url" layout="${event-properties:item=Url}"/>
        <attribute name="app_id" layout="Org.Gleek.AuthorizeSvc" />
        <attribute name="content" layout="${event-properties:item=Content}"/>
        <attribute name="service_id" layout="${local-ip:cachedSeconds=3600}" />
        <attribute name="serial_no" layout="${event-properties:item=SerialNo}"/>
        <attribute name="service_time" layout="${event-properties:item=ServiceTime}"/>
        <attribute name="total_seconds" layout="${event-properties:item=TotalSeconds}" encode="false"/>
        <attribute name="content_length" layout="${event-properties:item=ContentLength}" encode="false"/>
        <attribute name="total_milliseconds" layout="${event-properties:item=TotalMilliseconds}" encode="false"/>
      </layout>
    </target>
  </targets>
  <rules>
    <logger name="GlobalJson" levels="Debug,Info,Trace,Warn,Error,Fatal" writeTo="GlobalTargetJson" />
  </rules>
</nlog>
```

## 示例

!> NLog 不需要像其他组件一样进行注入，我们只需要注入他的依赖 Autofac 即可。

```C#
/// <summary>
/// 测试控制器
/// </summary>
[Route("test")]
public class TestController : BaseController
{
    /// <summary>
    /// 日志服务
    /// </summary>
    public NLogService NLogService { get; set; }

    /// <summary>
    /// 测试执行方法
    /// </summary>
    /// <returns></returns>
    [HttpGet("execute")]
    public async Task<ContractResult> ExecuteAsync()
    {
        var number = 1;
        NLogService.Debug($"测试Debug日志，变量：{number}");
        return await Task.FromResult(new ContractResult());
    }
}
```

## 输出

```Json
{ "level": "Debug", "app_id": "Com.GleekFramework.NLogSdk", "content": "测试Debug日志，变量：1", "service_id": "192.168.100.10", "service_time": "2024-04-21 14:14:44", "total_seconds": 0, "content_length": 14, "total_milliseconds": 0 }
```
