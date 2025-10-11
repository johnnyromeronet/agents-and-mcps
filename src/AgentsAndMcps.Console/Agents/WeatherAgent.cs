using System.ComponentModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AgentsAndMcps.Console.Agents;

public static class WeatherAgent
{
    private static readonly string _apiUrl = "https://api.open-meteo.com/v1/forecast?latitude=37.3952108&longitude=-5.9996715&hourly=temperature_2m,relative_humidity_2m,precipitation_probability&format=json&timeformat=unixtime";
    private static readonly string _sysPrompt = @"
Dada la información meteorológica (en formato json), por cada hora, en la provincia de Sevilla. Dispones de 3 métricas:
- Temperatura
- Humedad relativa
- Probabilidad de precipitación

Debes separar en 4 franjas: Mañana, Medio día, Tarde y Noche, haciendo una media de los 3 puntos anteriores en cada franja.
";

    public static ChatClientAgent CreateAgent(IChatClient client)
    {
        return new ChatClientAgent(client, new ChatClientAgentOptions()
        {
            Name = "WeatherAgent",
            Instructions = _sysPrompt,
            ChatOptions = new ChatOptions()
            {
                Tools = [AIFunctionFactory.Create(GetWeatherInfo)]
            }
        });
    }

    [Description("Obtiene la información meteorológica de Sevilla por franjas horarias, en formato json.")]
    private static async Task<string> GetWeatherInfo()
    {
        using HttpClient client = new();

        HttpResponseMessage response = await client.GetAsync($"{_apiUrl}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
