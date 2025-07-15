using MCP.Application.Intents.Lease.Models;
using MCP.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCP.Application.Intents.Lease
{
    public class LeaseContextResolver : IContextResolver<LeaseEntityInput, LeaseContextResolved>
    {
        public static readonly List<Tenant> Tenants = new()
        {
            new Tenant { TenantId = 123, Name = "John Doe" },
            new Tenant { TenantId = 124, Name = "Mike Ross" }
        };

        public static readonly List<Property> Properties = new()
        {
            new Property { PropertyId = 456, Name = "ABC Apartments" },
            new Property { PropertyId = 457, Name = "Elm Street House" }
        };

        public static readonly List<Unit> Units = new()
        {
            new Unit { UnitId = 789, Name = "A1" },
            new Unit { UnitId = 790, Name = "B2" }
        };

        public Task<LeaseContextResolved> ResolveAsync(LeaseEntityInput input)
        {
            var tenantId = Tenants.FirstOrDefault(t => t.Name == input.TenantName)?.TenantId;
            var propertyId = Properties.FirstOrDefault(p => p.Name == input.PropertyName)?.PropertyId;
            var unitId = Units.FirstOrDefault(u => u.Name == input.UnitName)?.UnitId;

            return Task.FromResult(new LeaseContextResolved
            {
                TenantId = tenantId,
                PropertyId = propertyId,
                UnitId = unitId
            });
        }
    }
}