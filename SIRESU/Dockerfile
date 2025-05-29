# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY *.sln .
COPY SIRESU/*.csproj ./SIRESU/
RUN dotnet restore

COPY SIRESU/. ./SIRESU/
WORKDIR /src/SIRESU

RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "SIRESU.dll"]
