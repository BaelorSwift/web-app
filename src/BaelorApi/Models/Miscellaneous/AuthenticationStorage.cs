using BaelorApi.Models.Database;
using System;

namespace BaelorApi.Models.Miscellaneous
{
	public class AuthenticationStorage
	{
		/// <summary>
		/// Created a new <see cref="AuthenticationStorage"/>.
		/// </summary>
		public AuthenticationStorage() { }

		/// <summary>
		/// Created a new <see cref="AuthenticationStorage"/> with a specified <see cref="User"/>.
		/// </summary>
		/// <param name="user">The authenticated <see cref="User"/>.</param>
		public AuthenticationStorage(User user)
		{
			ApiKey = user.ApiKey;
			Username = user.Username;
			EmailAddress = user.EmailAddress;
			IsAdmin = user.IsAdmin;
			IsDemo = user.IsDemo;
			UserId = user.Id;
		}

		/// <summary>
		/// Gets or Sets the Api Key of the authenticated <see cref="User"/>.
		/// </summary>
		public string ApiKey { get; set; }

		/// <summary>
		/// Gets or Sets the Username of the authenticated <see cref="User"/>.
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Gets or Sets the Email Address of authenticated the <see cref="User"/>.
		/// </summary>
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or Sets if the authenticated <see cref="User"/> has admin privileges.
		/// </summary>
		public bool IsAdmin { get; set; }

		/// <summary>
		/// Gets or Sets if the authenticated <see cref="User"/> is a demo account.
		/// </summary>
		public bool IsDemo { get; set; }

		/// <summary>
		/// Gets or Sets the Id of the authenticated <see cref="User"/>.
		/// </summary>
		public Guid UserId { get; set; }
	}
}