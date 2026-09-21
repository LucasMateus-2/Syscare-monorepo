@echo off
chcp 65001 >nul
title Sistema da Clinica - React + PostgreSQL
cd /d "%~dp0"

echo ============================================================
echo   Verificando se o Node.js esta instalado...
echo ============================================================
node --version >nul 2>&1
if errorlevel 1 (
    echo.
    echo [ERRO] Node.js nao foi encontrado no seu computador.
    echo Baixe e instale (versao LTS) em: https://nodejs.org/
    echo.
    pause
    exit /b
)

if not exist "backend\.env" (
    echo ============================================================
    echo   Primeira execucao: configurando o backend...
    echo ============================================================
    copy "backend\.env.example" "backend\.env" >nul
    echo.
    echo [ATENCAO] Foi criado o arquivo backend\.env
    echo Abra esse arquivo em um editor de texto e ajuste a linha DATABASE_URL
    echo com os dados de acesso do SEU PostgreSQL antes de continuar.
    echo Exemplo: postgresql://postgres:SUASENHA@localhost:5432/clinica
    echo.
    notepad "backend\.env"
    echo Pressione qualquer tecla depois de salvar o arquivo para continuar...
    pause >nul
)

echo ============================================================
echo   Instalando dependencias do backend (pode levar alguns minutos)...
echo ============================================================
cd backend
if not exist "node_modules" (
    call npm install
)

echo ============================================================
echo   Criando/verificando as tabelas no PostgreSQL...
echo ============================================================
call npm run db:init
if errorlevel 1 (
    echo.
    echo [ERRO] Nao foi possivel conectar ao PostgreSQL.
    echo Verifique se o PostgreSQL esta rodando e se o arquivo backend\.env
    echo esta com os dados corretos (usuario, senha, porta e nome do banco).
    echo.
    pause
    exit /b
)
cd ..

echo ============================================================
echo   Instalando dependencias do front-end (pode levar alguns minutos)...
echo ============================================================
cd frontend
if not exist "node_modules" (
    call npm install
)

echo ============================================================
echo   Compilando o front-end...
echo ============================================================
call npm run build
cd ..

echo ============================================================
echo   Iniciando o servidor...
echo ============================================================
start "" http://localhost:5000
cd backend
call npm start

pause
