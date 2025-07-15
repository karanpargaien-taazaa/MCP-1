namespace GenAI.Providers.Ollama
{
    public class OllamaConfig
    {
        public string Endpoint { get; set; }
        public string Model { get; set; } // If your local Mistral instance requires an API key
    }
}