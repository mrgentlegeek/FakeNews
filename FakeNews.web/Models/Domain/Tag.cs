

namespace FakeNews.web.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        //Navigation property which tells entity frame work this is a related property
        public ICollection<BlogPost> BlogPosts { get; set; }

    }
}
