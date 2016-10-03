using System.ComponentModel.DataAnnotations;

namespace Baelor.ViewModels.EmailVerifications
{
	public class CreateEmailVerificationViewModel
	{
		[Display(Name = "email_address")]
		[Required]
		public string EmailAddress { get; set; }
	}
}
