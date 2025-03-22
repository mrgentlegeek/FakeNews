using FakeNews.web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FakeNews.web.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			//Check if model data is valid
			if (ModelState.IsValid)
			{
				var identityUser = new IdentityUser
				{
					UserName = registerViewModel.Username,
					Email = registerViewModel.Email,
				};

				var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

				if (identityResult.Succeeded)
				{
					//assign this user the "User" role
					var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

					if (roleIdentityResult.Succeeded)
					{
						//Show success notification
						return RedirectToAction("Register");
					}
				}

			}

			//Show error notification
			return View();
		}


		[HttpGet]
		public IActionResult Login(string ReturnUrl)
		{
			var model = new LoginViewModel { ReturnUrl = ReturnUrl };
			return View(model);
		}


		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{

			if (loginViewModel.Username != null && loginViewModel.Password != null)
			{
				var response = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

				if (response != null && response.Succeeded)
				{
					if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl))
					{
						return Redirect(loginViewModel.ReturnUrl);
					}

					return RedirectToAction("Index", "Home");

				}

			}

			var model = new LoginViewModel
			{
				Error = "Incorrect Username or Password"
			};

			//show errors
			return View(model);

		}

		[HttpGet]

		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
