namespace MCP.Contracts
{
    public interface IEntityExtractor<T>
    {
        Task<T> ExtractEntitiesAsync(string userInput);
    }
}