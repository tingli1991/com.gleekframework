---
name: gen-docs
description: 自动生成官方文档
---

代码变更后同步更新 `docs/` 中英文文档（Docsify 体系）。

## 核心规则

1. **先读源码** — 打开对应项目理解 API、注入、配置后再写
2. **先中文后英文** — 先完成中文文档，再将内容翻译为英文写入 `en-us/` 镜像路径。代码注释随语言翻译，类名/方法名/NuGet 包名/命名空间/程序集名称保持原文
3. **新文档同步侧边栏** — 更新 `_sidebar.md`
4. **写完后校验** — 中英文文件路径镜像、数量一致

## 目录结构

```
docs/
├── index.html              # Docsify 配置（只读）
├── _coverpage.md           # 首页封面（一般不改）
├── docs/
│   ├── _404.md
│   ├── en-us/
│   │   ├── README.md
│   │   ├── _sidebar.md
│   │   ├── quickstart.md
│   │   ├── recommended.md
│   │   └── components/
│   └── zh-cn/
│       ├── README.md
│       ├── _sidebar.md
│       ├── quickstart.md
│       ├── recommended.md
│       └── components/
```

## 文档模板

每个组件文档固定 6 个章节，按以下顺序：

```
> {程序集名称}
- 描述：{一句话说明这个组件做什么}
- 用途：{在什么场景下使用}

## 依赖

> {依赖的程序集名称}

所有名称一律使用程序集名称，即 `src/` 下对应项目的 `.csproj` 文件名。没有依赖则跳过本节。

## 概述

一段话讲清楚"是什么 + 能做什么"，然后列出核心功能点。

## 注入

!> 使用该组件之前必须先通过注入来激活它

```C#
using {命名空间};
// ...
private static IHostBuilder CreateDefaultHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .Use{Name}()
    .UseGleekWebHostDefaults<Startup>();
```

`Use{Name}()` 来自 `Hostings/` 目录的扩展方法。如需配置项，在代码前贴出 `appsettings.json` 片段。纯工具类组件（无需注入）去掉本节，在概述中说明即可。

## 基本使用

按功能点拆分子章节，每节：标题 + 一句话说明 + 代码示例。

### {功能点名称}

{一句话说明}

```C#
// 注释说明用途
```

## 示例

提供一个完整的可运行示例，覆盖注入到使用的核心路径，让读者可以直接复制运行。

如果组件同时存在 Service 和 Provider，优先使用 Service 编写示例，Provider 为底层实现，不作为对外示例入口。

```C#
// 完整示例代码
```
```

## Docsify 特有语法

| 语法 | 用途 |
|------|------|
| ` ```C# ` | C# 代码块（大写 C，非 `csharp`） |
| `!>` | 重要警告 |
| `?>` | 参考提示 |
| `[text](path)` | 内部链接，用绝对路径 `/docs/{lang}/...` |

## 侧边栏

分组固定顺序：**入门 → 核心组件 → 基础组件 → 应用组件**。

新文档插入对应分组末尾：

```markdown
- {分组名}
  - [{显示名}](/docs/{lang}/components/{文件名}.md)
```

中文侧边栏显示名用中文（如 `Config 组件`），英文侧边栏翻译为英文（如 `Config Component`）。

## 操作流程

1. 读源码 — 对应项目下 `Hostings/`（注入）、`Options/`（配置）、`Services/`（接口）
2. 按模板写中文文档 → `docs/docs/zh-cn/components/{文件名}.md`
3. 将中文文档完整翻译为英文 → `docs/docs/en-us/components/{文件名}.md`
4. 更新两侧 `_sidebar.md`
5. 校验：代码块 `C#` 非 `csharp`、注释语言匹配、链接绝对路径、标识符未翻译
