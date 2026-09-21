namespace Prontuario.Domain.Entities;

public class DiagnosticoPaciente
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public string DiagnosticoChave { get; set; } = null!;
    public bool Ativo { get; set; } = true;
    public DateTimeOffset CriadoEm { get; set; } = DateTimeOffset.UtcNow;

    public Paciente? Paciente { get; set; }
    public ICollection<DiagnosticoAvaliacao> Avaliacoes { get; set; } = new List<DiagnosticoAvaliacao>();
    public ICollection<DiagnosticoAtividade> Atividades { get; set; } = new List<DiagnosticoAtividade>();
}
