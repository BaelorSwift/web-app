using System.Threading.Tasks;
using Baelor.Database.Models;
using MongoDB.Bson;

namespace Baelor.Database.Repositories.Interfaces
{
	public interface IUserRepository : IMongoRepository<User>
	{
		Task<User> GetByUsername(string username);

		Task<User> GetByEmailAddress(string emailAddress);

		Task<bool> SetUserVerifiedStatus(ObjectId id, bool verified);
	}
}
