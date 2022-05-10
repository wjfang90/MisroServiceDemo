using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServiceInstance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // dotnet ����Ҫ��dll Ŀ¼ִ��
            // dotnet MicroServiceInstance.dll --urls="http://*:7000" --ip="127.0.0.1" --port=7000  --weight=1
            //���������в���
            //new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddCommandLine(args)
            //    .Build();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(config =>
                    {
                        config.AddCommandLine(args); //���������в���
                        //config.SetBasePath(Directory.GetCurrentDirectory())//���÷����ַ
                        //    .AddJsonFile("hostsetting.json", true, false);

                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
