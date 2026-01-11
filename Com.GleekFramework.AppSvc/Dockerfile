# 使用ASP.NET运行时镜像
#FROM mcr.microsoft.com/dotnet/aspnet:10.0
FROM 192.168.100.19:1100/public/dotnet/aspnet:10.0
WORKDIR /app
COPY ./bin/publish/ . 
ENTRYPOINT ["dotnet", "Com.GleekFramework.AppSvc.dll"]
