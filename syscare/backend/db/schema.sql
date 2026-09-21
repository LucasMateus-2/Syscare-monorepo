-- Schema do banco de dados do sistema de prontuário / agendamento (PostgreSQL)

CREATE TABLE IF NOT EXISTS admin (
    id SERIAL PRIMARY KEY,
    usuario TEXT UNIQUE NOT NULL,
    senha_hash TEXT NOT NULL,
    nome TEXT NOT NULL,
    criado_em TIMESTAMPTZ DEFAULT now()
);

CREATE TABLE IF NOT EXISTS pacientes (
    id SERIAL PRIMARY KEY,
    nome TEXT NOT NULL,
    data_nascimento DATE,
    sexo TEXT,
    estado_civil TEXT,
    ocupacao TEXT,
    numero_filhos TEXT,
    religiao TEXT,
    telefone TEXT,
    endereco TEXT,
    numero TEXT,
    bairro TEXT,
    escolaridade TEXT,
    ubs_referencia TEXT,
    convenio_particular TEXT,
    passa_policlinica TEXT,
    foto TEXT,
    criado_em TIMESTAMPTZ DEFAULT now(),
    ativo BOOLEAN DEFAULT true
);

-- Avaliação integrativa da saúde física (um registro "atual" por paciente, editável)
CREATE TABLE IF NOT EXISTS avaliacao_saude (
    id SERIAL PRIMARY KEY,
    paciente_id INTEGER NOT NULL REFERENCES pacientes(id) ON DELETE CASCADE,
    doenca_base TEXT,
    doenca_base_quais JSONB DEFAULT '[]',
    medicacoes TEXT,
    medicacoes_quais TEXT,
    alergias TEXT,
    alergias_quais TEXT,
    cirurgias TEXT,
    cirurgias_quais TEXT,
    mobilidade TEXT,
    mobilidade_obs TEXT,
    higiene TEXT,
    higiene_obs TEXT,
    cuidado_ferida TEXT,
    cuidado_ferida_quem TEXT,
    alimentacao TEXT,
    atividade_fisica TEXT,
    habitos JSONB DEFAULT '[]',
    sono JSONB DEFAULT '[]',
    sono_medicacao TEXT,
    atualizado_em TIMESTAMPTZ DEFAULT now(),
    UNIQUE(paciente_id)
);

-- Avaliação do exame físico (cabeça aos pés) — histórico completo
CREATE TABLE IF NOT EXISTS exame_fisico (
    id SERIAL PRIMARY KEY,
    paciente_id INTEGER NOT NULL REFERENCES pacientes(id) ON DELETE CASCADE,
    dados JSONB NOT NULL DEFAULT '{}',
    criado_em TIMESTAMPTZ DEFAULT now()
);

-- Avaliações de ferida (pode haver várias ao longo do tempo / várias lesões)
CREATE TABLE IF NOT EXISTS feridas (
    id SERIAL PRIMARY KEY,
    paciente_id INTEGER NOT NULL REFERENCES pacientes(id) ON DELETE CASCADE,
    numero_lesao TEXT,
    etiologia TEXT,
    tempo_ferida TEXT,
    localizacao TEXT,
    comprimento TEXT,
    largura TEXT,
    profundidade TEXT,
    descolamento TEXT,
    time_tecido JSONB DEFAULT '[]',
    time_infeccao JSONB DEFAULT '[]',
    exsudato_tipo TEXT,
    exsudato_quantidade TEXT,
    bordas JSONB DEFAULT '[]',
    perilesional JSONB DEFAULT '[]',
    biofilme TEXT,
    biofilme_sinais JSONB DEFAULT '[]',
    itb_dados JSONB DEFAULT '{}',
    sensibilidade TEXT,
    sensibilidade_obs TEXT,
    foto TEXT,
    criado_em TIMESTAMPTZ DEFAULT now(),
    ativa BOOLEAN DEFAULT true
);

