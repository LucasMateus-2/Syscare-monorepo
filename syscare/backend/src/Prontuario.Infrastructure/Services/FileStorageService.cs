using Microsoft.Extensions.Options;
using Prontuario.Application.Interfaces;
using Prontuario.Domain.Common;

namespace Prontuario.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private static readonly HashSet<string> ExtensoesPermitidas = new(StringComparer.OrdinalIgnoreCase)
    {
        ".png", ".jpg", ".jpeg", ".webp", ".gif",
    };

    private readonly UploadOptions _options;

    public FileStorageService(IOptions<UploadOptions> options)
    {
        _options = options.Value;
    }

    public async Task<string?> SalvarAsync(string subpasta, IUploadedFile? arquivo, CancellationToken cancellationToken = default)
    {
        if (arquivo is null) return null;

        if (arquivo.Tamanho > _options.TamanhoMaximoBytes)
            throw AppException.BadRequest("Arquivo excede o tamanho máximo permitido (12 MB).");

        var extensao = Path.GetExtension(arquivo.NomeOriginal);
        if (string.IsNullOrEmpty(extensao) || !ExtensoesPermitidas.Contains(extensao))
            throw AppException.BadRequest("Tipo de arquivo não permitido. Envie uma imagem (png, jpg, jpeg, webp ou gif).");

        var pastaDestino = Path.Combine(_options.RootPath, subpasta);
        Directory.CreateDirectory(pastaDestino);

        var nomeArquivo = $"{Guid.NewGuid():N}{extensao.ToLowerInvariant()}";
        var caminhoCompleto = Path.Combine(pastaDestino, nomeArquivo);

        await using var origem = arquivo.AbrirLeitura();
        await using var destino = File.Create(caminhoCompleto);
        await origem.CopyToAsync(destino, cancellationToken);

        return nomeArquivo;
    }

    public void Remover(string subpasta, string? nomeArquivo)
    {
        if (string.IsNullOrWhiteSpace(nomeArquivo)) return;

        try
        {
            var caminho = Path.Combine(_options.RootPath, subpasta, nomeArquivo);
            if (File.Exists(caminho)) File.Delete(caminho);
        }
        catch
        {
            // Remoção é best-effort: uma falha aqui não deve interromper o fluxo principal.
        }
    }
}
