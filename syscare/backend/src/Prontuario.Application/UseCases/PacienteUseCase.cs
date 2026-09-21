using Microsoft.EntityFrameworkCore;
using Prontuario.Application.Common;
using Prontuario.Application.DTOs;
using Prontuario.Application.Interfaces;
using Prontuario.Domain.Common;
using Prontuario.Domain.Entities;

namespace Prontuario.Application.UseCases;

public class PacienteUseCase : IPacienteUseCase
{
    private const string SubpastaFotos = "pacientes";

    private readonly IApplicationDbContext _db;
    private readonly IFileStorageService _fileStorage;

    public PacienteUseCase(IApplicationDbContext db, IFileStorageService fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    public async Task<List<PacienteDto>> ListarAsync(string? busca, CancellationToken cancellationToken = default)
    {
        var termo = (busca ?? string.Empty).Trim();
        var query = _db.Pacientes.Where(p => p.Ativo);
        if (!string.IsNullOrEmpty(termo))
            query = query.Where(p => EF.Functions.Like(p.Nome, $"%{termo}%"));

        var pacientes = await query.OrderBy(p => p.Nome).ToListAsync(cancellationToken);
        return pacientes.Select(p => p.ToDto()).ToList();
    }

    public async Task<PacienteDto> CriarAsync(PacienteFormRequest request, IUploadedFile? foto, CancellationToken cancellationToken = default)
    {
        string? nomeArquivo = null;
        try
        {
            nomeArquivo = await _fileStorage.SalvarAsync(SubpastaFotos, foto, cancellationToken);

            var paciente = new Paciente
            {
                Nome = request.Nome.Trim(),
                DataNascimento = ParseDataNascimento(request.DataNascimento),
                Sexo = Formatos.TextoOuNulo(request.Sexo),
                EstadoCivil = Formatos.TextoOuNulo(request.EstadoCivil),
                Ocupacao = Formatos.TextoOuNulo(request.Ocupacao),
                NumeroFilhos = Formatos.TextoOuNulo(request.NumeroFilhos),
                Religiao = Formatos.TextoOuNulo(request.Religiao),
                Telefone = Formatos.TextoOuNulo(request.Telefone),
                Endereco = Formatos.TextoOuNulo(request.Endereco),
                Numero = Formatos.TextoOuNulo(request.Numero),
                Bairro = Formatos.TextoOuNulo(request.Bairro),
                Escolaridade = Formatos.TextoOuNulo(request.Escolaridade),
                UbsReferencia = Formatos.TextoOuNulo(request.UbsReferencia),
                ConvenioParticular = Formatos.TextoOuNulo(request.ConvenioParticular),
                PassaPoliclinica = Formatos.TextoOuNulo(request.PassaPoliclinica),
                Foto = nomeArquivo,
            };

            _db.Pacientes.Add(paciente);
            await _db.SaveChangesAsync(cancellationToken);
            return paciente.ToDto();
        }
        catch
        {
            _fileStorage.Remover(SubpastaFotos, nomeArquivo);
            throw;
        }
    }

    public async Task<FichaPacienteResponse> ObterFichaAsync(int id, CancellationToken cancellationToken = default)
    {
        var paciente = await ExigirPacienteAsync(id, incluirInativo: true, cancellationToken);

        var avaliacaoSaude = await _db.AvaliacoesSaude
            .FirstOrDefaultAsync(a => a.PacienteId == id, cancellationToken);

        var exameFisico = await _db.ExamesFisicos
            .Where(e => e.PacienteId == id)
            .OrderByDescending(e => e.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var feridas = await _db.Feridas
            .Where(f => f.PacienteId == id)
            .OrderByDescending(f => f.Ativa).ThenByDescending(f => f.CriadoEm)
            .ToListAsync(cancellationToken);

        var diagnosticosAtivos = await _db.DiagnosticosPaciente
            .Where(d => d.PacienteId == id && d.Ativo)
            .OrderBy(d => d.Id)
            .Select(d => d.DiagnosticoChave)
            .ToListAsync(cancellationToken);

        var prescricoes = await _db.Prescricoes
            .Where(p => p.PacienteId == id)
            .OrderByDescending(p => p.CriadoEm)
            .ToListAsync(cancellationToken);

        return new FichaPacienteResponse
        {
            Paciente = paciente.ToDto(),
            AvaliacaoSaude = avaliacaoSaude?.ToDto(),
            ExameFisico = exameFisico?.ToDto(),
            Feridas = feridas.Select(f => f.ToDto()).ToList(),
            DiagnosticosAtivos = diagnosticosAtivos,
            Prescricoes = prescricoes.Select(p => p.ToDto()).ToList(),
        };
    }

    public async Task<PacienteDto> AtualizarAsync(int id, PacienteFormRequest request, CancellationToken cancellationToken = default)
    {
        var paciente = await ExigirPacienteAsync(id, incluirInativo: true, cancellationToken);

        paciente.Nome = request.Nome.Trim();
        paciente.DataNascimento = ParseDataNascimento(request.DataNascimento);
        paciente.Sexo = Formatos.TextoOuNulo(request.Sexo);
        paciente.EstadoCivil = Formatos.TextoOuNulo(request.EstadoCivil);
        paciente.Ocupacao = Formatos.TextoOuNulo(request.Ocupacao);
        paciente.NumeroFilhos = Formatos.TextoOuNulo(request.NumeroFilhos);
        paciente.Religiao = Formatos.TextoOuNulo(request.Religiao);
        paciente.Telefone = Formatos.TextoOuNulo(request.Telefone);
        paciente.Endereco = Formatos.TextoOuNulo(request.Endereco);
        paciente.Numero = Formatos.TextoOuNulo(request.Numero);
        paciente.Bairro = Formatos.TextoOuNulo(request.Bairro);
        paciente.Escolaridade = Formatos.TextoOuNulo(request.Escolaridade);
        paciente.UbsReferencia = Formatos.TextoOuNulo(request.UbsReferencia);
        paciente.ConvenioParticular = Formatos.TextoOuNulo(request.ConvenioParticular);
        paciente.PassaPoliclinica = Formatos.TextoOuNulo(request.PassaPoliclinica);

        await _db.SaveChangesAsync(cancellationToken);
        return paciente.ToDto();
    }

    public async Task ExcluirAsync(int id, CancellationToken cancellationToken = default)
    {
        var paciente = await ExigirPacienteAsync(id, incluirInativo: true, cancellationToken);
        paciente.Ativo = false;
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<PacienteDto> AtualizarFotoAsync(int id, IUploadedFile foto, CancellationToken cancellationToken = default)
    {
        var paciente = await ExigirPacienteAsync(id, incluirInativo: true, cancellationToken);
        var fotoAnterior = paciente.Foto;
        var nomeArquivo = await _fileStorage.SalvarAsync(SubpastaFotos, foto, cancellationToken);

        try
        {
            paciente.Foto = nomeArquivo;
            await _db.SaveChangesAsync(cancellationToken);
            if (!string.IsNullOrEmpty(fotoAnterior)) _fileStorage.Remover(SubpastaFotos, fotoAnterior);
            return paciente.ToDto();
        }
        catch
        {
            _fileStorage.Remover(SubpastaFotos, nomeArquivo);
            throw;
        }
    }

    public async Task<AvaliacaoSaudeDto> SalvarAvaliacaoSaudeAsync(int id, AvaliacaoSaudeRequest request, CancellationToken cancellationToken = default)
    {
        await ExigirPacienteAsync(id, incluirInativo: true, cancellationToken);

        var avaliacao = await _db.AvaliacoesSaude.FirstOrDefaultAsync(a => a.PacienteId == id, cancellationToken);
        if (avaliacao is null)
        {
            avaliacao = new AvaliacaoSaude { PacienteId = id };
            _db.AvaliacoesSaude.Add(avaliacao);
        }

        avaliacao.DoencaBase = Formatos.TextoOuNulo(request.DoencaBase);
        avaliacao.DoencaBaseQuais = Formatos.Lista(request.DoencaBaseQuais);
        avaliacao.Medicacoes = Formatos.TextoOuNulo(request.Medicacoes);
        avaliacao.MedicacoesQuais = Formatos.TextoOuNulo(request.MedicacoesQuais);
        avaliacao.Alergias = Formatos.TextoOuNulo(request.Alergias);
        avaliacao.AlergiasQuais = Formatos.TextoOuNulo(request.AlergiasQuais);
        avaliacao.Cirurgias = Formatos.TextoOuNulo(request.Cirurgias);
        avaliacao.CirurgiasQuais = Formatos.TextoOuNulo(request.CirurgiasQuais);
        avaliacao.Mobilidade = Formatos.TextoOuNulo(request.Mobilidade);
        avaliacao.MobilidadeObs = Formatos.TextoOuNulo(request.MobilidadeObs);
        avaliacao.Higiene = Formatos.TextoOuNulo(request.Higiene);
        avaliacao.HigieneObs = Formatos.TextoOuNulo(request.HigieneObs);
        avaliacao.CuidadoFerida = Formatos.TextoOuNulo(request.CuidadoFerida);
        avaliacao.CuidadoFeridaQuem = Formatos.TextoOuNulo(request.CuidadoFeridaQuem);
        avaliacao.Alimentacao = Formatos.TextoOuNulo(request.Alimentacao);
        avaliacao.AtividadeFisica = Formatos.TextoOuNulo(request.AtividadeFisica);
        avaliacao.Habitos = Formatos.Lista(request.Habitos);
        avaliacao.Sono = Formatos.Lista(request.Sono);
        avaliacao.SonoMedicacao = Formatos.TextoOuNulo(request.SonoMedicacao);
        avaliacao.AtualizadoEm = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        return avaliacao.ToDto();
    }

    public async Task<ExameFisicoDto> RegistrarExameFisicoAsync(int id, ExameFisicoRequest request, CancellationToken cancellationToken = default)
    {
        await ExigirPacienteAsync(id, incluirInativo: true, cancellationToken);

        var exame = new ExameFisico
        {
            PacienteId = id,
            Dados = request.Dados ?? new Dictionary<string, object?>(),
        };

        _db.ExamesFisicos.Add(exame);
        await _db.SaveChangesAsync(cancellationToken);
        return exame.ToDto();
    }

    public async Task<List<ExameFisicoDto>> ObterHistoricoExameFisicoAsync(int id, CancellationToken cancellationToken = default)
    {
        await ExigirPacienteAsync(id, incluirInativo: true, cancellationToken);

        var historico = await _db.ExamesFisicos
            .Where(e => e.PacienteId == id)
            .OrderByDescending(e => e.Id)
            .ToListAsync(cancellationToken);

        return historico.Select(e => e.ToDto()).ToList();
    }

    private async Task<Paciente> ExigirPacienteAsync(int id, bool incluirInativo, CancellationToken cancellationToken)
    {
        if (id <= 0) throw AppException.BadRequest("Paciente inválido.");

        var query = _db.Pacientes.Where(p => p.Id == id);
        if (!incluirInativo) query = query.Where(p => p.Ativo);

        var paciente = await query.FirstOrDefaultAsync(cancellationToken);
        return paciente ?? throw AppException.NotFound("Paciente não encontrado.");
    }

    private static DateOnly? ParseDataNascimento(string? valor)
    {
        var texto = Formatos.TextoOuNulo(valor);
        return texto is null ? null : Formatos.ParseDataIso(texto, "Data de nascimento");
    }
}
