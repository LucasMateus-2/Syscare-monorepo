using Microsoft.EntityFrameworkCore;
using Prontuario.Application.Common;
using Prontuario.Application.DTOs;
using Prontuario.Application.Interfaces;
using Prontuario.Domain.Common;
using Prontuario.Domain.Entities;

namespace Prontuario.Application.UseCases;

public class FeridaUseCase : IFeridaUseCase
{
    private const string SubpastaFotos = "feridas";

    private readonly IApplicationDbContext _db;
    private readonly IFileStorageService _fileStorage;

    public FeridaUseCase(IApplicationDbContext db, IFileStorageService fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task<FeridaDto> RegistrarAsync(int pacienteId, FeridaFormRequest request, IUploadedFile? foto, CancellationToken cancellationToken = default)
    {
        await ExigirPacienteAsync(pacienteId, cancellationToken);

        string? nomeArquivo = null;
        try
        {
            nomeArquivo = await _fileStorage.SalvarAsync(SubpastaFotos, foto, cancellationToken);

            var itbDados = new Dictionary<string, object?>
            {
                ["braco_esq"] = Formatos.TextoOuNulo(request.ItbBracoEsq),
                ["braco_dir"] = Formatos.TextoOuNulo(request.ItbBracoDir),
                ["tornozelo_dir_tp"] = Formatos.TextoOuNulo(request.ItbTornozeloDirTp),
                ["tornozelo_dir_pd"] = Formatos.TextoOuNulo(request.ItbTornozeloDirPd),
                ["tornozelo_esq_tp"] = Formatos.TextoOuNulo(request.ItbTornozeloEsqTp),
                ["tornozelo_esq_pd"] = Formatos.TextoOuNulo(request.ItbTornozeloEsqPd),
                ["resultado_dir"] = Formatos.TextoOuNulo(request.ItbResultadoDir),
                ["resultado_esq"] = Formatos.TextoOuNulo(request.ItbResultadoEsq),
            };

            var ferida = new Ferida
            {
                PacienteId = pacienteId,
                NumeroLesao = Formatos.TextoOuNulo(request.NumeroLesao),
                Etiologia = Formatos.TextoOuNulo(request.Etiologia),
                TempoFerida = Formatos.TextoOuNulo(request.TempoFerida),
                Localizacao = Formatos.TextoOuNulo(request.Localizacao),
                Comprimento = Formatos.TextoOuNulo(request.Comprimento),
                Largura = Formatos.TextoOuNulo(request.Largura),
                Profundidade = Formatos.TextoOuNulo(request.Profundidade),
                Descolamento = Formatos.TextoOuNulo(request.Descolamento),
                TimeTecido = Formatos.Lista(request.TimeTecido),
                TimeInfeccao = Formatos.Lista(request.TimeInfeccao),
                ExsudatoTipo = Formatos.TextoOuNulo(request.ExsudatoTipo),
                ExsudatoQuantidade = Formatos.TextoOuNulo(request.ExsudatoQuantidade),
                Bordas = Formatos.Lista(request.Bordas),
                Perilesional = Formatos.Lista(request.Perilesional),
                Biofilme = Formatos.TextoOuNulo(request.Biofilme),
                BiofilmeSinais = Formatos.Lista(request.BiofilmeSinais),
                ItbDados = itbDados,
                Sensibilidade = Formatos.TextoOuNulo(request.Sensibilidade),
                SensibilidadeObs = Formatos.TextoOuNulo(request.SensibilidadeObs),
                Foto = nomeArquivo,
            };

            _db.Feridas.Add(ferida);
            await _db.SaveChangesAsync(cancellationToken);
            return ferida.ToDto();
        }
        catch
        {
            _fileStorage.Remover(SubpastaFotos, nomeArquivo);
            throw;
        }
    }

    public async Task<FeridaComEvolucoesResponse> ObterAsync(int pacienteId, int feridaId, CancellationToken cancellationToken = default)
    {
        var ferida = await ExigirFeridaAsync(pacienteId, feridaId, cancellationToken);

        var evolucoes = await _db.FeridaEvolucoes
            .Where(e => e.FeridaId == ferida.Id)
            .OrderByDescending(e => e.Data).ThenByDescending(e => e.Id)
            .ToListAsync(cancellationToken);

        return new FeridaComEvolucoesResponse
        {
            Ferida = ferida.ToDto(),
            Evolucoes = evolucoes.Select(e => e.ToDto()).ToList(),
        };
    }

    public async Task<FeridaEvolucaoDto> RegistrarEvolucaoAsync(int pacienteId, int feridaId, FeridaEvolucaoRequest request, IUploadedFile? foto, CancellationToken cancellationToken = default)
    {
        var ferida = await ExigirFeridaAsync(pacienteId, feridaId, cancellationToken);

        string? nomeArquivo = null;
        try
        {
            nomeArquivo = await _fileStorage.SalvarAsync(SubpastaFotos, foto, cancellationToken);

            var evolucao = new FeridaEvolucao
            {
                FeridaId = ferida.Id,
                Data = Formatos.ParseDataIsoOuHoje(request.Data),
                Comprimento = Formatos.TextoOuNulo(request.Comprimento),
                Largura = Formatos.TextoOuNulo(request.Largura),
                Profundidade = Formatos.TextoOuNulo(request.Profundidade),
                ExsudatoQuantidade = Formatos.TextoOuNulo(request.ExsudatoQuantidade),
                Observacoes = Formatos.TextoOuNulo(request.Observacoes),
                Foto = nomeArquivo,
            };

            _db.FeridaEvolucoes.Add(evolucao);
            await _db.SaveChangesAsync(cancellationToken);
            return evolucao.ToDto();
        }
        catch
        {
            _fileStorage.Remover(SubpastaFotos, nomeArquivo);
            throw;
        }
    }

    public async Task EncerrarAsync(int pacienteId, int feridaId, CancellationToken cancellationToken = default)
    {
        var ferida = await ExigirFeridaAsync(pacienteId, feridaId, cancellationToken);
        ferida.Ativa = false;
        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task ExigirPacienteAsync(int pacienteId, CancellationToken cancellationToken)
    {
        if (pacienteId <= 0) throw AppException.BadRequest("Paciente inválido.");
        var existe = await _db.Pacientes.AnyAsync(p => p.Id == pacienteId, cancellationToken);
        if (!existe) throw AppException.NotFound("Paciente não encontrado.");
    }

    private async Task<Ferida> ExigirFeridaAsync(int pacienteId, int feridaId, CancellationToken cancellationToken)
    {
        if (pacienteId <= 0) throw AppException.BadRequest("Paciente inválido.");
        if (feridaId <= 0) throw AppException.BadRequest("Ferida inválida.");

        var ferida = await _db.Feridas.FirstOrDefaultAsync(f => f.Id == feridaId && f.PacienteId == pacienteId, cancellationToken);
        return ferida ?? throw AppException.NotFound("Ferida não encontrada.");
    }
}
