# Runtime Base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# SDK Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiando arquivos de projeto
COPY ["MovimentosManual.Api/MovimentosManual.Api.csproj", "MovimentosManual.Api/"]
COPY ["MovimentosManual.Application/MovimentosManual.Application.csproj", "MovimentosManual.Application/"]
COPY ["MovimentosManual.Domain/MovimentosManual.Domain.csproj", "MovimentosManual.Domain/"]
COPY ["MovimentosManual.Infrastructure/MovimentosManual.Infrastructure.csproj", "MovimentosManual.Infrastructure/"]

# Restaurando dependências
RUN dotnet restore "MovimentosManual.Api/MovimentosManual.Api.csproj"

# Copiando todo o restante
COPY . .

# Compilando
WORKDIR "/src/MovimentosManual.Api"
RUN dotnet build "MovimentosManual.Api.csproj" -c Release -o /app/build

# Publicando (otimização final)
FROM build AS publish
RUN dotnet publish "MovimentosManual.Api.csproj" -c Release -o /app/publish

# Imagem final enxuta
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "MovimentosManual.Api.dll"]
