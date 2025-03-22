namespace FakeNews.web.Repositories
{
	public interface IImageRepository
	{
		Task<string> Upload(IFormFile file);
	}
}
