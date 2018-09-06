using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace AspNetSecurity_m1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
        .UseUrls("http://localhost:5001")
        .UseStartup<Startup>()
        .Build();

    }
}
