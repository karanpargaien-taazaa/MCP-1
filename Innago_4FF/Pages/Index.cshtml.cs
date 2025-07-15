using GenAI;
using MCP.Application.Intents.Lease;
using MCP.Application.Intents.Lease.Models;
using MCP.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Innago_4FF.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<Property> Properties { get; set; } = new();
        public List<Unit> Units { get; set; } = new();
        public List<Tenant> Tenants { get; set; } = new();
        [BindProperty]
        public string UserInput { get; set; }
        public LeaseIntentResponse LeaseResponse { get; set; }
        private readonly IIntentRouter _intentRouter;

        public IndexModel(ILogger<IndexModel> logger, IIntentRouter intentRouter)
        {
            _logger = logger;
            _intentRouter = intentRouter;
        }

        public void OnGet()
        {
            Properties = LeaseContextResolver.Properties;
            Units = LeaseContextResolver.Units;
            Tenants = LeaseContextResolver.Tenants;
        }

        public async Task OnPostAsync()
        {
            Properties = LeaseContextResolver.Properties;
            Units = LeaseContextResolver.Units;
            Tenants = LeaseContextResolver.Tenants;
            if (!string.IsNullOrWhiteSpace(UserInput))
            {
                // Replace with actual GenAI service call
                LeaseResponse = (LeaseIntentResponse)await _intentRouter.HandleAsync(UserInput);
            }
        }
    }
}
