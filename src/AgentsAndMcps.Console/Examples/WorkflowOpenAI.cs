using AgentsAndMcps.Console.Agents;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace AgentsAndMcps.Console.Examples;

public static class WorkflowOpenAI
{
    private static readonly string _apiKey = "sk-XXX";
    private static readonly string _deploymentModel = "gpt-4o-mini";
    private static readonly string _usrPrompt = "Sugiere un plan para disfrutar Sevilla teniendo en cuenta la situación meteorológica.";

    public static async Task ChatWithWorkflow()
    {
        System.Console.WriteLine("--- RESPUESTA WORKFLOW ---");

        IChatClient client = new ChatClient(_deploymentModel, _apiKey).AsIChatClient();

        var weatherAgent = WeatherAgent.CreateAgent(client);
        var plannerAgent = PlannerAgent.CreateAgent(client, _usrPrompt);

        var workflow = await AgentWorkflowBuilder.BuildSequential([weatherAgent, plannerAgent]).AsAgentAsync();
        var response = await workflow.RunAsync();

        foreach (var message in response.Messages.Where(x => !string.IsNullOrWhiteSpace(x.Text)))
        {
            System.Console.WriteLine("------------------------------------------------------");
            System.Console.WriteLine($"[{message.AuthorName}] {message.Text}");
        }
    }
}
