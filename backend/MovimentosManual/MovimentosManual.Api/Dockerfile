# 🔹 Runtime Base: .NET 8 LTS
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# 🔹 SDK para build e publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar projetos individuais para aproveitar cache de restore
COPY ["MovimentosManual.Api/MovimentosManual.Api.csproj", "MovimentosManual.Api/"]
COPY ["MovimentosManual.Application/MovimentosManual.Application.csproj", "MovimentosManual.Application/"]
COPY ["MovimentosManual.Domain/MovimentosManual.Domain.csproj", "MovimentosManual.Domain/"]
COPY ["MovimentosManual.Infrastructure/MovimentosManual.Infrastructure.csproj", "MovimentosManual.Infrastructure/"]

# 🔹 Restaurar dependências
RUN dotnet restore "MovimentosManual.Api/MovimentosManual.Api.csproj"

# Copiar todo o restante
COPY . .

# 🔹 Build
WORKDIR "/src/MovimentosManual.Api"
RUN dotnet build "MovimentosManual.Api.csproj" -c Release -o /app/build

# 🔹 Publish otimizado (sem arquivos desnecessários)
FROM build AS publish
RUN dotnet publish "MovimentosManual.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 🔹 Runtime final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovimentosManual.Api.dll"]
