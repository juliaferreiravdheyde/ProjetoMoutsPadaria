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

            
            builder.Services.AddDbContext<RazorPagesEstudoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("RazorPagesEstudoContext")
                    ?? throw new InvalidOperationException("Connection string 'RazorPagesEstudoContext' not found.")));

            builder.Services.AddRazorPages();

          
            builder.Services.AddScoped<VendaService>();

           
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
