using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DogSpyCam
{
    public interface IVideoClient
    {
        Task TakeVideo(CancellationToken cancellationToken);
    }
}
