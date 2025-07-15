using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace GenAI.Providers.Gemini
{
    public class Gemini : IGenAI
    {
        private readonly HttpClient _geminiClient;
        private readonly string _geminiApiKey;

        public Gemini(IOptions<GeminiConfig> config, HttpClient? httpClient = null)
        {
            _geminiApiKey = config.Value.ApiKey ?? throw new ArgumentNullException(nameof(config.Value.ApiKey));
            _geminiClient = httpClient ?? new HttpClient();
        }

        public async Task<T> GetResponseAsync<T>(string request, bool requestIncludeResponseSchema = false)
            where T : IGenAIResponse<T>, new()
        {
            T t = new T();
            var sampleResponse = t.GetSampleInstance();
            var sampleResponseJson = JsonConvert.SerializeObject(sampleResponse);

            if (!requestIncludeResponseSchema)
            {
                request += $". Just responsd with JSON(no extra text or affirmation). Response should be in following JSON Format: {sampleResponseJson}";
            }

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = request }
                        }
                    }
                }
            };

            var jsonString = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                response = await _geminiClient.PostAsync(
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_geminiApiKey}",
                    content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                // Log or handle HTTP errors
                throw new InvalidOperationException("Error calling Gemini API.", ex);
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            GeminiResponse? geminiResponse = null;
            try
            {
                geminiResponse = JsonConvert.DeserializeObject<GeminiResponse>(responseContent);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Failed to deserialize Gemini API response.", ex);
            }

            if (geminiResponse?.candidates == null || geminiResponse.candidates.Count == 0)
                throw new InvalidOperationException("No candidates returned from Gemini API.");

            var part = geminiResponse.candidates[0].content.parts.FirstOrDefault();
            if (part == null || string.IsNullOrWhiteSpace(part.text))
                throw new InvalidOperationException("No text content in Gemini API response.");

            string jsonResponse = part.text.Replace("```", "").Replace("json\n", "").Trim();

            T? responseObj;
            try
            {
                responseObj = JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Failed to deserialize Gemini response content.", ex);
            }

            return responseObj != null ? responseObj : throw new InvalidOperationException("No valid response object found.");
        }
    }
}