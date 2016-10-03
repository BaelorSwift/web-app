using System.ComponentModel.DataAnnotations;

namespace Baelor.ViewModels.Users
{
	public class CreateUserViewModel
	{
		[Display(Name = "username")]
		[Required]
		[RegularExpression(@"(?i)^[0-9a-zA-Z]+(?:\s[0-9a-zA-Z]+)*$")]
		[StringLength(25, MinimumLength = 1)]
		public string Username { get; set; }

		[Display(Name = "email_address")]
		[Required]
		[EmailAddress]
		public string EmailAddress { get; set; }

		[Display(Name = "name")]
		[Required]
		[RegularExpression(@"^[a-zA-Z ,.'-]+$")]
		public string Name { get; set; }

		[Display(Name = "password")]
		[Required]
		[RegularExpression(@"^(?:(?=.*[a-z])(?:(?=.*[A-Z])(?=.*[\d\W])|(?=.*\W)(?=.*\d))|(?=.*\W)(?=.*[A-Z])(?=.*\d)).{10,}$",
			ErrorMessage = @"Password's must be at least 10 characters long, and contain at least 3 of the following: letters, numbers, capital letters, or special characters.")]
		public string Password { get; set; }
	}
}
