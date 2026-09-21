using Microsoft.Extensions.DependencyInjection;
using Prontuario.Application.Interfaces;
using Prontuario.Application.UseCases;

namespace Prontuario.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthUseCase, AuthUseCase>();
        services.AddScoped<IPacienteUseCase, PacienteUseCase>();
        services.AddScoped<IFeridaUseCase, FeridaUseCase>();
        services.AddScoped<IDiagnosticoUseCase, DiagnosticoUseCase>();
        services.AddScoped<IPrescricaoUseCase, PrescricaoUseCase>();
        services.AddScoped<IAgendamentoUseCase, AgendamentoUseCase>();
        return services;
    }
}
