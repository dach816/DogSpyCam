using System;
using System.Threading;
using System.Threading.Tasks;
using MMALSharp;
using MMALSharp.Handlers;

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
            MMALCamera cam = MMALCamera.Instance;

            using (var ffCaptureHandler = FFmpegCaptureHandler.RawVideoToAvi(_videoSavePath, "testing1234"))
            using (var vidEncoder = new MMALVideoEncoder())
            using (var renderer = new MMALVideoRenderer())
            {
                cam.ConfigureCameraSettings(); 

                var portConfig = new MMALPortConfig(MMALEncoding.H264, MMALEncoding.I420, 10, MMALVideoEncoder.MaxBitrateLevel4, null);

                // Create our component pipeline. Here we are using the H.264 standard with a YUV420 pixel format. The video will be taken at 25Mb/s.
                vidEncoder.ConfigureOutputPort(portConfig, ffCaptureHandler);

                cam.Camera.VideoPort.ConnectTo(vidEncoder);
                cam.Camera.PreviewPort.ConnectTo(renderer);
                                
                // Camera warm up time
                await Task.Delay(2000);
                
                await cam.ProcessAsync(cam.Camera.VideoPort, cancellationToken);
            }

            // Only call when you no longer require the camera, i.e. on app shutdown.
            cam.Cleanup();
        }
    }
}
