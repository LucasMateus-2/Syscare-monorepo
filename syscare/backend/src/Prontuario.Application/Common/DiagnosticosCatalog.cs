namespace Prontuario.Application.Common;

/// <summary>
/// Chaves válidas de diagnósticos de enfermagem (NANDA/NOC/NIC) que podem ser
/// ativados para um paciente. O conteúdo clínico completo (nomes, indicadores,
/// atividades) é de responsabilidade do front-end; aqui mantemos apenas as
/// chaves para validação, espelhando o comportamento original do backend.
/// </summary>
public static class DiagnosticosCatalog
{
    public static readonly IReadOnlySet<string> ChavesValidas = new HashSet<string>
    {
        "integridade_pele",
        "dor",
        "glicemia",
        "risco_infeccao",
        "ansiedade",
        "autogestao_saude",
        "perfusao_perifericca",
    };

    public static bool EhValida(string? chave) => !string.IsNullOrWhiteSpace(chave) && ChavesValidas.Contains(chave);
}
