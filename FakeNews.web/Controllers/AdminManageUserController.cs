using FakeNews.web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FakeNews.web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminManageUserController : ControllerBase
	{
		private readonly UserManager<IdentityUser> userManager;

		public AdminManageUserController(UserManager<IdentityUser> userManager)
		{
			this.userManager = userManager;
		}


		[HttpPost]
		[Route("Add")]
		public async Task<IActionResult> Add([FromBody] AddUserRequest addUserRequest)
		{
			var identityUser = new IdentityUser
			{
				UserName = addUserRequest.Username,
				Email = addUserRequest.Email,
			};

			var identityResult = await userManager.CreateAsync(identityUser, addUserRequest.Password);

			if (identityResult.Succeeded)
			{
				//assign this user the "User" role
				var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, addUserRequest.Role);

				if (roleIdentityResult.Succeeded)
				{
					//Show success notification
					return Ok(new { Message = "User created and role assigned successfully!" });
				}
				else
				{
					var roleErrors = string.Join(", ", roleIdentityResult.Errors.Select(e => e.Description));

					return Problem($"User created but failed to assign role: {roleErrors}", null, (int)HttpStatusCode.InternalServerError);
				}
			}
			else
			{
				var creationErrors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
				return Problem($"Failed to create user: {creationErrors}", null, (int)HttpStatusCode.BadRequest);
			}
		}

		[HttpDelete]
		[Route("{id:Guid}/delete")]
		public async Task<IActionResult> Delete([FromRoute] Guid id)
		{
			var user = await userManager.FindByIdAsync(id.ToString());

			if (user != null)
			{
				var identityResult = await userManager.DeleteAsync(user);

				if (identityResult != null && identityResult.Succeeded)
				{
					return Ok(new { Message = "User deleted successfully!" });
				}
				else
				{
					return Problem($"Unable to delete user", null, (int)HttpStatusCode.BadRequest);
				}

			}
			else
			{
				return Problem($"User not found", null, (int)HttpStatusCode.BadRequest);
			}
		}

	}
}
