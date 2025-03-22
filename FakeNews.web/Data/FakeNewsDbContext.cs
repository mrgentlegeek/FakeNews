using FakeNews.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace FakeNews.web.Data
{
	public class FakeNewsDbContext : DbContext
	{
		public FakeNewsDbContext(DbContextOptions<FakeNewsDbContext> options) : base(options)
		{
		}

		public DbSet<BlogPost> BlogPosts { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<BlogPostLike> BlogPostLikes { get; set; }
		public DbSet<BlogPostComment> BlogPostComments { get; set; }
	}
}
