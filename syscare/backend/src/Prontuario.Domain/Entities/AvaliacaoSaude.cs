namespace Prontuario.Domain.Entities;

public class AvaliacaoSaude
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public string? DoencaBase { get; set; }
    public List<string> DoencaBaseQuais { get; set; } = new();
    public string? Medicacoes { get; set; }
    public string? MedicacoesQuais { get; set; }
    public string? Alergias { get; set; }
    public string? AlergiasQuais { get; set; }
    public string? Cirurgias { get; set; }
    public string? CirurgiasQuais { get; set; }
    public string? Mobilidade { get; set; }
    public string? MobilidadeObs { get; set; }
    public string? Higiene { get; set; }
    public string? HigieneObs { get; set; }
    public string? CuidadoFerida { get; set; }
    public string? CuidadoFeridaQuem { get; set; }
    public string? Alimentacao { get; set; }
    public string? AtividadeFisica { get; set; }
    public List<string> Habitos { get; set; } = new();
    public List<string> Sono { get; set; } = new();
    public string? SonoMedicacao { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; } = DateTimeOffset.UtcNow;

    public Paciente? Paciente { get; set; }
}
