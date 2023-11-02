## com.gleekframework
该框架是一个高性能的组件集合，适用于承载大流量网站。采用分布式架构和微服务的设计理念，经历过单日40亿PV流量的考验。该框架遵循极简的原则，以约定大于配置的规则为基础，帮助开发人员快速构建稳定可靠的系统。 无论是大型企业级项目还是小型应用，该框架都能满足各种需求，提供高效的开发和部署方案，同时代码风格也及其简单且统一。  

## 阅读建议
在每个项目的根目录都会有对用的MD文档，具体的代码使用示例、规范以及注意事项等，我都会在对应的项目目录会采用图文+代码示例的方式给大家进行详细讲解，也希望能给大家带来更高效和简洁的开发模式，同时也希望大家多给点建议(在下感激不尽)。

## 项目概览
|           项目名称                 |           项目描述                |              项目用途                                                                  |
|:-----------------------------------|:----------------------------------|:---------------------------------------------------------------------------------------|
| Com.GleekFramework.AssemblySdk     | 程序集开发工具包                  |管理和扫码程序集                                                                        |
| Com.GleekFramework.AttributeSdk    | 特性工具包                        |例如WebAPI的全局异常捕获、接口请求参数模型验证、接口切面日志以及基础控制器的定义等      |
| Com.GleekFramework.AuthorizeSdk    | 认证授权开发工具包                |主要用于做认证授权相关的功能拓展和规范 (待完善)                                         |
| Com.GleekFramework.AutofacSdk      | IOC(Autofac)开发工具包            |利用Autofac重写NET自带的IOC容器(所有的DI统一采用属性注入的方式)                         |
| Com.GleekFramework.CommonSdk       | 基础的开发工具包                  |约定和定义通用的特性、常量、枚举、拓展方法、数据转换方法以及验证方法等                  |
| Com.GleekFramework.AssemblySdk     | 配置文件拓展工具包                |管理和约定配置的定义以及读取方式                                                        |
| Com.GleekFramework.ConsumerSdk     | 消费者开发工具包                  |管理和约定消费的的实现规范以及自定义了一套基于消费者的消费者的切面开发规范和切面开发能力|
| Com.GleekFramework.ContractSdk     | 公共契约拓展工具包                |定义和约束项目暴露的契约数据(例如：接口的返回模型、项目的基础错误码以及流水号生成规范等)|
| Com.GleekFramework.DapperSdk       | ORM(Dapper)拓展工具包             |基于Dapper和DapperExtensions整合出来的ORM组件                                           |
| Com.GleekFramework.ElasticsearchSdk| ES拓展工具包                      |基于ES的CRUD等相关共嗯的约束和拓展 (待完善)                                             |
| Com.GleekFramework.GrpcSdk         | Grpc拓展工具包                    |基于Grpc的注入以及客户端的实现(待完善)                                                  |
| Com.GleekFramework.HttpSdk         | Http拓展工具包                    |HTTP请求的包装、支持重试、熔断等功能                                                    |
| Com.GleekFramework.KafkaSdk        | Kafka拓展工具包                   |用于约束生产和消费的编码规范、优雅停机以及AOP等能力                                     |
| Com.GleekFramework.MigrationSdk    | 版本迁移拓展工具包                |主要用于在进程内实现自动升级数据库架构、数据迁移、初始化脚本执行等                      |
| Com.GleekFramework.MongodbSdk      | Mongodb拓展工具包                 |Mongodb的数据库操作工具包                                                               |
| Com.GleekFramework.MqttSdk         | Mqtt拓展工具包                    |点对点的消息发送(带完善)                                                                |
| Com.GleekFramework.NacosSdk        | Nacos拓展工具包                   |配置中心、注册中心、服务注册与发现(服务发现目前只实现了http的通信)                      |
| Com.GleekFramework.NLogSdk         | NLog拓展工具包                    |基于NLog实现的日志输出框架                                                              |
| Com.GleekFramework.ObjectSdk       | 对象存储开发工具包                |阿里云、腾讯云的对象存储上传和下载(目前只实现了阿里云OSS)                               |
| Com.GleekFramework.OpenSdk         | OpenAPI拓展工具包                 |调用网关的统一封装(待完善)                                                              |
| Com.GleekFramework.QueueSdk        | 本地队列展工具包                  |先进显出的队列和先进后出的栈的功能封装                                                  |
| Com.GleekFramework.RabbitMQSdk     | RabbitMQ拓展工具包                |工作模式、发布订阅模式、RPC模式以及延迟队列(延迟还在开发)                               |
| Com.GleekFramework.RedisSdk        | Redis拓展工具包                   |基础类型的封装以及强大的分布式锁的拓展                                                  |
| Com.GleekFramework.RocketMQSdk     | RocketMQ拓展工具包                |目前只实现了普通消息和延迟消息的生产和消费(基本够用)                                    |
| Com.GleekFramework.SecuritySdk     | 加密/解密拓展工具包               |主要用于一些数据安全方面的加解密算法(待完善)                                            |
| Com.GleekFramework.SwaggerSdk      | Swagger文档拓展工具包             |利用Knife4jUI对旧的SWAGGER文档主题进行重新渲染                                          |