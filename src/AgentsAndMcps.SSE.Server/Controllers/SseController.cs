using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace AgentsAndMcps.SSE.Server.Controllers;

[ApiController]
public class SseController(ILoggerFactory logger, IOptions<McpServerOptions> mcpServerOptions, SseTransportManager transportManager) : ControllerBase
{
    private readonly SseTransportManager _transportManager = transportManager;
    private readonly IOptions<McpServerOptions> _mcpServerOptions = mcpServerOptions;
    private readonly ILoggerFactory _logger = logger;

    [Produces("text/event-stream")]
    [HttpGet("sse")]
    public async Task GetSse(CancellationToken ct)
    {
        Response.Headers.Append("Cache-Control", "no-cache"); 
        Response.Headers.Append("Connection", "keep-alive"); 
        Response.Headers.Append("Content-Type", "text/event-stream");

        await using var transport = new SseResponseStreamTransport(Response.Body);
        _transportManager.SetTransport(transport);

        await using var server = McpServer.Create(transport, _mcpServerOptions.Value, _logger, HttpContext.RequestServices);

        Task.WaitAll([transport.RunAsync(ct), server.RunAsync(ct)], ct);
    }

    [Produces("application/json")]
    [HttpPost("message")]
    public async Task<IActionResult> PostMessage([FromBody] JsonRpcMessage message, CancellationToken ct)
    {
        await _transportManager.Transport!.OnMessageReceivedAsync(message!, ct);
        return Ok();
    }
}
