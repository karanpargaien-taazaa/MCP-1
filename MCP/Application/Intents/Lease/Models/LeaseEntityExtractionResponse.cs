using GenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCP.Application.Intents.Lease.Models
{
    public class LeaseEntityExtractionResponse : IGenAIResponse<LeaseEntityExtractionResponse>
    {
        public string TenantName { get; set; } = "";
        public string PropertyName { get; set; } = "";
        public string UnitName { get; set; } = "";

        public LeaseEntityExtractionResponse GetSampleInstance() => new LeaseEntityExtractionResponse
        {
            TenantName = "John Doe",
            PropertyName = "ABC Apartments",
            UnitName = "A1"
        };
    }
}
