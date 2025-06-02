# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar archivos y restaurar dependencias
COPY ["SIPECA.sln", "./"]
COPY ["SIPECA.API/SIPECA.API.csproj", "SIPECA.API/"]
COPY ["SIPECA.Aplicacion/SIPECA.Aplicacion.csproj", "SIPECA.Aplicacion/"]
COPY ["SIPECA.Dominio/SIPECA.Dominio.csproj", "SIPECA.Dominio/"]
RUN dotnet restore "SIPECA.sln"


# Copiar resto de archivos y compilar
COPY . .
WORKDIR "/src/SIPECA.API"
RUN dotnet publish "SIPECA.API.csproj" -c Release -o /app/publish

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LatExcel.Api.dll"]