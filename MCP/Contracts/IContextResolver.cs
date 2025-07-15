namespace MCP.Contracts
{
    public interface IContextResolver<TInput, TResolved>
    {
        Task<TResolved> ResolveAsync(TInput input);
    }
}