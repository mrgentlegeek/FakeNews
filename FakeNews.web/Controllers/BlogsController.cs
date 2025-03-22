using FakeNews.web.Models.Domain;
using FakeNews.web.Models.ViewModels;
using FakeNews.web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FakeNews.web.Controllers
{
	public class BlogsController : Controller
	{
		private readonly IBlogPostRepository blogPostRepository;
		private readonly IBlogPostLikeRepository blogPostLikeRepository;
		private readonly IBlogPostCommentRepository blogPostCommentRepository;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly UserManager<IdentityUser> userManager;

		public BlogsController(IBlogPostRepository blogPostRepository,
			IBlogPostLikeRepository blogPostLikeRepository,
			IBlogPostCommentRepository blogPostCommentRepository,
			SignInManager<IdentityUser> signInManager,
			UserManager<IdentityUser> userManager)
		{
			this.blogPostRepository = blogPostRepository;
			this.blogPostLikeRepository = blogPostLikeRepository;
			this.blogPostCommentRepository = blogPostCommentRepository;
			this.signInManager = signInManager;
			this.userManager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string urlHandle)
		{
			var blogPost = await blogPostRepository.GetBlogByURLHandleAsync(urlHandle);
			var hasUserLiked = false;


			if (blogPost != null)
			{
				var likes = await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

				if (signInManager.IsSignedIn(User))
				{
					//Get all likes for this blog and filter the ones for this particular user

					var likesForBlog = await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);

					var userId = userManager.GetUserId(User);

					if (userId != null)
					{
						var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));

						if (likeFromUser != null)
						{
							hasUserLiked = true;
						}
					}

				}

				//Get comments for blog post
				var blogComments = await blogPostCommentRepository.GetAllCommentsByBlogIdAsync(blogPost.Id);
				var blogCommentsForView = new List<BlogComment>();

				if (blogComments != null)
				{

					foreach (var blogComment in blogComments)
					{
						blogCommentsForView.Add(new BlogComment
						{
							Comment = blogComment.Description,
							DateAdded = blogComment.DateAdded,
							Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
						});
					}
				}

				var blogPostViewModel = new BlogPostViewModel
				{
					Id = blogPost.Id,
					Heading = blogPost.Heading,
					PageTitle = blogPost.PageTitle,
					ShortDescription = blogPost.ShortDescription,
					Content = blogPost.Content,
					FeaturedImageUrl = blogPost.FeaturedImageUrl,
					UrlHandle = blogPost.UrlHandle,
					PublishedDate = blogPost.PublishedDate,
					Author = blogPost.Author,
					Visible = blogPost.Visible,
					Tags = blogPost.Tags,
					TotalLikes = likes,
					Liked = hasUserLiked,
					Comments = blogCommentsForView

				};

				return View(blogPostViewModel);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}

		}

		[HttpPost]
		public async Task<IActionResult> Index(BlogPostViewModel blogPostViewModel)
		{
			if (blogPostViewModel != null)
			{
				var userId = userManager.GetUserId(User);

				if (userId != null)
				{
					var comment = new BlogPostComment
					{
						BlogPostId = blogPostViewModel.Id,
						Description = blogPostViewModel.Comment,
						UserId = Guid.Parse(userId),
						DateAdded = DateTime.Now,
					};


					await blogPostCommentRepository.AddAsync(comment);

					return RedirectToAction("Index", "Blogs", new { urlHandle = blogPostViewModel.UrlHandle });

				}
			}

			return RedirectToAction("Index", "Home");
		}
	}
}
