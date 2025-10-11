using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using OpenAI;
using OpenAI.Chat;

namespace AgentsAndMcps.Console;

public static class SimpleOpenAI
{
    private static readonly string _apiKey = "sk-XXX";
    private static readonly string _apiUrl = "https://api.openai.com/v1/";
    private static readonly string _deploymentModel = "gpt-4o-mini";
    private static readonly string _sysPrompt = "Eres un guía turístico experto.";
    private static readonly string _usrPrompt = "Sugiere un plan para disfrutar Sevilla en verano sin sufrir el calor.";

    public static async Task ChatWithApi()
    {
        System.Console.WriteLine("--- RESPUESTA API ---");

        using HttpClient client = new();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var requestBody = new
        {
            model = _deploymentModel,
            messages = new[]
            {
                new { role = "system", content = _sysPrompt },
                new { role = "user", content = _usrPrompt }
            }
        };

        string jsonRequest = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync($"{_apiUrl}chat/completions", content);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(responseBody);

        var message = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        System.Console.WriteLine(message);
    }

    public static async Task ChatWithSdk()
    {
        System.Console.WriteLine("--- RESPUESTA SDK ---");

        var client = new OpenAIClient(_apiKey);
        var chatClient = client.GetChatClient(_deploymentModel);

        var messages = new List<ChatMessage>()
        {
            ChatMessage.CreateSystemMessage(_sysPrompt),
            ChatMessage.CreateUserMessage(_usrPrompt)
        };

        var response = await chatClient.CompleteChatAsync(messages);
        var message = response.Value.Content[0].Text;
        System.Console.WriteLine(message);
    }
}
