using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Coling.Api.Curriculum.Contratos.Repositorios;
using Coling.Api.Curriculum.implementacion.Repositorios;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IInstitucionRepositorio, InstitucionRepositorio>();
    })
    .Build();

host.Run();
