namespace Prontuario.Domain.Entities;

public class DiagnosticoAvaliacao
{
    public int Id { get; set; }
    public int DiagnosticoPacienteId { get; set; }
    public DateOnly Data { get; set; }
    public Dictionary<string, object?> Valores { get; set; } = new();
    public DateTimeOffset CriadoEm { get; set; } = DateTimeOffset.UtcNow;

    public DiagnosticoPaciente? DiagnosticoPaciente { get; set; }
}
