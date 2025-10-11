using Microsoft.Extensions.AI;
using OpenAI.Chat;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace AgentsAndMcps.Console.Examples;

public static class ExtensionsOpenAI
{
    private static readonly string _apiKey = "sk-XXX";
    private static readonly string _deploymentModel = "gpt-4o-mini";
    private static readonly string _sysPrompt = "Eres un guía turístico experto.";
    private static readonly string _usrPrompt = "Sugiere un plan para disfrutar Sevilla en verano sin sufrir el calor.";

    public static async Task ChatWithExtensions()
    {
        System.Console.WriteLine("--- RESPUESTA EXTENSIONS ---");

        IChatClient client = new ChatClient(_deploymentModel, _apiKey).AsIChatClient();

        var messages = new List<ChatMessage>()
        {
             new(ChatRole.System, _sysPrompt),
             new(ChatRole.User, _usrPrompt),
        };

        var response = await client.GetResponseAsync(messages);
        System.Console.WriteLine(response.Text);
    }
}
