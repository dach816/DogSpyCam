using System;
using System.Threading.Tasks;
using DogSpyCam.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DogSpyCam
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var hostBuilder = new HostBuilder()
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddJsonFile("appsettings.json", optional: true);
                        config.AddEnvironmentVariables();
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddOptions();
                        services.AddHostedService<CameraService>();

                        services.AddSingleton<IVideoClient>(x =>
                        {
                            var config = x.GetService<IOptions<VideoConfig>>().Value;
                            return new VideoClient(config.VideoSavePath);
                        });
                    });
                await hostBuilder.RunConsoleAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
