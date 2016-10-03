using System.Collections.Generic;
using System.Threading.Tasks;
using Baelor.Database.Models;
using Baelor.Database.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Baelor.Database.Repositories
{
	public class ClientRepository : MongoRepository<Client>, IClientRepository
	{
		public ClientRepository(MongoDatabase mongoDatabase)
			: base(mongoDatabase)
		{
			CollectionName = "clients";
		}

		public async Task<List<Client>> GetClientsByUser(ObjectId userId)
		{
			var result = await Query(Builders<Client>.Filter.Eq(c => c.UserId, userId));
			return await result.ToListAsync();
		}
	}
}
