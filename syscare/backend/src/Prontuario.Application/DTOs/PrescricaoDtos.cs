using System.Text.Json.Serialization;

namespace Prontuario.Application.DTOs;

public class PrescricaoRequest
{
    [JsonPropertyName("data")] public string? Data { get; set; }
    [JsonPropertyName("aos_cuidados_de")] public string? AosCuidadosDe { get; set; }
    [JsonPropertyName("limpeza_sf09")] public bool LimpezaSf09 { get; set; }
    [JsonPropertyName("phmb")] public bool Phmb { get; set; }
    [JsonPropertyName("creme_barreira")] public bool CremeBarreira { get; set; }
    [JsonPropertyName("cobertura")] public string? Cobertura { get; set; }
    [JsonPropertyName("cobrir_com")] public List<string>? CobrirCom { get; set; }
    [JsonPropertyName("frequencia_troca")] public string? FrequenciaTroca { get; set; }
    [JsonPropertyName("observacoes")] public string? Observacoes { get; set; }
}

public class PrescricaoDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("paciente_id")] public int PacienteId { get; set; }
    [JsonPropertyName("data")] public string? Data { get; set; }
    [JsonPropertyName("aos_cuidados_de")] public string? AosCuidadosDe { get; set; }
    [JsonPropertyName("limpeza_sf09")] public bool LimpezaSf09 { get; set; }
    [JsonPropertyName("phmb")] public bool Phmb { get; set; }
    [JsonPropertyName("creme_barreira")] public bool CremeBarreira { get; set; }
    [JsonPropertyName("cobertura")] public string? Cobertura { get; set; }
    [JsonPropertyName("cobrir_com")] public List<string> CobrirCom { get; set; } = new();
    [JsonPropertyName("frequencia_troca")] public string? FrequenciaTroca { get; set; }
    [JsonPropertyName("observacoes")] public string? Observacoes { get; set; }
    [JsonPropertyName("criado_em")] public DateTimeOffset CriadoEm { get; set; }
}

public class PrescricaoComPacienteResponse
{
    [JsonPropertyName("paciente")] public PacienteDto Paciente { get; set; } = null!;
    [JsonPropertyName("prescricao")] public PrescricaoDto Prescricao { get; set; } = null!;
}
