# 结构推荐

以下是我个人多年项目总结出来的一个项目接口，希望能够帮到大家在开发过程中，尽可能的不再为项目结构怎么建立而烦恼，当然这些目录不是说一上来就必须按照这个要求，把所有的目录都先建一遍，不是这样的，这里只是为了引导大家，当不知道怎么建立项目结构的时候，可以从这里快速检索到您想要的答案。

## 参阅项目

[GitHub](https://github.com/tingli1991/org.gleek.authorize.git)

[Gitee](https://gitee.com/litingxian1991/org.gleek.authorize.git)

## 目录结构

```text
org.gleek.authorize/                                                    -> 根目录
├── 00.Org.Gleek.AuthorizeSvc.Contracts/                                -> 契约层(虚拟目录，在解决方案上面体现)
│   ├── Org.Gleek.AuthorizeSvc.Entitys/                                 -> 数据库ORM实体工程目录
│       ├── Commons/                                                    -> 基础表实体文件存放目录
│       ├── Enums/                                                      -> 枚举文件存放目录
│       ├── Permissions/                                                -> 权限相关(按业务拆分)
│       ├── Organizations/                                              -> 组织架构(按业务拆分)
│       ├── Org.Gleek.AuthorizeSvc.Entitys.csproj                       -> 工程文件
│   ├── Org.Gleek.AuthorizeSvc.Models/                                  -> 业务模型工程目录
│       ├── Commons/                                                    -> 基础表实体文件存放目录
│       ├── Configs/                                                    -> 配置文件存放目录
│       ├── Constants/                                                  -> 常量配置存放目录
│       ├── Enums/                                                      -> 枚举文件存放目录
│       ├── Events/                                                     -> 事件生产和订阅的Data模型
│       ├── Handlers/                                                   -> 消费端消费的Data模型
│       ├── Models/                                                     -> 当前项目定义的业务模型
│       ├── Params/                                                     -> 接口的请求参数模型目录
│       ├── Requsts/                                                    -> 接口调用请求参数目录(调用其他的Http接口)
│       ├── Responses/                                                  -> 接口调用请求参数目录(调用其他的Http接口)
│       ├── Results/                                                    -> 接口返回的Data模型(为了简化繁琐的流程，部分Models的模型可以充当该模型)
│       ├── Org.Gleek.AuthorizeSvc.Models.csproj                        -> 工程文件
│   ├── Org.Gleek.AuthorizeSvc.Upgrations/                              -> 数据库升级脚本工程目录(可选)
│       ├── Scripts/                                                    -> 数据库sql叫版本存放目录
│       ├── Upgrations/                                                 -> 升级程序存放目录
│       ├── Org.Gleek.AuthorizeSvc.Upgrations.csproj                    -> 项目文件
├── 01.Org.Gleek.AuthorizeSvc.Repository/                               -> 数据仓储层(虚拟目录，在解决方案上面体现)
│   ├── Org.Gleek.AuthorizeSvc.Repository/                              -> 数据仓储工程目录
│       ├── Repositorys/                                                -> 存放仓储基础查询对象
│       ├── Commons/                                                    -> 基础表实体文件存放目录
│       ├── Permissions/                                                -> 权限相关(按业务拆分)
│       ├── Organizations/                                              -> 组织架构(按业务拆分)
│       ├── BaseRepository.cs                                           -> 基础仓储服务
│       ├── Org.Gleek.AuthorizeSvc.Repository.csproj                    -> 项目文件
├── 02.Org.Gleek.AuthorizeSvc.Service/                                  -> 领域层(虚拟目录，在解决方案上面体现)
│   ├── Org.Gleek.AuthorizeSvc.Service/                                 -> 领域服务工程目录
│       ├── Commons/                                                    -> 基础表实体文件存放目录
│       ├── Permissions/                                                -> 权限相关(按业务拆分)
│       ├── Organizations/                                              -> 组织架构(按业务拆分)
│       ├── BaseService.cs                                              -> 基础领域服务
│       ├── Org.Gleek.AuthorizeSvc.Service.csproj                       -> 项目文件
├── 03.Org.Gleek.AuthorizeSvc.AggregateService/                         -> 聚合层(虚拟目录，在解决方案上面体现)
│   ├── Org.Gleek.AuthorizeSvc.AggregateService/                        -> 聚合服务工程目录
│       ├── Commons/                                                    -> 基础表实体文件存放目录
│       ├── Permissions/                                                -> 权限相关(按业务拆分)
│       ├── Organizations/                                              -> 组织架构(按业务拆分)
│       ├── BaseAggregateService.cs                                     -> 基础聚合服务
│       ├── Org.Gleek.AuthorizeSvc.AggregateService.csproj              -> 项目文件
├── Org.Gleek.AuthorizeSvc/                                             -> 启动项目(定义接口、订阅消费)
│   ├── Properties/                                                     -> 项目属性配置目录
│       ├── launchSettings.json                                         -> 项目配置文件(例如：环境变量)
│   ├── Controllers/                                                    -> 控制器目录
│       ├── Permissions/                                                -> 权限相关(按业务拆分)
│       ├── Organizations/                                              -> 组织架构(按业务拆分)
│   ├── Handlers/                                                       -> 消费者处理接口定义目录
│       ├── Queues/                                                     -> 本地队列消费接口定义目录
│           ├── Permissions/                                            -> 权限相关(按业务拆分)
│           ├── Organizations/                                          -> 组织架构(按业务拆分)
│       ├── Business/                                                   -> 当前模块的消费业务
│           ├── Permissions/                                            -> 权限相关(按业务拆分)
│           ├── Organizations/                                          -> 组织架构(按业务拆分)
│       ├── Events/                                                     -> 订阅其他业务模块的消费业务
│           ├── Permissions/                                            -> 权限相关(按业务拆分)
│           ├── Organizations/                                          -> 组织架构(按业务拆分)
│   ├── Config/                                                         -> 配置文件目录
│       ├── application.json                                            -> 应用程序配置文件(不带环境变量，可选)
│       ├── application.dev.json                                        -> 应用程序配置文件(开发环境，可选)
│       ├── application.test.json                                       -> 应用程序配置文件(测试环境，可选)
│       ├── application.uat.json                                        -> 应用程序配置文件(灰度环境，可选)
│       ├── application.pro.json                                        -> 应用程序配置文件(生产环境，可选)
│       │
│       ├── bootstrap.json                                              -> 本地配置文件(不带环境变量，可选)
│       ├── bootstrap.dev.json                                          -> 本地配置文件(开发环境，可选)
│       ├── bootstrap.test.json                                         -> 本地配置文件(测试环境，可选)
│       ├── bootstrap.uat.json                                          -> 本地配置文件(灰度环境，可选)
│       ├── bootstrap.pro.json                                          -> 本地配置文件(生产环境，可选)
│       │
│       ├── nacos.json                                                  -> Nacos订阅配置文件(不带环境变量，可选)
│       ├── nacos.dev.json                                              -> Nacos订阅配置文件(开发环境，可选)
│       ├── nacos.test.json                                             -> Nacos订阅配置文件(测试环境，可选)
│       ├── nacos.uat.json                                              -> Nacos订阅配置文件(灰度环境，可选)
│       ├── nacos.pro.json                                              -> Nacos订阅配置文件(生产环境，可选)
│       │
│       ├── share.json                                                  -> 共享配置文件(不带环境变量，可选)
│       ├── share.dev.json                                              -> 共享配置文件(开发环境，可选)
│       ├── share.test.json                                             -> 共享配置文件(测试环境，可选)
│       ├── share.uat.json                                              -> 共享配置文件(灰度环境，可选)
│       ├── share.pro.json                                              -> 共享配置文件(生产环境，可选)
│       │
│       ├── subscription.json                                           -> 订阅配置文件(不带环境变量，可选)
│       ├── subscription.dev.json                                       -> 订阅配置文件(开发环境，可选)
│       ├── subscription.test.json                                      -> 订阅配置文件(测试环境，可选)
│       ├── subscription.uat.json                                       -> 订阅配置文件(灰度环境，可选)
│       ├── subscription.pro.json                                       -> 订阅配置文件(生产环境，可选)
│   ├── Program.cs                                                      -> 入口文件(定义Main函数)
│   ├── Startup.cs                                                      -> 程序激动类
│   ├── Org.Gleek.AuthorizeSvc.csproj                                   -> 项目文件
├── .dockerignore                                                       -> docker忽略文件
├── .gitignore                                                          -> docker忽略文件
├── NuGet.Config                                                        -> Nuget配置文件
├── org.gleek.authorize.sln                                             -> 解决方案
├── README.md                                                           -> README.md
```
