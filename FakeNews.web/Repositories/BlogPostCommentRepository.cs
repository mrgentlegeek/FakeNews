using FakeNews.web.Data;
using FakeNews.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace FakeNews.web.Repositories
{
	public class BlogPostCommentRepository : IBlogPostCommentRepository
	{
		private readonly FakeNewsDbContext fakeNewsDbContext;

		public BlogPostCommentRepository(FakeNewsDbContext fakeNewsDbContext)
		{
			this.fakeNewsDbContext = fakeNewsDbContext;
		}

		public async Task<BlogPostComment> AddAsync(BlogPostComment comment)
		{
			await fakeNewsDbContext.BlogPostComments.AddAsync(comment);
			await fakeNewsDbContext.SaveChangesAsync();

			return comment;
		}

		public async Task<IEnumerable<BlogPostComment>> GetAllCommentsByBlogIdAsync(Guid blogPostId)
		{
			return await fakeNewsDbContext.BlogPostComments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
		}
	}
}
