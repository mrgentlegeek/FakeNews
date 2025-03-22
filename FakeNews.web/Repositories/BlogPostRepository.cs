using FakeNews.web.Data;
using FakeNews.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace FakeNews.web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly FakeNewsDbContext fakeNewsDbContext;

        public BlogPostRepository(FakeNewsDbContext fakeNewsDbContext)
        {
            this.fakeNewsDbContext = fakeNewsDbContext;
        }

        public async Task<BlogPost> AddBlogAsync(BlogPost blogPost)
        {
            await fakeNewsDbContext.BlogPosts.AddAsync(blogPost);
            await fakeNewsDbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<BlogPost> DeleteBlogAsync(Guid id)
        {
            var existingBlog = await fakeNewsDbContext.BlogPosts.FindAsync(id);

            if (existingBlog != null)
            {
                fakeNewsDbContext.BlogPosts.Remove(existingBlog);
                await fakeNewsDbContext.SaveChangesAsync();

            }

            return existingBlog;
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogsAsync()
        {
            return await fakeNewsDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetBlogByIdAsync(Guid id)
        {
            return await fakeNewsDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetBlogByURLHandleAsync(string UrlHandle)
        {
            return await fakeNewsDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == UrlHandle);
        }

        public async Task<BlogPost?> UpdateBlogAsync(BlogPost blogPost)
        {
            //await fakeNewsDbContext.BlogPosts.UpdateAsync(blogPost);
            var existingBlog = await fakeNewsDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {

                existingBlog.Tags = blogPost.Tags;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.Author = blogPost.Author;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Visible = blogPost.Visible;

                await fakeNewsDbContext.SaveChangesAsync();

            }

            return existingBlog;

        }
    }
}

