using System;
using System.Threading;
using System.Threading.Tasks;
using MMALSharp;
using MMALSharp.Components;
using MMALSharp.Handlers;
using MMALSharp.Native;
using MMALSharp.Ports;

namespace DogSpyCam
{
    public class VideoClient : IVideoClient
    {
        private readonly string _videoSavePath;

        public VideoClient(string videoSavePath){
            _videoSavePath = videoSavePath;
        }

        public async Task TakeVideo(CancellationToken cancellationToken)
        {
            MMALCameraConfig.Rotation = 270;
            var cam = MMALCamera.Instance;

            using (var ffCaptureHandler = FFmpegCaptureHandler.RawVideoToAvi(_videoSavePath, $"Millie{DateTime.Now:s}"))
            using (var vidEncoder = new MMALVideoEncoder(ffCaptureHandler))
            using (var renderer = new MMALVideoRenderer())
            {
                cam.ConfigureCameraSettings(); 

                var portConfig = new MMALPortConfig(MMALEncoding.H264, MMALEncoding.I420, 10, MMALVideoEncoder.MaxBitrateLevel4, null);

                // Create our component pipeline. Here we are using the H.264 standard with a YUV420 pixel format. The video will be taken at 25Mb/s.
                vidEncoder.ConfigureOutputPort(portConfig);

                cam.Camera.VideoPort.ConnectTo(vidEncoder);
                cam.Camera.PreviewPort.ConnectTo(renderer);
                                
                // Camera warm up time
                await Task.Delay(2000, cancellationToken);
                
                await cam.ProcessAsync(cam.Camera.VideoPort, cancellationToken);
            }

            // Only call when you no longer require the camera, i.e. on app shutdown.
            cam.Cleanup();
        }
    }
}
