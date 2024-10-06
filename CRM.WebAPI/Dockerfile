FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CRM.WebAPI/CRM.WebAPI.csproj", "CRM.WebAPI/"]
COPY ["CRM.Domain/CRM.Domain.csproj", "CRM.Domain/"]
COPY ["CRM.Infrastructure/CRM.Infrastructure.csproj", "CRM.Infrastructure/"]
COPY ["CRM.Core/CRM.Core.csproj", "CRM.Core/"]
RUN dotnet restore "CRM.WebAPI/CRM.WebAPI.csproj"
COPY . .
WORKDIR "/src/CRM.WebAPI"
RUN dotnet build "CRM.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CRM.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRM.WebAPI.dll"]
