using MCP.Application.Intents.Lease.Models;
using MCP.Contracts;

namespace MCP.Application.Intents.Lease
{
    public class LeasePromptContextBuilder : IPromptContextBuilder<LeaseContextResolved, LeaseIntentResponse>
    {
        public string BuildPromptContext(string userInput, LeaseContextResolved context)
        {
            return $@"You are an assistant that generates structured lease creation responses.
If you don't have all required data...use ResponseData property to tell the user.

User asked: '{userInput}'

Context:
TenantId: {context.TenantId} //If this value is null or 0, it means the tenant was not found, an we need name in response model. Otherwise just Id.
PropertyId: {context.PropertyId}
UnitId: {context.UnitId}
CurrentDateTime: {DateTime.Now.ToString()}";
        }
    }
}