@echo off
if %time:~0,2% LEQ 9 (set now=%date:~0,4%%date:~5,2%%date:~8,2%0%time:~1,1%%time:~3,2%%time:~6,2%) else (set now=%date:~0,4%%date:~5,2%%date:~8,2%%time:~0,2%%time:~3,2%%time:~6,2%)

:: 指定上传的工程名称
set project_name=Com.GleekFramework.NLogSdk

:: 指定上传的api key
set api_key=278466c7-23cc-3ec8-86d8-43adde285742

:: 指定上传的url
set source_api_uri=http://192.168.100.15:8081/repository/nuget-hosted/index.json
 
:: 获取当前文件夹
set current_dir=%~dp0%

:: 项目路径(解决方案路径)
set solution_dir=%current_dir%..\

:: 设置当前工程文件的全名称(包含路径)
set csproj_path=%solution_dir%%project_name%\%project_name%.csproj

:: 指定packg目录
set packg_dir=%solution_dir%nupkgs\%project_name%\%now%

:: 编译项目输出pack包
echo start build and pack %project_name% ...
dotnet pack %csproj_path%  -c Release -o %packg_dir% -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg

:: 批量推送包
echo start push %packg_name% ...
dotnet nuget push  %packg_dir%\*.nupkg  -k %api_key% -s %source_api_uri%
echo push %packg_name% finish ...
pause