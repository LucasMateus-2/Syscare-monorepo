# Sistema da Clínica — React + Node.js + PostgreSQL

Versão reescrita do sistema usando:
- **Front-end**: React (Vite)
- **Back-end**: Node.js + Express (API REST)
- **Banco de dados**: PostgreSQL

Mesmas funcionalidades da versão anterior (Flask/SQLite): login do
administrador, agenda de atendimentos, prontuário completo (dados
cadastrais + foto, avaliação de saúde, exame físico, avaliação de
feridas com TIME/ITB/biofilme, diagnósticos de enfermagem NANDA/NOC/NIC
ativáveis por paciente, prescrição de curativo imprimível).

## Pré-requisitos

1. **Node.js** (versão 18 ou mais recente) — https://nodejs.org/
2. **PostgreSQL** já instalado e rodando no seu computador (você indicou que já tem)

## Como rodar (Windows)

1. Copie a pasta `prontuario-react` inteira para o seu computador.
2. Dê **dois cliques** em `INICIAR.bat`.
   - Na primeira execução, ele vai:
     a. Criar o arquivo `backend/.env` e abrir no Bloco de Notas para você
        colocar os dados de acesso do seu PostgreSQL (usuário, senha,
        porta, nome do banco). Edite a linha `DATABASE_URL` e salve.
     b. Instalar as dependências do backend e do frontend (`npm install`)
        — precisa de internet nesse passo.
     c. Criar as tabelas no banco automaticamente.
     d. Compilar o front-end.
   - Nas próximas vezes, abre direto (bem mais rápido).
3. O sistema abre em `http://localhost:5000`.
4. Na primeira tela, crie o usuário e senha do administrador.

Para **encerrar**, feche a janela preta (terminal).

## Configuração do banco (`backend/.env`)

```
DATABASE_URL=postgresql://usuario:senha@localhost:5432/clinica
PORT=5000
JWT_SECRET=alguma-frase-aleatoria-bem-grande
```

Se o banco `clinica` ainda não existir, crie antes pelo pgAdmin ou psql:

```sql
CREATE DATABASE clinica;
```

## Rodando em modo desenvolvimento (opcional, para quem for mexer no código)

Terminal 1 — backend:
```
cd backend
npm install
npm run db:init
npm run dev
```

Terminal 2 — frontend (com recarregamento automático):
```
cd frontend
npm install
npm run dev
```

Acesse `http://localhost:5173` (o Vite já redireciona as chamadas de API para o backend na porta 5000).

## Estrutura do projeto

```
prontuario-react/
  backend/
    src/
      server.js          → servidor Express
      db.js               → conexão com PostgreSQL
      schema.sql          → estrutura das tabelas
      initdb.js           → script que cria as tabelas
      middleware/auth.js  → autenticação (JWT em cookie)
      routes/              → auth.js, pacientes.js, agenda.js
      utils/               → upload de fotos, dados dos diagnósticos
    uploads/               → fotos de pacientes e feridas
  frontend/
    src/
      pages/               → telas (Dashboard, Pacientes, Agenda, etc.)
      components/          → Sidebar/Layout, componentes reutilizáveis
      context/             → autenticação e mensagens (flash)
      styles.css           → estilo visual completo
```

## Diagnósticos de enfermagem

Igual à versão anterior: na ficha do paciente, aba **Diagnósticos**, cada
diagnóstico tem uma chavinha para ativar/desativar. Só os diagnósticos
ativados mostram o botão "Preencher" com a tabela NOC/NIC completa.

## Sobre o selo "PolariScore"

Deixei um selo discreto no canto inferior direito de todas as telas,
como você pediu. Se quiser trocar por uma logo de verdade (imagem),
é só me enviar o arquivo que eu ajusto.

## Backup dos dados

Diferente da versão SQLite (um arquivo só), agora os dados ficam dentro
do PostgreSQL. Para fazer backup, use o `pg_dump`:

```
pg_dump -U postgres clinica > backup_clinica.sql
```

As fotos continuam sendo arquivos, na pasta `backend/uploads/` — inclua
essa pasta no seu backup também.
