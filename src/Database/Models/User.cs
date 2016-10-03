using Baelor.ViewModels.Users;
using MongoDB.Bson.Serialization.Attributes;

namespace Baelor.Database.Models
{
	public class User : Audit
	{
		public User() { }

		public User(CreateUserViewModel model)
		{
			Name = model.Name;
			Username = model.Username;
			EmailAddress = model.EmailAddress;
			Password = model.Password;
		}

		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("username")]
		public string Username { get; set; }

		[BsonElement("email_address")]
		public string EmailAddress { get; set; }

		[BsonElement("is_verified")]
		public bool IsVerified { get; set; }

		[BsonElement("password")]
		public string Password { get; set; }
	}
}
