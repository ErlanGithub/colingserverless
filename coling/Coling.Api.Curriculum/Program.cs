using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Coling.Api.Curriculum.Contratos.Repositorios;
using Coling.Api.Curriculum.implementacion.Repositorios;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(/*worker=>worker.UseNewtonsoftJson()*/)
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IInstitucionRepositorio, InstitucionRepositorio>();
        services.AddScoped<IEstudiosRepositorio, EstudiosImplementacion>();
        services.AddScoped<IExperienciaRepositorio,ExperienciaImplementacion>();
    })
    .Build();

host.Run();
