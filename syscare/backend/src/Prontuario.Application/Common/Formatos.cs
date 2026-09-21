using System.Globalization;
using Prontuario.Domain.Common;

namespace Prontuario.Application.Common;

/// <summary>
/// Pequenos helpers de parsing/normalização usados pelos casos de uso.
/// A validação de forma (obrigatoriedade, regex) já acontece via Data
/// Annotations nos DTOs; aqui tratamos apenas conversões que Data
/// Annotations não expressam bem (data de calendário real, faixas de hora).
/// </summary>
public static class Formatos
{
    public static DateOnly ParseDataIso(string valor, string nome = "Data")
    {
        if (!DateOnly.TryParseExact(valor, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var data))
            throw AppException.BadRequest($"{nome} deve estar no formato AAAA-MM-DD e ser uma data válida.");
        return data;
    }

    public static DateOnly ParseDataIsoOuHoje(string? valor, string nome = "Data") =>
        string.IsNullOrWhiteSpace(valor) ? DateOnly.FromDateTime(DateTime.UtcNow) : ParseDataIso(valor, nome);

    public static string ParseHora(string valor)
    {
        var texto = (valor ?? string.Empty).Trim();
        var partes = texto.Split(':');
        if (partes.Length < 2 || !int.TryParse(partes[0], out var h) || !int.TryParse(partes[1], out var m))
            throw AppException.BadRequest("Hora deve estar no formato HH:MM.");
        if (h is < 0 or > 23 || m is < 0 or > 59)
            throw AppException.BadRequest("Hora inválida.");
        return $"{h:D2}:{m:D2}";
    }

    public static string? TextoOuNulo(string? valor)
    {
        if (valor is null) return null;
        var texto = valor.Trim();
        return texto.Length == 0 ? null : texto;
    }

    public static List<string> Lista(IEnumerable<string>? valores) =>
        (valores ?? Enumerable.Empty<string>()).Where(v => !string.IsNullOrEmpty(v)).ToList();
}
