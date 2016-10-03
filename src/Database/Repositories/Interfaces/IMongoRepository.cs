using System.Collections.Generic;
using System.Threading.Tasks;
using Baelor.Database.Models;
using MongoDB.Bson;

namespace Baelor.Database.Repositories.Interfaces
{
	public interface IMongoRepository<T>
		where T : Audit
	{
		Task<IEnumerable<T>> All();

		Task<T> GetById(ObjectId id);

		Task Add(T item);

		Task<bool> Remove(ObjectId id);
	}
}
