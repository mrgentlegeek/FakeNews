
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace FakeNews.web.Repositories
{
    public class CloudnaryImageRepository : IImageRepository
    {
        private readonly IConfiguration configuration;
        private readonly Account cloudnaryAccount;

        public CloudnaryImageRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            cloudnaryAccount = new Account(
                configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]
                );
        }

        public async Task<string> Upload(IFormFile file)
        {
            var client = new Cloudinary(cloudnaryAccount);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName,
                Folder = "FakeNews-web",
            };

            var response = await client.UploadAsync(uploadParams);

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.SecureUrl.ToString();
            }

            return null;
        }
    }
}
