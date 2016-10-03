using Baelor.Crypto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Baelor.Database.Models
{
	public class ApiKey : Audit
	{
		public ApiKey() { }

		public ApiKey(string name)
		{
			Name = name;
			Key = RandomHelpers.GetUniqueKey(64);
		}

		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("key")]
		public string Key { get; set; }
	}
}
