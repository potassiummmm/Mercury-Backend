using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Mercury_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            // var host = new WebHostBuilder()
            //     .UseKestrel()
            //     .UseContentRoot(Directory.GetCurrentDirectory())
            //     .UseIISIntegration()
            //     .UseStartup<Startup>()
            //     .Build();
            //
            // host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://0.0.0.0:5000", "https://0.0.0.0:5001");
                    webBuilder.UseStartup<Startup>();
                })
                .UseContentRoot(Directory.GetCurrentDirectory());

    }
}