FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

RUN groupadd --system appgroup \
 && useradd  --system --no-create-home --gid appgroup --shell /usr/sbin/nologin appuser

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_ENVIRONMENT=Production \
    Jwt__Key=uqW8EXYt+3WOsDntgbG5Jt68rNTMmKZwpawNRcMIkSY=

EXPOSE 8080

USER appuser

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["backend.csproj", "./"]
RUN dotnet restore "./backend.csproj"

COPY . .
RUN dotnet build "./backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "backend.dll"]
