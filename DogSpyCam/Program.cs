using System;
using System.Threading.Tasks;
using DogSpyCam.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using CameraClient;

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
                        config.AddJsonFile("appsettings.json");
                        config.AddEnvironmentVariables();
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddOptions();
                        services.AddHostedService<CameraService>();

                        services.Configure<VideoConfig>(hostContext.Configuration.GetSection("Video"));
                        services.AddSingleton<IVideoClient>(x =>
                        {
                            var config = x.GetService<IOptions<VideoConfig>>().Value;
                            return new VideoClient(config.VideoSavePath);
                        });

                        services.Configure<CloudConfig>(hostContext.Configuration.GetSection("Cloud"));
                        services.AddSingleton<ICloudClient>(x =>
                        {
                            var config = x.GetService<IOptions<CloudConfig>>().Value;
                            return new CloudClient(config.Username, config.Password, config.BaseUrl, config.SavePath);
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
