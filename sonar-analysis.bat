dotnet sonarscanner begin /k:"com.gleekframework" /d:sonar.host.url="http://192.168.100.21:9000"  /d:sonar.login="caeec94237f29d90653bcaddd4fca10bd642b91c"
dotnet build
dotnet sonarscanner end /d:sonar.login="caeec94237f29d90653bcaddd4fca10bd642b91c"

pause
exit