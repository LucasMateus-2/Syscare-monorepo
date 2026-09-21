using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Prontuario.Application.DTOs;

public class AgendamentoRequest
{
    [Required(ErrorMessage = "Data deve estar no formato AAAA-MM-DD.")]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Data deve estar no formato AAAA-MM-DD.")]
    [JsonPropertyName("data")]
    public string Data { get; set; } = null!;

    [Required(ErrorMessage = "Informe a hora.")]
    [JsonPropertyName("hora")]
    public string Hora { get; set; } = null!;

    [JsonPropertyName("duracao_min")]
    public int? DuracaoMin { get; set; }

    [JsonPropertyName("paciente_id")]
    public int? PacienteId { get; set; }

    [JsonPropertyName("nome_paciente_avulso")]
    public string? NomePacienteAvulso { get; set; }

    [JsonPropertyName("telefone_avulso")]
    public string? TelefoneAvulso { get; set; }

    [JsonPropertyName("procedimento")]
    public string? Procedimento { get; set; }

    [JsonPropertyName("observacoes")]
    public string? Observacoes { get; set; }
}

public class AgendamentoStatusRequest
{
    [Required(ErrorMessage = "Informe o status.")]
    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;
}

public class AgendamentoDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("paciente_id")] public int? PacienteId { get; set; }
    [JsonPropertyName("nome_paciente_avulso")] public string? NomePacienteAvulso { get; set; }
    [JsonPropertyName("telefone_avulso")] public string? TelefoneAvulso { get; set; }
    [JsonPropertyName("data")] public string Data { get; set; } = null!;
    [JsonPropertyName("hora")] public string Hora { get; set; } = null!;
    [JsonPropertyName("duracao_min")] public int DuracaoMin { get; set; }
    [JsonPropertyName("procedimento")] public string? Procedimento { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; } = null!;
    [JsonPropertyName("observacoes")] public string? Observacoes { get; set; }
    [JsonPropertyName("criado_em")] public DateTimeOffset CriadoEm { get; set; }

    // Presentes apenas na listagem do dia (join com paciente).
    [JsonPropertyName("paciente_nome")] public string? PacienteNome { get; set; }
    [JsonPropertyName("paciente_foto")] public string? PacienteFoto { get; set; }
    [JsonPropertyName("paciente_telefone")] public string? PacienteTelefone { get; set; }
}

public class AgendaDoDiaResponse
{
    [JsonPropertyName("data")] public string Data { get; set; } = null!;
    [JsonPropertyName("agendamentos")] public List<AgendamentoDto> Agendamentos { get; set; } = new();
    [JsonPropertyName("contagemPorDia")] public Dictionary<string, int> ContagemPorDia { get; set; } = new();
}

public class PacienteBuscaDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("nome")] public string Nome { get; set; } = null!;
    [JsonPropertyName("telefone")] public string? Telefone { get; set; }
    [JsonPropertyName("foto")] public string? Foto { get; set; }
}
