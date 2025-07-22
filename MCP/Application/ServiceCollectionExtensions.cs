using MCP.Application.Intents.Lease;
using MCP.Application.Intents.Lease.Models;
using MCP.Application.Router;
using MCP.Contracts;
using MCP.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MCP.Application
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers MCP services for the dependency injection container, including the lease intent pipeline and intent router.
        /// </summary>
        /// <param name="services">The service collection to add MCP services to.</param>
        /// <returns>The service collection with MCP services registered.</returns>
        public static IServiceCollection AddMcp(this IServiceCollection services)
        {
            // Lease intent pipeline
            services.AddScoped<IEntityExtractor<LeaseEntityInput>, LeaseEntityExtractor>();
            services.AddScoped<IContextResolver<LeaseEntityInput, LeaseContextResolved>, LeaseContextResolver>();
            services.AddScoped<IPromptContextBuilder<LeaseContextResolved, LeaseIntentResponse>, LeasePromptContextBuilder>();
            services.AddScoped<IIntentHandler<BaseIntentResponse>, LeaseIntentHandler>();

            // Intent router
            services.AddScoped<IIntentRouter, IntentRouter>();

            return services;
        }
    }
}