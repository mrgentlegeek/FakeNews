using FakeNews.web.Validators;
using System.ComponentModel.DataAnnotations;

namespace FakeNews.web.Models.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		[MinLength(4, ErrorMessage = "UserName must be at least 4 characters long.")]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
		[RequireDigit]
		[RequireLowercase]
		[RequireUppercase]
		[RequireNonAlphanumeric]
		[RequireUniqueChars(1)]
		public string Password { get; set; }
	}
}
