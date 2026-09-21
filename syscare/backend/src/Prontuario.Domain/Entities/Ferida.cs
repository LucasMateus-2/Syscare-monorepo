namespace Prontuario.Domain.Entities;

public class Ferida
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public string? NumeroLesao { get; set; }
    public string? Etiologia { get; set; }
    public string? TempoFerida { get; set; }
    public string? Localizacao { get; set; }
    public string? Comprimento { get; set; }
    public string? Largura { get; set; }
    public string? Profundidade { get; set; }
    public string? Descolamento { get; set; }
    public List<string> TimeTecido { get; set; } = new();
    public List<string> TimeInfeccao { get; set; } = new();
    public string? ExsudatoTipo { get; set; }
    public string? ExsudatoQuantidade { get; set; }
    public List<string> Bordas { get; set; } = new();
    public List<string> Perilesional { get; set; } = new();
    public string? Biofilme { get; set; }
    public List<string> BiofilmeSinais { get; set; } = new();
    public Dictionary<string, object?> ItbDados { get; set; } = new();
    public string? Sensibilidade { get; set; }
    public string? SensibilidadeObs { get; set; }
    public string? Foto { get; set; }
    public DateTimeOffset CriadoEm { get; set; } = DateTimeOffset.UtcNow;
    public bool Ativa { get; set; } = true;

    public Paciente? Paciente { get; set; }
    public ICollection<FeridaEvolucao> Evolucoes { get; set; } = new List<FeridaEvolucao>();
}
