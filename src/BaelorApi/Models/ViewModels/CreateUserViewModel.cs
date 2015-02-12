using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using BaelorApi.Models.Database;

namespace BaelorApi.Models.ViewModels
{
	public class CreateUserViewModel
	{
		/// <summary>
		/// The desired Username for the <see cref="User"/>.
		/// </summary>
		[Display(Name = "Username")]
		[Required(ErrorMessage = "required")]
		[MinLength(1, ErrorMessage = "length_short")]
		[MaxLength(25, ErrorMessage = "length_long")]
		[RegularExpression(@"(?![\s])(?!.*[_-]{2})[a-zA-Z0-9-_]+(?<![\s])", ErrorMessage = "malformed")]
		[JsonProperty("username")]
		public string Username { get; set; }

		/// <summary>
		/// The desired Password for the <see cref="User"/>.
		/// </summary>
		[Display(Name = "Password")]
		[Required(ErrorMessage = "required")]
		[MinLength(8, ErrorMessage = "length_short")]
		[JsonProperty("password")]
		public string Password { get; set; }

		/// <summary>
		/// The Password entered again by the <see cref="User"/>, to make sure they entered the correct one.
		/// </summary>
		[Display(Name = "Password Confirmation")]
		[JsonProperty("password_confirm")]
		public string PasswordConfirm { get; set; }

		/// <summary>
		/// The desired Email Address for the <see cref="User"/>.
		/// </summary>
		[Display(Name = "Email Address")]
		[Required(ErrorMessage = "required")]
		[RegularExpression(@"[\w\d\-.+]+@[\w\d\-.+]+\.[a-zA-Z]{2,25}", ErrorMessage = "malformed")]
		[JsonProperty("email_address")]
		public string EmailAddress { get; set; }
	}
}
