FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5071

ENV ASPNETCORE_URLS=http://+:5071
ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo .csproj para o diretório de trabalho atual no contêiner
COPY src/poc.api.sqlserver.net8/poc.api.sqlserver.csproj .

# Restaura as dependências do projeto
RUN dotnet restore poc.api.sqlserver.csproj

# Copia o restante dos arquivos do projeto para o contêiner
COPY . .

# Define o diretório de trabalho e constrói o projeto
WORKDIR /src
RUN dotnet build src/poc.api.sqlserver.net8/poc.api.sqlserver.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish src/poc.api.sqlserver.net8/poc.api.sqlserver.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "poc.api.sqlserver.dll"]
