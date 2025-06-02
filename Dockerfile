# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar archivos y restaurar dependencias
COPY ["SIPECA.sln", "./"]
COPY ["SIPECA.API/SIPECA.API.csproj", "SIPECA.API/"]
COPY ["SIPECA.Aplicacion/SIPECA.Aplicacion.csproj", "SIPECA.Aplicacion/"]
COPY ["SIPECA.Dominio/SIPECA.Dominio.csproj", "SIPECA.Dominio/"]




RUN dotnet restore "./SIPECA.API/SIPECA.API.csproj"
COPY . .
WORKDIR "/src/SIPECA.API"
RUN dotnet build "./SIPECA.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SIPECA.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SIPECA.API.dll"]