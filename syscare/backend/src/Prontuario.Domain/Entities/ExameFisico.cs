namespace Prontuario.Domain.Entities;

public class ExameFisico
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public Dictionary<string, object?> Dados { get; set; } = new();
    public DateTimeOffset CriadoEm { get; set; } = DateTimeOffset.UtcNow;

    public Paciente? Paciente { get; set; }
}
