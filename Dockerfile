# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# archivos de proyecto
COPY AppEstudioContable/AppEstudioContable.csproj ./AppEstudioContable/
COPY ServiciosEC/ServiciosEC.csproj ./ServiciosEC/

# restaurar dependencias
RUN dotnet restore ./AppEstudioContable/AppEstudioContable.csproj
RUN dotnet restore ./ServiciosEC/ServiciosEC.csproj

# copiar el codigo
COPY . .

# compilar y publish
WORKDIR /src/AppEstudioContable
RUN dotnet publish -c Release -o /app/out

# imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80

# para que escuche en el puerto 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "AppEstudioContable.dll"]