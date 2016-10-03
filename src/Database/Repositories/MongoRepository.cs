using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Baelor.Database.Models;
using Baelor.Database.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Baelor.Database.Repositories
{
	public abstract class MongoRepository<T> : IMongoRepository<T>
		where T : Audit
	{
		internal readonly MongoDatabase _mongoDatabase;
		public string CollectionName;

		internal MongoRepository(MongoDatabase mongoDatabase)
		{
			_mongoDatabase = mongoDatabase;
		}

		public async Task Add(T item)
		{
			await _mongoDatabase.Database.GetCollection<T>(CollectionName).InsertOneAsync(item);
		}

		public async Task<IEnumerable<T>> All()
		{
			var result = await _mongoDatabase.Database
				.GetCollection<T>(CollectionName).FindAsync(new BsonDocument());

			return await result.ToListAsync();
		}

		public async Task<T> GetById(ObjectId id)
		{
			var result = await Query(Builders<T>.Filter.Eq(u => u.Id, id));
			return await result.FirstOrDefaultAsync();
		}

		public async Task<IAsyncCursor<T>> Query(FilterDefinition<T> filter)
		{
			return await _mongoDatabase.Database.GetCollection<T>(CollectionName).FindAsync(filter);
		}

		public async Task<bool> Remove(ObjectId id)
		{
			await Task.Delay(1);
			throw new NotImplementedException();
		}
	}
}
