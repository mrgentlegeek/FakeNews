using FakeNews.web.Models.Domain;

namespace FakeNews.web.Repositories
{
	public interface ITagRepository
	{
		Task<IEnumerable<Tag>> GetAllTagsAsync(string? searchQuery = null, string? sortBy = null, string? sortDirection = null, int pageSize = 100, int pageNumber = 1);

		Task<Tag?> GetTagByIdAsync(Guid id);

		Task<Tag> AddTagAsync(Tag tag);

		Task<Tag?> UpdateTagAsync(Tag tag);

		Task<Tag?> DeleteTagByIdAsync(Guid id);

		Task<int> CountsAsync();
	}
}
