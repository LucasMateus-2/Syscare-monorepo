using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Prontuario.Application.DTOs;

public class ConfigurarRequest
{
    [JsonPropertyName("nome")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "Informe um nome de usuário.")]
    [JsonPropertyName("usuario")]
    public string Usuario { get; set; } = null!;

    [Required(ErrorMessage = "Informe uma senha.")]
    [MinLength(4, ErrorMessage = "A senha deve ter pelo menos 4 caracteres.")]
    [JsonPropertyName("senha")]
    public string Senha { get; set; } = null!;
}

public class LoginRequest
{
    [Required(ErrorMessage = "Informe o nome de usuário.")]
    [JsonPropertyName("usuario")]
    public string Usuario { get; set; } = null!;

    [JsonPropertyName("senha")]
    public string Senha { get; set; } = string.Empty;
}

public class AdminDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("usuario")]
    public string Usuario { get; set; } = null!;

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = null!;
}

public class AuthStatusResponse
{
    [JsonPropertyName("hasAdmin")]
    public bool HasAdmin { get; set; }

    [JsonPropertyName("loggedIn")]
    public bool LoggedIn { get; set; }

    [JsonPropertyName("admin")]
    public AdminDto? Admin { get; set; }
}

public class AuthResponse
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; } = true;

    [JsonPropertyName("admin")]
    public AdminDto Admin { get; set; } = null!;
}

public class OkResponse
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; } = true;
}
