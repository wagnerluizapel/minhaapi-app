# Imagem base leve (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080

# Imagem de build leve
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copia tudo primeiro (Alpine precisa disso para restaurar corretamente)
COPY . .

# Restaura dependências
RUN dotnet restore

# Publica em modo Release
RUN dotnet publish MinhaApi.csproj -c Release -o /app/publish

# Imagem final
FROM base AS final
WORKDIR /app
RUN mkdir -p /app/logs
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MinhaApi.dll"]
