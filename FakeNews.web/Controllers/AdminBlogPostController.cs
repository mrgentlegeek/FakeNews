using FakeNews.web.Models.Domain;
using FakeNews.web.Models.ViewModels;
using FakeNews.web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FakeNews.web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminBlogPostController : Controller
	{
		private readonly ITagRepository tagRepository;
		private readonly IBlogPostRepository blogPostRepository;

		public AdminBlogPostController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
		{
			this.tagRepository = tagRepository;
			this.blogPostRepository = blogPostRepository;
		}


		[HttpGet]
		public async Task<IActionResult> Add()
		{
			//get tags from repository
			var tags = await tagRepository.GetAllTagsAsync();

			var model = new AddBlogPostRequest
			{
				Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
			};

			return View(model);
		}


		[HttpPost]
		public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
		{
			//Map view model to domain modal before passing to DB
			var blogPost = new BlogPost
			{
				Heading = addBlogPostRequest.Heading,
				PageTitle = addBlogPostRequest.PageTitle,
				Content = addBlogPostRequest.Content,
				ShortDescription = addBlogPostRequest.ShortDescription,
				FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
				UrlHandle = addBlogPostRequest.UrlHandle,
				PublishedDate = addBlogPostRequest.PublishedDate,
				Author = addBlogPostRequest.Author,
				Visible = addBlogPostRequest.Visible,
			};

			//Get selected tags from the DB and map to Tags
			var selectedTags = new List<Tag>();
			foreach (var selectedtTagId in addBlogPostRequest.SelectedTags)
			{
				var selectedTagIdAsGuid = Guid.Parse(selectedtTagId);
				var existingTag = await tagRepository.GetTagByIdAsync(selectedTagIdAsGuid);

				if (existingTag != null)
				{
					selectedTags.Add(existingTag);
				}
			}

			blogPost.Tags = selectedTags;

			await blogPostRepository.AddBlogAsync(blogPost);

			return RedirectToAction("List");
		}


		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			//Retrieve the blog from the repository
			var blogPost = await blogPostRepository.GetBlogByIdAsync(id);

			//Retrieve tags from tagRepository

			var tagsDomainModel = await tagRepository.GetAllTagsAsync();

			//map the domain model 'blogPost' into view model 'model'


			if (blogPost != null)
			{
				var model = new EditBlogPostRequest
				{
					Id = blogPost.Id,
					Heading = blogPost.Heading,
					PageTitle = blogPost.PageTitle,
					Content = blogPost.Content,
					Author = blogPost.Author,
					FeaturedImageUrl = blogPost.FeaturedImageUrl,
					UrlHandle = blogPost.UrlHandle,
					ShortDescription = blogPost.ShortDescription,
					PublishedDate = blogPost.PublishedDate,
					Visible = blogPost.Visible,
					Tags = tagsDomainModel.Select(x => new SelectListItem
					{
						Text = x.Name,
						Value = x.Id.ToString()
					}),
					SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
				};

				return View(model);
			}
			else
			{
				return RedirectToAction("List");
			}
		}

		[HttpPost]

		public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
		{

			if (editBlogPostRequest != null)
			{
				var blogPostDomainModel = new BlogPost
				{
					Id = editBlogPostRequest.Id,
					Heading = editBlogPostRequest.Heading,
					PageTitle = editBlogPostRequest.PageTitle,
					Content = editBlogPostRequest.Content,
					Author = editBlogPostRequest.Author,
					FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
					UrlHandle = editBlogPostRequest.UrlHandle,
					ShortDescription = editBlogPostRequest.ShortDescription,
					PublishedDate = editBlogPostRequest.PublishedDate,
					Visible = editBlogPostRequest.Visible,
				};

				//Get selected tags from the DB and map to Tags
				var selectedTags = new List<Tag>();
				foreach (var selectedtTagId in editBlogPostRequest.SelectedTags)
				{
					var selectedTagIdAsGuid = Guid.Parse(selectedtTagId);
					var existingTag = await tagRepository.GetTagByIdAsync(selectedTagIdAsGuid);

					if (existingTag != null)
					{
						selectedTags.Add(existingTag);
					}
				}

				blogPostDomainModel.Tags = selectedTags;

				var existingBlogPost = await blogPostRepository.UpdateBlogAsync(blogPostDomainModel);

				if (existingBlogPost != null)
				{
					//show success notification
				}
				else
				{
					//show error notification
				}

			}


			return RedirectToAction("List");

		}

		[HttpPost]
		public async Task<IActionResult> DeleteFromList(Guid id)
		{
			var existingBlogPost = await blogPostRepository.DeleteBlogAsync(id);

			if (existingBlogPost != null)
			{
				//show success notification
			}
			else
			{
				//show error notification
			}

			return RedirectToAction("List");
		}


		[HttpGet]
		public async Task<IActionResult> List()
		{

			var blogPosts = await blogPostRepository.GetAllBlogsAsync();

			return View(blogPosts);
		}
	}
}
