namespace MCP.Contracts
{
    public interface IIntentHandler<BaseIntentHandlerResponse>
    {
        string IntentName { get; }
        new Task<BaseIntentHandlerResponse> HandleAsync(string userInput);
    }
}