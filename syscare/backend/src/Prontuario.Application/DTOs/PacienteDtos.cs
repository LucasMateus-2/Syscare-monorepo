using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Prontuario.Application.DTOs;

/// <summary>
/// Dados de entrada para criar/editar paciente (multipart/form-data, com foto opcional).
/// Usa [FromForm(Name=...)] em vez de [JsonPropertyName] porque o binding de
/// multipart/form-data do ASP.NET Core não considera atributos de System.Text.Json.
/// </summary>
public class PacienteFormRequest
{
    [Required(ErrorMessage = "O nome do paciente é obrigatório.")]
    [FromForm(Name = "nome")]
    public string Nome { get; set; } = null!;

    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Data de nascimento deve estar no formato AAAA-MM-DD.")]
    [FromForm(Name = "data_nascimento")]
    public string? DataNascimento { get; set; }

    [FromForm(Name = "sexo")]
    public string? Sexo { get; set; }

    [FromForm(Name = "estado_civil")]
    public string? EstadoCivil { get; set; }

    [FromForm(Name = "ocupacao")]
    public string? Ocupacao { get; set; }

    [FromForm(Name = "numero_filhos")]
    public string? NumeroFilhos { get; set; }

    [FromForm(Name = "religiao")]
    public string? Religiao { get; set; }

    [FromForm(Name = "telefone")]
    public string? Telefone { get; set; }

    [FromForm(Name = "endereco")]
    public string? Endereco { get; set; }

    [FromForm(Name = "numero")]
    public string? Numero { get; set; }

    [FromForm(Name = "bairro")]
    public string? Bairro { get; set; }

    [FromForm(Name = "escolaridade")]
    public string? Escolaridade { get; set; }

    [FromForm(Name = "ubs_referencia")]
    public string? UbsReferencia { get; set; }

    [FromForm(Name = "convenio_particular")]
    public string? ConvenioParticular { get; set; }

    [FromForm(Name = "passa_policlinica")]
    public string? PassaPoliclinica { get; set; }
}

public class PacienteDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("nome")] public string Nome { get; set; } = null!;
    [JsonPropertyName("data_nascimento")] public string? DataNascimento { get; set; }
    [JsonPropertyName("sexo")] public string? Sexo { get; set; }
    [JsonPropertyName("estado_civil")] public string? EstadoCivil { get; set; }
    [JsonPropertyName("ocupacao")] public string? Ocupacao { get; set; }
    [JsonPropertyName("numero_filhos")] public string? NumeroFilhos { get; set; }
    [JsonPropertyName("religiao")] public string? Religiao { get; set; }
    [JsonPropertyName("telefone")] public string? Telefone { get; set; }
    [JsonPropertyName("endereco")] public string? Endereco { get; set; }
    [JsonPropertyName("numero")] public string? Numero { get; set; }
    [JsonPropertyName("bairro")] public string? Bairro { get; set; }
    [JsonPropertyName("escolaridade")] public string? Escolaridade { get; set; }
    [JsonPropertyName("ubs_referencia")] public string? UbsReferencia { get; set; }
    [JsonPropertyName("convenio_particular")] public string? ConvenioParticular { get; set; }
    [JsonPropertyName("passa_policlinica")] public string? PassaPoliclinica { get; set; }
    [JsonPropertyName("foto")] public string? Foto { get; set; }
    [JsonPropertyName("criado_em")] public DateTimeOffset CriadoEm { get; set; }
    [JsonPropertyName("ativo")] public bool Ativo { get; set; }
}

