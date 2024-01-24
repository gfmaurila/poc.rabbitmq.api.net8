### POC API SqlServer Dapper .NET Core 8

## Descrição
Este projeto consiste em uma Minimal API desenvolvida com .NET Core 8, utilizando o micro ORM Dapper para operações SQL, e integrada com SQL Server. O sistema é encapsulado em contêineres Docker, gerenciados pelo Docker Compose. O principal objetivo é fornecer uma API eficiente para cadastro, consulta, atualização e remoção de produtos.

## Tecnologias Utilizadas
- .NET Core 8
- SQL Server
- Dapper
- Docker
- Docker Compose

## Pré-requisitos
Para executar este projeto, você precisará:
- .NET Core 8 SDK
- Docker
- Docker Compose

## Configuração e Instalação

### Clonando o Repositório
Clone o repositório usando: https://github.com/gfmaurila/poc.api.sqlserver.dapper.net8

### Configurando o Docker e Docker Compose
docker-compose up --build
http://localhost:5072/swagger/index.html

### SQL Server
Add-Migration Inicial -Context SqlServerDb
Update-Database -Context SqlServerDb

## Youtube
https://youtu.be/hI-evhBOHT0

## Autor

- Guilherme Figueiras Maurila

[![Linkedin Badge](https://img.shields.io/badge/-Guilherme_Figueiras_Maurila-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/guilherme-maurila)](https://www.linkedin.com/in/guilherme-maurila)
[![Gmail Badge](https://img.shields.io/badge/-gfmaurila@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:gfmaurila@gmail.com)](mailto:gfmaurila@gmail.com)
