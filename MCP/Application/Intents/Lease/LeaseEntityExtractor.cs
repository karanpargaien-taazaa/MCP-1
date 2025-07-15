using GenAI;
using MCP.Application.Intents.Lease.Models;
using MCP.Contracts;

namespace MCP.Application.Intents.Lease
{
    public class LeaseEntityExtractor : IEntityExtractor<LeaseEntityInput>
    {
        private readonly IGenAI _genAI;

        public LeaseEntityExtractor(IGenAI genAI)
        {
            _genAI = genAI;
        }

        public async Task<LeaseEntityInput> ExtractEntitiesAsync(string userInput)
        {
            var prompt = $"""
            Extract the following details from this lease creation request: 
            - Tenant Name
            - Property Name
            - Unit Name

            User said: "{userInput}"
            """;

            var result = await _genAI.GetResponseAsync<LeaseEntityExtractionResponse>(prompt);

            return new LeaseEntityInput
            {
                TenantName = result.TenantName,
                PropertyName = result.PropertyName,
                UnitName = result.UnitName
            };
        }
    }
}