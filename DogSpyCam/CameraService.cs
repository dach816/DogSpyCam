using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace DogSpyCam
{
    public class CameraService : IHostedService
    {
        private readonly IVideoClient _videoClient;

        public CameraService(
            IVideoClient videoClient)
        {
            _videoClient = videoClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var input = Console.ReadLine();
                if (input == null)
                {
                    continue;
                }

                if (input.Equals("start", StringComparison.InvariantCultureIgnoreCase))
                {
                    await _videoClient.TakeVideo(cancellationToken);
                }
                else if (input.Equals("stop", StringComparison.InvariantCultureIgnoreCase))
                {
                    await StopAsync(cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
