namespace Prontuario.Domain.Entities;

public class Paciente
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public DateOnly? DataNascimento { get; set; }
    public string? Sexo { get; set; }
    public string? EstadoCivil { get; set; }
    public string? Ocupacao { get; set; }
    public string? NumeroFilhos { get; set; }
    public string? Religiao { get; set; }
    public string? Telefone { get; set; }
    public string? Endereco { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; }
    public string? Escolaridade { get; set; }
    public string? UbsReferencia { get; set; }
    public string? ConvenioParticular { get; set; }
    public string? PassaPoliclinica { get; set; }
    public string? Foto { get; set; }
    public DateTimeOffset CriadoEm { get; set; } = DateTimeOffset.UtcNow;
    public bool Ativo { get; set; } = true;

    public AvaliacaoSaude? AvaliacaoSaude { get; set; }
    public ICollection<ExameFisico> ExamesFisicos { get; set; } = new List<ExameFisico>();
    public ICollection<Ferida> Feridas { get; set; } = new List<Ferida>();
    public ICollection<DiagnosticoPaciente> Diagnosticos { get; set; } = new List<DiagnosticoPaciente>();
    public ICollection<Prescricao> Prescricoes { get; set; } = new List<Prescricao>();
    public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
}
