using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prontuario.Application.DTOs;
using Prontuario.Application.Interfaces;

namespace Prontuario.API.Controllers;

[ApiController]
[Authorize]
[Route("api/agenda")]
public class AgendaController : ControllerBase
{
    private readonly IAgendamentoUseCase _agendamentoUseCase;

    public AgendaController(IAgendamentoUseCase agendamentoUseCase)
    {
        _agendamentoUseCase = agendamentoUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<AgendaDoDiaResponse>> ObterDoDia([FromQuery] string? data, CancellationToken cancellationToken) =>
        Ok(await _agendamentoUseCase.ObterAgendaDoDiaAsync(data, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<AgendamentoDto>> Criar([FromBody] AgendamentoRequest request, CancellationToken cancellationToken)
    {
        var criado = await _agendamentoUseCase.CriarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObterDoDia), new { data = criado.Data }, criado);
    }

    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<AgendamentoDto>> AtualizarStatus(int id, [FromBody] AgendamentoStatusRequest request, CancellationToken cancellationToken) =>
        Ok(await _agendamentoUseCase.AtualizarStatusAsync(id, request, cancellationToken));

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AgendamentoDto>> Atualizar(int id, [FromBody] AgendamentoRequest request, CancellationToken cancellationToken) =>
        Ok(await _agendamentoUseCase.AtualizarAsync(id, request, cancellationToken));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id, CancellationToken cancellationToken)
    {
        await _agendamentoUseCase.ExcluirAsync(id, cancellationToken);
        return Ok(new OkResponse());
    }

    [HttpGet("buscar-pacientes")]
    public async Task<ActionResult<List<PacienteBuscaDto>>> BuscarPacientes([FromQuery] string? q, CancellationToken cancellationToken) =>
        Ok(await _agendamentoUseCase.BuscarPacientesAsync(q, cancellationToken));

    [HttpGet("paciente/{pacienteId:int}")]
    public async Task<ActionResult<List<AgendamentoDto>>> ListarPorPaciente(int pacienteId, CancellationToken cancellationToken) =>
        Ok(await _agendamentoUseCase.ListarProximosPorPacienteAsync(pacienteId, cancellationToken));
}
