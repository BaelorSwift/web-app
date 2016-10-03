using Baelor.Models.Internal;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Baelor.Database
{
	public class MongoDatabase
	{
		public MongoDatabase(IOptions<MongoOptions> options)
		{
			ClientSettings = MongoClientSettings.FromUrl(new MongoUrl(options.Value.ConnectionString));
			Client = new MongoClient(ClientSettings);
			Database = Client.GetDatabase(options.Value.Database);
		}

		private MongoClientSettings ClientSettings { get; set; }

		public IMongoClient Client { get; private set; }

		public IMongoDatabase Database { get; private set; }
	}
}
