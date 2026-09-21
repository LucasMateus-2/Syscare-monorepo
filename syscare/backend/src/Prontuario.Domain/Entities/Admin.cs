namespace Prontuario.Domain.Entities;

public class Admin
{
    public int Id { get; set; }
    public string Usuario { get; set; } = null!;
    public string SenhaHash { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public DateTimeOffset CriadoEm { get; set; } = DateTimeOffset.UtcNow;
}
