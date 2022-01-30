using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
          IHost host = CreateHostBuilder(args).Build();//.Run();
          using IServiceScope scope = host.Services.CreateScope();  
          var services = scope.ServiceProvider;
          

          try
          {
              DataContext context =  services.GetRequiredService<DataContext>();

              await context.Database.MigrateAsync();

              await Seed.seedUsers(context);
          }
          catch (System.Exception ex)
          {
               ILogger logger = services.GetRequiredService<ILogger<Program>>();
               logger.LogError(ex,"An error occurred during migrations");
          }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
