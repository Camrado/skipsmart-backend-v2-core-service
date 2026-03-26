FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5050
EXPOSE 5051

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/SkipSmart.Api/SkipSmart.Api.csproj", "src/SkipSmart.Api/"]
COPY ["src/SkipSmart.Application/SkipSmart.Application.csproj", "src/SkipSmart.Application/"]
COPY ["src/SkipSmart.Domain/SkipSmart.Domain.csproj", "src/SkipSmart.Domain/"]
COPY ["src/SkipSmart.Infrastructure/SkipSmart.Infrastructure.csproj", "src/SkipSmart.Infrastructure/"]
RUN dotnet restore "src/SkipSmart.Api/SkipSmart.Api.csproj"
COPY . .
WORKDIR "/src/src/SkipSmart.Api"
RUN dotnet build "SkipSmart.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SkipSmart.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkipSmart.Api.dll"]
`