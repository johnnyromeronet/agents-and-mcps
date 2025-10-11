using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//await SimpleOpenAI.ChatWithApi();
//await SimpleOpenAI.ChatWithSdk();
//await ExtensionsOpenAI.ChatWithExtensions();
//await AgentsOpenAI.ChatWithAgents();
//await WorkflowOpenAI.ChatWithWorkflow();

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

var app = builder.Build();
await app.RunAsync();