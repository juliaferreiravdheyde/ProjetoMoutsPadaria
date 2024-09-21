using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using RazorPagesEstudo.Models; // Adjusted namespace for ApplicationDbContext
using RazorPagesEstudo.Services;
using System;
using RazorPagesEstudo.Data;

namespace RazorPagesEstudo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Get the connection string for RazorPagesEstudoContext
            var connectionString = builder.Configuration.GetConnectionString("RazorPagesEstudoContext")
                ?? throw new InvalidOperationException("Connection string 'RazorPagesEstudoContext' not found.");

            // Register the RazorPagesEstudoContext
            builder.Services.AddDbContext<RazorPagesEstudoContext>(options =>
                options.UseSqlServer(connectionString));

            // Get the connection string for ApplicationDbContext
            /*
            var applicationConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            */
            // Register ApplicationDbContext
            /*
            builder.Services.AddDbContext<DbContext>(options =>
                options.UseSqlServer(applicationConnectionString));
            */
            // Register the HttpClient for ClienteApiClient
            builder.Services.AddHttpClient<ClienteApiClient>(client =>
            {
                client.BaseAddress = new Uri("http://your-api-url/"); // Use the URL of the Cliente API
            });

            // Register ProdutoService
            builder.Services.AddScoped<ProdutoService>(provider =>
            {
                return new ProdutoService(connectionString);
            });

            // Register ClienteService
            builder.Services.AddScoped<ClienteService>(provider =>
            {
                return new ClienteService(connectionString);
            });

            // Register VendaService
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
