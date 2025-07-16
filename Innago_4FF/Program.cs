using GenAI;
using GenAI.Providers.Gemini;
using GenAI.Providers.Mistral;
using MCP.Application;

namespace Innago_4FF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddOptions<GeminiConfig>()
                .BindConfiguration("Gemini")
                .ValidateDataAnnotations();
            builder.Services.AddOptions<MistralConfig>()
                .BindConfiguration("Mistral")
                .ValidateDataAnnotations();
            builder.Services.AddHttpClient<IGenAI, Mistral>();
            builder.Services.AddMcp();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
