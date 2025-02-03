using ContentHub.Repository.Content;
using ContentHub.Repository.Image;
using Serilog;
using Microsoft.Extensions.Logging;
using ContentHub.Services.Local;
using ContentHub.Services.ContentFetcher;

namespace ContentHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Initialize log service
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs\\ContentHub.txt", rollingInterval: RollingInterval.Day) // Log to a file, one per day
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);
            
            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Add the HttpClient and other services
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddScoped<IContentRepository, ContentRepository>();
            builder.Services.AddSingleton<IFetchWebsiteContent, FetchWebsiteContent>();
            builder.Services.AddSingleton<ILocalCache, LocalCache>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            
            try
            {
                // Write various log entries
                Log.Information("Application starting...");                

                app.Run();
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, "An error occurred");
            }
            finally
            {
                // Ensure logs are written out before the application shuts down
                Log.CloseAndFlush();
            }
            
        }
    }
}