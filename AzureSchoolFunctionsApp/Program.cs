using AzureSchoolFunctionsApp.Application.Services;
using AzureSchoolFunctionsApp.Core.Interfaces;
using AzureSchoolFunctionsApp.Infrastructure;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
       //// L�gg till Application Insights f�r funktioner
       //services.AddApplicationInsightsTelemetryWorkerService();
       //services.ConfigureFunctionsApplicationInsights();

        // H�mta milj�variabler fr�n Azure (eller fr�n local.settings.json vid lokal k�rning)
        var tenantId = Environment.GetEnvironmentVariable("TENANT_ID");
        var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
        var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
        var blobConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        // Registrera SharePointService med autentisering
        services.AddSingleton<ISharePointService>(new SharePointService(tenantId, clientId, clientSecret));

        // Registrera BlobStorageService
        services.AddSingleton<IBlobStorageService>(new AzureBlobStorageClient(blobConnectionString));
    })
    .Build();

host.Run();

