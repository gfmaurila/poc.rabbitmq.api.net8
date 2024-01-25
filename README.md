
### API SQL Server CRUD com RabbitMQ e Redis em .NET Core 8

## Descrição
Este projeto implementa uma API em .NET Core 8 para operações CRUD de cadastro de produtos, utilizando SQL Server. A API se destaca pela integração com RabbitMQ para o gerenciamento de filas e Redis para armazenamento de dados em cache, garantindo alta performance e eficiência. Tudo é orquestrado através do Docker Compose, assegurando uma implantação e escalabilidade simplificadas.

## Tecnologias Utilizadas
- .NET Core 8
- SQL Server
- RabbitMQ
- Redis
- Docker
- Docker Compose

## Pré-requisitos
Para rodar este projeto, é necessário ter instalado:
- .NET Core 8 SDK
- Docker
- SQL Server
- RabbitMQ
- Redis
- Docker Compose

## Configuração e Instalação

### Clonando o Repositório
Clone o repositório usando: [Link do seu repositório GitHub]

### Configurando o Docker e Docker Compose
Execute o comando `docker-compose up --build` para inicializar os contêineres.

### SQL Server
Instruções para configuração inicial do banco de dados SQL Server.
- Add-Migration Inicial -Context SqlServerDb
- Update-Database -Context SqlServerDb

## Utilização
Descreva como usar a API, incluindo rotas e exemplos de requisições.

#### API SQL Server - Docker
- GET ALL
```
curl -X 'GET' \
  'http://localhost:5071/api/produto' \
  -H 'accept: */*'
```

- POST
```
curl -X 'POST' \
  'http://localhost:5071/api/produto' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "id": 0,
  "nome": "string",
  "preco": 0
}'
```

- PUT
```
curl -X 'PUT' \
  'http://localhost:5071/api/produto' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "id": 0,
  "nome": "string",
  "preco": 0
}'
```

- GET BY ID
```
curl -X 'GET' \
  'http://localhost:5071/api/produto/1' \
  -H 'accept: */*'
```

- DELETE BY ID
```
curl -X 'DELETE' \
  'http://localhost:5071/api/produto/1' \
  -H 'accept: */*'
```

## Youtube
https://www.youtube.com/watch?v=v-_yNDviInQ

## Autor

- Guilherme Figueiras Maurila

[![Linkedin Badge](https://img.shields.io/badge/-Guilherme_Figueiras_Maurila-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/guilherme-maurila)](https://www.linkedin.com/in/guilherme-maurila)
[![Gmail Badge](https://img.shields.io/badge/-gfmaurila@gmail.com-c14438?style=flat-square&logo=Gmail&logoColor=white&link=mailto:gfmaurila@gmail.com)](mailto:gfmaurila@gmail.com)


