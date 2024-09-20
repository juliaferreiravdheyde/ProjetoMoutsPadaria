using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using RazorPagesEstudo.Data;
using RazorPagesEstudo.Services;

namespace RazorPagesEstudo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Get the connection string
            var connectionString = builder.Configuration.GetConnectionString("RazorPagesEstudoContext")
                ?? throw new InvalidOperationException("Connection string 'RazorPagesEstudoContext' not found.");

            // Register the DbContext if needed
            builder.Services.AddDbContext<RazorPagesEstudoContext>(options =>
                options.UseSqlServer(connectionString));

            // Register the HttpClient for ClienteApiClient
            builder.Services.AddHttpClient<ClienteApiClient>(client =>
            {
                client.BaseAddress = new Uri("http://your-api-url/"); // usar a URL da API de cliente 
            });

            builder.Services.AddScoped<VendaService>(provider =>
            {
                var clienteApiClient = provider.GetRequiredService<ClienteApiClient>();
                return new VendaService(connectionString, clienteApiClient);
            });

            // Register other services
            builder.Services.AddRazorPages();
            builder.Services.AddSession();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            var defaultCulture = new CultureInfo("en-US");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };

            app.UseRequestLocalization(localizationOptions);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            app.Run();
        }
    }
}
