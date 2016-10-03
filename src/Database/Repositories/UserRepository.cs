using System.Threading.Tasks;
using Baelor.Database.Models;
using Baelor.Database.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Baelor.Database.Repositories
{
	public class UserRepository : MongoRepository<User>, IUserRepository
	{
		public UserRepository(MongoDatabase mongoDatabase)
			: base(mongoDatabase)
		{
			CollectionName = "users";
		}

		public async Task<User> GetByEmailAddress(string emailAddress)
		{
			var result = await Query(Builders<User>.Filter.Eq(u => u.EmailAddress, emailAddress));
			return await result.FirstOrDefaultAsync();
		}

		public async Task<User> GetByUsername(string username)
		{
			var result = await Query(Builders<User>.Filter.Eq(u => u.Username, username));
			return await result.FirstOrDefaultAsync();
		}

		public async Task<bool> SetUserVerifiedStatus(ObjectId id, bool verified)
		{
			var filter = Builders<User>.Filter.Eq(u => u.Id, id);
			var update = Builders<User>.Update
				.Set("is_verified", verified)
				.CurrentDate("updated_at");

			var result = await _mongoDatabase.Database.GetCollection<User>(CollectionName)
				.UpdateOneAsync(filter, update);

			return result.IsAcknowledged;
		}
	}
}
