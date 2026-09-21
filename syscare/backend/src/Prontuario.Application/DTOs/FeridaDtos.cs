using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Prontuario.Application.DTOs;

/// <summary>
/// Usa [FromForm(Name=...)] (não [JsonPropertyName]) porque este DTO é vinculado a
/// partir de multipart/form-data, cujo model binder ignora atributos de System.Text.Json.
/// </summary>
public class FeridaFormRequest
{
    [FromForm(Name = "numero_lesao")] public string? NumeroLesao { get; set; }
    [FromForm(Name = "etiologia")] public string? Etiologia { get; set; }
    [FromForm(Name = "tempo_ferida")] public string? TempoFerida { get; set; }
    [FromForm(Name = "localizacao")] public string? Localizacao { get; set; }
    [FromForm(Name = "comprimento")] public string? Comprimento { get; set; }
    [FromForm(Name = "largura")] public string? Largura { get; set; }
    [FromForm(Name = "profundidade")] public string? Profundidade { get; set; }
    [FromForm(Name = "descolamento")] public string? Descolamento { get; set; }
    [FromForm(Name = "time_tecido")] public List<string>? TimeTecido { get; set; }
    [FromForm(Name = "time_infeccao")] public List<string>? TimeInfeccao { get; set; }
    [FromForm(Name = "exsudato_tipo")] public string? ExsudatoTipo { get; set; }
    [FromForm(Name = "exsudato_quantidade")] public string? ExsudatoQuantidade { get; set; }
    [FromForm(Name = "bordas")] public List<string>? Bordas { get; set; }
    [FromForm(Name = "perilesional")] public List<string>? Perilesional { get; set; }
    [FromForm(Name = "biofilme")] public string? Biofilme { get; set; }
    [FromForm(Name = "biofilme_sinais")] public List<string>? BiofilmeSinais { get; set; }
    [FromForm(Name = "sensibilidade")] public string? Sensibilidade { get; set; }
    [FromForm(Name = "sensibilidade_obs")] public string? SensibilidadeObs { get; set; }

    // Campos do ITB (índice tornozelo-braquial), enviados soltos no form original.
    [FromForm(Name = "itb_braco_esq")] public string? ItbBracoEsq { get; set; }
    [FromForm(Name = "itb_braco_dir")] public string? ItbBracoDir { get; set; }
    [FromForm(Name = "itb_tornozelo_dir_tp")] public string? ItbTornozeloDirTp { get; set; }
    [FromForm(Name = "itb_tornozelo_dir_pd")] public string? ItbTornozeloDirPd { get; set; }
    [FromForm(Name = "itb_tornozelo_esq_tp")] public string? ItbTornozeloEsqTp { get; set; }
    [FromForm(Name = "itb_tornozelo_esq_pd")] public string? ItbTornozeloEsqPd { get; set; }
    [FromForm(Name = "itb_resultado_dir")] public string? ItbResultadoDir { get; set; }
    [FromForm(Name = "itb_resultado_esq")] public string? ItbResultadoEsq { get; set; }
}

public class FeridaDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("paciente_id")] public int PacienteId { get; set; }
    [JsonPropertyName("numero_lesao")] public string? NumeroLesao { get; set; }
    [JsonPropertyName("etiologia")] public string? Etiologia { get; set; }
    [JsonPropertyName("tempo_ferida")] public string? TempoFerida { get; set; }
    [JsonPropertyName("localizacao")] public string? Localizacao { get; set; }
    [JsonPropertyName("comprimento")] public string? Comprimento { get; set; }
    [JsonPropertyName("largura")] public string? Largura { get; set; }
    [JsonPropertyName("profundidade")] public string? Profundidade { get; set; }
    [JsonPropertyName("descolamento")] public string? Descolamento { get; set; }
    [JsonPropertyName("time_tecido")] public List<string> TimeTecido { get; set; } = new();
    [JsonPropertyName("time_infeccao")] public List<string> TimeInfeccao { get; set; } = new();
    [JsonPropertyName("exsudato_tipo")] public string? ExsudatoTipo { get; set; }
    [JsonPropertyName("exsudato_quantidade")] public string? ExsudatoQuantidade { get; set; }
    [JsonPropertyName("bordas")] public List<string> Bordas { get; set; } = new();
    [JsonPropertyName("perilesional")] public List<string> Perilesional { get; set; } = new();
    [JsonPropertyName("biofilme")] public string? Biofilme { get; set; }
    [JsonPropertyName("biofilme_sinais")] public List<string> BiofilmeSinais { get; set; } = new();
    [JsonPropertyName("itb_dados")] public Dictionary<string, object?> ItbDados { get; set; } = new();
    [JsonPropertyName("sensibilidade")] public string? Sensibilidade { get; set; }
    [JsonPropertyName("sensibilidade_obs")] public string? SensibilidadeObs { get; set; }
    [JsonPropertyName("foto")] public string? Foto { get; set; }
    [JsonPropertyName("criado_em")] public DateTimeOffset CriadoEm { get; set; }
    [JsonPropertyName("ativa")] public bool Ativa { get; set; }
}

public class FeridaEvolucaoRequest
{
    [FromForm(Name = "data")] public string? Data { get; set; }
    [FromForm(Name = "comprimento")] public string? Comprimento { get; set; }
    [FromForm(Name = "largura")] public string? Largura { get; set; }
    [FromForm(Name = "profundidade")] public string? Profundidade { get; set; }
    [FromForm(Name = "exsudato_quantidade")] public string? ExsudatoQuantidade { get; set; }
    [FromForm(Name = "observacoes")] public string? Observacoes { get; set; }
}

public class FeridaEvolucaoDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("ferida_id")] public int FeridaId { get; set; }
    [JsonPropertyName("data")] public string Data { get; set; } = null!;
    [JsonPropertyName("comprimento")] public string? Comprimento { get; set; }
    [JsonPropertyName("largura")] public string? Largura { get; set; }
    [JsonPropertyName("profundidade")] public string? Profundidade { get; set; }
    [JsonPropertyName("exsudato_quantidade")] public string? ExsudatoQuantidade { get; set; }
    [JsonPropertyName("observacoes")] public string? Observacoes { get; set; }
    [JsonPropertyName("foto")] public string? Foto { get; set; }
    [JsonPropertyName("criado_em")] public DateTimeOffset CriadoEm { get; set; }
}

public class FeridaComEvolucoesResponse
{
    [JsonPropertyName("ferida")] public FeridaDto Ferida { get; set; } = null!;
    [JsonPropertyName("evolucoes")] public List<FeridaEvolucaoDto> Evolucoes { get; set; } = new();
}
