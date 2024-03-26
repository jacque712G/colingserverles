using Coling.API.Bolsatrabajo.Context;
using Coling.API.Bolsatrabajo.Contratos;
using Coling.API.Bolsatrabajo.Implementacion;
using Coling.Utilitarios.Middlewares;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()   
    .ConfigureServices(services =>
    {
        var configuration = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
       .AddEnvironmentVariables()
       .Build();
        services.AddSingleton<Contexto>();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IInstitucionLogic,InstitucionLogic>();
        services.AddScoped<IOfertaLogic,OfertaLogic>();
        services.AddScoped<ISolicitudLogic,SolicitudLogic>();
		services.AddSingleton<JWTMiddleware>();

	}).ConfigureFunctionsWebApplication(x =>
	{
		x.UseMiddleware<JWTMiddleware>();
	}
	)
	.Build();

host.Run();
