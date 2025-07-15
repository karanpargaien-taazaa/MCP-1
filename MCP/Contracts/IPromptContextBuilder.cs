namespace MCP.Contracts
{
    public interface IPromptContextBuilder<TContext, TResponse>
    {
        string BuildPromptContext(string originalUserInput, TContext context);
    }
}