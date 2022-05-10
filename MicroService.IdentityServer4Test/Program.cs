using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.IdentityServer4Test
{
    public class Program
    {

        /*
         dotnet MicroService.IdentityServer4Test.dll --urls="http://localhost:7300"
         */
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(
                    config =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false, false)
                        .AddCommandLine(args);
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
