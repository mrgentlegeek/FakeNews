using FakeNews.web.Models.Domain;

namespace FakeNews.web.Repositories
{
	public interface IBlogPostCommentRepository
	{
		Task<BlogPostComment> AddAsync(BlogPostComment comment);

		Task<IEnumerable<BlogPostComment>> GetAllCommentsByBlogIdAsync(Guid blogPostId);
	}
}
