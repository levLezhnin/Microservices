FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/UserApi
COPY UserApi.sln ./
COPY Dal/*.csproj ./Dal/
COPY Logic/*.csproj ./Logic/
COPY Api/*.csproj ./Api/

RUN dotnet restore
COPY . .
WORKDIR /src/Api
RUN dotnet build -c Release -o /app

WORKDIR /src/Logic
RUN dotnet build -c Release -o /app

WORKDIR /src/Dal
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "U.dll"]