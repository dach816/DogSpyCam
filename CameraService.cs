﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace DogSpyCam
{
    public class CameraService : IHostedService
    {
        private readonly IVideoClient _videoClient;

        public CameraService(IVideoClient videoClient)
        {
            _videoClient = videoClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting...");
            Console.WriteLine("Type start to start");
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine("Waiting for input...");
                    var input = Console.ReadLine();
                    if (input == null)
                    {
                        continue;
                    }

                    if (input.Equals("start", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("Starting video...");
                        await _videoClient.TakeVideo(cancellationToken);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping...");
            return Task.CompletedTask;
        }
    }
}
