using FakeNews.web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FakeNews.web.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly AuthDbContext authDbContext;

		public UserRepository(AuthDbContext authDbContext)
		{
			this.authDbContext = authDbContext;
		}

		public async Task<IEnumerable<IdentityUser>> GetAllUsers()
		{
			var users = await authDbContext.Users.Where(x => x.Email != "superadmin@fakenews.com").ToListAsync();

			return users;
		}
	}
}
