using System.ComponentModel.DataAnnotations;

namespace Baelor.ViewModels.EmailVerifications
{
	public class VerifyUserViewModel
	{
		[Display(Name = "code")]
		[Required]
		public string Code { get; set; }
	}
}
