using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YourApp.Services
{
    public interface IChatService
    {
        Task<string> GetResponseAsync(string message, string sessionId);
    }

    public class GeminiChatService : IChatService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private static Dictionary<string, List<MessageContent>> _sessionHistory = new();

        public GeminiChatService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiKey = configuration["Gemini:ApiKey"];
            _httpClient = httpClient;

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("Gemini API Key not found in appsettings.json");
            }
        }

        public async Task<string> GetResponseAsync(string message, string sessionId)
        {
            try
            {
                // Initialize session history if new
                if (!_sessionHistory.ContainsKey(sessionId))
                {
                    _sessionHistory[sessionId] = new List<MessageContent>();
                }

                // Add user message to history
                _sessionHistory[sessionId].Add(new MessageContent
                {
                    role = "user",
                    parts = new List<Part> { new Part { text = message } }
                });

                // Prepare request
                var requestBody = new
                {
                    contents = _sessionHistory[sessionId],
                    generationConfig = new
                    {
                        temperature = 0.7,
                        maxOutputTokens = 512
                    }
                };

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(requestBody, jsonOptions),
                    Encoding.UTF8,
                    "application/json"
                );

                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

                var response = await _httpClient.PostAsync(url, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API Error: {response.StatusCode} - {errorContent}");
                }

                var aiResponse = await ExtractResponseText(response);

                if (string.IsNullOrEmpty(aiResponse))
                {
                    aiResponse = "I couldn't generate a response. Please try again.";
                }

                // Add AI response to history
                _sessionHistory[sessionId].Add(new MessageContent
                {
                    role = "model",
                    parts = new List<Part> { new Part { text = aiResponse } }
                });

                return aiResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting AI response: {ex.Message}", ex);
            }
        }

        private async Task<string> ExtractResponseText(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(responseContent);
            var root = jsonDocument.RootElement;

            string aiResponse = null;

            if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
            {
                var firstCandidate = candidates[0];
                if (firstCandidate.TryGetProperty("content", out var content))
                {
                    if (content.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                    {
                        if (parts[0].TryGetProperty("text", out var text))
                        {
                            aiResponse = text.GetString();
                        }
                    }
                }
            }

            return aiResponse;
        }

        public void ClearSession(string sessionId)
        {
            if (_sessionHistory.ContainsKey(sessionId))
            {
                _sessionHistory.Remove(sessionId);
            }
        }
    }

    public class MessageContent
    {
        [JsonPropertyName("role")]
        public string role { get; set; }

        [JsonPropertyName("parts")]
        public List<Part> parts { get; set; }
    }

    public class Part
    {
        [JsonPropertyName("text")]
        public string text { get; set; }
    }
}