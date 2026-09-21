namespace Prontuario.Application.Interfaces;

/// <summary>
/// Representa um arquivo recebido de fora da camada Application (ex.: um
/// IFormFile adaptado na API), sem acoplar a Application ao ASP.NET Core.
/// </summary>
public interface IUploadedFile
{
    string NomeOriginal { get; }
    long Tamanho { get; }
    Stream AbrirLeitura();
}

public interface IFileStorageService
{
    /// <summary>Salva o arquivo na subpasta informada e retorna o nome gerado, ou null se não houver arquivo.</summary>
    Task<string?> SalvarAsync(string subpasta, IUploadedFile? arquivo, CancellationToken cancellationToken = default);

    /// <summary>Remove (best-effort) um arquivo previamente salvo.</summary>
    void Remover(string subpasta, string? nomeArquivo);
}
