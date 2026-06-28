## Project

> Com.GleekFramework.NLogSdk

### Overview

An NLog extension toolkit, it primarily utilizes NLog components, re-encapsulating them to standardize our log format and define some core fields that the logs should contain in a microservices architecture.

### Dependencies

> Com.GleekFramework.AutofacSdk

## NLog.config

The native configuration definition of NLog.config here has not been adjusted, what is constrained is only its output fields, of course, here it is configured to output to File (we can also configure it to output to the console).

!> NLog.config must be placed in the root directory of the startup project and set its property to "Always Copy"

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

## Example

!> NLog does not need to be injected like other components, we just need to inject its dependency Autofac.

```C#
/// <summary>
/// Test controller
/// </summary>
[Route("test")]
public class TestController : BaseController
{
    /// <summary>
    /// Log service
    /// </summary>
    public NLogService NLogService { get; set; }

    /// <summary>
    /// Test execution method
    /// </summary>
    /// <returns></returns>
    [HttpGet("execute")]
    public async Task<ContractResult> ExecuteAsync()
    {
        var number = 1;
        NLogService.Debug($"Test Debug log, variable: {number}");
        return await Task.FromResult(new ContractResult());
    }
}
```

## Output

```text
{ "level": "Debug", "app_id": "Com.GleekFramework.NLogSdk", "content": "Test Debug log, variable: 1", "service_id": "192.168.100.10", "service_time": "2024-04-21 14:14:44", "total_seconds": 0, "content_length": 14, "total_milliseconds": 0 }
```
