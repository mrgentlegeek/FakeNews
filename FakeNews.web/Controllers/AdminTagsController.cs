using FakeNews.web.Models.Domain;
using FakeNews.web.Models.ViewModels;
using FakeNews.web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeNews.web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminTagsController : Controller
	{
		private readonly ITagRepository tagRepository;

		public AdminTagsController(ITagRepository tagRepository)
		{
			this.tagRepository = tagRepository;
		}

		//Get the view
		//[Authorize(Roles = "Admin")] NB: this can be added to individual actions or to the entire controller. I have added to the controller to cover all actions
		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}


		[HttpPost]
		[ActionName("Add")]
		public async Task<IActionResult> AddTag(AddTagRequest addTagRequest)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			//Mapping AddTagRequest to Tag domain model
			var tag = new Tag

			{
				Name = addTagRequest.Name,
				DisplayName = addTagRequest.DisplayName,
			};

			await tagRepository.AddTagAsync(tag);


			return RedirectToAction("List");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			//1st method
			//var tag = _fakeNewsDbContext.Tags.Find(id);

			//2nd method
			var tag = await tagRepository.GetTagByIdAsync(id);

			if (tag != null)
			{
				var editTagRequest = new EditTagRequest
				{
					Id = tag.Id,
					Name = tag.Name,
					DisplayName = tag.DisplayName,

				};
				return View(editTagRequest);
			}
			return View(null);
		}


		[HttpPost]
		public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
		{

			if (!ModelState.IsValid)
			{
				return View();
			}

			var tag = new Tag
			{
				Id = editTagRequest.Id,
				Name = editTagRequest.Name,
				DisplayName = editTagRequest.DisplayName,
			};

			var existingTag = await tagRepository.UpdateTagAsync(tag);

			if (existingTag != null)
			{
				return RedirectToAction("List");
			}


			return RedirectToAction("Edit", new { id = editTagRequest.Id });



		}


		[HttpPost]
		public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
		{

			var existingtag = await tagRepository.DeleteTagByIdAsync(editTagRequest.Id);

			if (existingtag != null)
			{
				return RedirectToAction("List");
			}

			return RedirectToAction("Edit", new { id = editTagRequest.Id });

		}


		[HttpPost]
		public async Task<IActionResult> DeleteFromList(Guid Id)
		{

			var existingtag = await tagRepository.DeleteTagByIdAsync(Id);

			return RedirectToAction("List");

		}


		[HttpGet]
		public async Task<IActionResult> List(string? searchQuery, string? sortBy, string? sortDirection, int pageSize = 4, int pageNumber = 1)
		{
			var totalRecords = await tagRepository.CountsAsync();
			var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);
			//temporarily save  input field content so that it is retained when the data is fetched. Check razor page
			ViewBag.SearchQuery = searchQuery;
			ViewBag.SortBy = sortBy;
			ViewBag.SortDirection = sortDirection;
			ViewBag.PageSize = pageSize;
			ViewBag.PageNumber = pageNumber;
			ViewBag.TotalPages = totalPages;


			if (pageNumber > totalPages)
			{
				pageNumber--;
				ViewBag.PageNumber = pageNumber;
			}

			if (pageNumber < 1)
			{
				pageNumber++;
				ViewBag.PageNumber = pageNumber;
			}

			//use dbContext to read the tags
			var tags = await tagRepository.GetAllTagsAsync(searchQuery, sortBy, sortDirection, pageSize, pageNumber);

			//var totalRecords = tags.Count();

			return View(tags);
		}
	}
}
