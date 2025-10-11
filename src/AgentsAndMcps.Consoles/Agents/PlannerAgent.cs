using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AgentsAndMcps.Console.Agents;

public static class PlannerAgent
{
    private static readonly string _sysPrompt = "Eres un guía turístico experto.";

    public static ChatClientAgent CreateAgent(IChatClient client)
    {
        return new ChatClientAgent(client, _sysPrompt, "PlannerAgent");
    }
}
