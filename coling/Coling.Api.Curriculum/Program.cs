using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Coling.Api.Curriculum.Contratos.Repositorios;
using Coling.Api.Curriculum.implementacion.Repositorios;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Coling.Utilitarios.Middleware;

var host = new HostBuilder()
    //.ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IInstitucionRepositorio, InstitucionRepositorio>();
        services.AddScoped<IEstudiosRepositorio, EstudiosImplementacion>();
        services.AddScoped<IExperienciaRepositorio,ExperienciaImplementacion>();
        //
        services.AddSingleton<JwtMiddleware>();
    }).ConfigureFunctionsWebApplication(x =>
    {
        x.UseMiddleware<JwtMiddleware>();
    })
    .Build();

host.Run();
