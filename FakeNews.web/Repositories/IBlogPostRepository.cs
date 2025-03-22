using FakeNews.web.Models.Domain;

namespace FakeNews.web.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllBlogsAsync();

        Task<BlogPost?> GetBlogByIdAsync(Guid id);

        Task<BlogPost?> GetBlogByURLHandleAsync(string UrlHandle);

        Task<BlogPost> AddBlogAsync(BlogPost blogPost);

        Task<BlogPost?> UpdateBlogAsync(BlogPost blogPost);

        Task<BlogPost> DeleteBlogAsync(Guid id);
    }
}
