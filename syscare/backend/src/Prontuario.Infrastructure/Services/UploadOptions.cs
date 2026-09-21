namespace Prontuario.Infrastructure.Services;

public class UploadOptions
{
    public const string SectionName = "Upload";

    /// <summary>Diretório raiz (absoluto ou relativo ao diretório da aplicação) onde os arquivos são salvos.</summary>
    public string RootPath { get; set; } = "uploads";

    /// <summary>Tamanho máximo permitido por arquivo, em bytes (padrão: 12 MB).</summary>
    public long TamanhoMaximoBytes { get; set; } = 12 * 1024 * 1024;
}
