namespace BaelorApi.Models.Database
{
	public class User
		: Audit
	{
		public const int ApiKeyLength = 32;

		public string ApiKey { get; set; }

		public string Username { get; set; }

		public string PasswordHash { get; set; }
		
		public string PasswordSalt { get; set; }

		public int PasswordIterations { get; set; }

		public string EmailAddress { get; set; }

		public bool IsAdmin { get; set; }

		public bool IsDemo { get; set; }

		public bool IsRevoked { get; set; }
	}
}
