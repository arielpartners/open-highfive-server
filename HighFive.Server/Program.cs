using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace HighFive.Server
{
    public class Program
    {
        private Program()
        {
        }
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
