# Structure Recommendation

Here is a project interface that I have personally summarized after years of working on various projects. I hope this will assist you all during your development process and aim to minimize any worries regarding project structure setup. Please note, it's not necessary to build all these directories right from the start according to this recommendation. The idea here is not to prescribe, but to guide. In case you find yourself uncertain about how to set up your project structure, this can serve as a quick reference to find the answers you need.

## Referenced Projects

[GitHub](https://github.com/tingli1991/org.gleek.authorize.git)

[Gitee](https://gitee.com/litingxian1991/org.gleek.authorize.git)

## Directory Structure

```text
org.gleek.authorize/                                                    -> Root directory
├── 00.Org.Gleek.AuthorizeSvc.Contracts/                                -> Contract layer (Virtual Directory, reflected on the solution)
│   ├── Org.Gleek.AuthorizeSvc.Entitys/                                 -> Database ORM entity project directory
│       ├── Commons/                                                    -> Basic entity file storage directory
│       ├── Enums/                                                      -> Enum file storage directory
│       ├── Permissions/                                                -> Permissions related (split by business)
│       ├── Organizations/                                              -> Organizational structure (split by business)
│       ├── Org.Gleek.AuthorizeSvc.Entitys.csproj                       -> Project file
│   ├── Org.Gleek.AuthorizeSvc.Models/                                 -> Business model project directory
│       ├── Commons/                                                    -> Basic entity file storage directory
│       ├── Configs/                                                    -> Configuration file storage directory
│       ├── Constants/                                                  -> Constant configuration storage directory
│       ├── Enums/                                                      -> Enum file storage directory
│       ├── Events/                                                     -> Event production and subscription Data model
│       ├── Handlers/                                                   -> Consumer consuming Data model
│       ├── Models/                                                     -> Current project-defined business model
│       ├── Params/                                                     -> Interface request parameter model directory
│       ├── Requsts/                                                    -> Interface call request parameter directory (calling other Http interfaces)
│       ├── Responses/                                                  -> Interface request parameter directory (calling other Http interfaces)
│       ├── Results/                                                    -> Data model returned by the interface (Some models of the Models can play this model role to simplify complicated procedures)
│       ├── Org.Gleek.AuthorizeSvc.Models.csproj                        -> Project file
│   ├── Org.Gleek.AuthorizeSvc.Upgrations/                              -> Database upgrade script project directory (optional)
│       ├── Scripts/                                                    -> Database sql version storage directory
│       ├── Upgrations/                                                 -> Upgrade program storage directory
│       ├── Org.Gleek.AuthorizeSvc.Upgrations.csproj                    -> Project file
├── 01.Org.Gleek.AuthorizeSvc.Repository/                               -> Data repository layer (Virtual directory, reflected on the solution)
│   ├── Org.Gleek.AuthorizeSvc.Repository/                              -> Data Repository project directory
│       ├── Repositorys/                                                -> Basic query object repository storage
│       ├── Commons/                                                    -> Basic entity file storage directory
│       ├── Permissions/                                                -> Permissions related (split by business)
│       ├── Organizations/                                              -> Organizational structure (split by business)
│       ├── BaseRepository.cs                                           -> Basic repository service
│       ├── Org.Gleek.AuthorizeSvc.Repository.csproj                    -> Project file
├── 02.Org.Gleek.AuthorizeSvc.Service/                                  -> Domain layer (Virtual directory, reflected on the solution)
│   ├── Org.Gleek.AuthorizeSvc.Service/                                 -> Domain service project directory
│       ├── Commons/                                                    -> Basic entity file storage directory
│       ├── Permissions/                                                -> Permissions related (split by business)
│       ├── Organizations/                                              -> Organizational structure (split by business)
│       ├── BaseService.cs                                              -> Basic domain service
│       ├── Org.Gleek.AuthorizeSvc.Service.csproj                       -> Project file
├── 03.Org.Gleek.AuthorizeSvc.AggregateService/                         -> Aggregation layer (Virtual directory, reflected on the solution)
│   ├── Org.Gleek.AuthorizeSvc.AggregateService/                        -> Aggregated service project directory
│       ├── Commons/                                                    -> Basic entity file storage directory
│       ├── Permissions/                                                -> Permissions related (split by business)
│       ├── Organizations/                                              -> Organizational structure (split by business)
│       ├── BaseAggregateService.cs                                     -> Basic aggregate service
│       ├── Org.Gleek.AuthorizeSvc.AggregateService.csproj              -> Project file
├── Org.Gleek.AuthorizeSvc/                                             -> Start-up Project (Define the interface, subscribe and consume)
│   ├── Properties/                                                     -> Project attribute configuration directory
│       ├── launchSettings.json                                         -> Project configuration file (example: environmental variables)
│   ├── Controllers/                                                    -> Controller directory
│       ├── Permissions/                                                -> Permissions related (split by business)
│       ├── Organizations/                                              -> Organizational structure (split by business)
│   ├── Handlers/                                                       -> Consumer Processing Interface Definition Directory
│       ├── Queues/                                                     -> Local Queue Consumption Interface Definition Directory
│           ├── Permissions/                                            -> Permissions related (split by business)
│           ├── Organizations/                                          -> Organizational structure (split by business)
│       ├── Business/                                                   -> Current module's Consumer business
│           ├── Permissions/                                            -> Permissions related (split by business)
│           ├── Organizations/                                          -> Organizational structure (split by business)
│       ├── Events/                                                     -> Subscribed  business consumption of other business modules
│           ├── Permissions/                                            -> Permissions related (split by business)
│           ├── Organizations/                                          -> Organizational structure (split by business)
│   ├── Config/                                                         -> Configuration file directory
│       ├── application.json                                            -> Application configuration file (without environmental variables, optional)
│       ├── application.dev.json                                        -> Application configuration file (development environment, optional)
│       ├── application.test.json                                       -> Application configuration file (testing environment, optional)
│       ├── application.uat.json                                        -> Application configuration file (grey environment, optional)
│       ├── application.pro.json                                        -> Application configuration file (production environment, optional)
│       │
│       ├── bootstrap.json                                              -> Local configuration file (without environmental variables, optional)
│       ├── bootstrap.dev.json                                          -> Local configuration file (development environment, optional)
│       ├── bootstrap.test.json                                         -> Local configuration file (testing environment, optional)
│       ├── bootstrap.uat.json                                          -> Local configuration file (grey environment, optional)
│       ├── bootstrap.pro.json                                          -> Local configuration file (production environment, optional)
│       │
│       ├── nacos.json                                                  -> Nacos subscription configuration file (without environmental variables, optional)
│       ├── nacos.dev.json                                              -> Nacos subscription configuration file (development environment, optional)
│       ├── nacos.test.json                                             -> Nacos subscription configuration file (testing environment, optional)
│       ├── nacos.uat.json                                              -> Nacos subscription configuration file (grey environment, optional)
│       ├── nacos.pro.json                                              -> Nacos subscription configuration file (production environment, optional)
│       │
│       ├── share.json                                                  -> Shared configuration file (without environmental variables, optional)
│       ├── share.dev.json                                              -> Shared configuration file (development environment, optional)
│       ├── share.test.json                                             -> Shared configuration file (testing environment, optional)
│       ├── share.uat.json                                              -> Shared configuration file (grey environment, optional)
│       ├── share.pro.json                                              -> Shared configuration file (production environment, optional)
│       │
│       ├── subscription.json                                           -> Subscription configuration file (without environmental variables, optional)
│       ├── subscription.dev.json                                       -> Subscription configuration file (development environment, optional)
│       ├── subscription.test.json                                      -> Subscription configuration file (testing environment, optional)
│       ├── subscription.uat.json                                       -> Subscription configuration file (grey environment, optional)
│       ├── subscription.pro.json                                       -> Subscription configuration file (production environment, optional)
│   ├── Program.cs                                                      -> Entrance file (define Main function)
│   ├── Startup.cs                                                      -> Program activation class
│   ├── Org.Gleek.AuthorizeSvc.csproj                                   -> Project file
├── .dockerignore                                                       -> Docker ignore file
├── .gitignore                                                          -> Git ignore file
├── NuGet.Config                                                        -> Nuget configuration file
├── org.gleek.authorize.sln                                             -> Solution
├── README.md                                                           -> README.md
```
