using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
namespace Dashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //.UseWebRoot("Uploads")
                    //.UseUrls("http://localhost:7000");
                });
    }
}
