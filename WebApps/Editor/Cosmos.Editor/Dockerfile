#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
# COPY ["nuget.config", "."]
COPY ["/home/runner/work/Cosmos/Cosmos/WebApps/Editor/Cosmos.Editor/Cosmos.Editor.csproj", "Cosmos.Editor/Cosmos.Editor.csproj"]
COPY /home/runner/work/Cosmos/Cosmos/WebApps/Editor/Cosmos.Editor Cosmos.Editor
RUN dotnet restore "Cosmos.Editor/Cosmos.Editor.csproj"

WORKDIR "/src/Cosmos.Editor"
RUN dotnet build "Cosmos.Editor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cosmos.Editor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cosmos.Editor.dll"]