using System.Collections.ObjectModel;
using Baelor.Converters;
using Baelor.Extensions;
using Baelor.ViewModels.Clients;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Baelor.Database.Models
{
	public class Client : Audit
	{
		public Client() { }

		public Client(CreateClientViewModel model)
		{
			Slug = (Name = model.Name).ToSlug();
		}

		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("slug")]
		public string Slug { get; set; }

		[BsonElement("internal")]
		public bool Internal { get; set; }

		[BsonElement("user_id")]
		[JsonConverter(typeof(ObjectIdConverter))]
		public ObjectId UserId { get; set; }

		[BsonElement("api_keys")]
		public Collection<ApiKey> ApiKeys { get; set; } = new Collection<ApiKey>();
	}
}
