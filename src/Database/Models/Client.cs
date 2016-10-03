using System.Collections.ObjectModel;
using Baelor.Converters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Baelor.Database.Models
{
	public class Client : Audit
	{
		public Client() { }

		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("user_id")]
		[JsonConverter(typeof(ObjectIdConverter))]
		public ObjectId UserId { get; set; }

		[BsonElement("keys")]
		public Collection<ApiKey> ApiKeys { get; set; }
	}
}
