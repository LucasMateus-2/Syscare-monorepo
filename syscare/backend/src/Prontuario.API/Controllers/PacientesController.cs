using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prontuario.API.Common;
using Prontuario.Application.DTOs;
using Prontuario.Application.Interfaces;
using Prontuario.Domain.Common;

namespace Prontuario.API.Controllers;

[ApiController]
[Authorize]
[Route("api/pacientes")]
public class PacientesController : ControllerBase
{
    private readonly IPacienteUseCase _pacienteUseCase;
    private readonly IFeridaUseCase _feridaUseCase;
    private readonly IDiagnosticoUseCase _diagnosticoUseCase;
    private readonly IPrescricaoUseCase _prescricaoUseCase;

    public PacientesController(
        IPacienteUseCase pacienteUseCase,
        IFeridaUseCase feridaUseCase,
        IDiagnosticoUseCase diagnosticoUseCase,
        IPrescricaoUseCase prescricaoUseCase)
    {
        _pacienteUseCase = pacienteUseCase;
        _feridaUseCase = feridaUseCase;
        _diagnosticoUseCase = diagnosticoUseCase;
        _prescricaoUseCase = prescricaoUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<List<PacienteDto>>> Listar([FromQuery] string? q, CancellationToken cancellationToken) =>
        Ok(await _pacienteUseCase.ListarAsync(q, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<PacienteDto>> Criar([FromForm] PacienteFormRequest request, IFormFile? foto, CancellationToken cancellationToken)
    {
        var criado = await _pacienteUseCase.CriarAsync(request, FormFileUploadedFile.DeOpcional(foto), cancellationToken);
        return CreatedAtAction(nameof(ObterFicha), new { id = criado.Id }, criado);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FichaPacienteResponse>> ObterFicha(int id, CancellationToken cancellationToken) =>
        Ok(await _pacienteUseCase.ObterFichaAsync(id, cancellationToken));

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PacienteDto>> Atualizar(int id, [FromForm] PacienteFormRequest request, CancellationToken cancellationToken) =>
        Ok(await _pacienteUseCase.AtualizarAsync(id, request, cancellationToken));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id, CancellationToken cancellationToken)
    {
        await _pacienteUseCase.ExcluirAsync(id, cancellationToken);
        return Ok(new OkResponse());
    }

    [HttpPost("{id:int}/foto")]
    public async Task<ActionResult<PacienteDto>> AtualizarFoto(int id, IFormFile? foto, CancellationToken cancellationToken)
    {
        if (foto is null || foto.Length == 0)
            throw AppException.BadRequest("Envie um arquivo de foto.");

        return Ok(await _pacienteUseCase.AtualizarFotoAsync(id, new FormFileUploadedFile(foto), cancellationToken));
    }

    [HttpPut("{id:int}/saude")]
    public async Task<ActionResult<AvaliacaoSaudeDto>> SalvarSaude(int id, [FromBody] AvaliacaoSaudeRequest request, CancellationToken cancellationToken) =>
        Ok(await _pacienteUseCase.SalvarAvaliacaoSaudeAsync(id, request, cancellationToken));

    [HttpPost("{id:int}/exame")]
    public async Task<ActionResult<ExameFisicoDto>> RegistrarExame(int id, [FromBody] ExameFisicoRequest request, CancellationToken cancellationToken) =>
        Ok(await _pacienteUseCase.RegistrarExameFisicoAsync(id, request, cancellationToken));

    [HttpGet("{id:int}/exame/historico")]
    public async Task<ActionResult<List<ExameFisicoDto>>> HistoricoExame(int id, CancellationToken cancellationToken) =>
        Ok(await _pacienteUseCase.ObterHistoricoExameFisicoAsync(id, cancellationToken));

    // --- Feridas ---

    [HttpPost("{id:int}/feridas")]
    public async Task<ActionResult<FeridaDto>> RegistrarFerida(int id, [FromForm] FeridaFormRequest request, IFormFile? foto, CancellationToken cancellationToken)
    {
        var ferida = await _feridaUseCase.RegistrarAsync(id, request, FormFileUploadedFile.DeOpcional(foto), cancellationToken);
        return CreatedAtAction(nameof(ObterFerida), new { id, feridaId = ferida.Id }, ferida);
    }

    [HttpGet("{id:int}/feridas/{feridaId:int}")]
    public async Task<ActionResult<FeridaComEvolucoesResponse>> ObterFerida(int id, int feridaId, CancellationToken cancellationToken) =>
        Ok(await _feridaUseCase.ObterAsync(id, feridaId, cancellationToken));

    [HttpPost("{id:int}/feridas/{feridaId:int}/evolucoes")]
    public async Task<ActionResult<FeridaEvolucaoDto>> RegistrarEvolucao(int id, int feridaId, [FromForm] FeridaEvolucaoRequest request, IFormFile? foto, CancellationToken cancellationToken) =>
        Ok(await _feridaUseCase.RegistrarEvolucaoAsync(id, feridaId, request, FormFileUploadedFile.DeOpcional(foto), cancellationToken));

    [HttpPost("{id:int}/feridas/{feridaId:int}/encerrar")]
    public async Task<IActionResult> EncerrarFerida(int id, int feridaId, CancellationToken cancellationToken)
    {
        await _feridaUseCase.EncerrarAsync(id, feridaId, cancellationToken);
        return Ok(new OkResponse());
    }

    // --- Diagnósticos de enfermagem ---

    [HttpPost("{id:int}/diagnosticos/toggle")]
    public async Task<IActionResult> ToggleDiagnostico(int id, [FromBody] ToggleDiagnosticoRequest request, CancellationToken cancellationToken)
    {
        await _diagnosticoUseCase.ToggleAsync(id, request, cancellationToken);
        return Ok(new OkResponse());
    }

    [HttpGet("{id:int}/diagnosticos/{chave}")]
    public async Task<ActionResult<DiagnosticoDetalheResponse>> ObterDiagnostico(int id, string chave, CancellationToken cancellationToken) =>
        Ok(await _diagnosticoUseCase.ObterAsync(id, chave, cancellationToken));

    [HttpPost("{id:int}/diagnosticos/{chave}/avaliacoes")]
    public async Task<ActionResult<DiagnosticoAvaliacaoDto>> RegistrarAvaliacaoDiagnostico(int id, string chave, [FromBody] DiagnosticoAvaliacaoRequest request, CancellationToken cancellationToken) =>
        Ok(await _diagnosticoUseCase.RegistrarAvaliacaoAsync(id, chave, request, cancellationToken));

    [HttpPost("{id:int}/diagnosticos/{chave}/atividades")]
    public async Task<ActionResult<DiagnosticoAtividadeDto>> RegistrarAtividadeDiagnostico(int id, string chave, [FromBody] DiagnosticoAtividadeRequest request, CancellationToken cancellationToken) =>
        Ok(await _diagnosticoUseCase.RegistrarAtividadeAsync(id, chave, request, cancellationToken));

    // --- Prescrições ---

    [HttpPost("{id:int}/prescricoes")]
    public async Task<ActionResult<PrescricaoDto>> RegistrarPrescricao(int id, [FromBody] PrescricaoRequest request, CancellationToken cancellationToken)
    {
        var prescricao = await _prescricaoUseCase.RegistrarAsync(id, request, cancellationToken);
        return CreatedAtAction(nameof(ObterPrescricao), new { id, prescricaoId = prescricao.Id }, prescricao);
    }

    [HttpGet("{id:int}/prescricoes/{prescricaoId:int}")]
    public async Task<ActionResult<PrescricaoComPacienteResponse>> ObterPrescricao(int id, int prescricaoId, CancellationToken cancellationToken) =>
        Ok(await _prescricaoUseCase.ObterAsync(id, prescricaoId, cancellationToken));
}
