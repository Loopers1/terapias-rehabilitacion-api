FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Terapias-rehabilitaciones.csproj", "."]
RUN dotnet restore "./Terapias-rehabilitaciones.csproj"
COPY . .
RUN dotnet build "Terapias-rehabilitaciones.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Terapias-rehabilitaciones.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Terapias-rehabilitaciones.dll"]