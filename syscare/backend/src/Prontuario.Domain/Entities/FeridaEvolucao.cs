namespace Prontuario.Domain.Entities;

public class FeridaEvolucao
{
    public int Id { get; set; }
    public int FeridaId { get; set; }
    public DateOnly Data { get; set; }
    public string? Comprimento { get; set; }
    public string? Largura { get; set; }
    public string? Profundidade { get; set; }
    public string? ExsudatoQuantidade { get; set; }
    public string? Observacoes { get; set; }
    public string? Foto { get; set; }
    public DateTimeOffset CriadoEm { get; set; } = DateTimeOffset.UtcNow;

    public Ferida? Ferida { get; set; }
}
