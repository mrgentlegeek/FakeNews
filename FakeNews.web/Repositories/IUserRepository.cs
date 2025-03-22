using Microsoft.AspNetCore.Identity;

namespace FakeNews.web.Repositories
{
	public interface IUserRepository
	{
		Task<IEnumerable<IdentityUser>> GetAllUsers();
	}
}
