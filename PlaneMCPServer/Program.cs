using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.Sources.Clear();

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

var planeApiKey = builder.Configuration["PlaneAPIKey"];
var baseUrl = builder.Configuration["BaseUrl"];
var workSpace = builder.Configuration["WorkSpace"];
var projectId = builder.Configuration["ProjectId"];

if(string.IsNullOrEmpty(planeApiKey))
    throw new InvalidOperationException("Plane API key is missing from config");
if(string.IsNullOrEmpty(baseUrl))
    throw new InvalidOperationException("Base url is missing from config");
if(string.IsNullOrEmpty(workSpace))
    throw new InvalidOperationException("Work space is missing from config");
if(string.IsNullOrEmpty(projectId))
    throw new InvalidOperationException("Project id is missing from config");

builder.Services.AddHttpClient();

builder.Services.AddSingleton(sp =>
{
    var httpclientFac = sp.GetRequiredService<IHttpClientFactory>();
    return new PlaneAPIService(httpclientFac, baseUrl, workSpace, projectId, planeApiKey);
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder
    .Build()
    .RunAsync();