namespace Prontuario.Domain.Entities;

public class Agendamento
{
    public static readonly string[] StatusPermitidos =
        { "agendado", "confirmado", "concluido", "cancelado", "faltou" };

    public int Id { get; set; }
    public int? PacienteId { get; set; }
    public string? NomePacienteAvulso { get; set; }
    public string? TelefoneAvulso { get; set; }
    public DateOnly Data { get; set; }
    public string Hora { get; set; } = null!;
    public int DuracaoMin { get; set; } = 30;
    public string? Procedimento { get; set; }
    public string Status { get; set; } = "agendado";
    public string? Observacoes { get; set; }
    public DateTimeOffset CriadoEm { get; set; } = DateTimeOffset.UtcNow;

    public Paciente? Paciente { get; set; }
}
