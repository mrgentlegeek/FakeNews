using System.ComponentModel.DataAnnotations;

namespace FakeNews.web.Models.ViewModels
{
	public class EditTagRequest
	{
		public Guid Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string DisplayName { get; set; }
	}
}
