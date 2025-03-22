using FakeNews.web.Models.Domain;

namespace FakeNews.web.Repositories
{
	public interface IBlogPostLikeRepository
	{
		Task<int> GetTotalLikes(Guid blogPostId);

		Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);

		Task<BlogPostLike> AddBlogPostLike(BlogPostLike blogPostLike);
	}
}
