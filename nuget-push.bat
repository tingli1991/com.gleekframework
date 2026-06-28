@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

:: ▸ ANSI 颜色
for /f %%a in ('echo prompt $E ^| cmd') do set "ESC=%%a"
set "C_CYAN=%ESC%[36m"
set "C_GREEN=%ESC%[32m"
set "C_YELLOW=%ESC%[33m"
set "C_RED=%ESC%[31m"
set "C_RESET=%ESC%[0m"

:: 时间戳目录，避免重复冲突
for /f %%i in ('powershell -NoP -C "Get-Date -F yyyyMMddHHmmss"') do set now=%%i

:: NuGet 服务器配置（从环境变量读取）
if "%NUGET_API_KEY%"=="" (
    echo %C_RED%[错误] 环境变量 NUGET_API_KEY 未设置%C_RESET%
    echo %C_YELLOW%  请先设置：set NUGET_API_KEY=your-api-key%C_RESET%
    pause
    exit /b 1
)
if "%NUGET_SOURCE_URI%"=="" (
    echo %C_RED%[错误] 环境变量 NUGET_SOURCE_URI 未设置%C_RESET%
    echo %C_YELLOW%  请先设置：set NUGET_SOURCE_URI=https://your-nuget-server/%C_RESET%
    pause
    exit /b 1
)
set api_key=%NUGET_API_KEY%
set source_api_uri=%NUGET_SOURCE_URI%

:: 脚本所在目录为解决方案根目录
set solution_dir=%~dp0
set packg_root=%solution_dir%nupkgs\%now%

:: 项目列表（基础库在前，上层库在后）
set projects=^
Com.GleekFramework.CommonSdk ^
Com.GleekFramework.AttributeSdk ^
Com.GleekFramework.ObjectSdk ^
Com.GleekFramework.ContractSdk ^
Com.GleekFramework.ConfigSdk ^
Com.GleekFramework.SecuritySdk ^
Com.GleekFramework.AssemblySdk ^
Com.GleekFramework.NLogSdk ^
Com.GleekFramework.DapperSdk ^
Com.GleekFramework.HttpSdk ^
Com.GleekFramework.NacosSdk ^
Com.GleekFramework.AutofacSdk ^
Com.GleekFramework.RedisSdk ^
Com.GleekFramework.MigrationSdk ^
Com.GleekFramework.MongodbSdk ^
Com.GleekFramework.QueueSdk ^
Com.GleekFramework.KafkaSdk ^
Com.GleekFramework.RabbitMQSdk ^
Com.GleekFramework.RocketMQSdk ^
Com.GleekFramework.GrpcSdk ^
Com.GleekFramework.ConsumerSdk ^
Com.GleekFramework.SwaggerSdk

echo %C_CYAN%===============================%C_RESET%
echo %C_CYAN%  GleekFramework NuGet 批量发布%C_RESET%
echo %C_CYAN%  开始时间 %now%%C_RESET%
echo %C_CYAN%===============================%C_RESET%
echo.

:: 第一步：还原依赖
echo %C_CYAN%[1/4] 还原项目依赖%C_RESET%
dotnet restore "%solution_dir%src\com.gleekframework.sln"

:: 第二步：编译(无还原，第一步已完成)
echo.
echo %C_CYAN%[2/4] 编译所有项目（Release）%C_RESET%
dotnet build "%solution_dir%src\com.gleekframework.sln" -c Release --no-restore

echo.
echo %C_CYAN%[3/4] 打包并推送 NuGet%C_RESET%
echo.

for %%p in (%projects%) do (
    set "proj=%%p"

    rem 从csproj提取Version
    set "package_version="
    set "csproj_file=%solution_dir%src\%%p\%%p.csproj"
    findstr /c:"<Version>" "!csproj_file!" > "%temp%\gleek_version.tmp" 2>nul
    for /f "tokens=3 delims=<>" %%a in (%temp%\gleek_version.tmp) do set "package_version=%%a"
    del "%temp%\gleek_version.tmp" 2>nul

    rem 尝试清理远程同版本旧包，确保推送不冲突
    if defined package_version (
        >nul 2>nul dotnet nuget delete %%p !package_version! -k %api_key% -s %source_api_uri% --non-interactive
    )

    rem 打包（--no-build：第二步已完成编译）
    dotnet pack "%solution_dir%src\%%p\%%p.csproj" ^
        -c Release --no-build ^
        -o "%packg_root%\%%p" ^
        -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg >nul
    if errorlevel 1 (
        echo [DEBUG] %C_RED% !proj!  ✗ 打包失败%C_RESET%
    ) else (
        if exist "%packg_root%\%%p\*.nupkg" (
            dotnet nuget push "%packg_root%\%%p\*.nupkg" -k %api_key% -s %source_api_uri% >nul
            if errorlevel 1 (
                echo [DEBUG] %C_RED% !proj!  ✓ 打包成功  ✗ 推送失败%C_RESET%
            ) else (
                echo [DEBUG] %C_GREEN% !proj!  ✓ 打包成功  ✓ 推送完成%C_RESET%
            )
        ) else (
            echo [DEBUG] %C_YELLOW% !proj!  ✓ 打包成功  - 未生成 nupkg，跳过%C_RESET%
        )
    )
)

:: 第四步：清空本地缓存
echo.
echo %C_CYAN%[4/4] 清空本地 NuGet 缓存%C_RESET%
dotnet nuget locals all --clear
echo %C_GREEN%    ✓ 缓存已清理%C_RESET%

:: ── 完成 ──
echo.
echo %C_GREEN%全部完成%C_RESET%
echo 包目录 %packg_root%
pause
