namespace Prontuario.Domain.Entities;

public class Prescricao
{
    public int Id { get; set; }
    public int PacienteId { get; set; }
    public DateOnly? Data { get; set; }
    public string? AosCuidadosDe { get; set; }
    public bool LimpezaSf09 { get; set; }
    public bool Phmb { get; set; }
    public bool CremeBarreira { get; set; }
    public string? Cobertura { get; set; }
    public List<string> CobrirCom { get; set; } = new();
    public string? FrequenciaTroca { get; set; }
    public string? Observacoes { get; set; }
    public DateTimeOffset CriadoEm { get; set; } = DateTimeOffset.UtcNow;

    public Paciente? Paciente { get; set; }
}
