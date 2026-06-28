# CLAUDE.md

## 1. 核心指令

**先阅读代码，再形成判断。** 不预设项目类型、技术栈或目录结构。

修改前必须：
- 阅读目标文件及其直接调用方
- 理解项目现有的 DI、配置、日志、异常处理模式
- 遵循既有模式，仅改动当前任务所需，不做无关"顺手"修改

## 2. 构建与运行

```bash
dotnet restore
dotnet build <SolutionName>.sln          # 优先使用解决方案构建
dotnet build <path-to-project.csproj>    # 单项目构建
dotnet test                              # 若无测试项目，以 build 为最低验证
dotnet run --project <path-to-project.csproj>
```

## 3. 工具使用

- 搜索代码内容 → `Grep`（不用 `grep`/`rg`/`findstr`）
- 搜索文件名 → `Glob`（不用 `find`/`ls`/`dir`）
- 读取文件 → `Read`（不用 `cat`/`head`/`tail` 查看内容）
- 局部修改 → `Edit`；整文件重写 → `Write`
- 运行命令 → `Bash`
- 复杂多步骤任务 → 先用 `EnterPlanMode` 输出方案，用户确认后再实施

## 4. 代码风格

以下仅列出**项目级约定**。标准 C# 规范（`I` 前缀、`Async` 后缀、camelCase/PascalCase 等）直接沿用，无需在此重复。

- 注释语言与现有代码库一致
- 新增类型遵循项目既有的修饰约定（`partial`/`sealed`/`static`）
- Nullable 配置以 `.csproj` 中的 `<Nullable>` 设置为准
- 序列化方式与项目现有模式一致（System.Text.Json / Newtonsoft.Json / protobuf）
- 命名空间遵循项目既有格式；优先复用已有常量、扩展方法、工具类

## 5. 架构约束

**DI**：遵循项目已有的容器和注入方式（构造注入或属性注入）。不引入新容器，不将 DI 服务改为静态访问（项目已有静态回退模式除外）。

**配置**：遵循项目现有的配置层级和绑定模式（`IOptions<T>` / 自定义特性 / 配置中心）。不将环境差异写死。新增配置项同步更新配置类与配置文件。

**数据访问**：遵循项目现有 ORM 和仓储模式。必须参数化查询，禁止拼接 SQL。不改动历史迁移类。

## 6. 单元测试规范

应用于 `Com.GleekFramework.Tests.UnitTest` 项目及后续所有测试项目。

### 6.1 命名规范

- **方法命名**：使用大驼峰命名法（PascalCase），禁止使用下划线连接
  - 正确：`GetSerialNoReturnsNotEmpty`、`SetSuceccfulSuccessIsTrue`
  - 错误：`GetSerialNo_ReturnsNotEmpty`、`SetSuceccful_SuccessIsTrue`
- **类命名**：使用大驼峰命名法，以 `Tests` 结尾
  - 正确：`SnowflakeServiceTests`、`ResultExtensionsTests`
- **文件命名**：与类名一致

### 6.2 注释规范

- **类注释**：每个测试类必须有 `<summary>` XML 注释，说明测试的组件
- **方法注释**：每个测试方法必须有 `<summary>` XML 注释，说明测试场景和预期

### 6.3 DisplayName 规范

- 所有 `[Fact]` 和 `[Theory]` 必须使用 `DisplayName` 参数
- `DisplayName` 使用中文描述，简洁明了
- 格式：动词 + 主语 + 预期结果
  - 正确：`"获取流水号返回不为空"`、`"SetSuceccful后Success为True"`

### 6.4 项目结构

- 按组件分目录：`Tests/{ComponentName}/{TestClass}Tests.cs`
- 每个 SDK 项目的测试放在对应目录下
- 测试类继承 `BaseUnitTest`（提供配置初始化和 DI 容器）

### 6.5 测试原则

- 每个测试方法只验证一个行为
- 优先使用静态/纯函数测试，减少对外部依赖
- 需要 DI 的服务通过 `GetService<T>()` 获取
- 避免测试间相互依赖

## 7. 禁止

- 替换核心基础设施（DI 容器、ORM、日志框架）
- 删除测试或测试断言
- 大规模重构 / 批量重命名
- 无故升级 NuGet 版本或引入新第三方包
- 修改 CI/CD、Docker、k8s 配置
- 修改证书、密钥、生产配置
- 硬编码凭据（历史遗留明文凭据不得效仿）
- 为通过编译而注释关键代码
- 临时调试代码遗留在正式代码中

## 8. 项目类型补充

代理应通过阅读代码确认项目类型，遵循对应规则：

**Web API**：路由/校验/返回码与现有 Controller 一致；业务逻辑不在 Controller 中堆积；若启用 Swagger，接口变更同步文档。

**Worker Service**：透传 CancellationToken；注意幂等性、重试、超时；避免循环中内存泄漏。

**Blazor / Razor / MVC**：前后端契约一致；视图层不承载业务逻辑。

**gRPC**：Proto 契约与 .NET 类型映射一致；拦截器放入项目约定目录。

**类库**：注意向后兼容；公共 API 变更谨慎；新增类型前确认是否已有类似抽象。

## 9. 优先级

用户明确要求 > 现有架构一致 > 可读性 > 最小改动 > 可测试性 > 可维护性 > 性能 > 语法新颖性
