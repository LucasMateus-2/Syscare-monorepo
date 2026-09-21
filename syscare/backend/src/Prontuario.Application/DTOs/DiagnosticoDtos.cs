using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Prontuario.Application.DTOs;

public class ToggleDiagnosticoRequest
{
    [Required(ErrorMessage = "Diagnóstico inválido.")]
    [JsonPropertyName("chave")]
    public string Chave { get; set; } = null!;

    [JsonPropertyName("ligar")]
    public bool Ligar { get; set; }
}

public class DiagnosticoPacienteDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("paciente_id")] public int PacienteId { get; set; }
    [JsonPropertyName("diagnostico_chave")] public string DiagnosticoChave { get; set; } = null!;
    [JsonPropertyName("ativo")] public bool Ativo { get; set; }
    [JsonPropertyName("criado_em")] public DateTimeOffset CriadoEm { get; set; }
}

public class DiagnosticoAvaliacaoRequest
{
    [JsonPropertyName("data")] public string? Data { get; set; }
    [JsonPropertyName("valores")] public Dictionary<string, object?>? Valores { get; set; }
}

public class DiagnosticoAvaliacaoDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("diagnostico_paciente_id")] public int DiagnosticoPacienteId { get; set; }
    [JsonPropertyName("data")] public string Data { get; set; } = null!;
    [JsonPropertyName("valores")] public Dictionary<string, object?> Valores { get; set; } = new();
    [JsonPropertyName("criado_em")] public DateTimeOffset CriadoEm { get; set; }
}

public class DiagnosticoAtividadeRequest
{
    [Required(ErrorMessage = "Informe a atividade.")]
    [JsonPropertyName("atividade")]
    public string Atividade { get; set; } = null!;

    [JsonPropertyName("data")]
    public string? Data { get; set; }
}

public class DiagnosticoAtividadeDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("diagnostico_paciente_id")] public int DiagnosticoPacienteId { get; set; }
    [JsonPropertyName("atividade")] public string Atividade { get; set; } = null!;
    [JsonPropertyName("data")] public string Data { get; set; } = null!;
    [JsonPropertyName("criado_em")] public DateTimeOffset CriadoEm { get; set; }
}

/// <summary>Resposta de GET /api/pacientes/{id}/diagnosticos/{chave}.</summary>
public class DiagnosticoDetalheResponse
{
    [JsonPropertyName("diagnosticoPaciente")] public DiagnosticoPacienteDto DiagnosticoPaciente { get; set; } = null!;
    [JsonPropertyName("avaliacoes")] public List<DiagnosticoAvaliacaoDto> Avaliacoes { get; set; } = new();
    [JsonPropertyName("atividadesMap")] public Dictionary<string, List<string>> AtividadesMap { get; set; } = new();
}
