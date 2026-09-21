# SysCare

Sistema de gestão de **atendimento clínico de feridas** para ambulatório universitário, desenvolvido como Trabalho de Conclusão de Curso (TCC) em Ciência da Computação.

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-ASP.NET%20Core-239120?logo=csharp&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-EF%20Core-4169E1?logo=postgresql&logoColor=white)
![React](https://img.shields.io/badge/Frontend-React-61DAFB?logo=react&logoColor=black)
![License](https://img.shields.io/badge/license-MIT-green)

---

## Sobre o projeto

O SysCare digitaliza o registro clínico de pacientes com feridas atendidos em um ambulatório universitário. O projeto foi aprovado pela direção da instituição para **implantação real**, o que significa que lida com dados clínicos verdadeiros e precisa respeitar a **LGPD** (criptografia, registro de acessos e consentimento documentado do paciente).

O sistema atende um grupo pequeno e fixo de usuários internos, com dois perfis:

| Perfil | Descrição |
|---|---|
| **Nurse** (Enfermeiro) | Cria os demais usuários e conduz os atendimentos |
| **Student** (Estudante) | Participa dos atendimentos sob supervisão |

Não há login de paciente: o paciente é uma entidade do domínio, não um usuário do sistema.

## Funcionalidades

- **Cadastro de pacientes** com os campos da ficha clínica de admissão (estado civil, sexo, ocupação, endereço, unidade de saúde de referência, convênio)
- **Agendamento e registro de atendimentos**
- **Avaliação clínica de feridas** (contexto `WoundCare`):
  - saúde física e estilo de vida
  - exame físico, incluindo pontos de sensibilidade com monofilamento
  - avaliação do tecido pelo protocolo **TIME**
  - avaliação de **biofilme**
  - `WoundCareRecord` (1:1 com o atendimento) e `WoundAssessment` (1:N)
- **Autenticação JWT** com criação dinâmica de usuários (sem seed de banco)

## Arquitetura

O backend segue **Clean Architecture** com **DDD**, dividido em quatro projetos:

```
SysCare.Domain           → entidades, value objects, serviços de domínio
SysCare.Application      → casos de uso (UseCases), DTOs, validações
SysCare.Infrastructure   → EF Core, repositórios, segurança (JWT, BCrypt)
SysCare.API              → controllers, configuração, Program.cs
```

Convenções adotadas:

- Uma classe por arquivo
- Casos de uso na camada Application chamados de `UseCase`; algoritmos de domínio chamados de `Services`
- Handlers chamados diretamente, **sem MediatR**
- Validação por **Data Annotations** nos DTOs de request (sem FluentValidation)
- Código em inglês; alguns campos de DTO voltados ao frontend em português (`nome`, `senha`)
- Entidades de domínio com construtor privado sem parâmetros para materialização pelo EF Core

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Backend | C# / ASP.NET Core (.NET 10) |
| Persistência | EF Core + Npgsql, PostgreSQL |
| Autenticação | JWT Bearer, BCrypt (work factor 12) |
| Frontend | React |
| Infra local | Docker Compose (PostgreSQL) |
| Deploy | Railway (PostgreSQL) e Vercel (frontend) |

## Autenticação e primeiro acesso

O sistema inicia com o banco **vazio**. O primeiro usuário é criado por um fluxo de bootstrap:

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/Auth/status` | Informa se já existe algum usuário |
| `POST` | `/api/Auth/bootstrap` | Cria o primeiro usuário (sempre `Nurse`, uso único) |
| `POST` | `/api/Auth/register` | Cria novos usuários (somente `Nurse`, exige JWT) |

O frontend armazena o token Bearer no `localStorage` e o envia no header `Authorization`.

## Como executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) e Docker Compose
- Node.js (para o frontend)

### Backend

```bash
# clonar o repositório
git clone https://github.com/LucasMateus-2/Syscare-monorepo.git
cd Syscare-monorepo/syscare

# subir o PostgreSQL local
docker compose up -d

# configurar o segredo do JWT (apenas desenvolvimento)
dotnet user-secrets set "Jwt:Secret" "<uma-chave-longa-e-aleatoria>" --project SysCare.API

# aplicar migrations e executar
dotnet ef database update --project SysCare.Infrastructure --startup-project SysCare.API
dotnet run --project SysCare.API
```

<!-- TODO: confirmar nomes exatos de pastas, chave de configuração do JWT e localização do docker-compose.yml -->

### Variáveis de ambiente em produção

Em produção, os segredos vêm de variáveis de ambiente, usando `__` como separador de seções:

```
Jwt__Secret=...
ConnectionStrings__DefaultConnection=...
```

### CORS

O `Program.cs` precisa liberar a origem do frontend. Se o login parecer quebrado, **verifique o CORS primeiro**: falhas de rede podem cair silenciosamente em fallbacks locais e esconder o erro real da API.

### Linux: limite do inotify

Se o `dotnet watch` falhar por limite de arquivos observados:

```bash
sudo sysctl fs.inotify.max_user_instances=512
```

## LGPD

Por lidar com dados clínicos reais, o projeto considera:

- criptografia de dados sensíveis
- registro de acessos (access logging)
- consentimento documentado do paciente
- controle de acesso por perfil

<!-- Ajustar esta seção conforme o que já estiver implementado -->

## Documentação da API

O contrato completo para o frontend (endpoints, formatos JSON, tabelas de enums e formato de erros) está documentado em `api-para-frontend.md`.

## Status

Em desenvolvimento como TCC (2026). Próximos passos:

- [ ] Finalizar e validar a integração completa frontend ↔ backend
- [ ] Revisar requisitos de conformidade com a LGPD
- [ ] Deploy (PostgreSQL no Railway, frontend na Vercel)

## Autor

**Lucas** — [@LucasMateus-2](https://github.com/LucasMateus-2)

**Alexander** 

## Licença

Distribuído sob a licença MIT. Veja o arquivo [LICENSE](LICENSE).
