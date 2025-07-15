namespace MCP.Contracts
{
    public interface IIntentRouter
    {
        Task<object?> HandleAsync(string userInput);
    }
}
