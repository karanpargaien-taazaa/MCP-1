namespace MCP.Contracts
{
    public interface IIntentHandler<BaseIntentResponse>
    {
        string IntentName { get; }
        new Task<BaseIntentResponse> HandleAsync(string userInput);
    }
}