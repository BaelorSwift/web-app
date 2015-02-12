using BaelorApi.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaelorApi.Models.Repositories
{
	public class UserRepository
		: IUserRepository
	{
		/// <summary>
		/// Gets the <see cref="DatabaseContext"/> used by the repository.
		/// </summary>
		private readonly DatabaseContext _db;

		/// <summary>
		/// Creates a new <see cref="UserRepository"/>.
		/// </summary>
		/// <param name="db">An initalized <see cref="DatabaseContext"/> used for database connection management.</param>
		public UserRepository(DatabaseContext db)
		{
			_db = db;
		}

		public IEnumerable<User> GetAll
		{
			get
			{
				return _db.Users.AsEnumerable();
			}
		}

		public User Add(User item)
		{
			_db.Users.Add(item);

			if (_db.SaveChanges() > 0)
				return item;

			return null;
		}

		public User GetById(Guid id)
		{
			return _db.Users.FirstOrDefault(a => a.Id == id);
		}

		public User GetByEmailAddress(string emailAddress)
		{
			return _db.Users.FirstOrDefault(a => a.EmailAddress == emailAddress);
		}

		public User GetByUsername(string username)
		{
			return _db.Users.FirstOrDefault(a => a.Username == username);
		}

		public User GetByApiKey(string apiKey)
		{
			return _db.Users.FirstOrDefault(a => a.ApiKey == apiKey);
		}

		public User Update(Guid id, User item)
		{
			throw new NotImplementedException();
		}

		public bool TryDelete(Guid id)
		{
			var User = _db.Users.FirstOrDefault(c => c.Id == id);
			if (User != null)
			{
				_db.Users.Remove(User);
				return _db.SaveChanges() > 0;
			}
			return false;
		}
	}
}