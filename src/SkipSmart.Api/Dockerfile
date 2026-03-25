FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5050
EXPOSE 5051

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SkipSmart.Api/SkipSmart.Api.csproj", "SkipSmart.Api/"]
RUN dotnet restore "SkipSmart.Api/SkipSmart.Api.csproj"
COPY . .
WORKDIR "/src/SkipSmart.Api"
RUN dotnet build "SkipSmart.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SkipSmart.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkipSmart.Api.dll"]
