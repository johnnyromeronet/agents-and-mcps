using AgentsAndMcps.Console.Agents;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace AgentsAndMcps.Console.Examples;

public static class AgentsOpenAI
{
    private static readonly string _apiKey = "sk-XXX";
    private static readonly string _deploymentModel = "gpt-4o-mini";
    private static readonly string _usrPrompt = "Sugiere un plan para disfrutar Sevilla en verano sin sufrir el calor.";

    public static async Task ChatWithAgents()
    {
        System.Console.WriteLine("--- RESPUESTA AGENTS ---");

        IChatClient client = new ChatClient(_deploymentModel, _apiKey).AsIChatClient();
        var plannerAgent = PlannerAgent.CreateAgent(client);

        var response = await plannerAgent.RunAsync(_usrPrompt);
        System.Console.WriteLine(response.Text);
    }
}