-- Medições/evoluções de uma ferida ao longo do tempo (histórico)
CREATE TABLE IF NOT EXISTS ferida_evolucoes (
    id SERIAL PRIMARY KEY,
    ferida_id INTEGER NOT NULL REFERENCES feridas(id) ON DELETE CASCADE,
    data DATE NOT NULL,
    comprimento TEXT,
    largura TEXT,
    profundidade TEXT,
    exsudato_quantidade TEXT,
    observacoes TEXT,
    foto TEXT,
    criado_em TIMESTAMPTZ DEFAULT now()
);

-- Diagnósticos de enfermagem ativados por paciente (NANDA/NOC/NIC)
CREATE TABLE IF NOT EXISTS diagnosticos_paciente (
    id SERIAL PRIMARY KEY,
    paciente_id INTEGER NOT NULL REFERENCES pacientes(id) ON DELETE CASCADE,
    diagnostico_chave TEXT NOT NULL,
    ativo BOOLEAN DEFAULT true,
    criado_em TIMESTAMPTZ DEFAULT now(),
    UNIQUE(paciente_id, diagnostico_chave)
);

-- Avaliações (colunas de data) de indicadores NOC para um diagnóstico do paciente
CREATE TABLE IF NOT EXISTS diagnostico_avaliacoes (
    id SERIAL PRIMARY KEY,
    diagnostico_paciente_id INTEGER NOT NULL REFERENCES diagnosticos_paciente(id) ON DELETE CASCADE,
    data DATE NOT NULL,
    valores JSONB NOT NULL DEFAULT '{}',
    criado_em TIMESTAMPTZ DEFAULT now()
);

-- Realizações de atividades NIC (marcação de datas em que a atividade foi feita)
CREATE TABLE IF NOT EXISTS diagnostico_atividades (
    id SERIAL PRIMARY KEY,
    diagnostico_paciente_id INTEGER NOT NULL REFERENCES diagnosticos_paciente(id) ON DELETE CASCADE,
    atividade TEXT NOT NULL,
    data DATE NOT NULL,
    criado_em TIMESTAMPTZ DEFAULT now()
);

-- Prescrições de curativo
CREATE TABLE IF NOT EXISTS prescricoes (
    id SERIAL PRIMARY KEY,
    paciente_id INTEGER NOT NULL REFERENCES pacientes(id) ON DELETE CASCADE,
    data DATE,
    aos_cuidados_de TEXT,
    limpeza_sf09 BOOLEAN DEFAULT false,
    phmb BOOLEAN DEFAULT false,
    creme_barreira BOOLEAN DEFAULT false,
    cobertura TEXT,
    cobrir_com JSONB DEFAULT '[]',
    frequencia_troca TEXT,
    observacoes TEXT,
    criado_em TIMESTAMPTZ DEFAULT now()
);

-- Agendamentos
CREATE TABLE IF NOT EXISTS agendamentos (
    id SERIAL PRIMARY KEY,
    paciente_id INTEGER REFERENCES pacientes(id) ON DELETE SET NULL,
    nome_paciente_avulso TEXT,
    telefone_avulso TEXT,
    data DATE NOT NULL,
    hora TEXT NOT NULL,
    duracao_min INTEGER DEFAULT 30,
    procedimento TEXT,
    status TEXT DEFAULT 'agendado',
    observacoes TEXT,
    criado_em TIMESTAMPTZ DEFAULT now()
);

CREATE INDEX IF NOT EXISTS idx_agendamentos_data ON agendamentos(data);
CREATE INDEX IF NOT EXISTS idx_feridas_paciente ON feridas(paciente_id);
CREATE INDEX IF NOT EXISTS idx_diag_paciente ON diagnosticos_paciente(paciente_id);
CREATE INDEX IF NOT EXISTS idx_diag_avaliacoes ON diagnostico_avaliacoes(diagnostico_paciente_id);
CREATE INDEX IF NOT EXISTS idx_diag_atividades ON diagnostico_atividades(diagnostico_paciente_id);
