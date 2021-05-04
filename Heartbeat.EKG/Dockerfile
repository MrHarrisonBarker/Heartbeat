FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src


COPY "Heartbeat.sln" "Heartbeat.sln"

COPY "Heartbeat/Heartbeat.csproj" "Heartbeat/Heartbeat.csproj"
COPY "Heartbeat.EKG/Heartbeat.EKG.csproj" "Heartbeat.EKG/Heartbeat.EKG.csproj"
COPY "MockService/MockService.csproj" "MockService/MockService.csproj"
COPY "AnotherMockService/AnotherMockService.csproj" "AnotherMockService/AnotherMockService.csproj"

COPY "docker-compose.dcproj" "docker-compose.dcproj"
COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "Heartbeat.sln"

COPY . .
WORKDIR /src/Heartbeat.EKG
RUN dotnet publish -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Heartbeat.EKG.dll"]