/// <summary>Avaliação integrativa de saúde física (registro único e editável por paciente).</summary>
public class AvaliacaoSaudeRequest
{
    [JsonPropertyName("doenca_base")] public string? DoencaBase { get; set; }
    [JsonPropertyName("doenca_base_quais")] public List<string>? DoencaBaseQuais { get; set; }
    [JsonPropertyName("medicacoes")] public string? Medicacoes { get; set; }
    [JsonPropertyName("medicacoes_quais")] public string? MedicacoesQuais { get; set; }
    [JsonPropertyName("alergias")] public string? Alergias { get; set; }
    [JsonPropertyName("alergias_quais")] public string? AlergiasQuais { get; set; }
    [JsonPropertyName("cirurgias")] public string? Cirurgias { get; set; }
    [JsonPropertyName("cirurgias_quais")] public string? CirurgiasQuais { get; set; }
    [JsonPropertyName("mobilidade")] public string? Mobilidade { get; set; }
    [JsonPropertyName("mobilidade_obs")] public string? MobilidadeObs { get; set; }
    [JsonPropertyName("higiene")] public string? Higiene { get; set; }
    [JsonPropertyName("higiene_obs")] public string? HigieneObs { get; set; }
    [JsonPropertyName("cuidado_ferida")] public string? CuidadoFerida { get; set; }
    [JsonPropertyName("cuidado_ferida_quem")] public string? CuidadoFeridaQuem { get; set; }
    [JsonPropertyName("alimentacao")] public string? Alimentacao { get; set; }
    [JsonPropertyName("atividade_fisica")] public string? AtividadeFisica { get; set; }
    [JsonPropertyName("habitos")] public List<string>? Habitos { get; set; }
    [JsonPropertyName("sono")] public List<string>? Sono { get; set; }
    [JsonPropertyName("sono_medicacao")] public string? SonoMedicacao { get; set; }
}

public class AvaliacaoSaudeDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("paciente_id")] public int PacienteId { get; set; }
    [JsonPropertyName("doenca_base")] public string? DoencaBase { get; set; }
    [JsonPropertyName("doenca_base_quais")] public List<string> DoencaBaseQuais { get; set; } = new();
    [JsonPropertyName("medicacoes")] public string? Medicacoes { get; set; }
    [JsonPropertyName("medicacoes_quais")] public string? MedicacoesQuais { get; set; }
    [JsonPropertyName("alergias")] public string? Alergias { get; set; }
    [JsonPropertyName("alergias_quais")] public string? AlergiasQuais { get; set; }
    [JsonPropertyName("cirurgias")] public string? Cirurgias { get; set; }
    [JsonPropertyName("cirurgias_quais")] public string? CirurgiasQuais { get; set; }
    [JsonPropertyName("mobilidade")] public string? Mobilidade { get; set; }
    [JsonPropertyName("mobilidade_obs")] public string? MobilidadeObs { get; set; }
    [JsonPropertyName("higiene")] public string? Higiene { get; set; }
    [JsonPropertyName("higiene_obs")] public string? HigieneObs { get; set; }
    [JsonPropertyName("cuidado_ferida")] public string? CuidadoFerida { get; set; }
    [JsonPropertyName("cuidado_ferida_quem")] public string? CuidadoFeridaQuem { get; set; }
    [JsonPropertyName("alimentacao")] public string? Alimentacao { get; set; }
    [JsonPropertyName("atividade_fisica")] public string? AtividadeFisica { get; set; }
    [JsonPropertyName("habitos")] public List<string> Habitos { get; set; } = new();
    [JsonPropertyName("sono")] public List<string> Sono { get; set; } = new();
    [JsonPropertyName("sono_medicacao")] public string? SonoMedicacao { get; set; }
    [JsonPropertyName("atualizado_em")] public DateTimeOffset AtualizadoEm { get; set; }
}

public class ExameFisicoRequest
{
    [JsonPropertyName("dados")]
    public Dictionary<string, object?>? Dados { get; set; }
}

public class ExameFisicoDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("paciente_id")] public int PacienteId { get; set; }
    [JsonPropertyName("dados")] public Dictionary<string, object?> Dados { get; set; } = new();
    [JsonPropertyName("criado_em")] public DateTimeOffset CriadoEm { get; set; }
}

/// <summary>Resposta da ficha completa do paciente (GET /api/pacientes/{id}).</summary>
public class FichaPacienteResponse
{
    [JsonPropertyName("paciente")] public PacienteDto Paciente { get; set; } = null!;
    [JsonPropertyName("avaliacaoSaude")] public AvaliacaoSaudeDto? AvaliacaoSaude { get; set; }
    [JsonPropertyName("exameFisico")] public ExameFisicoDto? ExameFisico { get; set; }
    [JsonPropertyName("feridas")] public List<FeridaDto> Feridas { get; set; } = new();
    [JsonPropertyName("diagnosticosAtivos")] public List<string> DiagnosticosAtivos { get; set; } = new();
    [JsonPropertyName("prescricoes")] public List<PrescricaoDto> Prescricoes { get; set; } = new();
}
