using MCP.Contracts;
using GenAI;
using MCP.Application.Intents.Lease.Models;
using MCP.Models;

namespace MCP.Application.Intents.Lease
{
    public class LeaseIntentHandler : IIntentHandler<BaseIntentResponse>
    {
        public string IntentName => "create_lease";

        private readonly IEntityExtractor<LeaseEntityInput> _extractor;
        private readonly IContextResolver<LeaseEntityInput, LeaseContextResolved> _resolver;
        private readonly IPromptContextBuilder<LeaseContextResolved, LeaseIntentResponse> _promptBuilder;
        private readonly IGenAI _genAI;

        public LeaseIntentHandler(
            IEntityExtractor<LeaseEntityInput> extractor,
            IContextResolver<LeaseEntityInput, LeaseContextResolved> resolver,
            IPromptContextBuilder<LeaseContextResolved, LeaseIntentResponse> promptBuilder,
            IGenAI genAI)
        {
            _extractor = extractor;
            _resolver = resolver;
            _promptBuilder = promptBuilder;
            _genAI = genAI;
        }

        public async Task<BaseIntentResponse> HandleAsync(string userInput)
        {
            var extracted = await _extractor.ExtractEntitiesAsync(userInput);
            var resolved = await _resolver.ResolveAsync(extracted);
            var prompt = _promptBuilder.BuildPromptContext(userInput, resolved);
            var response = await _genAI.GetResponseAsync<LeaseIntentResponse>(prompt);
            return response;
        }
    }
}