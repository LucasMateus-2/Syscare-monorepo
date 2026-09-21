using Microsoft.AspNetCore.Http;
using Prontuario.Application.Interfaces;

namespace Prontuario.API.Common;

public class FormFileUploadedFile : IUploadedFile
{
    private readonly IFormFile _formFile;

    public FormFileUploadedFile(IFormFile formFile)
    {
        _formFile = formFile;
    }

    public string NomeOriginal => _formFile.FileName;
    public long Tamanho => _formFile.Length;
    public Stream AbrirLeitura() => _formFile.OpenReadStream();

    public static IUploadedFile? DeOpcional(IFormFile? formFile) =>
        formFile is null || formFile.Length == 0 ? null : new FormFileUploadedFile(formFile);
}
