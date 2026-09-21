using Prontuario.Application.DTOs;
using Prontuario.Domain.Entities;

namespace Prontuario.Application.Common;

/// <summary>Conversões Entity -> DTO usadas pelos casos de uso ao montar as respostas da API.</summary>
public static class Mappers
{
    public static PacienteDto ToDto(this Paciente p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        DataNascimento = p.DataNascimento?.ToString("yyyy-MM-dd"),
        Sexo = p.Sexo,
        EstadoCivil = p.EstadoCivil,
        Ocupacao = p.Ocupacao,
        NumeroFilhos = p.NumeroFilhos,
        Religiao = p.Religiao,
        Telefone = p.Telefone,
        Endereco = p.Endereco,
        Numero = p.Numero,
        Bairro = p.Bairro,
        Escolaridade = p.Escolaridade,
        UbsReferencia = p.UbsReferencia,
        ConvenioParticular = p.ConvenioParticular,
        PassaPoliclinica = p.PassaPoliclinica,
        Foto = p.Foto,
        CriadoEm = p.CriadoEm,
        Ativo = p.Ativo,
    };

    public static AvaliacaoSaudeDto ToDto(this AvaliacaoSaude a) => new()
    {
        Id = a.Id,
        PacienteId = a.PacienteId,
        DoencaBase = a.DoencaBase,
        DoencaBaseQuais = a.DoencaBaseQuais,
        Medicacoes = a.Medicacoes,
        MedicacoesQuais = a.MedicacoesQuais,
        Alergias = a.Alergias,
        AlergiasQuais = a.AlergiasQuais,
        Cirurgias = a.Cirurgias,
        CirurgiasQuais = a.CirurgiasQuais,
        Mobilidade = a.Mobilidade,
        MobilidadeObs = a.MobilidadeObs,
        Higiene = a.Higiene,
        HigieneObs = a.HigieneObs,
        CuidadoFerida = a.CuidadoFerida,
        CuidadoFeridaQuem = a.CuidadoFeridaQuem,
        Alimentacao = a.Alimentacao,
        AtividadeFisica = a.AtividadeFisica,
        Habitos = a.Habitos,
        Sono = a.Sono,
        SonoMedicacao = a.SonoMedicacao,
        AtualizadoEm = a.AtualizadoEm,
    };

    public static ExameFisicoDto ToDto(this ExameFisico e) => new()
    {
        Id = e.Id,
        PacienteId = e.PacienteId,
        Dados = e.Dados,
        CriadoEm = e.CriadoEm,
    };

    public static FeridaDto ToDto(this Ferida f) => new()
    {
        Id = f.Id,
        PacienteId = f.PacienteId,
        NumeroLesao = f.NumeroLesao,
        Etiologia = f.Etiologia,
        TempoFerida = f.TempoFerida,
        Localizacao = f.Localizacao,
        Comprimento = f.Comprimento,
        Largura = f.Largura,
        Profundidade = f.Profundidade,
        Descolamento = f.Descolamento,
        TimeTecido = f.TimeTecido,
        TimeInfeccao = f.TimeInfeccao,
        ExsudatoTipo = f.ExsudatoTipo,
        ExsudatoQuantidade = f.ExsudatoQuantidade,
        Bordas = f.Bordas,
        Perilesional = f.Perilesional,
        Biofilme = f.Biofilme,
        BiofilmeSinais = f.BiofilmeSinais,
        ItbDados = f.ItbDados,
        Sensibilidade = f.Sensibilidade,
        SensibilidadeObs = f.SensibilidadeObs,
        Foto = f.Foto,
        CriadoEm = f.CriadoEm,
        Ativa = f.Ativa,
    };

    public static FeridaEvolucaoDto ToDto(this FeridaEvolucao e) => new()
    {
        Id = e.Id,
        FeridaId = e.FeridaId,
        Data = e.Data.ToString("yyyy-MM-dd"),
        Comprimento = e.Comprimento,
        Largura = e.Largura,
        Profundidade = e.Profundidade,
        ExsudatoQuantidade = e.ExsudatoQuantidade,
        Observacoes = e.Observacoes,
        Foto = e.Foto,
        CriadoEm = e.CriadoEm,
    };

    public static DiagnosticoPacienteDto ToDto(this DiagnosticoPaciente d) => new()
    {
        Id = d.Id,
        PacienteId = d.PacienteId,
        DiagnosticoChave = d.DiagnosticoChave,
        Ativo = d.Ativo,
        CriadoEm = d.CriadoEm,
    };

    public static DiagnosticoAvaliacaoDto ToDto(this DiagnosticoAvaliacao a) => new()
    {
        Id = a.Id,
        DiagnosticoPacienteId = a.DiagnosticoPacienteId,
        Data = a.Data.ToString("yyyy-MM-dd"),
        Valores = a.Valores,
        CriadoEm = a.CriadoEm,
    };

    public static DiagnosticoAtividadeDto ToDto(this DiagnosticoAtividade a) => new()
    {
        Id = a.Id,
        DiagnosticoPacienteId = a.DiagnosticoPacienteId,
        Atividade = a.Atividade,
        Data = a.Data.ToString("yyyy-MM-dd"),
        CriadoEm = a.CriadoEm,
    };

    public static PrescricaoDto ToDto(this Prescricao p) => new()
    {
        Id = p.Id,
        PacienteId = p.PacienteId,
        Data = p.Data?.ToString("yyyy-MM-dd"),
        AosCuidadosDe = p.AosCuidadosDe,
        LimpezaSf09 = p.LimpezaSf09,
        Phmb = p.Phmb,
        CremeBarreira = p.CremeBarreira,
        Cobertura = p.Cobertura,
        CobrirCom = p.CobrirCom,
        FrequenciaTroca = p.FrequenciaTroca,
        Observacoes = p.Observacoes,
        CriadoEm = p.CriadoEm,
    };

    public static AgendamentoDto ToDto(this Agendamento a) => new()
    {
        Id = a.Id,
        PacienteId = a.PacienteId,
        NomePacienteAvulso = a.NomePacienteAvulso,
        TelefoneAvulso = a.TelefoneAvulso,
        Data = a.Data.ToString("yyyy-MM-dd"),
        Hora = a.Hora,
        DuracaoMin = a.DuracaoMin,
        Procedimento = a.Procedimento,
        Status = a.Status,
        Observacoes = a.Observacoes,
        CriadoEm = a.CriadoEm,
        PacienteNome = a.Paciente?.Nome,
        PacienteFoto = a.Paciente?.Foto,
        PacienteTelefone = a.Paciente?.Telefone,
    };
}
