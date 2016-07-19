using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Blongo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseUrls("http://localhost:47328")
                .UseStartup<Startup>()
                .UseIISIntegration()
                .Build();

            host.Run();
        }
    }
}