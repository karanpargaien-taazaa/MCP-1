using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace GenAI.Providers.Ollama
{
    public class Ollama : IGenAI
    {
        private readonly HttpClient _client;
        private readonly string _endpoint;
        private readonly string? _model;

        public Ollama(IOptions<OllamaConfig> config, HttpClient? httpClient = null)
        {
            _endpoint = config.Value.Endpoint ?? throw new ArgumentNullException(nameof(config.Value.Endpoint));
            _model = config.Value.Model;
            _client = httpClient ?? new HttpClient();
        }

        public async Task<T> GetResponseAsync<T>(string request, bool requestIncludeResponseSchema = false)
            where T : IGenAIResponse<T>, new()
        {
            T t = new T();
            var sampleResponse = t.GetSampleInstance();
            var sampleResponseJson = JsonConvert.SerializeObject(sampleResponse);

            if (!requestIncludeResponseSchema)
            {
                request += $". Just respond with JSON (no extra text or affirmation). Response should be in following JSON Format: {sampleResponseJson}";
            }

            var requestBody = new
            {
                model= _model,
                prompt = request,
                stream = false
            };

            var jsonString = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                response = await _client.PostAsync(_endpoint, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error calling Ollama API.", ex);
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            OllamaResponse? ollamaResponse = null;
            try
            {
                ollamaResponse = JsonConvert.DeserializeObject<OllamaResponse>(responseContent);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Failed to deserialize Ollama API response.", ex);
            }

            if (ollamaResponse == null || string.IsNullOrWhiteSpace(ollamaResponse.response))
                throw new InvalidOperationException("No response returned from Ollama API.");

            string jsonResponse = ollamaResponse.response.Replace("```", "").Replace("json\n", "").Trim();

            T? responseObj;
            try
            {
                responseObj = JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Failed to deserialize Ollama response content.", ex);
            }

            return responseObj != null ? responseObj : throw new InvalidOperationException("No valid response object found.");
        }
    }
}