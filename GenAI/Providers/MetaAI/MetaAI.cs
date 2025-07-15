using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace GenAI.Providers.MetaAI
{
    public class MetaAI : IGenAI
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly string _endpoint;

        public MetaAI(IOptions<MetaAIConfig> config, HttpClient? httpClient = null)
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
                model = "meta-llama/llama-3-70b-instruct", // or your preferred model
                messages = new[]
                {
                    new { role = "user", content = request }
                }
            };

            var jsonString = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            content.Headers.Add("Authorization", $"Bearer {_apiKey}");

            HttpResponseMessage response;
            try
            {
                response = await _client.PostAsync(_endpoint, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error calling MetaAI API.", ex);
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            dynamic? metaResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

            string? text = metaResponse?.choices?[0]?.message?.content?.ToString();
            if (string.IsNullOrWhiteSpace(text))
                throw new InvalidOperationException("No text content in MetaAI API response.");

            string jsonResponse = text.Replace("```", "").Replace("json\n", "").Trim();

            List<T>? responseObj;
            try
            {
                responseObj = JsonConvert.DeserializeObject<List<T>>(jsonResponse);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Failed to deserialize MetaAI response content.", ex);
            }

            return responseObj != null ? responseObj[0] : throw new InvalidOperationException("No valid response object found.");
        }
    }
}