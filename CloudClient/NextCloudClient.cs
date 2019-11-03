namespace CloudClient
{
    public class NextCloudClient
    {
        private static HttpClient Client;
        private string _savePath;

        public NextCloudClient(string username, string password, string baseUrl, string savePath){
            Client = new HttpClient(baseUrl);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
            _savePath = savePath;
        }

        public async Task UploadFile(string filePath, string filename) {
            Console.WriteLine($"Uploading file {filename} to NextCloud");
            using (var file = File.OpenRead(filePath)){
                var content = new StreamContent(file);
                var response = await Client.PutAsync(_savePath, content);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}