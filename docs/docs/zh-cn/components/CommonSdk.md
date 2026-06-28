## 项目

> Com.GleekFramework.CommonSdk

### 概述

基础开发工具包，它主要用于对项目内的一些常用的基础方法进行二次拓展包装，有利于大家在开发过程中更加的高效、规范以及代码风格统一等。

> 该项目由于 90%以上的类和方法都是基于拓展的方式进行编写，这里则不提供示例代码，统一按照拓展方法的调用方式进行调用即可，具体有哪些拓展方法请参考[目录介绍](#目录介绍)

## 目录介绍

```text
Com.GleekFramework.CommonSdk/
├── Attributes/                           -> 自定义特性目录
│   ├── ActionAttribute.cs                -> 行为特性(用于标识消费者服务的方法名称)
│   ├── CommentAttribute.cs               -> 表备注特性(用于定义表和字段的备注)
│   ├── IndexAttribute.cs                 -> 索引特性(用于定义表索引)
│   ├── NotMapAttribute.cs                -> 表示应将属性或类从映射中排除
│   ├── PrecisionAttribute.cs             -> 精度特性(用于表示表字段的精度)
├── Enums/                                -> 枚举目录
│   ├── VersionCompare.cs ->              -> 版本对比枚举
├── Extensions/                           -> 拓展目录
│   ├── ActivatorExtensions.cs            -> 实例激活拓展类(反射)
│   ├── AssemblyExtensions.cs             -> 针对Type类型的扩展(反射)
│   ├── AttributeExtensions.cs            -> 特性扩展
│   ├── Base64Extensions.cs               -> Base64拓展类
│   ├── ClassAttributeExtensions.cs       -> 类特性拓展类(反射)
│   ├── ConnectionExtensions.cs           -> 连接字符串拓展
│   ├── ConvertExtensions.cs              -> 数据转换方法
│   ├── DataMapperExtensions.cs           -> 数据模型映射拓展类（类似AutoMap，但是比AutoMap性能高，因为他是基于表达式树实现的）
│   ├── DateTimeExtensions.cs             -> 时间拓展类（时间的转换类）
│   ├── DictionaryExtensions.cs           -> 字典类型扩展(主要是取值，例如：属性值和特性值)
│   ├── EnumerableExtensions.cs           -> 集合基础拓展(例如：取值、新增、遍历、内存分页等)
│   ├── EnumExtensions.cs                 -> 枚举拓展类(获取枚举的特性、例如：描述、方法名称[ActionAttribute]等)
│   ├── HeaderExtensions.cs               -> 头部信息拓展类(主要拓展消息的头部信息和Http请求的头部信息)
│   ├── JsonFormatExtensions.cs           -> JSON格式化拓展类(格式化、压缩等)
│   ├── LambdaExtensions.cs               -> Lambda表达式扩展(Lambda表达式合并、and跟or的条件拼接)
│   ├── MatchExtensions.cs                -> Match拓展类(例如：提取数据库名称)
│   ├── Md5Extensions.cs                  -> MD5加密拓展类
│   ├── ObjectExtensions.cs               -> 对象拓展类(例如：获取属性值、设置属性值)
│   ├── PropertyExtensions.cs             -> 属性反射拓展类(反射，主要是对PropertyInfo的优化和拓展，性能比原生更加优越)
│   ├── ProtobufExtensions.cs             -> Protobuf序列化拓展类
│   ├── QueryableExtensions.cs            -> 查询器拓展(主要是对IQueryable<T>对象进行拓展，从而提升代码质量、效率以及编码风格)
│   ├── RadomExtensions.cs                -> 随机数基础拓展(能够降低并发过高时，随机重复率过高的问题)
│   ├── SerializeExtensions.cs            -> 序列化拓展类
│   ├── StringExtensions.cs               -> 字符串基础拓展类(去掉末尾字符串、去掉开始字符串等)
│   ├── VersionExtensions.cs              -> 版本号扩展(主要是版本对比)
├── Hostings/                             -> Host拓展目录
│   ├── CommonHostExtensions.cs           -> 自定义主机拓展类(例如：注册主机启动完成事件、注册主机停止中事件和注册主机停止完成事件)
│   ├── CommonHostingExtensions.cs        -> 主机拓展类(启用信号两开关，所谓的信号量则是用于控制Task的线程数量，主要用于消费端的信号量控制)
├── Mappers/                              -> 对象映射目录
│   ├── DataMapper.cs                     -> 数据模型映射拓展类`DataMapperExtensions`的具体实现(类似AutoMap)
├── Providers/                            -> 拓展类和方法具体实现目录(不推荐直接使用，通常下面的实现在Extensions目录都能找到)
│   ├── ActivatorProvider.cs              -> 实例激活实现类
│   ├── AssemblyProvider.cs               -> 程序集实现类
│   ├── AssemblyTypeProvider.cs           -> 程序集对象类型实现类
│   ├── ClassAttributeProvider.cs         -> 类特性实现类
│   ├── DocumentProvider.cs               -> 程序集的文档实现类
│   ├── FieldAttributeProvider.cs         -> 字段自定义特性实现类
│   ├── FieldProvider.cs                  -> 字段实现类
│   ├── LibraryProvider.cs                -> 编译库实现类
│   ├── PropertyAttributeProvider.cs      -> 属性自定义特性实现类
│   ├── PropertyProvider.cs               -> 反射属性实现类
│   ├── SemaphoreProvider.cs              -> 信号量拓展实现类
└── VerifyExtensions/                     -> 验证拓展目录(例如：非空验证)
    ├── EmptyVerifyExtensions.cs          -> 非空验证拓展类
    ├── IdCardVerifyExtensions.cs         -> 身份证号码验证
    ├── JsonVerifyExtensions.cs           -> JSON验证拓展类
    ├── VerifyExtensions.cs               -> 数据验证基础拓展类(例如：是否是字符串类型、是否是基础类型、检查IP地址的格式是否正确、是否是Bool值等)
```
