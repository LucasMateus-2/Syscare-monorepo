namespace Prontuario.Domain.Entities;

public class DiagnosticoAtividade
{
    public int Id { get; set; }
    public int DiagnosticoPacienteId { get; set; }
    public string Atividade { get; set; } = null!;
    public DateOnly Data { get; set; }
    public DateTimeOffset CriadoEm { get; set; } = DateTimeOffset.UtcNow;

    public DiagnosticoPaciente? DiagnosticoPaciente { get; set; }
}
