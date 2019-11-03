using System;

namespace CloudClient
{
    public interface ICloudClient
    {
        Task UploadFile(string filePath, string filename);
    }
}
