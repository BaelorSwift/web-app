using Newtonsoft.Json;

namespace BaelorApi.Models.Api.Response.Partials
{
	public class User
	{
		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("email_address")]
		public string EmailAddress { get; set; }

		[JsonProperty("api_key")]
		public string ApiKey { get; set; }

		[JsonProperty("is_admin")]
		public bool IsAdmin { get; set; }

		public static User Create(Database.User user)
		{
			return new User
			{
				ApiKey = user.ApiKey,
				IsAdmin = user.IsAdmin,
				Username = user.Username,
				EmailAddress = user.EmailAddress
			};
		}
	}
}