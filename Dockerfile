# Imagen base de .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY . .

# Restaurar dependencias
RUN dotnet restore BackEndAdministradorCitas/BackEndAdministradorCitas.csproj

# Compilar y publicar
RUN dotnet publish BackEndAdministradorCitas/BackEndAdministradorCitas.csproj -c Release -o /app/publish

# Imagen base para producción (runtime solamente)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/publish .

# Exponer puerto (modifica si usas otro)
EXPOSE 80

# Comando para iniciar la app
ENTRYPOINT ["dotnet", "BackEndAdministradorCitas.dll"]
