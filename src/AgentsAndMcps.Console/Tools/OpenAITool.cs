using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using NAudio.Wave;

namespace AgentsAndMcps.Console.Tools;

[McpServerToolType]
public static class OpenAITool
{
    private static readonly string _apiKey = "sk-XXX";
    private static readonly string _apiUrl = "https://api.openai.com/v1/";
    private static readonly string _ttsModel = "tts-1";
    private static readonly int _timeout = 5;

    [McpServerTool, Description("Convierte un texto en audio y lo reproduce.")]
    public static async Task<string> TextToSpeech(
       [Description("Texto para transformar en audio y reproducir.")] string inputText)
    {
        try
        {
            using HttpClient client = new();
            client.Timeout = TimeSpan.FromMinutes(_timeout);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var requestBody = new
            {
                model = _ttsModel,
                input = inputText,
                voice = "ash"
            };

            string jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{_apiUrl}audio/speech", content);
            response.EnsureSuccessStatusCode();

            var audio = await response.Content.ReadAsByteArrayAsync();

            using (var ms = new MemoryStream(audio))
            using (var reader = new Mp3FileReader(ms))
            using (var waveOut = new WaveOutEvent())
            {
                var playbackStopped = new ManualResetEvent(false);

                waveOut.Init(reader);
                waveOut.PlaybackStopped += (s, a) => playbackStopped.Set();
                waveOut.Play();

                // Espera hasta que termine
                playbackStopped.WaitOne();
            }

            return string.Empty;
        }
        catch (Exception ex)
        {
            var exceptionMsg = ex.InnerException != null ? $"{ex.Message} - {ex.InnerException.Message}" : ex.Message;
            var message = $"TextToSpeech --> [ERROR] {exceptionMsg}";
            throw new McpException(message);
        }
    }
}
