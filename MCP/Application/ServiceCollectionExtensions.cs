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
        public static IServiceCollection AddMcp(this IServiceCollection services)
        {
            // Lease intent pipeline
            services.AddScoped<IEntityExtractor<LeaseEntityInput>, LeaseEntityExtractor>();
            services.AddScoped<IContextResolver<LeaseEntityInput, LeaseContextResolved>, LeaseContextResolver>();
            services.AddScoped<IPromptContextBuilder<LeaseContextResolved, LeaseIntentResponse>, LeasePromptContextBuilder>();
            services.AddScoped<IIntentHandler<BaseIntentHandlerResponse>, LeaseIntentHandler>();

            // Intent router
            services.AddScoped<IIntentRouter, IntentRouter>();

            return services;
        }
    }
}