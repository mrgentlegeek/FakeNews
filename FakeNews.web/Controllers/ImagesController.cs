using FakeNews.web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FakeNews.web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly IImageRepository imageRepository;

		public ImagesController(IImageRepository imageRepository)
		{
			this.imageRepository = imageRepository;
		}


		[HttpPost]
		public async Task<IActionResult> Upload(IFormFile file)
		{
			var response = await imageRepository.Upload(file);

			if (response == null)
			{
				return Problem("Something went wrong!", response, (int)HttpStatusCode.InternalServerError);
			}

			return new JsonResult(new { link = response });
		}
	}
}
