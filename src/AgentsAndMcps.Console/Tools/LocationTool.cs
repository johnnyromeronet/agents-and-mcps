using System.ComponentModel;
using ModelContextProtocol.Server;

namespace AgentsAndMcps.Console.Tools;

[McpServerToolType]
public class LocationTool
{
    [McpServerTool, Description("Obtiene información (en formato json) sobre la localización de un lugar.")]
    public static string GetLocationInfo(string site)
    {
        try
        {
            var apiUrl = $"https://nominatim.openstreetmap.org/search?q={site}&addressdetails=1&limit=1&format=json";

            using HttpClient client = new();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("AgentsAndMcps.Server.Tools/1.0");

            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;
            System.Console.WriteLine($"GetLocationInfo --> {result}");

            return result;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"GetLocationInfo --> [ERROR] {ex.Message}");
            return string.Empty;
        }
    }

    [McpServerTool, Description("Obtiene mi nombre propio.")]
    public static string GetName()
    {
        System.Console.WriteLine("GetName");
        return "Johnny Romero";
    }
}