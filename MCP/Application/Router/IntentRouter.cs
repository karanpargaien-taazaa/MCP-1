using MCP.Contracts;
using MCP.Models;

namespace MCP.Application.Router
{
    public class IntentRouter : IIntentRouter
    {
        private readonly IEnumerable<IIntentHandler<BaseIntentResponse>> _leaseIntentHandlers;
        public IntentRouter(IEnumerable<IIntentHandler<BaseIntentResponse>> intentHandlers)
        {
            _leaseIntentHandlers = intentHandlers;
        }
        public async Task<object?> HandleAsync(string userInput)
        {
            var intent = await ResolveIntentAsync(userInput);
            var handler = _leaseIntentHandlers.FirstOrDefault(h => h.IntentName.Equals(intent));
            if (handler != null)
            {
                return await handler.HandleAsync(userInput);
            }
            return null;
        }
        private Task<string> ResolveIntentAsync(string userInput)
        {
            if (userInput.Contains("lease", StringComparison.OrdinalIgnoreCase)
                && (userInput.Contains("create", StringComparison.OrdinalIgnoreCase)
                || userInput.Contains("add", StringComparison.OrdinalIgnoreCase)
                || userInput.Contains("make", StringComparison.OrdinalIgnoreCase)))
                return Task.FromResult("create_lease");
            return Task.FromResult("unknown");
        }
    }
}
