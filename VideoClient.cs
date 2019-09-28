using System;
using System.Threading;
using System.Threading.Tasks;
using MMALSharp;
using MMALSharp.Handlers;

namespace DogSpyCam
{
    public class VideoClient : IVideoClient
    {
        public async Task TakeVideo(CancellationToken cancellationToken)
        {
            var cam = MMALCamera.Instance;
            using (var vidCaptureHandler = new VideoStreamCaptureHandler("/home/pi/videos/", "h264"))
            {
                await cam.TakeVideo(vidCaptureHandler, cancellationToken);
            }

            // Only call when you no longer require the camera, i.e. on app shutdown.
            cam.Cleanup();
        }
    }
}
