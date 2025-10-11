using ModelContextProtocol.Server;

namespace AgentsAndMcps.SSE.Server;

public class SseTransportManager
{
    public SseResponseStreamTransport? Transport { get; private set; }

    public void SetTransport(SseResponseStreamTransport transport)
    {
        Transport = transport;
    }
}

