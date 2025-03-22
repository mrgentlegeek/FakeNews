using FakeNews.web.Data;
using FakeNews.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace FakeNews.web.Repositories
{
	public class TagRepository : ITagRepository
	{
		private readonly FakeNewsDbContext fakeNewsDbContext;

		public TagRepository(FakeNewsDbContext fakeNewsDbContext)
		{
			this.fakeNewsDbContext = fakeNewsDbContext;
		}

		public async Task<Tag> AddTagAsync(Tag tag)
		{
			await fakeNewsDbContext.Tags.AddAsync(tag);
			await fakeNewsDbContext.SaveChangesAsync();

			return tag;
		}

		public async Task<Tag?> UpdateTagAsync(Tag tag)
		{
			var existingTag = await fakeNewsDbContext.Tags.FindAsync(tag.Id);

			if (existingTag != null)
			{
				existingTag.Name = tag.Name;
				existingTag.DisplayName = tag.DisplayName;

				//Save changes
				fakeNewsDbContext.SaveChanges();
			}

			return existingTag;
		}

		public async Task<Tag?> DeleteTagByIdAsync(Guid id)
		{
			var existingTag = await fakeNewsDbContext.Tags.FindAsync(id);

			if (existingTag != null)
			{
				fakeNewsDbContext.Tags.Remove(existingTag);

				//Save changes
				await fakeNewsDbContext.SaveChangesAsync();
			}

			return existingTag;
		}

		public async Task<IEnumerable<Tag>> GetAllTagsAsync(string? searchQuery, string? sortBy, string? sortDirection, int pageSize = 100, int pageNumber = 1)
		{
			var query = fakeNewsDbContext.Tags.AsQueryable();

			//Searching
			if (string.IsNullOrWhiteSpace(searchQuery) == false)
			{
				query = query.Where(x => x.Name.Contains(searchQuery) || x.DisplayName.Contains(searchQuery));
			}


			//Sorting
			if (string.IsNullOrWhiteSpace(sortBy) == false)
			{
				var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

				if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
				{
					query = isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
				}

				if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
				{
					query = isDesc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);
				}

			}

			//Pagination
			//Skip 0 Take 5 -> Page 1 of 5 Results (User is on page 1, display 1 to 5)
			//Skip 5 Take 5 -> Page 2 of 5 Results (User is on page 2, display 6 to 10)
			var skipResults = (pageNumber - 1) * pageSize;
			query = query.Skip(skipResults).Take(pageSize);


			return await query.ToListAsync();


			//return all data without search, sort or pagination
			//return await fakeNewsDbContext.Tags.ToListAsync();
		}

		public async Task<Tag?> GetTagByIdAsync(Guid id)
		{
			return await fakeNewsDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<int> CountsAsync()
		{
			return await fakeNewsDbContext.Tags.CountAsync();
		}

	}
}
