using FakeNews.web.Models.Domain;
using FakeNews.web.Models.ViewModels;
using FakeNews.web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FakeNews.web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogPostLikeController : ControllerBase
	{
		private readonly IBlogPostLikeRepository blogPostLikeRepository;

		public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
		{
			this.blogPostLikeRepository = blogPostLikeRepository;
		}


		[HttpPost]
		[Route("Add")]
		//from body of payload get addLikeRequest of type AddLikeRequest
		public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
		{
			var requestData = new BlogPostLike
			{
				UserId = addLikeRequest.UserId,
				BlogPostId = addLikeRequest.BlogPostId,
			};

			var response = await blogPostLikeRepository.AddBlogPostLike(requestData);

			if (response == null)
			{
				return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);
			}

			return Ok();
		}


		[HttpGet]
		[Route("{blogPostId:Guid}/totalLikes")]
		public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId)
		{
			var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPostId);

			return Ok(totalLikes);
		}

	}
}
