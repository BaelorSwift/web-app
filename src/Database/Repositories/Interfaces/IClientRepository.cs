using System.Collections.Generic;
using System.Threading.Tasks;
using Baelor.Database.Models;
using MongoDB.Bson;

namespace Baelor.Database.Repositories.Interfaces
{
	public interface IClientRepository : IMongoRepository<Client>
	{
		Task<List<Client>> GetClientsByUser(ObjectId userId);
	}
}
