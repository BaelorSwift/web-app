using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BaelorApi.Models.ViewModels
{
	public class CreateSessionViewModel
	{
		/// <summary>
		/// The desired Identity for the <see cref="User"/>.
		/// </summary>
		[Display(Name = "Identity")]
		[Required(ErrorMessage = "required")]
		[JsonProperty("identity")]
		public string Identity { get; set; }

		/// <summary>
		/// The desired Password for the <see cref="User"/>.
		/// </summary>
		[Display(Name = "Password")]
		[Required(ErrorMessage = "required")]
		[JsonProperty("password")]
		public string Password { get; set; }
	}
}
