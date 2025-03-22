
using FakeNews.web.Data;
using FakeNews.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace FakeNews.web.Repositories
{
	public class BlogPostLikeRepository : IBlogPostLikeRepository
	{
		private readonly FakeNewsDbContext fakeNewsDbContext;

		public BlogPostLikeRepository(FakeNewsDbContext fakeNewsDbContext)
		{
			this.fakeNewsDbContext = fakeNewsDbContext;
		}

		public async Task<int> GetTotalLikes(Guid blogPostId)
		{
			return await fakeNewsDbContext.BlogPostLikes.CountAsync(x => x.BlogPostId == blogPostId);
		}


		public async Task<BlogPostLike> AddBlogPostLike(BlogPostLike blogPostLike)
		{
			await fakeNewsDbContext.BlogPostLikes.AddAsync(blogPostLike);

			await fakeNewsDbContext.SaveChangesAsync();

			return blogPostLike;
		}

		public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
		{
			return await fakeNewsDbContext.BlogPostLikes.Where(x => x.BlogPostId == blogPostId).ToListAsync();
		}
	}
}
