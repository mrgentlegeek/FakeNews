namespace FakeNews.web.Models.ViewModels
{
	public class AddUserRequest
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Role { get; set; } = "User";
	}
}
