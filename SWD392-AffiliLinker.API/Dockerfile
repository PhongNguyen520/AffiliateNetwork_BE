FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN ls -la

COPY ["SWD392-AffiliLinker.API/SWD392-AffiliLinker.API.csproj", "SWD392-AffiliLinker.API/"]
COPY ["SWD392-AffiliLinker.Services/SWD392-AffiliLinker.Services.csproj", "SWD392-AffiliLinker.Services/"]
COPY ["SWD392-AffiliLinker.Repositories/SWD392-AffiliLinker.Repositories.csproj", "SWD392-AffiliLinker.Repositories/"]
COPY ["SWD392-AffiliLinker.Core/SWD392-AffiliLinker.Core.csproj", "SWD392-AffiliLinker.Core/"]

RUN ls -la SWD392-AffiliLinker.API/

RUN dotnet restore "./SWD392-AffiliLinker.API/SWD392-AffiliLinker.API.csproj"
COPY . .

RUN ls -la
RUN ls -la SWD392-AffiliLinker.API/

WORKDIR "/src/SWD392-AffiliLinker.API"
RUN dotnet build "./SWD392-AffiliLinker.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SWD392-AffiliLinker.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN ls -la /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN ls -la

ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "SWD392-AffiliLinker.API.dll"]