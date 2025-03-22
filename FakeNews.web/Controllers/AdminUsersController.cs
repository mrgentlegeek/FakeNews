using FakeNews.web.Models.ViewModels;
using FakeNews.web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FakeNews.web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminUsersController : Controller
	{
		private readonly IUserRepository userRepository;
		private readonly UserManager<IdentityUser> userManager;

		public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
		{
			this.userRepository = userRepository;
			this.userManager = userManager;
		}

		public async Task<IActionResult> List()
		{
			var users = await userRepository.GetAllUsers();
			var model = new List<UserViewModel>();

			if (users != null && users.Any())
			{
				foreach (var user in users)
				{
					var roles = await userManager.GetRolesAsync(user);

					var data = new UserViewModel
					{
						Id = Guid.Parse(user.Id),
						UserName = user.UserName,
						Email = user.Email,
						Role = string.Join(", ", roles),

					};

					model.Add(data);
				}
			}


			return View(model);
		}
	}
}
