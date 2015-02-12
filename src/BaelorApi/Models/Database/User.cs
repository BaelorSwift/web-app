namespace BaelorApi.Models.Database
{
	public class User
		: Audit
	{
		public string ApiKey { get; set; }

		public string Username { get; set; }

		public string PasswordHash { get; set; }
		
		public string PasswordSalt { get; set; }

		public int PasswordIterations { get; set; }

		public string Email { get; set; }
	}
}
