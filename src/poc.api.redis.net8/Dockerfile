FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5074

ENV ASPNETCORE_URLS=http://+:5074
ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo .csproj para o diretório de trabalho atual no contêiner
COPY src/poc.api.redis.net8/poc.api.redis.csproj .

# Restaura as dependências do projeto
RUN dotnet restore poc.api.redis.csproj

# Copia o restante dos arquivos do projeto para o contêiner
COPY . .

# Define o diretório de trabalho e constrói o projeto
WORKDIR /src
RUN dotnet build src/poc.api.redis.net8/poc.api.redis.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish src/poc.api.redis.net8/poc.api.redis.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "poc.api.redis.dll"]