using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Implementacion.Repositorio;
using Coling.API.Curriculum.Implementacion.Repositorios;
using Coling.Utilitarios.Middlewares;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()    
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IInstitucionRepositorio, InstitucionRepositorio>();
        services.AddScoped<IEstudiosRepositorio, EstudiosRepositorio>();
        services.AddScoped<IExperienciaLaboralRepositorio, ExperienciaLaboralRepositorio>();
        services.AddScoped<IProfesionRepositorio, ProfesionRepositorio>();
		services.AddSingleton<JWTMiddleware>();
	}).ConfigureFunctionsWebApplication(x=>
	{
		x.UseMiddleware<JWTMiddleware>();
	}
	)
	.Build();

host.Run();
