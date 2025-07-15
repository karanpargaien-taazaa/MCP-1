using GenAI;
using MCP.Models;
namespace MCP.Application.Intents.Lease.Models
{
    public class LeaseIntentResponse : BaseIntentHandlerResponse, IGenAIResponse<LeaseIntentResponse>
    {
        public int? PropertyId { get; set; }
        public int? UnitId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Tenant> Tenants { get; set; }

        public LeaseIntentResponse GetSampleInstance() => new LeaseIntentResponse
        {
            PropertyId = 456,
            UnitId = 789,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddYears(1),
            Tenants = new List<Tenant>
            {
                new Tenant { TenantId = 123, FirstName = "John", LastName = "Doe", Email="any@any.com" },
                new Tenant { TenantId = 124, FirstName = "Mike", LastName = "Ross", Email="any@any.com" }
            }
        };

        public class Tenant
        {
            public int? TenantId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }
    }
}