#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# COPY ["Cosmos.Publisher/Cosmos.Publisher.csproj", "Cosmos.Publisher/"]
COPY WebApps/Publisher/Cosmos.Publisher Cosmos.Publisher
RUN dotnet restore "Cosmos.Publisher/Cosmos.Publisher.csproj"
WORKDIR "/src/Cosmos.Publisher"
RUN dotnet build "Cosmos.Publisher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cosmos.Publisher.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cosmos.Publisher.dll"]
