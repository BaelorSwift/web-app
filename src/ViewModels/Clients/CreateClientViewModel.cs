using System.ComponentModel.DataAnnotations;

namespace Baelor.ViewModels.Clients
{
	public class CreateClientViewModel
	{
		[Display(Name = "name")]
		[Required]
		[RegularExpression(@"^[a-zA-Z -]+$")]
		public string Name { get; set; }
	}
}
