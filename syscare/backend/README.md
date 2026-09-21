# Prontuário — Backend (C# / .NET 8, Clean Architecture)

Reescrita do backend original (Node.js/Express) em **C# com ASP.NET Core**,
organizada em **Clean Architecture** com quatro projetos, e banco de dados
**PostgreSQL** subindo via **Docker Compose**. O contrato da API (rotas,
formato JSON, autenticação por cookie) foi mantido idêntico ao backend
original para que o front-end React já existente continue funcionando sem
alterações.

## Arquitetura

```
Prontuario.sln
├── src/Prontuario.Domain          → Entidades e exceções de domínio. Zero dependências externas.
├── src/Prontuario.Application     → Casos de uso (UseCases), DTOs, interfaces (portas) e validação.
├── src/Prontuario.Infrastructure  → EF Core + Npgsql, JWT, hash de senha (BCrypt), armazenamento de arquivos.
└── src/Prontuario.API             → ASP.NET Core Web API: controllers, middleware, composição (Program.cs).
```

Regra de dependência: `API → Application/Infrastructure → Domain`.
A Application não conhece EF Core concreto nem Npgsql — apenas a abstração
`IApplicationDbContext`, implementada pela Infrastructure.

## Banco de dados

O schema (`db/schema.sql`) é o mesmo do backend original, sem alterações, e é
executado **automaticamente** pela imagem oficial do Postgres na primeira
inicialização do container (via `/docker-entrypoint-initdb.d`). Não há
migrations do EF Core — o EF Core apenas mapeia (via Fluent API, em
`Prontuario.Infrastructure/Persistence/Configurations`) para as tabelas já
existentes.

## Como rodar

1. Copie o arquivo de variáveis de ambiente e ajuste os valores:

   ```bash
   cp .env.example .env
   ```

   Gere um segredo forte para o JWT, por exemplo:

   ```bash
   openssl rand -base64 48
   ```

2. Suba tudo com Docker Compose:

   ```bash
   docker compose up --build
   ```

   Isso sobe dois serviços:
   - `db`: PostgreSQL 16, com o schema já aplicado e dados persistidos em volume.
   - `api`: a API .NET, publicada e escutando em `http://localhost:8080` (ou a porta definida em `API_PORT`).

3. Aponte o front-end para `http://localhost:8080/api` (ou configure
   `FRONTEND_ORIGIN` no `.env` com a URL onde o front-end estiver rodando,
   para o CORS liberar as chamadas com cookies).

Para derrubar tudo (mantendo os dados): `docker compose down`.
Para apagar também os volumes (banco e uploads): `docker compose down -v`.

## Desenvolvimento local (sem Docker para a API)

Suba só o banco:

```bash
docker compose up db
```

E rode a API com o SDK do .NET 8 instalado:

```bash
cd src/Prontuario.API
dotnet run
```

Nesse modo, a configuração vem de `appsettings.Development.json` (já
apontando para `localhost:5432`) e/ou de `dotnet user-secrets`.

## Autenticação

Mesmo fluxo do backend original:

- `GET /api/auth/status` → `{ hasAdmin, loggedIn, admin }` (indica se já existe
  administrador e se a requisição atual está autenticada).
- `POST /api/auth/configurar` → cria o único administrador do sistema (só
  funciona enquanto não existir nenhum) e define o cookie de sessão.
- `POST /api/auth/login` / `POST /api/auth/logout`.

O token JWT é entregue em um cookie `httpOnly` chamado `token` (não em
`Authorization: Bearer`), então o front-end deve continuar chamando a API com
`credentials: "include"`.

## Endpoints principais

Todos sob `/api`, protegidos por autenticação (exceto `auth/*` e `health`):

- `pacientes` — CRUD de pacientes, foto, avaliação de saúde, exame físico
  (com histórico), feridas (com evoluções), diagnósticos de enfermagem
  (NANDA/NOC/NIC) e prescrições de curativo — tudo aninhado sob
  `/api/pacientes/{id}/...`, como no backend original.
- `agenda` — agenda do dia, contagem por dia no mês, criação/edição/exclusão
  de agendamentos e busca rápida de pacientes.

Uploads (fotos) ficam acessíveis em `/uploads/{subpasta}/{arquivo}`.

## Observações de compatibilidade

- Os DTOs de resposta usam `[JsonPropertyName]` para reproduzir exatamente os
  mesmos nomes de campo (snake_case) do backend original.
- Os DTOs usados em `multipart/form-data` (criação/edição de paciente,
  registro de feridas e evoluções) usam `[FromForm(Name = "...")]` pelo mesmo
  motivo — o model binder de formulário do ASP.NET Core não lê atributos do
  `System.Text.Json`.
- Erros são sempre retornados como `{ "erro": "mensagem" }`, com o mesmo
  status HTTP que o backend original usava (400, 401, 404, 409, etc.).
