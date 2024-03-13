using Coling.API.Afiliados;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Coling.API.Afiliados.Contratos;
using Coling.API.Afiliados.Implementacion;
using Coling.Shared;


var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<Contexto>(options => options.UseSqlServer(
                   configuration.GetConnectionString("cadenaConexion")));
        services.AddScoped<IPersonaLogic, PersonaLogic>();
        services.AddScoped<ITelefonoLogic, TelefonoLogic>();
        services.AddScoped<IDireccionLogic, DireccionLogic>();
        services.AddScoped<ITipoSocialLogic, TipoSocialLogic>();
        services.AddScoped<IPersonaTipoSocialLogic, PersonaTipoSocialLogic>();
        services.AddScoped<IAfiliadoLogic, AfiliadoLogic>();
        services.AddScoped<IGradoAcademicoLogic, GradoAcademicoLogic>();
        services.AddScoped<IProfesionLogic, ProfesionLogic>();
        services.AddScoped<IProfesionAfiliadoLogic, ProfesionAfiliadoLogic>();
        services.AddScoped<IIdiomaLogic, IdiomaLogic>();
        services.AddScoped<IIdiomaAfiliadoLogic, IdiomaAfiliadoLogic>();


    })
    .Build();

host.Run();

