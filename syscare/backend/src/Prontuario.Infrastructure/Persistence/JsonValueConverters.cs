using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Prontuario.Infrastructure.Persistence;

/// <summary>
/// Conversores para mapear List&lt;string&gt; e Dictionary&lt;string, object?&gt; em
/// colunas Postgres do tipo jsonb, reproduzindo os campos JSONB do schema
/// original (ex.: doenca_base_quais, habitos, itb_dados, valores).
/// </summary>
public static class JsonValueConverters
{
    private static readonly JsonSerializerOptions Options = new();

    public static readonly ValueConverter<List<string>, string> StringListConverter = new(
        v => JsonSerializer.Serialize(v, Options),
        v => string.IsNullOrWhiteSpace(v)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(v, Options) ?? new List<string>());

    public static readonly ValueComparer<List<string>> StringListComparer = new(
        (a, b) => (a ?? new()).SequenceEqual(b ?? new()),
        v => v.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode())),
        v => v.ToList());

    public static readonly ValueConverter<Dictionary<string, object?>, string> ObjectMapConverter = new(
        v => JsonSerializer.Serialize(v, Options),
        v => string.IsNullOrWhiteSpace(v)
            ? new Dictionary<string, object?>()
            : JsonSerializer.Deserialize<Dictionary<string, object?>>(v, Options) ?? new Dictionary<string, object?>());

    public static readonly ValueComparer<Dictionary<string, object?>> ObjectMapComparer = new(
        (a, b) => JsonSerializer.Serialize(a, Options) == JsonSerializer.Serialize(b, Options),
        v => JsonSerializer.Serialize(v, Options).GetHashCode(),
        v => new Dictionary<string, object?>(v));
}
