#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /home/app
COPY bin/Debug/net6.0/publish .

ENTRYPOINT ["dotnet", "Prestadito.Security.API.dll"]