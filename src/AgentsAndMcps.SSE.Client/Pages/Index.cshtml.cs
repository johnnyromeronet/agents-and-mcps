using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelContextProtocol.Client;

namespace AgentsAndMcps.SSE.Client.Pages
{
    public class IndexModel(ILogger<IndexModel> logger, McpClient mcpClient) : PageModel
    {
        private readonly ILogger<IndexModel> _logger = logger;
        private readonly McpClient _mcpClient = mcpClient;

        public async Task OnGet()
        {
            var tools = await mcpClient.ListToolsAsync();

            Console.WriteLine("=== Herramientas MCP disponibles ===");
            foreach (var tool in tools)
            {
                Console.WriteLine($"{tool.Name}: {tool.Description}");
            }
        }
    }
}
