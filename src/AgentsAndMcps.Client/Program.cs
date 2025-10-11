using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ModelContextProtocol.Client;

// ===================================================
// CONFIGURACIÓN BASE
// ===================================================

string apiKey = "sk-XXX";
string deploymentModel = "gpt-4o-mini";
string sysPrompt = "Eres un experto en geolocalizar ubicaciones.";

// ===================================================
// CONFIGURAR TRANSPORTE Y CLIENTE MCP
// ===================================================

var transport = new StdioClientTransport(new()
{
    Name = "AgentsAndMcps.Console",
    Command = "dotnet",
    Arguments = ["../../../../AgentsAndMcps.Console/bin/Debug/net8.0/AgentsAndMcps.Console.dll"]
});

var mcpClient = await McpClient.CreateAsync(transport);
var tools = await mcpClient.ListToolsAsync();

Console.WriteLine("=== Herramientas MCP disponibles ===");
foreach (var tool in tools)
{
    Console.WriteLine($"{tool.Name}: {tool.Description}");
}

// ===================================================
// CREAR EL KERNEL DE SEMANTIC KERNEL
// ===================================================

var kernelBuilder = Kernel.CreateBuilder();
kernelBuilder.AddOpenAIChatCompletion(deploymentModel, apiKey);

var kernel = kernelBuilder.Build();
kernel.Plugins.AddFromFunctions("LocationTool", tools.Select(x => x.AsKernelFunction()));

// ===================================================
// CONFIGURAR CLIENTE DE CHAT (Microsoft.Extensions.AI)
// ===================================================

var chatClient = kernel.GetRequiredService<IChatCompletionService>();

var messages = new ChatHistory();
messages.AddSystemMessage(sysPrompt);

var settings = new OpenAIPromptExecutionSettings()
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

// ===================================================
// LOOP INTERACTIVO CON EL USUARIO
// ===================================================

Console.WriteLine("=== Chat interactivo ===");
while (true)
{
    Console.WriteLine("Indica la ubicación que quieres localizar: ");
    var usrPrompt = Console.ReadLine();

    messages.AddUserMessage(usrPrompt!);
    var response = await chatClient.GetChatMessageContentsAsync(messages, settings, kernel);

    Console.WriteLine(response[0].Content);
}