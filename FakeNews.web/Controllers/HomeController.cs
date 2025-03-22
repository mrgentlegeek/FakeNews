using FakeNews.web.Models;
using FakeNews.web.Models.ViewModels;
using FakeNews.web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FakeNews.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;

        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index()
        {
            var blogPosts = await blogPostRepository.GetAllBlogsAsync();

            var tags = await tagRepository.GetAllTagsAsync();

            var homeViewModel = new HomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
