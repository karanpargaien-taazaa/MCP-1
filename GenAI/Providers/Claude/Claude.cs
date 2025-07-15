using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace GenAI.Providers.Claude
{
    public class Claude : IGenAI
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly string _endpoint;

        public Claude(IOptions<ClaudeConfig> config, HttpClient? httpClient = null)
        {
            _apiKey = config.Value.ApiKey ?? throw new ArgumentNullException(nameof(config.Value.ApiKey));
            _endpoint = config.Value.Endpoint ?? throw new ArgumentNullException(nameof(config.Value.Endpoint));
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
                request += $". Response should be in following JSON Format: [{sampleResponseJson}]";
            }

            var requestBody = new
            {
                model = "claude-3-opus-20240229", // or your preferred Claude model
                max_tokens = 1024,
                messages = new[]
                {
                    new { role = "user", content = request }
                }
            };

            var jsonString = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            content.Headers.Add("x-api-key", _apiKey);
            content.Headers.Add("anthropic-version", "2023-06-01");

            HttpResponseMessage response;
            try
            {
                response = await _client.PostAsync(_endpoint, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error calling Claude API.", ex);
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            dynamic? claudeResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

            string? text = claudeResponse?.content?[0]?.text?.ToString();
            if (string.IsNullOrWhiteSpace(text))
                throw new InvalidOperationException("No text content in Claude API response.");

            string jsonResponse = text.Replace("```", "").Replace("json\n", "").Trim();

            List<T>? responseObj;
            try
            {
                responseObj = JsonConvert.DeserializeObject<List<T>>(jsonResponse);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Failed to deserialize Claude response content.", ex);
            }

            return responseObj != null ? responseObj[0] : throw new InvalidOperationException("No valid response object found.");
        }
    }
}