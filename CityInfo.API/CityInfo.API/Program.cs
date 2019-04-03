using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace CityInfo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration(
                    (webHostbuilderContext, configurationBuilder) =>
                    {
                        //configurationBuilder.Sources.Clear();

                        // rename appSettings.json to appconfig.json to make configuration string work
                        // https://app.pluralsight.com/player?course=aspdotnet-core-2-upgrading-web-api&author=kevin-dockx&name=f1a5017c-b48a-4ecd-8189-3a290c12ec16&clip=5&mode=live
                        // var jsonConfigurationSources = configurationBuilder.Sources
                        //    .OfType<JsonConfigurationSource>()
                        //    .Where(c => c.Path.ToLowerInvariant().Contains("appsettings"));

                        //foreach (var source in jsonConfigurationSources)
                        //{
                        //    source.Path = source.Path.Replace("settings", "config");
                        //}
                    })        
        ;
        // NLog with asp.net core 2
        //https://github.com/NLog/NLog.Web/wiki/Getting-started-with-ASP.NET-Core-2
        // CreateDefaultBuilder source code
        //https://github.com/aspnet/MetaPackages/blob/master/src/Microsoft.AspNetCore/WebHost.cs

    }
}
